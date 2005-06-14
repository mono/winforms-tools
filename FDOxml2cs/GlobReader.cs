// Authors:
//	Alexander Olk, <xenomorph2@onlinehome.de>

using System;
using System.Xml;

namespace FDOxml2cs
{
	public class GlobReader
	{
		XmlTextReader xtr;
		
		MimeType mt;
		
		public GlobReader( XmlTextReader xtr, MimeType mt )
		{
			this.xtr = xtr;
			this.mt = mt;
		}
		
		public void Start( )
		{
			if ( xtr.HasAttributes )
			{
//				string pattern = xtr.GetAttribute( "pattern" );
//
//				if ( pattern.StartsWith( "*" ) )
//					pattern = pattern.Replace( "*", "" );
				
				mt.GlobPatterns.Add( xtr.GetAttribute( "pattern" ) );
			}
		}
	}
}
