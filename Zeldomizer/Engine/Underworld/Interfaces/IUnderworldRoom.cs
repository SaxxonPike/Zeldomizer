namespace Zeldomizer.Engine.Underworld.Interfaces
{
    public interface IUnderworldRoom
    {
        int Color0 { get; set; }
        int Color1 { get; set; }
        int FloorItem { get; set; }
        int Layout { get; set; }
        bool LayoutFlag { get; set; }
        int Monsters { get; set; }
        int SpecialItem { get; set; }
    }
}