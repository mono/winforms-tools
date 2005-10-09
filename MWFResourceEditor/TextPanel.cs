// Authors:
//	Alexander Olk, <xenomorph2@onlinehome.de>

using System;
using System.Drawing;
using System.Windows.Forms;

namespace MWFResourceEditor
{
	public class TextPanel : Panel, IPanel
	{
		private TextBox contentTextBox;
		private Button acceptButton;
		
		private MainForm parentForm;
		
		public TextPanel( MainForm parentForm )
		{
			this.parentForm = parentForm;
			
			contentTextBox = new TextBox( );
			acceptButton = new Button( );
			
			SuspendLayout( );
			
			Dock = DockStyle.Fill;
			DockPadding.All = 5;
			
			contentTextBox.Location = new Point( 3, 30 );
			ContentTextBox.Anchor = ( (System.Windows.Forms.AnchorStyles)( ( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom )
			| System.Windows.Forms.AnchorStyles.Left )
			| System.Windows.Forms.AnchorStyles.Right ) ) );
			contentTextBox.Size = new Size( 586, 180 );
			contentTextBox.Multiline = true;
			contentTextBox.AcceptsReturn = true;
			contentTextBox.AcceptsTab = true;
//			contentTextBox.ScrollBars = ScrollBars.Both;
			
			acceptButton.Text = "Accept Changes";
			acceptButton.Location = new Point( 3, 3 );
			acceptButton.Size = new Size( 150, 23 );
			acceptButton.Click += new EventHandler( OnAcceptButtonClick );
			
			Controls.Add( contentTextBox );
			Controls.Add( acceptButton );
			
			ResumeLayout( false );
		}
		
		public TextBox ContentTextBox
		{
			set {
				contentTextBox = value;
			}
			
			get {
				return contentTextBox;
			}
		}
		
		public object Value
		{
			get {
				return contentTextBox.Text;
			}
		}
		
		void OnAcceptButtonClick( object sender, EventArgs e )
		{
			parentForm.ChangeResourceContent( );
		}
	}
}


