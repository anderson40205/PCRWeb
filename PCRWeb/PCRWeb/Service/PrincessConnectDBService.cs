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
        public void InsertPrincessConnectTen(PrincessConnect newData)
        {
            if (newData.Content == "" || newData.Content == null)
                newData.Content = "";

            string sql = $@" INSERT INTO PrincessConnect(Content,a1,a2,a3,a4,a5,d1,d2,d3,d4,d5,positive,negative,DTRecord) VALUES( N'{newData.Content}',
            N'{newData.a1}',N'{newData.a2}',N'{newData.a3}',N'{newData.a4}',N'{newData.a5}',N'{newData.d1}',N'{newData.d2}',N'{newData.d3}',
            N'{newData.d4}',N'{newData.d5}','{0}','{0}','{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}' ); ";
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
        public List<PrincessConnect> SearchDefense(PrincessConnect d)
        {
            List<PrincessConnect> dataList = new List<PrincessConnect>();
            string sql = $@" SELECT * FROM PrincessConnect WHERE d1 = N'{d.d1}' AND d2=N'{d.d2}' AND d3=N'{d.d3}' AND d4=N'{d.d4}' AND d5=N'{d.d5}' ";
            try
            {
                conn.Open();//開啟DB連線
                SqlCommand cmd = new SqlCommand(sql, conn);//取得Sql資料
                SqlDataReader dr = cmd.ExecuteReader();
                //SqlDataReader 的預設位置是在第一筆記錄之前。 因此，您必須呼叫 Read 才能開始存取任何資料。
                while(dr.Read())
                {
                    PrincessConnect Data = new PrincessConnect();
                    Data.Id = Convert.ToInt32(dr["Id"]);
                    Data.a1 = dr["a1"].ToString(); Data.a2 = dr["a2"].ToString(); Data.a3 = dr["a3"].ToString(); Data.a4 = dr["a4"].ToString(); Data.a5 = dr["a5"].ToString();
                    Data.d1 = dr["d1"].ToString(); Data.d2 = dr["d2"].ToString(); Data.d3 = dr["d3"].ToString(); Data.d4 = dr["d4"].ToString(); Data.d5 = dr["d5"].ToString();
                    Data.insertTime = Convert.ToDateTime(dr["DTRecord"]); Data.Content = dr["Content"].ToString();
                    Data.positive = Convert.ToInt32(dr["positive"]); Data.negative = Convert.ToInt32(dr["negative"]);
                    dataList.Add(Data);
                }
                
            }
            catch (Exception s)
            {
                Console.WriteLine(s.Message);
            }
            finally
            {
                conn.Close();
            }
            return dataList;
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