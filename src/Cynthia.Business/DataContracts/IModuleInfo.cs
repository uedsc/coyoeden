using System;

namespace Cynthia.Business.DataContracts
{
	public interface IModuleInfo
	{
		Guid FeatureGuid { get; set; }
		string ResourceFile { get; set; }
		string SearchListName { get; set; }
	}
}
