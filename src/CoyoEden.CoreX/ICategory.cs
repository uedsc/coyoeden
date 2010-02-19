using System;
using Csla.Core;
using System.ComponentModel;
using CoyoEden.CoreX.Models;
namespace CoyoEden.CoreX
{
    public interface ICategory : IEditableBusinessObject, INotifyPropertyChanged
    {
        Guid Id { get; set; }
        string Name { get; set; }
        string Description { get; set; }
        string Tag { get; set; }
        Guid? ParentID { get; set; }

        CategoryCollection SubItems { get; }
    }
}
