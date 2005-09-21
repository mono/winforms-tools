// Authors:
//	Peter Dennis Bartok, <pbartok@novell.com>
//	Alexander Olk, <xenomorph2@onlinehome.de>

using System;
using System.Drawing;
using System.Windows.Forms;

namespace MWFResourceEditor
{
	public class ResourceCursor : ResourceBase, IResource, IResourceRenderer
	{
		private Cursor cursor = null;
		
		public ResourceCursor( )
		{}
		
		public ResourceCursor( string name, Cursor cursor )
		{
			ResourceName = name;
			Cursor = cursor;
		}
		
		public ResourceType ResourceType
		{
			get {
				return ResourceType.TypeCursor;
			}
		}
		
		public object Clone( )
		{
			return this.MemberwiseClone( );
		}
		
		public Cursor Cursor
		{
			set {
				if ( cursor != null )
					all_data_for_rendering_available = 1;
				
				cursor = value;
				
				all_data_for_rendering_available++;
				
				if ( all_data_for_rendering_available == 2 )
					CreateRenderBitmap( );
			}
			
			get {
				return cursor;
			}
		}

		public Bitmap RenderContent
		{
			get {
				return renderBitmap;
			}
		}
		
		public string ContentString( )
		{
			string imagesize = "Width = ";
			
			imagesize += cursor.Size.Width + ", Height = ";
			imagesize += cursor.Size.Height;
			
			return imagesize;
		}
		
		protected override void CreateRenderBitmap( )
		{
			Graphics gr = CreateNewRenderBitmap( );
			
			if ( cursor.Size.Width > thumb_size.Width || cursor.Size.Height > thumb_size.Height )
			{
				cursor.DrawStretched(gr, new Rectangle(thumb_location.X, thumb_location.Y, thumb_size.Width, thumb_size.Height));
			}
			else
			{
				int x = ( thumb_size.Width / 2 ) - ( cursor.Size.Width / 2 );
				int y = ( thumb_size.Height / 2 ) - ( cursor.Size.Height / 2 );
				
				cursor.Draw(gr, new Rectangle(x, y, cursor.Size.Width, cursor.Size.Height));
			}
			
			gr.DrawString( "Name: " + resource_name, smallFont, solidBrushBlack, content_text_x_pos, content_name_y_pos );
			
			gr.DrawString( "Type: " + cursor.GetType( ), smallFont, solidBrushBlack, content_text_x_pos, content_type_y_pos );
			
			gr.DrawString( "Size: " + ContentString( ), smallFont, solidBrushBlack, content_text_x_pos, content_content_y_pos );
			
			gr.Dispose( );
		}
	}
}

