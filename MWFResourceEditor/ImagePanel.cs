// Authors:
//	Alexander Olk, <xenomorph2@onlinehome.de>

using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace MWFResourceEditor
{
	public class ImagePanel : Panel
	{
		private Image image;
		private Icon icon;
		private Icon old_icon;
		private string imageOrIconName;
		private PictureBox pictureBox;
		private Button button;
		
		private MainForm parentForm;
		
		public ImagePanel( MainForm parentForm )
		{
			this.parentForm = parentForm;
			
			button = new Button( );
			pictureBox = new PictureBox( );
			SuspendLayout( );
			
			BackColor = Color.LightSlateGray;
			pictureBox.BackColor = Color.LightSlateGray;
			
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
		
		public string ImageOrIconName
		{
			set {
				imageOrIconName = value;
			}
			
			get {
				return imageOrIconName;
			}
		}
		
		public Icon Icon
		{
			set {
				icon = value;
				
				if ( old_icon != null )
					old_icon.Dispose();
				
				old_icon = icon;
				
				pictureBox.Image = icon.ToBitmap( );
				pictureBox.Size = pictureBox.Image.Size;
				pictureBox.Location = new Point( ( Width / 2 ) - ( icon.Width / 2 ), ( Height / 2 ) - ( icon.Height / 2 ) );
			}
			
			get {
				return icon;
			}
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
			
			ofd.Filter = "Images (*.png;*.jpg;*.gif;*.bmp)|*.png;*.jpg;*.gif;*.bmp|Icons (*.ico)|*.ico|All files (*.*)|*.*";
			
			if ( DialogResult.OK == ofd.ShowDialog( ) )
			{
				try
				{
					string upper_file_name = ofd.FileName.ToUpper( );
					
					// icon
					if ( upper_file_name.EndsWith( ".ICO" ) )
					{
						Icon = new Icon( ofd.FileName );
						
						imageOrIconName = Path.GetFileName( ofd.FileName );
						
						parentForm.ChangeResourceContent( );
					}
					else
					{
						Image = Image.FromFile( ofd.FileName );
						
						imageOrIconName = Path.GetFileName( ofd.FileName );
						
						parentForm.ChangeResourceContent( );
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine( "File {0} not found.", ofd.FileName );
				}
			}
		}
	}
}

