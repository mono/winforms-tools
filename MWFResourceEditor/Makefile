all: mono

mono:
	mcs -out:resedit.exe AboutDialog.cs ImagePanel.cs IResource.cs IResourceRenderer.cs MainForm.cs ResourceBase.cs ResourceIcon.cs ResourceImage.cs ResourceListBox.cs ResourceString.cs ResourceType.cs TextEntryDialog.cs TextPanel.cs /r:System.Windows.Forms.dll /r:System.Drawing.dll

dotnet:
	csc -out:resedit.exe AboutDialog.cs ImagePanel.cs IResource.cs IResourceRenderer.cs MainForm.cs ResourceBase.cs ResourceIcon.cs ResourceImage.cs ResourceListBox.cs ResourceString.cs ResourceType.cs TextEntryDialog.cs TextPanel.cs /r:System.Windows.Forms.dll /r:System.Drawing.dll

clean:
	rm resedit.exe -r -f
