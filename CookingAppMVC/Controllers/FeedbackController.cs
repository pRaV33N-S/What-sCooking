using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CookingAppMVC.Models;
using Newtonsoft.Json;
using System.Text;
using System.Data;
using Microsoft.AspNetCore.Authorization;

namespace CookingAppMVC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class FeedbackController : Controller
    {
        Uri baseAddress = new Uri("http://localhost:5006/api");
        HttpClient client;

        public FeedbackController()
        {
            client = new HttpClient();
            client.BaseAddress = baseAddress;
        }

        public ActionResult Index()
        {
            List<Feedback> feedbacks = new List<Feedback>();
            HttpResponseMessage response = client.GetAsync(client.BaseAddress + "/Feedbacks").Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                feedbacks = JsonConvert.DeserializeObject<List<Feedback>>(data);
            }
            return View(feedbacks);
        }


        [HttpGet]
        public ActionResult Details(int id)
        {
            Feedback feedback = new Feedback();
            try
            {
                HttpResponseMessage response = client.GetAsync(client.BaseAddress + "/Feedbacks/" + id).Result;
                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    feedback = JsonConvert.DeserializeObject<Feedback>(data);
                }
                return View(feedback);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex}");
                return RedirectToAction("Index");
            }
        }

            [HttpGet]
        public ActionResult Delete(int id)
        {
            Feedback feedbacks = new Feedback();
            try
            {
                HttpResponseMessage response = client.GetAsync(client.BaseAddress + "/Feedbacks/" + id).Result;
                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    feedbacks = JsonConvert.DeserializeObject<Feedback>(data);
                }
                return View(feedbacks);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex}");
                return RedirectToAction("Index");
            }

        }

        [HttpPost]
        public ActionResult Delete(Feedback feedbacks)
        {
            try
            {
                string data = JsonConvert.SerializeObject(feedbacks);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = client.DeleteAsync(client.BaseAddress + "/Feedbacks/" + feedbacks.FeedbackID).Result;

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                return View(feedbacks);
            }
            catch (Exception ex)
            {

                Console.WriteLine($"An error occurred: {ex}");
                ModelState.AddModelError(string.Empty, "An error occurred while deleting Feedback. Please try again later.");
                return View(feedbacks);
            }

        }
    }
}