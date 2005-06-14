// Authors:
//	Alexander Olk, <xenomorph2@onlinehome.de>

using System;
using System.Collections;
using System.Collections.Specialized;

namespace FDOxml2cs
{
	public class MimeType
	{
		string typeName;
		
		string comment; // default comment
		
		Hashtable commentsLanguage = new Hashtable(); // key: language (de, en); value: comment
		
		int magicPriority = 50;
		
		ArrayList matches = new ArrayList();
		
		StringCollection globPatterns = new StringCollection();
		
		StringCollection subClasses = new StringCollection();
		
		StringCollection aliasTypes = new StringCollection();
		
		public string TypeName
		{
			set {
				typeName = value;
			}
			
			get {
				return typeName;
			}
		}
		
		public string Comment
		{
			set {
				comment = value;
			}
			
			get {
				return comment;
			}
		}
		
		public Hashtable CommentsLanguage
		{
			set {
				commentsLanguage = value;
			}
			
			get {
				return commentsLanguage;
			}
		}
		
		public int MagicPriority
		{
			set {
				magicPriority = value;
			}
			
			get {
				return magicPriority;
			}
		}
		
		public ArrayList Matches
		{
			set {
				matches = value;
			}
			
			get {
				return matches;
			}
		}
		
		public StringCollection GlobPatterns
		{
			set {
				globPatterns = value;
			}
			
			get {
				return globPatterns;
			}
		}
		
		public StringCollection SubClasses
		{
			set {
				subClasses = value;
			}
			
			get {
				return subClasses;
			}
		}
		
		public StringCollection AliasTypes
		{
			set {
				aliasTypes = value;
			}
			
			get {
				return aliasTypes;
			}
		}
	}
}
