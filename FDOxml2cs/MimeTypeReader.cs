// Authors:
//	Alexander Olk, <xenomorph2@onlinehome.de>

using System;
using System.Xml;

namespace FDOxml2cs
{
	public class MimeTypeReader
	{
		XmlTextReader xtr;
		
		MimeType mt;
		
		public MimeTypeReader( XmlTextReader xtr, MimeType mt )
		{
			this.xtr = xtr;
			this.mt = mt;
		}
		
		public void Start( )
		{
			if ( xtr.HasAttributes )
			{
				mt.TypeName = xtr.GetAttribute( "type" );
			}
			
			while ( xtr.Read( ) )
			{
				switch ( xtr.NodeType )
				{
					case XmlNodeType.Element:
						if ( xtr.Name == "sub-class-of" )
						{
							SubClassReader scr = new SubClassReader( xtr, mt );
							
							scr.Start( );
						}
						else
						if ( xtr.Name == "comment" )
						{
							CommentReader cr = new CommentReader( xtr, mt );
							
							cr.Start( );
						}
						else
						if ( xtr.Name == "glob" )
						{
							GlobReader gr = new GlobReader( xtr, mt );
							
							gr.Start( );
						}
						else
						if ( xtr.Name == "magic" )
						{
							MagicReader mr = new MagicReader( xtr, mt );
							
							mr.Start( );
						}
						else
						if ( xtr.Name == "alias" )
						{
							AliasReader ar = new AliasReader( xtr, mt );
							
							ar.Start( );
						}
						break;
						
					case XmlNodeType.EndElement:
						if ( xtr.Name == "mime-type" )
							return;
						break;
				}
			}
		}
	}
}
