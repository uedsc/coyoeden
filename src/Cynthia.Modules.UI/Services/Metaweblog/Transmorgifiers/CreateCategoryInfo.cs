// Author:				Tom Opgenorth	
// Created:				2008-04-11
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.
using System;
using System.Data;

using Cynthia.Web.Services.Metaweblog.Domain;

namespace Cynthia.Web.Services.Metaweblog.Transmorgifiers
{
    /// <summary>
    /// This class will convert the current record in an <see cref="IDataReader "/> into a 
    /// <see cref="CategoryInfo" /> object.
    /// </summary>
    public class CreateCategoryInfo : ITransmorgifier<IDataReader, CategoryInfo>
    {
        private int _moduleId;
        private readonly string _siteRoot;

        public CreateCategoryInfo(string siteRoot) : this(siteRoot, Int32.MinValue) {}

        public CreateCategoryInfo(string siteRoot, int moduleId)
        {
            _moduleId = moduleId;
            _siteRoot = siteRoot;
        }

        public int ModuleId
        {
            get { return _moduleId; }
            set { _moduleId = value; }
        }

        public CategoryInfo Transmorgify(IDataReader rdr)
        {
            CategoryInfo categoryInfo = new CategoryInfo();
            string categoryId = rdr["CategoryID"].ToString();
            string title = rdr["Category"].ToString();
            categoryInfo.categoryid = categoryId;
            categoryInfo.title = title;
            categoryInfo.description = title;
            categoryInfo.htmlUrl = String.Format("{2}/BlogCategoryView.aspx?cat={0}&mid={1}&pageid=1", categoryId, _moduleId, _siteRoot);
            categoryInfo.rssUrl = categoryInfo.htmlUrl; // TODO [TO080428@2145] Confirm this.
            return categoryInfo;
        }
    }
}