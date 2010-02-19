using System;
using Csla;
using SystemX.Csla;
using CoyoEden.DataAccess;

namespace CoyoEden.CoreX.Models
{

    /// <summary>
    /// The default implementation of the <see cref="ICategoryInfo"/> interface.
    /// </summary>
    [Serializable]
    public partial class CategoryInfo:XReadOnlyBase<CategoryInfo>,ICategoryInfo
    {

        private CategoryInfo() { }

        #region ICategoryInfo Members
        private static PropertyInfo<Guid> IdPro = RegisterProperty<Guid>(x => x.Id); 
        public Guid Id
        {
            get
            {
                return GetProperty<Guid>(IdPro);
            }
        }

        private static PropertyInfo<string> NamePro = RegisterProperty<string>(x => x.Name); 
        public string Name
        {
            get
            {
                return GetProperty<string>(NamePro);
            }
        }

        private static PropertyInfo<string> DescPro = RegisterProperty<string>(x => x.Description); 
        public string Description
        {
            get
            {
                return GetProperty<string>(DescPro);
            }
        }

        private static PropertyInfo<string> TagPro = RegisterProperty<string>(x => x.Tag); 
        public string Tag
        {
            get
            {
                return GetProperty<String>(TagPro);
            }
        }

        #endregion

        #region data access

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode",
            Justification = "This method is called indirectly by the CSLA.NET DataPortal.")]
        private void Child_Fetch(ICategoryInfoDto dto)
        {
            LoadProperty<Guid>(IdPro, dto.Id);
            LoadProperty<string>(NamePro, dto.Name);
            LoadProperty<string>(DescPro, dto.Description);
            LoadProperty<string>(TagPro, dto.Tag);
        }

        #endregion
    }
}
