using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Brainf_ckCSharp
{
  public class SimpleInterpreter
  {
    public string Run(string program, Stream inputStream = null, TimeSpan? maxRunTime = null)
      => new Interpreter(inputStream: inputStream ?? Console.OpenStandardInput())
        .Interpret(new Parser().Parse(program), maxRunTime: maxRunTime)
        .ReadOutput();
  }
}
