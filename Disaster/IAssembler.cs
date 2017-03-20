namespace Disaster
{
    public interface IAssembler
    {
        void Assemble(Instruction instruction, ICodeBlock codeBlock, int address);
    }
}