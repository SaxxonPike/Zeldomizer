namespace Breadbox
{
    public interface ISystem
    {
        int Read(int address);
        void Write(int address, int value);
        bool Ready { get; }
        bool Nmi { get; }
        bool Irq { get; }
    }
}
