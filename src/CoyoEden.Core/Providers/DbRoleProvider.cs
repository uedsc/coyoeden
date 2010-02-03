using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Configuration.Provider;
using System.Data;
using System.Data.Common;
using System.Web.Security;
using System.Linq;
using SystemX.Infrastructure;

namespace CoyoEden.Core.Providers
{
    /// <summary>
    /// Generic Db Role Provider
    /// </summary>
    public class DbRoleProvider : RoleProvider
    {
        private string connStringName;
        private string tablePrefix;
        private string parmPrefix;
        private string applicationName;

        /// <summary>
        /// Initializes the provider
        /// </summary>
        /// <param name="name">Configuration name</param>
        /// <param name="config">Configuration settings</param>
        public override void Initialize(string name, NameValueCollection config)
        {
            if (config == null)
            {
                throw new ArgumentNullException("config");
            }

            if (String.IsNullOrEmpty(name))
            {
                name = "DbMembershipProvider";
            }

            if (String.IsNullOrEmpty(config["description"]))
            {
                config.Remove("description");
                config.Add("description", "Generic Database Membership Provider");
            }

            base.Initialize(name, config);

            if (config["connectionStringName"] == null)
            {
                // default to CoyoEden
                config["connectionStringName"] = "CoyoEden";
            }
            connStringName = config["connectionStringName"];
            config.Remove("connectionStringName");

            if (config["tablePrefix"] == null)
            {
                // default
                config["tablePrefix"] = "cy_";
            }
            tablePrefix = config["tablePrefix"];
            config.Remove("tablePrefix");

            if (config["parmPrefix"] == null)
            {
                // default
                config["parmPrefix"] = "@";
            }
            parmPrefix = config["parmPrefix"];
            config.Remove("parmPrefix");

            if (config["applicationName"] == null)
            {
                // default to CoyoEden
                config["applicationName"] = "CoyoEden";
            }
            applicationName = config["applicationName"];
            config.Remove("applicationName");

            // Throw an exception if unrecognized attributes remain
            if (config.Count > 0)
            {
                string attr = config.GetKey(0);
                if (!String.IsNullOrEmpty(attr))
					throw new ProviderException(String.Format("Unrecognized attribute: {0}", attr));
            }
        }

        /// <summary>
        /// Check to see if user is in a role
        /// </summary>
        /// <param name="username"></param>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public override bool IsUserInRole(string username, string roleName)
        {
            bool roleFound = false;
			var boUser = User.GetUser(username);
			if (boUser != null) {
				var userRole = boUser.UserRoles.SingleOrDefault(x=>x.Role.Name==roleName);
				if (userRole != null) {
					roleFound = true;
				}
			};
			return roleFound;
        }

        /// <summary>
        /// Return an array of roles that user is in
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public override string[] GetRolesForUser(string username)
        {
            List<string> roles = new List<string>();
			var boUser = User.GetUser(username);
			if (boUser!=null)
			{
				boUser.UserRoles.ForEach(x => {
					roles.Add(x.Role.Name);
				});
			}

            return roles.ToArray();
        }

        /// <summary>
        /// Adds a new role to the database
        /// </summary>
        /// <param name="roleName"></param>
        public override void CreateRole(string roleName)
        {
			var boRole = new Role(roleName);
			boRole.Save();    
		}

        /// <summary>
        /// Removes a role from database
        /// </summary>
        /// <param name="roleName"></param>
        /// <param name="throwOnPopulatedRole"></param>
        /// <returns></returns>
        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            bool success = false;
			var msg = new BOMessager();
            if (roleName != "Administrators")
            {
				var boRole = Role.GetRole(roleName);
				if (boRole != null) {
					try
					{
						boRole.MarkForDelete();
						boRole.Delete();
						success = true;
					}
					catch (Exception ex) {
						msg.Error<DbRoleProvider>(ex, true);
					}
				};
            }

            return success;
        }

        /// <summary>
        /// Checks to see if role exists
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public override bool RoleExists(string roleName)
        {
            bool roleFound = false;
			var boRole = Role.GetRole(roleName);
			roleFound = boRole != null;
            return roleFound;
        }

        /// <summary>
        /// Adds all users in user array to all roles in role array
        /// </summary>
        /// <param name="usernames"></param>
        /// <param name="roleNames"></param>
        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
			var boUser = default(User);
			var boRole=default(Role);
			var boUserRole = default(UserRole);
			usernames.ToList().ForEach(x => {
				boUser = User.GetUser(x);
				if (boUser != null) {
					roleNames.ToList().ForEach(r => {
						boRole = Role.GetRole(r);
						if (boRole != null) {
							boUserRole = boUser.UserRoles.SingleOrDefault(ur=>ur.Role==boRole);
							if (boUserRole == null) {
								boUserRole = new UserRole { 
									Id=GuidExt.NewGuid(GuidExt.GuidStrategy.OrderedSequential),
									User=boUser,
									Role=boRole
								};
								boUserRole.Save();
							};
						};
					});
				}
			});
           
        }

        /// <summary>
        /// Removes all users in user array from all roles in role array
        /// </summary>
        /// <param name="usernames"></param>
        /// <param name="roleNames"></param>
        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
			var boUser = default(User);
			var boRole = default(Role);
			var boUserRole = default(UserRole);
			usernames.ToList().ForEach(x =>
			{
				boUser = User.GetUser(x);
				if (boUser != null)
				{
					roleNames.ToList().ForEach(r =>
					{
						boRole = Role.GetRole(r);
						if (boRole != null)
						{
							boUserRole = boUser.UserRoles.SingleOrDefault(ur => ur.Role == boRole);
							if (boUserRole != null)
							{
								boUser.UserRoles.MarkForDelete(boUserRole);
								boUser.UserRoles.SaveAll();
							};
						};
					});
				}
			});
        }

        /// <summary>
        /// Returns array of users in selected role
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public override string[] GetUsersInRole(string roleName)
        {
            List<string> users = new List<string>();
			var boRole = Role.GetRole(roleName);
			if (boRole != null) {
				boRole.UserRoles.ForEach(x => {
					users.Add(x.User.UserName);
				});
			};
            return users.ToArray();
        }

        /// <summary>
        /// Returns array of all roles in database
        /// </summary>
        /// <returns></returns>
        public override string[] GetAllRoles()
        {
            List<string> roles = new List<string>();
			if (Role.Roles != null && Role.Roles.Count > 0) {
				Role.Roles.ForEach(x => {
					roles.Add(x.Name);
				});
			};
            return roles.ToArray();
        }

        /// <summary>
        /// Returns all users in selected role with names that match usernameToMatch
        /// </summary>
        /// <param name="roleName"></param>
        /// <param name="usernameToMatch"></param>
        /// <returns></returns>
        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            List<string> users = new List<string>();
			var tempUsers = GetUsersInRole(roleName).ToList();

			tempUsers.ForEach(x => {
				if (x.IndexOf(usernameToMatch) >= 0) {
					users.Add(x);
				};
			});

            return users.ToArray();
        }

        /// <summary>
        /// Returns the application name as set in the web.config
        /// otherwise returns CoyoEden.  Set will throw an error.
        /// </summary>
        public override string ApplicationName
        {
            get { return applicationName; }
            set { throw new NotImplementedException(); }
        }
    }
}
