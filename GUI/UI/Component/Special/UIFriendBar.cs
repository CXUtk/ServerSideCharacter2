using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.GameContent.UI.Elements;
using ReLogic.Graphics;
using Terraria.GameInput;
using ReLogic.OS;
using Microsoft.Xna.Framework.Input;
using Terraria.UI.Chat;
using ServerSideCharacter2.JsonData;

namespace ServerSideCharacter2.GUI.UI.Component.Special
{
	public class UIFriendBar : UIElement
	{
		private SimplifiedPlayerInfo playerInfo;

		private UIAdvPanel _onlinePlayerPanel;

		private const float LABEL_MAX_WIDTH = 100;
		private const float GENDER_ICON_SIZE = 25;

		public UIFriendBar(SimplifiedPlayerInfo info)
		{
			playerInfo = info;
			this.Width.Set(0, 1f);
			this.Height.Set(35f, 0f);

			_onlinePlayerPanel = new UIAdvPanel(ServerSideCharacter2.ModTexturesTable["Box2"]);
			_onlinePlayerPanel.Top.Set(0, 0);
			_onlinePlayerPanel.Left.Set(0, 0);
			_onlinePlayerPanel.Width.Set(0, 1);
			_onlinePlayerPanel.Height.Set(0, 1);
			_onlinePlayerPanel.Color = Color.White;
			Append(_onlinePlayerPanel);

			UIText nameLabel = new UIText(playerInfo.Name);
			nameLabel.Top.Set(5, 0);
			nameLabel.Left.Set(0, 0);
			nameLabel.Width.Set(LABEL_MAX_WIDTH, 0);
			nameLabel.Height.Set(0, 1);
			nameLabel.SetPadding(0);
			Append(nameLabel);

			//bool male = Main.player[playerInfo.PlayerID].Male;
			//UIImage _genderImage = new UIImage(ServerSideCharacter2.ModTexturesTable[male ? "Male" : "Female"]);
			//_genderImage.Top.Set(-GENDER_ICON_SIZE / 2, 0.5f);
			//_genderImage.Left.Set(LABEL_MAX_WIDTH + 10, 0);
			//_genderImage.Width.Set(GENDER_ICON_SIZE, 0);
			//_genderImage.Height.Set(GENDER_ICON_SIZE, 0);
			//_onlinePlayerPanel.Append(_genderImage);

			//UIButton addFriendButton = new UIButton(null, true);
			//addFriendButton.MarginTop = 10f;
			//addFriendButton.MarginBottom = 10f;
			//addFriendButton.Top.Set(0f, 0f);
			//addFriendButton.Left.Set(LABEL_MAX_WIDTH + GENDER_ICON_SIZE + 20, 0f);
			//addFriendButton.Width.Set(60f, 0f);
			//addFriendButton.Height.Set(30f, 0f);
			//addFriendButton.BoxTexture = ServerSideCharacter2.ModTexturesTable["AdvInvBack3"];
			//addFriendButton.CornerSize = new Vector2(12, 12);
			//addFriendButton.ButtonText = "添加好友";
			//addFriendButton.OnClick += AddFriendButton_OnClick;
			//_onlinePlayerPanel.Append(addFriendButton);

		}

		private void AddFriendButton_OnClick(UIMouseEvent evt, UIElement listeningElement)
		{
			ServerSideCharacter2.Instance.ShowMessage("没有实现", 120, Color.White);
		}
	}
}
