using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SystemX.Services;
using CoyoEden.DataAccess;
using SystemX.Infrastructure;
using Csla;

namespace CoyoEden.CoreX.Models
{
    /// <summary>
    /// Logical seperation for data access logic of Category business object.
    /// </summary>
    public partial class Category
    {
        private IRepository<ICategoryDto, Guid> _repo = ServiceLocatorX<IRepository<ICategoryDto, Guid>>.GetService();
        [RunLocal]
        protected override void DataPortal_Create()
        {
            LoadProperty<Guid>(IdPro, GuidExt.NewGuid());
            ValidationRules.CheckRules();
        }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode",
    Justification = "This method is called indirectly by the CSLA.NET DataPortal.")]
        private void DataPortal_Fetch(SingleCriteria<Category, Guid> criteria)
        {
            EnsureDependency(_repo);
            var dto=_repo.Find(criteria.Value);
            //populate
            LoadProperty<Guid>(IdPro,dto.Id);
            LoadProperty<string>(NamePro,dto.Name);
            LoadProperty<string>(DescPro,dto.Description);
            LoadProperty<string>(TagPro,dto.Tag);
            LoadProperty<Guid?>(ParentIDPro,dto.ParentID);
            //populate child data
            LoadProperty<CategoryCollection>(SubItemsPro, DataPortal.FetchChild<CategoryCollection>(dto));
            
        }

        /// <summary>
        /// Overrides the <see cref="BusinessBase"/> class's 
        /// <see cref="BusinessBase.DataPortal_Insert"/> method in order to 
        /// perform the data access necessary to insert an item into the database.
        /// </summary>
        protected override void DataPortal_Insert()
        {
            using (var context=EnsureDependency(_repo).NewTrans())
            {
                //create DTO
                var dto = ServiceLocatorX<ICategoryDto>.GetService();
                dto.ParentID = null;
                dto.Id = ReadProperty<Guid>(IdPro);
                dto.Name = ReadProperty<string>(NamePro);
                dto.Description = ReadProperty<string>(DescPro);
                dto.Tag = ReadProperty<string>(TagPro);
                //insert data
                _repo.Save<ICategoryDto>(dto);
                //update child data
                DataPortal.UpdateChild(ReadProperty<CategoryCollection>(SubItemsPro), this, _repo);
                //commit transaction
                context.Complete();
            }
        }

        /// <summary>
        /// Overrides the <see cref="BusinessBase"/> class's 
        /// <see cref="BusinessBase.DataPortal_Update"/> method in order to 
        /// perform the data access necessary to update an existing item in the database.
        /// </summary>
        protected override void DataPortal_Update()
        {
            using (var context = EnsureDependency(_repo).NewTrans())
            {
                if (IsSelfDirty)
                {
                    //create DTO
                    var dto = ServiceLocatorX<ICategoryDto>.GetService();
                    dto.ParentID = null;
                    dto.Id = ReadProperty<Guid>(IdPro);
                    dto.Name = ReadProperty<string>(NamePro);
                    dto.Description = ReadProperty<string>(DescPro);
                    dto.Tag = ReadProperty<string>(TagPro);
                    //update order data
                    _repo.Save<ICategoryDto>(dto);
                }
                //update child data
                DataPortal.UpdateChild(ReadProperty<CategoryCollection>(SubItemsPro), this, _repo);
                //commit transaction
                context.Complete();
            }
        }

        /// <summary>
        /// Overrides the <see cref="BusinessBase"/> class's 
        /// <see cref="BusinessBase.DataPortal_DeleteSelf"/> method in order to 
        /// perform the data access necessary to delete an existing item in the database.
        /// </summary>
        protected override void DataPortal_DeleteSelf()
        {
            var criteria = new SingleCriteria<Category, Guid>(ReadProperty<Guid>(IdPro));
            DataPortal_Delete(criteria);
        }

        private void DataPortal_Delete(SingleCriteria<Category, Guid> criteria)
        {
            using (var context = EnsureDependency(_repo).NewTrans())
            {
                var dto = ServiceLocatorX<ICategoryDto>.GetService();
                dto.ParentID = ReadProperty<Guid?>(ParentIDPro);
                dto.Id = ReadProperty<Guid>(IdPro);
                dto.Name = ReadProperty<string>(NamePro);
                dto.Description = ReadProperty<string>(DescPro);
                dto.Tag = ReadProperty<string>(TagPro);
                
                //delete data
                _repo.Delete(dto.Id);
                //reset child list field
                SetProperty<CategoryCollection>(SubItemsPro, DataPortal.CreateChild<CategoryCollection>());
                //commit transaction
                context.Complete();
            }
        }
        //child data access

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode",
            Justification = "This method is called indirectly by the CSLA.NET DataPortal.")]
        private void Child_Fetch(ICategoryDto dto)
        {
            //populate
            LoadProperty<Guid>(IdPro, dto.Id);
            LoadProperty<string>(NamePro, dto.Name);
            LoadProperty<string>(DescPro, dto.Description);
            LoadProperty<string>(TagPro, dto.Tag);
            LoadProperty<Guid?>(ParentIDPro, dto.ParentID);
            //child data population~...
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode",
            Justification = "This method is called indirectly by the CSLA.NET DataPortal.")]
        private void Child_Insert(ICategory parent)
        {
            EnsureDependency<IRepository<ICategoryDto, Guid>>(_repo);
            //create DTO
            var dto = ServiceLocatorX<ICategoryDto>.GetService();
            dto.ParentID = parent.Id;
            dto.Id = ReadProperty<Guid>(IdPro);
            dto.Name = ReadProperty<string>(NamePro);
            dto.Description = ReadProperty<string>(DescPro);
            dto.Tag = ReadProperty<string>(TagPro);
            //insert
            _repo.Save<ICategoryDto>(dto);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode",
            Justification = "This method is called indirectly by the CSLA.NET DataPortal.")]
        private void Child_Update(ICategory parent, IRepository<ICategoryDto,Guid> repo)
        {
            //create DTO
            var dto = ServiceLocatorX<ICategoryDto>.GetService();
            dto.ParentID = parent.Id;
            dto.Id = ReadProperty<Guid>(IdPro);
            dto.Name = ReadProperty<string>(NamePro);
            dto.Description = ReadProperty<string>(DescPro);
            dto.Tag = ReadProperty<string>(TagPro);
            //update data
            repo.Save<ICategoryDto>(dto);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode",
            Justification = "This method is called indirectly by the CSLA.NET DataPortal.")]
        private void Child_DeleteSelf(ICategory parent, IRepository<ICategoryDto, Guid> repo)
        {
            //delete data
            DataPortal_DeleteSelf();
            
        }

    }
}
