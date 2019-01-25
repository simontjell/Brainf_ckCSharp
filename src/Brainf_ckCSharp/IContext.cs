using System.IO;

namespace Brainf_ckCSharp
{
  public interface IContext
  {
    Stream InputStream { get; set; }
    Stream OutputStream { get; set; }
    IParsedProgram Program { get; set; }
    IState State { get; set; }
  }
}