// Authors:
//	Alexander Olk, <xenomorph2@onlinehome.de>

using System;
using System.Xml;

namespace FDOxml2cs
{
	public class MatchReader
	{
		XmlTextReader xtr;
		
		Match m;
		
		public MatchReader( XmlTextReader xtr, Match m )
		{
			this.xtr = xtr;
			
			this.m = m;
		}
		
		public void Start( )
		{
			if ( xtr.HasAttributes )
			{
				m.MatchValue = xtr.GetAttribute( "value" );
				
				string match_type = xtr.GetAttribute( "type" );
				
				if ( match_type == "string" )
					m.MatchType = MatchTypes.TypeString;
				else
				if ( match_type == "host16" )
					m.MatchType = MatchTypes.TypeHost16;
				else
				if ( match_type == "host32" )
					m.MatchType = MatchTypes.TypeHost32;
				else
				if ( match_type == "big16" )
					m.MatchType = MatchTypes.TypeBig16;
				else
				if ( match_type == "big32" )
					m.MatchType = MatchTypes.TypeBig32;
				else
				if ( match_type == "little16" )
					m.MatchType = MatchTypes.TypeLittle16;
				else
				if ( match_type == "little32" )
					m.MatchType = MatchTypes.TypeLittle32;
				else
				if ( match_type == "byte" )
					m.MatchType = MatchTypes.TypeByte;
				
				string offset = xtr.GetAttribute( "offset" );
				
				if ( offset.IndexOf( ":" ) != -1 )
				{
					string[] split = offset.Split( new char[] { ':' } );
					
					if ( split.Length == 2 )
					{
						m.Offset = System.Convert.ToInt32( split[ 0 ] );
						m.OffsetEnd = System.Convert.ToInt32( split[ 1 ] );
					}
					
				}
				else
					m.Offset = System.Convert.ToInt32( offset );
				
				string mask = xtr.GetAttribute( "mask" );
				
				if ( mask != "" )
					m.Mask = mask;
			}
			
			if ( xtr.IsEmptyElement )
				return;
			
			while ( xtr.Read( ) )
			{
				switch ( xtr.NodeType )
				{
					case XmlNodeType.Element:
						if ( xtr.Name == "match" )
						{
							Match nm = new Match( );
							
							SubMatchReader mr = new SubMatchReader( xtr, nm );
							
							mr.Start( );
							
							m.Matches.Add( nm );
						}
						break;
						
					case XmlNodeType.EndElement:
						if ( xtr.Name == "match" )
							return;
						break;
						
				}
			}
		}
	}
}
