namespace Zeldomizer.Metal.Cpu
{
    public interface IDisassembler
    {
        LocatedInstruction Disassemble(ICodeBlock codeBlock, int address);
    }
}