using DragonC.Domain.Compilator;

namespace DragonC.CLI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Compilator.Compilator compilator = new Compilator.Compilator();

            CompiledCode code = compilator.Compile("const test 3;\r\n\r\nlabel main:\r\n\ttest;\ncomm1;comm2;\r\njmp main;");
        }
    }
}