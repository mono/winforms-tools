using System;
using System.Drawing;
using System.Windows.Forms;

namespace MWFResourceEditor
{
	public class ImagePanel : Panel
	{
		private Image image;
		private string imageName;
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
			
			button.Location = new Point( 10, 10 );
			button.Size = new Size( 120, 21 );
			button.Text = "Change Image";
			button.Click += new EventHandler( OnClickButton );
			
			Controls.Add( button );
			Controls.Add( pictureBox );
			
			ResumeLayout( false );
		}
		
		public Image Image
		{
			set
			{
				image = value;
				pictureBox.Image = value;
				pictureBox.Size = image.Size;
				pictureBox.Location = new Point( ( Width / 2 ) - ( image.Width / 2 ), ( Height / 2 ) - ( image.Height / 2 ) );
			}
			
			get
			{
				return image;
			}
		}
		
		public string ImageName
		{
			set
			{
				imageName = value;
			}
			
			get
			{
				return imageName;
			}
		}
		
		void OnClickButton( object sender, EventArgs e )
		{
			OpenFileDialog ofd = new OpenFileDialog( );
			ofd.CheckFileExists = true;
			
			ofd.Filter = "Images (*.png;*.jpg;*.gif;*.bmp)|*.png;*.jpg;*.gif;*.bmp|All files (*.*)|*.*";
			
			if ( DialogResult.OK == ofd.ShowDialog( ) )
			{
				Image = Image.FromFile( ofd.FileName );
				
				imageName = ofd.FileName;
				
				string[] split = imageName.Split( new Char[] { '\\', '/' } );
				
				if ( split.Length > 0 )
					imageName = split[ split.Length - 1 ];
				
				parentForm.ChangeContentImage( );
			}
		}
	}
}

