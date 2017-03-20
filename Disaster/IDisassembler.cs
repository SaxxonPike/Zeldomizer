namespace Disaster
{
    public interface IDisassembler
    {
        LocatedInstruction Disassemble(ICodeBlock codeBlock, int address);
    }
}