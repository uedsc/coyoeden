using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoyoEden.Core.DataContracts;

namespace CoyoEden.Core.Infrastructure
{
	/// <summary>
	/// Public interfaces and enums for DataStore
	/// ISettingsBehavior incapsulates saving and retreaving
	/// settings objects to and from data storage
	/// </summary>
	public interface ISettingsBehavior
	{
		/// <summary>
		/// Save settings interface
		/// </summary>
		/// <param name="exType">Extensio Type</param>
		/// <param name="exId">Extensio Id</param>
		/// <param name="settings">Settings object</param>
		/// <returns>True if saved</returns>
		bool SaveSettings<T>(SettingTypes exType, string name, T settings);

		/// <summary>
		/// Get settings interface
		/// </summary>
		/// <param name="exType">Extension Type</param>
		/// <param name="exId">Extension Id</param>
		/// <returns>Settings object</returns>
		T GetSettings<T>(SettingTypes exType, string name) where T:class;
	}
}
