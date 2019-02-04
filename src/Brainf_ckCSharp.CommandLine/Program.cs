using System;
using System.IO;
using CommandLine;

namespace Brainf_ckCSharp.Cli
{
  [Verb("run")]
  public class RunOptions
  {
    [Option('f', HelpText = "The path to a file containing a program")]
    public string File { get; set; }

    [Option('p', HelpText = "A program as a string")]
    public string Program { get; set; }

    [Option('t', HelpText = "Maximum execution time (in seconds)")]
    public int? RunTime { get; set; }
  }

  public enum ReturnCodes
  {
    Ok = 0,
    Error = 1,
    NoProgram = 2,
    InterpreterError = 3
  }

  class Program
  {
    static int Main(string[] args)
      => CommandLine.Parser.Default.ParseArguments<RunOptions>(args).MapResult(
          (RunOptions options) => (int)HandleRunVerb(options),
          errors => (int)ReturnCodes.Error
        );

    private static ReturnCodes HandleRunVerb(RunOptions options)
    {
      if(String.IsNullOrEmpty(options.File) == false)
      {
        if(File.Exists(options.File))
        {
          return RunProgram(File.ReadAllText(options.File), options);
        }
      }

      if(String.IsNullOrEmpty(options.Program) == false)
      {
        return RunProgram(options.Program?.Trim('\"') ?? string.Empty, options);
      }

      Console.Error.WriteLine("Please provide either a path for a file containing a program or a program as a string");
      return ReturnCodes.NoProgram;
    }

    private static ReturnCodes RunProgram(string program, RunOptions options)
    {
      try
      {
        new Interpreter(Console.OpenStandardInput(), Console.OpenStandardOutput()).Interpret(
          new Brainf_ckCSharp.Parser().Parse(program),
          maxRunTime: options.RunTime.HasValue ? (TimeSpan?)TimeSpan.FromSeconds(options.RunTime.Value) : null
        );

        return ReturnCodes.Ok;
      }
      catch (Parser.ParserException exception)
      {
        Console.Error.WriteLine($"An error occurred during parsing: {exception.Message}");
        return ReturnCodes.InterpreterError;
      }
      catch (Interpreter.InterpreterException exception)
      {
        Console.Error.WriteLine($"An error occurred during interpretation: {exception.Message}");
        return ReturnCodes.InterpreterError;
      }
    }
  }
}
