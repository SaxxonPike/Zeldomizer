using Disaster.Assembly.Interfaces;

namespace Disaster.Assembly
{
    public class Disassembler : IDisassembler
    {
        private static int GetOperand(ICodeBlock codeBlock, int offset)
        {
            return codeBlock.Rom[offset + 1];
        }

        private static int GetFullOperand(ICodeBlock codeBlock, int offset)
        {
            return GetOperand(codeBlock, offset) | (codeBlock.Rom[offset + 2] << 8);
        }

        public LocatedInstruction Disassemble(ICodeBlock codeBlock, int address)
        {
            var romOffset = codeBlock.ConvertMappedAddressToRomOffset(address);
            var input = codeBlock.Rom[romOffset];
            var decoded = OpcodeTables.Decode(input);
            var result = new LocatedInstruction
            {
                AddressingMode = decoded.AddressingMode,
                Opcode = decoded.Opcode,
                Address = address
            };

            switch (result.Length)
            {
                case 2:
                    result.Operand = GetOperand(codeBlock, romOffset);
                    break;
                case 3:
                    result.Operand = GetFullOperand(codeBlock, romOffset);
                    break;
            }
            return result;
        }
    }
}
