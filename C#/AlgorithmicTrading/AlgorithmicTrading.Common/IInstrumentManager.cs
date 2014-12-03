namespace AlgorithmicTrading.Common
{
    public interface IInstrumentManager
    {
        string[] InstrumentKeys { get; set; }

        Instrument[] Instruments { get; }

        void Initialize();
    }

    public abstract class InstrumentManager : IInstrumentManager
    {
        public string[] InstrumentKeys { get; set; }

        public Instrument[] Instruments { get; protected set; }

        public abstract void Initialize();
    }
}