using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using TabOrganizer___WebApp.Authentication;
using TabOrganizer___WebApp.Authorization;
using TabOrganizer___WebApp.Models;

namespace TabOrganizer___WebApp.Controllers
{
    public class ContainerController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly string apiBaseUrl;
        private static HttpClient Client = new HttpClient();

        public ContainerController(IConfiguration configuration)
        {
            _configuration = configuration;

            apiBaseUrl = _configuration.GetValue<string>("WebAPIBaseUrl");
        }

        // GET: ContainerController
        [Authorize(AuthenticationSchemes = SchemesNamesConst.TokenAuthenticationDefaultScheme)]
        public async Task<ActionResult> Index()
        {
            var containers = await GetAllContainers();
            return View("~/Views/Container/ContainersList.cshtml", containers);
        }

        public async Task<ActionResult> IndexPartial()
        {
            var containers = await GetAllContainers();
            return PartialView("ContainerListPartial", containers);
        }


        public async Task<IEnumerable<Container>> GetAllContainers()
        {
            string endpoint = apiBaseUrl + "/containers";
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

                    var containers = JsonConvert.DeserializeObject<IEnumerable<Container>>(apiResponse);

                    return containers;
                }
                else
                {
                    List<Models.Container> emptyContainers = null;
                    return emptyContainers;
                }
            }
        }


        public async Task<Container> GetContainerById(int id)
        {
            string endpoint = apiBaseUrl + "/containers/" + id;
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

                    var container = JsonConvert.DeserializeObject<Container>(apiResponse);

                    return container;
                }
                else
                {
                    Container emptyContainer = null;
                    return emptyContainer;
                }
            }
        }


        // GET: ContainerController/Create
        [Authorize(AuthenticationSchemes = SchemesNamesConst.TokenAuthenticationDefaultScheme)]
        public ActionResult Create()
        {
            var model = new Container { };

            return PartialView("CreateContainerPartial", model);

            //return View("~/Views/Container/CreateContainer.cshtml");
        }

        // POST: ContainerController/Create
        [Authorize(AuthenticationSchemes = SchemesNamesConst.TokenAuthenticationDefaultScheme)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Container container)
        {

            if (!TryValidateModel(container, nameof(container)))
            {
                return PartialView("CreateContainerPartial", container);
                //return View("~/Views/Container/CreateContainer.cshtml");
            }

            try
            {
                string endpoint = apiBaseUrl + "/containers";
                using (var requestMessage =
                new HttpRequestMessage(HttpMethod.Post, endpoint))
                {
                    StringValues value;
                    var authHeader = HttpContext.Request.Headers.TryGetValue("Authorization", out value);
                    AuthenticationHeaderValue authHeaders = new AuthenticationHeaderValue("Bearer", value.ToString());
                    requestMessage.Headers.Authorization = authHeaders;
                    requestMessage.Content = new StringContent(JsonConvert.SerializeObject(container), Encoding.UTF8, "application/json");

                    HttpResponseMessage response = new HttpResponseMessage();
                    response = await Client.SendAsync(requestMessage);

                    if (response.StatusCode == System.Net.HttpStatusCode.Created)
                    {

                        string apiResponse = await response.Content.ReadAsStringAsync();

                        var containerCreated = JsonConvert.DeserializeObject<Container>(apiResponse);

                        ViewBag.message = "Success! Container created.";

                        //return View("~/Views/Container/CreateContainer.cshtml");
                        return PartialView("CreateContainerPartial", container);
                    }
                    else
                    {
                        ModelState.Clear();
                        ModelState.AddModelError(string.Empty, "Container not created. Try again");
                        return PartialView("CreateContainerPartial");
                        //return View("~/Views/Container/CreateContainer.cshtml");
                    }
                }

            }
            catch
            {
                ModelState.Clear();
                ModelState.AddModelError(string.Empty, "Container not created. Try again");
                return PartialView("CreateContainerPartial");
                //return View("~/Views/Container/CreateContainer.cshtml");
            }
        }

        // GET: ContainerController/Edit/5
        [Authorize(AuthenticationSchemes = SchemesNamesConst.TokenAuthenticationDefaultScheme)]
        public async Task<ActionResult> Edit(int id)
        {
            // var model = new Container {Name="TestName", Description="Test description" };

            var model = await GetContainerById(id);

            return PartialView("EditContainerPartial", model);
        }

        // POST: ContainerController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, Container container)
        {
            if (!TryValidateModel(container, nameof(container)))
            {
                return PartialView("EditContainerPartial", container);
                //return View("~/Views/Container/CreateContainer.cshtml");
            }

            try
            {
                string endpoint = apiBaseUrl + "/containers/"+id;
                using (var requestMessage =
                new HttpRequestMessage(HttpMethod.Put, endpoint))
                {
                    StringValues value;
                    var authHeader = HttpContext.Request.Headers.TryGetValue("Authorization", out value);
                    AuthenticationHeaderValue authHeaders = new AuthenticationHeaderValue("Bearer", value.ToString());
                    requestMessage.Headers.Authorization = authHeaders;
                    requestMessage.Content = new StringContent(JsonConvert.SerializeObject(container), Encoding.UTF8, "application/json");

                    HttpResponseMessage response = new HttpResponseMessage();
                    response = await Client.SendAsync(requestMessage);

                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {

                        string apiResponse = await response.Content.ReadAsStringAsync();

                        var containerEdited = JsonConvert.DeserializeObject<Container>(apiResponse);

                        ViewBag.message = "Success! Container updated.";

                        //return View("~/Views/Container/CreateContainer.cshtml");
                        return PartialView("EditContainerPartial", containerEdited);
                    }
                    else
                    {
                        ModelState.Clear();
                        ModelState.AddModelError(string.Empty, "Container not updated. Try again");
                        return PartialView("EditContainerPartial");
                    }
                }

            }
            catch
            {
                ModelState.Clear();
                ModelState.AddModelError(string.Empty, "Container not updated. Try again");
                return PartialView("EditContainerPartial");
            }
        }

        public ActionResult Delete(int id)
        {
            return null;
            //AJAX
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            //AJAX
            return null;
        }
    }
}
