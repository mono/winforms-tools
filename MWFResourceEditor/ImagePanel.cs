// Authors:
//	Alexander Olk, <xenomorph2@onlinehome.de>

using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace MWFResourceEditor
{
	public class ImagePanel : Panel, IPanel
	{
		private Image image;
		private Icon icon;
		private Cursor cursor;
		private PictureBox pictureBox;
		private Button button;
		private Bitmap old_bmp;
		
		private ResourceContentControl parentControl;
		
		public ImagePanel( ResourceContentControl parentControl )
		{
			this.parentControl = parentControl;
			
			button = new Button( );
			pictureBox = new PictureBox( );
			SuspendLayout( );
			
			button.Location = new Point( 10, 10 );
			button.Size = new Size( 120, 21 );
			button.Text = "Change Image";
			button.Click += new EventHandler( OnClickButton );
			
			Dock = DockStyle.Fill;
			DockPadding.All = 5;
			
			Controls.Add( pictureBox );
			Controls.Add( button );
			
			ResumeLayout( false );
		}
		
		public Image Image
		{
			set {
				image = value;
				
				pictureBox.Image = value;
				pictureBox.Size = image.Size;
				pictureBox.Location = new Point( ( Width / 2 ) - ( image.Width / 2 ), ( Height / 2 ) - ( image.Height / 2 ) );
			}
			
			get {
				return image;
			}
		}
		
		public Icon Icon
		{
			set {
				icon = value;
				
				pictureBox.Image = icon.ToBitmap( );
				pictureBox.Size = pictureBox.Image.Size;
				pictureBox.Location = new Point( ( Width / 2 ) - ( icon.Width / 2 ), ( Height / 2 ) - ( icon.Height / 2 ) );
			}
			
			get {
				return icon;
			}
		}
		
		public new Cursor Cursor
		{
			set {
				cursor = value;
				
				if ( old_bmp != null )
					old_bmp.Dispose( );
				
				Bitmap bmp = new Bitmap( cursor.Size.Width, cursor.Size.Height );
				
				using ( Graphics gr = Graphics.FromImage( bmp ) )
					cursor.Draw( gr, new Rectangle( 0, 0, bmp.Width, bmp.Height ) );
				
				old_bmp = bmp;
				
				pictureBox.Image = bmp;
				pictureBox.Size = pictureBox.Image.Size;
				pictureBox.Location = new Point( ( Width / 2 ) - ( cursor.Size.Width / 2 ), ( Height / 2 ) - ( cursor.Size.Height / 2 ) );
			}
		}
		
		public void ClearResource( )
		{
			image = null;
			icon = null;
			cursor = null;
			pictureBox.Image = null;
		}
		
		protected override void OnSizeChanged( EventArgs e )
		{
			if ( pictureBox.Image != null )
				pictureBox.Location = new Point( ( Width / 2 ) - ( pictureBox.Image.Width / 2 ), ( Height / 2 ) - ( pictureBox.Image.Height / 2 ) );
			
			base.OnSizeChanged( e );
		}
		
		void OnClickButton( object sender, EventArgs e )
		{
			OpenFileDialog ofd = new OpenFileDialog( );
			ofd.CheckFileExists = true;
			
			ofd.Filter = "Images (*.png;*.jpg;*.gif;*.bmp)|*.png;*.jpg;*.gif;*.bmp|Icons (*.ico)|*.ico|Cursors (*.cur)|*.cur|All files (*.*)|*.*";
			
			if ( DialogResult.OK == ofd.ShowDialog( ) )
			{
				try
				{
					string upper_file_name = ofd.FileName.ToUpper( );
					
					// icon
					if ( upper_file_name.EndsWith( ".ICO" ) )
					{
						Icon = new Icon( ofd.FileName );
						
						parentControl.Change_Resource_Content( icon );
					}
					else
					if ( upper_file_name.EndsWith( ".CUR" ) )
					{
						Cursor = new Cursor( ofd.FileName );
						
						parentControl.Change_Resource_Content( cursor );
					}
					else
					{
						Image = Image.FromFile( ofd.FileName );
						
						parentControl.Change_Resource_Content( image );
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine( "File {0} not found.", ofd.FileName );
					Console.WriteLine( ex );
				}
			}
		}
	}
}

