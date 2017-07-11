namespace Zeldomizer.Engine.Underworld.Interfaces
{
    public interface IUnderworldRoom
    {
        bool Dark { get; set; }
        UnderworldExitType ExitEast { get; set; }
        UnderworldExitType ExitNorth { get; set; }
        UnderworldExitType ExitSouth { get; set; }
        UnderworldExitType ExitWest { get; set; }
        int FloorItem { get; set; }
        ItemKind FloorItemKind { get; set; }
        bool HasPushableBlock { get; set; }
        int ItemDropPosition { get; set; }
        int Layout { get; set; }
        int MonsterArrangement { get; set; }
        int Monsters { get; set; }
        int RoarType { get; set; }
        UnderworldRoomScript Script { get; set; }
    }
}