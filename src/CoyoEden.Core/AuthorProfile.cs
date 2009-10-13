#region Using

using System;
using System.Collections.Generic;
using CoyoEden.Core.Providers;
using System.Linq;
using Vivasky.Core;
using Habanero.BO;

#endregion

namespace CoyoEden.Core
{
	public class AuthorProfile
	{

		#region Constructors

		public AuthorProfile()
		{

		}

		public AuthorProfile(string username)
		{
			UserName = username;
		}

		#endregion

		#region Properties
		public BusinessObjectCollection<Profile> CurrentSettings { get; set; }
		private static object _SyncRoot = new object();
		private static List<AuthorProfile> _Profiles;
		/// <summary>
		/// Gets an unsorted list of all pages.
		/// </summary>
		public static List<AuthorProfile> Profiles
		{
			get
			{
				if (_Profiles == null)
				{
					lock (_SyncRoot)
					{
						if (_Profiles == null)
						{
							_Profiles = LoadAll();
						}
					}
				}

				return _Profiles;
			}
		}
		public Guid Id { get; set; }
		public string UserName
		{
			get;
			set;
		}


		public string FullName
		{
			get { return (String.Format("{0} {1} {2}", FirstName, MiddleName, LastName)).Replace("  ", " "); }
		}

		public bool IsPrivate
		{
			get;
			set;
		}

		public string FirstName
		{
			get;
			set;
		}
		public string MiddleName
		{
			get;
			set;
		}

		public string LastName
		{
			get;
			set;
		}

		public string DisplayName
		{
			get;
			set;
		}
		public string PhotoURL
		{
			get;
			set;
		}


		public DateTime Birthday
		{
			get;
			set;
		}

		public string CityTown
		{
			get;
			set;
		}

		public string RegionState
		{
			get;
			set;
		}

		public string Country
		{
			get;
			set;
		}

		public string PhoneMain
		{
			get;
			set;
		}

		public string PhoneFax
		{
			get;
			set;
		}
		public string PhoneMobile
		{
			get;
			set;
		}
		public string EmailAddress
		{
			get;
			set;
		}

		public string Company
		{
			get;
			set;
		}
		public string AboutMe
		{
			get;
			set;
		}

		public string RelativeLink
		{
			get { return String.Format("{0}author/{1}.aspx", Utils.RelativeWebRoot, UserName); ; }
		}


		#endregion

		#region Methods

		public static AuthorProfile GetProfile(string username)
		{
			var profile = new AuthorProfile(username);
			var settings = Profile.LoadAll(username);
			loadFromSettings(ref profile, settings);

			return profile;
		}

		public override string ToString()
		{
			return FullName;
		}
		public AuthorProfile MarkForDelete() {
			this.CurrentSettings.ForEach(x => {
				x.MarkForDelete();
			});
			return this;
		}
		public void Save() {
			this.CurrentSettings.ForEach(x =>
			{
				x.SettingValue = this.GetPropertyValue(x.SettingName).ToString();
			});
			this.CurrentSettings.SaveAll();
		}
		/// <summary>
		/// Return collection for AuthorProfiles from database
		/// </summary>
		/// <returns></returns>
		public static List<AuthorProfile> LoadAll()
		{
			List<AuthorProfile> profiles = new List<AuthorProfile>();

			var allItems = Profile.LoadAll();
			var gProfiles = from item in allItems
							group item by item.SettingName into g
							select new { Key = g.Key, Items = g };
			var tempProfile = default(AuthorProfile);
			foreach (var p in gProfiles)
			{
				tempProfile = new AuthorProfile(p.Key);
				loadFromSettings(ref tempProfile, p.Items);
				profiles.Add(tempProfile);
			}

			return profiles;
		}
		#endregion

		#region helper methods
		static void loadFromSettings(ref AuthorProfile profile, IEnumerable<Profile> settings)
		{
			var tempBOList = new BusinessObjectCollection<Profile>();
			tempBOList.AddRange(settings);
			profile.CurrentSettings = tempBOList;
			foreach (var x in settings)
			{
				profile.SetPropertyValue(x.SettingName, x.SettingValue, true);
			}

		}
		#endregion
	}
}