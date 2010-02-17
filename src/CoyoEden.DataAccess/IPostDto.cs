using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoyoEden.DataAccess
{
    public interface IPostDto
    {
        Guid Id { get; set; }
        string Title { get; set; }

    }
}
