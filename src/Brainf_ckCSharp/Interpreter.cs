using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Brainf_ckCSharp
{
    public class Interpreter
    {
        private readonly Stream _inputStream;
        private readonly Stream _outputStream;

        public Interpreter(Stream inputStream, Stream outputStream)
        {
            _inputStream = inputStream;
            _outputStream = outputStream;
        }
        // Idea: For longer running programs, it would probably be more efficient to translate into C# and compile it
        // ...or to translate directly to IL 

        public void Interpret(IList<Parser.Commands> program, IList<char> initialBuffer = null, TimeSpan? maxRunTime = null)
        {
            long dataPointer = 0;
            var instructionPointer = 0;
            var buffer = initialBuffer == null ? new Dictionary<long, char>() : initialBuffer.Select((v, i) => new { Index = (long)i, Value = v}).ToDictionary(iv => iv.Index, iv => iv.Value);

            char GetDataValueOrZero()
                => buffer.TryGetValue(dataPointer, out var c) ? c : '\0';
            var startTime = DateTime.Now;
            var endTime = maxRunTime.HasValue ? startTime.Add(maxRunTime.Value) : DateTime.MaxValue;

            while(instructionPointer < program.Count && DateTime.Now <= endTime)
            {
                switch(program[instructionPointer])
                {
                    case Parser.Commands.IncrementDataPointer:
                        dataPointer += 1;
                        break;

                    case Parser.Commands.DecrementDataPointer:
                        dataPointer = Math.Max(0, dataPointer - 1);
                        break;

                    case Parser.Commands.IncrementDataValue:
                        buffer[dataPointer] = (char)((int)GetDataValueOrZero() + 1);
                        break;

                    case Parser.Commands.DecrementDataValue:
                        buffer[dataPointer] = (char)((int)GetDataValueOrZero() - 1);
                        break;

                    case Parser.Commands.PrintDataValue:
                        WriteChar(GetDataValueOrZero());
                        break;

                    case Parser.Commands.ReadDataValue:
                        buffer[dataPointer] = ReadChar();
                        break;

                    case Parser.Commands.IfZero:
                        if (TestValue(buffer, dataPointer, value => value == 0))
                        {
                            var loopLevel = 1;
                            while(loopLevel > 0)
                            {
                                instructionPointer += 1;
                                var c = program[instructionPointer];
                                switch(c)
                                {
                                    case Parser.Commands.IfZero:
                                        loopLevel += 1;
                                        break;

                                    case Parser.Commands.IfNonZero:
                                        loopLevel -= 1;
                                        break;

                                    default:
                                        // Do nothing
                                        break;
                                }
                            }
                        }
                        break;

                    case Parser.Commands.IfNonZero:
                        if(TestValue(buffer, dataPointer, value => value != 0))
                        {
                            var loopLevel = 1;
                            while(loopLevel > 0)
                            {
                                instructionPointer -= 1;
                                var c = program[instructionPointer];
                                switch(c)
                                {
                                    case Parser.Commands.IfZero:
                                        loopLevel -= 1;
                                        break;

                                    case Parser.Commands.IfNonZero:
                                        loopLevel += 1;
                                        break;

                                    default:
                                        // Do nothing
                                        break;
                                }
                            }
                            instructionPointer -= 1;
                        }
                        break;
                }

                instructionPointer += 1; 
            }
        }

        private bool TestValue(Dictionary<long, char> buffer, long dataPointer, Func<char, bool> predicate)
            => buffer.TryGetValue(dataPointer, out var value) && predicate(value);

        private void WriteChar(char c)
            => _outputStream.WriteByte((byte)c);

        private char ReadChar()
        {
            int b;
            while ((b = _inputStream.ReadByte()) == -1);
            return (char)b;
        }
    }


}
