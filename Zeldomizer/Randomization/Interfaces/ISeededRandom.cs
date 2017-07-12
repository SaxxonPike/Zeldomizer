namespace Zeldomizer.Randomization.Interfaces
{
    public interface ISeededRandom
    {
        int GetInt(int max);
        int GetInt(int min, int max);
        bool GetBool();
        void Reset();
        int Seed { get; }
    }
}