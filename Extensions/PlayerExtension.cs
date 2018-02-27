using System;
using Terraria;

namespace ServerSideCharacter2
{
	public static class PlayerExtension
	{
		public static ServerPlayer GetServerPlayer(this Player p)
		{
			if (ServerSideCharacter2.PlayerCollection.ContainsKey(p.name))
			{
				return ServerSideCharacter2.PlayerCollection.Get(p.name);
			}
			else
			{
				throw new ArgumentException("Player name not found!");
			}
		}
	}
}
