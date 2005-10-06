// Authors:
//	Alexander Olk, <xenomorph2@onlinehome.de>

using System;
using System.Drawing;

namespace MWFResourceEditor
{
	public class ResourceIcon : ResourceBase, IResource
	{
		private Icon icon = null;
		
		public ResourceIcon( string name, Icon icon )
		{
			ResourceName = name;
			Icon = icon;
		}
		
		public ResourceType ResourceType
		{
			get {
				return ResourceType.TypeIcon;
			}
		}
		
		public Icon Icon
		{
			set {
				if ( icon != null )
					all_data_for_rendering_available = 1;
				
				icon = value;
				
				all_data_for_rendering_available++;
				
				if ( all_data_for_rendering_available == 2 )
					CreateRenderBitmap( );
			}
			
			get {
				return icon;
			}
		}
		
		public string ContentString( )
		{
			string imagesize = "Width = ";
			
			imagesize += icon.Width + ", Height = ";
			imagesize += icon.Height;
			
			return imagesize;
		}
		
		protected override void CreateRenderBitmap( )
		{
			using ( Graphics gr = CreateNewRenderBitmap( ) )
			{
				if ( icon.Width >= thumb_size.Width || icon.Height >= thumb_size.Height )
				{
					int new_width = icon.Width < thumb_size.Width - 1 ? icon.Width : thumb_size.Width - 1;
					int new_height = icon.Height < thumb_size.Height - 1 ? icon.Height : thumb_size.Height - 1;
					
					using ( Image thumbnail = GetThumbNail( icon.ToBitmap( ) , new_width, new_height ) )
					{
						int x = ( thumb_size.Width / 2 ) - ( thumbnail.Width / 2 ) - 1;
						int y = ( thumb_size.Height / 2 ) - ( thumbnail.Height / 2 );
						
						gr.DrawImage( thumbnail, thumb_location.X + x, thumb_location.Y + y );
					}
				}
				else
				{
					int x = ( thumb_size.Width / 2 ) - ( icon.Width / 2 );
					int y = ( thumb_size.Height / 2 ) - ( icon.Height / 2 );
					
					gr.DrawImage( icon.ToBitmap( ), thumb_location.X + x, thumb_location.Y + y );
				}
				
				gr.DrawString( "Name: " + resource_name, smallFont, solidBrushBlack, content_text_x_pos, content_name_y_pos );
				
				gr.DrawString( "Type: " + icon.GetType( ), smallFont, solidBrushBlack, content_text_x_pos, content_type_y_pos );
				
				gr.DrawString( "Size: " + ContentString( ), smallFont, solidBrushBlack, content_text_x_pos, content_content_y_pos );
			}
		}
	}
}

