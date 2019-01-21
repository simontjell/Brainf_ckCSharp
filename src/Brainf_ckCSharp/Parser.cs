using System;
using System.Collections.Generic;
using System.Linq;

namespace Brainf_ckCSharp
{
    public class Parser
    {
        public enum Commands {
            IncrementDataPointer = 0,
            DecrementDataPointer,
            IncrementDataValue,
            DecrementDataValue,
            PrintDataValue,
            ReadDataValue,
            IfZero,
            IfNonZero
        }

        public Program Parse(string input)
            => Validate(new Program(input.Select(Parse).Where(c => c != null).ToList()));

        private Command Parse(char command){
            switch(command){
                case '>': return new IncrementDataPointerCommand();
                case '<': return new DecrementDataPointerCommand();
                case '+': return new IncrementDataValueCommand();
                case '-': return new DecrementDataValueCommand();
                case '.': return new PrintDataValueCommand();
                case ',': return new ReadDataValueCommand();
                case '[': return new IfZeroCommand();
                case ']': return new IfNonZeroCommand();
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

        private Program Validate(Program program){
            //if(program.Where(c => c == Commands.IfZero).Count() != program.Where(c => c == Commands.IfNonZero).Count())
            //{
            //    throw new Exception("Expected the same number of ['s and ]'s");
            //}

            return program;
        }
    }


}
