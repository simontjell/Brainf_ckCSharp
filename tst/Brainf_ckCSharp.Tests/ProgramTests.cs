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
    public void When_test_programs_are_executed_the_expected_output_is_produced(string program, string expectedOutput, int? maxRunTimeInSeconds = null)
    {
      // Arrange
      var sut = new SimpleInterpreter(new Interpreter(), new Parser());
      var maxRunTime = maxRunTimeInSeconds.HasValue ? (TimeSpan?)TimeSpan.FromSeconds(maxRunTimeInSeconds.Value) : null;

      // Act
      var actualOutput = sut.Run(program, maxRunTime: maxRunTime);

      // Assert
      Assert.StartsWith(expectedOutput, actualOutput);
    }
  }
}
