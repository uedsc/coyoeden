using System;
using System.Collections.Generic;

namespace CoyoEden.DataAccess
{
    public partial interface ICategoryDto
    {
        IEnumerable<ICategoryDto> SubItems { get; }
    }
}
