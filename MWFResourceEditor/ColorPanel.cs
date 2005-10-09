// Authors:
//	Alexander Olk, <xenomorph2@onlinehome.de>

using System;
using System.Drawing;
using System.Windows.Forms;

namespace MWFResourceEditor
{
	public class ColorPanel : Panel, IPanel
	{
		private Color color;
		private Button button;
		private Panel panel;
		private Label label;
		
		private MainForm parentForm;
		
		public ColorPanel( MainForm parentForm )
		{
			this.parentForm = parentForm;
			
			button = new Button( );
			panel = new Panel( );
			label = new Label( );
			SuspendLayout( );
			
			BackColor = Color.LightSlateGray;
			
			button.Location = new Point( 10, 10 );
			button.Size = new Size( 120, 21 );
			button.Text = "Change Color";
			button.Click += new EventHandler( OnClickButton );
			
			panel.Size = new Size( 60, 60 );
			
			label.AutoSize = true;
			label.BackColor = BackColor;
			
			Dock = DockStyle.Fill;
			DockPadding.All = 5;
			
			Controls.Add( button );
			Controls.Add( panel );
			Controls.Add( label );
			
			ResumeLayout( false );
		}
		
		public Color Color
		{
			set {
				color = value;
				
				panel.BackColor = color;
				panel.Location = new Point( ( Width / 2 ) - ( panel.Width / 2 ), ( Height / 2 ) - ( panel.Height / 2 ) );
				
				label.Text = color.ToString( );
				
				label.Location = new Point( ( Width / 2 ) - ( label.Width / 2 ), ( Height / 2 ) - ( panel.Height / 2 ) - 30 );
			}
			
			get {
				return color;
			}
		}
		
		public object Value
		{
			get {
				return color;
			}
		}
		
		void OnClickButton( object sender, EventArgs e )
		{
			ColorDialog cd = new ColorDialog( );
			cd.Color = color;
			
			if ( DialogResult.OK == cd.ShowDialog( ) )
			{
				color = cd.Color;
				
				parentForm.ChangeResourceContent( );
				
				Invalidate( );
				Update( );
			}
		}
		
		protected override void OnSizeChanged( EventArgs e )
		{
			panel.Location = new Point( ( Width / 2 ) - ( panel.Width / 2 ), ( Height / 2 ) - ( panel.Height / 2 ) );
			
			label.Location = new Point( ( Width / 2 ) - ( label.Width / 2 ), ( Height / 2 ) - ( panel.Height / 2 ) - 30 );
			
			base.OnSizeChanged( e );
		}
	}
}

