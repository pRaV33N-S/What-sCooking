using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using CookingAppMVC.Models;
using Microsoft.AspNetCore.Authorization;

namespace CookingAppMVC.Controllers
{
    
    public class UserController : Controller
    {
        Uri baseAddress = new Uri("http://localhost:5006/api");
        HttpClient client;

        public UserController()
        {
            client = new HttpClient();
            client.BaseAddress = baseAddress;
        }
        [Authorize(Roles = "Admin")]

        public ActionResult Index()
        {
            try
            {
                List<User> users = new List<User>();
                HttpResponseMessage response = client.GetAsync(client.BaseAddress + "/Users").Result;
                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    users = JsonConvert.DeserializeObject<List<User>>(data);
                }
                return View(users);
            }
            catch (Exception ex)
            {
               
                Console.WriteLine($"An error occurred: {ex}");
                ViewBag.ErrorMessage = "Failed to retrieve user data. Please try again later.";
                return View(new List<User>()); 
            }
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            User user = new User();
            return View(user);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Create(User users)
        {
            if (!ModelState.IsValid)
            {
                return View(users);
            }

            try
            {
                string data = JsonConvert.SerializeObject(users);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = client.PostAsync(client.BaseAddress + "/Users", content).Result;
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    
                    var statusCode = response.StatusCode;
                    if (statusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        ModelState.AddModelError(string.Empty, "Invalid data. Please check your input.");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, $"Error creating User. Status Code: {statusCode}");
                    }
                    return View(users);
                }
            }
            catch (Exception ex)
            {
                
                Console.WriteLine($"An error occurred: {ex}");
                ModelState.AddModelError(string.Empty, "An error occurred while creating User. Please try again later.");
                return View(users);
            }
        }
        [HttpGet]
        [Authorize(Roles = "Admin,User")]
        public ActionResult EditById(int id)
        {
            User user = new User();
            HttpResponseMessage response = client.GetAsync(client.BaseAddress + "/Users/" + id).Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                user = JsonConvert.DeserializeObject<User>(data);
            }
            return View(user);
        }
        [HttpPost]
        [Authorize(Roles = "Admin,User")]
        public ActionResult EditById(User user)
        {
            try
            {
                string data = JsonConvert.SerializeObject(user);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = client.PutAsync(client.BaseAddress + "/Users/Id/" + user.UserId, content).Result;

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index", "Recipe");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Error updating User. Status Code: " + response.StatusCode);
                    return View(user);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An error occurred: " + ex.Message);
                return View(user);
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin,User")]
        public ActionResult EditByUsername(string username)
        {
            User user = new User();
            HttpResponseMessage response = client.GetAsync(client.BaseAddress + "/Users/Username/" + username).Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                user = JsonConvert.DeserializeObject<User>(data);
            }
            return View(user);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,User")]
        public ActionResult EditByUsername(User user)
        {
            try
            {
                string data = JsonConvert.SerializeObject(user);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = client.PutAsync(client.BaseAddress + "/Users/Username/" + user.Username, content).Result;

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index", "Recipe");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Error updating User. Status Code: " + response.StatusCode);
                    return View(user);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An error occurred: " + ex.Message);
                return View(user);
            }
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Details(int id)
        {
            User user = new User();
            try
            {
                HttpResponseMessage response = client.GetAsync(client.BaseAddress + "/Users/" + id).Result;
                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    user = JsonConvert.DeserializeObject<User>(data);
                }
                return View(user);
            }
            catch (Exception ex)
            {
                
                Console.WriteLine($"An error occurred: {ex}");
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            User users = new User();
            try
            {
                HttpResponseMessage response = client.GetAsync(client.BaseAddress + "/Users/" + id).Result;
                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    users = JsonConvert.DeserializeObject<User>(data);
                }
                return View(users);
            }
            catch (Exception ex)
            {
                
                Console.WriteLine($"An error occurred: {ex}");
                return RedirectToAction("Index"); 
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(User users)
        {
            try
            {
                string data = JsonConvert.SerializeObject(users);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = client.DeleteAsync(client.BaseAddress + "/Users/" + users.UserId).Result;

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    
                    var statusCode = response.StatusCode;
                    if (statusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        ModelState.AddModelError(string.Empty, "Invalid data. Please check your input.");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, $"Error deleting User. Status Code: {statusCode}");
                    }
                    return View(users);
                }
            }
            catch (Exception ex)
            {
                
                Console.WriteLine($"An error occurred: {ex}");
                ModelState.AddModelError(string.Empty, "An error occurred while deleting User. Please try again later.");
                return View(users);
            }
        }

    }
}
