using System;
using System.Drawing;

namespace MWFResourceEditor
{
	public struct ContentStruct
	{
		public Image image;
		
		public string text;
		
		public ContentType ctype;
		
		public ContentStruct Clone( )
		{
			return (ContentStruct)MemberwiseClone( );
		}
	}
}





