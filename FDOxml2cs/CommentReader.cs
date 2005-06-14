// Authors:
//	Alexander Olk, <xenomorph2@onlinehome.de>

using System;
using System.Xml;

namespace FDOxml2cs
{
	public class CommentReader
	{
		XmlTextReader xtr;
		
		MimeType mt;
		
		public CommentReader( XmlTextReader xtr, MimeType mt )
		{
			this.xtr = xtr;
			this.mt = mt;
		}
		
		public void Start( )
		{
			bool hasLanguageAttribute = false;
			
			string language = "";
			
			if ( xtr.HasAttributes )
			{
				hasLanguageAttribute = true;
				
				language = xtr.GetAttribute( "xml:lang" );
			}
			
			xtr.Read( );
			
			if ( !hasLanguageAttribute )
			{
				string comment = xtr.Value;
				
				if ( comment.IndexOf( "\"" ) != -1 )
					comment = comment.Replace( "\"", "\\\"" );
				
				mt.Comment = comment;
			}
			// currently disabled, need to look for some weird chars in the xml comments
			// System.Convert ?
//			else
//			{
//				string comment = xtr.Value;
//
//				if ( comment.IndexOf ( "\"" ) != -1 )
//					comment = comment.Replace( "\"", "\\\"" );
//
//				mt.CommentsLanguage.Add( language, comment );
//			}
		}
	}
}
