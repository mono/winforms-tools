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
//		private MenuItem menuItemCut;
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
		private Splitter splitter;
		
		private ResXResourceReader resXResourceReader;
		
		private Panel activePanel;
		
		private IResource resourceCopy;
		
		private string fullFileName = "New Resource.resx";
		
		public MainForm( )
		{
			InitializeComponent( );
		}
		
		private void InitializeComponent( )
		{
			splitter = new Splitter( );
			
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
//			menuItemCut = new MenuItem( );
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
//								menuItemCut,
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
			menuItemDash2.Index = 11;
			menuItemDash2.Text = "-";
			
			// menuItemDelete
			menuItemDelete.Index = 12;
			menuItemDelete.Text = "&Delete";
			menuItemDelete.Click += new EventHandler( OnMenuItemDeleteClick );
			
			// menuItemCopy
			menuItemCopy.Index = 13;
			menuItemCopy.Text = "&Copy";
			menuItemCopy.Click += new EventHandler( OnMenuItemCopyClick );
			
			// menuItemCut
//			menuItemCut.Index = 14;
//			menuItemCut.Text = "Cut";
//			menuItemCut.Click += new EventHandler( OnMenuItemCutClick );
			
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
			resourceListBox.Location = new Point( 0, 0 );
			resourceListBox.Size = new Size( 592, 328 );
			resourceListBox.Dock = DockStyle.Fill;
			resourceListBox.TabIndex = 0;
			resourceListBox.ContextMenu = contextMenu;
			
			// resourcePanel
			resourcePanel.Controls.Add( resourceListBox );
			resourcePanel.Dock = DockStyle.Top;
			resourcePanel.Location = new Point( 0, 0 );
			resourcePanel.Size = new Size( 592, 328 );
			resourcePanel.TabIndex = 0;
			resourcePanel.DockPadding.All = 5;
			
			// splitter
			splitter.Dock = DockStyle.Top;
			splitter.MinExtra = 213;
			splitter.MinSize = 328;
			
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
			Text = "MWF ResourceEditor";
			
			Controls.Add( contentControl );
			Controls.Add( splitter );
			Controls.Add( resourcePanel );
			
			contentControl.ResumeLayout( false );
			resourcePanel.ResumeLayout( false );
			
			ResumeLayout( false );
			
			resourceListBox.SelectedIndexChanged += new EventHandler( OnResourceListBoxSelectedIndexChanged );
		}
		
		void OnMenuItemNewClick( object sender, EventArgs e )
		{
			fullFileName = "New Resource.resx";
			
			Text = fullFileName;
			
			ResetListBox( );
		}
		
		private void ResetListBox( )
		{
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
			ResetListBox( );
			
			OpenFileDialog ofd = new OpenFileDialog( );
			ofd.CheckFileExists = true;
			ofd.Filter = "resx files (*.resx)|*.resx|All files (*.*)|*.*";
			
			if ( DialogResult.OK == ofd.ShowDialog( ) )
			{
				resXResourceReader = new ResXResourceReader( ofd.FileName );
				
				Text = Path.GetFileName( ofd.FileName );
				
				fullFileName = ofd.FileName;
				
				FillListBox( );
				
				resXResourceReader.Close( );
			}
		}
		
		void OnMenuItemSaveClick( object sender, EventArgs e )
		{
			if ( resourceListBox.Items.Count == 0 )
				return;
			
			File.Delete( fullFileName );
			
			WriteResourceFile( );
		}
		
		void OnMenuItemSaveAsClick( object sender, EventArgs e )
		{
			SaveFileDialog sfd = new SaveFileDialog( );
			sfd.CheckFileExists = true;
			sfd.DefaultExt = "resx";
			sfd.Filter = "resx files (*.resx)|*.resx|All files (*.*)|*.*";
			
			if ( DialogResult.OK == sfd.ShowDialog( ) )
			{
				fullFileName = sfd.FileName;
				
				Text = Path.GetFileName( fullFileName );
				
				WriteResourceFile( );
			}
		}
		
		private void WriteResourceFile( )
		{
			ResXResourceWriter rxrw = new ResXResourceWriter( fullFileName );
			
			while ( resourceListBox.Items.GetEnumerator( ).MoveNext( ) )
			{
				IResource res_abstract = (IResource)resourceListBox.Items.GetEnumerator( ).Current;
				
				switch ( res_abstract.ResourceType )
				{
					case ResourceType.TypeImage:
						ResourceImage resImage = (ResourceImage)res_abstract;
						
						if ( resImage.Image == null )
							continue;
						
						rxrw.AddResource( resImage.ResourceName, resImage.Image );
						break;
						
					case ResourceType.TypeString:
						ResourceString resStr = (ResourceString)res_abstract;
						
						if ( resStr == null )
							continue;
						
						rxrw.AddResource( resStr.ResourceName, resStr.Text );
						break;
						
					case ResourceType.TypeIcon:
						ResourceIcon resIcon = (ResourceIcon)res_abstract;
						
						if ( resIcon == null )
							continue;
						
						rxrw.AddResource( resIcon.ResourceName, resIcon.Icon );
						break;
						
					case ResourceType.TypeColor:
						ResourceColor resColor = (ResourceColor)res_abstract;
						
						if ( resColor == null )
							continue;
						
						rxrw.AddResource( resColor.ResourceName, resColor.Color );
						break;
						
					case ResourceType.TypeByteArray:
						ResourceByteArray resByteArray = (ResourceByteArray)res_abstract;
						
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
			Close( );
		}
		
		void OnMenuItemAddStringClick( object sender, EventArgs e )
		{
			string new_item_string = TextEntryDialog.Show( "New String", "Enter new string:" );
			
			if ( new_item_string.Length != 0 )
			{
				ResourceString resStr = new ResourceString( new_item_string, new_item_string );
				
				resourceListBox.BeginUpdate( );
				if ( resourceListBox.Items.Count == 0 )
					resourceListBox.AddResourceDirect( resStr );
				else
					resourceListBox.InsertResourceDirect( 0, resStr );
				resourceListBox.EndUpdate( );
				
				resourceListBox.SelectedIndex = 0;
				
				// FIXME: TopIndex has not the effect that is wanted here
				// bug in ListBox ???
				resourceListBox.TopIndex = 0;
				
				resourceListBox.Invalidate( );
			}
		}
		
		void OnMenuItemAddFilesClick( object sender, EventArgs e )
		{
			OpenFileDialog ofd = new OpenFileDialog( );
			ofd.CheckFileExists = true;
			ofd.Multiselect = true;
			
			ofd.Filter = "Images (*.png;*.jpg;*.gif;*.bmp)|*.png;*.jpg;*.gif;*.bmp|Icons (*.ico)|*.ico|All files (*.*)|*.*";
			
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
							ResourceIcon resIcon = new ResourceIcon( Path.GetFileName( file_name ), new Icon( file_name ) );
							
							resourceListBox.BeginUpdate( );
							resourceListBox.AddResourceDirect( resIcon );
							resourceListBox.EndUpdate( );
						}
						else
						// images
						if ( upper_file_name.EndsWith( ".PNG" ) || upper_file_name.EndsWith( ".JPG" ) ||
						    upper_file_name.EndsWith( ".GIF" ) || upper_file_name.EndsWith( ".BMP" ) )
						{
							ResourceImage resImage = new ResourceImage( Path.GetFileName( file_name ), Image.FromFile( file_name )  );
							
							resourceListBox.BeginUpdate( );
							resourceListBox.AddResourceDirect( resImage );
							resourceListBox.EndUpdate( );
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
							
							ResourceByteArray resByteArray = new ResourceByteArray( Path.GetFileName( file_name ), bytes );
							
							resourceListBox.BeginUpdate( );
							resourceListBox.AddResourceDirect( resByteArray );
							resourceListBox.EndUpdate( );
						}
					}
					catch (Exception ex)
					{
						Console.WriteLine( "File {0} not found.", file_name );
					}
				}
				
				if ( ofd.FileNames.Length > 0 )
				{
					if ( resourceListBox.Items.Count > 0 )
					{
						resourceListBox.SelectedIndex = 0;
						
						// FIXME: TopIndex has not the effect that is wanted
						// bug in ListBox ???
						resourceListBox.TopIndex = 0;
						
						resourceListBox.Invalidate( );
					}
				}
			}
		}
		
		void OnMenuItemAddColorClick( object sender, EventArgs e )
		{
			ColorDialog cd = new ColorDialog( );
			
			if ( DialogResult.OK == cd.ShowDialog( ) )
			{
				string resource_name = resourceListBox.UniqueResourceName( "Color" );
				
				resourceListBox.AddResource( resource_name, cd.Color );
				
				resourceListBox.Invalidate( );
			}
		}
		
		void OnMenuItemDeleteClick( object sender, EventArgs e )
		{
			if ( resourceListBox.SelectedItems.Count == 0 )
				return;
			
			resourceListBox.RemoveResource( (IResource)resourceListBox.SelectedItems[ 0 ] );
			
			resourceListBox.Invalidate( );
		}
		
		void OnMenuItemCopyClick( object sender, EventArgs e )
		{
			if ( resourceListBox.SelectedItems.Count == 0 )
				return;
			
			IResource res_abstract = (IResource)resourceListBox.SelectedItems[ 0 ];
			
			resourceCopy = (IResource)res_abstract.Clone( );
			
			resourceCopy.ResourceName = resourceListBox.UniqueResourceName( resourceCopy.ResourceName );
		}
		
//		void OnMenuItemCutClick( object sender, EventArgs e )
//		{
//			if ( resourceListBox.SelectedItems.Count == 0 )
//				return;
//		}
		
		void OnMenuItemPasteClick( object sender, EventArgs e )
		{
			resourceListBox.AddResourceDirect( resourceCopy );
			
			resourceListBox.Invalidate( );
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
		
		void FillListBox( )
		{
			resourceListBox.BeginUpdate( );
			
			foreach ( DictionaryEntry de in resXResourceReader )
				resourceListBox.AddResource( de.Key.ToString( ), de.Value );
			
			resourceListBox.EndUpdate( );
			
			if ( resourceListBox.Items.Count > 0 )
				resourceListBox.SelectedIndex = 0;
		}
		
		public void ChangeResourceContent( )
		{
			if ( resourceListBox.SelectedItems.Count != 0 )
			{
				resourceListBox.BeginUpdate( );
				
				IResource oldRes = (IResource)resourceListBox.SelectedItems[ 0 ];
				IResource newRes = null;
				
				switch ( oldRes.ResourceType )
				{
					case ResourceType.TypeString:
						ResourceString newresString = new ResourceString( oldRes.ResourceName, textPanel.ContentTextBox.Text );
						newRes = newresString;
						break;
						
					case ResourceType.TypeImage:
						ResourceImage newresImage = new ResourceImage( oldRes.ResourceName, imagePanel.Image );
						newRes = newresImage;
						break;
						
					case ResourceType.TypeIcon:
						ResourceIcon newresIcon = new ResourceIcon( oldRes.ResourceName, imagePanel.Icon );
						newRes = newresIcon;
						break;
						
					case ResourceType.TypeColor:
						ResourceColor newresColor = new ResourceColor( oldRes.ResourceName, colorPanel.Color );
						newRes = newresColor;
						break;
						
					default:
						break;
				}
				
				resourceListBox.ReplaceResource( oldRes, newRes );
				
				resourceListBox.EndUpdate( );
				
				resourceListBox.Invalidate( );
			}
		}
		
		[STAThread]
		static void Main( )
		{
			Application.Run( new MainForm( ) );
		}
	}
}

