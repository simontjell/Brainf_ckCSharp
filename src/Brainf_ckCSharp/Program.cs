using System;

namespace BF1
{
    // https://en.wikipedia.org/wiki/Brainfuck
    class Program
    {
        static void Main(string[] args)
        {
            //var program = "++++++++++[>+++++++>++++++++++>+++>+<<<<-]>++.>+.+++++++..+++.>++.<<+++++++++++++++.>.+++.------.--------.>+.>.";
            var program = @"
>++++++++++>+>+[
    [+++++[>++++++++<-]>.<++++++[>--------<-]+<<<]>.>>[
        [-]<[>+<-]>>[<<+>+>-]<[>+<-[>+<-[>+<-[>+<-[>+<-[>+<-
            [>+<-[>+<-[>+<-[>[-]>+>+<<<-[>+<-]]]]]]]]]]]+>>>
    ]<<<
]
            ";

            new Interpreter(
                Console.OpenStandardInput(), 
                Console.OpenStandardOutput()
            )
            .Interpret(
                new Parser()
                .Parse(program)
            );
        }
    }
}
