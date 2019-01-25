using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Brainf_ckCSharp
{
  public class Interpreter : IDisposable, IInterpreter
  {
    private readonly Stream _inputStream;
    private readonly Stream _outputStream;

    public Interpreter(Stream inputStream = null, Stream outputStream = null)
    {
      _inputStream = inputStream ?? new MemoryStream();
      _outputStream = outputStream ?? new MemoryStream();
    }
    // Idea: For longer running programs, it would probably be more efficient to translate into C# and compile it
    // ...or to translate directly to IL 

    public class InterpreterException : Exception
    {
      public InterpreterException(Exception inner) : base(inner.Message, inner)
      {

      }
    }

    public IInterpreter Interpret(IParsedProgram program, IList<char> initialBuffer = null, TimeSpan? maxRunTime = null)
    {
      try
      {
        IState state = new State(initialBuffer);

        var startTime = DateTime.Now;
        var endTime = maxRunTime.HasValue ? startTime.Add(maxRunTime.Value) : DateTime.MaxValue;

        while (state.InstructionPointer < program.Length && DateTime.Now <= endTime)
        {
          state = program[state.InstructionPointer].Execute(new Context { State = state, Program = program, InputStream = _inputStream, OutputStream = _outputStream }).WithUpdatedInstructionPointer(+1);
        }

        _outputStream.Flush();

        return this;
      }
      catch (Exception exception)
      {
        throw new InterpreterException(exception); 
      }
    }

    public string ReadOutput()
    {
      using (var ms = new MemoryStream())
      {
        var position = _outputStream.Position;
        _outputStream.Position = 0;
        _outputStream.CopyTo(ms);
        _outputStream.Position = position;
        return Encoding.UTF8.GetString(ms.ToArray());
      }
    }

    public void Dispose()
    {
      _outputStream.Flush();
      _inputStream.Dispose();
      _outputStream.Dispose();
    }
  }


}
