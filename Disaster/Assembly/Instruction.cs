using System;

namespace Disaster.Assembly
{
    public class Instruction
    {
        public Opcode Opcode { get; set; }

        private int _operand;

        public int Operand
        {
            get { return Length > 1 ? _operand : 0; }
            set { _operand = value; }
        }

        public AddressingMode AddressingMode { get; set; }

        protected string OpcodeString => Opcode.ToString().ToUpperInvariant();
        protected int LowOperand => Operand & 0xFF;
        protected int FullOperand => Operand & 0xFFFF;

        public int Length
        {
            get
            {
                switch (AddressingMode)
                {
                    case AddressingMode.Accumulator:
                    case AddressingMode.Implied:
                        return 1;
                    case AddressingMode.Immediate:
                    case AddressingMode.IndirectZeroPageX:
                    case AddressingMode.IndirectZeroPageY:
                    case AddressingMode.Relative:
                    case AddressingMode.ZeroPage:
                    case AddressingMode.ZeroPageX:
                    case AddressingMode.ZeroPageY:
                        return 2;
                    case AddressingMode.Absolute:
                    case AddressingMode.AbsoluteX:
                    case AddressingMode.AbsoluteY:
                    case AddressingMode.Indirect:
                        return 3;
                    default:
                        throw new Exception("Invalid addressing mode.");
                }
            }
        }

        public virtual string Mnemonic
        {
            get
            {
                switch (AddressingMode)
                {
                    case AddressingMode.Absolute:
                        return $"{OpcodeString} ${FullOperand:X4}";
                    case AddressingMode.AbsoluteX:
                        return $"{OpcodeString} ${FullOperand:X4},X";
                    case AddressingMode.AbsoluteY:
                        return $"{OpcodeString} ${FullOperand:X4},Y";
                    case AddressingMode.Accumulator:
                        return $"{OpcodeString} A";
                    case AddressingMode.Immediate:
                        return $"{OpcodeString} #${LowOperand:X2}";
                    case AddressingMode.Implied:
                        return $"{OpcodeString}";
                    case AddressingMode.Indirect:
                        return $"{OpcodeString} (${FullOperand:X4})";
                    case AddressingMode.IndirectZeroPageX:
                        return $"{OpcodeString} (${LowOperand:X2},X)";
                    case AddressingMode.IndirectZeroPageY:
                        return $"{OpcodeString} (${LowOperand:X2}),Y";
                    case AddressingMode.Relative:
                        return $"{OpcodeString} {(LowOperand >= 0x80 ? $"-{0x102 - LowOperand}" : $"+{LowOperand + 2}")}";
                    case AddressingMode.ZeroPage:
                        return $"{OpcodeString} ${LowOperand:X2}";
                    case AddressingMode.ZeroPageX:
                        return $"{OpcodeString} ${LowOperand:X2},X";
                    case AddressingMode.ZeroPageY:
                        return $"{OpcodeString} ${LowOperand:X2},Y";
                }
                throw new Exception("Invalid addressing mode.");
            }
        }

        public override string ToString()
        {
            return Mnemonic;
        }
    }
}
