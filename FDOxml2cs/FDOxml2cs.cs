// Authors:
//	Alexander Olk, <xenomorph2@onlinehome.de>

// TODO: add command line parameter for output path

using System;
using System.IO;

namespace FDOxml2cs
{
	class FDOxml2csMain
	{
		public static string nameSpace = "System.Windows.Forms";
		
		static void PrintUsage( )
		{
			string usage =
			"The freedesktop.org.xml file can be found at:\nhttp://cvs.freedesktop.org/mime/shared-mime-info/\n\n" +
			"Usage: mono FDOxmls2cs.exe <--namespace NameSpace (optional)> <Freedesktop.org mime xml file(s) directory>\n\n" +
			"If you do not specify a directory FDOxmls2cs looks in the current working\ndirectory for the xml file(s).\n\n";
			
			Console.WriteLine( usage );
			
			System.Environment.Exit( 0 );
		}
		
		static void CheckArgs( string[] args, ref string directory )
		{
			if ( args[ 0 ] == "--help" )
			{
				PrintUsage( );
			}
			
			if ( args[ 0 ] == "--namespace" )
			{
				if ( args.Length == 1 )
				{
					Console.WriteLine( "\nError: Please specify a namespace\n" );
					
					System.Environment.Exit( 0 );
				}
				
				nameSpace = args[ 1 ];
				
				if ( args.Length == 3 )
				{
					directory = args[ 2 ];
				}
				
				return;
			}
			else
			{
				directory = args[ 0 ];
				
				if ( !Directory.Exists( directory ) )
				{
					Console.WriteLine( "Directory \"" + directory + "\" not found..." );
					
					System.Environment.Exit( 0 );
				}
			}
		}
		
		[STAThread]
		static void Main( string[] args )
		{
			Console.WriteLine( "FDOxml2cs - Convert Freedesktop.org mime xml files to C# source file." );
			Console.WriteLine( "\"mono FDOxml2cs.exe --help\" for help...\n" );
			
			string directory = Directory.GetCurrentDirectory( );
			
			if ( args.Length > 0 )
			{
				CheckArgs( args, ref directory );
			}
			
			MainReader mr = new MainReader( directory );
			
			mr.Start( );
			
			if ( MainReader.MainReaderSuccess )
			{
				SourceWriter sw = new SourceWriter( );
				
				sw.Start( );
				
				sw.Flush( );
				
				Console.WriteLine( "Success..." );
			}
			else
				Console.WriteLine( "No correct xml file found..." );
		}
	}
}
