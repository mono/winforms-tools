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
		
		private ResourceContentControl parentControl;
		
		public TextPanel( ResourceContentControl parentControl )
		{
			this.parentControl = parentControl;
			
			contentTextBox = new TextBox( );
			acceptButton = new Button( );
			
			SuspendLayout( );
			
			Dock = DockStyle.Fill;
			DockPadding.All = 5;
			
			contentTextBox.Location = new Point( 3, 30 );
			contentTextBox.Multiline = true;
			contentTextBox.AcceptsReturn = true;
			contentTextBox.AcceptsTab = true;
			contentTextBox.TextChanged += new EventHandler( OnContentTextBoxTextChanged );
			contentTextBox.ScrollBars = ScrollBars.Both;
			
			acceptButton.Text = "Accept Changes";
			acceptButton.Location = new Point( 3, 3 );
			acceptButton.Size = new Size( 150, 23 );
			acceptButton.Enabled = false;
			acceptButton.Click += new EventHandler( OnAcceptButtonClick );
			
			Controls.Add( contentTextBox );
			Controls.Add( acceptButton );
			
			ResumeLayout( false );
		}
		
		public string ResourceText
		{
			set {
				contentTextBox.Text = value;
				acceptButton.Enabled = false;
			}
			
			get {
				return contentTextBox.Text;
			}
		}
		
		public void ClearResource( )
		{
			contentTextBox.Clear( );
			acceptButton.Enabled = false;
		}
		
		void OnAcceptButtonClick( object sender, EventArgs e )
		{
			parentControl.Change_Resource_Content( contentTextBox.Text );
			acceptButton.Enabled = false;
		}
		
		void OnContentTextBoxTextChanged( Object sender, EventArgs e )
		{
			acceptButton.Enabled = true;
		}
		
		protected override void OnSizeChanged( EventArgs e )
		{
			contentTextBox.Size = new Size( Width - 10, Height - 40 );
			base.OnSizeChanged( e );
		}
	}
}


