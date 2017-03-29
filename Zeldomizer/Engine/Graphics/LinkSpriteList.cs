using System.Collections.Generic;
using Zeldomizer.Engine.Graphics.Interfaces;
using Zeldomizer.Metal;

namespace Zeldomizer.Engine.Graphics
{
    public class LinkSpriteList : SpriteListBase
    {
        public LinkSpriteList(IRom rom) : base(rom, 0x807F)
        {
        }

        protected override IEnumerable<ISprite> GetSprites()
        {
            yield return WalkRight1;
            yield return WalkRight2;
            yield return WalkDown;
            yield return WalkUp;
            yield return AttackRight;
            yield return AttackDown;
            yield return AttackUp;
            yield return SmallShieldDown1;
            yield return SmallShieldDown2;
            yield return LargeShieldRight;
            yield return LargeShieldDown;
        }

        public ISprite WalkRight1 => GetSprite(0x00, 2, 2);
        public ISprite WalkRight2 => GetSprite(0x04, 2, 2);
        public ISprite WalkDown => GetSprite(0x08, 2, 2);
        public ISprite WalkUp => GetSprite(0x0C, 2, 2);
        public ISprite AttackRight => GetSprite(0x10, 2, 2);
        public ISprite AttackDown => GetSprite(0x14, 2, 2);
        public ISprite AttackUp => GetSprite(0x18, 2, 2);
        public ISprite LargeShieldRight => GetSprite(0x54, 1, 2);
        public ISprite SmallShieldDown1 => GetSprite(0x58, 1, 2);
        public ISprite SmallShieldDown2 => GetSprite(0x58, 1, 2);
        public ISprite LargeShieldDown => GetSprite(0x60, 1, 2);
    }
}
