all: mono

mono:
	mcs -out:resedit.exe AboutDialog.cs ByteArrayPanel.cs ColorPanel.cs ImagePanel.cs IResource.cs IResourceRenderer.cs MainForm.cs ResourceBase.cs ResourceByteArray.cs ResourceColor.cs ResourceCursor.cs  ResourceIcon.cs ResourceImage.cs ResourceListBox.cs ResourceString.cs ResourceType.cs TextEntryDialog.cs TextPanel.cs ResourceTreeView.cs ResourceTreeNode.cs IPanel.cs /r:System.Windows.Forms.dll /r:System.Drawing.dll

dotnet:
	csc -out:resedit.exe AboutDialog.cs ByteArrayPanel.cs ColorPanel.cs ImagePanel.cs IResource.cs IResourceRenderer.cs MainForm.cs ResourceBase.cs ResourceByteArray.cs ResourceColor.cs ResourceCursor.cs  ResourceIcon.cs ResourceImage.cs ResourceListBox.cs ResourceString.cs ResourceType.cs TextEntryDialog.cs TextPanel.cs ResourceTreeView.cs ResourceTreeNode.cs IPanel.cs /r:System.Windows.Forms.dll /r:System.Drawing.dll

clean:
	rm resedit.exe -r -f
