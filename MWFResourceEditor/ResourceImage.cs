// Authors:
//	Alexander Olk, <xenomorph2@onlinehome.de>

using System;
using System.Drawing;

namespace MWFResourceEditor
{
	public class ResourceImage : ResourceBase, IResource, IResourceRenderer
	{
		private Image image = null;
		
		public ResourceImage( )
		{}
		
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
		
		public Object Clone( )
		{
			return this.MemberwiseClone( );
		}
		
		public Image Image
		{
			set {
				if ( image != null )
					all_data_for_rendering_available = 1;
				
				image = value;
				
				all_data_for_rendering_available++;
				
				if ( all_data_for_rendering_available == 2 )
					CreateRenderBitmap( );
			}
			
			get {
				return image;
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
			
			imagesize += image.Width + ", Height = ";
			imagesize += image.Height;
			
			return imagesize;
		}
		
		protected override void CreateRenderBitmap( )
		{
			Graphics gr = CreateNewRenderBitmap( );
			
			if ( image.Width > thumb_size.Width || image.Height > thumb_size.Height )
			{
				Bitmap reformated = new Bitmap( thumb_size.Width, thumb_size.Height );
				
				Graphics gb = Graphics.FromImage( reformated );
				
				gb.DrawImage( image, new Rectangle( 0, 0, thumb_size.Width, thumb_size.Height ), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel );
				
				gr.DrawImage( reformated, thumb_location.X, thumb_location.Y );
				
				gb.Dispose( );
			}
			else
			{
				int x = ( thumb_size.Width / 2 ) - ( image.Width / 2 );
				int y = ( thumb_size.Height / 2 ) - ( image.Height / 2 );
				
				gr.DrawImage( image, x, y );
			}
			
			gr.DrawString( "Name: " + resource_name, smallFont, solidBrushBlack, content_text_x_pos, content_name_y_pos );
			
			gr.DrawString( "Type: " + image.GetType( ), smallFont, solidBrushBlack, content_text_x_pos, content_type_y_pos );
			
			gr.DrawString( "Size: " + ContentString( ), smallFont, solidBrushBlack, content_text_x_pos, content_content_y_pos );
			
			gr.Dispose( );
		}
	}
}

