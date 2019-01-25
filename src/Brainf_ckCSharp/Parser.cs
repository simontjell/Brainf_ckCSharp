using System;
using System.Collections.Generic;
using System.Linq;

namespace Brainf_ckCSharp
{
  public class Parser : IParser
  {
    public enum Commands
    {
      IncrementDataPointer = 0,
      DecrementDataPointer,
      IncrementDataValue,
      DecrementDataValue,
      PrintDataValue,
      ReadDataValue,
      IfZero,
      IfNonZero
    }

    public ParsedProgram Parse(string input)
        => Validate(new ParsedProgram(input.Select(Parse).Where(c => c != null).ToList()));

    private Command Parse(char command)
    {
      switch (command)
      {
        case '>': return new IncrementDataPointerCommand();
        case '<': return new DecrementDataPointerCommand();
        case '+': return new IncrementDataValueCommand();
        case '-': return new DecrementDataValueCommand();
        case '.': return new PrintDataValueCommand();
        case ',': return new ReadDataValueCommand();
        case '[': return new IfZeroCommand();
        case ']': return new IfNonZeroCommand();
        default: return null;
      }
    }

    public class ParserException : Exception
    {
      public ParserException(string message) : base(message)
      {

      }
    }

    private ParsedProgram Validate(ParsedProgram program)
    {
      if (program.Commands.OfType<IfZeroCommand>().Count() != program.Commands.OfType<IfNonZeroCommand>().Count())
      {
        throw new ParserException("Expected the same number of ['s and ]'s");
      }

      return program;
    }
  }


}
