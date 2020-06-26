using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Latihan.Models;
using System.Data.SqlClient;

namespace Latihan.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString =
            "Data Source=DESKTOP-B66Q0OG\\MSSQLSERVER2016;" +
            "Initial Catalog=LATIHAN_MAVEN;" +
            "Integrated Security=SSPI;";
            conn.Open();

            string sql = "SELECT*from UserProfile where";
            SqlCommand cmd = new SqlCommand(sql, conn);
            SqlDataReader reader = cmd.ExecuteReader();

            List<UserProfile> listDataUser = new List<UserProfile>();
            while (reader.Read())
            {
                UserProfile user = new UserProfile
                {
                   USERNAME = Convert.ToString(reader["UserName"]),
                   PASSWORD = Convert.ToString(reader["Password"])
                };
                listDataUser.Add(user);
            }
            reader.Close();
            conn.Close();

            ViewData["ListDataUser"] = listDataUser;

            return View();
        }
    }
}
