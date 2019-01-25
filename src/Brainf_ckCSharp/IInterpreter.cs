using System;
using System.Collections.Generic;

namespace Brainf_ckCSharp
{
  public interface IInterpreter
  {
    IInterpreter Interpret(IParsedProgram program, IList<char> initialBuffer = null, TimeSpan? maxRunTime = null);
    string ReadOutput();
  }
}