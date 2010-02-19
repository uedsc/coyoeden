using System;
using System.Linq;
using System.Collections.Generic;
using SubSonic.SqlGeneration.Schema;

namespace CoyoEden.DataAccess.SubSonicX
{
    [SubSonicTableNameOverride("cy_Category")]
    public class Category:RecordBase<Guid>,ICategoryDto
    {
        #region ICategoryDto Members

        public IEnumerable<ICategoryDto> SubItems
        {
            get {
                var items= GetForeignList<Category, Guid>(x => x.ParentID.Value == Id);
                return items.Cast<ICategoryDto>();
            }
        }

        public string Name
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }

        public string Tag
        {
            get;
            set;
        }

        public Guid? ParentID
        {
            get;
            set;
        }

        #endregion
    }
}
