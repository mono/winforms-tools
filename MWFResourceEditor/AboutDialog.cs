// Authors:
//	Alexander Olk, <xenomorph2@onlinehome.de>

using System;
using System.Drawing;
using System.Windows.Forms;

namespace MWFResourceEditor
{
	public class AboutDialog : Form
	{
		private Button okButton;
		private PaintPanel paintPanel;
		
		public AboutDialog( )
		{
			okButton = new Button( );
			paintPanel = new PaintPanel( );
			
			SuspendLayout( );
			
			okButton.Text = "OK";
			okButton.Location = new Point( 170, 260 );
			okButton.Click += new EventHandler( OnOkButtonClick );
			
			paintPanel.BorderStyle = BorderStyle.Fixed3D;
			paintPanel.Location = new Point( 10, 10 );
			paintPanel.Size = new Size( 380, 240 );
			paintPanel.BackColor = Color.White;
			
			Text = "About MWFResourceEditor...";
			
			this.FormBorderStyle = FormBorderStyle.FixedDialog;
			
			ClientSize = new Size( 400, 300 );
			
			AcceptButton = okButton;
			
			Controls.Add( okButton );
			Controls.Add( paintPanel );
			
			ResumeLayout( false );
		}
		
		void OnOkButtonClick( object sender, EventArgs e )
		{
			Close( );
		}
		
		internal class PaintPanel : Panel
		{
			private System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
			
			private Font paintFont;
			private Font smallFont;
			private SolidBrush managedPaintBrush;
			private SolidBrush windowsPaintBrush;
			private SolidBrush formsPaintBrush;
			private SolidBrush shadowBrush;
			private Color paintColor;
			private int counter = 0;
			private bool managedDrawn = false;
			private bool windowsDrawn = false;
			private bool formsDrawn = false;
			
			private const int alphaMax = 32;
			
			public PaintPanel( )
			{
				paintFont = new Font( FontFamily.GenericMonospace, 36, FontStyle.Regular );
				smallFont = new Font( FontFamily.GenericSansSerif, 18, FontStyle.Italic );
				
				paintColor = Color.FromArgb( 0, Color.Red );
				shadowBrush = new SolidBrush( Color.FromArgb( 180, Color.LightBlue ) );
				
				windowsPaintBrush = new SolidBrush( paintColor );
				managedPaintBrush = new SolidBrush( paintColor );
				formsPaintBrush = new SolidBrush( paintColor );
				
				timer.Tick += new EventHandler( OnTimerTick );
				timer.Interval = 50;
			}
			
			protected override void OnPaint( PaintEventArgs pea )
			{
				base.OnPaint( pea );
				
				Bitmap bmp = new Bitmap( pea.ClipRectangle.Width, pea.ClipRectangle.Height, pea.Graphics );
				Graphics gr = Graphics.FromImage( bmp );
				
				if ( formsDrawn )
				{
					gr.DrawString( "Managed", paintFont, shadowBrush, new Point( 75, 15 ) );
					gr.DrawString( "Windows", paintFont, shadowBrush, new Point( 75, 65 ) );
					gr.DrawString( "Forms", paintFont, shadowBrush, new Point( 75, 115 ) );
					
					gr.DrawString( "Resource Editor", smallFont, new SolidBrush( Color.Black ), new Point( 80, 185 ) );
					
					managedPaintBrush = new SolidBrush( Color.Red );
					windowsPaintBrush = new SolidBrush( Color.Red );
					formsPaintBrush = new SolidBrush( Color.Red );
				}
				
				gr.DrawString( "Managed", paintFont, managedPaintBrush, new Point( 70, 10 ) );
				
				gr.DrawString( "Windows", paintFont, windowsPaintBrush, new Point( 70, 60 ) );
				
				gr.DrawString( "Forms", paintFont, formsPaintBrush, new Point( 70, 110 ) );
				
				pea.Graphics.DrawImage( bmp, pea.ClipRectangle.X, pea.ClipRectangle.Y );
			}
			
			protected override void OnVisibleChanged( EventArgs e )
			{
				if ( Visible )
					timer.Start( );
				else
					timer.Stop( );
				
				base.OnVisibleChanged( e );
			}
			
			void OnTimerTick( object sender, EventArgs e )
			{
				if ( !managedDrawn )
				{
					paintColor = Color.FromArgb( counter++, Color.Red );
					managedPaintBrush = new SolidBrush( paintColor );
					if ( counter == alphaMax )
					{
						managedDrawn = true;
						counter = 0;
					}
				}
				else
				if ( !windowsDrawn )
				{
					paintColor = Color.FromArgb( counter++, Color.Red );
					windowsPaintBrush = new SolidBrush( paintColor );
					if ( counter == alphaMax )
					{
						windowsDrawn = true;
						counter = 0;
					}
				}
				else
				if ( !formsDrawn )
				{
					paintColor = Color.FromArgb( counter++, Color.Red );
					formsPaintBrush = new SolidBrush( paintColor );
					if ( counter == alphaMax )
					{
						formsDrawn = true;
						timer.Stop( );
					}
				}
				
				
				Invalidate( );
				Update( );
			}
		}
	}
}

