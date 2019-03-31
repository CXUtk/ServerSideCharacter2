using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerSideCharacter2.Utils
{
	public class KMP
	{
		private int[] next;
		private string matchstr;
		public KMP(string match)
		{
			matchstr = match;
			next = new int[match.Length];
			next[0] = -1;
			var k = -1;
			var j = 0;
			while (j < match.Length - 1)
			{
				if (k == -1 || match[j] == match[k])
				{
					++j;
					++k;
					if (match[j] != match[k])
						next[j] = k;
					else
						next[j] = next[k];
				}
				else
				{
					k = next[k];
				}
			}
		}

		public bool Match(string s)
		{
			var i = 0;
			var j = 0;
			var sLen = s.Length;
			var pLen = matchstr.Length;
			while (i < sLen && j < pLen)
			{  
				if (j == -1 || s[i] == matchstr[j])
				{
					i++;
					j++;
				}
				else
				{
					j = next[j];
				}
				if (j == pLen) return true;
			}
			return false;
		}
	}
}
