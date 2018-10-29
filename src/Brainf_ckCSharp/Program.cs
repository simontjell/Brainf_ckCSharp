using System;

namespace Brainf_ckCSharp
{
    // https://en.wikipedia.org/wiki/Brainfuck
    class Program
    {
        static void Main(string[] args)
        {
            new Interpreter(
                Console.OpenStandardInput(), 
                Console.OpenStandardOutput()
            )
            .Interpret(
                new Parser()
                .Parse(TestPrograms.HelloWorld)
            );
        }
    }
}
