using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;

namespace ServerSideCharacter2.Groups
{
	public class Group
	{
		public string GroupName { get; set; }
		public HashSet<string> permissions = new HashSet<string>();
		public Color ChatColor = new Color();
		public string ChatPrefix = "";
		public bool IsSuperAdmin { get; set; }

		public bool AddPermission(string name)
		{
			var perm = ServerSideCharacter2.GroupManager.PermissionList.GetPermission(name);
			if (perm == null) throw new SSCException("不存在这个权限名字：" + name);
			return permissions.Add(name);
		}

		public bool HasPermission(string name)
		{
			if (IsSuperAdmin) return true;
			if (Main.netMode == 2)
			{
				var perm = ServerSideCharacter2.GroupManager.PermissionList.GetPermission(name);
				if (perm == null) return false;
			}
			return permissions.Contains(name);
		}

		internal void UnitePermission(Group group)
		{
			permissions.UnionWith(group.permissions);
		}

		public Group(string name)
		{
			GroupName = name;
			ChatColor = Color.White;
		}
	}
}
