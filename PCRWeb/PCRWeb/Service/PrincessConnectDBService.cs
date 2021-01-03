using PCRWeb.Models;
using PCRWeb.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;


namespace PCRWeb.Service
{
    public class PrincessConnectDBService
    {
        private readonly static string cnstr = ConfigurationManager.ConnectionStrings["ASP.NET MVC"].ConnectionString;
        //建立與資料庫的連線
        private readonly SqlConnection conn = new SqlConnection(cnstr);
        #region 查詢陣列資料
        public List<PrincessConnect> GetDataList()
        {
            List<PrincessConnect> DataList = new List<PrincessConnect>();
            string sql = @" SELECT * FROM PrincessConnect; ";
            try
            {
                conn.Open();//開啟資料庫連線
                SqlCommand cmd = new SqlCommand(sql, conn);//取得Sql指令
                SqlDataReader dr = cmd.ExecuteReader();//取得Sql資料
                while (dr.Read())//獲得下一筆資料直到沒有資料
                {
                    PrincessConnect Data = new PrincessConnect();
                    Data.Id = Convert.ToInt32(dr["Id"]);
                    //確定此留言是否回復，且不允許是空白
                    DataList.Add(Data);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
            return DataList;
        }
        #endregion
        public List<PrincessConnect> GetDataList(string Search)
        {
            List<PrincessConnect> DataList = new List<PrincessConnect>();

            string sql = string.Empty;
            if(!string.IsNullOrWhiteSpace(Search))
            {
                sql = $@" SELECT * FROM PrincessConnect WHERE Content LIKE '%{Search}'; ";
            }
            else
            {
                sql = @" SELECT * FROM PrincessConnect; ";
            }
            try
            {
                conn.Open();//開啟資料庫連線
                SqlCommand cmd = new SqlCommand(sql, conn);//取得Sql指令
                SqlDataReader dr = cmd.ExecuteReader();//取得Sql資料
                while (dr.Read())//獲得下一筆資料直到沒有資料
                {
                    PrincessConnect Data = new PrincessConnect();
                    Data.Id = Convert.ToInt32(dr["Id"]);
                    //確定此留言是否回復，且不允許是空白
                    DataList.Add(Data);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
            return DataList;
        }
        #region 新增資料
        public void InsertPrincessConnectTen(PrincessConnectViewModel newData)
        {
            string sql = $@" INSERT INTO PrincessConnect(Content,a1,a2,a3,a4,a5,d1,d2,d3,d4,d5,positive,negative,DTRecord) VALUES( N'{newData.content}',
            N'{newData.a1}',N'{newData.a2}',N'{newData.a3}',N'{newData.a4}',N'{newData.a5}',N'{newData.d1}',N'{newData.d2}',N'{newData.d3}',
            N'{newData.d4}',N'{newData.d5}','{null}','{null}','{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}' ); ";
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);//取得Sql指令
                cmd.ExecuteNonQuery();//存資料進資料庫的關鍵語法
            }
            catch (Exception e)
            {
                throw new Exception(e.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
        }
        #endregion
        public PrincessConnect GetDataById(int Id)
        {
            PrincessConnect Data = new PrincessConnect();
            string sql = $@" SELECT * FROM PrincessConnect WHERE Id = {Id} ";
            try
            {
                conn.Open();//開啟DB連線
                SqlCommand cmd = new SqlCommand(sql, conn);//取得Sql資料
                SqlDataReader dr = cmd.ExecuteReader();
                //SqlDataReader 的預設位置是在第一筆記錄之前。 因此，您必須呼叫 Read 才能開始存取任何資料。
                dr.Read();
                Data.Id = Convert.ToInt32(dr["Id"]);
            }
            catch (Exception)
            {
                Data = null;//查無資料
            }
            finally
            {
                conn.Close();
            }
            return Data;
        }
        #region 修改留言
        public void UpdatePrincessConnect(PrincessConnect UpdateData)
        {
            //string sql = $@" UPDATE PrincessConnect SET Content='{UpdateData.Content}' WHERE Id = '{UpdateData.Id}'";
            //try
            //{
            //    conn.Open();//開啟DB連線
            //    SqlCommand cmd = new SqlCommand(sql, conn);//取得Sql資料
            //    cmd.ExecuteNonQuery();
            //}
            //catch (Exception e)
            //{
            //    throw new Exception(e.Message.ToString());
            //}
            //finally
            //{
            //    conn.Close();
            //}
        }
        #endregion

        public void DeleteGuestbooks(int Id)
        {
            string sql = $@" DELETE FROM PrincessConnect WHERE Id = {Id}; ";
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.ExecuteNonQuery();
            }
            catch(Exception e)
            {
                throw new Exception(e.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
        }

        #region 檢查相關
        public bool CheckUpdate(int Id)
        {
            PrincessConnect Data = GetDataById(Id);//根據Id取得要修改的資料
            return (Data != null);//判斷是否有資料以及回復
        }
        #endregion
    }
}