namespace Zeldomizer.Metal.Cpu
{
    public class LocatedInstruction : Instruction
    {
        public int Address { get; set; }

        public override string Mnemonic
        {
            get
            {
                switch (AddressingMode)
                {
                    case AddressingMode.Relative:
                        return $"{OpcodeString} ${Address + (LowOperand >= 0x80 ? LowOperand - 0xFE : LowOperand + 2):X4}";
                }
                return base.Mnemonic;
            }
        }

        public override string ToString()
        {
            return $"${Address:X4}: {base.ToString()}";
        }
    }
}
