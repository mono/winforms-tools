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
		private StatusBar statusBar;
		
		private ResourceControl resourceControl;
		
		public MainForm( )
		{
			InitializeComponent( );
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
			
			resourceControl = new ResourceControl( );
			
			statusBar = new StatusBar( );
			
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
			menuItemNew.Index = 0;
			menuItemNew.Text = "&New";
			menuItemNew.Click += new EventHandler( OnMenuItemNewClick );
			
			// menuItemLoad
			menuItemLoad.Index = 1;
			menuItemLoad.Text = "L&oad";
			menuItemLoad.Click += new EventHandler( OnMenuItemLoadClick );
			
			// menuItemSave
			menuItemSave.Index = 2;
			menuItemSave.Text = "&Save";
			menuItemSave.Click += new EventHandler( OnMenuItemSaveClick );
			
			// menuItemSaveAs
			menuItemSaveAs.Index = 3;
			menuItemSaveAs.Text = "S&ave as";
			menuItemSaveAs.Click += new EventHandler( OnMenuItemSaveAsClick );
			
			// menuItemDash
			menuItemDash1.Index = 4;
			menuItemDash1.Text = "-";
			
			// menuItemExit
			menuItemExit.Index = 5;
			menuItemExit.Text = "E&xit";
			menuItemExit.Click += new EventHandler( OnMenuItemExitClick );
			
			// menuItemResources
			menuItemResources.Index = 1;
			menuItemResources.MenuItems.AddRange( new MenuItem[] {
								     menuItemAddString,
								     menuItemAddFiles,
								     menuItemAddColor,
								     menuItemDash2,
								     menuItemDelete,
								     menuItemCopy,
								     menuItemPaste,
								     menuItemDash3,
								     menuItemRename } );
			menuItemResources.Text = "Resources";
			
			// menuItemAddString
			menuItemAddString.Index = 0;
			menuItemAddString.Text = "Add S&tring";
			menuItemAddString.Click += new EventHandler( OnMenuItemAddStringClick );
			
			// menuItemAddFiles
			menuItemAddFiles.Index = 1;
			menuItemAddFiles.Text = "Add Fi&le(s)";
			menuItemAddFiles.Click += new EventHandler( OnMenuItemAddFilesClick );
			
			// menuItemAddColor
			menuItemAddColor.Index = 2;
			menuItemAddColor.Text = "Add Colo&r";
			menuItemAddColor.Click += new EventHandler( OnMenuItemAddColorClick );
			
			// menuItemDash2
			menuItemDash2.Index = 3;
			menuItemDash2.Text = "-";
			
			// menuItemDelete
			menuItemDelete.Index = 4;
			menuItemDelete.Text = "&Delete";
			menuItemDelete.Click += new EventHandler( OnMenuItemDeleteClick );
			
			// menuItemCopy
			menuItemCopy.Index = 5;
			menuItemCopy.Text = "&Copy";
			menuItemCopy.Click += new EventHandler( OnMenuItemCopyClick );
			
			// menuItemPaste
			menuItemPaste.Index = 6;
			menuItemPaste.Text = "Paste";
			menuItemPaste.Click += new EventHandler( OnMenuItemPasteClick );
			
			menuItemDash3.Index = 7;
			menuItemDash3.Text = "-";
			
			menuItemRename.Index = 8;
			menuItemRename.Text = "Rename";
			menuItemRename.Click += new EventHandler( OnMenuItemRenameClick );
			
			// menuItemHelp
			menuItemHelp.Index = 2;
			menuItemHelp.MenuItems.AddRange( new MenuItem[] {
								menuItemAbout} );
			menuItemHelp.Text = "&Help";
			
			// menuItemAbout
			menuItemAbout.Index = 0;
			menuItemAbout.Text = "A&bout";
			menuItemAbout.Click += new EventHandler( OnMenuItemAboutClick );
			
			// mainMenu
			mainMenu.MenuItems.AddRange( new MenuItem[] {
							    menuItemFile,
							    menuItemResources,
							    menuItemHelp} );
			
			// resourceControl
			resourceControl.Dock = DockStyle.Fill;
			resourceControl.InternalContextMenu.MenuItems.AddRange( new MenuItem[] {
										       menuItemAddString.CloneMenu( ),
										       menuItemAddFiles.CloneMenu( ),
										       menuItemAddColor.CloneMenu( ),
										       menuItemDash2.CloneMenu( ),
										       menuItemDelete.CloneMenu( ),
										       menuItemCopy.CloneMenu( ),
										       menuItemPaste.CloneMenu( ),
										       menuItemRename.CloneMenu( )
									       } );
			
			// statusBar
			StatusBarPanel panel1 = new StatusBarPanel( );
			StatusBarPanel panel2 = new StatusBarPanel( );
			
			panel1.BorderStyle = StatusBarPanelBorderStyle.Sunken;
			panel1.AutoSize = StatusBarPanelAutoSize.Spring;
			panel2.BorderStyle = StatusBarPanelBorderStyle.Sunken;
			panel2.AutoSize = StatusBarPanelAutoSize.Spring;
			statusBar.ShowPanels = true;
			statusBar.Panels.Add( panel1 );
			statusBar.Panels.Add( panel2 );
			
			resourceControl.ParentStatusBar = statusBar;
			
			// MainForm
//			AutoScaleBaseSize = new Size( 5, 14 );
			ClientSize = new Size( 592, 541 );
			
			Menu = mainMenu;
			Text = "New Resource.resx";
			
			ResumeLayout( false );
			
			Controls.Add( resourceControl );
			Controls.Add( statusBar );
		}
		
		void OnMenuItemNewClick( object sender, EventArgs e )
		{
			resourceControl.NewResource( );
		}
		
		
		void OnMenuItemLoadClick( object sender, EventArgs e )
		{
			resourceControl.LoadResource( );
		}
		
		void OnMenuItemSaveClick( object sender, EventArgs e )
		{
			resourceControl.SaveResource( );
		}
		
		void OnMenuItemSaveAsClick( object sender, EventArgs e )
		{
			resourceControl.SaveResourceAs( );
		}
		
		void OnMenuItemExitClick( object sender, EventArgs e )
		{
			if ( resourceControl.CanExit( ) )
				Close( );
		}
		
		void OnMenuItemAddStringClick( object sender, EventArgs e )
		{
			resourceControl.AddString( );
		}
		
		void OnMenuItemAddFilesClick( object sender, EventArgs e )
		{
			resourceControl.AddFiles( );
		}
		
		void OnMenuItemAddColorClick( object sender, EventArgs e )
		{
			resourceControl.AddColor( );
		}
		
		void OnMenuItemDeleteClick( object sender, EventArgs e )
		{
			resourceControl.DeleteResource( );
		}
		
		void OnMenuItemCopyClick( object sender, EventArgs e )
		{
			resourceControl.CopyResource( );
		}
		
		void OnMenuItemPasteClick( object sender, EventArgs e )
		{
			resourceControl.PasteResource( );
		}
		
		void OnMenuItemRenameClick( object sender, EventArgs e )
		{
			resourceControl.RenameResource( );
		}
		
		void OnMenuItemAboutClick( object sender, EventArgs e )
		{
			AboutDialog ad = new AboutDialog( );
			ad.ShowDialog( );
		}
		
		[STAThread]
		static void Main( )
		{
			Application.Run( new MainForm( ) );
		}
	}
}

