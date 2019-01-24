using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Brainf_ckCSharp
{
  public class Context
  {
    public State State { get; set; }
    public ParsedProgram Program { get; set; }
    public Stream InputStream { get; set; }
    public Stream OutputStream { get; set; }
  }

  public abstract class Command
  {
    public abstract State Execute(Context context);
  }

  public class IncrementDataPointerCommand : Command
  {
    public override State Execute(Context context)
      => context.State.WithUpdatedDataPointer(+1);
  }

  public class DecrementDataPointerCommand : Command
  {
    public override State Execute(Context context)
      => context.State.WithUpdatedDataPointer(-1);
  }

  public class IncrementDataValueCommand : Command
  {
    public override State Execute(Context context)
      => context.State.WithUpdatedDataValue(1);
  }

  public class DecrementDataValueCommand : Command
  {
    public override State Execute(Context context)
      => context.State.WithUpdatedDataValue(-1);
  }

  public class IfZeroCommand : Command
  {
    public override State Execute(Context context)
    {
      if (context.State.TestValue(value => value == 0))
      {
        var loopLevel = 1;
        while (loopLevel > 0)
        {
          context.State = context.State.WithUpdatedInstructionPointer(1);
          if (context.State.InstructionPointer >= context.Program.Length)
          {
            throw new InstructionPointOutOfRangeException();
          }
          var c = context.Program[context.State.InstructionPointer];

          if (c is IfZeroCommand)
          {
            loopLevel += 1;
          }
          else if (c is IfNonZeroCommand)
          {
            loopLevel -= 1;
          }

        }
      }

      return context.State;
    }
  }

  public class IfNonZeroCommand : Command
  {
    public override State Execute(Context context)
    {
      if (context.State.TestValue(value => value != 0))
      {
        var loopLevel = 1;
        while (loopLevel > 0)
        {
          context.State = context.State.WithUpdatedInstructionPointer(-1);
          if (context.State.InstructionPointer >= context.Program.Length)
          {
            throw new InstructionPointOutOfRangeException();
          }
          var c = context.Program[context.State.InstructionPointer];

          if (c is IfZeroCommand)
          {
            loopLevel -= 1;
          }
          else if (c is IfNonZeroCommand)
          {
            loopLevel += 1;
          }
        }
        context.State = context.State.WithUpdatedInstructionPointer(-1);
      }

      return context.State;
    }
  }


  public class PrintDataValueCommand : Command
  {
    public override State Execute(Context context)
    {
      WriteChar(context.OutputStream, context.State.GetDataValueOrZero());
      return context.State;
    }

    private void WriteChar(Stream outputStream, char c)
      => outputStream.WriteByte((byte)c);
  }

  public class ReadDataValueCommand : Command
  {
    public override State Execute(Context context)
      => context.State.WithUpdatedDataValue(ReadChar(context.InputStream));

    private char ReadChar(Stream inputStream)
    {
      int b;
      while ((b = inputStream.ReadByte()) == -1) ;
      return (char)b;
    }
  }
}
