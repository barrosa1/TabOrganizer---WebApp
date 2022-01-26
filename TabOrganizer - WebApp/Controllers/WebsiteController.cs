using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using TabOrganizer___WebApp.Authentication;
using TabOrganizer___WebApp.Models;

namespace TabOrganizer___WebApp.Controllers
{
    public class WebsiteController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly string apiBaseUrl;
        private static HttpClient Client = new HttpClient();

        public WebsiteController(IConfiguration configuration)
        {
            _configuration = configuration;

            apiBaseUrl = _configuration.GetValue<string>("WebAPIBaseUrl");
        }

        // GET: WebsiteController
        //[HttpGet("website/{containerId}")]
        [Authorize(AuthenticationSchemes = SchemesNamesConst.TokenAuthenticationDefaultScheme)]
        public async Task<ActionResult> GetWebsites([FromQuery] int id)
        {
            var websites = await GetWebsitesFromApi(id);
            return View("~/Views/Website/WebsitesList.cshtml", websites);
        }


        public async Task<IEnumerable<Website>> GetWebsitesFromApi(int containerId)
        {
            string endpoint = apiBaseUrl + "/websites/" + containerId;
            using (var requestMessage =
            new HttpRequestMessage(HttpMethod.Get, endpoint))
            {
                StringValues value;
                var authHeader = HttpContext.Request.Headers.TryGetValue("Authorization", out value);
                AuthenticationHeaderValue authHeaders = new AuthenticationHeaderValue("Bearer", value.ToString());
                requestMessage.Headers.Authorization = authHeaders;

                HttpResponseMessage response = new HttpResponseMessage();
                response = await Client.SendAsync(requestMessage);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {

                    string apiResponse = await response.Content.ReadAsStringAsync();

                    var websites = JsonConvert.DeserializeObject<IEnumerable<Website>>(apiResponse);

                    return websites;
                }
                else
                {
                    List<Website> emptyWebsites = null;
                    return emptyWebsites;
                }
            }
        }

        public async virtual Task<ActionResult> WebsiteDetails(int containerId, int websiteId)
        {
            var website = await GetWebsiteFromApi(containerId, websiteId);
            return PartialView("~/Views/Website/WebsitePartial.cshtml", website);
        }

        public async Task<Website> GetWebsiteFromApi(int containerId, int websiteId)
        {
            string endpoint = apiBaseUrl + "/websites/" + containerId + "/" + websiteId;
            using (var requestMessage =
            new HttpRequestMessage(HttpMethod.Get, endpoint))
            {
                StringValues value;
                var authHeader = HttpContext.Request.Headers.TryGetValue("Authorization", out value);
                AuthenticationHeaderValue authHeaders = new AuthenticationHeaderValue("Bearer", value.ToString());
                requestMessage.Headers.Authorization = authHeaders;

                HttpResponseMessage response = new HttpResponseMessage();
                response = await Client.SendAsync(requestMessage);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();

                    var website = JsonConvert.DeserializeObject<Website>(apiResponse);

                    return website;
                }
                else
                {
                    Website emptyWebsite = null;
                    return emptyWebsite;
                }
            }
        }



        // GET: WebsiteController/Create
        [Authorize(AuthenticationSchemes = SchemesNamesConst.TokenAuthenticationDefaultScheme)]
        public ActionResult Create()
        {
            return PartialView("CreateWebsitePartial");
        }

        // POST: WebsiteController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(int containerId, Website website)
        {
            containerId = int.Parse(Request.Headers["Referer"].ToString().Split('=')[1]);
            if (!TryValidateModel(website, nameof(website)))
            {
                return PartialView("CreateWebsitePartial", website);
            }

            try
            {
                string endpoint = apiBaseUrl + "/websites/" + containerId;
                using (var requestMessage =
                new HttpRequestMessage(HttpMethod.Post, endpoint))
                {
                    StringValues value;
                    var authHeader = HttpContext.Request.Headers.TryGetValue("Authorization", out value);
                    AuthenticationHeaderValue authHeaders = new AuthenticationHeaderValue("Bearer", value.ToString());
                    requestMessage.Headers.Authorization = authHeaders;
                    requestMessage.Content = new StringContent(JsonConvert.SerializeObject(website), Encoding.UTF8, "application/json");

                    HttpResponseMessage response = new HttpResponseMessage();
                    response = await Client.SendAsync(requestMessage);

                    if (response.StatusCode == System.Net.HttpStatusCode.Created)
                    {

                        string apiResponse = await response.Content.ReadAsStringAsync();

                        var websiteCreated = JsonConvert.DeserializeObject<Website>(apiResponse);

                        ViewBag.message = "Success! Website created.";

                        return PartialView("CreateWebsitePartial", websiteCreated);
                    }
                    else
                    {
                        ModelState.Clear();
                        ModelState.AddModelError(string.Empty, "Website not created. Try again");
                        return PartialView("CreateWebsitePartial");
                    }
                }

            }
            catch
            {
                ModelState.Clear();
                ModelState.AddModelError(string.Empty, "Website not created. Try again");
                return PartialView("CreateWebsitePartial");
            }
        }

        [Authorize(AuthenticationSchemes = SchemesNamesConst.TokenAuthenticationDefaultScheme)]
        public async Task<ActionResult> Edit(int containerId, int id)
        {

            var model = await GetWebsiteFromApi(containerId, id);

            return PartialView("EditWebsitePartial", model);
        }

        // POST: WebsiteController/Edit/5/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int containerId, int id, Website website)
        {
            if (!TryValidateModel(website, nameof(website)))
            {
                return PartialView("EditWebsitePartial", website);
            }

            try
            {
                string endpoint = apiBaseUrl + "/websites/" + containerId + "/" + id;
                using (var requestMessage =
                new HttpRequestMessage(HttpMethod.Put, endpoint))
                {
                    StringValues value;
                    var authHeader = HttpContext.Request.Headers.TryGetValue("Authorization", out value);
                    AuthenticationHeaderValue authHeaders = new AuthenticationHeaderValue("Bearer", value.ToString());
                    requestMessage.Headers.Authorization = authHeaders;
                    requestMessage.Content = new StringContent(JsonConvert.SerializeObject(website), Encoding.UTF8, "application/json");

                    HttpResponseMessage response = new HttpResponseMessage();
                    response = await Client.SendAsync(requestMessage);

                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {

                        string apiResponse = await response.Content.ReadAsStringAsync();

                        var websiteEdited = JsonConvert.DeserializeObject<Website>(apiResponse);

                        ViewBag.message = "Success! Website updated.";

                        return PartialView("EditWebsitePartial", websiteEdited);
                    }
                    else
                    {
                        ModelState.Clear();
                        ModelState.AddModelError(string.Empty, "Website not updated. Try again");
                        return PartialView("EditWebsitePartial", website);
                    }
                }
            }
            catch
            {
                ModelState.Clear();
                ModelState.AddModelError(string.Empty, "Website not updated. Try again");
                return PartialView("EditWebsitePartial", website);
            }
        }

        // GET: WebsiteController/Delete/5
        public async Task<ActionResult> Delete(int containerId, int id)
        {
            //await DeletePost(containerId, id);
            return new EmptyResult();
        }

        // POST: WebsiteController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeletePost(int containerId, int id)
        {
            string containerIdFromUrl = Request.Headers["Referer"].ToString().Split('=')[1];
            containerId = int.Parse(containerIdFromUrl);
            try
            {
                string endpoint = apiBaseUrl + "/websites/" + containerId + "/" + id;
                using (var requestMessage =
                new HttpRequestMessage(HttpMethod.Delete, endpoint))
                {
                    StringValues value;
                    var authHeader = HttpContext.Request.Headers.TryGetValue("Authorization", out value);
                    AuthenticationHeaderValue authHeaders = new AuthenticationHeaderValue("Bearer", value.ToString());
                    requestMessage.Headers.Authorization = authHeaders;

                    HttpResponseMessage response = new HttpResponseMessage();
                    response = await Client.SendAsync(requestMessage);

                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {

                        ViewBag.message = "Success! Website deleted.";
                        return NoContent();
                        //var websites = await GetWebsitesFromApi(containerId);
                        //return View("~/Views/Website/WebsitesList.cshtml", websites);
                    }
                    else
                    {
                        ModelState.Clear();
                        ModelState.AddModelError(string.Empty, "Website not deleted. Try again");
                        return NoContent();
                        // var websites = await GetWebsitesFromApi(containerId);
                        //return View("~/Views/Website/WebsitesList.cshtml", websites);
                    }
                }
            }
            catch
            {
                return NoContent();
                //var websites = await GetWebsitesFromApi(containerId);
                //return View("~/Views/Website/WebsitesList.cshtml", websites);
            }
        }
    }
}
