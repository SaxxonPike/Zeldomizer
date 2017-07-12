namespace Zeldomizer.UI.Models
{
    public class ListItemModel
    {
        public object Tag { get; set; }
        public string Text { get; set; } = string.Empty;
        public override string ToString() => Text;
    }
}
