using System;

namespace CoyoEden.CoreX
{
    public interface ICategoryInfo
    {
        Guid Id { get;}
        string Name { get;}
        string Description { get;}
        string Tag { get;}
    }
}
