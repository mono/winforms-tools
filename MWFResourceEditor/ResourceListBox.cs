// Authors:
//	Alexander Olk, <xenomorph2@onlinehome.de>

using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections;
using System.Collections.Specialized;

namespace MWFResourceEditor
{
	public class ResourceListBox : ListBox
	{
		private StringCollection resourceNames = new StringCollection();
		
		private static int newResourceCounter = 1;
		
		private SolidBrush lightBlueBrush = new SolidBrush( Color.LightBlue );
		private SolidBrush beigeBrush = new SolidBrush( Color.Beige );
		
		private ResourceType showType = ResourceType.All;
		private ListBox.ObjectCollection allItems;
		
		private ResourceType old_resource_type = ResourceType.None;
		
		private int old_selected_index = -1;
		
		private ResourceTreeView resourceTreeView;
		
		public ResourceListBox( )
		{
			Size = new Size( 500, 300 );
			DrawMode = DrawMode.OwnerDrawFixed;
			ItemHeight = 53;
			BackColor = Color.Beige ;
			
			allItems = new ListBox.ObjectCollection( this );
		}
		
		public ListBox.ObjectCollection AllItems
		{
			get {
				return allItems;
			}
		}
		
		public ResourceTreeView ResourceTreeView
		{
			set {
				resourceTreeView = value;
			}
			
			get {
				return resourceTreeView;
			}
		}
		
		public void ShowNode( ResourceType type )
		{
			BeginUpdate( );
			Create_Show_Items( type );
			Invalidate( );
			EndUpdate( );
		}
		
		private void Create_Show_Items( ResourceType type )
		{
			if ( type == old_resource_type )
				return;
			
			old_resource_type = type;
			
			showType = type;
			
			Items.Clear( );
			
			if ( type == ResourceType.All )
			{
				Items.AddRange( allItems );
				return;
			}
			
			foreach ( IResource resource in allItems )
			{
				if ( resource.ResourceType == type )
					Items.Add( resource );
			}
		}
		
		public void ShowNode( string name, ResourceType type_to_show )
		{
			IResource resource = null;
			
			foreach ( IResource iresource in allItems )
				if ( iresource.ResourceName == name )
				{
					resource = iresource;
					break;
				}
			
			if ( resource == null )
				return;
			
			if ( old_resource_type != type_to_show )
			{
				BeginUpdate( );
				Create_Show_Items( type_to_show );
				int index = Items.IndexOf( resource );
				SelectedIndex = index;
				TopIndex = index;
				EndUpdate( );
			}
			else
			{
				BeginUpdate( );
				int index = Items.IndexOf( resource );
				SelectedIndex = index;
				TopIndex = index;
				EndUpdate( );
			}
		}
		
		protected override void OnDrawItem( DrawItemEventArgs e )
		{
			if ( e.Index != -1 )
			{
				IResourceRenderer renderer = (IResourceRenderer)Items[ e.Index ];
				
				if ( ( e.State & DrawItemState.Selected ) == DrawItemState.Selected )
					e.Graphics.FillRectangle( lightBlueBrush, e.Bounds );
				else
					e.Graphics.FillRectangle( beigeBrush, e.Bounds );
				
				e.Graphics.DrawImage( renderer.RenderContent, e.Bounds.X + 3, e.Bounds.Y + 1 );
				
				e.Graphics.DrawLine( Pens.Blue, e.Bounds.X, e.Bounds.Y - 1, e.Bounds.Width - 1 , e.Bounds.Y - 1 );
			}
		}
		
		public void AddResource( string resource_name, object resource )
		{
			if ( resourceNames.Contains( resource_name ) )
				return;
			
			resourceNames.Add( resource_name );
			
			DoAddResource( resource, resource_name );
		}
		
		private void DoAddResource( object resource, string resource_name )
		{
			if ( resource is Bitmap )
			{
				ResourceImage resImage = new ResourceImage( resource_name, (Image)resource );
				
				allItems.Add( resImage );
			}
			else
			if ( resource is String )
			{
				ResourceString resString = new ResourceString( resource_name, resource.ToString( ) );
				
				allItems.Add( resString );
			}
			else
			if ( resource is Icon )
			{
				ResourceIcon resIcon = new ResourceIcon( resource_name, (Icon)resource );
				
				allItems.Add( resIcon );
			}
			else
			if ( resource is Color )
			{
				ResourceColor resColor = new ResourceColor( resource_name, (Color)resource );
				
				allItems.Add( resColor );
			}
			else
			if ( resource is Cursor )
			{
				ResourceCursor resCursor = new ResourceCursor( resource_name, (Cursor)resource );
				
				allItems.Add( resCursor );
			}
		}
		
		public void AddResourceDirect( IResource resource )
		{
			if ( resourceNames.Contains( resource.ResourceName ) )
				return;
			
			resourceNames.Add( resource.ResourceName );
			
			BeginUpdate( );
			
			allItems.Add( resource );
			
			if ( resource.ResourceType == showType || showType == ResourceType.All )
				Items.Add( resource );
			
			EndUpdate( );
		}
		
		public void InsertResourceDirect( int index, IResource resource )
		{
			if ( resourceNames.Contains( resource.ResourceName ) )
				return;
			
			resourceNames.Add( resource.ResourceName );
			
			BeginUpdate( );
			
			if ( resource.ResourceType == showType || showType == ResourceType.All )
				Items.Insert( index, resource ); 
			
			// FIXME
			allItems.Insert( 0, resource );
			
			EndUpdate( );
		}
		
		public void RemoveResource( IResource resource )
		{
			resourceNames.Remove( resource.ResourceName );
			
			BeginUpdate( );
			
			Items.Remove( resource );
			
			allItems.Remove( resource );
			
			EndUpdate( );
		}
		
		public void RenameResource( IResource resource, string new_name )
		{
			int index = Items.IndexOf( resource );
			
			if ( index != -1 )
			{
				BeginUpdate( );
				
				resourceNames.Remove( resource.ResourceName );
				
				Items.Remove( resource );
				
				int index_allItems = allItems.IndexOf( resource );
				
				allItems.Remove( resource );
				
				resource.ResourceName = new_name;
				
				resourceNames.Add( new_name );
				
				Items.Insert( index, resource );
				
				allItems.Insert( index_allItems, resource );
				
				EndUpdate( );
			}
		}
		
		public void ReplaceResource( IResource old_resource, IResource new_resource )
		{
			int index = Items.IndexOf( old_resource );
			int allItems_index = allItems.IndexOf( old_resource );
			
			BeginUpdate( );
			
			Items.Remove( old_resource );
			
			Items.Insert( index, new_resource );
			
			allItems.Remove( old_resource );
			
			allItems.Insert( allItems_index, new_resource );
			
			EndUpdate( );
		}
		
		public void ClearResources( )
		{
			BeginUpdate( );
			
			resourceNames.Clear( );
			Items.Clear( );
			allItems.Clear( );
			
			EndUpdate( );
		}
		
		public string UniqueResourceName( string pre )
		{
			string new_name = pre + "_" + newResourceCounter;
			
			while ( resourceNames.Contains( new_name ) )
			{
				newResourceCounter++;
				new_name = pre + "_" + newResourceCounter;
			}
			
			return new_name;
		}
		
		protected override void OnClick( EventArgs e )
		{
			if ( SelectedIndex != old_selected_index )
			{
				old_selected_index = SelectedIndex;
				resourceTreeView.ShowItem( Items[ SelectedIndex ] as IResource, showType );
			}
			base.OnClick( e );
		}
	}
}


