namespace StreamInsightConsoleApplication
{
    public class Payload
    {
        public Payload()
        {
        }

        public Payload(double x, double y)
        {
            X = x;
            Y = y;
        }

        public double X { get; set; }

        public double Y { get; set; }

        public override string ToString()
        {
            return string.Format("{0},{1}", X, Y);
        }
    }
}