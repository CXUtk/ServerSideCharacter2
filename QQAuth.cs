using Microsoft.Xna.Framework;
using MySql.Data.MySqlClient;
using ServerSideCharacter2.Core;
using ServerSideCharacter2.Crypto;
using ServerSideCharacter2.JsonData;
using ServerSideCharacter2.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ID;
using Terraria.Localization;

namespace ServerSideCharacter2
{
	public class QQAuth
	{
		// 成员
		public static string ConnectionStr;
		public string CharacterName = "";
		public string QQ = "";
		public string OpenID = "";
		public string Ban = "";
		public string Banner = "";
		public string BanReason = "";
		public string CustomChatPrefix = "";

		public string ErrorLog = "";


        // 初始化
        /// <summary>
		/// 创建一个QQ验证实例
		/// </summary>
        public QQAuth()
        { }

        // 类
        /// <summary>
        /// 验证状态
        /// </summary>
        public class States
        {
            /// <summary>
            /// 登录验证状态
            /// </summary>
            public enum LoginState : byte
            {
                /// <summary>
                /// 登录验证成功
                /// </summary>
                LoginSuccess,
                /// <summary>
                /// 用户未绑定QQ
                /// </summary>
                Unbound,
                /// <summary>
                /// 用户被封禁
                /// </summary>
                Banned,
                /// <summary>
                /// 数据库操作出错
                /// </summary>
                Error,
                /// <summary>
                /// Debug模式
                /// </summary>
                Debug
            }
            /// <summary>
            /// 注册验证状态
            /// </summary>
            public enum RegisterState : byte
            {
                /// <summary>
                /// 注册验证成功
                /// </summary>
                RegisterSuccess,
                /// <summary>
                /// 重复注册
                /// </summary>
                RegisterRep,
                /// <summary>
                /// QQ为空
                /// </summary>
                NullQQ,
                /// <summary>
                /// QQ已被绑定
                /// </summary>
                QQBound,
                /// <summary>
                /// 数据库操作出错
                /// </summary>
                Error,
                /// <summary>
                /// Debug模式
                /// </summary>
                Debug
            }

        }
        public class MySqlManager
        {
            public MySqlCommand command;
            public static MySqlConnection connection;
            public string connectionstr { get { return ConnectionStr; } }

            public void Connect()
            {
                try
                {
                    if (connection == null)
                    {
                        connection = new MySqlConnection(connectionstr);
                        connection.Open();
                    }
                    command = new MySqlCommand("set names utf8", connection)
                    {
                        CommandType = System.Data.CommandType.Text,
                    };
                }
                catch
                { Reconnect(); }
            }
            public void Reconnect()
            {
                try
                {
                    Disconnect();
                    connection = new MySqlConnection(connectionstr);
                    connection.Open();
                    command = new MySqlCommand("set names utf8", connection)
                    {
                        CommandType = System.Data.CommandType.Text,
                    };
                }
                catch(Exception)
                { throw; }
            }
            public void Disconnect()
            {
                if (command != null)
                {
                    command.Cancel();
                    command.Dispose();
                }
                if (connection != null)
                {
                    connection.Close();
                    connection.Dispose();
                }
            }

        }

        // 方法
        /// <summary>
		/// 登录验证
		/// </summary>
        public States.LoginState Login(CryptedUserInfo info)
        {
            if(ServerSideCharacter2.DEBUGMODE)
            { return States.LoginState.Debug; }
            try
            {
                MySqlManager dbm = new MySqlManager();
                dbm.Connect();
                MySqlCommand cmd = dbm.command;
                cmd.CommandText = "select qq,openid,ban,banner,banreason,customchatprefix from users where username = @UserName";
				cmd.Parameters.AddWithValue("@UserName", CharacterName);
                MySqlDataReader mdr = cmd.ExecuteReader();
                if (mdr.Read())
                {
                    QQ = mdr["qq"].ToString();
                    OpenID = mdr["openid"].ToString();
                    Ban = mdr["ban"].ToString();
                    Banner = mdr["banner"].ToString();
                    BanReason = mdr["banreason"].ToString();
                    CustomChatPrefix = mdr["customchatprefix"].ToString();
                }
                mdr.Close();
                cmd.Cancel();
                if (QQ == "" || OpenID == "")
                {
                    return States.LoginState.Unbound;
                }
                else if (Ban != "0")
                {
                    return States.LoginState.Banned;
                }
                else
                {
                    info.UserName = QQ;
                    info.OpenID = OpenID;
                    return States.LoginState.LoginSuccess;
                }
            }
            catch (Exception ex)
            {
                ErrorLog = ex.Message;
                return States.LoginState.Error;
            }
        }
        /// <summary>
		/// 用户注册
		/// </summary>
        public States.RegisterState Register(CryptedUserInfo info)
        {
            if (ServerSideCharacter2.DEBUGMODE)
            { return States.RegisterState.Debug; }
            QQ = info.UserName;
            string UserName = "";
            if (QQ == "")
            {
                return States.RegisterState.NullQQ;
            }
            else
            {
                try
                {
                    MySqlManager dbm = new MySqlManager();
                    dbm.Connect();
                    MySqlCommand cmd = dbm.command;
                    cmd.CommandText = "select username from users where qq = @QQ";
                    cmd.Parameters.AddWithValue("@QQ", QQ);
                    MySqlDataReader mdr = cmd.ExecuteReader();
                    if (mdr.Read())
                    {
                        UserName = mdr["username"].ToString();
                    }
                    mdr.Close();
                    cmd.Cancel();
                    if (UserName == "")
                    {
                        try
                        {
                            MySqlManager _dbm = new MySqlManager();
                            _dbm.Connect();
                            MySqlCommand _cmd = _dbm.command;
                            _cmd.CommandText = "insert into users set qq = @QQ , username = @UserName , ban = 0";
                            _cmd.Parameters.AddWithValue("@QQ", QQ);
                            _cmd.Parameters.AddWithValue("@UserName", CharacterName);
                            _cmd.ExecuteNonQuery();
                            _cmd.Cancel();
                            return States.RegisterState.RegisterSuccess;
                        }
                        catch (Exception ex)
                        {
                            ErrorLog = ex.Message;
                            return States.RegisterState.Error;
                        }
                    }
                    else if (UserName == CharacterName)
                    {
                        return States.RegisterState.RegisterRep;
                    }
                    else
                    {
                        return States.RegisterState.QQBound;
                    }
                }
                catch (Exception ex)
                {
                    ErrorLog = ex.Message;
                    return States.RegisterState.Error;
                }
            }
        }

		/// <summary>
		/// 用户封禁
		/// </summary>
		/// <param name="banPlayer">封禁的用户</param>
		/// <param name="banReason">封禁的原因</param>
		/// <returns>成功返回true，失败返回false</returns>
		public bool IsBanned(ServerPlayer banPlayer)
		{
			try
			{
				MySqlManager dbm = new MySqlManager();
				dbm.Connect();
				MySqlCommand cmd = dbm.command;
				cmd.CommandText = "select ban from users where username = @UserName";
				cmd.Parameters.AddWithValue("@UserName", banPlayer.Name);
				MySqlDataReader mdr = cmd.ExecuteReader();
				if (mdr.Read())
				{
					Ban = mdr["ban"].ToString();
				}
				cmd.Cancel();
				return Ban == "1";
			}
			catch (Exception ex)
			{
				ErrorLog = ex.Message;
				return false;
			}
		}
		/// <summary>
		/// 用户封禁
		/// </summary>
		/// <param name="banPlayer">封禁的用户</param>
		/// <param name="banReason">封禁的原因</param>
		/// <returns>成功返回true，失败返回false</returns>
		public bool BanPlayer(ServerPlayer banPlayer, string banReason)
        {
            try
            {
                MySqlManager dbm = new MySqlManager();
                dbm.Connect();
                MySqlCommand cmd = dbm.command;
                cmd.CommandText = "update users set ban = 1 , banner = @Banner , banreason = @BanReason where username = @UserName";
				cmd.Parameters.AddWithValue("@UserName", banPlayer.Name);
                cmd.Parameters.AddWithValue("@Banner", CharacterName);
                cmd.Parameters.AddWithValue("@BanReason", banReason);
                cmd.Cancel();
                return true;
            }
            catch (Exception ex)
            {
                ErrorLog = ex.Message;
                return false;
            }
        }
        /// <summary>
		/// 用户解封
		/// </summary>
        /// <param name="banPlayer">解封的用户</param>
		/// <returns>成功返回true，失败返回false</returns>
        public bool UnbanPlayer(ServerPlayer banPlayer)
        {
            try
            {
                MySqlManager dbm = new MySqlManager();
                dbm.Connect();
                MySqlCommand cmd = dbm.command;
                cmd.CommandText = "update users set ban = 0 , banner = @Banner where username = @UserName";
                cmd.Parameters.AddWithValue("@UserName", banPlayer.Name);
                cmd.Parameters.AddWithValue("@Banner", CharacterName);
                cmd.Cancel();
                return true;
            }
            catch (Exception ex)
            {
                ErrorLog = ex.Message;
                return false;
            }
        }
        /// <summary>
        /// 头衔修改
        /// </summary>
        /// <param name="toPlayer">修改的用户</param>
        /// <param name="Prefix">头衔</param>
        /// <returns>成功返回true，失败返回false</returns>
        public bool ChangePrefix(ServerPlayer toPlayer, string Prefix)
        {
            try
            {
                MySqlManager dbm = new MySqlManager();
                dbm.Connect();
                MySqlCommand cmd = dbm.command;
                cmd.CommandText = "update users set customchatprefix = @Prefix where username = @UserName";
                cmd.Parameters.AddWithValue("@UserName", toPlayer.Name);
                cmd.Parameters.AddWithValue("@Prefix", Prefix);
                cmd.Cancel();
                return true;
            }
            catch (Exception ex)
            {
                ErrorLog = ex.Message;
                return false;
            }
        }

    }
}
