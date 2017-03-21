namespace Disaster.Assembly.Interfaces
{
    public interface IDisassembler
    {
        LocatedInstruction Disassemble(ICodeBlock codeBlock, int address);
    }
}