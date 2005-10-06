// Authors:
//	Alexander Olk, <xenomorph2@onlinehome.de>

using System;
using System.Drawing;

namespace MWFResourceEditor
{
	public class ResourceByteArray  : ResourceBase, IResource
	{
		private byte[] byteArray = null;
		
		private static Bitmap one_and_zero;
		
		static ResourceByteArray( )
		{
			one_and_zero = new Bitmap( ResourceBase.thumb_size.Width, ResourceBase.thumb_size.Height );
			
			using ( Graphics gr = Graphics.FromImage( one_and_zero ) )
			{
				Font font = new Font( FontFamily.GenericMonospace, 5 );
				
				SizeF zero_size = gr.MeasureString( "0", font );
				SizeF one_size = gr.MeasureString( "1", font );
				
				int y_counter = 0;
				
				while ( y_counter < thumb_size.Height )
				{
					int x_counter = 0;
					
					while ( x_counter < thumb_size.Width )
					{
						Random random = new Random( );
						
						string what = random.Next( 2 ) == 1 ? "1" : "0";
						
						gr.DrawString( what, font, solidBrushBlack, x_counter, y_counter );
						
						x_counter += what == "1" ? (int)one_size.Width : (int)zero_size.Width;
					}
					
					y_counter += (int)zero_size.Height;
				}
			}
		}
		
		public ResourceByteArray( string name, byte[] byteArray )
		{
			ResourceName = name;
			ByteArray = byteArray;
		}
		
		public byte[] ByteArray
		{
			set {
				if ( byteArray != null )
					all_data_for_rendering_available = 1;
				
				byteArray = value;
				
				all_data_for_rendering_available++;
				
				if ( all_data_for_rendering_available == 2 )
					CreateRenderBitmap( );
			}
			
			get {
				return byteArray;
			}
		}
		
		public ResourceType ResourceType
		{
			get {
				return ResourceType.TypeByteArray;
			}
		}
		
		public string ContentString( )
		{
			return byteArray.ToString( );
		}
		
		protected override void CreateRenderBitmap( )
		{
			using ( Graphics gr = CreateNewRenderBitmap( ) )
			{
				gr.DrawImage( one_and_zero, thumb_location.X, thumb_location.Y, thumb_size.Width, thumb_size.Height );
				
				gr.DrawString( "Name: " + resource_name, smallFont, solidBrushBlack, content_text_x_pos, content_name_y_pos );
				
				gr.DrawString( "Type: " + byteArray.GetType( ), smallFont, solidBrushBlack, content_text_x_pos, content_type_y_pos );
				
				gr.DrawString( "Size: " + byteArray.Length, smallFont, solidBrushBlack, content_text_x_pos, content_content_y_pos );
			}
		}
	}
}

