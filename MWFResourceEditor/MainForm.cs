using System;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;
using System.Resources;

using System.ComponentModel;

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
		private ListView resourceListView;
		private ColumnHeader nameColumnHeader;
		private ColumnHeader typeColumnHeader;
		private ColumnHeader contentColumnHeader;
		private ContextMenu contextMenu;
		
		private ResXResourceReader resXResourceReader;
		
		private ImageList imageList = new ImageList();
		
		private Hashtable hashtable = new Hashtable();
		
		private Panel activePanel;
		
		private ContentStruct contentStructCopy;
		private string itemNameCopy = "";
		
		private string fullFileName = "New Resource.resx";
		
		static int copyCounter = 1;
		
		public MainForm( )
		{
			InitializeComponent( );
		}
		
		public TextPanel TextPanel
		{
			set
			{
				textPanel = value;
			}
			
			get
			{
				return textPanel;
			}
		}
		
		private void InitializeComponent( )
		{
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
			
			nameColumnHeader = new ColumnHeader( );
			typeColumnHeader = new ColumnHeader( );
			contentColumnHeader = new ColumnHeader( );
			
			resourceListView = new ListView( );
			
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
//													 menuItemCut,
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
			
			// contentControl
			contentControl.Dock = DockStyle.Fill;
			contentControl.Location = new Point( 0, 328 );
			contentControl.Size = new Size( 592, 213 );
			contentControl.TabIndex = 3;
			contentControl.Controls.Add( textPanel );
			
			activePanel = textPanel;
			
			// textPanel
			textPanel.Dock = DockStyle.Fill;
			textPanel.Location = new Point( 0, 0 );
			textPanel.Size = new Size( 592, 213 );
			
			// imagePanel
			imagePanel.Dock = DockStyle.Fill;
			imagePanel.Location = new Point( 0, 0 );
			imagePanel.Size = new Size( 592, 213 );
			
			// nameColumnHeader
			nameColumnHeader.Text = "Name";
			nameColumnHeader.Width = 200;
			
			// typeColumnHeader
			typeColumnHeader.Text = "Type";
			typeColumnHeader.Width = 150;
			
			// contentColumnHeader
			contentColumnHeader.Text = "Content";
			contentColumnHeader.Width = 250;
			
			// resourceListView
			resourceListView.Columns.AddRange( new ColumnHeader[] {
								  nameColumnHeader,
								  typeColumnHeader,
								  contentColumnHeader} );
			resourceListView.Dock = DockStyle.Fill;
			resourceListView.GridLines = true;
			resourceListView.Location = new Point( 0, 0 );
			resourceListView.Size = new Size( 592, 328 );
			resourceListView.TabIndex = 0;
			resourceListView.View = View.Details;
			resourceListView.FullRowSelect = true;
			resourceListView.MultiSelect = false;
			resourceListView.LabelEdit = true;
			resourceListView.ContextMenu = contextMenu;
			
			// resourcePanel
			resourcePanel.Controls.Add( resourceListView );
			resourcePanel.Dock = DockStyle.Top;
			resourcePanel.Location = new Point( 0, 0 );
			resourcePanel.Size = new Size( 592, 328 );
			resourcePanel.TabIndex = 0;
			
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
			AutoScaleBaseSize = new Size( 5, 14 );
			ClientSize = new Size( 592, 541 );
			
			Controls.Add( contentControl );
			Controls.Add( resourcePanel );
			
			Menu = mainMenu;
			Text = "MWF ResourceEditor";
			contentControl.ResumeLayout( false );
			resourcePanel.ResumeLayout( false );
			ResumeLayout( false );
			
			resourceListView.SelectedIndexChanged += new EventHandler( OnResourceListViewSelectedIndexChanged );
		}
		
		void OnMenuItemNewClick( object sender, EventArgs e )
		{
			fullFileName = "New Resource.resx";
			
			Text = fullFileName;
			
			ResetListViewAndHashtable( );
		}
		
		private void ResetListViewAndHashtable( )
		{
			if ( resourceListView.Items.Count > 0 )
				resourceListView.Clear( );
			
			if ( hashtable.Count > 0 )
				hashtable.Clear( );
			
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
			ResetListViewAndHashtable( );
			
			OpenFileDialog ofd = new OpenFileDialog( );
			ofd.CheckFileExists = true;
			ofd.Filter = "resx files (*.resx)|*.resx|All files (*.*)|*.*";
			
			if ( DialogResult.OK == ofd.ShowDialog( ) )
			{
				resXResourceReader = new ResXResourceReader( ofd.FileName );
				
				string fileName = ofd.FileName;
				
				fullFileName = fileName;
				
				string[] split = fileName.Split( new Char[] { '\\', '/' } );
				
				if ( split.Length > 0 )
					fileName = split[ split.Length - 1 ];
				
				Text = fileName;
				
				FillListView( );
				
				resXResourceReader.Close( );
			}
		}
		
		void OnMenuItemSaveClick( object sender, EventArgs e )
		{
			if ( resourceListView.Items.Count == 0 )
				return;
			
			// disabled for now...
			// as soon as ResXResourceWriter is fixed it will be enabled again
			
			/*System.IO.File.Delete( fullFileName );
			 
			 ResXResourceWriter rxrw = new ResXResourceWriter( fullFileName );
			 
			 IDictionaryEnumerator ienumerator = hashtable.GetEnumerator();
			 
			 while ( ienumerator.MoveNext() )
			 {
			 ContentStruct cs = (ContentStruct)ienumerator.Value;
			 
			 if ( cs.ctype == ContentType.TypeImage )
			 {
			 using (System.IO.MemoryStream mem = new System.IO.MemoryStream())
			 {
			 cs.image.Save(mem, System.Drawing.Imaging.ImageFormat.Png);
			 
			 rxrw.AddResource( ienumerator.Key.ToString(), mem.ToArray() );
			 }
			 }
			 else
			 if ( cs.ctype == ContentType.TypeString )
			 rxrw.AddResource( ienumerator.Key.ToString(), cs.text );
			 }
			 
			 rxrw.Close();*/
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
				ContentStruct cs = new ContentStruct( );
				cs.text = new_item_string;
				cs.ctype = ContentType.TypeString;
				
				hashtable.Add( new_item_string, cs );
				
				ListViewItem listViewItem = new ListViewItem( new_item_string );
				
				listViewItem.SubItems.Add( "System.String" );
				listViewItem.SubItems.Add( new_item_string );
				
				resourceListView.BeginUpdate( );
				if ( resourceListView.Items.Count == 0 )
					resourceListView.Items.Add( listViewItem );
				else
					resourceListView.Items.Insert( 0, listViewItem );
				resourceListView.EndUpdate( );
			}
		}
		
		// currently only images...
		void OnMenuItemAddFilesClick( object sender, EventArgs e )
		{
			OpenFileDialog ofd = new OpenFileDialog( );
			ofd.CheckFileExists = true;
			ofd.Multiselect = true;
			
			ofd.Filter = "Images (*.png;*.jpg;*.gif;*.bmp)|*.png;*.jpg;*.gif;*.bmp|All files (*.*)|*.*";
			
			if ( DialogResult.OK == ofd.ShowDialog( ) )
			{
				foreach ( string s in ofd.FileNames )
				{
					try
					{
						Image image = Image.FromFile( s );
						
						string[] split = s.Split( new Char[] { '\\', '/' } );
						
						string imageName = "";
						
						if ( split.Length > 0 )
							imageName = split[ split.Length - 1 ];
						
						ContentStruct cs = new ContentStruct( );
						cs.image = image;
						cs.ctype = ContentType.TypeImage;
						
						imageList.Images.Add( image );
						
						hashtable.Add( imageName, cs );
						
						ListViewItem listViewItem = new ListViewItem( imageName );
						
						listViewItem.SubItems.Add( image.ToString( ) );
						listViewItem.SubItems.Add( GetImageSizeString( image ) );
						
						resourceListView.BeginUpdate( );
						if ( resourceListView.Items.Count == 0 )
							resourceListView.Items.Add( listViewItem );
						else
							resourceListView.Items.Insert( 0, listViewItem );
						resourceListView.EndUpdate( );
					}
					catch (Exception ex)
					{
						Console.WriteLine( "File {0} not found.", s );
					}
				}
			}
		}
		
		void OnMenuItemDeleteClick( object sender, EventArgs e )
		{
			if ( resourceListView.SelectedItems.Count == 0 )
				return;
			
			hashtable.Remove( resourceListView.SelectedItems[ 0 ].Text );
			
			resourceListView.SelectedItems[ 0 ].Remove( );
		}
		
		void OnMenuItemCopyClick( object sender, EventArgs e )
		{
			if ( resourceListView.SelectedItems.Count == 0 )
				return;
			
			itemNameCopy = resourceListView.SelectedItems[ 0 ].Text;
			
			ContentStruct cs = (ContentStruct)hashtable[ itemNameCopy ];
			
			contentStructCopy = cs.Clone( );
			
			BuildNameWithNumber( );
		}
		
		private void BuildNameWithNumber( )
		{
			string itemNameTest = itemNameCopy + "_" + copyCounter.ToString( );
			
			while ( hashtable.ContainsKey( itemNameTest ) )
			{
				copyCounter++;
				itemNameTest = itemNameCopy + "_" + copyCounter.ToString( );
			}
			
			itemNameCopy = itemNameTest;
		}
		
//		void OnMenuItemCutClick( object sender, EventArgs e )
//		{
//			if ( resourceListView.SelectedItems.Count == 0 )
//				return;
//		}
		
		void OnMenuItemPasteClick( object sender, EventArgs e )
		{
			if ( itemNameCopy == "" )
				return;
			
			hashtable.Add( itemNameCopy, contentStructCopy );
			
			ListViewItem listViewItem = new ListViewItem( itemNameCopy );
			
			if ( contentStructCopy.ctype == ContentType.TypeString )
			{
				listViewItem.SubItems.Add( "System.String" );
				listViewItem.SubItems.Add( contentStructCopy.text );
			}
			else
			if ( contentStructCopy.ctype == ContentType.TypeImage )
			{
				listViewItem.SubItems.Add( contentStructCopy.image.ToString( ) );
				listViewItem.SubItems.Add( GetImageSizeString( contentStructCopy.image ) );
			}
			
			resourceListView.BeginUpdate( );
			resourceListView.Items.Add( listViewItem );
			resourceListView.EndUpdate( );
		}
		
		void OnMenuItemRenameClick( object sender, EventArgs e )
		{
			if ( resourceListView.SelectedItems.Count == 0 )
				return;
			
			string item_name = resourceListView.SelectedItems[ 0 ].Text;
			
			string new_item_string = TextEntryDialog.Show( "Rename", "Enter new name for \"" + item_name + "\":" );
			
			if ( new_item_string.Length != 0 )
			{
				ContentStruct cs = (ContentStruct)hashtable[ item_name ];
				
				hashtable.Remove( item_name );
				
				hashtable.Add( new_item_string, cs );
				
				resourceListView.BeginUpdate( );
				resourceListView.SelectedItems[ 0 ].Text = new_item_string;
				resourceListView.EndUpdate( );
			}
		}
		
		void OnResourceListViewSelectedIndexChanged( object sender, EventArgs e )
		{
			if ( resourceListView.SelectedItems.Count == 0 )
				return;
			
			ContentStruct cs = (ContentStruct)hashtable[ resourceListView.SelectedItems[ 0 ].Text ];
			
			if ( cs.ctype == ContentType.TypeImage )
			{
				if ( activePanel == textPanel )
				{
					textPanel.Hide( );
					contentControl.Controls.Remove( textPanel );
					contentControl.Controls.Add( imagePanel );
					
					activePanel = imagePanel;
					imagePanel.Show( );
				}
				
				imagePanel.Image = cs.image;
			}
			else
			if ( cs.ctype == ContentType.TypeString )
			{
				if ( activePanel == imagePanel )
				{
					imagePanel.Hide( );
					contentControl.Controls.Remove( imagePanel );
					contentControl.Controls.Add( textPanel );
					
					activePanel = textPanel;
					textPanel.Show( );
				}
				
				textPanel.ContentTextBox.Text = cs.text;
			}
		}
		
		void OnMenuItemAboutClick( object sender, EventArgs e )
		{
			AboutDialog ad = new AboutDialog( );
			ad.ShowDialog( );
		}
		
		void FillListView( )
		{
			IDictionaryEnumerator id = resXResourceReader.GetEnumerator( );
			
			resourceListView.BeginUpdate( );
			
			foreach ( DictionaryEntry de in resXResourceReader )
			{
				ContentStruct cs = new ContentStruct( );
				
				string hashName = de.Key.ToString( );
				
				ListViewItem listViewItem = new ListViewItem( hashName );
				
				if ( de.Value.GetType( ) == typeof(Bitmap) )
				{
					listViewItem.SubItems.Add( de.Value.GetType( ).ToString( ) );
					
					Image image = (Image)de.Value;
					
					cs.ctype = ContentType.TypeImage;
					cs.image = image;
					
					string imagesize = GetImageSizeString( image );
					
					listViewItem.SubItems.Add( imagesize );
					
					imageList.Images.Add( image );
				}
				else
				if ( de.Value.GetType( ) == typeof(String) )
				{
					cs.image = null;
					cs.ctype = ContentType.TypeString;
					
					cs.text = de.Value.ToString( );
					
					listViewItem.SubItems.Add( de.Value.GetType( ).ToString( ) );
					listViewItem.SubItems.Add( de.Value.ToString( ) );
				}
				
				hashtable.Add( hashName, cs );
				resourceListView.Items.Add( listViewItem );
			}
			
			resourceListView.EndUpdate( );
		}
		
		private string GetImageSizeString( Image image )
		{
			string imagesize = "[Widht = ";
			imagesize += image.Width + ", Height = ";
			imagesize += image.Height + "]";
			return imagesize;
		}
		
		public void ChangeContentText( )
		{
			if ( resourceListView.SelectedItems.Count != 0 )
			{
				resourceListView.BeginUpdate( );
				
				string name = resourceListView.SelectedItems[ 0 ].Text;
				
				ContentStruct csnew = new ContentStruct( );
				
				csnew.ctype = ContentType.TypeString;
				csnew.text = textPanel.ContentTextBox.Text;
				
				hashtable.Remove( name );
				
				hashtable.Add( name, csnew );
				
				ListViewItem lvichange = resourceListView.Items[ resourceListView.Items.IndexOf( resourceListView.SelectedItems[ 0 ] ) ];
				
				lvichange.BeginEdit( );
				lvichange.SubItems[ 2 ].Text = textPanel.ContentTextBox.Text;
				
				resourceListView.EndUpdate( );
			}
		}
		
		public void ChangeContentImage( )
		{
			if ( resourceListView.SelectedItems.Count != 0 )
			{
				string imagename = resourceListView.SelectedItems[ 0 ].Text;
				
				ContentStruct csold = (ContentStruct)hashtable[ imagename ];
				
				ContentStruct csnew = new ContentStruct( );
				
				csnew.image = imagePanel.Image;
				csnew.ctype = ContentType.TypeImage;
				
				hashtable.Remove( imagename );
				
				hashtable.Add( imagename, csnew );
				
				if ( csold.image.Size != csnew.image.Size )
				{
					resourceListView.BeginUpdate( );
					
					ListViewItem lvichange = resourceListView.Items[ resourceListView.Items.IndexOf( resourceListView.SelectedItems[ 0 ] ) ];
					
					lvichange.BeginEdit( );
					lvichange.SubItems[ 2 ].Text = GetImageSizeString( csnew.image );
					
					resourceListView.EndUpdate( );
				}
			}
		}
		
		[STAThread]
		static void Main( )
		{
			Application.Run( new MainForm( ) );
		}
	}
}
