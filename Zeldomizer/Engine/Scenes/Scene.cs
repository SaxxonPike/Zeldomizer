using System;
using Zeldomizer.Engine.Scenes.Interfaces;
using Zeldomizer.Metal;

namespace Zeldomizer.Engine.Scenes
{
    public class Scene : FixedList<SceneRow>, IScene
    {
        private readonly ISource _source;
        private readonly ITextConversionTable _textConversionTable;

        public Scene(ISource source, ITextConversionTable textConversionTable) : base(30)
        {
            _source = source;
            _textConversionTable = textConversionTable;
        }

        public override SceneRow this[int index]
        {
            get => new SceneRow(new SourceBlock(_source, 3 + index * 0x23), _textConversionTable);
            set => throw new Exception("Can't set scene rows this way (yet...)");
        }
    }
}
