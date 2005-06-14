// Authors:
//	Alexander Olk, <xenomorph2@onlinehome.de>

using System;
using System.Collections;

namespace FDOxml2cs
{
	public class MimeUtils
	{
		public static ArrayList mimeTypes = new ArrayList();
		
		public static bool CheckIfMimetypeExists( MimeType mt )
		{
			foreach ( MimeType m in mimeTypes )
			{
				if ( m.TypeName == mt.TypeName )
					return true;
			}
			
			return false;
		}
	}
}
