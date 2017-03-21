using System.Collections.Generic;
using System.Linq;
using Disaster.Assembly;
using Disaster.Assembly.Interfaces;

namespace Disaster.Analysis
{
    public class StaticAnalyzer
    {
        private readonly IDisassembler _disassembler;

        public StaticAnalyzer(IDisassembler disassembler)
        {
            _disassembler = disassembler;
        }

        public CodeAnalysis Analyze(IEnumerable<ICodeBlock> codeBlocks, IEnumerable<int> addresses)
        {
            var blocks = codeBlocks.ToArray();
            var instructions = new Dictionary<int, LocatedInstruction>();
            var traces = new Queue<int>(addresses);

            ICodeBlock GetContainingCodeBlock(int address)
            {
                return blocks.FirstOrDefault(block =>
                {
                    var offset = block.ConvertMappedAddressToRomOffset(address);
                    return offset >= block.Offset && offset < block.Offset + block.Length;
                });
            }

            int? ReadVector(int address)
            {
                var lowByteBlock = GetContainingCodeBlock(address & 0xFFFF);
                var highByteBlock = GetContainingCodeBlock((address & 0xFF00) | ((address + 1) & 0x00FF));
                if (lowByteBlock != null && highByteBlock != null)
                {
                    return lowByteBlock.Rom[lowByteBlock.ConvertMappedAddressToRomOffset(address)] |
                           (highByteBlock.Rom[highByteBlock.ConvertMappedAddressToRomOffset(address + 1)] << 8);
                }
                return null;
            }

            while (traces.Count > 0)
            {
                var address = traces.Dequeue();
                while (!instructions.ContainsKey(address))
                {
                    // Find the code block containing the current address.
                    var codeBlock = GetContainingCodeBlock(address);
                    if (codeBlock == null)
                        break;

                    // Read the current instruction.
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
                        traces.Enqueue(branchTarget);
                    }

                    // Jumps move the address pointer.
                    if (instruction.Opcode == Opcode.Jmp)
                    {
                        if (instruction.AddressingMode == AddressingMode.Indirect)
                        {
                            // If it's indirect, try reading the vector.
                            var vector = ReadVector(instruction.Operand);

                            // If we can't read the vector, end.
                            if (vector == null)
                                break;

                            // Get the target address and go there.
                            address = vector.Value;
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
