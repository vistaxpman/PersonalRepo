namespace DbgViewDemo
{
	public class DbgViewDemo
	{
		public static void Main()
		{
			for (int i = 0; i < 100; i ++)
			{
				System.Diagnostics.Debug.WriteLine(string.Format("{0:D2}: I am a DBEUG output.", i));
			}
			System.Console.ReadKey();
		}
	}
}