// Authors:
//	Alexander Olk, <xenomorph2@onlinehome.de>

using System;
using System.Xml;

namespace FDOxml2cs
{
	public class SubMatchReader
	{
		XmlTextReader xtr;
		
		Match sm;
		
		public SubMatchReader( XmlTextReader xtr, Match sm )
		{
			this.xtr = xtr;
			
			this.sm = sm;
		}
		
		public void Start( )
		{
			if ( xtr.HasAttributes )
			{
				sm.MatchValue = xtr.GetAttribute( "value" );
				
				string match_type = xtr.GetAttribute( "type" );
				
				if ( match_type == "string" )
					sm.MatchType = MatchTypes.TypeString;
				else
				if ( match_type == "host16" )
					sm.MatchType = MatchTypes.TypeHost16;
				else
				if ( match_type == "host32" )
					sm.MatchType = MatchTypes.TypeHost32;
				else
				if ( match_type == "big16" )
					sm.MatchType = MatchTypes.TypeBig16;
				else
				if ( match_type == "big32" )
					sm.MatchType = MatchTypes.TypeBig32;
				else
				if ( match_type == "little16" )
					sm.MatchType = MatchTypes.TypeLittle16;
				else
				if ( match_type == "little32" )
					sm.MatchType = MatchTypes.TypeLittle32;
				else
				if ( match_type == "byte" )
					sm.MatchType = MatchTypes.TypeByte;
				
				string offset = xtr.GetAttribute( "offset" );
				
				if ( offset.IndexOf( ":" ) != -1 )
				{
					string[] split = offset.Split( new char[] { ':' } );
					
					if ( split.Length == 2 )
					{
						sm.Offset = System.Convert.ToInt32( split[ 0 ] );
						sm.OffsetEnd = System.Convert.ToInt32( split[ 1 ] );
					}
					
				}
				else
					sm.Offset = System.Convert.ToInt32( offset );
				
				string mask = xtr.GetAttribute( "mask" );
				
				if ( mask != "" )
					sm.Mask = mask;
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
							
							sm.Matches.Add( nm );
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
