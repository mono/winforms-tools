// Authors:
//	Alexander Olk, <xenomorph2@onlinehome.de>

using System;
using System.Collections;
using System.Collections.Specialized;

namespace FDOxml2cs
{
	public class SourceWriter
	{
		StringCollection outputLines = new StringCollection();
		
		public SourceWriter( )
		{
			InitOutput( );
		}
		
		public StringCollection OutputLines
		{
			set {
				outputLines = value;
			}
			
			get {
				return outputLines;
			}
		}
		
		public void Start( )
		{
			GenerateCode( );
			
			EndOutput( );
		}
		
		private void GenerateCode( )
		{
			GenerateAliases( );
			
			outputLines.Add( "" );
			
			GenerateSubClasses( );
			
			outputLines.Add( "" );
			
			GenerateGlobals( );
			
			outputLines.Add( "" );
			outputLines.Add( "\t\t\tMimeType mt = null;" );
			outputLines.Add( "" );
			
			GenerateMimeTypes( );
			
			outputLines.Add( "\t\t\tMatch match0 = null;" );
			outputLines.Add( "" );
			
			GenerateMatches( );
		}
		
		private void GenerateMatches( )
		{
			foreach ( MimeType mt in MimeUtils.mimeTypes )
			{
				if ( mt.Matches.Count > 0 )
				{
					AddMatches( mt.Matches, mt, 0, null );
				}
			}
		}
		
		private void GenerateMimeTypes( )
		{
			foreach ( MimeType mt in MimeUtils.mimeTypes )
			{
				outputLines.Add( "\t\t\tmt = new MimeType();" );
				outputLines.Add( "\t\t\tmt.Comment = \"" + mt.Comment + "\";" );
				
				foreach ( DictionaryEntry de in mt.CommentsLanguage )
				{
					outputLines.Add( "\t\t\tmt.CommentsLanguage.Add( \"" + de.Key + "\", \"" + de.Value + "\" );" );
				}
				
				outputLines.Add( "\t\t\tMimeTypes.Add( \"" + mt.TypeName + "\", mt );" );
				
				outputLines.Add( "" );
			}
		}
		
		private void GenerateGlobals( )
		{
			foreach ( MimeType mt in MimeUtils.mimeTypes )
			{
				foreach ( string s in mt.GlobPatterns )
				{
					if ( s.IndexOf( "*" ) == -1 )
					{
						outputLines.Add( "\t\t\tGlobalLiterals.Add( \"" + s + "\", \"" + mt.TypeName + "\" );" );
					}
					else
					if ( s.IndexOf( "*." ) == -1 )
					{
						outputLines.Add( "\t\t\tGlobalSufPref.Add( \"" + s + "\", \"" + mt.TypeName + "\" );" );
					}
					else
					{
						string gp = s.Replace( "*", "" );
						
						string[] gpsplit = gp.Split( new char[] { '.' } );
						
						if ( gpsplit.Length > 2 )
							outputLines.Add( "\t\t\tGlobalPatternsLong.Add( \"" + gp + "\", \"" + mt.TypeName + "\" );" );
						else
							outputLines.Add( "\t\t\tGlobalPatternsShort.Add( \"" + gp + "\", \"" + mt.TypeName + "\" );" );
					}
				}
			}
		}
		
		private void GenerateSubClasses( )
		{
			foreach ( MimeType mt in MimeUtils.mimeTypes )
			{
				foreach ( string s in mt.SubClasses )
				{
					outputLines.Add( "\t\t\tSubClasses.Add( \"" + mt.TypeName + "\", \"" + s + "\" );" );
				}
			}
		}
		
		private void GenerateAliases( )
		{
			foreach ( MimeType mt in MimeUtils.mimeTypes )
			{
				foreach ( string s in mt.AliasTypes )
				{
					outputLines.Add( "\t\t\tAliases.Add( \"" + mt.TypeName + "\", \"" + s + "\" );" );
				}
			}
		}
		
		private void AddMatches( ArrayList matches, MimeType mimeType, int cookie, string lastmatchname )
		{
			foreach ( Match match in matches )
			{
				string matchname = "";
				
				string tabs = new String( '\t', 3 + cookie );
				
				if ( cookie == 0 )
				{
					outputLines.Add( tabs + "match0 = new Match();" );
					outputLines.Add( tabs + "match0.MimeType = \"" + mimeType.TypeName + "\";" );
					matchname = "match0";
				}
				else
				{
					matchname = "match" + cookie;
					if ( lastmatchname != null && lastmatchname != matchname )
						outputLines.Add( tabs + "Match " + matchname + " = null;" );
					
					lastmatchname = matchname;
					
					outputLines.Add( tabs + matchname + " = new Match();" );
				}
				
				if ( cookie == 0 )
					outputLines.Add( tabs + matchname + ".Priority = " + mimeType.MagicPriority + ";" );
				
				outputLines.Add( tabs + matchname + ".Offset = " + match.Offset + ";" );
				outputLines.Add( tabs + matchname + ".OffsetLength = " + ( match.OffsetEnd == -1 ? "1" : ( match.OffsetEnd - match.Offset ).ToString( ) ) + ";" );
				outputLines.Add( tabs + matchname + ".MatchType = MatchTypes." + match.MatchType + ";" );
				
				int word_size = -1;
				
				switch ( match.MatchType )
				{
					case MatchTypes.TypeHost16:
						word_size = 2;
						break;
					case MatchTypes.TypeHost32:
						word_size = 4;
						break;
					default:
						break;
				}
				
				if ( word_size != -1 )
				{
					outputLines.Add( tabs + matchname + ".WordSize = " + word_size + ";" );
				}
				
				if ( match.MatchType == MatchTypes.TypeString )
				{
					byte[] bmatchvalue = MatchValueStringToByteArray( match.MatchValue );
					
					BuildByteArrays( tabs, matchname, bmatchvalue, match );
				}
				else
				{
					byte[] bmatchvalue = MatchValueOtherToByteArray( match.MatchValue, match.MatchType );
					
					BuildByteArrays( tabs, matchname, bmatchvalue, match );
				}
				
				
				if ( match.Matches.Count != 0 )
				{
					cookie++;
					
					outputLines.Add( "" );
					outputLines.Add( tabs + "if ( " + matchname + ".Matches.Count > 0 )" );
					outputLines.Add( tabs + "{" );
					
					AddMatches( match.Matches, null, cookie, matchname );
					
					outputLines.Add( tabs + "}" );
					
					cookie--;
				}
				
				if ( cookie == 0 )
				{
					if ( mimeType.MagicPriority < 80 )
						outputLines.Add( "\t\t\tMatchesBelow80.Add( match0 );" );
					else
						outputLines.Add( "\t\t\tMatches80Plus.Add( match0 );" );
				}
				else
					outputLines.Add( tabs + "match" + ( cookie - 1 ).ToString( ) + ".Matches.Add( match" + cookie + " );" );
				
				outputLines.Add( "" );
			}
		}
		
		private void BuildByteArrays( string tabs, string matchname, byte[] bmatchvalue, Match match )
		{
			AddByteArray( tabs, matchname, bmatchvalue, 0 );
			
			if ( match.Mask != null )
			{
				byte[] bmask = HexStringToByteArray( match.Mask );
				
				AddByteArray( tabs, matchname, bmask, 1 );
			}
		}
		
		private void AddByteArray( string tabs, string matchname, byte[] bmatchvalue, int btype  ) // 0 == ByteValue, 1 == mask
		{
			string type_string = ".ByteValue";
			
			if ( btype == 1 )
				type_string = ".Mask";
			
			string outs = tabs + matchname + type_string + " = new byte[ " + bmatchvalue.Length + " ] { ";
			
			int counter = 0;
			
			foreach ( byte b in bmatchvalue )
			{
				if ( counter % 8 == 0 )
				{
					outs += "\n" + tabs + "\t\t";
				}
				
				outs += b.ToString( ) + ", ";
				
				counter++;
			}
			
			outs = outs.Remove( outs.Length - 2, 2 );
			
			outs += " };";
			
			outputLines.Add( outs );
		}
		
		private byte[] MatchValueOtherToByteArray( string match, MatchTypes matchType )
		{
			byte[] retval = null;
			
			// hex
			if ( match.StartsWith( "0x" ) )
			{
				retval = HexStringToByteArray( match );
			}
			else
			{
				if ( matchType == MatchTypes.TypeByte )
				{
					retval = new byte[ 1 ];
					
					retval[ 0 ] = System.Convert.ToByte( match );
				}
				else
				if ( matchType == MatchTypes.TypeHost16 || matchType == MatchTypes.TypeLittle16 )
				{
					retval = new byte[ 2 ];
					
				 	UInt16 i16 = System.Convert.ToUInt16( OctalStringToInt( match ) );
					
					retval[ 0 ] = System.Convert.ToByte( i16 & 0x00FF );
					retval[ 1 ] = System.Convert.ToByte( i16 >> 8 );
				}
				if ( matchType == MatchTypes.TypeBig16 )
				{
					retval = new byte[ 2 ];
					
				 	UInt16 i16 = System.Convert.ToUInt16( OctalStringToInt( match ) );
					
					retval[ 0 ] = System.Convert.ToByte( i16 >> 8 );
					retval[ 1 ] = System.Convert.ToByte( i16 & 0x00FF );
				}
				else
				if ( matchType == MatchTypes.TypeHost32 || matchType == MatchTypes.TypeLittle32 )
				{
					retval = new byte[ 4 ];
					
				 	UInt32 i32 = OctalStringToInt( match );
					
					retval[ 0 ] = System.Convert.ToByte( i32 & 0x000000FF );
					retval[ 1 ] = System.Convert.ToByte( ( i32 & 0x0000FF00 ) >> 8 );
					retval[ 2 ] = System.Convert.ToByte( ( i32 & 0x00FF0000 ) >> 16 );
					retval[ 3 ] = System.Convert.ToByte( ( i32 & 0xFF000000 ) >> 24 );
				}
				else
				if ( matchType == MatchTypes.TypeBig32 )
				{
					retval = new byte[ 4 ];
					
				 	UInt32 i32 = OctalStringToInt( match );
					
					retval[ 0 ] = System.Convert.ToByte( ( i32 & 0xFF000000 ) >> 24 );
					retval[ 1 ] = System.Convert.ToByte( ( i32 & 0x00FF0000 ) >> 16 );
					retval[ 2 ] = System.Convert.ToByte( ( i32 & 0x0000FF00 ) >> 8 );
					retval[ 3 ] = System.Convert.ToByte( i32 & 0x000000FF );
				}
			}
			
			return retval;
		}
		
		private uint OctalStringToInt( string input )
		{
			if ( input.Length == 0 )
				return 0;
			
			int result = input[ 0 ] - '0';
			
			if ( input.Length > 1 )
				for ( int i = 1; i < input.Length; i++ )
				{
					char c = input[ i ];
					result = 8 * result + ( c - '0' );
				}
			
			return (uint)result;
		}
		
		private byte[] HexStringToByteArray( string match )
		{
			ArrayList bytelist = new ArrayList( );
			
			string match_tmp = match.Replace( "0x", "" );
			
			for ( int i = 0; i < match_tmp.Length; i += 2 )
			{
				bytelist.Add( System.Convert.ToByte( match_tmp.Substring( i, 2 ), 16 ) );
			}
			
			byte[] retval = new byte[ bytelist.Count ];
			
			bytelist.CopyTo( retval );
			
			return retval;
		}
		
		private byte[] MatchValueStringToByteArray( string match )
		{
			ArrayList bytelist = new ArrayList( );
			
			int i = 0;
			int l = match.Length;
			
			while ( i < l )
			{
				char c = match[ i++ ];
				
				if ( c == '\\' )
				{
					if ( i == l )
					{
						bytelist.Add( System.Convert.ToByte( '\\' ) );
						break;
					}
					
					c = match[ i++ ];
					
					switch ( c )
					{
						case 'n':
							bytelist.Add( System.Convert.ToByte( '\n' ) );
							break;
						case 'r':
							bytelist.Add( System.Convert.ToByte( '\r' ) );
							break;
						case 'b':
							bytelist.Add( System.Convert.ToByte( '\b' ) );
							break;
						case 't':
							bytelist.Add( System.Convert.ToByte( '\t' ) );
							break;
						case 'f':
							bytelist.Add( System.Convert.ToByte( '\f' ) );
							break;
						case 'v':
							bytelist.Add( System.Convert.ToByte( '\v' ) );
							break;
							// octal
						case '0':
						case '1':
						case '2':
						case '3':
						case '4':
						case '5':
						case '6':
						case '7':
							int result = c - '0';
							
							if ( i < l )
							{
								c = match[ i++ ];
								
								if ( c >= '0' && c <= '7' )
								{
									result = 8 * result + ( c - '0' );
									
									if ( i < l )
									{
										c = match[ i++ ];
										
										if ( c >= '0' && c <= '7' )
										{
											result = 8 * result + ( c - '0' );
										}
										else
											i--;
									}
								}
								else
									i--;
							}
							
							bytelist.Add( System.Convert.ToByte( result ) );
							break;
							// hex
						case 'x':
							int byte_to_add = System.Convert.ToByte( 'x' );
							
							c = match[ i++ ];
							
							try
							{
								int tmp = System.Convert.ToByte( c.ToString( ), 16 );
								
								byte_to_add = tmp;
								
								if ( i < l )
								{
									c = match[ i++ ];
									
									try
									{
										tmp = System.Convert.ToByte( c.ToString( ), 16 );
										
										byte_to_add = ( byte_to_add << 4 ) + tmp;
									}
									catch ( System.FormatException )
									{
										i--;
									}
								}
							}
							catch ( System.FormatException )
							{
								i--;
							}
							
							bytelist.Add( System.Convert.ToByte( byte_to_add ) );
							
							break;
						default:
							bytelist.Add( System.Convert.ToByte( c ) );
							break;
					}
				}
				else
					bytelist.Add( System.Convert.ToByte( c ) );
			}
			
			byte[] retval = new byte[ bytelist.Count ];
			
			bytelist.CopyTo( retval );
			
			return retval;
		}
		
		private void InitOutput( )
		{
			string date = DateTime.Now.ToString( );
			
			string begin_str =
			
			"#region generated code " + date + "\n\n" +
			"using System;\n" +
			"using System.Collections;\n" +
			"using System.Collections.Specialized;\n\n" +
			"namespace " + FDOxml2csMain.nameSpace + "\n" +
			"{\n" +
			"\tinternal struct MimeGenerated\n" +
			"\t{\n" +
			"\t\tpublic static NameValueCollection Aliases = new NameValueCollection();\n" +
			"\t\tpublic static NameValueCollection SubClasses = new NameValueCollection();\n\n" +
			"\t\tpublic static NameValueCollection GlobalPatternsShort = new NameValueCollection();\n" +
			"\t\tpublic static NameValueCollection GlobalPatternsLong = new NameValueCollection();\n" +
			"\t\tpublic static NameValueCollection GlobalLiterals = new NameValueCollection();\n" +
			"\t\tpublic static NameValueCollection GlobalSufPref = new NameValueCollection();\n" +
			"\t\tpublic static Hashtable MimeTypes = new Hashtable();\n\n" +
			"\t\tpublic static ArrayList Matches80Plus = new ArrayList();\n" +
			"\t\tpublic static ArrayList MatchesBelow80 = new ArrayList();\n\n" +
			"\t\tpublic static void Init()\n" +
			"\t\t{\n";
			
			outputLines.Add( begin_str );
		}
		
		private void EndOutput( )
		{
			string end_str =
			
			"\t\t}\n" +
			"\t}\n" +
			"}\n\n" +
			"#endregion\n";
			
			outputLines.Add( end_str );
		}
		
		public void Flush( )
		{
			Console.WriteLine( "Writing \"MimeGenerated.cs\"..." );
			
			System.IO.StreamWriter sw = new System.IO.StreamWriter( "MimeGenerated.cs" );
			
			foreach ( string s in outputLines )
			{
				sw.WriteLine( s );
			}
			
			sw.Close( );
		}
	}
}

