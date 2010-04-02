// Author:					Joe Audette
// Created:				    2004-07-19
// Last Modified:		    2009-07-22
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software. 

using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using Cynthia.Data;
using log4net;

namespace Cynthia.Business
{
	/// <summary>
	/// Represents a user role.
	/// </summary>
	public class Role
	{
        private static readonly ILog log = LogManager.GetLogger(typeof(Role));

		#region Constructors


		public Role()
		{}

		public Role(int roleId)
		{
			GetRole(roleId);
		}

		public Role(int siteId, string roleName)
		{
			GetRole(siteId, roleName);
		}


		#endregion

		#region Private Properties

        private Guid roleGuid = Guid.Empty;
		private int roleID = -1;
		private int siteID = -1;
        private Guid siteGuid = Guid.Empty;
		private string roleName = string.Empty;
		private string displayName = string.Empty;
        private bool enforceRelatedSitesMode = false;

        

        private static bool UseRelatedSiteMode
        {
            get
            {
                if (
                    (ConfigurationManager.AppSettings["UseRelatedSiteMode"] != null)
                    && (ConfigurationManager.AppSettings["UseRelatedSiteMode"] == "true")
                )
                {
                    return true;
                }

                return false;
            }
        }

        private static int RelatedSiteID
        {
            get
            {
                int result = 1;
                if (ConfigurationManager.AppSettings["RelatedSiteID"] != null)
                {
                    int.TryParse(ConfigurationManager.AppSettings["RelatedSiteID"], out result);
                }

                return result;
            }
        }

		#endregion

		#region Public Properties

		public int RoleId
		{
			get{return roleID;}
		}

        public Guid RoleGuid
        {
            get { return roleGuid; }
           
        }

        public Guid SiteGuid
        {
            get { return siteGuid; }
            set { siteGuid = value; }
        }

		public int SiteId
		{
			get
            {
                //if (UseRelatedSiteMode) { return RelatedSiteID; }
                return siteID;
            }
			set
            {
                //if (UseRelatedSiteMode)
                //{
                //    siteID = RelatedSiteID;
                //}
                //else
                //{
                   siteID = value;
                //}
            }
		}

		public string RoleName
		{
			get{return displayName;}
			set{displayName = value;}
		}

        public string DisplayName
        {
            get { return displayName; }
        }

        public bool EnforceRelatedSitesMode
        {
            get { return enforceRelatedSitesMode; }
            set { enforceRelatedSitesMode = value; }
        }

		#endregion


		#region Private Methods

		private void GetRole(int roleId)
		{
            using (IDataReader reader = DBRoles.GetById(roleId))
            {
                if (reader.Read())
                {
                    this.roleID = int.Parse(reader["RoleID"].ToString());

                    this.siteID = int.Parse(reader["SiteID"].ToString());

                    if (UseRelatedSiteMode) { siteID = RelatedSiteID; }

                    this.roleName = reader["RoleName"].ToString();
                    this.displayName = reader["DisplayName"].ToString();
                    this.siteGuid = new Guid(reader["SiteGuid"].ToString());
                    this.roleGuid = new Guid(reader["RoleGuid"].ToString());

                }

            }


		}

		private void GetRole(int siteId, string roleName)
		{
            if (UseRelatedSiteMode) { siteId = RelatedSiteID; }

            using (IDataReader reader = DBRoles.GetByName(siteId, roleName))
            {
                if (reader.Read())
                {
                    this.roleID = int.Parse(reader["RoleID"].ToString());
                    this.siteID = int.Parse(reader["SiteID"].ToString());
                    this.roleName = reader["RoleName"].ToString();
                    this.displayName = reader["DisplayName"].ToString();
                    this.siteGuid = new Guid(reader["SiteGuid"].ToString());
                    this.roleGuid = new Guid(reader["RoleGuid"].ToString());

                }

            }


		}

		private bool Create()
		{
			int newID = 0;
			// role name never changes after creation
			// only display name is allowed to change
			// otherwise permissions to view pages 
			// and edit modules would be orphaned when
			// a role was re-named

            if (UseRelatedSiteMode && enforceRelatedSitesMode) { siteID = RelatedSiteID; }

			if(Exists(this.siteID, this.displayName))
			{
				//string errorMessage = ConfigurationManager.AppSettings["RoleExistsError"];
				//throw new Exception(errorMessage);
                // do nothing instead of  throwing an exception

			}
			else
			{
                this.roleGuid = Guid.NewGuid();

               
				newID = DBRoles.RoleCreate(
                    this.roleGuid,
                    this.siteGuid,
                    this.siteID,
                    this.displayName);
			}

			if(newID > 0)
			{
				this.roleID = newID;
				this.roleName = this.displayName;
				return true;
			}
			else
			{
				return false;
			}
		}

		private bool Update()
		{
			return DBRoles.Update(this.roleID,this.displayName);
		}

        public bool Equals(string roleName)
        {
            bool result = false;
            if (roleName == this.roleName) result = true;

            return result;

        }

		#endregion


		#region Public Methods

		

		public bool Save()
		{
			if(this.roleID > -1)
			{
				return Update();
			}
			else
			{
				return Create();
			}
		}

        public bool HasUsers()
        {
            return (CountOfUsers() > 0);
        }

        public int CountOfUsers()
        {
            // TODO: implement actual select count from db
            // this is works but is not ideal
            int count = 0;
            using (IDataReader reader = GetRoleMembers(this.roleID))
            {
                while (reader.Read())
                {
                    count += 1;
                }
            }

            return count;

        }



		#endregion

		#region Static Methods

		public static IDataReader GetSiteRoles(int siteId)
		{
            if (UseRelatedSiteMode) { siteId = RelatedSiteID; }

			return DBRoles.GetSiteRoles(siteId);
		}

        //public static Collection<Role> GetbySite(int siteId)
        //{
        //    bool enforceRelatedSitesMode = false;
        //    return GetbySite(siteId, enforceRelatedSitesMode);
        //}

        public static Collection<Role> GetbySite(int siteId, bool enforceRelatedSitesMode)
        {
            if (UseRelatedSiteMode && enforceRelatedSitesMode) { siteId = RelatedSiteID; }

            Collection<Role> roles = new Collection<Role>();
            using (IDataReader reader = DBRoles.GetSiteRoles(siteId))
            {
                while (reader.Read())
                {
                    Role role = new Role();
                    role.roleID = Convert.ToInt32(reader["RoleID"]);
                    role.siteID = Convert.ToInt32(reader["SiteID"]);
                    role.displayName = reader["DisplayName"].ToString();
                    role.roleName = reader["RoleName"].ToString();
                    role.roleGuid = new Guid(reader["RoleGuid"].ToString());
                    role.siteGuid = new Guid(reader["SiteGuid"].ToString());

                    roles.Add(role);
                }
            }

            return roles;

        }

        public static Role GetRoleByName(int siteId, string roleName)
        {
            if (UseRelatedSiteMode) { siteId = RelatedSiteID; }
            Role role = null;

            using (IDataReader reader = DBRoles.GetSiteRoles(siteId))
            {
                while (reader.Read())
                {
                    string foundName = reader["RoleName"].ToString();
                    if (foundName == roleName)
                    {
                        role = new Role();
                        role.roleID = Convert.ToInt32(reader["RoleID"]);
                        role.siteID = Convert.ToInt32(reader["SiteID"]);
                        role.displayName = reader["DisplayName"].ToString();
                        role.roleName = reader["RoleName"].ToString();
                        role.roleGuid = new Guid(reader["RoleGuid"].ToString());
                        role.siteGuid = new Guid(reader["SiteGuid"].ToString());
                    }
                }
            }

            return role;

        }

        public static List<int> GetRoleIds(int siteId, string roleNamesSeparatedBySemiColons)
        {
            List<int> roleIds = new List<int>();

            List<string> roleNames = GetRolesNames(roleNamesSeparatedBySemiColons);

            foreach (string roleName in roleNames)
            {
                if (string.IsNullOrEmpty(roleName)) { continue; }
                Role r = Role.GetRoleByName(siteId, roleName);
                if (r == null)
                {
                    log.Debug("could not get roleid for role named " + roleName);
                    continue;
                }
                if (r.RoleId > -1) { roleIds.Add(r.RoleId); }
            }

            return roleIds;
        }

        public static List<string> GetRolesNames(string roleNamesSeparatedBySemiColons)
        {
            List<string> roleNames = new List<string>();
            string[] roles = roleNamesSeparatedBySemiColons.Split(';');
            foreach (string r in roles)
            {
                if (!roleNames.Contains(r)) { roleNames.Add(r); }
            }

            return roleNames;

        }


        public static int CountOfRoles(int siteId)
        {
            // TODO: implement actual select count from db
            // this is works but is not ideal
            if (UseRelatedSiteMode) { siteId = RelatedSiteID; }
            int count = 0;
            using(IDataReader reader = GetSiteRoles(siteId))
            {
                while (reader.Read())
                {
                    count += 1;
                }
                
            }

            return count;

        }

		public static bool DeleteRole(int roleId)
		{
			return DBRoles.Delete(roleId);
		}

		public static bool Exists(int siteId, String roleName)
		{
            //if (UseRelatedSiteMode) { siteId = RelatedSiteID; }
			return DBRoles.Exists(siteId, roleName);
		}

		public static IDataReader GetRoleMembers(int roleId)
		{
			return DBRoles.GetRoleMembers(roleId);
		}

        public static IDataReader GetUsersNotInRole(int siteId, int roleId, int pageNumber, int pageSize, out int totalPages)
        {
            if (UseRelatedSiteMode) { siteId = RelatedSiteID; }
            return DBRoles.GetUsersNotInRole(siteId, roleId, pageNumber, pageSize, out totalPages);
        }

        public static IDataReader GetRolesUserIsNotIn(
            int siteId,
            int userId)
        {
            if (UseRelatedSiteMode) { siteId = RelatedSiteID; }
            return DBRoles.GetRolesUserIsNotIn(siteId, userId);
        }

		public static bool AddUser(
            int roleId, 
            int userId,
            Guid roleGuid,
            Guid userGuid
            )
		{
			return DBRoles.AddUser(roleId,userId,roleGuid,userGuid);
		}

		public static bool RemoveUser(int roleId, int userId)
		{
			return DBRoles.RemoveUser(roleId,userId);
		}

		public static bool DeleteUserRoles(int userId)
		{
			return DBRoles.DeleteUserRoles(userId);
		}

        public static void AddUserToDefaultRoles(SiteUser siteUser)
        {

            Role role = new Role(siteUser.SiteId, "Authenticated Users");
            if (role.RoleId > 0)
            {
                Role.AddUser(role.RoleId, siteUser.UserId, role.RoleGuid, siteUser.UserGuid);
            }


        }



		#endregion

	}
}
