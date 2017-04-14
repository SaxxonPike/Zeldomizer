namespace Zeldomizer.Engine.Text.Interfaces
{
    public interface IEndingText
    {
        string BottomText1 { get; set; }
        int BottomText1Length { get; }
        string BottomText2 { get; set; }
        int BottomText2Length { get; }
        string BottomText3 { get; set; }
        int BottomText3Length { get; }
        string TopText { get; set; }
        int TopTextLength { get; }
    }
}