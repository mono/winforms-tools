using System;
using System.Drawing;
using System.Windows.Forms;

namespace MWFResourceEditor
{
	public class ResourceContentControl : Control
	{
//		delegate void ActivePanelChangedDelegate( IPanel panel );
//		ActivePanelChangedDelegate activePanelChanged;
		
		private ImagePanel imagePanel;
		private TextPanel textPanel;
		private ColorPanel colorPanel;
		private ByteArrayPanel byteArrayPanel;
		
		private Panel activePanel;
		
		public ResourceControl.ChangeResourceContentDelegate change_Resource_Content;
		
		public ResourceContentControl( ResourceControl resourceControl )
		{
			imagePanel = new ImagePanel( this );
			textPanel = new TextPanel( this );
			colorPanel = new ColorPanel( this );
			byteArrayPanel = new ByteArrayPanel( this );
			
			SuspendLayout( );
			
			activePanel = textPanel;
			
			Controls.Add( textPanel );
			
			ResumeLayout( false );
			
			change_Resource_Content = new ResourceControl.ChangeResourceContentDelegate( resourceControl.ChangeResourceContent );
		}
		
		public Panel ActivePanel
		{
			set {
				activePanel = value;
			}
			get {
				return activePanel;
			}
		}
		
		
		public TextPanel TextPanel
		{
			get {
				return textPanel;
			}
		}
		
		public ImagePanel ImagePanel
		{
			get {
				return imagePanel;
			}
		}
		
		public ColorPanel ColorPanel
		{
			get {
				return colorPanel;
			}
		}
		
		public ByteArrayPanel ByteArrayPanel
		{
			get {
				return byteArrayPanel;
			}
		}
		
		public ResourceControl.ChangeResourceContentDelegate Change_Resource_Content
		{
			get {
				return change_Resource_Content;
			}
		}
		
		public void ActivateTextPanel( )
		{
			if ( activePanel != textPanel )
			{
				activePanel.Hide( );
				Controls.Remove( activePanel );
				Controls.Add( textPanel );
				
				activePanel = textPanel;
				textPanel.Show( );
			}
		}
		
		public void ActivateImagePanel( )
		{
			if ( activePanel != imagePanel )
			{
				activePanel.Hide( );
				Controls.Remove( activePanel );
				Controls.Add( imagePanel );
				
				ActivePanel = imagePanel;
				imagePanel.Show( );
			}
		}
		
		public void ActivateColorPanel( )
		{
			if ( activePanel != colorPanel )
			{
				activePanel.Hide( );
				Controls.Remove( activePanel );
				Controls.Add( colorPanel );
				
				ActivePanel = colorPanel;
				colorPanel.Show( );
			}
		}
		
		public void ActivateByteArrayPanel( )
		{
			if ( activePanel != byteArrayPanel )
			{
				activePanel.Hide( );
				Controls.Remove( activePanel );
				Controls.Add( byteArrayPanel );
				
				ActivePanel = byteArrayPanel;
				byteArrayPanel.Show( );
			}
		}
		
		public void ClearActivePanel( )
		{
			IPanel ipanel = activePanel as IPanel;
			ipanel.ClearResource( );
		}
		
		public void ClearResources( )
		{
			foreach ( Control control in Controls )
			{
				if ( control is IPanel )
				{
					( (IPanel)control ).ClearResource( );
				}
			}
		}
	}
}


