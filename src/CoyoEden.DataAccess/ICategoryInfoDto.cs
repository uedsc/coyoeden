using System;

namespace CoyoEden.DataAccess
{
    public interface ICategoryInfoDto
    {
        Guid Id { get; set; }
        string Name { get; set; }
        string Description { get; set; }
        string Tag { get; set; }
    }
}
