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

        public void Interpret(Program program, IList<char> initialBuffer = null, TimeSpan? maxRunTime = null)
        {
            var state = new State(initialBuffer);
            
            var startTime = DateTime.Now;
            var endTime = maxRunTime.HasValue ? startTime.Add(maxRunTime.Value) : DateTime.MaxValue;

            while(state.InstructionPointer < program.Length && DateTime.Now <= endTime)
            {
                state = program[state.InstructionPointer].Execute(new Context { State = state, Program = program, InputStream = _inputStream, OutputStream = _outputStream }).WithUpdatedInstructionPointer(+1);
            }

            _outputStream.Flush();
        }
    }


}
