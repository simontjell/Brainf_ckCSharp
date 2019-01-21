using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;

namespace Brainf_ckCSharp
{
  public class State
  {
    private readonly ImmutableDictionary<long, char> _buffer;
    public int DataPointer { get; }
    public int InstructionPointer { get; }

    public State(IList<char> initialBuffer)
    {
      DataPointer = 0;
      InstructionPointer = 0;
      _buffer = initialBuffer == null ? ImmutableDictionary<long, char>.Empty : initialBuffer.Select((v, i) => new { Index = (long)i, Value = v }).ToImmutableDictionary(iv => iv.Index, iv => iv.Value);
    }

    public State(ImmutableDictionary<long, char> buffer, int dataPointer, int instructionPointer)
    {
      _buffer = buffer;
      InstructionPointer = instructionPointer;
      DataPointer = dataPointer;
    }

    public State WithUpdatedDataValue(char newValue)
      => new State(_buffer.SetItem(DataPointer, newValue), DataPointer, InstructionPointer);

    public State WithUpdatedDataValue(int delta)
      => new State(_buffer.SetItem(DataPointer, (char)((int)GetDataValueOrZero() + delta)), DataPointer, InstructionPointer);

    public State WithUpdatedDataPointer(int delta)
      => new State(_buffer, DataPointer + delta, InstructionPointer);

    public State WithUpdatedInstructionPointer(int delta)
      => new State(_buffer, DataPointer, InstructionPointer + delta);

    public char GetDataValueOrZero()
      => _buffer.TryGetValue(DataPointer, out var c) ? c : '\0';

    public bool TestValue(Func<char, bool> predicate)
      => _buffer.TryGetValue(DataPointer, out var value) && predicate(value);
  }
}
