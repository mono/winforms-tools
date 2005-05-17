// Authors:
//	Alexander Olk, <xenomorph2@onlinehome.de>

using System;
using System.Drawing;
using System.Windows.Forms;
using System.Text;

namespace MWFResourceEditor
{
	public class ByteArrayPanel : Panel
	{
		private Byte[] byteArray;
		
		private CheckBox textHexCheckBox;
		private TextBox textBox;
		
		private StringBuilder sb_text;
		private StringBuilder sb_hex;
		
		public ByteArrayPanel( MainForm parentForm )
		{
			textHexCheckBox = new CheckBox( );
			textBox = new TextBox( );
			
			SuspendLayout( );
			
			Dock = DockStyle.Fill;
			DockPadding.All = 5;
			
			textHexCheckBox.FlatStyle = FlatStyle.System;
			textHexCheckBox.Location = new Point( 3, 3 );
			textHexCheckBox.TabIndex = 1;
			textHexCheckBox.Text = "Show as Hex";
			textHexCheckBox.Checked = false;
			textHexCheckBox.CheckedChanged += new EventHandler( OnCheckedChangedTextCheckBox );
			
			textBox.Location = new Point( 3, 30 );
			textBox.Multiline = true;
			textBox.ReadOnly = true;
			textBox.Anchor = ( (System.Windows.Forms.AnchorStyles)( ( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom )
			| System.Windows.Forms.AnchorStyles.Left )
			| System.Windows.Forms.AnchorStyles.Right ) ) );
			textBox.Size = new Size( 586, 180 );
			textBox.ScrollBars = ScrollBars.Vertical | ScrollBars.Horizontal;
			textBox.Font = new Font( FontFamily.GenericMonospace, 8 );
			
			Controls.Add( textBox );
			Controls.Add( textHexCheckBox );
			
			ResumeLayout( false );
			
		}
		
		public Byte[] ByteArray
		{
			set {
				byteArray = value;
				
				sb_text = new StringBuilder( byteArray.Length );
				sb_hex = new StringBuilder( byteArray.Length * 4 );
				
				GenerateTextAndHexOutput( );
				
				textBox.Text = sb_text.ToString( );
			}
			
			get {
				return byteArray;
			}
		}
		
		void OnCheckedChangedTextCheckBox( object sender, EventArgs e )
		{
			if ( textHexCheckBox.Checked )
				textBox.Text = sb_hex.ToString( );
			else
				textBox.Text = sb_text.ToString( );
			
			textBox.Invalidate( );
		}
		
		private void GenerateTextAndHexOutput( )
		{
			bool new_line = false;
			
			StringBuilder back_string = back_string = new StringBuilder( 19 );
			
			sb_hex.Append( "00000000  " );
			
			for ( int i = 0; i < byteArray.Length; i++ )
			{
				sb_text.Append( Convert.ToChar( byteArray[ i ] ) );
				
				if ( new_line )
				{
					new_line = false;
					
					back_string.Append( "\n" );
					
					sb_hex.Append( back_string );
					
					sb_hex.Append( i.ToString( "X8" ) + "  " );
					
					back_string = new StringBuilder( 19 );
					
					back_string.Append( "  " );
				}
				
				sb_hex.Append( byteArray[ i ].ToString( "X2" ) + " " );
				
				if ( byteArray[ i ] != 9 && byteArray[ i ] != 10 && byteArray[ i ] != 13 )
					back_string.Append( Convert.ToChar( byteArray[ i ] ) );
				else
					back_string.Append( "." );
				
				if ( i > 0 && ( i % 16 ) == 0 )
				{
					new_line = true;
				}
			}
		}
	}
}
