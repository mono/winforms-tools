// Authors:
//	Alexander Olk, <xenomorph2@onlinehome.de>

using System;
using System.Drawing;
using System.Windows.Forms;

namespace MWFResourceEditor
{
	public class ResourceTreeView : TreeView
	{
		private ResourceListBox resourceListBox;
		
		ResourceTreeNode all = new ResourceTreeNode( "All", ResourceType.None, ResourceType.All );
		ResourceTreeNode image = new ResourceTreeNode( "Images", ResourceType.None, ResourceType.TypeImage );
		ResourceTreeNode tstring = new ResourceTreeNode( "Strings", ResourceType.None, ResourceType.TypeString );
		ResourceTreeNode icon = new ResourceTreeNode( "Icons", ResourceType.None, ResourceType.TypeIcon );
		ResourceTreeNode color = new ResourceTreeNode( "Colors", ResourceType.None, ResourceType.TypeColor );
		ResourceTreeNode cursor = new ResourceTreeNode( "Cursors", ResourceType.None, ResourceType.TypeCursor );
		ResourceTreeNode bytearray = new ResourceTreeNode( "Byte Arrays", ResourceType.None, ResourceType.TypeByteArray );
		
		public ResourceTreeView( ResourceListBox resourceListBox )
		{
			this.resourceListBox = resourceListBox;
			
			BorderStyle = BorderStyle.Fixed3D;
			ShowLines = true;			
			
			ItemHeight = 21;
			
			Nodes.Add( all );			
			Nodes.Add( image );
			Nodes.Add( tstring );
			Nodes.Add( icon );
			Nodes.Add( color );
			Nodes.Add( cursor );
			Nodes.Add( bytearray );
		}
		
		public void ShowItem( IResource iResource, ResourceType showType )
		{
			ResourceTreeNode to_select = null;
			
			switch ( showType )
			{
				case ResourceType.All:
					to_select = GetNodeIndex( iResource.ResourceName, all );
					break;
				case ResourceType.TypeImage:
					to_select = GetNodeIndex( iResource.ResourceName, image );
					break;
				case ResourceType.TypeString:
					to_select = GetNodeIndex( iResource.ResourceName, tstring );
					break;
				case ResourceType.TypeIcon:
					to_select = GetNodeIndex( iResource.ResourceName, icon );
					break;
				case ResourceType.TypeColor:
					to_select = GetNodeIndex( iResource.ResourceName, color );
					break;
				case ResourceType.TypeCursor:
					to_select = GetNodeIndex( iResource.ResourceName, cursor );
					break;
				case ResourceType.TypeByteArray:
					to_select = GetNodeIndex( iResource.ResourceName, bytearray );
					break;
				default:
					break;
			}
			
			if ( to_select != null )
			{
				BeginUpdate( );
				this.CollapseAll( );
				to_select.EnsureVisible( );
				SelectedNode = to_select;
				EndUpdate( );
			}
		}
		
		private ResourceTreeNode GetNodeIndex( string name, ResourceTreeNode node )
		{
			ResourceTreeNode tnode = null;
			
			for ( int i = 0; i < node.Nodes.Count; i++ )
			{
				if ( name == node.Nodes[ i ].Text )
				{
					tnode = node.Nodes[ i ] as ResourceTreeNode;
					break;
				}
			}
			
			return tnode;
		}
		
		public void Fill( )
		{
			Clear( );
			
			BeginUpdate( );
			
			foreach ( IResource resource in resourceListBox.AllItems )
			{
				switch ( resource.ResourceType )
				{
					case ResourceType.TypeImage:
						image.Nodes.Add( new ResourceTreeNode( resource.ResourceName, ResourceType.TypeImage, ResourceType.None ) );
						all.Nodes.Add( new ResourceTreeNode( resource.ResourceName, ResourceType.TypeImage, ResourceType.None ) );
						break;
					case ResourceType.TypeByteArray:
						bytearray.Nodes.Add( new ResourceTreeNode( resource.ResourceName, ResourceType.TypeByteArray, ResourceType.None ) );
						all.Nodes.Add( new ResourceTreeNode( resource.ResourceName, ResourceType.TypeByteArray, ResourceType.None ) );
						break;
					case ResourceType.TypeString:
						tstring.Nodes.Add( new ResourceTreeNode( resource.ResourceName, ResourceType.TypeString, ResourceType.None ) );
						all.Nodes.Add( new ResourceTreeNode( resource.ResourceName, ResourceType.TypeString, ResourceType.None ) );
						break;
					case ResourceType.TypeColor:
						color.Nodes.Add( new ResourceTreeNode( resource.ResourceName, ResourceType.TypeColor, ResourceType.None ) );
						all.Nodes.Add( new ResourceTreeNode( resource.ResourceName, ResourceType.TypeColor, ResourceType.None ) );
						break;
					case ResourceType.TypeCursor:
						cursor.Nodes.Add( new ResourceTreeNode( resource.ResourceName, ResourceType.TypeCursor, ResourceType.None ) );
						all.Nodes.Add( new ResourceTreeNode( resource.ResourceName, ResourceType.TypeCursor, ResourceType.None ) );
						break;
					case ResourceType.TypeIcon:
						icon.Nodes.Add( new ResourceTreeNode( resource.ResourceName, ResourceType.TypeIcon, ResourceType.None ) );
						all.Nodes.Add( new ResourceTreeNode( resource.ResourceName, ResourceType.TypeIcon, ResourceType.None ) );
						break;
					default:
						break;
				}
			}
			
			EndUpdate( );
		}
		
		public void Clear( )
		{
			BeginUpdate( );
			all.Nodes.Clear( );
			image.Nodes.Clear( );
			tstring.Nodes.Clear( );
			icon.Nodes.Clear( );
			color.Nodes.Clear( );
			cursor.Nodes.Clear( );
			bytearray.Nodes.Clear( );
			EndUpdate( );
		}
		
		protected override void OnClick( EventArgs e )
		{
			ResourceTreeNode selected = SelectedNode as ResourceTreeNode;
			
			if ( selected != null )
			{
				if ( selected.CommandType != ResourceType.None )
					resourceListBox.ShowNode( selected.CommandType );
				else
					resourceListBox.ShowNode( selected.Text, selected.ResourceType );
			}
			
			base.OnClick( e );
		}
	}
}

