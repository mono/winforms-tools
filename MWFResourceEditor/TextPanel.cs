using System;
using System.Drawing;
using System.Windows.Forms;

namespace MWFResourceEditor
{
	public class TextPanel : Panel
	{
		private TextBox contentTextBox;
		
		private MainForm parentForm;
		
		public TextPanel( MainForm parentForm )
		{
			this.parentForm = parentForm;
			
			contentTextBox = new TextBox( );
			
			SuspendLayout( );
			
			contentTextBox.Multiline = true;
			contentTextBox.Dock = DockStyle.Fill;
			contentTextBox.AcceptsReturn = true;
			contentTextBox.AcceptsTab = true;
			contentTextBox.ScrollBars = ScrollBars.Vertical | ScrollBars.Horizontal;
			
			Controls.Add( contentTextBox );
			
			contentTextBox.TextChanged += new EventHandler( OnContentTextBoxTextChanged );
			
			ResumeLayout( false );
		}
		
		public TextBox ContentTextBox
		{
			set
			{
				contentTextBox = value;
			}
			
			get
			{
				return contentTextBox;
			}
		}
		
		void OnContentTextBoxTextChanged( object sender, EventArgs e )
		{
			parentForm.ChangeContentText( );
		}
	}
}


