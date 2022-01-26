using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using TabOrganizer___WebApp.Models;

namespace TabOrganizer___WebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly string apiBaseUrl;

        public AccountController(IConfiguration configuration)
        {
            _configuration = configuration;

            apiBaseUrl = _configuration.GetValue<string>("WebAPIBaseUrl");
        }
        // GET: AccountController
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> LoginUser(User user)
        {
            using (HttpClient client = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
                string endpoint = apiBaseUrl + "/users/authenticate";

                using (var Response = await client.PostAsync(endpoint, content))
                {
                    if (Response.StatusCode == System.Net.HttpStatusCode.OK)
                    {

                        string apiResponse = await Response.Content.ReadAsStringAsync();

                        var authUser = JsonConvert.DeserializeObject<User>(apiResponse);

                        HttpContext.Session.SetString("JWToken", authUser.Token);

                        return RedirectToAction("Index", "Home");

                    }
                    else
                    {
                        ModelState.Clear();
                        ModelState.AddModelError(string.Empty, "Username or Password is Incorrect");
                        return View("~/Views/Account/Index.cshtml");

                    }

                }
            }
        }

        [HttpPost]
        public async Task<IActionResult> RegisterUser(User user)
        {
            using (HttpClient client = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
                string endpoint = apiBaseUrl + "/users/register";

                using (var Response = await client.PostAsync(endpoint, content))
                {
                    if (Response.StatusCode == System.Net.HttpStatusCode.OK)
                    {

                        string apiResponse = await Response.Content.ReadAsStringAsync();

                       // var authUser = JsonConvert.DeserializeObject<User>(apiResponse);

                        //HttpContext.Session.SetString("JWToken", authUser.Token);

                        return RedirectToAction("Index");

                    }
                    else
                    {
                        ModelState.Clear();
                        ModelState.AddModelError(string.Empty, "Username or Password is Incorrect");
                        return View("~/Views/Account/Register.cshtml");

                    }
                }
            }
        }

        public IActionResult Logoff()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }



        // GET: AccountController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AccountController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AccountController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AccountController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AccountController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AccountController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
