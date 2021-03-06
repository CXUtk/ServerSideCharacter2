﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.GameContent.UI.Elements;
using ServerSideCharacter2.GUI.UI.Component;
using Terraria.GameContent.UI.States;
using ServerSideCharacter2.Utils;
using System.Security.Cryptography;
using ServerSideCharacter2.Core;
using ServerSideCharacter2.GUI.UI.Component.Special;

namespace ServerSideCharacter2.GUI.UI
{
	public class PlayerInventoryState2 : AdvWindowUIState
	{

		private int _relaxTimer;
		private float _rotation;

		public static PlayerInventoryState2 Instance;


		private const float WINDOW_WIDTH = 540;
		private const float WINDOW_HEIGHT = 500;
		private const float INVENTORY_WIDTH = 500;
		private const float INVENTORY_HEIGHT = 400;
		private const float ITEM_BROWSER_OFFSETY = 80;
		private const float ITEM_BROWSER_OFFSETX = 20f;
		private const float Y_OFFSET = 20;
		private const float X_OFFSET = 20;
		private const float BUTTON_WIDTH = 80;
		private const float BUTTON_HEIGHT = 35;
		private const float PLAYER_IMAGE_OFFSET_X = 50;
		private const float PLAYER_IMAGE_OFFSET_Y = 65;

		private int _targetID;
		private UIAdvGrid _itemGrid;
		private UIPanel _itemPanel;
		private UIAdvTextBox _searchTextBox;
		private UIText _playerName;
		private List<UISlot> slots;
		private string name;


		public PlayerInventoryState2()
		{
			Instance = this;
			slots = new List<UISlot>();
		}


		protected override void Initialize(UIAdvPanel WindowPanel)
		{
			WindowPanel.MainTexture = ServerSideCharacter2.ModTexturesTable["AdvInvBack1"];
			WindowPanel.Left.Set(Main.screenWidth / 2 - WINDOW_WIDTH / 2, 0f);
			WindowPanel.Top.Set(Main.screenHeight / 2 - WINDOW_HEIGHT / 2, 0f);
			WindowPanel.Width.Set(WINDOW_WIDTH, 0f);
			WindowPanel.Height.Set(WINDOW_HEIGHT, 0f);
			WindowPanel.Color = Color.White * 0.8f;

			_itemPanel = new UIPanel();
			_itemPanel.BackgroundColor = Color.DarkBlue * 0.75f;
			_itemPanel.Top.Set(ITEM_BROWSER_OFFSETY, 0f);
			_itemPanel.Left.Set(ITEM_BROWSER_OFFSETX, 0f);
			_itemPanel.Width.Set(INVENTORY_WIDTH, 0f);
			_itemPanel.Height.Set(INVENTORY_HEIGHT, 0f);


			//UISwitch uISwitch = new UISwitch();
			//uISwitch.Top.Set(50, 0);
			//uISwitch.Left.Set(50, 0);
			//uISwitch.Width.Set(60, 0f);
			//uISwitch.Height.Set(30, 0f);
			//WindowPanel.Append(uISwitch);

			_itemGrid = new UIAdvGrid();
			_itemGrid.Width.Set(-25f, 1f);
			_itemGrid.Height.Set(0f, 1f);
			_itemGrid.ListPadding = 5f;
			_itemPanel.Append(_itemGrid);

			// ScrollBar设定
			var uiscrollbar = new UIAdvScrollBar();
			uiscrollbar.SetView(100f, 1000f);
			uiscrollbar.Height.Set(0f, 1f);
			uiscrollbar.HAlign = 1f;
			_itemPanel.Append(uiscrollbar);
			_itemGrid.SetScrollbar(uiscrollbar);

			WindowPanel.Append(_itemPanel);


			_playerName = new UIText("玩家的背包", 0.7f, true);
			_playerName.HAlign = 0.5f;
			_playerName.Top.Set(30, 0f);

			WindowPanel.Append(_playerName);


		}

		internal void GetInventory(string name, List<Item> items)
		{
			this.name = name;
			_playerName.SetText($"{name} 的背包");
			if (slots.Count != items.Count)
			{
				slots.Clear();
				for (int i = 0; i < items.Count; i++)
				{
					var uislot = new UISlot();
					uislot.Width.Set(60, 0f);
					uislot.Height.Set(60, 0f);
					uislot.ContainedItem = items[i];
					uislot.Index = i;
					uislot.PostExchangeItem += Uislot_PostExchangeItem;
					slots.Add(uislot);
					_itemGrid.Add(uislot);
				}
			}
			else
			{
				for (int i = 0; i < items.Count; i++)
				{
					slots[i].ContainedItem = items[i];
				}
			}
		}




		private void Uislot_PostExchangeItem(UIElement target)
		{
			if (Main.netMode != 1) return;
			UISlot slot = (UISlot)target;
			MessageSender.SyncSingleEquip2(name, slot.Index, slot.ContainedItem);
		}


		//public void AppendFriends(JsonData.SimplifiedPlayerInfo info)
		//{
		//	UIFriendBar bar = new UIFriendBar(info);
		//	uIFriendBars.Add(bar);
		//	_friendList.Add(bar);
		//}




		protected override void OnClose(UIMouseEvent evt, UIElement listeningElement)
		{
			ServerSideCharacter2.Instance.ChangeState(SSCUIState.InventoryPage2);
		}
	}
}
