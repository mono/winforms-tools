// Authors:
//	Alexander Olk, <xenomorph2@onlinehome.de>

using System;
using System.Drawing;

namespace MWFResourceEditor
{
	public class ResourceColor : ResourceBase, IResource, IResourceRenderer
	{
		private Color color;
		
		public ResourceColor( )
		{}
		
		public ResourceColor( string name, Color color )
		{
			ResourceName = name;
			Color = color;
		}
		
		public ResourceType ResourceType
		{
			get {
				return ResourceType.TypeColor;
			}
		}
		
		public object Clone( )
		{
			return this.MemberwiseClone( );
		}
		
		public Color Color
		{
			set {
				if ( color != Color.Empty )
					all_data_for_rendering_available = 1;
				
				color = value;
				
				all_data_for_rendering_available++;
				
				if ( all_data_for_rendering_available == 2 )
					CreateRenderBitmap( );
			}
			
			get {
				return color;
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
			return color.ToString( );
		}
		
		protected override void CreateRenderBitmap( )
		{
			Graphics gr = CreateNewRenderBitmap( );
			
			gr.FillRectangle( new SolidBrush( color ), thumb_location.X, thumb_location.Y, thumb_size.Width, thumb_size.Height );
			
			gr.DrawString( "Name: " + resource_name, smallFont, solidBrushBlack, content_text_x_pos, content_name_y_pos );
			
			gr.DrawString( "Type: " + color.GetType( ), smallFont, solidBrushBlack, content_text_x_pos, content_type_y_pos );
			
			gr.DrawString( "Color: " + ContentString( ), smallFont, solidBrushBlack, content_text_x_pos, content_content_y_pos );
			
			gr.Dispose( );
		}
	}
}

