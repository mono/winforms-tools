// Authors:
//	Alexander Olk, <xenomorph2@onlinehome.de>

using System;

namespace MWFResourceEditor
{
	public interface IResource
	{
		string ResourceName
		{ get; set; }
		
		object Value
		{ get; }
		
		ResourceType ResourceType
		{ get; }
		
		string ContentString( );
	}
}

