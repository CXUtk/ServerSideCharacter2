using System;
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
using Terraria.ModLoader.UI.Elements;
using System.Windows.Forms;

namespace ServerSideCharacter2.GUI.UI
{
	

	public class NPCUIState : AdvWindowUIState
	{

		private class UISimpleSlot : UIElement
		{
			private int NPCType { get; set; }
			public NPC Npc { get; private set; }

			public UISimpleSlot(int type)
			{
				NPCType = type;
				Npc = new NPC();
				Npc.SetDefaults(type);
			}

			public override void Click(UIMouseEvent evt)
			{
				base.Click(evt);
				Main.PlaySound(7, -1, -1, 1, 1f, 0.0f);
				if(Main.netMode == 1)
				{
					MessageSender.SendSummonCommand(NPCType, 1);
				}
				else
				{
					var player = Main.LocalPlayer;
					var x = (int)player.Bottom.X + player.direction * 200;
					var y = (int)player.Bottom.Y;
					var index = NPC.NewNPC(x, y, NPCType, 0, 0f, 0f, 0f, 0f, 255);
				}
				//Main.playerInventory = true;
				//Main.mouseItem = new Item();
				//Main.mouseItem.SetDefaults(ItemType);
				//Main.mouseItem.stack = Main.mouseItem.maxStack;
			}

			protected override void DrawSelf(SpriteBatch spriteBatch)
			{
				base.DrawSelf(spriteBatch);
				Main.instance.LoadNPC(NPCType);
				if (IsMouseHovering)
				{
					ServerSideCharacter2.ShowTooltip = Lang.GetNPCNameValue(Npc.netID)
						+ ((Npc.modNPC != null) ? (" [" + Npc.modNPC.mod.Name + "]") : "");
				}
				var slotbackTex = ServerSideCharacter2.ModTexturesTable["Box"];
				var DrawRectangle = GetDimensions();
				Drawing.DrawAdvBox(spriteBatch, (int)DrawRectangle.X, (int)DrawRectangle.Y,
					(int)DrawRectangle.Width, (int)DrawRectangle.Height,
					Drawing.DefaultBoxColor, slotbackTex, new Vector2(8, 8));

				var frame = new Rectangle(0, 0, Main.npcTexture[NPCType].Width, Main.npcTexture[NPCType].Height / Main.npcFrameCount[NPCType]);
				var size = frame.Size();
				var texScale = 1f;
				if (size.X > DrawRectangle.Width || size.Y > DrawRectangle.Height)
				{
					texScale = size.X > size.Y ? size.X / DrawRectangle.Width : size.Y / DrawRectangle.Height;
					texScale = 0.75f / texScale;
					size *= texScale;
				}
				spriteBatch.Draw(Main.npcTexture[NPCType], new Vector2(DrawRectangle.X + DrawRectangle.Width / 2 - (size.X) / 2,
					DrawRectangle.Y + DrawRectangle.Height / 2 - (size.Y) / 2), frame, Color.White, 0, Vector2.Zero, texScale, 0, 0);

			}
		}

		private UIAdvGrid _npcGrid;
		private UIPanel _itemPanel;
		private UIAdvTextBox _searchTextBox;

		private List<UISimpleSlot> uISlots;

		private const float WINDOW_WIDTH = 540;
		private const float WINDOW_HEIGHT = 480;
		private const float ITEM_BROWSER_OFFSETY = 100;
		private const float ITEM_BROWSER_OFFSETX = 20f;
		private const float SEARCH_BAR_WIDTH = 165;
		private const float SEARCH_BAR_HEIGHT= 30f;

		private const int NEGATIVE_SLOTS = 65;

		protected override void Initialize(UIAdvPanel WindowPanel)
		{
			
			WindowPanel.MainTexture = ServerSideCharacter2.ModTexturesTable["AdvInvBack1"];
			WindowPanel.Left.Set(Main.screenWidth / 2 - WINDOW_WIDTH / 2, 0f);
			WindowPanel.Top.Set(Main.screenHeight / 2 - WINDOW_HEIGHT / 2, 0f);
			WindowPanel.Width.Set(WINDOW_WIDTH, 0f);
			WindowPanel.Height.Set(WINDOW_HEIGHT, 0f);
			WindowPanel.Color = Color.White * 0.8f;

			_itemPanel = new UIPanel {BackgroundColor = Color.DarkBlue * 0.75f};
			_itemPanel.Top.Set(ITEM_BROWSER_OFFSETY, 0f);
			_itemPanel.Left.Set(ITEM_BROWSER_OFFSETX, 0f);
			_itemPanel.Width.Set(-2 * ITEM_BROWSER_OFFSETX, 1f);
			_itemPanel.Height.Set(-20 - ITEM_BROWSER_OFFSETY, 1f);

			_npcGrid = new UIAdvGrid();
			_npcGrid.Width.Set(-25f, 1f);
			_npcGrid.Height.Set(0f, 1f);
			_npcGrid.ListPadding = 5f;
			_itemPanel.Append(_npcGrid);


			uISlots = new List<UISimpleSlot>();
			for (var i = 1; i < Main.npcTexture.Length; i++)
			{
				try
				{
					var simpleslot = new UISimpleSlot(i);

					simpleslot.Width.Set(40f, 0f);
					simpleslot.Height.Set(40f, 0f);
					uISlots.Add(simpleslot);
				}
				catch(Exception ex)
				{
					// ignored
				}
			}
			_npcGrid.AddRange(uISlots);

			// ScrollBar设定
			var uiscrollbar = new UIAdvScrollBar();
			uiscrollbar.SetView(100f, 1000f);
			uiscrollbar.Height.Set(0f, 1f);
			uiscrollbar.HAlign = 1f;
			_itemPanel.Append(uiscrollbar);
			_npcGrid.SetScrollbar(uiscrollbar);

			WindowPanel.Append(_itemPanel);

			_searchTextBox = new UIAdvTextBox();
			_searchTextBox.Left.Set(-ITEM_BROWSER_OFFSETX - SEARCH_BAR_WIDTH, 1f);
			_searchTextBox.Top.Set(50, 0f);
			_searchTextBox.Width.Set(SEARCH_BAR_WIDTH, 0f);
			_searchTextBox.Height.Set(SEARCH_BAR_HEIGHT, 0f);
			_searchTextBox.OnTextChange += _searchTextBox_OnTextChange;
			WindowPanel.Append(_searchTextBox);
		}

		private void _searchTextBox_OnTextChange(string oldString, string curString)
		{
			curString = curString.Trim(' ');
			if (curString == "")
			{
				_npcGrid.Clear();
				_npcGrid.AddRange(uISlots);
				return;
			}
			var kmp = new KMP(curString.ToLower());
			_npcGrid.Clear();
			var slots = new List<UISimpleSlot>();
			for (var i = 0; i < uISlots.Count; i++)
			{
				if (kmp.Match(Lang.GetNPCNameValue(uISlots[i].Npc.netID).ToLower()))
				{
					slots.Add(uISlots[i]);
				}
			}
			_npcGrid.AddRange(slots);

		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			base.Draw(spriteBatch);
			var searchdim = _searchTextBox.GetDimensions();
			var searchDrawPos = new Vector2(searchdim.X - 35f, searchdim.Y + searchdim.Height / 2);
			var texture = ServerSideCharacter2.ModTexturesTable["Search"];
			spriteBatch.Draw(texture, searchDrawPos, null, Color.White, 0f, texture.Size() * 0.5f, 0.85f, SpriteEffects.FlipHorizontally, 0f);
		}

		protected override void OnClose(UIMouseEvent evt, UIElement listeningElement)
		{
			ServerSideCharacter2.Instance.ChangeState(SSCUIState.NPCPage);
		}
	}
}
