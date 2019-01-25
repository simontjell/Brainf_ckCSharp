using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;

namespace Brainf_ckCSharp
{
  public class State : IState
  {
    private readonly IImmutableDictionary<long, char> _buffer;
    public int DataPointer { get; }
    public int InstructionPointer { get; }

    public State(IList<char> initialBuffer)
    {
      DataPointer = 0;
      InstructionPointer = 0;
      _buffer = initialBuffer == null ? ImmutableDictionary<long, char>.Empty : initialBuffer.Select((v, i) => new { Index = (long)i, Value = v }).ToImmutableDictionary(iv => iv.Index, iv => iv.Value);
    }

    public State(IImmutableDictionary<long, char> buffer, int dataPointer, int instructionPointer)
    {
      _buffer = buffer;
      InstructionPointer = instructionPointer;
      DataPointer = dataPointer;
    }

    public IState WithUpdatedDataValue(char newValue)
      => new State(_buffer.SetItem(DataPointer, newValue), DataPointer, InstructionPointer);

    public IState WithUpdatedDataValue(int delta)
      => new State(_buffer.SetItem(DataPointer, (char)((int)GetDataValueOrZero() + delta)), DataPointer, InstructionPointer);

    public IState WithUpdatedDataPointer(int delta)
      => new State(_buffer, DataPointer + delta, InstructionPointer);

    public IState WithUpdatedInstructionPointer(int delta)
      => new State(_buffer, DataPointer, InstructionPointer + delta);

    public char GetDataValueOrZero()
      => _buffer.TryGetValue(DataPointer, out var c) ? c : '\0';

    public bool TestValue(Func<char, bool> predicate)
      => _buffer.TryGetValue(DataPointer, out var value) && predicate(value);
  }
}
