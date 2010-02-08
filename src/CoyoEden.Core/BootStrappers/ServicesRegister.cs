using System;
using StructureMap;
using StructureMap.Configuration.DSL;
using System.Collections.Specialized;
using System.Configuration;
using Microsoft.Practices.ServiceLocation;
using CoyoEden.Core.Infrastructure;

namespace CoyoEden.Core.BootStrappers
{
	public class ServicesRegister
	{
		public static void Boot() {
			//register services to the container
			var container = ServicesRegister.AddComponentsTo(ObjectFactory.Container);
			//register to common service locator
			var serviceLocator = new StructureMapServiceLocator(container);
			ServiceLocator.SetLocatorProvider(() => serviceLocator);
		}
		/// <summary>
		/// Add Components to StructureMap container.
		/// </summary>
		/// <param name="container"></param>
		/// <returns></returns>
		static IContainer AddComponentsTo(IContainer container)
		{
			container.Configure(
					x =>
					{
						x.AddRegistry(new InnerRegistry());
					}
				);
			return container;
		}

		private class InnerRegistry : Registry {
			public InnerRegistry() { 
				//appsettings
				ForSingletonOf<NameValueCollection>().Use(ConfigurationManager.AppSettings);
				//setting service
				ForSingletonOf<ISettingsBehavior>().Use(new Setting());
			}
		}
	}
}
