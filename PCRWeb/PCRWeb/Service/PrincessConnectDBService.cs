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
        public PositiveAndNegativeReturn SearchPositive(PrincessConnect p,Members m)
        {//這邊一律處理使用者點讚這個動作，沒有=>新增到關聯表，使用者以經典過讚了=>從關聯表移除這列，PCR表該行讚減一
            //使用者是點倒讚=>更新關聯表倒讚變讚，
            string sql = $@" SELECT * FROM MandP WHERE MPAccount = N'{m.Account}' AND PID = N'{p.Id}'";
            PositiveAndNegativeReturn Data = new PositiveAndNegativeReturn();
            MandP mData = new MandP();
            try
            {
                conn.Open();//開啟DB連線
                SqlCommand cmd = new SqlCommand(sql, conn);//取得Sql資料
                SqlDataReader dr = cmd.ExecuteReader();
                //SqlDataReader 的預設位置是在第一筆記錄之前。 因此，您必須呼叫 Read 才能開始存取任何資料。
                if (dr.Read())
                {
                    mData.MPAccount = dr["MPAccount"].ToString();
                    mData.PID = Convert.ToInt32(dr["PID"]);
                    mData.status = dr["status"].ToString();
                    if(mData.status.Equals("p"))
                    {
                        dr.Close();
                        new SqlCommand($@" DELETE FROM MandP WHERE MPAccount = N'{m.Account}' AND PID = N'{mData.PID}' AND status = N'{'p'}'", conn).ExecuteNonQuery();
                        SqlDataReader drTemp = new SqlCommand($@" SELECT * FROM PrincessConnect WHERE Id = N'{p.Id}'", conn).ExecuteReader();
                        if(drTemp.Read())
                        {
                            int positiveCount = Convert.ToInt32(drTemp["positive"]);
                            drTemp.Close();
                            new SqlCommand($@" UPDATE PrincessConnect SET positive = N'{positiveCount-1}' WHERE Id='{p.Id}'",conn).ExecuteNonQuery();
                            Data.positive = positiveCount - 1;
                        }
                    }
                    else if(mData.status.Equals("n"))
                    {
                        dr.Close();
                        new SqlCommand($@" UPDATE MandP SET status = '{'p'}' WHERE MPAccount = N'{m.Account}' AND PID = N'{p.Id}'", conn).ExecuteNonQuery();
                        SqlDataReader drTemp = new SqlCommand($@" SELECT * FROM PrincessConnect WHERE Id = N'{p.Id}'", conn).ExecuteReader();
                        if (drTemp.Read())
                        {
                            int positiveCount = Convert.ToInt32(drTemp["positive"]); int negativeCount = Convert.ToInt32(drTemp["negative"]);
                            drTemp.Close();
                            new SqlCommand($@" UPDATE PrincessConnect SET positive = N'{positiveCount + 1}' , negative = N'{negativeCount - 1}' WHERE Id='{p.Id}'", conn).ExecuteNonQuery();
                            Data.positive = positiveCount + 1; Data.negative = negativeCount - 1;
                        }
                    }
                }
                else
                {
                    dr.Close();
                    string insertSql = $@" INSERT INTO MandP(MPAccount,PID,status) VALUES( N'{m.Account}',N'{p.Id}',N'{'p'}' ); ";
                    new SqlCommand(insertSql, conn).ExecuteNonQuery();
                    SqlDataReader drTemp = new SqlCommand($@" SELECT * FROM PrincessConnect WHERE Id = N'{p.Id}'", conn).ExecuteReader();
                    if (drTemp.Read())
                    {
                        int positiveCount = Convert.ToInt32(drTemp["positive"]);
                        drTemp.Close();
                        new SqlCommand($@" UPDATE PrincessConnect SET positive = N'{positiveCount + 1}' WHERE Id='{p.Id}'", conn).ExecuteNonQuery();
                    }
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
            return Data;
        }
        public PositiveAndNegativeReturn SearchNegative(PrincessConnect p, Members m)
        {//這邊一律處理使用者點讚這個動作，沒有=>新增到關聯表，使用者以經典過讚了=>從關聯表移除這列，PCR表該行讚減一
            //使用者是點倒讚=>更新關聯表倒讚變讚，
            string sql = $@" SELECT * FROM MandP WHERE MPAccount = N'{m.Account}' AND PID = N'{p.Id}'";
            PositiveAndNegativeReturn Data = new PositiveAndNegativeReturn();
            MandP mData = new MandP();
            try
            {
                conn.Open();//開啟DB連線
                SqlCommand cmd = new SqlCommand(sql, conn);//取得Sql資料
                SqlDataReader dr = cmd.ExecuteReader();
                //SqlDataReader 的預設位置是在第一筆記錄之前。 因此，您必須呼叫 Read 才能開始存取任何資料。
                if (dr.Read())
                {
                    mData.MPAccount = dr["MPAccount"].ToString();
                    mData.PID = Convert.ToInt32(dr["PID"]);
                    mData.status = dr["status"].ToString();
                    if (mData.status.Equals("n"))
                    {
                        dr.Close();
                        new SqlCommand($@" DELETE FROM MandP WHERE MPAccount = N'{m.Account}' AND PID = N'{mData.PID}' AND status = N'{'n'}'", conn).ExecuteNonQuery();
                        SqlDataReader drTemp = new SqlCommand($@" SELECT * FROM PrincessConnect WHERE Id = N'{p.Id}'", conn).ExecuteReader();
                        if (drTemp.Read())
                        {
                            int negativeCount = Convert.ToInt32(drTemp["negative"]);
                            drTemp.Close();
                            new SqlCommand($@" UPDATE PrincessConnect SET negative = N'{negativeCount - 1}' WHERE Id='{p.Id}'", conn).ExecuteNonQuery();
                            Data.negative = negativeCount - 1;
                        }
                    }
                    else if (mData.status.Equals("p"))
                    {
                        dr.Close();
                        new SqlCommand($@" UPDATE MandP SET status = '{'n'}' WHERE MPAccount = N'{m.Account}' AND PID = N'{p.Id}'", conn).ExecuteNonQuery();
                        SqlDataReader drTemp = new SqlCommand($@" SELECT * FROM PrincessConnect WHERE Id = N'{p.Id}'", conn).ExecuteReader();
                        if (drTemp.Read())
                        {
                            int positiveCount = Convert.ToInt32(drTemp["positive"]); int negativeCount = Convert.ToInt32(drTemp["negative"]);
                            drTemp.Close();
                            new SqlCommand($@" UPDATE PrincessConnect SET positive = N'{positiveCount - 1}' , negative = N'{negativeCount + 1}' WHERE Id='{p.Id}'", conn).ExecuteNonQuery();
                            Data.positive = positiveCount - 1; Data.negative = negativeCount + 1;
                        }
                    }
                }
                else
                {
                    dr.Close();
                    string insertSql = $@" INSERT INTO MandP(MPAccount,PID,status) VALUES( N'{m.Account}',N'{p.Id}',N'{'n'}' ); ";
                    new SqlCommand(insertSql, conn).ExecuteNonQuery();
                    SqlDataReader drTemp = new SqlCommand($@" SELECT * FROM PrincessConnect WHERE Id = N'{p.Id}'", conn).ExecuteReader();
                    if (drTemp.Read())
                    {
                        int negativeCount = Convert.ToInt32(drTemp["negative"]);
                        drTemp.Close();
                        new SqlCommand($@" UPDATE PrincessConnect SET negative = N'{negativeCount + 1}' WHERE Id='{p.Id}'", conn).ExecuteNonQuery();
                    }
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
            return Data;
        }
    }
}