using System.Collections.Generic;
using System.Linq;

namespace Zeldomizer.Metal.Cpu
{
    public class StaticAnalyzer
    {
        private readonly IDisassembler _disassembler;

        public StaticAnalyzer(IDisassembler disassembler)
        {
            _disassembler = disassembler;
        }

        public CodeAnalysis Analyze(ICodeBlock codeBlock)
        {
            var instructions = new Dictionary<int, LocatedInstruction>();
            var traces = new Queue<int>();

            traces.Enqueue(codeBlock.AnalysisHintAddresses.Any()
                ? codeBlock.AnalysisHintAddresses.First()
                : codeBlock.Origin);

            while (traces.Count > 0)
            {
                var address = traces.Dequeue();
                while (!instructions.ContainsKey(address))
                {
                    var instruction = _disassembler.Disassemble(codeBlock, address);
                    instructions[address] = instruction;

                    // Branches split the path in two.
                    if (instruction.AddressingMode == AddressingMode.Relative)
                    {
                        // Create another trace at the branch location.
                        var branchTarget = address + instruction.Length +
                                           (instruction.Operand >= 0x80
                                               ? (instruction.Operand - 0x100)
                                               : instruction.Operand);
                        if (branchTarget != address)
                            traces.Enqueue(branchTarget);
                    }

                    // Jumps move the address pointer.
                    if (instruction.Opcode == Opcode.Jmp)
                    {
                        if (instruction.AddressingMode == AddressingMode.Indirect)
                        {
                            // If it's indirect, see if the memory location is within the code block.
                            var vectorAddress = instruction.Operand;
                            var romOffset = codeBlock.ConvertMappedAddressToRomOffset(vectorAddress);
                            if (romOffset < codeBlock.Offset || romOffset >= codeBlock.Offset + codeBlock.Length)
                            {
                                // The address is outside the range of the code block, fail.
                                break;
                            }

                            // Get the target address and go there.
                            address = codeBlock.Rom[romOffset] | (codeBlock.Rom[romOffset + 1] << 8);
                            continue;
                        }

                        // Go directly to absolute addresses.
                        address = instruction.Operand;
                        continue;
                    }

                    if (instruction.Opcode == Opcode.Rts || instruction.Opcode == Opcode.Rti)
                    {
                        // Can't do much about returns because we don't have a stack.
                        break;
                    }

                    if (instruction.Opcode == Opcode.Jsr)
                    {
                        // Create a new trace for the subroutine.
                        traces.Enqueue(instruction.Operand);
                    }

                    // Move the pointer forward for the next instruction.
                    address += instruction.Length;
                }
            }

            return new CodeAnalysis
            {
                Instructions = instructions
                    .Select(kv => kv.Value)
                    .OrderBy(i => i.Address)
                    .ToDictionary(i => i.Address, i => i)
            };
        }
    }
}
