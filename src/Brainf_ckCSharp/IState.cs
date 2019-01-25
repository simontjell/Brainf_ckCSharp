using System;

namespace Brainf_ckCSharp
{
  public interface IState
  {
    int DataPointer { get; }
    int InstructionPointer { get; }

    char GetDataValueOrZero();
    bool TestValue(Func<char, bool> predicate);
    IState WithUpdatedDataPointer(int delta);
    IState WithUpdatedDataValue(char newValue);
    IState WithUpdatedDataValue(int delta);
    IState WithUpdatedInstructionPointer(int delta);
  }
}