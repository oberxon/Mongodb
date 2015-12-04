using System.Reflection;

namespace Manage.Service.MongoDb
{
    public abstract partial class assemblyInfo
    {
        public abstract Assembly GetAssembly();
    }

    public partial class fileLocatedAssemblyInfo
    {
        public override Assembly GetAssembly()
        {
            return Assembly.LoadFile(this.assemblyPath);
        }
    }

    public partial class longNameLocatedAssemblyInfo
    {
        public override Assembly GetAssembly()
        {
            return Assembly.Load(this.assemblyName);
        }
    }
}
