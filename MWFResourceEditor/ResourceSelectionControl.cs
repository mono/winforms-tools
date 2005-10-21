using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections;
using System.Resources;

namespace MWFResourceEditor
{
	public class ResourceSelectionControl : Control
	{
		private ResourceListBox resourceListBox;
		private Splitter resourceSplitter;
		private ResourceTreeView resourceTreeView;
		
		private ResourceList resourceList = ResourceList.Instance;
		
		public ResourceSelectionControl( ResourceControl resourceControl )
		{
			resourceSplitter = new Splitter( );
			
			resourceListBox = new ResourceListBox( );
			resourceTreeView = new ResourceTreeView( resourceListBox );
			resourceListBox.ResourceTreeView = resourceTreeView;
			
			SuspendLayout( );
			
			// resourceListBox
			resourceListBox.Size = new Size( 592, 328 );
			resourceListBox.Dock = DockStyle.Fill;
			resourceListBox.TabIndex = 0;
			resourceListBox.ContextMenu = resourceControl.InternalContextMenu;
			
			// resourceTreeView
			resourceTreeView.Dock = DockStyle.Left;
			resourceTreeView.Size = new Size( 150, 328 );
			
			// resourceSplitter
			resourceSplitter.Dock = DockStyle.Left;
			resourceSplitter.MinExtra = 150;
			resourceSplitter.MinSize = 150;
			
			Controls.Add( resourceListBox );
			Controls.Add( resourceSplitter );
			Controls.Add( resourceTreeView );
			
			ResumeLayout( false );
		}
		
		public ResourceListBox ResourceListBox
		{
			get {
				return resourceListBox;
			}
		}
		
		public IResource SelectedResourceListBoxItem
		{
			get {
				if ( resourceListBox.SelectedItems.Count == 0 )
					return null;
				
				return resourceListBox.SelectedItems[ 0 ] as IResource;
			}
		}
		
		public int LoadResource( ResXResourceReader resXResourceReader )
		{
			int number_resources = 0;
			
			ClearResources( );
			
			foreach ( DictionaryEntry de in resXResourceReader )
				number_resources += resourceList.AddResource( de.Key.ToString( ), de.Value );
			
			resourceTreeView.FillNodes( );
			resourceListBox.ShowNode( ResourceType.TypeImage );
			
			if ( resourceListBox.Items.Count > 0 )
				resourceListBox.SelectedIndex = 0;
			
			return number_resources;
		}
		
		public void EnsureVisible( )
		{
			if ( resourceListBox.Items.Count > 0 )
			{
				resourceListBox.SelectedIndex = 0;
				
				resourceListBox.TopIndex = 0;
			}
		}
		
		public void AddString( IResource resource )
		{
			if ( resourceListBox.Items.Count == 0 )
				AddResourceDirect( resource );
			else
				InsertResourceDirect( 0, resource );
			
			resourceListBox.SelectedIndex = 0;
			
			resourceListBox.TopIndex = 0;
		}
		
		public void AddResourceDirect( IResource resource )
		{
			if ( resourceList.ContainsName( resource.ResourceName ) )
				return;
			
			resourceList.AddResourceDirect( resource );
			
			resourceListBox.AddResourceDirect( resource );
			
			resourceTreeView.AddResourceDirect( resource );
		}
		
		public void InsertResourceDirect( int index, IResource resource )
		{
			if ( resourceList.ContainsName( resource.ResourceName ) )
				return;
			
			resourceList.InsertResourceDirect( index, resource );
			
			resourceListBox.InsertResourceDirect( index, resource );
			
			resourceTreeView.AddResourceDirect( resource );
		}
		
		public void RemoveResource( )
		{
			IResource resource = SelectedResourceListBoxItem;
			
			if ( resource == null )
				return;
			
			resourceList.RemoveResource( resource );
			
			resourceListBox.RemoveResource( resource );
			
			resourceTreeView.RemoveResource( resource );
		}
		
		public void RenameResource( IResource resource, string new_name )
		{
			resourceList.RenameResource( resource, new_name );
			
			resourceListBox.RenameResource( resource, new_name );
			
			// TODO: resourceTreeView.Rename
		}
		
		public void ReplaceResource( IResource old_resource, IResource new_resource )
		{
			resourceList.ReplaceResource( old_resource, new_resource );
			
			resourceListBox.ReplaceResource( old_resource, new_resource );
			
			resourceTreeView.Focus( );
		}
		
		public void ClearResources( )
		{
			resourceList.ClearResources( );
			
			resourceListBox.ClearResources( );
			resourceTreeView.ClearResources( );
		}
	}
}

