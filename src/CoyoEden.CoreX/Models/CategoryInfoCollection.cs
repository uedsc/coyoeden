using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SystemX.Csla;
using CoyoEden.DataAccess;
using SystemX.Services;
using SystemX.Infrastructure;
using Csla;

namespace CoyoEden.CoreX.Models
{
    /// <summary>
    /// A read-only collection of <see cref="ICategoryInfo"/> objects.
    /// </summary>
    [Serializable]
    public partial class CategoryInfoCollection:XReadOnlyListBase<CategoryInfoCollection,ICategoryInfo>
    {
        private CategoryInfoCollection()
        {
            
        }

        #region Data Access

        private IRepository<ICategoryInfoDto, Guid> repo = ServiceLocatorX<IRepository<ICategoryInfoDto, Guid>>.GetService();

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode",
            Justification = "Method called dynamically by CSLA.NET.")]
        private void DataPortal_Fetch()
        {
            RaiseListChangedEvents = false;
            EnsureDependency<IRepository<ICategoryInfoDto, Guid>>(repo);

            IsReadOnly = false;
            foreach (var dto in repo.GetAll())
            {
                var child = DataPortal.FetchChild<CategoryInfo>(dto);
                this.Add(child);
            }
            IsReadOnly = true;

            RaiseListChangedEvents = true;
        }

        #endregion

    }
}
