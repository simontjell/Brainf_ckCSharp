# Brainf_ckCSharp
A simple implementation of an interpreter for the Brainf*ck language in C#

You can use it in your own programs like this:
```csharp
var parser = new Parser();
var program = parser.Parse("++++++++++[>+++++++>++++++++++>+++>+<<<<-]>++.>+.+++++++..+++.>++.<<+++++++++++++++.>.+++.------.--------.>+.>.");
var interpreter = new Interpreter();

var output =
  interpreter
  .Interpret(program)
  .ReadOutput()
;
```

...or by using the combined parser/interpreter like this:
```csharp
var output = 
    new SimpleInterpreter(new Interpreter(), new Parser())
    .Run("++++++++++[>+++++++>++++++++++>+++>+<<<<-]>++.>+.+++++++..+++.>++.<<+++++++++++++++.>.+++.------.--------.>+.>.");
```