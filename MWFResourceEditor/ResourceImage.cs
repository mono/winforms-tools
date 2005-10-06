// Authors:
//	Alexander Olk, <xenomorph2@onlinehome.de>

using System;
using System.Drawing;

namespace MWFResourceEditor
{
	public class ResourceImage : ResourceBase, IResource
	{
		private Image image = null;
		private Image old_image = null;
		
		public ResourceImage( string name, Image image )
		{
			ResourceName = name;
			Image = image;
		}
		
		public ResourceType ResourceType
		{
			get {
				return ResourceType.TypeImage;
			}
		}
		
		public Image Image
		{
			set {
				if ( old_image != null )
					old_image.Dispose( );
				
				if ( image != null )
					all_data_for_rendering_available = 1;
				
				image = value;
				old_image = image;
				
				all_data_for_rendering_available++;
				
				if ( all_data_for_rendering_available == 2 )
					CreateRenderBitmap( );
			}
			
			get {
				return image;
			}
		}
		
		public string ContentString( )
		{
			string imagesize = "Width = ";
			
			imagesize += image.Width + ", Height = ";
			imagesize += image.Height;
			
			return imagesize;
		}
		
		protected override void CreateRenderBitmap( )
		{
			using ( Graphics gr = CreateNewRenderBitmap( ) )
			{
				if ( image.Width >= thumb_size.Width || image.Height >= thumb_size.Height )
				{
					int new_width = image.Width < thumb_size.Width - 1 ? image.Width : thumb_size.Width - 1;
					int new_height = image.Height < thumb_size.Height - 1 ? image.Height : thumb_size.Height - 1;
					
					using ( Image thumbnail = GetThumbNail( image , new_width, new_height ) )
					{
						int x = ( thumb_size.Width / 2 ) - ( thumbnail.Width / 2 ) - 1;
						int y = ( thumb_size.Height / 2 ) - ( thumbnail.Height / 2 );
						
						gr.DrawImage( thumbnail, thumb_location.X + x, thumb_location.Y + y );
					}
				}
				else
				{
					int x = ( thumb_size.Width / 2 ) - ( image.Width / 2 );
					int y = ( thumb_size.Height / 2 ) - ( image.Height / 2 );
					
					gr.DrawImage( image, thumb_location.X + x, thumb_location.Y + y );
				}
				
				gr.DrawString( "Name: " + resource_name, smallFont, solidBrushBlack, content_text_x_pos, content_name_y_pos );
				
				gr.DrawString( "Type: " + image.GetType( ), smallFont, solidBrushBlack, content_text_x_pos, content_type_y_pos );
				
				gr.DrawString( "Size: " + ContentString( ), smallFont, solidBrushBlack, content_text_x_pos, content_content_y_pos );
			}
		}
	}
}

