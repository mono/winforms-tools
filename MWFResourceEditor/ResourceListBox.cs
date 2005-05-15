// Authors:
//	Alexander Olk, <xenomorph2@onlinehome.de>

using System;
using System.Drawing;
using System.Windows.Forms;

namespace MWFResourceEditor
{
	public class ResourceListBox : ListBox
	{
		public ResourceListBox( )
		{
			Size = new Size( 500, 300 );
			DrawMode = DrawMode.OwnerDrawFixed;
			ItemHeight = 53;
			BackColor = Color.Beige ;
		}
		
		protected override void OnDrawItem( DrawItemEventArgs e )
		{
			if ( e.Index != -1 )
			{
				IResourceRenderer renderer = (IResourceRenderer)Items[ e.Index ];
				
				if ( ( e.State & DrawItemState.Selected ) == DrawItemState.Selected )
					e.Graphics.FillRectangle( new SolidBrush( Color.LightBlue ), e.Bounds );
				else
					e.Graphics.FillRectangle( new SolidBrush( Color.Beige ), e.Bounds );
				
				e.Graphics.DrawImage( renderer.RenderContent, e.Bounds.X + 3, e.Bounds.Y + 1 );
				
				e.Graphics.DrawLine( Pens.LightBlue, e.Bounds.X, e.Bounds.Y - 1, e.Bounds.Width - 1 , e.Bounds.Y - 1 );
			}
		}
	}
}


