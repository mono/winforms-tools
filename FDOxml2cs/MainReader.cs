// Authors:
//	Alexander Olk, <xenomorph2@onlinehome.de>

using System;
using System.IO;
using System.Xml;

namespace FDOxml2cs
{
	public class MainReader
	{
		public static bool MainReaderSuccess = false;
		
		string path;
		XmlTextReader xtr;
		
		public MainReader( string path )
		{
			this.path = path;
		}
		
		public void Start( )
		{
			DirectoryInfo directoryInfo = new DirectoryInfo( path );
			
			FileInfo[] files = directoryInfo.GetFiles( );
			
			foreach ( FileInfo fi in files )
			{
				if ( fi.Extension.ToUpper( ) == ".XML" )
				{
					if ( ReadXMLFile( fi ) )
					{
						MainReaderSuccess = true;
						
						Console.WriteLine( fi.Name + " success..." );
					}
				}
			}
		}
		
		private bool ReadXMLFile( FileInfo fi )
		{
			xtr = new XmlTextReader( fi.FullName );
			
			if ( !CheckIfMimeFileIsCorrect( ) )
			{
				xtr.Close( );
				Console.WriteLine( fi.Name + " doesn't seem to be a correct freedesktop shared mime info file..." );
				return false;
			}
			
			Console.WriteLine( fi.Name + " seems to be a correct freedesktop shared mime info file..." );
			Console.WriteLine( "Start parsing..." );
			
			while ( xtr.Read( ) )
			{
				switch ( xtr.NodeType )
				{
					case XmlNodeType.Element:
						if ( xtr.Name == "mime-type" )
						{
							MimeType mt = new MimeType( );
							
							MimeTypeReader mtr = new MimeTypeReader( xtr, mt );
							
							mtr.Start( );
							
							if ( !MimeUtils.CheckIfMimetypeExists( mt ) )
								MimeUtils.mimeTypes.Add( mt );
						}
						break;
						
					case XmlNodeType.EndElement:
						if ( xtr.Name == "mime-info" )
							break;
						break;
				}
			}
			
			xtr.Close( );
			
			return true;
		}
		
		private bool CheckIfMimeFileIsCorrect( )
		{
			while ( xtr.Read( ) )
			{
				switch ( xtr.NodeType )
				{
					case XmlNodeType.Element:
						if ( xtr.Name == "mime-info" )
						{
							if ( xtr.MoveToFirstAttribute( ) )
							{
								if ( xtr.Name == "xmlns" )
								{
									if ( xtr.Value == "http://www.freedesktop.org/standards/shared-mime-info" )
										return true;
								}
							}
						}
						break;
				}
			}
			
			return false;
		}
	}
}
