using Brainf_ckCSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Experiments
{
    class Program
    {
        private static Parser _parser;
        private static Random _random;

        static void Main(string[] args)
        {
            _random = new Random((int)DateTime.Now.Ticks);

            //var population = new Dictionary<IList<Parser.Commands>, int>();
            var test = Evaluate(new Parser().Parse(TestPrograms.HelloWorld));



            int populationSize = 100;
            int maxProgramSize = 200;
            var population =
                Enumerable.Range(0, populationSize)
                .Select(_ => MakeRandomProgram(maxProgramSize))
                .Select(Evaluate)
                .OrderBy(e => e.Fitness)
                .ToList()
            ;


            while(true)
            {
                population = 
                    population
                    .Select(p1 => new[] { p1, population[_random.Next(population.Count)], Evaluate(MakeRandomProgram(maxProgramSize)) })
                    .SelectMany(parents => parents.Union(Combine(parents).Select(Evaluate)))
                    .OrderBy(e => e.Fitness)
                    .Take(populationSize)
                    .ToList()
                ;

                Console.WriteLine(population.Min(i => i.Fitness));
            }

            Console.WriteLine(LevenshteinDistance.Compute("Hello World!", "Hello, World! :-)"));
        }

        private static IEnumerable<IList<Parser.Commands>> Combine(Evaluated<IList<Parser.Commands>>[] parents)
        {
            foreach(var combination in parents.SelectMany((p1, i1) => parents.Where((p2, i2) => i2 != i1).Select(p2 => new { p1, p2 })))
            {
                // TODO: Make more sophisticated combinations (+ mutations)
                yield return combination.p1.Individual.Take(combination.p1.Individual.Count / 2).Union(combination.p2.Individual.Skip(combination.p2.Individual.Count / 2)).ToList();
            }
        }

        private static List<Parser.Commands> MakeRandomProgram(int maxProgramSize)
            => Enumerable.Range(0, maxProgramSize)
                .Select(__ => GetRandomCommand())
                .ToList();

        private static IList<Parser.Commands> _commands = Enum.GetValues(typeof(Parser.Commands)).Cast<Parser.Commands>().Except(new[] { Parser.Commands.ReadDataValue }).ToList();

        private static Parser.Commands GetRandomCommand()
            => _commands[_random.Next(_commands.Count)];

        private static Evaluated<IList<Parser.Commands>> Evaluate(IList<Parser.Commands> program)
        {
            using (var outputStream = new MemoryStream())
            {
                var interpreter = new Interpreter(new MemoryStream(), outputStream);
                try
                {
                    interpreter.Interpret(program, maxRunTime: TimeSpan.FromMilliseconds(1));
                }
                catch (InstructionPointOutOfRangeException)
                {
                    return Evaluated<IList<Parser.Commands>>.Create(program, double.MaxValue);
                }
                var output = Encoding.UTF8.GetString(outputStream.ToArray());
                return Evaluated<IList<Parser.Commands>>.Create(program, LevenshteinDistance.Compute("Hello World!", output));
            }
        }
    }
}
