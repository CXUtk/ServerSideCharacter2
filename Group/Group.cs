using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerSideCharacter2.Group
{
	public class Group
	{
		public string GroupName { get; set; }
		public HashSet<Permission> permissions = new HashSet<Permission>();
		public Color ChatColor = new Color();
		public string ChatPrefix = "";
		public bool IsSuperAdmin { get; set; }

		public Group(string name)
		{
			GroupName = name;
			ChatColor = Color.White;
		}

		public bool HasPermission(string name)
		{
			return IsSuperAdmin || permissions.Any(t => t.Name == name);
		}
	}
}
