using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AlgorithmicTrading.Qai.UnitTest
{
    [TestClass]
    public class QaiInstrumentManagerUnitTest
    {
        [TestMethod]
        public void TestInstrumentsProperty()
        {
            var instrumentManager = new QaiInstrumentManager {InstrumentKeys = new[] {"72990", "39988", "46244"}};
            instrumentManager.Initialize();
            Assert.AreEqual(3, instrumentManager.Instruments.Length);
        }
    }
}