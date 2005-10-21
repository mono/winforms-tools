// Authors:
//	Alexander Olk, <xenomorph2@onlinehome.de>

using System;
using System.Drawing;
using System.Windows.Forms;
using System.Text;
using System.Collections.Specialized;

namespace MWFResourceEditor
{
	public class ByteArrayPanel : Panel, IPanel
	{
		private Byte[] byteArray;
		
		private CheckBox textHexCheckBox;
		private TextBox textBox;
		
		private StringCollection sc_text = new StringCollection();
		private StringCollection sc_hex = new StringCollection();
		
		public ByteArrayPanel( ResourceContentControl parentControl )
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
			textBox.ScrollBars = ScrollBars.Vertical;
			textBox.Font = new Font( FontFamily.GenericMonospace, 8 );
			textBox.WordWrap = true;
			
			Controls.Add( textBox );
			Controls.Add( textHexCheckBox );
			
			ResumeLayout( false );
		}
		
		public Byte[] ByteArray
		{
			set {
				byteArray = value;
				
				sc_text.Clear( );
				sc_hex.Clear( );
				
				GenerateTextAndHexOutput( );
				
				textBox.Lines = GetLines( textHexCheckBox.Checked ? sc_hex : sc_text );
			}
			
			get {
				return byteArray;
			}
		}
		
		public void ClearResource( )
		{
			byteArray = null;
			sc_text.Clear( );
			sc_hex.Clear( );
			textBox.Clear( );
		}
		
		void OnCheckedChangedTextCheckBox( object sender, EventArgs e )
		{
			if ( textHexCheckBox.Checked )
				textBox.Lines = GetLines( sc_hex );
			else
				textBox.Lines = GetLines( sc_text );
			
			textBox.Invalidate( );
		}
		
		private string[] GetLines( StringCollection sc )
		{
			string[] tmp = new string[ sc.Count ];
			
			sc.CopyTo( tmp, 0 );
			
			return tmp;
		}
		
		private void GenerateTextAndHexOutput( )
		{
			StringBuilder sb_text = new StringBuilder( );
			StringBuilder sb_hex = new StringBuilder( );
			
			int show_how_many_bytes = byteArray.Length > 1024 ? 1024 : byteArray.Length;
			
			StringBuilder back_string = back_string = new StringBuilder( 19 );
			back_string.Append( "  " );
			
			sb_hex.Append( "00000000  " );
			
			int counter = 0;
			
			for ( int i = 0; i < show_how_many_bytes; i++ )
			{
				counter++;
				
				char c = Convert.ToChar( byteArray[ i ] );
				
				sb_hex.Append( byteArray[ i ].ToString( "X2" ) + " " );
				
				if ( Char.IsLetterOrDigit( c ) )
					back_string.Append( c );
				else
					back_string.Append( "." );
				
				if ( i > 0 && ( counter % 16 ) == 0 )
				{
					sb_hex.Append( back_string );
					
					back_string = new StringBuilder( 19 );
					
					back_string.Append( "  " );
					
					sc_hex.Add( sb_hex.ToString( ) );
					sb_hex = new StringBuilder( );
					
					sb_hex.Append( i.ToString( "X8" ) + "  " );
				}
				
				if ( c != '\n' && c != '\r' )
					sb_text.Append( c );
				else
				if ( c == '\n' )
				{
					sc_text.Add( sb_text.ToString( ) );
					sb_text = new StringBuilder( );
				}
			}
		}
		
		protected override void OnSizeChanged( EventArgs e )
		{
			textBox.Size = new Size( Width - 10, Height - 40 );
			base.OnSizeChanged( e );
		}
	}
}

