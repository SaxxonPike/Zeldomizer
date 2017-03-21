namespace Disaster.Assembly.Interfaces
{
    public interface IAssembler
    {
        void Assemble(Instruction instruction, ICodeBlock codeBlock, int address);
    }
}