// Authors:
//	Alexander Olk, <xenomorph2@onlinehome.de>

using System;
using System.Collections;

namespace FDOxml2cs
{
	public class Match
	{
		string matchvalue;
		
		MatchTypes matchType;
		
		int offset;
		
		int offsetEnd = -1;
		
		string mask = null;
		
		ArrayList subMatches = new ArrayList();
		
		public string MatchValue
		{
			set {
				matchvalue = value;
			}
			
			get {
				return matchvalue;
			}
		}
		
		public MatchTypes MatchType
		{
			set {
				matchType = value;
			}
			
			get {
				return matchType;
			}
		}
		
		public int Offset
		{
			set {
				offset = value;
			}
			
			get {
				return offset;
			}
		}
		
		public string Mask
		{
			set {
				mask = value;
			}
			
			get {
				return mask;
			}
		}
		
		public ArrayList Matches
		{
			set {
				subMatches = value;
			}
			
			get {
				return subMatches;
			}
		}
		
		public int OffsetEnd
		{
			set {
				offsetEnd = value;
			}
			
			get {
				return offsetEnd;
			}
		}
	}
}
