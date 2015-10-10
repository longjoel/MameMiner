using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using MameMiner.Model;
using MameMiner.Model.Model;


namespace MameMiner.Model.Tests
{
    [TestClass]
    public class MameGameRomDetails_Constructor
    {
        [TestMethod]
        public void BasicTest()
        {
            MameGameRomDetails details = null;

            try
            {
                details = new MameGameRomDetails();
            }
            catch { }

            Assert.IsNotNull(details);
        }
    }
}
