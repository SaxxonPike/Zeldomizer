namespace Zeldomizer.Metal
{
    public interface IExportable : IRawExportable
    {
        byte[] Export();
    }
}
