// Authors:
//	Alexander Olk, <xenomorph2@onlinehome.de>

using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Specialized;

namespace MWFResourceEditor
{
	public class ResourceListBox : ListBox
	{
		private StringCollection resourceNames = new StringCollection();
		
		private static int newResourceCounter = 1;
		
		public ResourceListBox( )
		{
			Size = new Size( 500, 300 );
			DrawMode = DrawMode.OwnerDrawFixed;
			ItemHeight = 53;
			BackColor = Color.Beige ;
		}
		
		protected override void OnDrawItem( DrawItemEventArgs e )
		{
			if ( e.Index != -1 )
			{
				IResourceRenderer renderer = (IResourceRenderer)Items[ e.Index ];
				
				if ( ( e.State & DrawItemState.Selected ) == DrawItemState.Selected )
					e.Graphics.FillRectangle( new SolidBrush( Color.LightBlue ), e.Bounds );
				else
					e.Graphics.FillRectangle( new SolidBrush( Color.Beige ), e.Bounds );
				
				e.Graphics.DrawImage( renderer.RenderContent, e.Bounds.X + 3, e.Bounds.Y + 1 );
				
				e.Graphics.DrawLine( Pens.LightBlue, e.Bounds.X, e.Bounds.Y - 1, e.Bounds.Width - 1 , e.Bounds.Y - 1 );
			}
		}
		
		public void AddResource( string resource_name, object resource )
		{
			if ( resourceNames.Contains( resource_name ) )
				return;
			
			resourceNames.Add( resource_name );
			
			if ( resource.GetType( ) == typeof(Bitmap) )
			{
				ResourceImage resImage = new ResourceImage( resource_name, (Image)resource );
				
				Items.Add( resImage );
			}
			else
			if ( resource.GetType( ) == typeof(String) )
			{
				ResourceString resString = new ResourceString( resource_name, resource.ToString( ) );
				
				Items.Add( resString );
			}
			else
			if ( resource.GetType( ) == typeof( Icon ) )
			{
				ResourceIcon resIcon = new ResourceIcon( resource_name, (Icon)resource );
				
				Items.Add( resIcon );
			}
			else
			if ( resource.GetType( ) == typeof( Color ) )
			{
				ResourceColor resColor = new ResourceColor( resource_name, (Color)resource );
				
				Items.Add( resColor );
			}
		}
		
		public void AddResourceDirect( IResource resource )
		{
			if ( resourceNames.Contains( resource.ResourceName ) )
				return;
			
			resourceNames.Add( resource.ResourceName );
			
			Items.Add( resource );
		}
		
		public void InsertResourceDirect( int index, IResource resource )
		{
			if ( resourceNames.Contains( resource.ResourceName ) )
				return;
			
			resourceNames.Add( resource.ResourceName );
			
			Items.Insert( index, resource );
		}
		
		public void RemoveResource( IResource resource )
		{
			resourceNames.Remove( resource.ResourceName );
			
			Items.Remove( resource );
		}
		
		public void RenameResource( IResource resource, string new_name )
		{
			int index = Items.IndexOf( resource );
			
			if ( index != -1 )
			{
				resourceNames.Remove( resource.ResourceName );
				
				Items.Remove( resource );
				
				resource.ResourceName = new_name;
				
				resourceNames.Add( new_name );
				
				Items.Insert( index, resource );
			}
		}
		
		public void ReplaceResource( IResource old_resource, IResource new_resource )
		{
			int index = Items.IndexOf( old_resource );
			
			Items.Remove( old_resource );
			
			Items.Insert( index, new_resource );
		}
		
		public void ClearResources( )
		{
			resourceNames.Clear( );
			Items.Clear( );
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
	}
}


