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
    [InlineData(TestPrograms.Fibonacci, "0\n1\n1\n2\n3\n5\n8\n13\n21\n34\n55\n89\n")]
    [InlineData(TestPrograms.HelloWorld, "Hello World!\n")]
    public void When_test_programs_are_executed_the_expected_output_is_produced(string program, string expectedOutput)
    {
      // Arrange
      var (inputStream, outputStream) = (new MemoryStream(), new MemoryStream());
      var sut = new Interpreter(inputStream, outputStream);

      // Act
      sut.Interpret(new Parser().Parse(program), maxRunTime: TimeSpan.FromSeconds(5));
      var actualOutput = Encoding.UTF8.GetString(outputStream.ToArray());

      // Assert
      Assert.StartsWith(expectedOutput, actualOutput);
    }
  }
}
