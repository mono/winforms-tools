// Authors:
//	Alexander Olk, <xenomorph2@onlinehome.de>

using System;
using System.Xml;

namespace FDOxml2cs
{
	public class SubClassReader
	{
		XmlTextReader xtr;
		
		MimeType mt;
		
		public SubClassReader( XmlTextReader xtr, MimeType mt )
		{
			this.xtr = xtr;
			this.mt = mt;
		}
		
		public void Start( )
		{
			if ( xtr.HasAttributes )
			{
				mt.SubClasses.Add( xtr.GetAttribute( "type" ) );
			}
		}
	}
}
