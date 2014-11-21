using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System;

class Program
{
    [Import]
    public string Text { get; set; }

    public Program()
    {
        var assemblyCatalog = new AssemblyCatalog(typeof(Program).Assembly);
        var container = new CompositionContainer(assemblyCatalog);
        container.SatisfyImportsOnce(this);
    }


    static void Main(string[] args)
    {
        var p = new Program();
        Console.WriteLine(p.Text);
        Console.ReadKey();
    }
}

public class Exports
{
    [Export]
    public string HelloMef
    {
        get { return "Hello, MEF!"; }
    }
}

