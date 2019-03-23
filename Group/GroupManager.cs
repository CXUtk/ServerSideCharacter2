using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ServerSideCharacter2.Group
{
	public class GroupManager
	{
		[JsonIgnore]
		public readonly Dictionary<string, Group> DefaultGroups = new Dictionary<string, Group>();
		public Dictionary<string, Group> Groups = new Dictionary<string, Group>();

		private void AddToGroup(Group g)
		{
			Groups.Add(g.GroupName, g);
		}

		public GroupManager()
		{
			Group crminalGroup = new Group("criminal")
			{
				ChatColor = Color.Gray,
				ChatPrefix = "Criminal"
			};
			Group defaultGroup = new Group("default")
			{
				ChatPrefix = "Default"
			};
			defaultGroup.permissions.Add(new Permission("tp", "Teleport player"));
			defaultGroup.permissions.Add(new Permission("ls", "List online player's info"));
			defaultGroup.permissions.Add(new Permission("auth", "Authorize as super admin"));
			Group admin = new Group("admin")
			{
				ChatColor = Color.Red,
				ChatPrefix = "Admin",
				permissions = new HashSet<Permission>(defaultGroup.permissions)
				{
					new Permission("time", "Changing times"),
					new Permission("butcher", "Kill all monsters"),
					new Permission("ls -al", "List all player's info"),
					new Permission("lock", "Lock a player"),
					new Permission("sm", "Summon monsters"),
					new Permission("tphere", "Force teleport a player to your place"),
					new Permission("region", "Manage regions"),
					new Permission("region-create", "Create region"),
					new Permission("region-remove", "Remove regions"),
					new Permission("expert", "toggle expert"),
					new Permission("hardmode", "toggle hardmode"),
					new Permission("region-share", "Share regions"),
					new Permission("ban-item", "Ban certain item"),
					new Permission("chest", "Open locked chest"),
					new Permission("gen-res", "Generate world resources")
				}
			};
			Group superAdmin = new Group("spadmin")
			{
				ChatColor = Color.Cyan,
				ChatPrefix = "Super Admin",
				IsSuperAdmin = true
			};

			DefaultGroups.Add("default", defaultGroup);
			DefaultGroups.Add("criminal", crminalGroup);
			DefaultGroups.Add("admin", admin);
			DefaultGroups.Add("spadmin", superAdmin);
		}

		internal void AddDefaults()
		{
			AddToGroup(DefaultGroups["default"]);
			AddToGroup(DefaultGroups["criminal"]);
			AddToGroup(DefaultGroups["admin"]);
			AddToGroup(DefaultGroups["spadmin"]);
		}
	}
}
