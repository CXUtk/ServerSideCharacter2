using Microsoft.Xna.Framework;
using MySql.Data.MySqlClient;
using ServerSideCharacter2.Core;
using ServerSideCharacter2.Crypto;
using ServerSideCharacter2.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ID;
using Terraria.Localization;

namespace ServerSideCharacter2.Services.Login
{
    public class QQAuth
    {
        // 成员
        private static string _constr = "server=localhost;User Id=mysqlserver;Password=258741369;Database=steamcityqqauth";
        private CryptedUserInfo info = new CryptedUserInfo();
        private ServerPlayer serverPlayer = new ServerPlayer();
        private int playerNumber;

        // 初始化
        public QQAuth(CryptedUserInfo info, ServerPlayer serverPlayer, int playerNumber)
        {
            this.info = info;
            this.serverPlayer = serverPlayer;
            this.playerNumber = playerNumber;
        }

        // 方法
        public bool Login()
        {
            string QQ = "";
            string OpenID = "";
            try
            {
                MySqlConnection mycon = new MySqlConnection(_constr);
                mycon.Open();
                MySqlCommand cmd = new MySqlCommand("set names utf8", mycon);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = "select qq,openid from users where username = @UserName";
                cmd.Parameters.AddWithValue("@UserName", serverPlayer.Name);
                MySqlDataReader mdr = cmd.ExecuteReader();
                if (mdr.Read())
                {
                    QQ = mdr["qq"].ToString();
                    OpenID = mdr["openid"].ToString();
                }
                mdr.Close();
                cmd.Cancel();
                mycon.Close();
                if (QQ == "" || OpenID == "")
                {
                    // 用户未绑定QQ
                    CommandBoardcast.ConsoleMessage($"玩家 {serverPlayer.Name} 认证失败：未绑定QQ.");
                    MessageSender.SendLoginFailed(playerNumber, "请先绑定QQ！");
                    return false;
                }
                else
                {
                    // 用户已绑定QQ
                    info.UserName = QQ;
                    info.OpenID = OpenID;
                    return true;
                }
            }
            catch (Exception ex)
            {
                // 程序出错
                MessageSender.SendLoginFailed(playerNumber, "数据库操作出错！");
                CommandBoardcast.ConsoleMessage("QQ验证模块 登录验证 出现错误，信息：" + ex.Message);
                return false;
            }
        }
        public bool Register()
        {
            string QQ = info.UserName;
            string UserName = "";
            if (QQ == "")
            {
                MessageSender.SendLoginFailed(playerNumber, "注册时QQ不能为空！");
                return false;
            }
            else
            {
                try
                {
                    MySqlConnection mycon = new MySqlConnection(_constr);
                    mycon.Open();
                    MySqlCommand cmd = new MySqlCommand("set names utf8", mycon);
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.CommandText = "select username from users where qq = @QQ";
                    cmd.Parameters.AddWithValue("@QQ", QQ);
                    MySqlDataReader mdr = cmd.ExecuteReader();
                    if (mdr.Read())
                    {
                        UserName = mdr["username"].ToString();
                    }
                    mdr.Close();
                    cmd.Cancel();
                    mycon.Close();
                    if (UserName == "")
                    {
                        // QQ未绑定到角色，允许注册
                        try
                        {
                            MySqlConnection _mycon = new MySqlConnection(_constr);
                            _mycon.Open();
                            MySqlCommand _cmd = new MySqlCommand("set names utf8", _mycon);
                            _cmd.CommandType = System.Data.CommandType.Text;
                            _cmd.CommandText = "insert into users set qq = @QQ , username = @UserName";
                            _cmd.Parameters.AddWithValue("@QQ", QQ);
                            _cmd.Parameters.AddWithValue("@UserName", serverPlayer.Name);
                            _cmd.ExecuteNonQuery();
                            _cmd.Cancel();
                            _mycon.Close();
                            CommandBoardcast.ConsoleMessage($"玩家 {serverPlayer.Name} 注册请求合法.");
                            return true;
                        }
                        catch (Exception ex)
                        {
                            // 程序出错
                            MessageSender.SendLoginFailed(playerNumber, "数据库操作出错！");
                            CommandBoardcast.ConsoleMessage("QQ验证模块 用户注册 出现错误，信息：" + ex.Message);
                            return false;
                        }
                    }
                    else if (UserName == serverPlayer.Name)
                    {
                        // QQ已被自己绑定，允许注册
                        CommandBoardcast.ConsoleMessage($"玩家 {serverPlayer.Name} 注册请求合法（角色可能丢失）.");
                        return true;
                    }
                    else
                    {
                        // QQ已被其他角色绑定，禁止注册
                        MessageSender.SendLoginFailed(playerNumber, "该QQ已被其他角色绑定！");
                        CommandBoardcast.ConsoleMessage($"玩家 {serverPlayer.Name} 注册请求被拒.");
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    // 程序出错
                    MessageSender.SendLoginFailed(playerNumber, "数据库操作出错！");
                    CommandBoardcast.ConsoleMessage("QQ验证模块 注册验证 出现错误，信息：" + ex.Message);
                    return false;
                }
            }
        }

    }
}
