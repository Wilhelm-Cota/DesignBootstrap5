using DesignBootstrap5.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

namespace DesignBootstrap5.Controllers
{
    public class HomeController : Controller
    {
        SqlConnection con = new SqlConnection("Data Source=PETER-LAPTOP\\SQLEXPRESS;Initial Catalog=DesignBoostrap5;Integrated Security=True");
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Index(UserLogin userLogin)
        {
            try
            {
                string conn = "SELECT * FROM Registeration WHERE (username=@username OR email=@email) AND password=@password";
                SqlCommand cmd = new SqlCommand(conn, con);
                cmd.Parameters.AddWithValue("@username", userLogin.Username);
                cmd.Parameters.AddWithValue("@email", userLogin.Email);
                cmd.Parameters.AddWithValue("@password", userLogin.Password);

                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    return RedirectToAction("Home");
                }
                return View();

            }
            catch (Exception ex)
            {
                return Error();
            }
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(UserRegistration userRegistration)
        {

            string conn = "INSERT INTO Registeration(username,firstname,lastname,email,password,gender,age,department,role) VALUES(@username,@firstname,@lastname,@email,@password,@gender,@age,@department,@role)";
            SqlCommand cmd = new SqlCommand(conn, con);
            
            cmd.Parameters.AddWithValue("@username", userRegistration.UserName);
            cmd.Parameters.AddWithValue("@firstname", userRegistration.FirstName);
            cmd.Parameters.AddWithValue("@lastname", userRegistration.LastName);
            cmd.Parameters.AddWithValue("@email", userRegistration.Email);
            cmd.Parameters.AddWithValue("@password", userRegistration.Password);
            cmd.Parameters.AddWithValue("@gender", userRegistration.Gender);
            cmd.Parameters.AddWithValue("@age", userRegistration.Age);
            cmd.Parameters.AddWithValue("@department", userRegistration.Department);
            cmd.Parameters.AddWithValue("@role", userRegistration.Role);
            con.Open();
            int result = cmd.ExecuteNonQuery();
            con.Close();
            if (result > 0)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View();
                // handle the case where the query did not insert any rows
            }

        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}