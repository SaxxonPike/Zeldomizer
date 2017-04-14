namespace Zeldomizer.Engine.Text
{
    public interface IMenuText
    {
        string EliminationModeText { get; set; }
        int EliminationModeTextLength { get; }
        string RegisterText { get; set; }
        int RegisterTextLength { get; }
        string RegisterYourNameText { get; set; }
        int RegisterYourNameTextLength { get; }
        string SpecialNameText { get; set; }
        int SpecialNameTextLength { get; }
    }
}