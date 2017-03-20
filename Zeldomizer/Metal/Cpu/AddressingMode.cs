namespace Zeldomizer.Metal.Cpu
{
    public enum AddressingMode
    {
        Invalid,
        IndirectZeroPageX,
        ZeroPage,
        Immediate,
        Absolute,
        IndirectZeroPageY,
        ZeroPageX,
        AbsoluteY,
        AbsoluteX,
        Accumulator,
        ZeroPageY,
        Indirect,
        Implied,
        Relative
    }
}
