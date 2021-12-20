using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication4.Models;
using System.Data.SqlClient;

namespace WebApplication4.Controllers
{
    public class HomeController : Controller
    {
        private static string connectionString = @"Data Source=DESKTOP-B66Q0OG\MSSQLSERVER2016;Initial Catalog=HERYANA_BE_DB;User ID=sa;Password=123";

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult InputNasabah()
        {
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            List<Nasabah> nasabah = new List<Nasabah>();
            try
            {
                SqlCommand cmd;
                SqlDataReader dataReader;
                string sql;

                sql = "Select ROW_NUMBER() OVER(ORDER BY AccountId DESC) AS [NO], * from TB_NASABAH";
                cmd = new SqlCommand(sql, conn);

                
                using (dataReader = cmd.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        nasabah.Add(new Nasabah() { NO = dataReader["NO"].ToString(), AccountId = dataReader["AccountId"].ToString(), Name = dataReader["Name"].ToString() });
                    }
                }
                dataReader.Close();
            }
            catch (Exception ex)
            {

            }
            conn.Close();

            ViewData["DataNasabah"] = nasabah;
            //return PartialView("InputNasabah");
            return View();
        }
        public ActionResult InputTransaksi()
        {
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            List<Transaksi> transaksi = new List<Transaksi>();
            try
            {
                SqlCommand cmd;
                SqlDataReader dataReader;
                string sql;

                sql = "Select ROW_NUMBER() OVER(ORDER BY AccountId DESC) AS [NO], AccountId,convert(varchar(10), TransactionDate, 126)TransactionDate, Description, DebitCreditStatus, Amount from TB_TRANSAKSI";
                cmd = new SqlCommand(sql, conn);


                using (dataReader = cmd.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        transaksi.Add(new Transaksi() { NO = dataReader["NO"].ToString(), AccountId = dataReader["AccountId"].ToString(), TransactionDate = dataReader["TransactionDate"].ToString()
                        , Description = dataReader["Description"].ToString(), DebitCreditStatus = dataReader["DebitCreditStatus"].ToString(), Amount = dataReader["Amount"].ToString()});
                    }
                }
                dataReader.Close();
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
            }
            conn.Close();

            ViewData["DataTransaksi"] = transaksi;
            return View();
        }
        public ActionResult Poin()
        {
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            List<Poin> Poin = new List<Poin>();
            try
            {
                SqlCommand cmd;
                SqlDataReader dataReader;
                string sql;

                sql = "select ROW_NUMBER() OVER(ORDER BY aa.[Name] DESC) AS [NO], aa.AccountId ,aa.[Name],isnull(cast(SUM(aa.LISTRIK) + SUM(aa.PULSA) as int), 0 )TotalPoin from("
                    + " select N.AccountId, N.[Name], T.[Description],"
                   +" SUM(case when T.[Description] like '%pulsa%' then"
	               + "    (case when T.Amount between 0 and 10000 then T.Amount/1000*0"
                   + "     when T.Amount between 10001 and 30000 then T.Amount/1000*1"
                   + "     when T.Amount > 30000 then T.Amount/1000*2 END)"
                   + "     END ) PULSA,"
                   + " SUM(case when T.[Description] like '%listrik%' then"
                   + "    (case when T.Amount between 0 and 50000 then T.Amount/2000*0"
                   + "     when T.Amount between 50001 and 100000 then T.Amount/2000*1"
                   + "     when T.Amount >100000 then T.Amount/2000*2  END)"
                   + "  	END) LISTRIK"
                   + "  	from TB_NASABAH N"
                   + "      join TB_TRANSAKSI T on N.AccountId = T.AccountId"
                   + "      group by N.AccountId, N.[Name], T.[Description], T.Amount"
                   + "      )aa"
                   + "      group by aa.AccountId, aa.[Name]";
                cmd = new SqlCommand(sql, conn);


                using (dataReader = cmd.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        Poin.Add(new Poin()
                        {
                            NO = dataReader["NO"].ToString(),
                            AccountId = dataReader["AccountId"].ToString(),
                            Name = dataReader["Name"].ToString(),
                            TotalPoin = dataReader["TotalPoin"].ToString(),
                        });
                    }
                }
                dataReader.Close();
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
            }
            conn.Close();

            ViewData["DataPoin"] = Poin;
            return View();
        }

        public ActionResult Report(string accid, string startdt, string enddt)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            List<Report> Report = new List<Report>();
            try
            {
                SqlCommand cmd;
                SqlDataReader dataReader;
                string sql;

                sql = "select ROW_NUMBER() OVER(ORDER BY bb.TransactionDate DESC) AS[NO], convert(varchar(10), bb.TransactionDate, 126)TransactionDate, bb.Description,bb.CREDIT,bb.DEBIT, bb.AMOUNT from("
                    + " select aa.TransactionDate, aa.[Description], isnull(cast(SUM(aa.CREDIT) as varchar),'-') CREDIT, isnull(cast(SUM(aa.DEBIT) as varchar),'-') DEBIT, "
                    + " isnull(SUM(aa.CREDIT), 0) + isnull(SUM(aa.DEBIT), 0) AMOUNT from("
                    + "   select T.TransactionDate, T.[Description]"
                    + "    ,case when T.DebitCreditStatus = 'C' Then T.Amount END CREDIT"
                    + "    ,case when T.DebitCreditStatus = 'D' Then T.Amount END DEBIT"
                    + " from TB_TRANSAKSI T"
                    + " join TB_NASABAH N on N.AccountId = T.AccountId"
                    + " where 1=1"
                    + " and (nullif('"+accid+ "', '') is null or N.AccountId like '%"+ accid+"%')"
                    //+ " and T.TransactionDate between '" + startdt + "' and '" + enddt + "'"
                    + " )aa"
                    + " group by aa.TransactionDate, aa.[Description], aa.CREDIT, aa.DEBIT"
                    + " )bb";

                //if (accid != null)
                //    sql = string.Format(sql, "where N.AccountId = '" + accid+"'");
                //else
                //    sql = string.Format(sql, " ");

                if (startdt != null)
                    sql = string.Format(sql, "and T.TransactionDate = '" + startdt + "'");
                else if (enddt != null)
                    sql = string.Format(sql, "and T.TransactionDate= '" + enddt + "'");
                else if (startdt != null && enddt != null)
                    sql = string.Format(sql, "and T.TransactionDate between '" + startdt + "' and '" + enddt + "'  ");
                else
                    sql = string.Format(sql, " ");

               
                //if (accid != null && startdt != null && enddt != null)
                //    sql = string.Format(sql, "and N.AccountId = '" + accid+ "' and T.TransactionDate between '" + startdt + "' and '" + enddt + "'  ");
                //else
                //    sql = string.Format(sql, " ");

                cmd = new SqlCommand(sql, conn);


                using (dataReader = cmd.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        Report.Add(new Report()
                        {
                            NO = dataReader["NO"].ToString(),
                            TransactionDate = dataReader["TransactionDate"].ToString(),
                            Description = dataReader["Description"].ToString(),
                            Credit = dataReader["Credit"].ToString(),
                            Debit = dataReader["Debit"].ToString(),
                            Amount = dataReader["Amount"].ToString(),
                        });
                    }
                }
                dataReader.Close();
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
            }
            conn.Close();

            ViewData["DataReport"] = Report;

            return View();
            //return PartialView("Report");
        }

        public ActionResult SaveInputNasabah(string accId, string name)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            try
            {
                SqlCommand cmd;
                //SqlDataReader dataReader;
                SqlDataAdapter Adapter = new SqlDataAdapter();
                string sql;

                sql = "insert into TB_NASABAH (AccountId, Name) values ('" + accId + "', '" + name + "')";
                cmd = new SqlCommand(sql, conn);

                Adapter.InsertCommand = new SqlCommand(sql, conn);
                Adapter.InsertCommand.ExecuteNonQuery();

                cmd.Dispose();
            }
            catch (Exception ex)
            {
                
            }
            conn.Close();

            return View();
        }

        public ActionResult SaveInputTransaksi(string accId, string TransactionDate, string Description, string DebitCreditStatus, string Amount)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            try
            {
                SqlCommand cmd;
                //SqlDataReader dataReader;
                SqlDataAdapter Adapter = new SqlDataAdapter();
                string sql;

                sql = "insert into TB_TRANSAKSI (AccountId, TransactionDate,Description,DebitCreditStatus,Amount) values ('" + accId + "', '" + TransactionDate + "', '" + Description + "', '" + DebitCreditStatus + "', '" + Amount + "')";
                cmd = new SqlCommand(sql, conn);

                Adapter.InsertCommand = new SqlCommand(sql, conn);
                Adapter.InsertCommand.ExecuteNonQuery();

                cmd.Dispose();
            }
            catch (Exception ex)
            {

            }
            conn.Close();

            return View();
        }
    }
}