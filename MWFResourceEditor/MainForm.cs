// Authors:
//	Alexander Olk, <xenomorph2@onlinehome.de>

using System;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;
using System.Resources;
using System.IO;

namespace MWFResourceEditor
{
	public class MainForm : Form
	{
		private MainMenu mainMenu;
		private MenuItem menuItemFile;
		private MenuItem menuItemNew;
		private MenuItem menuItemLoad;
		private MenuItem menuItemSave;
		private MenuItem menuItemSaveAs;
		private MenuItem menuItemDash1;
		private MenuItem menuItemExit;
		private MenuItem menuItemResources;
		private MenuItem menuItemAddString;
		private MenuItem menuItemAddFiles;
		private MenuItem menuItemAddColor;
		private MenuItem menuItemCopy;
		private MenuItem menuItemDelete;
		private MenuItem menuItemPaste;
		private MenuItem menuItemDash3;
		private MenuItem menuItemRename;
		private MenuItem menuItemHelp;
		private MenuItem menuItemAbout;
		private MenuItem menuItemDash2;
		private Panel resourcePanel;
		private Control contentControl;
		private ImagePanel imagePanel;
		private TextPanel textPanel;
		private ColorPanel colorPanel;
		private ByteArrayPanel byteArrayPanel;
		private ResourceListBox resourceListBox;
		private ContextMenu contextMenu;
		private Splitter mainSplitter;
		private Splitter resourceSplitter;
		private ResourceTreeView resourceTreeView;
		
		private ResXResourceReader resXResourceReader;
		
		private Panel activePanel;
		
		private IResource resourceCopy = null;
		
		private string fullFileName = "New Resource.resx";
		
		private string last_load_resx_directory = String.Empty;
		private string last_add_files_directory = String.Empty;
		private string last_save_as_directory = String.Empty;
		
		private bool fileSaved = true;
		private bool first_time_saved = false;
		
		public MainForm( )
		{
			InitializeComponent( );
		}
		
		private void InitializeComponent( )
		{
			mainSplitter = new Splitter( );
			resourceSplitter = new Splitter( );
			
			menuItemFile = new MenuItem( );
			menuItemNew = new MenuItem( );
			menuItemLoad = new MenuItem( );
			menuItemSave = new MenuItem( );
			menuItemSaveAs = new MenuItem( );
			menuItemDash1 = new MenuItem( );
			menuItemExit = new MenuItem( );
			
			menuItemResources = new MenuItem( );
			menuItemAddString = new MenuItem( );
			menuItemAddFiles = new MenuItem( );
			menuItemAddColor = new MenuItem( );
			menuItemDash2 = new MenuItem( );
			menuItemDelete = new MenuItem( );
			menuItemCopy = new MenuItem( );
			menuItemPaste = new MenuItem( );
			menuItemDash3 = new MenuItem( );
			menuItemRename = new MenuItem( );
			
			menuItemHelp = new MenuItem( );
			menuItemAbout = new MenuItem( );
			
			mainMenu = new MainMenu( );
			
			contextMenu = new ContextMenu( );
			
			imagePanel = new ImagePanel( this );
			textPanel = new TextPanel( this );
			colorPanel = new ColorPanel( this );
			byteArrayPanel = new ByteArrayPanel( this );
			resourcePanel = new Panel( );
			
			resourceListBox = new ResourceListBox( );
			resourceTreeView = new ResourceTreeView( resourceListBox );
			resourceListBox.ResourceTreeView = resourceTreeView;
			
			contentControl = new Control( );
			contentControl.SuspendLayout( );
			resourcePanel.SuspendLayout( );
			SuspendLayout( );
			
			// menuItemFile
			menuItemFile.Index = 0;
			menuItemFile.MenuItems.AddRange( new MenuItem[] {
								menuItemNew,
								menuItemLoad,
								menuItemSave,
								menuItemSaveAs,
								menuItemDash1,
								menuItemExit} );
			menuItemFile.Text = "&File";
			
			// menuItemNew
			menuItemNew.Index = 1;
			menuItemNew.Text = "&New";
			menuItemNew.Click += new EventHandler( OnMenuItemNewClick );
			
			// menuItemLoad
			menuItemLoad.Index = 2;
			menuItemLoad.Text = "L&oad";
			menuItemLoad.Click += new EventHandler( OnMenuItemLoadClick );
			
			// menuItemSave
			menuItemSave.Index = 3;
			menuItemSave.Text = "&Save";
			menuItemSave.Click += new EventHandler( OnMenuItemSaveClick );
			
			// menuItemSaveAs
			menuItemSaveAs.Index = 4;
			menuItemSaveAs.Text = "S&ave as";
			menuItemSaveAs.Click += new EventHandler( OnMenuItemSaveAsClick );
			
			// menuItemDash
			menuItemDash1.Index = 5;
			menuItemDash1.Text = "-";
			
			// menuItemExit
			menuItemExit.Index = 6;
			menuItemExit.Text = "E&xit";
			menuItemExit.Click += new EventHandler( OnMenuItemExitClick );
			
			// menuItemResources
			menuItemResources.Index = 7;
			menuItemResources.MenuItems.AddRange( new MenuItem[] {
								     menuItemAddString,
								     menuItemAddFiles,
								     menuItemAddColor,
								     menuItemDash2,
								     menuItemDelete,
								     menuItemCopy,
								     menuItemPaste,
								     menuItemRename } );
			menuItemResources.Text = "Resources";
			
			// menuItemAddString
			menuItemAddString.Index = 8;
			menuItemAddString.Text = "Add S&tring";
			menuItemAddString.Click += new EventHandler( OnMenuItemAddStringClick );
			
			// menuItemAddFiles
			menuItemAddFiles.Index = 9;
			menuItemAddFiles.Text = "Add Fi&le(s)";
			menuItemAddFiles.Click += new EventHandler( OnMenuItemAddFilesClick );
			
			// menuItemAddColor
			menuItemAddColor.Index = 10;
			menuItemAddColor.Text = "Add Colo&r";
			menuItemAddColor.Click += new EventHandler( OnMenuItemAddColorClick );
			
			// menuItemDash2
			menuItemDash2.Index = 12;
			menuItemDash2.Text = "-";
			
			// menuItemDelete
			menuItemDelete.Index = 13;
			menuItemDelete.Text = "&Delete";
			menuItemDelete.Click += new EventHandler( OnMenuItemDeleteClick );
			
			// menuItemCopy
			menuItemCopy.Index = 14;
			menuItemCopy.Text = "&Copy";
			menuItemCopy.Click += new EventHandler( OnMenuItemCopyClick );
			
			// menuItemPaste
			menuItemPaste.Index = 15;
			menuItemPaste.Text = "Paste";
			menuItemPaste.Click += new EventHandler( OnMenuItemPasteClick );
			
			menuItemDash3.Index = 16;
			menuItemDash3.Text = "-";
			
			menuItemRename.Index = 17;
			menuItemRename.Text = "Rename";
			menuItemRename.Click += new EventHandler( OnMenuItemRenameClick );
			
			// menuItemHelp
			menuItemHelp.Index = 18;
			menuItemHelp.MenuItems.AddRange( new MenuItem[] {
								menuItemAbout} );
			menuItemHelp.Text = "&Help";
			
			// menuItemAbout
			menuItemAbout.Index = 19;
			menuItemAbout.Text = "A&bout";
			menuItemAbout.Click += new EventHandler( OnMenuItemAboutClick );
			
			// mainMenu
			mainMenu.MenuItems.AddRange( new MenuItem[] {
							    menuItemFile,
							    menuItemResources,
							    menuItemHelp} );
			
			activePanel = textPanel;
			
			// textPanel
//			textPanel.Location = new Point( 0, 0 );
//			textPanel.Size = new Size( 592, 213 );
			
			// resourceListBox
			resourceListBox.Size = new Size( 592, 328 );
			resourceListBox.Dock = DockStyle.Fill;
			resourceListBox.TabIndex = 0;
			resourceListBox.ContextMenu = contextMenu;
			
			// resourceTreeView
			resourceTreeView.Dock = DockStyle.Left;
			resourceTreeView.Size = new Size( 150, 328 );
			
			// resourcePanel
			resourcePanel.Controls.Add( resourceListBox );
			resourcePanel.Controls.Add( resourceSplitter );
			resourcePanel.Controls.Add( resourceTreeView );
			resourcePanel.Dock = DockStyle.Top;
			resourcePanel.Location = new Point( 0, 0 );
			resourcePanel.Size = new Size( 592, 328 );
			resourcePanel.TabIndex = 0;
			resourcePanel.DockPadding.All = 5;
			
			// mainSplitter
			mainSplitter.Dock = DockStyle.Top;
			mainSplitter.MinExtra = 213;
			mainSplitter.MinSize = 328;
			
			// resourceSplitter
			resourceSplitter.Dock = DockStyle.Left;
			resourceSplitter.MinExtra = 150;
			resourceSplitter.MinSize = 150;
			
			// contentControl
			contentControl.Location = new Point( 0, 328 );
			contentControl.Size = new Size( 592, 213 );
			contentControl.Dock = DockStyle.Fill;
			contentControl.TabIndex = 3;
			contentControl.Controls.Add( textPanel );
			
			// contextMenu
			contextMenu.MenuItems.AddRange( new MenuItem[] {
							       menuItemAddString.CloneMenu( ),
							       menuItemAddFiles.CloneMenu( ),
							       menuItemAddColor.CloneMenu( ),
							       menuItemDash2.CloneMenu( ),
							       menuItemDelete.CloneMenu( ),
							       menuItemCopy.CloneMenu( ),
							       menuItemPaste.CloneMenu( ),
							       menuItemRename.CloneMenu( )
						       } );
			
			// MainForm
//			AutoScaleBaseSize = new Size( 5, 14 );
			ClientSize = new Size( 592, 541 );
			
			Menu = mainMenu;
			Text = "New Resource.resx";
			
			Controls.Add( contentControl );
			Controls.Add( mainSplitter );
			Controls.Add( resourcePanel );
			
			contentControl.ResumeLayout( false );
			resourcePanel.ResumeLayout( false );
			
			ResumeLayout( false );
			
			resourceListBox.SelectedIndexChanged += new EventHandler( OnResourceListBoxSelectedIndexChanged );
		}
		
		public bool FileSaved
		{
			set {
				fileSaved = value;
				
				if ( !fileSaved )
				{
					string title = Text;
					
					if ( !title.EndsWith( " *" ) )
					{
						title += " *";
						
						Text = title;
					}
				}
				else
				{
					string title = Text;
					
					if ( title.EndsWith( " *" ) )
					{
						title = title.Replace( " *", "" );
						
						Text = title;
					}
				}
			}
			
			get {
				return fileSaved;
			}
		}
		
		void OnMenuItemNewClick( object sender, EventArgs e )
		{
			fullFileName = "New Resource.resx";
			
			Text = fullFileName;
			
			FileSaved = false;
			
			first_time_saved = false;
			
			ResetListBoxAndTreeView( );
		}
		
		private void ResetListBoxAndTreeView( )
		{
			resourceTreeView.ClearResources( );
			
			if ( resourceListBox.Items.Count > 0 )
				resourceListBox.ClearResources( );
			
			if ( activePanel != textPanel )
			{
				activePanel.Hide( );
				contentControl.Controls.Remove( activePanel );
				contentControl.Controls.Add( textPanel );
				
				activePanel = textPanel;
				textPanel.ContentTextBox.Clear( );
				textPanel.Show( );
			}
		}
		
		void OnMenuItemLoadClick( object sender, EventArgs e )
		{
			OpenFileDialog ofd = new OpenFileDialog( );
			ofd.CheckFileExists = true;
			ofd.Filter = "resx files (*.resx)|*.resx|All files (*.*)|*.*";
			
			if ( last_load_resx_directory != String.Empty )
				if ( Directory.Exists( last_load_resx_directory ) )
					ofd.InitialDirectory = last_load_resx_directory;
			
			if ( DialogResult.OK == ofd.ShowDialog( ) )
			{
				ResetListBoxAndTreeView( );
				
				resXResourceReader = new ResXResourceReader( ofd.FileName );
				
				Text = Path.GetFileName( ofd.FileName );
				
				fullFileName = ofd.FileName;
				
				last_load_resx_directory = Path.GetDirectoryName( ofd.FileName );
				
				FillListBoxAndTreeView( );
				
				resXResourceReader.Close( );
			}
		}
		
		void OnMenuItemSaveClick( object sender, EventArgs e )
		{
			if ( resourceListBox.Items.Count == 0 )
				return;
			
			if ( !first_time_saved )
			{
				OnMenuItemSaveAsClick( sender, e );
				return;
			}
			
			File.Delete( fullFileName );
			
			WriteResourceFile( );
			
			FileSaved = true;
		}
		
		void OnMenuItemSaveAsClick( object sender, EventArgs e )
		{
			if ( resourceListBox.Items.Count == 0 )
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
		
		private void WriteResourceFile( )
		{
			ResXResourceWriter rxrw = new ResXResourceWriter( fullFileName );
			
			foreach ( IResource res_abstract in resourceListBox.AllItems )
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
		
		void OnMenuItemExitClick( object sender, EventArgs e )
		{
			if ( resourceListBox.Items.Count == 0 || FileSaved )
				Close( );
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
						OnMenuItemSaveClick( null, EventArgs.Empty );
						if ( FileSaved )
							Close( );
						break;
					case DialogResult.No:
						Close( );
						break;
					default:
						break;
				}
			}
		}
		
		void OnMenuItemAddStringClick( object sender, EventArgs e )
		{
			string new_item_string = TextEntryDialog.Show( "New String", "Enter new string:" );
			
			if ( new_item_string.Length != 0 )
			{
				IResource resource = CreateNewResource( new_item_string, new_item_string );
				
				if ( resourceListBox.Items.Count == 0 )
					resourceListBox.AddResourceDirect( resource );
				else
					resourceListBox.InsertResourceDirect( 0, resource );
				
				resourceListBox.SelectedIndex = 0;
				
				// FIXME: TopIndex has not the effect that is wanted here
				// bug in ListBox ???
				resourceListBox.TopIndex = 0;
				
				resourceListBox.Invalidate( );
				
				FileSaved = false;
			}
		}
		
		void OnMenuItemAddFilesClick( object sender, EventArgs e )
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
							
							resourceListBox.AddResourceDirect( resIcon );
						}
						else if ( upper_file_name.EndsWith( ".CUR" ) )
						{
							IResource resCursor = CreateNewResource( Path.GetFileName( file_name ), new Cursor( file_name ) );
							
							resourceListBox.AddResourceDirect( resCursor );
						}
						else
						// images
						if ( upper_file_name.EndsWith( ".PNG" ) || upper_file_name.EndsWith( ".JPG" ) ||
						    upper_file_name.EndsWith( ".GIF" ) || upper_file_name.EndsWith( ".BMP" ) )
						{
							IResource resImage = CreateNewResource( Path.GetFileName( file_name ), Image.FromFile( file_name )  );
							
							resourceListBox.AddResourceDirect( resImage );
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
							
							resourceListBox.AddResourceDirect( resByteArray );
						}
					}
					catch (Exception ex)
					{
						Console.WriteLine( "File {0} not found.", file_name );
					}
				}
				
				if ( ofd.FileNames.Length > 0 )
				{
					last_add_files_directory = Path.GetDirectoryName( ofd.FileNames[ 0 ] );
					
					if ( resourceListBox.Items.Count > 0 )
					{
						resourceListBox.SelectedIndex = 0;
						
						// FIXME: TopIndex has not the effect that is wanted
						// bug in ListBox ???
						resourceListBox.TopIndex = 0;
						
						resourceListBox.Invalidate( );
					}
					
					FileSaved = false;
				}
			}
		}
		
		void OnMenuItemAddColorClick( object sender, EventArgs e )
		{
			ColorDialog cd = new ColorDialog( );
			
			if ( DialogResult.OK == cd.ShowDialog( ) )
			{
				string resource_name = resourceListBox.UniqueResourceName( "Color" );
				
				IResource res_color = CreateNewResource( resource_name, cd.Color );
				
				resourceListBox.AddResourceDirect( res_color );
				
				resourceListBox.Invalidate( );
				
				FileSaved = false;
			}
		}
		
		void OnMenuItemDeleteClick( object sender, EventArgs e )
		{
			if ( resourceListBox.SelectedItems.Count == 0 )
				return;
			
			resourceListBox.RemoveResource( (IResource)resourceListBox.SelectedItems[ 0 ] );
			
			resourceListBox.Invalidate( );
			
			FileSaved = false;
		}
		
		void OnMenuItemCopyClick( object sender, EventArgs e )
		{
			if ( resourceListBox.SelectedItems.Count == 0 )
				return;
			
			IResource res_abstract = (IResource)resourceListBox.SelectedItems[ 0 ];
			
			resourceCopy = CreateNewResource( resourceListBox.UniqueResourceName( res_abstract.ResourceName ), res_abstract.Value );
		}
		
		void OnMenuItemPasteClick( object sender, EventArgs e )
		{
			if ( resourceCopy == null )
				return;
			
			resourceListBox.AddResourceDirect( resourceCopy );
			
			resourceListBox.Invalidate( );
			
			FileSaved = false;
		}
		
		void OnMenuItemRenameClick( object sender, EventArgs e )
		{
			if ( resourceListBox.SelectedItems.Count == 0 )
				return;
			
			IResource resource = (IResource)resourceListBox.SelectedItems[ 0 ];
			string resource_name = resource.ResourceName;
			
			string new_resource_name = TextEntryDialog.Show( "Rename", "Enter new name for \"" + resource_name + "\":" );
			
			if ( new_resource_name.Length != 0 )
			{
				resourceListBox.RenameResource( resource, new_resource_name );
				
				resourceListBox.Invalidate( );
				
				FileSaved = false;
			}
		}
		
		void OnResourceListBoxSelectedIndexChanged( object sender, EventArgs e )
		{
			if ( resourceListBox.SelectedItems.Count == 0 )
				return;
			
			IResource resource = (IResource)resourceListBox.SelectedItems[ 0 ];
			
			switch ( resource.ResourceType )
			{
				case ResourceType.TypeImage:
					if ( activePanel != imagePanel )
					{
						activePanel.Hide( );
						contentControl.Controls.Remove( activePanel );
						contentControl.Controls.Add( imagePanel );
						
						activePanel = imagePanel;
						imagePanel.Show( );
					}
					
					ResourceImage resImage = (ResourceImage)resource;
					imagePanel.Image = resImage.Image;
					break;
					
				case ResourceType.TypeString:
					if ( activePanel != textPanel )
					{
						activePanel.Hide( );
						contentControl.Controls.Remove( activePanel );
						contentControl.Controls.Add( textPanel );
						
						activePanel = textPanel;
						textPanel.Show( );
					}
					
					ResourceString resString = (ResourceString)resource;
					textPanel.ContentTextBox.Text = resString.Text;
					break;
					
				case ResourceType.TypeCursor:
					if ( activePanel != imagePanel )
					{
						activePanel.Hide( );
						contentControl.Controls.Remove( activePanel );
						contentControl.Controls.Add( imagePanel );
						
						activePanel = imagePanel;
						imagePanel.Show( );
					}
					
					ResourceCursor resCursor = (ResourceCursor)resource;
					imagePanel.Image = resCursor.RenderContent;
					break;
					
					
				case ResourceType.TypeIcon:
					if ( activePanel != imagePanel )
					{
						activePanel.Hide( );
						contentControl.Controls.Remove( activePanel );
						contentControl.Controls.Add( imagePanel );
						
						activePanel = imagePanel;
						imagePanel.Show( );
					}
					
					ResourceIcon resIcon = (ResourceIcon)resource;
					imagePanel.Icon = resIcon.Icon;
					break;
					
				case ResourceType.TypeColor:
					if ( activePanel != colorPanel )
					{
						activePanel.Hide( );
						contentControl.Controls.Remove( activePanel );
						contentControl.Controls.Add( colorPanel );
						
						activePanel = colorPanel;
						colorPanel.Show( );
					}
					
					ResourceColor resColor = (ResourceColor)resource;
					colorPanel.Color = resColor.Color;
					break;
					
				case ResourceType.TypeByteArray:
					if ( activePanel != byteArrayPanel )
					{
						activePanel.Hide( );
						contentControl.Controls.Remove( activePanel );
						contentControl.Controls.Add( byteArrayPanel );
						
						activePanel = byteArrayPanel;
						byteArrayPanel.Show( );
					}
					
					ResourceByteArray resByteArray = (ResourceByteArray)resource;
					byteArrayPanel.ByteArray = resByteArray.ByteArray;
					break;
					
				default:
					break;
			}
		}
		
		void OnMenuItemAboutClick( object sender, EventArgs e )
		{
			AboutDialog ad = new AboutDialog( );
			ad.ShowDialog( );
		}
		
		void FillListBoxAndTreeView( )
		{
			resourceListBox.BeginUpdate( );
			
			foreach ( DictionaryEntry de in resXResourceReader )
				resourceListBox.AddResource( de.Key.ToString( ), de.Value );
			
			resourceListBox.EndUpdate( );
			
			resourceTreeView.Fill( );
			
			resourceListBox.ShowNode( ResourceType.All );
			
			if ( resourceListBox.Items.Count > 0 )
				resourceListBox.SelectedIndex = 0;
		}
		
		public void ChangeResourceContent( )
		{
			if ( resourceListBox.SelectedItems.Count != 0 )
			{
				resourceListBox.BeginUpdate( );
				
				IResource oldRes = (IResource)resourceListBox.SelectedItems[ 0 ];
				IResource newRes = CreateNewResource( oldRes.ResourceName, ( (IPanel)activePanel ).Value );
				
				resourceListBox.ReplaceResource( oldRes, newRes );
				
				resourceListBox.EndUpdate( );
				
				resourceListBox.Invalidate( );
				
				FileSaved = false;
			}
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
		
		[STAThread]
		static void Main( )
		{
			Application.Run( new MainForm( ) );
		}
	}
}

