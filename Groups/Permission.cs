using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerSideCharacter2.Groups
{
	public class Permission
	{
		[JsonIgnore]
		public string Description { get; set; }
		public string Name { get; set; }

		public Permission(string name, string description)
		{
			Name = name;
			Description = description;
		}

		public override int GetHashCode()
		{
			return Name.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			var other = obj as Permission;
			return Name.Equals(other.Name);
		}
	}
}
