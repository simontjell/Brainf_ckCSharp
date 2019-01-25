using System.Collections.Generic;

namespace Brainf_ckCSharp
{
  public interface IParsedProgram
  {
    Command this[int index] { get; }

    IList<Command> Commands { get; }
    int Length { get; }
  }
}