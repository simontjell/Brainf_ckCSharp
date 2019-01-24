using System.Collections.Generic;
using static Brainf_ckCSharp.Parser;

namespace Brainf_ckCSharp
{
  public class ParsedProgram
  {
    public ParsedProgram(IList<Command> commands)
    {
      Commands = commands;
    }

    public IList<Command> Commands { get; }

    public int Length => Commands.Count;

    public Command this[int index]
      => Commands[index];
  }
}