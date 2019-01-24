using System;
using Xunit;
using Brainf_ckCSharp;
using System.IO;
using System.Text;

namespace Brainf_ckCSharp.Tests
{
  public class ProgramTests
  {
    [Theory]
    [InlineData(TestPrograms.Fibonacci, "0\n1\n1\n2\n3\n5\n8\n13\n21\n34\n55\n89\n", 5)]
    [InlineData(TestPrograms.HelloWorld, "Hello World!\n")]
    [InlineData(TestPrograms.Comment, "")]
    public void When_test_programs_are_executed_the_expected_output_is_produced(string program, string expectedOutput, int? maxRunTime = null)
    {
      // Arrange
      var sut = new Interpreter();

      // Act
      var actualOutput = 
        sut
        .Interpret(new Parser().Parse(program), maxRunTime: maxRunTime.HasValue ? (TimeSpan?)TimeSpan.FromSeconds(maxRunTime.Value) : null)
        .ReadOutput();
      
      // Assert
      Assert.StartsWith(expectedOutput, actualOutput);
    }
  }
}
