// Authors:
//	Alexander Olk, <xenomorph2@onlinehome.de>

using System;
using System.Drawing;

namespace MWFResourceEditor
{
	public abstract class ResourceBase : IResourceRenderer, ICloneable
	{
		protected string resource_name = null;
		
		protected static int content_text_x_pos = 60;
		protected static int content_name_y_pos = 3;
		protected static int content_type_y_pos = 16;
		protected static int content_content_y_pos = 29;
		
		protected static Point thumb_location = new Point( 3, 3 );
		protected static Size thumb_size = new Size( 42, 42 );
		protected static Size renderBitmap_size = new Size( 300, 50 );
		
		protected static SolidBrush solidBrushBlack = new SolidBrush( Color.Black );
		protected static SolidBrush solidBrushTransparent = new SolidBrush( Color.Transparent );
		protected static SolidBrush solidBrushWhite = new SolidBrush( Color.White );
		protected static SolidBrush solidBrushAqua = new SolidBrush( Color.Aqua );
		
		protected static Font smallFont = new Font ("Arial", 8);
		
		protected Bitmap renderBitmap;
		
		protected int all_data_for_rendering_available = 0;
		
		public Bitmap RenderContent
		{
			get {
				return renderBitmap;
			}
		}
		
		public string ResourceName
		{
			get {
				return resource_name;
			}
			set {
				if ( resource_name != null )
					all_data_for_rendering_available = 1;
				
				resource_name = value;
				
				all_data_for_rendering_available++;
				
				if ( all_data_for_rendering_available == 2 )
					CreateRenderBitmap( );
			}
		}
		
		protected Graphics CreateNewRenderBitmap( )
		{
			if ( renderBitmap != null )
				renderBitmap.Dispose( );
			
			renderBitmap = new Bitmap( renderBitmap_size.Width, renderBitmap_size.Height );
			
			Graphics gr = Graphics.FromImage( renderBitmap );
			
			gr.FillRectangle( solidBrushTransparent, gr.ClipBounds );
			
			gr.FillRectangle( solidBrushWhite, new Rectangle( thumb_location.X - 2, thumb_location.Y - 2, thumb_size.Width + 4, thumb_size.Height + 4 ) );
			gr.DrawRectangle( Pens.Black, new Rectangle( thumb_location.X - 2, thumb_location.Y - 2, thumb_size.Width + 4, thumb_size.Height + 4 ) );
			
			return gr;
		}
		
		protected abstract void CreateRenderBitmap( );
		
		public Object Clone( )
		{
			return this.MemberwiseClone( );
		}
		
		public bool DummyThumbnailCallback( )
		{
			return false;
		}
		
		public Image GetThumbNail( Image image, int new_width, int new_height )
		{
			Image.GetThumbnailImageAbort thumbnailCallback = new Image.GetThumbnailImageAbort( DummyThumbnailCallback );
			
			Image thumbnail = image.GetThumbnailImage( new_width, new_height, thumbnailCallback, IntPtr.Zero );
			
			return thumbnail;
		}
	}
}

