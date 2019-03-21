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
	public class UINormalPlayerBar : UIAdvPanel
	{
		private SimplifiedPlayerInfo playerInfo;

		private const float LABEL_MAX_WIDTH = 100;
		private const float GENDER_ICON_SIZE = 25;

		public UINormalPlayerBar(SimplifiedPlayerInfo info)
		{
			playerInfo = info;
			this.Width.Set(0, 1f);
			this.Height.Set(50f, 0f);
			base.MainTexture = ServerSideCharacter2.ModTexturesTable["Box2"];
			base.Color = Color.White;
			base.SetPadding(6f);


			UIText nameLabel = new UIText(playerInfo.Name);
			nameLabel.Top.Set(-10, 0.5f);
			nameLabel.Left.Set(5, 0);
			base.Append(nameLabel);

			//bool male = Main.player[playerInfo.PlayerID].Male;
			//UIImage _genderImage = new UIImage(ServerSideCharacter2.ModTexturesTable[male ? "Male" : "Female"]);
			//_genderImage.Top.Set(-GENDER_ICON_SIZE / 2, 0.5f);
			//_genderImage.Left.Set(LABEL_MAX_WIDTH + 10, 0);
			//_genderImage.Width.Set(GENDER_ICON_SIZE, 0);
			//_genderImage.Height.Set(GENDER_ICON_SIZE, 0);
			//_onlinePlayerPanel.Append(_genderImage);

			UIButton addFriendButton = new UIButton(null, true);
			addFriendButton.Top.Set(0f, 0f);
			addFriendButton.Left.Set(-70f, 1f);
			addFriendButton.Width.Set(70f, 0f);
			addFriendButton.Height.Set(0f, 1f);
			addFriendButton.BoxTexture = ServerSideCharacter2.ModTexturesTable["AdvInvBack3"];
			addFriendButton.ButtonDefaultColor = new Color(200, 200, 200);
			addFriendButton.ButtonChangeColor = Color.White;
			addFriendButton.CornerSize = new Vector2(12, 12);
			addFriendButton.ButtonText = "+好友";
			addFriendButton.OnClick += AddFriendButton_OnClick;
			this.Append(addFriendButton);

		}


		private void AddFriendButton_OnClick(UIMouseEvent evt, UIElement listeningElement)
		{
			ServerSideCharacter2.Instance.ShowMessage("目前没有实现，等裙子有时间写", 120, Color.White);
		}
	}
}
