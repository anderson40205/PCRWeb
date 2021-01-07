using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using PCRWeb.Models;

namespace PCRWeb.Services
{
    public class MembersDBService
    {
        //建立與資料庫的連線字串
        private readonly static string cnstr = ConfigurationManager.ConnectionStrings["ASP.NET MVC"].ConnectionString;
        //建立與資料庫的連線
        private readonly SqlConnection conn = new SqlConnection(cnstr);

        #region 註冊
        //註冊新會員方法
        public void Register(Members newMember)
        {
            //將密碼Hash過
            newMember.Password = HashPassword(newMember.Password);
            //sql新增語法
            //IsAdmin 預設為0
            string sql = $@" INSERT INTO Members (Account,Password,IsAdmin) VALUES ('{newMember.Account}','{newMember.Password}','0') ";
            //確保程式不會因執行錯誤而整個中斷
            try
            {
                //開啟資料庫連線
                conn.Open();
                //執行Sql指令
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                //丟出錯誤
                throw new Exception(e.Message.ToString());
            }
            finally
            {
                //關閉資料庫連線
                conn.Close();
            }
        }
        #endregion

        #region Hash密碼
        //Hash密碼用的方法
        public string HashPassword(string Password)
        {
            //宣告Hash時所添加的無意義亂數值
            string saltkey = "1q2w3e4r5t6y7u8ui9o0po7tyy";
            //將剛剛宣告的字串與密碼結合
            string saltAndPassword = String.Concat(Password, saltkey);
            //定義SHA256的HASH物件
            SHA256CryptoServiceProvider sha256Hasher = new SHA256CryptoServiceProvider();
            //取得密碼轉換成byte資料
            byte[] PasswordData = Encoding.Default.GetBytes(saltAndPassword);
            //取得Hash後byte資料
            byte[] HashDate = sha256Hasher.ComputeHash(PasswordData);
            //將Hash後byte資料轉換成string
            string Hashresult = Convert.ToBase64String(HashDate);
            //回傳Hash後結果
            return Hashresult;
        }
        #endregion

        #region 查詢一筆資料
        //藉由帳號取得單筆資料的方法
        private Members GetDataByAccount(string Account)
        {
            Members Data = new Members();
            //Sql語法
            string sql = $@" select * from Members where Account = '{Account}' ";
            //確保程式不會因執行錯誤而整個中斷
            try
            {
                //開啟資料庫連線
                conn.Open();
                //執行Sql指令
                SqlCommand cmd = new SqlCommand(sql, conn);
                //取得Sql資料
                SqlDataReader dr = cmd.ExecuteReader();
                dr.Read();
                Data.Account = dr["Account"].ToString();
                Data.Password = dr["Password"].ToString();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception是:" + e.Message);
                //查無資料
                Data = null;
            }
            finally
            {
                //關閉資料庫連線
                conn.Close();
            }
            //回傳根據編號所取得的資料
            return Data;
        }
        #endregion

        #region 帳號註冊重複確認
        //確認要註冊帳號是否有被註冊過的方法
        public bool AccountCheck(string Account)
        {
            //藉由傳入帳號取得會員資料
            Members Data = GetDataByAccount(Account);
            //判斷是否有查詢到會員
            bool result = (Data == null);
            //回傳結果
            return result;
        }
        public string LoginCheck(string Account, string Password)
        {
            //取得傳入帳號的會員資料
            Members LoginMember = GetDataByAccount(Account);
            //判斷是否有此會員
            if (LoginMember != null)
            {
                //進行帳號密碼確認
                if (PasswordCheck(LoginMember, Password))
                {
                    return "";
                }
                else
                {
                    return "密碼輸入錯誤";
                }
            }
            else
            {
                return "無此會員帳號，請去註冊";
            }
        }
        #endregion



        #region 取得角色
        //取得會員的權限角色資料
        public string GetRole(string Account)
        {
            //宣告初始角色字串
            string Role = "User";
            //取得傳入帳號的會員資料
            Members LoginMember = GetDataByAccount(Account);
            //判斷資料庫欄位，用以確認是否為Admon
            if (LoginMember.IsAdmin)
            {
                Role += ",Admin"; //添加Admin
            }
            //回傳最後結果
            return Role;
        }
        #endregion


        #region 密碼確認
        //進行密碼確認方法
        public bool PasswordCheck(Members CheckMember, string Password)
        {
            //判斷資料庫裡的密碼資料與傳入密碼資料Hash後是否一樣
            bool result = CheckMember.Password.Equals(HashPassword(Password));
            //回傳結果
            return result;
        }
        #endregion
    }
}