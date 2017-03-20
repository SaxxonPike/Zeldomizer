using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Breadbox.Test.Mos6567
{
    [TestFixture]
    [Parallelizable(ParallelScope.Children)]
    public class Mos6567BaseTestFixture
    {
        protected Mos6567Configuration Config { get; private set; }
        protected Mos6567Chip Vic { get; private set; }

        [SetUp]
        public void Initialize()
        {
            SetUpMocks();

            Config = new Mos6567Configuration(65, 263);
            Vic = new Mos6567Chip(Config);
        }

        protected virtual void SetUpMocks()
        {
        }
    }
}
