using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Brainf_ckCSharp
{
  public class SimpleInterpreter
  {
    private readonly IInterpreter _interpreter;
    private readonly IParser _parser;

    public SimpleInterpreter(IInterpreter interpreter, IParser parser)
    {
      _interpreter = interpreter ?? throw new ArgumentNullException(nameof(interpreter));
      _parser = parser ?? throw new ArgumentNullException(nameof(parser));
    }

    public string Run(string program, TimeSpan? maxRunTime = null)
      => 
        _interpreter
        .Interpret(_parser.Parse(program), maxRunTime: maxRunTime)
        .ReadOutput();
  }
}
