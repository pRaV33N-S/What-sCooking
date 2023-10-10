using CookingAppMVC.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;


namespace CookingAppMVC.Controllers
{
    public class LoginRegController : Controller
    {
        private readonly string _connectionString = "server=DESKTOP-CHNJ5UD;database=Rllproject;trusted_connection=true;TrustServerCertificate=true;";

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(Models.Login Login)
        {
          

            if (ModelState.IsValid)
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(_connectionString))
                    {
                        connection.Open();

                        if (IsValidAdminUser(connection, Login.Username, Login.Password))
                        {
                            var claims = new List<Claim>
                            {
                                new Claim(ClaimTypes.Name, Login.Username),
                                new Claim(ClaimTypes.Role, "Admin")
                            };

                            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                            var principal = new ClaimsPrincipal(identity);
                            var login = HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                            return RedirectToAction("Index", "User");
                        }

                        if (IsValidUser(connection, Login.Username, Login.Password))
                        {
                            var claims = new List<Claim>
                            {
                                new Claim(ClaimTypes.Name, Login.Username),
                                new Claim(ClaimTypes.Role, "User")
                            };

                            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                            var principal = new ClaimsPrincipal(identity);
                            var login = HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                            return RedirectToAction("Index", "Home");
                        }
                   
                    }

                    //ModelState.AddModelError(string.Empty, "Invalid login attempt");
                    TempData["ErrorMessage"] = "Invalid login attempt";
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, $"Error: {ex.Message}");
                }
            }

            return View(Login);
        }

        private bool IsValidAdminUser(SqlConnection connection, string username, string password)
        {
            string query = "SELECT COUNT(*) FROM Admin WHERE Adminname = @Adminname AND BINARY_CHECKSUM(Password) = BINARY_CHECKSUM(@Password)";




            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@Adminname", SqlDbType.NVarChar).Value = username;
                command.Parameters.Add("@Password", SqlDbType.NVarChar).Value = password;

                int result = (int)command.ExecuteScalar();
                return result > 0;
            }
        }

        private bool IsValidUser(SqlConnection connection, string username, string password)
        {
            string query = "SELECT COUNT(*) FROM Users WHERE UserName = @UserName AND BINARY_CHECKSUM (Password) =BINARY_CHECKSUM (@Password )";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@UserName", SqlDbType.NVarChar).Value = username;
                command.Parameters.Add("@Password", SqlDbType.NVarChar).Value = password;

                int result = (int)command.ExecuteScalar();
                return result > 0;
            }
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(Models.Register register)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(_connectionString))
                    {
                        await connection.OpenAsync();

                        string query = "Insert into Users (Username,FirstName,LastName,Email,Password) values (@Username,@FirstName,@LastName,@Email,@Password)";
                        using (SqlCommand cmd = new SqlCommand(query, connection))
                        {
                            cmd.Parameters.AddWithValue("@FirstName", register.FirstName);
                            cmd.Parameters.AddWithValue("@LastName", register.LastName);
                            cmd.Parameters.AddWithValue("@Username", register.Username);
                            cmd.Parameters.AddWithValue("@Email", register.Email);
                            cmd.Parameters.AddWithValue("@Password", register.Password);

                            object result = cmd.ExecuteNonQuery();
                            if (result != null)
                            {
                                return RedirectToAction("Login", "LoginReg");
                            }
                        }
                    }

                    ModelState.AddModelError(string.Empty, "Invalid attempt");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, $"Error: {ex.Message}");
                }
            }

            return View(register);
        }
        public async Task<IActionResult> Logout()
        {
            // Sign out the user
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Redirect to the login page
            return RedirectToAction("Login", "LoginReg"); // Change "LoginReg" to your actual controller name
        }
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public IActionResult VerifyUser(string firstName, string lastName)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    if (IsValidUserByFirstNameAndLastName(connection, firstName, lastName))
                    {
                        return Json(new { isValid = true });
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new { isValid = false, error = ex.Message });
            }

            return Json(new { isValid = false });
        }

        private bool IsValidUserByFirstNameAndLastName(SqlConnection connection, string firstName, string lastName)
        {
            string query = "SELECT COUNT(*) FROM Users WHERE FirstName = @FirstName AND LastName = @LastName";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@FirstName", SqlDbType.NVarChar).Value = firstName;
                command.Parameters.Add("@LastName", SqlDbType.NVarChar).Value = lastName;

                int result = (int)command.ExecuteScalar();
                return result > 0;
            }
        }

        [HttpPost]
        public IActionResult ResetPassword(string firstName, string lastName, string newPassword)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    if (IsValidUserByFirstNameAndLastName(connection, firstName, lastName))
                    {
                        if (UpdateUserPassword(connection, firstName, lastName, newPassword))
                        {
                            return Json(new { success = true, message = "Password updated successfully" });
                        }
                        else
                        {
                            return Json(new { success = false, message = "Failed to update password" });
                        }
                    }
                    else
                    {
                        return Json(new { success = false, message = "User not found. Please check your first name and last name." });
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        private bool UpdateUserPassword(SqlConnection connection, string firstName, string lastName, string newPassword)
        {
            string query = "UPDATE Users SET Password = @NewPassword WHERE FirstName = @FirstName AND LastName = @LastName";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@NewPassword", SqlDbType.NVarChar).Value = newPassword;
                command.Parameters.Add("@FirstName", SqlDbType.NVarChar).Value = firstName;
                command.Parameters.Add("@LastName", SqlDbType.NVarChar).Value = lastName;

                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }


    }
}
    
