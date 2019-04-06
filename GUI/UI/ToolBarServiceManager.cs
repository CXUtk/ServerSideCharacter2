using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using ServerSideCharacter2.Services;
using ServerSideCharacter2.GUI.UI.Component;

namespace ServerSideCharacter2.GUI.UI
{
	public class ToolBarServiceManager
	{
		private List<ISSCToolBarService> _services = new List<ISSCToolBarService>();

		public int ServicesCount
		{
			get { return _services.Count; }
		}

		public void Add(ISSCToolBarService service)
		{
			_services.Add(service);
		}

		public void Disable(string name)
		{
			foreach (var service in _services)
			{
				if(service.Name == name)
				{
					service.Enabled = false;
				}
			}

			throw new ArgumentException("没找到名字为: " + name + " 的服务");
		}

		public void Enable(string name)
		{
			foreach (var service in _services)
			{
				if (service.Name == name)
				{
					service.Enabled = true;
				}
			}

			throw new ArgumentException("没找到名字为: " + name + " 的服务");
		}

		public void Remove(string name)
		{
			_services.RemoveAll((s) => s.Name == name);
		}

		internal void GetButtons(List<UIButton> list)
		{
			list.Clear();
			foreach (var s in _services)
			{
				if (!s.Enabled) continue;
				var boxTex = ServerSideCharacter2.ModTexturesTable["Box"];
				var button = new UIButton(s.Texture, false);
				button.OnClick += s.OnButtonClicked;
				button.Width.Set(35, 0f);
				button.Height.Set(35, 0f);
				button.ButtonDefaultColor = new Color(200, 200, 200);
				button.ButtonChangeColor = Color.White;
				button.Tooltip = s.Tooltip;
				list.Add(button);
			}
		}

		public ToolBarServiceManager()
		{
			SetUpDefault();
		}

		internal void SetUpDefault()
		{
			Add(new Services.Login.LoginService());
			Add(new Services.OnlinePlayer.OnlinePlayerService());
			Add(new Services.HomePage.HomePageService());
			Add(new Services.FriendSystem.CommunicationService());
			Add(new Services.Matches.MatchService());
			Add(new Services.Union.UnionPageService());
			
			if(Main.netMode == 0)
			{
				Add(new Services.Misc.ItemServices());
				Add(new Services.Misc.NPCService());
			}
			//Add(new Services.OnlinePlayer.OnlinePlayerService());
			//Add(new Services.OnlinePlayer.OnlinePlayerService());
			//Add(new Services.OnlinePlayer.OnlinePlayerService());
		}

		internal void ResetDefault()
		{
			_services.Clear();
			SetUpDefault();
			if (ServerSideCharacter2.MainPlayerGroup.HasPermission("item"))
			{
				Add(new Services.Misc.ItemServices());
			}
			if (ServerSideCharacter2.MainPlayerGroup.HasPermission("sm"))
			{
				Add(new Services.Misc.NPCService());
			}
		}
	}
}
