using System.Collections.Generic;
using Zeldomizer.Metal;

namespace Zeldomizer.Engine.Graphics
{
    public class ItemSpriteList : SpriteListBase
    {
        public ItemSpriteList(IRom rom) : base(rom, 0x807F)
        {
        }

        protected override IEnumerable<ISprite> GetSprites()
        {
            yield return Cursor;
            yield return Sword;
            yield return Food;
            yield return Recorder;
            yield return Candle;
            yield return Arrow;
            yield return Bow;
            yield return MagicKey;
            yield return Key;
            yield return Rupy;
            yield return Bomb;
            yield return Boomerang1;
            yield return Boomerang2;
            yield return Boomerang3;
            yield return Damage;
            yield return MapDot;
            yield return Potion;
            yield return Book;
            yield return Fireball;
            yield return Ring;
            yield return MagicSword;
            yield return Rod;
            yield return Map;
            yield return Bracelet;
            yield return Fairy1;
            yield return Fairy2;
            yield return Shield;
            yield return Fire;
            yield return Sparkle1;
            yield return Sparkle2;
            yield return Stopwatch;
            yield return Heart;
            yield return Compass;
            yield return Raft;
            yield return Triforce;
        }

        public ISprite Cursor => GetSprite(0x1E, 1, 2);
        public ISprite Sword => GetSprite(0x20, 1, 2);
        public ISprite Food => GetSprite(0x22, 1, 2);
        public ISprite Recorder => GetSprite(0x24, 1, 2);
        public ISprite Candle => GetSprite(0x26, 1, 2);
        public ISprite Arrow => GetSprite(0x28, 1, 2);
        public ISprite Bow => GetSprite(0x2A, 1, 2);
        public ISprite MagicKey => GetSprite(0x2C, 1, 2);
        public ISprite Key => GetSprite(0x2E, 1, 2);
        public ISprite Rupy => GetSprite(0x32, 1, 2);
        public ISprite Bomb => GetSprite(0x34, 1, 2);
        public ISprite Boomerang1 => GetSprite(0x36, 1, 2);
        public ISprite Boomerang2 => GetSprite(0x38, 1, 2);
        public ISprite Boomerang3 => GetSprite(0x3A, 1, 2);
        public ISprite Damage => GetSprite(0x3C, 1, 2);
        public ISprite MapDot => GetSprite(0x3E, 1, 2);
        public ISprite Potion => GetSprite(0x40, 1, 2);
        public ISprite Book => GetSprite(0x42, 1, 2);
        public ISprite Fireball => GetSprite(0x44, 1, 2);
        public ISprite Ring => GetSprite(0x46, 1, 2);
        public ISprite MagicSword => GetSprite(0x48, 1, 2);
        public ISprite Rod => GetSprite(0x4A, 1, 2);
        public ISprite Map => GetSprite(0x4C, 1, 2);
        public ISprite Bracelet => GetSprite(0x4E, 1, 2);
        public ISprite Fairy1 => GetSprite(0x50, 1, 2);
        public ISprite Fairy2 => GetSprite(0x52, 1, 2);
        public ISprite Shield => GetSprite(0x56, 1, 2);
        public ISprite Fire => GetSprite(0x5C, 2, 2);
        public ISprite Sparkle1 => GetSprite(0x62, 1, 2);
        public ISprite Sparkle2 => GetSprite(0x64, 1, 2);
        public ISprite Stopwatch => GetSprite(0x66, 1, 2);
        public ISprite Heart => GetSprite(0x68, 1, 2);
        public ISprite Compass => GetSprite(0x6A, 1, 2);
        public ISprite Raft => GetSprite(0x6C, 1, 2);
        public ISprite Triforce => GetSprite(0x6E, 1, 2);
    }
}
