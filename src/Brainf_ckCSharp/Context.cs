using System.IO;

namespace Brainf_ckCSharp
{
  public class Context : IContext
  {
    public IState State { get; set; }
    public IParsedProgram Program { get; set; }
    public Stream InputStream { get; set; }
    public Stream OutputStream { get; set; }
  }
}
