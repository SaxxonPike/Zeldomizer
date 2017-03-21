using Disaster.Assembly.Interfaces;

namespace Disaster.Assembly
{
    public class Assembler : IAssembler
    {
        public void Assemble(Instruction instruction, ICodeBlock codeBlock, int address)
        {
            var romOffset = codeBlock.ConvertMappedAddressToRomOffset(address);
            var data = OpcodeTables.Encode(instruction.AddressingMode, instruction.Opcode);

            codeBlock.Rom[romOffset] = unchecked((byte) data);

            var operand = instruction.Operand;
            for (var i = 1; i < instruction.Length; i++)
            {
                codeBlock.Rom[romOffset + i] = unchecked((byte) operand);
                operand >>= 8;
            }
        }
    }
}
