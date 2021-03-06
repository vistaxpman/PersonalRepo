﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AlgorithmicTrading.Qai.UnitTest
{
    [TestClass]
    public class QaiPriceEventSourceUnitTest
    {
        [TestMethod]
        public void TestEventsProperty()
        {
            var eventSource = new QaiStockSplitEventSource
            {
                FromTime = DateTime.Parse("2014-06-01"),
                ToTime = DateTime.Parse("2014-07-01"),
                InstrumentKeys = new[] { "72990", "39988", "46244" }
            };
            eventSource.Initialize();
            Assert.IsTrue(eventSource.Events.Length > 0);
        }
    }
}