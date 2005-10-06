// Authors:
//	Alexander Olk, <xenomorph2@onlinehome.de>

using System;
using System.Drawing;

namespace MWFResourceEditor
{
	public class ResourceString  : ResourceBase, IResource
	{
		private string text = null;
		
		public ResourceString( string name, string text )
		{
			ResourceName = name;
			Text = text;
		}
		
		public ResourceType ResourceType
		{
			get {
				return ResourceType.TypeString;
			}
		}
		
		public string Text
		{
			set {
				if ( text != null )
					all_data_for_rendering_available = 1;
				
				text = value;
				
				all_data_for_rendering_available++;
				
				if ( all_data_for_rendering_available == 2 )
					CreateRenderBitmap( );
			}
			
			get {
				return text;
			}
		}
		
		public string ContentString( )
		{
			return text;
		}
		
		protected override void CreateRenderBitmap( )
		{
			using ( Graphics gr = CreateNewRenderBitmap( ) )
			{
				SizeF fontSizeF = gr.MeasureString( text, smallFont );
				
				int text_width = (int)fontSizeF.Width;
				int text_height = (int)fontSizeF.Height;
				
				int x = ( thumb_size.Width / 2 ) - ( text_width / 2 );
				if ( x < 0 ) x = 0;
				
				int y = ( thumb_size.Height / 2 ) - ( text_height / 2 );
				
				using ( Bitmap bmp = new Bitmap( thumb_size.Width, thumb_size.Height ) )
				{
					using ( Graphics gr_bmp = Graphics.FromImage( bmp ) )
					{
						gr_bmp.DrawString( text, smallFont, solidBrushAqua, x, y );
					}
					
					gr.DrawImage( bmp, thumb_location.X, thumb_location.Y );
					
					gr.DrawString( "Name: " + resource_name, smallFont, solidBrushBlack, content_text_x_pos, content_name_y_pos );
					
					gr.DrawString( "Type: " + text.GetType( ), smallFont, solidBrushBlack, content_text_x_pos, content_type_y_pos );
					
					gr.DrawString( "Content: " + text, smallFont, solidBrushBlack, content_text_x_pos, content_content_y_pos );
				}
			}
		}
	}
}


