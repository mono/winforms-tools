// Authors:
//	Alexander Olk, <xenomorph2@onlinehome.de>

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Collections;
using System.Collections.Specialized;

namespace MWFResourceEditor
{
	public class ResourceListBox : ListBox
	{
		private SolidBrush evenBrush = new SolidBrush( Color.White );
		private SolidBrush oddBrush = new SolidBrush( Color.FromArgb( 233, 233, 233 ) );
		
		private ResourceType showType = ResourceType.TypeImage;
		
		private ResourceType old_resource_type = ResourceType.None;
		
		private int old_selected_index = -1;
		
		private ResourceTreeView resourceTreeView;
		
		private ResourceList resourceList = ResourceList.Instance;
		
		public ResourceListBox( )
		{
			Size = new Size( 500, 300 );
			DrawMode = DrawMode.OwnerDrawFixed;
			ItemHeight = 53;
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
			EndUpdate( );
		}
		
		private void Create_Show_Items( ResourceType type )
		{
			if ( type == old_resource_type )
				return;
			
			old_resource_type = type;
			
			showType = type;
			
			Items.Clear( );
			
			Items.AddRange( resourceList.ItemsAsArray( type ) );
		}
		
		public void ShowNode( string name, ResourceType type_to_show )
		{
			IResource resource = resourceList.GetResourceByName( name );
			
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
		
		public void AddResourceDirect( IResource resource )
		{
			BeginUpdate( );
			
			if ( resource.ResourceType == showType )
				Items.Add( resource );
			
			EndUpdate( );
		}
		
		public void InsertResourceDirect( int index, IResource resource )
		{
			BeginUpdate( );
			
			if ( resource.ResourceType == showType )
				Items.Insert( index, resource );
			
			EndUpdate( );
		}
		
		public void RemoveResource( IResource resource )
		{
			BeginUpdate( );
			
			if ( resource.ResourceType == showType )
				Items.Remove( resource );
			
			EndUpdate( );
		}
		
		public void RenameResource( IResource resource, string new_name )
		{
			int index = Items.IndexOf( resource );
			
			if ( index != -1 )
			{
				BeginUpdate( );
				
				Items.Remove( resource );
				
				resource.ResourceName = new_name;
				
				Items.Insert( index, resource );
				
				EndUpdate( );
			}
		}
		
		public void ReplaceResource( IResource old_resource, IResource new_resource )
		{
			int index = Items.IndexOf( old_resource );
			
			BeginUpdate( );
			
			Items.Remove( old_resource );
			
			Items.Insert( index, new_resource );
			
			SelectedIndex = index;
			
			EndUpdate( );
		}
		
		public void ClearResources( )
		{
			showType = ResourceType.TypeImage;
			old_resource_type = ResourceType.None;
			
			BeginUpdate( );
			
			Items.Clear( );
			
			EndUpdate( );
		}
		
		protected override void OnClick( EventArgs e )
		{
			if ( SelectedIndex != -1 && SelectedIndex != old_selected_index )
			{
				old_selected_index = SelectedIndex;
				resourceTreeView.ShowItem( Items[ SelectedIndex ] as IResource, showType );
				resourceTreeView.Focus( );
			}
			base.OnClick( e );
		}
		
		protected override void OnDrawItem( DrawItemEventArgs e )
		{
			if ( e.Index != -1 )
			{
				IResourceRenderer renderer = Items[ e.Index ] as IResourceRenderer;
				
				Brush brush = null;
				
				if ( e.Index % 2 == 0 )
					brush = evenBrush;
				else
					brush = oddBrush;
				
				if ( ( e.State & DrawItemState.Selected ) == DrawItemState.Selected )
				{
//					using ( LinearGradientBrush lgbr = new LinearGradientBrush( new Point( 0, e.Bounds.Y - 1), new Point( 0, e.Bounds.Bottom ),Color.White, Color.Red ) )
					using ( LinearGradientBrush lgbr = new LinearGradientBrush( new Point( e.Bounds.X - 1, 0 ), new Point( e.Bounds.Right, 0 ), Color.White, Color.Blue ) )
						e.Graphics.FillRectangle( lgbr, e.Bounds );
				}
				else
					e.Graphics.FillRectangle( brush, e.Bounds );
				
				e.Graphics.DrawImage( renderer.RenderContent, e.Bounds.X + 3, e.Bounds.Y );
			}
		}
	}
}


