// Authors:
//	Alexander Olk, <xenomorph2@onlinehome.de>

using System;
using System.Drawing;
using System.Windows.Forms;

// simple text entry dialog
// returns an empty string if cancel is pressed otherwise the entered string
// ListView item edit isn't implemented yet, so let's use this one...

namespace MWFResourceEditor
{
	public class TextEntryDialog : Form
	{
		private string message;
		private string dialogTitle;
		private string dialogResultText;
		private Button okButton;
		private Button cancelButton;
		private Label messageLabel;
		private TextBox textBox;
		
		public TextEntryDialog( )
		{
			okButton = new Button( );
			cancelButton = new Button( );
			textBox = new TextBox( );
			messageLabel = new Label( );
			
			SuspendLayout( );
			
			okButton.Location = new Point( 40, 85 );
			okButton.Size = new Size( 75, 23 );
			okButton.Text = "OK";
			okButton.Click += new EventHandler( OnClickButton );
			
			cancelButton.Location = new Point( 130, 85 );
			cancelButton.Size = new Size( 75, 23 );
			cancelButton.Text = "Cancel";
			cancelButton.Click += new EventHandler( OnClickButton );
			
			messageLabel.Location = new Point( 10, 10 );
			messageLabel.AutoSize = true;
			
			textBox.Location = new Point( 10, 40 );
			textBox.Size = new Size( 198, 23 );
			textBox.ReadOnly = false;
			
			ClientSize = new Size( 220, 140 );
			
			FormBorderStyle = FormBorderStyle.FixedDialog;
			MaximizeBox = false;
			MinimizeBox = false;
			AcceptButton = okButton;
			CancelButton = cancelButton;
			CenterToParent( );
			
			Controls.Add( textBox );
			Controls.Add( messageLabel );
			Controls.Add( okButton );
			Controls.Add( cancelButton );
			
			ResumeLayout( false );
		}
		
		public string Message
		{
			set {
				message = value;
				messageLabel.Text = message;
			}
			
			get {
				return message;
			}
		}
		
		public string DialogResultText
		{
			set {
				dialogResultText = value;
			}
			
			get {
				return dialogResultText;
			}
		}
		
		public string DialogTitle
		{
			set {
				dialogTitle = value;
				Text = value;
			}
			
			get {
				return dialogTitle;
			}
		}
		
		
		void OnClickButton( object sender, EventArgs e )
		{
			if ( sender == okButton )
			{
				dialogResultText = textBox.Text.Trim( );
			}
			else
			{
				dialogResultText = "";
			}
			
			Close( );
		}
		
		public static string Show( string message )
		{
			return Show( "Enter Text", message );
		}
		
		public static string Show( string title, string message )
		{
			TextEntryDialog td = new TextEntryDialog( );
			td.DialogTitle = title;
			td.Message = message;
			td.ShowDialog( );
			return td.DialogResultText;
		}
	}
}

