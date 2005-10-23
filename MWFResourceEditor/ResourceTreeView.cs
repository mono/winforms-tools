// Authors:
//	Alexander Olk, <xenomorph2@onlinehome.de>

using System;
using System.Drawing;
using System.Windows.Forms;

// TODO replace resource

namespace MWFResourceEditor
{
	public class ResourceTreeView : TreeView
	{
		private ResourceListBox resourceListBox;
		
		ResourceTreeNode image = new ResourceTreeNode( "Images", ResourceType.None, ResourceType.TypeImage );
		ResourceTreeNode tstring = new ResourceTreeNode( "Strings", ResourceType.None, ResourceType.TypeString );
		ResourceTreeNode icon = new ResourceTreeNode( "Icons", ResourceType.None, ResourceType.TypeIcon );
		ResourceTreeNode color = new ResourceTreeNode( "Colors", ResourceType.None, ResourceType.TypeColor );
		ResourceTreeNode cursor = new ResourceTreeNode( "Cursors", ResourceType.None, ResourceType.TypeCursor );
		ResourceTreeNode bytearray = new ResourceTreeNode( "Byte Arrays", ResourceType.None, ResourceType.TypeByteArray );
		
		private ResourceList resourceList = ResourceList.Instance;
		
		public ResourceTreeView(ResourceListBox resourceListBox)
		{
			this.resourceListBox = resourceListBox;
			
			BorderStyle = BorderStyle.Fixed3D;
			ShowLines = true;
			
			ItemHeight = 21;
			
			Nodes.Add(image);
			Nodes.Add(tstring);
			Nodes.Add(icon);
			Nodes.Add(color);
			Nodes.Add(cursor);
			Nodes.Add(bytearray);
		}
		
		public void ShowItem(IResource iResource, ResourceType showType)
		{
			ResourceTreeNode to_select = null;
			
			switch (showType)
			{
				case ResourceType.TypeImage:
					to_select = GetNode(iResource.ResourceName, image);
					break;
				case ResourceType.TypeString:
					to_select = GetNode(iResource.ResourceName, tstring);
					break;
				case ResourceType.TypeIcon:
					to_select = GetNode(iResource.ResourceName, icon);
					break;
				case ResourceType.TypeColor:
					to_select = GetNode(iResource.ResourceName, color);
					break;
				case ResourceType.TypeCursor:
					to_select = GetNode(iResource.ResourceName, cursor);
					break;
				case ResourceType.TypeByteArray:
					to_select = GetNode(iResource.ResourceName, bytearray);
					break;
				default:
					break;
			}
			
			if (to_select != null)
			{
				BeginUpdate();
				CollapseAll();
				to_select.EnsureVisible();
				SelectedNode = to_select;
				EndUpdate();
			}
		}
		
		private ResourceTreeNode GetNode(string name, ResourceTreeNode node)
		{
			foreach (ResourceTreeNode rnode in node.Nodes)
				if (name == rnode.Text)
					return rnode;
			
			return null;
		}
		
		private void AddToNode(IResource resource)
		{
			switch (resource.ResourceType)
			{
				case ResourceType.TypeImage:
					image.Nodes.Add(new ResourceTreeNode(resource.ResourceName, ResourceType.TypeImage, ResourceType.None));
					break;
				case ResourceType.TypeByteArray:
					bytearray.Nodes.Add(new ResourceTreeNode(resource.ResourceName, ResourceType.TypeByteArray, ResourceType.None));
					break;
				case ResourceType.TypeString:
					tstring.Nodes.Add(new ResourceTreeNode(resource.ResourceName, ResourceType.TypeString, ResourceType.None));
					break;
				case ResourceType.TypeColor:
					color.Nodes.Add(new ResourceTreeNode(resource.ResourceName, ResourceType.TypeColor, ResourceType.None));
					break;
				case ResourceType.TypeCursor:
					cursor.Nodes.Add(new ResourceTreeNode(resource.ResourceName, ResourceType.TypeCursor, ResourceType.None));
					break;
				case ResourceType.TypeIcon:
					icon.Nodes.Add(new ResourceTreeNode(resource.ResourceName, ResourceType.TypeIcon, ResourceType.None));
					break;
				default:
					break;
			}
		}
		
		public void FillNodes()
		{
			foreach (IResource resource in resourceList.Items)
				AddToNode(resource);
		}
		
		public void ClearResources()
		{
			BeginUpdate();
			image.Nodes.Clear();
			tstring.Nodes.Clear();
			icon.Nodes.Clear();
			color.Nodes.Clear();
			cursor.Nodes.Clear();
			bytearray.Nodes.Clear();
			EndUpdate();
		}
		
		public void AddResourceDirect(IResource resource)
		{
			BeginUpdate();
			AddToNode(resource);
			EndUpdate();
		}
		
		public void RemoveResource(IResource resource)
		{
			BeginUpdate();
			foreach (ResourceTreeNode pnode in Nodes)
			{
				ResourceTreeNode to_delete = GetNode(resource.ResourceName, pnode);
				
				if (to_delete != null)
				{
					pnode.Nodes.Remove(to_delete);
					break;
				}
			}
			EndUpdate();
		}
		
		protected override void OnAfterSelect(TreeViewEventArgs e)
		{
			ResourceTreeNode selected = e.Node as ResourceTreeNode;
			
			if (selected != null)
			{
				if (selected.CommandType != ResourceType.None)
				{
					if ( Focused )
						resourceListBox.ShowNode(selected.CommandType);
					selected.Expand();
				}
				else
				{
					if ( Focused )
						resourceListBox.ShowNode(selected.Text, selected.ResourceType);
				}
			}
			
			base.OnAfterSelect(e);
		}
	}
}

