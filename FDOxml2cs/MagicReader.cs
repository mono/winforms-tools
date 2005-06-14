// Authors:
//	Alexander Olk, <xenomorph2@onlinehome.de>

using System;
using System.Xml;

namespace FDOxml2cs
{
	public class MagicReader
	{
		XmlTextReader xtr;
		
		MimeType mt;
		
		public MagicReader( XmlTextReader xtr, MimeType mt )
		{
			this.xtr = xtr;
			this.mt = mt;
		}
		
		public void Start( )
		{
			if ( xtr.HasAttributes )
			{
				mt.MagicPriority = System.Convert.ToInt32( xtr.GetAttribute( "priority" ) );
			}
			
			while ( xtr.Read( ) )
			{
				switch ( xtr.NodeType )
				{
					case XmlNodeType.Element:
						if ( xtr.Name == "match" )
						{
							Match m = new Match( );
							
							MatchReader mr = new MatchReader( xtr, m );
							
							mr.Start( );
							
							mt.Matches.Add( m );
						}
						break;
						
					case XmlNodeType.EndElement:
						if ( xtr.Name == "magic" )
							return;
						break;
						
				}
			}
		}
	}
}
