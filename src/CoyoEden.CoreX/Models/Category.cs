using System;
using SystemX.Csla;
using Csla;
using Csla.Validation;

namespace CoyoEden.CoreX.Models
{
    [Serializable]
    public partial class Category:XBusinessBase<Category>,ICategory
    {

        #region .ctor
        private Category() { }
        #endregion

        #region ICategory Members
        private static PropertyInfo<Guid> IdPro = RegisterProperty<Guid>(x => x.Id);
        public Guid Id
        {
            get
            {
                return GetProperty<Guid>(IdPro);
            }
            set
            {
                SetProperty<Guid>(IdPro, value);
            }
        }
        private static PropertyInfo<string> NamePro = RegisterProperty<string>(x => x.Name); 
        public string Name
        {
            get
            {
                return GetProperty<string>(NamePro);
            }
            set
            {
                SetProperty<string>(NamePro, value);
            }
        }
        private static PropertyInfo<string> DescPro = RegisterProperty<string>(x => x.Description); 
        public string Description
        {
            get
            {
                return GetProperty<string>(DescPro);
            }
            set
            {
                SetProperty<string>(DescPro, value);
            }
        }
        private static PropertyInfo<string> TagPro = RegisterProperty<string>(x => x.Tag); 
        public string Tag
        {
            get
            {
                return GetProperty<string>(TagPro);
            }
            set
            {
                SetProperty<string>(TagPro, value);
            }
        }
        private static PropertyInfo<Guid?> ParentIDPro = RegisterProperty<Guid?>(x => x.ParentID); 
        public Guid? ParentID
        {
            get
            {
                return GetProperty<Guid?>(ParentIDPro);
            }
            set
            {
                SetProperty<Guid?>(ParentIDPro, value);
            }
        }
        private static PropertyInfo<CategoryCollection> SubItemsPro = RegisterProperty<CategoryCollection>(x => x.SubItems); 
        public CategoryCollection SubItems
        {
            get {
                if (!(FieldManager.FieldExists(SubItemsPro)))
                {
                    LoadProperty<CategoryCollection>(SubItemsPro, DataPortal.CreateChild<CategoryCollection>());
                }
                return GetProperty<CategoryCollection>(SubItemsPro);
            }
        }
        #endregion

        #region validation rules
        protected override void AddBusinessRules()
        {
            ValidationRules.AddRule(CommonRules.StringRequired, new RuleArgs(NamePro));
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs(NamePro, 50));
        }
        #endregion

    }
}
