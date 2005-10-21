using System;
using System.Drawing;
using System.Windows.Forms;
using System.Resources;
using System.IO;
using System.Collections;

namespace MWFResourceEditor
{
	public class ResourceControl : Control
	{
		public delegate void ChangeResourceContentDelegate( object new_resource_content );
		
		private ResourceSelectionControl resourceSelectionControl;
		private ResourceContentControl resourceContentControl;
		private Splitter mainSplitter;
		
		private ContextMenu contextMenu;
		
		private string fullFileName = "New Resource.resx";
		
		private bool fileSaved = true;
		
		private ResXResourceReader resXResourceReader;
		
		private string last_load_resx_directory = String.Empty;
		private string last_add_files_directory = String.Empty;
		private string last_save_as_directory = String.Empty;
		
		private bool first_time_saved = false;
		
		private IResource resourceCopy = null;
		
		private ResourceList resourceList = ResourceList.Instance;
		
		private StatusBar parentStatusBar;
		
		private int number_resources = 0;
		private string statusbar_file_name = "New Resource.resx";
		
		public ResourceControl( )
		{
			mainSplitter = new Splitter( );
			
			contextMenu = new ContextMenu( );
			
			resourceSelectionControl = new ResourceSelectionControl( this );
			
			resourceContentControl = new ResourceContentControl( this );
			
			SuspendLayout( );
			
			// resourceSelectionControl
			resourceSelectionControl.Dock = DockStyle.Top;
			resourceSelectionControl.Location = new Point( 0, 0 );
			resourceSelectionControl.Size = new Size( 592, 328 );
			resourceSelectionControl.TabIndex = 0;
//			resourceSelectionPanel.DockPadding.All = 5;
			
			// mainSplitter
			mainSplitter.Dock = DockStyle.Top;
			mainSplitter.MinExtra = 213;
			mainSplitter.MinSize = 328;
			
			// contentControl
			resourceContentControl.Location = new Point( 0, 328 );
			resourceContentControl.Size = new Size( 592, 213 );
			resourceContentControl.Dock = DockStyle.Fill;
			resourceContentControl.TabIndex = 3;
			
			Controls.Add( resourceContentControl );
			Controls.Add( mainSplitter );
			Controls.Add( resourceSelectionControl );
			
			ResumeLayout( false );
			
			resourceSelectionControl.ResourceListBox.SelectedIndexChanged += new EventHandler( OnResourceListBoxSelectedIndexChanged );
		}
		
		public bool FileSaved
		{
			set {
				fileSaved = value;
				
				string title = Parent.Text;
				
				if ( !fileSaved )
				{
					if ( !title.EndsWith( " *" ) )
					{
						title += " *";
						
						Parent.Text = title;
					}
				}
				else
				{
					if ( title.EndsWith( " *" ) )
					{
						title = title.Replace( " *", "" );
						
						Parent.Text = title;
					}
				}
				
				statusbar_file_name = title;
				UpdateStatusBar( );
			}
			
			get {
				return fileSaved;
			}
		}
		
		public ContextMenu InternalContextMenu
		{
			set {
				contextMenu = value;
			}
			
			get {
				return contextMenu;
			}
		}
		
		
		public StatusBar ParentStatusBar
		{
			set {
				parentStatusBar = value;
				UpdateStatusBar( );
			}
			
			get {
				return parentStatusBar;
			}
		}
		
		public void NewResource( )
		{
			fullFileName = "New Resource.resx";
			
			Text = fullFileName;
			
			FileSaved = false;
			
			first_time_saved = false;
			
			ClearResources( );
		}
		
		public void LoadResource( )
		{
			OpenFileDialog ofd = new OpenFileDialog( );
			ofd.CheckFileExists = true;
			ofd.Filter = "resx files (*.resx)|*.resx|All files (*.*)|*.*";
			
			if ( last_load_resx_directory != String.Empty )
				if ( Directory.Exists( last_load_resx_directory ) )
					ofd.InitialDirectory = last_load_resx_directory;
			
			if ( DialogResult.OK == ofd.ShowDialog( ) )
			{
				ClearResources( );
				
				resXResourceReader = new ResXResourceReader( ofd.FileName );
				
				Text = Path.GetFileName( ofd.FileName );
				
				fullFileName = ofd.FileName;
				
				last_load_resx_directory = Path.GetDirectoryName( ofd.FileName );
				
				number_resources = resourceSelectionControl.LoadResource( resXResourceReader );
				
				resXResourceReader.Close( );
				
				UpdateStatusBar( );
			}
		}
		
		public void SaveResource( )
		{
			if ( resourceList.Count == 0 )
				return;
			
			if ( !first_time_saved )
			{
				SaveResourceAs( );
				return;
			}
			
			File.Delete( fullFileName );
			
			WriteResourceFile( );
			
			FileSaved = true;
		}
		
		public void SaveResourceAs( )
		{
			if ( resourceList.Count == 0 )
				return;
			
			SaveFileDialog sfd = new SaveFileDialog( );
			sfd.CheckFileExists = true;
			sfd.DefaultExt = "resx";
			sfd.Filter = "resx files (*.resx)|*.resx|All files (*.*)|*.*";
			
			if ( last_save_as_directory == String.Empty )
				if ( last_load_resx_directory != String.Empty )
					last_save_as_directory = last_load_resx_directory;
			
			if ( last_save_as_directory != String.Empty )
				if ( Directory.Exists( last_save_as_directory ) )
					sfd.InitialDirectory = last_save_as_directory;
			
			if ( DialogResult.OK == sfd.ShowDialog( ) )
			{
				fullFileName = sfd.FileName;
				
				Text = Path.GetFileName( fullFileName );
				
				last_save_as_directory = Path.GetDirectoryName( sfd.FileName );
				
				if ( File.Exists( fullFileName ) )
					File.Delete( fullFileName );
				
				WriteResourceFile( );
				
				FileSaved = true;
				
				first_time_saved = true;
			}
		}
		
		public bool CanExit( )
		{
			if ( resourceList.Count == 0 || FileSaved )
				return true;
			else
			{
				string filename_short = Path.GetFileName( fullFileName );
				
				DialogResult result = MessageBox.Show( "File " + filename_short + " not saved.\n\n" + "Save ?",
								      "Exit...",
								      MessageBoxButtons.YesNoCancel,
								      MessageBoxIcon.Question );
				
				switch ( result )
				{
					case DialogResult.Yes:
						SaveResource( );
						if ( FileSaved )
							return true;
						break;
					case DialogResult.No:
						return true;
				}
			}
			
			return false;
		}
		
		public void AddString( )
		{
			string new_item_string = TextEntryDialog.Show( "New String", "Enter new string:" );
			
			if ( new_item_string.Length != 0 )
			{
				IResource resource = CreateNewResource( new_item_string, new_item_string );
				
				resourceSelectionControl.AddString( resource );
				
				number_resources++;
				FileSaved = false;
			}
		}
		
		public void AddFiles( )
		{
			OpenFileDialog ofd = new OpenFileDialog( );
			ofd.CheckFileExists = true;
			ofd.Multiselect = true;
			
			ofd.Filter = "Images (*.png;*.jpg;*.gif;*.bmp)|*.png;*.jpg;*.gif;*.bmp|Icons (*.ico)|*.ico|Cursors (*.cur)|*.cur|All files (*.*)|*.*";
			
			if ( last_add_files_directory != String.Empty )
				if ( Directory.Exists( last_add_files_directory ) )
					ofd.InitialDirectory = last_add_files_directory;
			
			if ( DialogResult.OK == ofd.ShowDialog( ) )
			{
				foreach ( string file_name in ofd.FileNames )
				{
					try
					{
						string upper_file_name = file_name.ToUpper( );
						
						// icon
						if ( upper_file_name.EndsWith( ".ICO" ) )
						{
							IResource resIcon = CreateNewResource( Path.GetFileName( file_name ), new Icon( file_name ) );
							
							resourceSelectionControl.AddResourceDirect( resIcon );
						}
						else if ( upper_file_name.EndsWith( ".CUR" ) )
						{
							IResource resCursor = CreateNewResource( Path.GetFileName( file_name ), new Cursor( file_name ) );
							
							resourceSelectionControl.AddResourceDirect( resCursor );
						}
						else
						// images
						if ( upper_file_name.EndsWith( ".PNG" ) || upper_file_name.EndsWith( ".JPG" ) ||
						    upper_file_name.EndsWith( ".GIF" ) || upper_file_name.EndsWith( ".BMP" ) )
						{
							IResource resImage = CreateNewResource( Path.GetFileName( file_name ), Image.FromFile( file_name )  );
							
							resourceSelectionControl.AddResourceDirect( resImage );
						}
						else
						// byte array
						{
							byte[] bytes = null;
							
							using ( FileStream fs = File.OpenRead( file_name ) )
							{
								bytes = new byte[ fs.Length ];
								
								fs.Read( bytes, 0, bytes.Length );
							}
							
							IResource resByteArray = CreateNewResource( Path.GetFileName( file_name ), bytes );
							
							resourceSelectionControl.AddResourceDirect( resByteArray );
						}
						
						number_resources++;
					}
					catch (Exception ex)
					{
						Console.WriteLine( "File {0} not found.", file_name );
						Console.WriteLine( ex );
					}
				}
				
				if ( ofd.FileNames.Length > 0 )
				{
					last_add_files_directory = Path.GetDirectoryName( ofd.FileNames[ 0 ] );
					
					resourceSelectionControl.EnsureVisible( );
					
					FileSaved = false;
				}
			}
		}
		
		public void AddColor( )
		{
			ColorDialog cd = new ColorDialog( );
			
			if ( DialogResult.OK == cd.ShowDialog( ) )
			{
				string resource_name = resourceList.UniqueResourceName( "Color" );
				
				IResource res_color = CreateNewResource( resource_name, cd.Color );
				
				resourceSelectionControl.AddResourceDirect( res_color );
				
				number_resources++;
				FileSaved = false;
			}
		}
		
		public void DeleteResource( )
		{
			resourceSelectionControl.RemoveResource( );
			
			number_resources--;
			FileSaved = false;
		}
		
		public void CopyResource( )
		{
			IResource resource = resourceSelectionControl.SelectedResourceListBoxItem;
			
			if (  resource != null )
				resourceCopy = CreateNewResource( resourceList.UniqueResourceName( resource.ResourceName ), resource.Value );
		}
		
		public void PasteResource( )
		{
			if ( resourceCopy == null )
				return;
			
			resourceSelectionControl.AddResourceDirect( resourceCopy );
			
			number_resources++;
			FileSaved = false;
		}
		
		public void RenameResource( )
		{
			IResource resource = resourceSelectionControl.SelectedResourceListBoxItem;
			
			if ( resource == null )
				return;
			
			string resource_name = resource.ResourceName;
			
			string new_resource_name = TextEntryDialog.Show( "Rename", "Enter new name for \"" + resource_name + "\":" );
			
			if ( new_resource_name.Length != 0 )
			{
				resourceSelectionControl.RenameResource( resource, new_resource_name );
				
				FileSaved = false;
			}
		}
		
		public void UpdateStatusBar( )
		{
			if ( parentStatusBar == null )
				return;
			
			parentStatusBar.Panels[ 0 ].Text = statusbar_file_name;
			parentStatusBar.Panels[ 1 ].Text = " Resource items: " + number_resources.ToString( );
		}
		
		void ClearResources( )
		{
			if ( resourceList.Count > 0 )
				resourceSelectionControl.ClearResources( );
			
			resourceContentControl.ClearResources( );
			resourceContentControl.ActivateTextPanel( );
			
			fullFileName = "New Resource.resx";
			statusbar_file_name = fullFileName;
			number_resources = 0;
			UpdateStatusBar( );
		}
		
		void WriteResourceFile( )
		{
			ResXResourceWriter rxrw = new ResXResourceWriter( fullFileName );
			
			foreach ( IResource res_abstract in resourceList.Items )
			{
				switch ( res_abstract.ResourceType )
				{
					case ResourceType.TypeImage:
						ResourceImage resImage = res_abstract as ResourceImage;
						
						if ( resImage.Image == null )
							continue;
						
						rxrw.AddResource( resImage.ResourceName, resImage.Image );
						break;
						
					case ResourceType.TypeString:
						ResourceString resStr = res_abstract as ResourceString;
						
						if ( resStr == null )
							continue;
						
						rxrw.AddResource( resStr.ResourceName, resStr.Text );
						break;
						
					case ResourceType.TypeCursor:
						ResourceCursor resCursor = res_abstract as ResourceCursor;
						
						if ( resCursor == null )
							continue;
						
						rxrw.AddResource( resCursor.ResourceName, resCursor.Cursor );
						break;
						
					case ResourceType.TypeIcon:
						ResourceIcon resIcon = res_abstract as ResourceIcon;
						
						if ( resIcon == null )
							continue;
						
						rxrw.AddResource( resIcon.ResourceName, resIcon.Icon );
						break;
						
					case ResourceType.TypeColor:
						ResourceColor resColor = res_abstract as ResourceColor;
						
						if ( resColor == null )
							continue;
						
						rxrw.AddResource( resColor.ResourceName, resColor.Color );
						break;
						
					case ResourceType.TypeByteArray:
						ResourceByteArray resByteArray = res_abstract as ResourceByteArray;
						
						if ( resByteArray == null )
							continue;
						
						rxrw.AddResource( resByteArray.ResourceName, resByteArray.ByteArray );
						break;
						
					default:
						break;
				}
			}
			
			rxrw.Close( );
		}
		
		void OnResourceListBoxSelectedIndexChanged( object sender, EventArgs e )
		{
			resourceContentControl.ClearActivePanel( );
			
			IResource resource = resourceSelectionControl.SelectedResourceListBoxItem;
			
			if ( resource == null )
				return;
			
			switch ( resource.ResourceType )
			{
				case ResourceType.TypeImage:
					resourceContentControl.ActivateImagePanel( );
					
					ResourceImage resImage = resource as ResourceImage;
					resourceContentControl.ImagePanel.Image = resImage.Image;
					break;
					
				case ResourceType.TypeString:
					resourceContentControl.ActivateTextPanel( );
					
					ResourceString resString = resource as ResourceString;
					resourceContentControl.TextPanel.ResourceText = resString.Text;
					break;
					
				case ResourceType.TypeCursor:
					resourceContentControl.ActivateImagePanel( );
					
					ResourceCursor resCursor = resource as ResourceCursor;
					resourceContentControl.ImagePanel.Cursor = resCursor.Cursor;
					break;
					
				case ResourceType.TypeIcon:
					resourceContentControl.ActivateImagePanel( );
					
					ResourceIcon resIcon = resource as ResourceIcon;
					resourceContentControl.ImagePanel.Icon = resIcon.Icon;
					break;
					
				case ResourceType.TypeColor:
					resourceContentControl.ActivateColorPanel( );
					
					ResourceColor resColor = (ResourceColor)resource;
					resourceContentControl.ColorPanel.Color = resColor.Color;
					break;
					
				case ResourceType.TypeByteArray:
					resourceContentControl.ActivateByteArrayPanel( );
					
					ResourceByteArray resByteArray = (ResourceByteArray)resource;
					resourceContentControl.ByteArrayPanel.ByteArray = resByteArray.ByteArray;
					break;
					
				default:
					break;
			}
		}
		
		public void ChangeResourceContent( object new_resource_value )
		{
			IResource oldRes = resourceSelectionControl.SelectedResourceListBoxItem;
			
			if ( oldRes == null )
				return;
			
			IResource newRes = CreateNewResource( oldRes.ResourceName, new_resource_value );
			
			resourceSelectionControl.ReplaceResource( oldRes, newRes );
			
			FileSaved = false;
		}
		
		private IResource CreateNewResource( string name, object content )
		{
			Type new_type;
			
			if ( content is Bitmap )
				new_type = typeof( ResourceImage );
			else
			if ( content is String )
				new_type = typeof( ResourceString );
			else
			if ( content is Icon )
				new_type = typeof( ResourceIcon );
			else
			if ( content is Cursor )
				new_type = typeof( ResourceCursor );
			else
			if ( content is Color )
				new_type = typeof( ResourceColor );
			else
				new_type = typeof( ResourceByteArray );
			
			IResource resource = Activator.CreateInstance( new_type, new object[] { name, content } ) as IResource;
			
			return resource;
		}
	}
}
