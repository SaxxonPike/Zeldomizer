using System.Collections.Generic;

namespace Zeldomizer.Engine.Graphics.Interfaces
{
    public interface ISprite : IEnumerable<int>
    {
        int this[int index] { get; set; }
        int Width { get; }
        int Height { get; }
    }
}