// Authors:
//	Alexander Olk, <xenomorph2@onlinehome.de>

using System;
using System.Drawing;

namespace MWFResourceEditor
{
	public class ResourceIcon : ResourceBase, IResource, IResourceRenderer
	{
		private Icon icon = null;
		
		public ResourceIcon( )
		{}
		
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
		
		public object Clone( )
		{
			return this.MemberwiseClone( );
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
		
		public Bitmap RenderContent
		{
			get {
				return renderBitmap;
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
			Graphics gr = CreateNewRenderBitmap( );
			
			if ( icon.Width > thumb_size.Width || icon.Height > thumb_size.Height )
			{
				Bitmap reformated = new Bitmap( thumb_size.Width, thumb_size.Height );
				
				Graphics gb = Graphics.FromImage( reformated );
				
				Image image = icon.ToBitmap( );
				
				gb.DrawImage( image, new Rectangle( 0, 0, thumb_size.Width, thumb_size.Height ), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel );
				
				gr.DrawImage( reformated, thumb_location.X, thumb_location.Y );
				
				gb.Dispose( );
			}
			else
			{
				int x = ( thumb_size.Width / 2 ) - ( icon.Width / 2 );
				int y = ( thumb_size.Height / 2 ) - ( icon.Height / 2 );
				
				gr.DrawIcon( icon, x, y );
			}
			
			gr.DrawString( "Name: " + resource_name, smallFont, solidBrushBlack, content_text_x_pos, content_name_y_pos );
			
			gr.DrawString( "Type: " + icon.GetType( ), smallFont, solidBrushBlack, content_text_x_pos, content_type_y_pos );
			
			gr.DrawString( "Size: " + ContentString( ), smallFont, solidBrushBlack, content_text_x_pos, content_content_y_pos );
			
			gr.Dispose( );
		}
	}
}

