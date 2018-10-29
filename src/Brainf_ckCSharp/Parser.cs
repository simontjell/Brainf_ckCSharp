using System;
using System.Collections.Generic;
using System.Linq;

namespace Brainf_ckCSharp
{
    public class Parser
    {
        public enum Commands {
            IncrementDataPointer,
            DecrementDataPointer,
            IncrementDataValue,
            DecrementDataValue,
            PrintDataValue,
            ReadDataValue,
            IfZero,
            IfNonZero
        }

        public IList<Commands> Parse(string program)
            => Validate(program.Select(Parse).Where(c => c.HasValue).Select(c => c.Value).ToList());

        private Commands? Parse(char command){
            switch(command){
                case '>': return Commands.IncrementDataPointer;
                case '<': return Commands.DecrementDataPointer;
                case '+': return Commands.IncrementDataValue;
                case '-': return Commands.DecrementDataValue;
                case '.': return Commands.PrintDataValue;
                case ',': return Commands.ReadDataValue;
                case '[': return Commands.IfZero;
                case ']': return Commands.IfNonZero;
                default:
                    if(char.IsWhiteSpace(command)) 
                    {
                        return null;
                    }
                    else
                    {
                        throw new Exception("Unhandled command: " + command);
                    }
            }
        }

        private IList<Commands> Validate(IList<Commands> program){
            if(program.Where(c => c == Commands.IfZero).Count() != program.Where(c => c == Commands.IfNonZero).Count())
            {
                throw new Exception("Expected the same number of ['s and ]'s");
            }

            return program;
        }
    }


}
