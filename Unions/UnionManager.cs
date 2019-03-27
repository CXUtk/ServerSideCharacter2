using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;

namespace ServerSideCharacter2.Unions
{
	public class UnionManager
	{
		internal Dictionary<string, Union> Unions;

		public UnionManager()
		{
			Unions = new Dictionary<string, Union>();
		}

		public void CreateUnion(string name)
		{

		}

		public void RemoveUnion(string name)
		{

		}

	}
}
