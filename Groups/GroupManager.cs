using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ServerSideCharacter2.Groups
{
	public class GroupManager
	{
		[JsonIgnore]
		public readonly Dictionary<string, Group> DefaultGroups = new Dictionary<string, Group>();
		public Dictionary<string, Group> Groups = new Dictionary<string, Group>();
		public PermissionList PermissionList;

		private void AddToGroup(Group g)
		{
			Groups.Add(g.GroupName, g);
		}

		public GroupManager()
		{
			PermissionList = new PermissionList();
		}

		public void SetGroups()
		{
			Group crminalGroup = new Group("罪犯")
			{
				ChatColor = Color.Gray,
				ChatPrefix = "罪犯"
			};
			Group defaultGroup = new Group("公民")
			{
				ChatPrefix = "公民"
			};
			defaultGroup.AddPermission("ls");
			defaultGroup.AddPermission("friend");
			Group admin = new Group("管理员")
			{
				ChatColor = Color.Red,
				ChatPrefix = "管理",

			};
			admin.UnitePermission(defaultGroup);
			admin.AddPermission("time");
			admin.AddPermission("butcher");
			admin.AddPermission("mute");
			admin.AddPermission("tp");
			admin.AddPermission("sm");
			admin.AddPermission("tphere");
			admin.AddPermission("lock");
			admin.AddPermission("hardmode");
			admin.AddPermission("expert");
			admin.AddPermission("changetile");
			Group superAdmin = new Group("超级管理员")
			{
				ChatColor = Color.Cyan,
				ChatPrefix = "超管",
				IsSuperAdmin = true
			};

			DefaultGroups.Add(defaultGroup.GroupName, defaultGroup);
			DefaultGroups.Add(crminalGroup.GroupName, crminalGroup);
			DefaultGroups.Add(admin.GroupName, admin);
			DefaultGroups.Add(superAdmin.GroupName, superAdmin);
		}

		internal void AddDefaults()
		{
			foreach(var g in DefaultGroups)
			{
				AddToGroup(g.Value);
			}
			//AddToGroup(DefaultGroups["公民"]);
			//AddToGroup(DefaultGroups["罪犯"]);
			//AddToGroup(DefaultGroups["管理员"]);
			//AddToGroup(DefaultGroups["超级管理员"]);
		}
	}
}
