using System.IO;

namespace CoyoEden.Core
{
    public static class BOBroker
    {
        public static string GetClassDefsXml()
        {
            StreamReader classDefStream = new StreamReader(
                typeof(BOBroker).Assembly.GetManifestResourceStream("CoyoEden.Core.ClassDefs.xml"));
            return classDefStream.ReadToEnd();
        }
    }
}