using AlgorithmicTrading.Common;
using AlgorithmicTrading.Qai.Repositories;
using System.Collections.Generic;

namespace AlgorithmicTrading.Qai
{
    public class QaiInstrumentManager : InstrumentManager
    {
        public override void Initialize()
        {
            Instruments = new QaiRepository().GetInstruments(InstrumentKeys);
        }
    }
}