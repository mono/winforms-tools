// Authors:
//	Alexander Olk, <xenomorph2@onlinehome.de>

using System;
using System.Windows.Forms;

namespace MWFResourceEditor
{
	public class ResourceTreeNode : TreeNode
	{
		private ResourceType resourceType;
		private ResourceType commandType;
		
		public ResourceTreeNode( string text, ResourceType resourceType, ResourceType commandType )
		{
			Text = text;
			this.resourceType = resourceType;
			this.commandType = commandType;
		}
		
		public ResourceType ResourceType
		{
			get {
				return resourceType;
			}
		}
		
		public ResourceType CommandType
		{
			get {
				return commandType;
			}
		}
	}
}

