using System;
using System.Collections;
using System.Collections.Specialized;
using System.Drawing;
using System.Windows.Forms;

namespace MWFResourceEditor
{
	public class ResourceList
	{
		public static ResourceList Instance = new ResourceList();
		
		private static int newResourceCounter = 1;
		private StringCollection resourceNames = new StringCollection();
		
		private ArrayList items = new ArrayList();
		
		public ArrayList Items
		{
			set {
				items = value;
			}
			
			get {
				return items;
			}
		}
		
		public int Count
		{
			get {
				return items.Count;
			}
		}
		
		public void AddResourceDirect( IResource resource )
		{
			if ( items.Contains( resource ) )
				return;
			
			items.Add( resource );
		}
		
		public void InsertResourceDirect( int index, IResource resource )
		{
			items.Insert( index, resource );
		}
		
		public void RemoveResource( IResource resource )
		{
			resourceNames.Remove( resource.ResourceName );
			items.Remove( resource );
		}
		
		public void RenameResource( IResource resource, string new_name )
		{
			int index_allItems = IndexOfResource( resource );
			
			RemoveResource( resource );
			
			InsertResourceDirect( index_allItems, resource );
		}
		
		public void ReplaceResource( IResource old_resource, IResource new_resource )
		{
			int allItems_index = IndexOfResource( old_resource );
			
			RemoveResource( old_resource );
			
			InsertResourceDirect( allItems_index, new_resource );
		}
		
		public object[] ItemsAsArray( ResourceType resourceType )
		{
			object[] array = null;
			
			ArrayList tmp_list = new ArrayList( );
			
			foreach ( IResource resource in items )
			{
				if ( resource.ResourceType == resourceType )
					tmp_list.Add( resource );
			}
			
			array = tmp_list.ToArray( );
			
			return array;
		}
		
		public IResource GetResourceByName( string name )
		{
			IResource resource = null;
			
			foreach ( IResource iresource in items )
				if ( iresource.ResourceName == name )
				{
					resource = iresource;
					break;
				}
			
			return resource;
		}
		
		public int IndexOfResource( IResource resource )
		{
			return items.IndexOf( resource );
		}
		
		public int AddResource( string resource_name, object resource )
		{
			if ( ContainsName( resource_name ) )
				return 0;
			
			IResource iresource = null;
			
			if ( resource is Bitmap )
			{
				iresource = new ResourceImage( resource_name, resource as Image );
			}
			else
			if ( resource is String )
			{
				iresource = new ResourceString( resource_name, resource.ToString( ) );
			}
			else
			if ( resource is Icon )
			{
				iresource = new ResourceIcon( resource_name, resource as Icon );
			}
			else
			if ( resource is Color )
			{
				iresource = new ResourceColor( resource_name, (Color)resource );
			}
			else
			if ( resource is Cursor )
			{
				iresource = new ResourceCursor( resource_name, resource as Cursor );
			}
			
			items.Add( iresource );
			
			return 1;
		}
		
		public string UniqueResourceName( string pre )
		{
			string new_name = pre + "_" + newResourceCounter;
			
			while ( resourceNames.Contains( new_name ) )
			{
				newResourceCounter++;
				new_name = pre + "_" + newResourceCounter;
			}
			
			return new_name;
		}
		
		public bool ContainsName( string name )
		{
			bool has_name = resourceNames.Contains( name );
			
			if ( !has_name )
				resourceNames.Add( name );
			
			return has_name;
		}
		
		public void ClearResources( )
		{
			items.Clear( );
			resourceNames.Clear( );
		}
	}
}

