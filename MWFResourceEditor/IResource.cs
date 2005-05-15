// Authors:
//	Alexander Olk, <xenomorph2@onlinehome.de>

using System;

namespace MWFResourceEditor
{
	public interface IResource : ICloneable
	{
		string ResourceName
		{ get; set; }
		
		ResourceType ResourceType
		{ get; }
		
		string ContentString( );
	}
}

