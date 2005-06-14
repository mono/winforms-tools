all: mono

mono:
	mcs -out:FDOxml2cs.exe AliasReader.cs CommentReader.cs FDOxml2cs.cs GlobReader.cs MagicReader.cs MainReader.cs Match.cs MatchReader.cs MatchTypes.cs MimeType.cs MimeTypeReader.cs MimeUtils.cs SourceWriter.cs SubClassReader.cs SubMatchReader.cs /r:System.Xml.dll

dotnet:
	csc -out:FDOxml2cs.exe AliasReader.cs CommentReader.cs FDOxml2cs.cs GlobReader.cs MagicReader.cs MainReader.cs Match.cs MatchReader.cs MatchTypes.cs MimeType.cs MimeTypeReader.cs MimeUtils.cs SourceWriter.cs SubClassReader.cs SubMatchReader.cs /r:System.Xml.dll

clean:
	rm FDOxml2cs.exe -r -f
