all: mono

mono:
	mcs -out:resedit.exe MainForm.cs TextPanel.cs ImagePanel.cs TextEntryDialog.cs AboutDialog.cs ContentType.cs ContentStruct.cs /r:System.Windows.Forms.dll /r:System.Drawing.dll

dotnet:
	csc -out:resedit.exe MainForm.cs TextPanel.cs ImagePanel.cs TextEntryDialog.cs AboutDialog.cs ContentType.cs ContentStruct.cs /r:System.Windows.Forms.dll /r:System.Drawing.dll

clean:
	rm resedit.exe -r -f
