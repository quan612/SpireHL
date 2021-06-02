using System.IO;
using System.Reflection;

namespace SpireHL.Core
{
    public class ModuleConstants
    {
        public static readonly string ProjectDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
    }
}
