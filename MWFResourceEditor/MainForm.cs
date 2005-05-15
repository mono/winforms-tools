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
		private ResourceListBox resourceListBox;
		private ContextMenu contextMenu;
		private Splitter splitter;
		
		private ResXResourceReader resXResourceReader;
		
		private Hashtable resourceHashtable = new Hashtable();
		
		private Panel activePanel;
		
		private IResource resourceCopy;
		
		private string itemNameCopy = "";
		
		private string fullFileName = "New Resource.resx";
		
		static int copyCounter = 1;
		
		public MainForm( )
		{
			InitializeComponent( );
		}
		
		public TextPanel TextPanel
		{
			set {
				textPanel = value;
			}
			
			get {
				return textPanel;
			}
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
			
			// menuItemDash2
			menuItemDash2.Index = 10;
			menuItemDash2.Text = "-";
			
			// menuItemDelete
			menuItemDelete.Index = 11;
			menuItemDelete.Text = "&Delete";
			menuItemDelete.Click += new EventHandler( OnMenuItemDeleteClick );
			
			// menuItemCopy
			menuItemCopy.Index = 12;
			menuItemCopy.Text = "&Copy";
			menuItemCopy.Click += new EventHandler( OnMenuItemCopyClick );
			
			// menuItemCut
//			menuItemCut.Index = 13;
//			menuItemCut.Text = "Cut";
//			menuItemCut.Click += new EventHandler( OnMenuItemCutClick );
			
			// menuItemPaste
			menuItemPaste.Index = 14;
			menuItemPaste.Text = "Paste";
			menuItemPaste.Click += new EventHandler( OnMenuItemPasteClick );
			
			menuItemDash3.Index = 15;
			menuItemDash3.Text = "-";
			
			menuItemRename.Index = 16;
			menuItemRename.Text = "Rename";
			menuItemRename.Click += new EventHandler( OnMenuItemRenameClick );
			
			// menuItemHelp
			menuItemHelp.Index = 17;
			menuItemHelp.MenuItems.AddRange( new MenuItem[] {
								menuItemAbout} );
			menuItemHelp.Text = "&Help";
			
			// menuItemAbout
			menuItemAbout.Index = 18;
			menuItemAbout.Text = "A&bout";
			menuItemAbout.Click += new EventHandler( OnMenuItemAboutClick );
			
			// mainMenu
			mainMenu.MenuItems.AddRange( new MenuItem[] {
							    menuItemFile,
							    menuItemResources,
							    menuItemHelp} );
			
			activePanel = textPanel;
			
			// textPanel
			textPanel.Location = new Point( 0, 0 );
			textPanel.Size = new Size( 592, 213 );
			
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
			
			ResetListBoxAndHashtable( );
		}
		
		private void ResetListBoxAndHashtable( )
		{
			if ( resourceListBox.Items.Count > 0 )
				resourceListBox.Items.Clear( );
			
			if ( resourceHashtable.Count > 0 )
				resourceHashtable.Clear( );
			
			if ( activePanel == imagePanel )
			{
				imagePanel.Hide( );
				contentControl.Controls.Remove( imagePanel );
				contentControl.Controls.Add( textPanel );
				
				activePanel = textPanel;
				textPanel.Show( );
			}
		}
		
		void OnMenuItemLoadClick( object sender, EventArgs e )
		{
			ResetListBoxAndHashtable( );
			
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
			
			IDictionaryEnumerator ienumerator = resourceHashtable.GetEnumerator( );
			
			while ( ienumerator.MoveNext( ) )
			{
				IResource res_abstract = (IResource)ienumerator.Value;
				
				switch ( res_abstract.ResourceType )
				{
					case ResourceType.TypeImage:
						ResourceImage resImage = (ResourceImage)res_abstract;
						
						if ( resImage.Image == null )
							continue;
						
						rxrw.AddResource( ienumerator.Key.ToString( ), resImage.Image );
						break;
						
					case ResourceType.TypeString:
						ResourceString resStr = (ResourceString)res_abstract;
						
						if ( resStr == null )
							continue;
						
						rxrw.AddResource( ienumerator.Key.ToString( ), resStr.Text );
						break;
						
					case ResourceType.TypeIcon:
						ResourceIcon resIcon = (ResourceIcon)res_abstract;
						
						if ( resIcon == null )
							continue;
						
						rxrw.AddResource( ienumerator.Key.ToString( ), resIcon.Icon );
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
				ResourceString resStr = new ResourceString( );
				
				resStr.ResourceName = new_item_string;
				resStr.Text = new_item_string;
				
				resourceHashtable.Add( new_item_string, resStr );
				
				resourceListBox.BeginUpdate( );
				if ( resourceListBox.Items.Count == 0 )
					resourceListBox.Items.Add( resStr );
				else
					resourceListBox.Items.Insert( 0, resStr );
				resourceListBox.EndUpdate( );
				
				resourceListBox.SelectedIndex = 0;
				
				// FIXME: TopIndex has not the effect that is wanted here
				// bug in ListBox ???
				resourceListBox.TopIndex = 0;
				
				resourceListBox.Refresh( );
			}
		}
		
		// currently only images and icons...
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
						
						// FIXME: Use serialazation ???
						
						string upper_file_name = file_name.ToUpper( );
						
						// icon
						if ( upper_file_name.EndsWith( ".ICO" ) )
						{
							ResourceIcon resIcon = new ResourceIcon( );
							
							string icon_name = Path.GetFileName( file_name );
							
							resIcon.ResourceName = icon_name;
							
							resIcon.Icon = new Icon( file_name );
							
							resourceHashtable.Add( icon_name, resIcon );
							
							resourceListBox.BeginUpdate( );
							if ( resourceListBox.Items.Count == 0 )
								resourceListBox.Items.Add( resIcon );
							else
								resourceListBox.Items.Insert( 0, resIcon );
							resourceListBox.EndUpdate( );
						}
						else
						{
							ResourceImage resImage = new ResourceImage( );
							
							string imageName = Path.GetFileName( file_name );
							
							resImage.ResourceName = imageName;
							
							resImage.Image = Image.FromFile( file_name );
							
							resourceHashtable.Add( imageName, resImage );
							
							resourceListBox.BeginUpdate( );
							if ( resourceListBox.Items.Count == 0 )
								resourceListBox.Items.Add( resImage );
							else
								resourceListBox.Items.Insert( 0, resImage );
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
					resourceListBox.SelectedIndex = 0;
					
					// FIXME: TopIndex has not the effect that is wanted
					// bug in ListBox ???
					resourceListBox.TopIndex = 0;
					resourceListBox.Refresh( );
				}
			}
		}
		
		void OnMenuItemDeleteClick( object sender, EventArgs e )
		{
			if ( resourceListBox.SelectedItems.Count == 0 )
				return;
			
			IResource resource = (IResource)resourceListBox.SelectedItems[ 0 ];
			
			switch ( resource.ResourceType )
			{
				case ResourceType.TypeIcon:
					ResourceIcon resIcon = (ResourceIcon)resource;
					resourceHashtable.Remove( resIcon );
					break;
				case ResourceType.TypeImage:
					ResourceImage resImage = (ResourceImage)resource;
					resourceHashtable.Remove( resImage );
					break;
				case ResourceType.TypeString:
					ResourceString resString = (ResourceString)resource;
					resourceHashtable.Remove( resString );
					break;
				default:
					break;
			}
			
			resourceListBox.Items.Remove( resource );
			
			resourceListBox.Refresh( );
		}
		
		void OnMenuItemCopyClick( object sender, EventArgs e )
		{
			if ( resourceListBox.SelectedItems.Count == 0 )
				return;
			
			itemNameCopy = ( (IResource)resourceListBox.SelectedItems[ 0 ] ).ResourceName;
			
			IResource res_abstract = (IResource)resourceHashtable[ itemNameCopy ];
			
			resourceCopy = (IResource)res_abstract.Clone( );
			
			BuildNameWithNumber( );
		}
		
		private void BuildNameWithNumber( )
		{
			string itemNameTest = itemNameCopy + "_" + copyCounter.ToString( );
			
			while ( resourceHashtable.ContainsKey( itemNameTest ) )
			{
				copyCounter++;
				itemNameTest = itemNameCopy + "_" + copyCounter.ToString( );
			}
			
			itemNameCopy = itemNameTest;
		}
		
//		void OnMenuItemCutClick( object sender, EventArgs e )
//		{
//			if ( resourceListBox.SelectedItems.Count == 0 )
//				return;
//		}
		
		void OnMenuItemPasteClick( object sender, EventArgs e )
		{
			if ( itemNameCopy == "" )
				return;
			
			resourceHashtable.Add( itemNameCopy, resourceCopy );
			
			switch ( resourceCopy.ResourceType )
			{
				case ResourceType.TypeString:
					ResourceString resString = resourceCopy as ResourceString;
					
					resString.ResourceName = itemNameCopy;
					
					resourceListBox.Items.Add( resString );
					break;
					
				case ResourceType.TypeImage:
					ResourceImage resImage = resourceCopy as ResourceImage;
					
					resImage.ResourceName = itemNameCopy;
					
					resourceListBox.Items.Add( resImage );
					break;
					
				case ResourceType.TypeIcon:
					ResourceIcon resIcon = resourceCopy as ResourceIcon;
					
					resIcon.ResourceName = itemNameCopy;
					
					resourceListBox.Items.Add( resIcon );
					break;
				default:
					break;
			}
			
			resourceListBox.Refresh( );
		}
		
		void OnMenuItemRenameClick( object sender, EventArgs e )
		{
			if ( resourceListBox.SelectedItems.Count == 0 )
				return;
			
			IResource resource = (IResource)resourceListBox.SelectedItems[ 0 ];
			string item_name = resource.ResourceName;
			
			string new_item_string = TextEntryDialog.Show( "Rename", "Enter new name for \"" + item_name + "\":" );
			
			if ( new_item_string.Length != 0 )
			{
				resourceHashtable.Remove( item_name );
				
				int index = resourceListBox.Items.IndexOf( resource );
				
				resourceListBox.Items.Remove( resource );
				
				resource.ResourceName = new_item_string;
				
				resourceHashtable.Add( new_item_string, resource );
				
				resourceListBox.Items.Insert( index, resource );
				
				resourceListBox.Refresh( );
			}
		}
		
		void OnResourceListBoxSelectedIndexChanged( object sender, EventArgs e )
		{
			if ( resourceListBox.SelectedItems.Count == 0 )
				return;
			
			IResource resource = (IResource)resourceListBox.SelectedItems[ 0 ];
			
			string key = resource.ResourceName;
			
			IResource res_abstract = (IResource)resourceHashtable[ key ];
			
			switch ( res_abstract.ResourceType )
			{
				case ResourceType.TypeImage:
					if ( activePanel == textPanel )
					{
						textPanel.Hide( );
						contentControl.Controls.Remove( textPanel );
						contentControl.Controls.Add( imagePanel );
						
						activePanel = imagePanel;
						imagePanel.Show( );
					}
					
					ResourceImage resImage = (ResourceImage)res_abstract;
					imagePanel.Image = resImage.Image;
					break;
					
				case ResourceType.TypeString:
					if ( activePanel == imagePanel )
					{
						imagePanel.Hide( );
						contentControl.Controls.Remove( imagePanel );
						contentControl.Controls.Add( textPanel );
						
						activePanel = textPanel;
						textPanel.Show( );
					}
					
					ResourceString resString = (ResourceString)res_abstract;
					textPanel.ContentTextBox.Text = resString.Text;
					break;
					
				case ResourceType.TypeIcon:
					if ( activePanel == textPanel )
					{
						textPanel.Hide( );
						contentControl.Controls.Remove( textPanel );
						contentControl.Controls.Add( imagePanel );
						
						activePanel = imagePanel;
						imagePanel.Show( );
					}
					
					ResourceIcon resIcon = (ResourceIcon)res_abstract;
					imagePanel.Icon = resIcon.Icon;
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
			{
				string hashName = de.Key.ToString( );
				
				if ( de.Value.GetType( ) == typeof(Bitmap) )
				{
					ResourceImage resImage = new ResourceImage( );
					
					resImage.ResourceName = hashName;
					resImage.Image = (Image)de.Value;
					
					resourceListBox.Items.Add( resImage );
					resourceHashtable.Add( hashName, resImage );
				}
				else
				if ( de.Value.GetType( ) == typeof(String) )
				{
					ResourceString resString = new ResourceString( );
					
					resString.ResourceName = hashName;
					resString.Text = de.Value.ToString( );
					
					resourceListBox.Items.Add( resString );
					resourceHashtable.Add( hashName, resString );
				}
				else
				if ( de.Value.GetType( ) == typeof( Icon ) )
				{
					ResourceIcon resIcon = new ResourceIcon( );
					
					resIcon.ResourceName = hashName;
					resIcon.Icon = (Icon)de.Value;
					
					resourceListBox.Items.Add( resIcon );
					resourceHashtable.Add( hashName, resIcon );
				}
			}
			
			resourceListBox.EndUpdate( );
			
			if ( resourceListBox.Items.Count > 0 )
				resourceListBox.SelectedIndex = 0;
		}
		
		public void ChangeStringResource( )
		{
			if ( resourceListBox.SelectedItems.Count != 0 )
			{
				resourceListBox.BeginUpdate( );
				
				IResource resource = (IResource)resourceListBox.SelectedItems[ 0 ];
				string name = resource.ResourceName;
				
				ResourceString oldresString = (ResourceString)resourceHashtable[ name ];
				
				ResourceString newresString = new ResourceString( );
				
				newresString.ResourceName = name;
				newresString.Text = textPanel.ContentTextBox.Text;
				
				resourceHashtable.Remove( name );
				
				resourceHashtable.Add( name, newresString );
				
				int index = resourceListBox.Items.IndexOf( oldresString );
				
				resourceListBox.Items.Remove( oldresString );
				
				resourceListBox.Items.Insert( index, newresString );
				
				resourceListBox.EndUpdate( );
				
				resourceListBox.Refresh( );
			}
		}
		
		public void ChangeImageResource( )
		{
			if ( resourceListBox.SelectedItems.Count != 0 )
			{
				resourceListBox.BeginUpdate( );
				
				IResource resource = (IResource)resourceListBox.SelectedItems[ 0 ];
				string imagename = resource.ResourceName;
				
				ResourceImage oldresImage = (ResourceImage)resourceHashtable[ imagename ];
				
				ResourceImage newresImage = new ResourceImage( );
				
				newresImage.ResourceName = oldresImage.ResourceName;
				newresImage.Image = imagePanel.Image;
				
				resourceHashtable.Remove( imagename );
				
				resourceHashtable.Add( imagename, newresImage );
				
				int index = resourceListBox.Items.IndexOf( oldresImage );
				
				resourceListBox.Items.Remove( oldresImage );
				
				resourceListBox.Items.Insert( index, newresImage );
				
				resourceListBox.EndUpdate( );
				
				resourceListBox.Refresh( );
			}
		}
		
		public void ChangeIconResource( )
		{
			if ( resourceListBox.SelectedItems.Count != 0 )
			{
				resourceListBox.BeginUpdate( );
				
				IResource resource = (IResource)resourceListBox.SelectedItems[ 0 ];
				string iconname = resource.ResourceName;
				
				ResourceIcon oldresIcon = (ResourceIcon)resourceHashtable[ iconname ];
				
				ResourceIcon newresIcon = new ResourceIcon( );
				
				newresIcon.ResourceName = oldresIcon.ResourceName;
				newresIcon.Icon = imagePanel.Icon;
				
				resourceHashtable.Remove( iconname );
				
				resourceHashtable.Add( iconname, newresIcon );
				
				int index = resourceListBox.Items.IndexOf( oldresIcon );
				
				resourceListBox.Items.Remove( oldresIcon );
				
				resourceListBox.Items.Insert( index, newresIcon );
				
				resourceListBox.EndUpdate( );
				
				resourceListBox.Refresh( );
			}
		}
		
		[STAThread]
		static void Main( )
		{
			Application.Run( new MainForm( ) );
		}
	}
}

