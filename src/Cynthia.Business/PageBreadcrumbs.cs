//		Author:				Joe Audette
//		Created:			2005-01-01
//		Last Modified:		2009-02-01
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
using System.Globalization;
using Cynthia.Data;

namespace Cynthia.Business
{
	/// <summary>
	///	 Represents the breadcrumbs for a page
	/// </summary>
	public class PageBreadcrumbs
	{
		#region Constructors

		public PageBreadcrumbs(int pageId)
		{
			GetBreadcrumbs(pageId);
			
		}

		#endregion


		#region Private Properties

		private int pageID = -1;
		private string pageName = string.Empty;

		private int parent1ID = -1;
		private string parent1Name = string.Empty;
		private int parent2ID = -1;
		private string parent2Name = string.Empty;
		private int parent3ID = -1;
		private string parent3Name = string.Empty;
		private int parent4ID = -1;
		private string parent4Name = string.Empty;
		private int parent5ID = -1;
		private string parent5Name = string.Empty;
		private int parent6ID = -1;
		private string parent6Name = string.Empty;
		private int parent7ID = -1;
		private string parent7Name = string.Empty;
		private int parent8ID = -1;
		private string parent8Name = string.Empty;
		private int parent9ID = -1;
		private string parent9Name = string.Empty;
		private int parent10ID = -1;
		private string parent10Name = string.Empty;
		private int parent11ID = -1;
		private string parent11Name = string.Empty;
		private int parent12ID = -1;
		private string parent12Name = string.Empty;



		#endregion

		#region Public Properties

		public int PageId
		{	
			get {return pageID;}	
		}

		public string PageName
		{	
			get {return pageName;}	
		}

		public int Parent1Id
		{	
			get {return parent1ID;}
		}

		public string Parent1Name
		{	
			get {return parent1Name;}
		}

		public int Parent2Id
		{	
			get {return parent2ID;}
		}

		public string Parent2Name
		{	
			get {return parent2Name;}
		}

		public int Parent3Id
		{	
			get {return parent3ID;}
		}

		public string Parent3Name
		{	
			get {return parent3Name;}
		}

		public int Parent4Id
		{	
			get {return parent4ID;}
		}

		public string Parent4Name
		{	
			get {return parent4Name;}
		}

		public int Parent5Id
		{	
			get {return parent5ID;}
		}

		public string Parent5Name
		{	
			get {return parent5Name;}
		}

		public int Parent6Id
		{	
			get {return parent6ID;}
		}

		public string Parent6Name
		{	
			get {return parent6Name;}
		}

		public int Parent7Id
		{	
			get {return parent7ID;}
		}

		public string Parent7Name
		{	
			get {return parent7Name;}
		}

		public int Parent8Id
		{	
			get {return parent8ID;}
		}

		public string Parent8Name
		{	
			get {return parent8Name;}
		}

		public int Parent9Id
		{	
			get {return parent9ID;}
		}

		public string Parent9Name
		{	
			get {return parent9Name;}
		}

		public int Parent10Id
		{	
			get {return parent10ID;}
		}

		public string Parent10Name
		{	
			get {return parent10Name;}
		}

		public int Parent11Id
		{	
			get {return parent11ID;}
		}

		public string Parent11Name
		{	
			get {return parent11Name;}
		}

		public int Parent12Id
		{	
			get {return parent12ID;}
		}

		public string Parent12Name
		{	
			get {return parent12Name;}
		}



		#endregion


		#region Private Methods

		private void GetBreadcrumbs(int pageId)
		{
            using (IDataReader reader = DBPageSettings.GetBreadcrumbs(pageId))
            {
                if (reader.Read())
                {
                    this.pageID = Convert.ToInt32(reader["PageID"], CultureInfo.InvariantCulture);
                    this.pageName = reader["PageName"].ToString();

                    this.parent1ID = Convert.ToInt32(reader["Parent1ID"], CultureInfo.InvariantCulture);
                    this.parent1Name = reader["Parent1Name"].ToString();
                    this.parent2ID = Convert.ToInt32(reader["Parent2ID"], CultureInfo.InvariantCulture);
                    this.parent2Name = reader["Parent2Name"].ToString();
                    this.parent3ID = Convert.ToInt32(reader["Parent3ID"], CultureInfo.InvariantCulture);
                    this.parent3Name = reader["Parent3Name"].ToString();
                    this.parent4ID = Convert.ToInt32(reader["Parent4ID"], CultureInfo.InvariantCulture);
                    this.parent4Name = reader["Parent4Name"].ToString();
                    this.parent5ID = Convert.ToInt32(reader["Parent5ID"], CultureInfo.InvariantCulture);
                    this.parent5Name = reader["Parent5Name"].ToString();
                    this.parent6ID = Convert.ToInt32(reader["Parent6ID"], CultureInfo.InvariantCulture);
                    this.parent6Name = reader["Parent6Name"].ToString();
                    this.parent7ID = Convert.ToInt32(reader["Parent7ID"], CultureInfo.InvariantCulture);
                    this.parent7Name = reader["Parent7Name"].ToString();
                    this.parent8ID = Convert.ToInt32(reader["Parent8ID"], CultureInfo.InvariantCulture);
                    this.parent8Name = reader["Parent8Name"].ToString();
                    this.parent9ID = Convert.ToInt32(reader["Parent9ID"], CultureInfo.InvariantCulture);
                    this.parent9Name = reader["Parent9Name"].ToString();
                    this.parent10ID = Convert.ToInt32(reader["Parent10ID"], CultureInfo.InvariantCulture);
                    this.parent10Name = reader["Parent10Name"].ToString();
                    this.parent11ID = Convert.ToInt32(reader["Parent11ID"], CultureInfo.InvariantCulture);
                    this.parent11Name = reader["Parent11Name"].ToString();
                    this.parent12ID = Convert.ToInt32(reader["Parent12ID"], CultureInfo.InvariantCulture);
                    this.parent12Name = reader["Parent12Name"].ToString();


                }

            }


		}



		#endregion


	}
}
