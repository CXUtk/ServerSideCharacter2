using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerSideCharacter2.Group
{
	public class PermissionList
	{
		private Dictionary<string, Permission> _permissions;
		public PermissionList()
		{
			_permissions = new Dictionary<string, Permission>();

			AddPermission(new Permission("mute", "可以不可禁言玩家"));
			AddPermission(new Permission("tp", "传送到玩家身边"));
			AddPermission(new Permission("friend", "可不可以加好友"));
			AddPermission(new Permission("ls", "获取在线玩家信息"));
			AddPermission(new Permission("time", "改变时间"));
			AddPermission(new Permission("butcher", "全屏杀死怪物"));
			AddPermission(new Permission("lock", "强行锁住一个玩家"));
			AddPermission(new Permission("sm", "召唤怪物"));
			AddPermission(new Permission("tphere", "强制把玩家传送到你身边"));
			AddPermission(new Permission("expert", "切换专家模式"));
			AddPermission(new Permission("hardmode", "切换肉山前后"));
			AddPermission(new Permission("group", "管理权限组"));
			AddPermission(new Permission("banitem", "禁用物品"));
		}

		public void AddPermission(Permission permission)
		{
			_permissions.Add(permission.Name, permission);
		}

		public Permission GetPermission(string name)
		{
			if (_permissions.ContainsKey(name))
			{
				return _permissions[name];
			}
			return null;
		}
	}
}
