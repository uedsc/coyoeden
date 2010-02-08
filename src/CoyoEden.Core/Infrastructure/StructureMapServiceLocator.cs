using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.ServiceLocation;
using StructureMap;

namespace CoyoEden.Core.Infrastructure
{
    public class StructureMapServiceLocator : ServiceLocatorImplBase
    {
        private IContainer _container;

        public StructureMapServiceLocator() { }
        
        public StructureMapServiceLocator(IContainer container) {
            _container = container;
        }

        protected override IEnumerable<object> DoGetAllInstances(Type serviceType)
        {
            return ObjectFactory.GetAllInstances(serviceType).Cast<object>();
        }

        protected override object DoGetInstance(Type serviceType, string key)
        {
            if (string.IsNullOrEmpty(key)) return _container.GetInstance(serviceType);
            return ObjectFactory.GetNamedInstance(serviceType, key);
        }
    }

}
