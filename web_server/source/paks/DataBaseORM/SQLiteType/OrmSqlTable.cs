using System.Reflection.PortableExecutable;

namespace web_server{
    abstract class OrmSqlTable
    {
        abstract public bool Create();
        abstract public bool IsNotNull();
    }
}