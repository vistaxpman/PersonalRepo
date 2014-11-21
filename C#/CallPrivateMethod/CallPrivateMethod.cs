using System.Reflection;
using System.IO;

class CallPrivateMethod
{
	static void Main(string[] args)
	{
		var assembly = Assembly.LoadFile(
			Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "TestAssembly.dll"));
		var type = assembly.GetType("Test");
		var methodInfo = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Static);
		methodInfo[0].Invoke(null, null);
	}
}
