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
        public string MachineCode = "";

        public string QQ = "";
		public string OpenID = "";
		public string Ban = "";
		public string Banner = "";
		public string BanReason = "";
		public string CustomChatPrefix = "";
        public string MachineCode_DB = "";
        public string SkipMCCheck = "";

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
                /// 申请改密
                /// </summary>
                ChangePasswordRequired,
                /// <summary>
                /// 用户未绑定QQ
                /// </summary>
                Unbound,
                /// <summary>
                /// 用户被封禁
                /// </summary>
                Banned,
                /// <summary>
                /// 数据库记录丢失
                /// </summary>
                NotFound,
                /// <summary>
                /// 数据库操作出错
                /// </summary>
                Error,
                /// <summary>
                /// Debug模式
                /// </summary>
                Debug,
                /// <summary>
                /// 获取机器码失败
                /// </summary>
                GetMCFailed,
                /// <summary>
                /// 机器码校验失败
                /// </summary>
                MCCheckFailed
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
                Debug,
                /// <summary>
                /// 获取机器码失败
                /// </summary>
                GetMCFailed,
                /// <summary>
                /// 机器码已被绑定
                /// </summary>
                MCBound
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
                string ChangePasswordRequired = "";
                bool isCharacterFound = false;
                MySqlManager dbm = new MySqlManager();
                dbm.Connect();
                MySqlCommand cmd = dbm.command;
                cmd.CommandText = "select * from users where username = @UserName";
				cmd.Parameters.AddWithValue("@UserName", CharacterName);
                MySqlDataReader mdr = cmd.ExecuteReader();
                if (mdr.Read())
                {
                    isCharacterFound = true;
                    QQ = mdr["qq"].ToString();
                    OpenID = mdr["openid"].ToString();
                    Ban = mdr["ban"].ToString();
                    Banner = mdr["banner"].ToString();
                    BanReason = mdr["banreason"].ToString();
                    CustomChatPrefix = mdr["customchatprefix"].ToString();
                    ChangePasswordRequired = mdr["setpwreq"].ToString();
                    MachineCode_DB = mdr["machinecode"].ToString();
                    SkipMCCheck = mdr["skipmccheck"].ToString();
                }
                mdr.Close();
                cmd.Cancel();
                if (isCharacterFound)
                {
                    // 给上版本注册的用户补充注册机器码的代码
                    if (MachineCode_DB == "" && MachineCode != "")
                    {
                        MachineCode_DB = MachineCode;
                        MySqlManager _dbm = new MySqlManager();
                        _dbm.Connect();
                        MySqlCommand _cmd = _dbm.command;
                        _cmd.CommandText = "update users set machinecode = @MachineCode where username = @UserName";
                        _cmd.Parameters.AddWithValue("@UserName", CharacterName);
                        _cmd.Parameters.AddWithValue("@MachineCode", MachineCode);
                        _cmd.ExecuteNonQuery();
                        _cmd.Cancel();
                    }
                    // 校验机器码
                    if (SkipMCCheck != "1")
                    {
                        if (MachineCode == "")
                        { return States.LoginState.GetMCFailed; }
                        else if (MachineCode != MachineCode_DB)
                        { return States.LoginState.MCCheckFailed; }
                    }
                    // 校验绑定
                    if (QQ == "" || OpenID == "")
                    {
                        return States.LoginState.Unbound;
                    }
                    // 校验封禁
                    if (Ban != "0")
                    {
                        return States.LoginState.Banned;
                    }
                    // 校验改密状态
                    if (ChangePasswordRequired != "0")
                    {
                        MySqlManager _dbm = new MySqlManager();
                        _dbm.Connect();
                        MySqlCommand _cmd = _dbm.command;
                        _cmd.CommandText = "update users set setpwreq = 0 where username = @UserName";
                        _cmd.Parameters.AddWithValue("@UserName", CharacterName);
                        _cmd.ExecuteNonQuery();
                        _cmd.Cancel();
                        return States.LoginState.ChangePasswordRequired;
                    }
                    else
                    {
                        info.UserName = QQ;
                        info.OpenID = OpenID;
                        return States.LoginState.LoginSuccess;
                    }
                }
                else
                { return States.LoginState.NotFound; }
            }
            catch (Exception ex)
            {
                if (MySqlManager.connection != null)
                {
                    MySqlManager.connection.Close();
                    MySqlManager.connection.Dispose();
                    MySqlManager.connection = null;
                }
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
            try
            {
                // 判断QQ是否跳过机器码验证
                bool SkipMCCheck = false;
                MySqlManager _dbm = new MySqlManager();
                _dbm.Connect();
                MySqlCommand _cmd = _dbm.command;
                _cmd.CommandText = "select * from users where qq = @QQ and skipmccheck = 1";
                _cmd.Parameters.AddWithValue("@QQ", QQ);
                MySqlDataReader _mdr = _cmd.ExecuteReader();
                if (_mdr.Read())
                {
                    SkipMCCheck = true;
                }
                _mdr.Close();
                _cmd.Cancel();
                // 机器码校验
                if (!SkipMCCheck)
                {
                    // 机器码获取失败
                    if (MachineCode == "")
                    { return States.RegisterState.GetMCFailed; }
                    else
                    {
                        MySqlManager __dbm = new MySqlManager();
                        __dbm.Connect();
                        MySqlCommand __cmd = __dbm.command;
                        __cmd.CommandText = "select username from users where machinecode = @MachineCode";
                        __cmd.Parameters.AddWithValue("@MachineCode", MachineCode);
                        MySqlDataReader __mdr = __cmd.ExecuteReader();
                        if (__mdr.Read())
                        {
                            UserName = __mdr["username"].ToString();
                        }
                        __mdr.Close();
                        __cmd.Cancel();
                        // 机器码重复绑定
                        if (UserName != "" && UserName != CharacterName)
                        { return States.RegisterState.MCBound; }
                    }
                }
                // 判断角色是否已绑定QQ
                string _QQ = "";
                MySqlManager dbm = new MySqlManager();
                dbm.Connect();
                MySqlCommand cmd = dbm.command;
                cmd.CommandText = "select qq from users where username = @UserName";
                cmd.Parameters.AddWithValue("@UserName", CharacterName);
                MySqlDataReader mdr = cmd.ExecuteReader();
                if (mdr.HasRows)
                {
                    if (mdr.Read())
                    {
                        _QQ = mdr["qq"].ToString();
                    }
                }
                else
                {
                    _QQ = "";
                }
                mdr.Close();
                cmd.Cancel();
                // 判断QQ是否已绑定角色
                MySqlManager ___dbm = new MySqlManager();
                ___dbm.Connect();
                MySqlCommand ___cmd = ___dbm.command;
                ___cmd.CommandText = "select username from users where qq = @QQ";
                ___cmd.Parameters.AddWithValue("@QQ", QQ);
                MySqlDataReader ___mdr = ___cmd.ExecuteReader();
                if (___mdr.HasRows)
                {
                    if (___mdr.Read())
                    {
                        UserName = ___mdr["username"].ToString();
                    }
                }
                else
                {
                    UserName = "";
                }
                ___mdr.Close();
                ___cmd.Cancel();

                // 未绑定
                if (_QQ == "" && UserName == "")
                {
                    try
                    {
                        MySqlManager __dbm = new MySqlManager();
                        __dbm.Connect();
                        MySqlCommand __cmd = __dbm.command;
                        __cmd.CommandText = "insert into users set qq = @QQ , username = @UserName , machinecode = @MachineCode , ban = 0 , setpwreq = 0 ON DUPLICATE KEY UPDATE qq = @QQ , username = @UserName , machinecode = @MachineCode , ban = 0 , setpwreq = 0";
                        __cmd.Parameters.AddWithValue("@QQ", QQ);
                        __cmd.Parameters.AddWithValue("@UserName", CharacterName);
                        __cmd.Parameters.AddWithValue("@MachineCode", MachineCode);
                        __cmd.ExecuteNonQuery();
                        __cmd.Cancel();
                        return States.RegisterState.RegisterSuccess;
                    }
                    catch (Exception ex)
                    {
                        ErrorLog = ex.Message;
                        return States.RegisterState.Error;
                    }
                }
                if (_QQ == QQ && UserName == CharacterName)
                {
                    // 已被当前角色绑定
                    return States.RegisterState.RegisterRep;
                }
                else
                {
                    // 已被其他角色绑定
                    return States.RegisterState.QQBound;
                }
            }
            catch (Exception ex)
            {
                if (MySqlManager.connection != null)
                {
                    MySqlManager.connection.Close();
                    MySqlManager.connection.Dispose();
                    MySqlManager.connection = null;
                }
                ErrorLog = ex.Message;
                return States.RegisterState.Error;
            }

        }


		/// <summary>
		/// 获取封禁原因
		/// </summary>
		/// <param name="banPlayer">目标玩家</param>
		/// <returns></returns>
		public string GetBanReason(ServerPlayer banPlayer)
		{
			try
			{
				MySqlManager dbm = new MySqlManager();
				dbm.Connect();
				MySqlCommand cmd = dbm.command;
				cmd.CommandText = "select ban, banreason from users where username = @UserName";
				cmd.Parameters.AddWithValue("@UserName", banPlayer.Name);
				using (MySqlDataReader mdr = cmd.ExecuteReader())
				{
					if (mdr.Read())
					{
						Ban = mdr["ban"].ToString();
						BanReason = mdr["banreason"].ToString();
					}
				}
				cmd.Cancel();
				if(Ban == "1")
				{
					return BanReason;
				}
				else
				{
					return "";
				}
			}
			catch (Exception ex)
			{
				ErrorLog = ex.Message;
				return "";
			}
		}
        /// <summary>
        /// 用户封禁
        /// </summary>
        /// <param name="banPlayer">封禁的用户</param>
        /// <param name="banner">封禁操作者</param>
        /// <param name="banReason">封禁的原因</param>
        /// <returns>成功返回true，失败返回false</returns>
        public bool BanPlayer(ServerPlayer banPlayer, string banner, string banReason)
        {
			Console.WriteLine("Ban");
            try
            {
                MySqlManager dbm = new MySqlManager();
                dbm.Connect();
                MySqlCommand cmd = dbm.command;
                cmd.CommandText = "update users set ban = 1 , banner = @Banner , banreason = @BanReason where username = @UserName";
				cmd.Parameters.AddWithValue("@UserName", banPlayer.Name);
                cmd.Parameters.AddWithValue("@Banner", banner);
                cmd.Parameters.AddWithValue("@BanReason", banReason);
				cmd.ExecuteNonQuery();
				cmd.Cancel();
                return true;
            }
            catch (Exception ex)
            {
				CommandBoardcast.ConsoleError(ex);
                ErrorLog = ex.Message;
                return false;
            }
        }
        /// <summary>
		/// 用户解封
		/// </summary>
        /// <param name="banPlayer">解封的用户</param>
        /// <param name="banner">解封操作者</param>
		/// <returns>成功返回true，失败返回false</returns>
        public bool UnbanPlayer(ServerPlayer banPlayer, string banner)
        {
            try
            {
                MySqlManager dbm = new MySqlManager();
                dbm.Connect();
                MySqlCommand cmd = dbm.command;
                cmd.CommandText = "update users set ban = 0 , banner = @Banner where username = @UserName";
                cmd.Parameters.AddWithValue("@UserName", banPlayer.Name);
                cmd.Parameters.AddWithValue("@Banner", banner);
                cmd.ExecuteNonQuery();
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
