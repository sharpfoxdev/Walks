using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using Walks.UI.Models;
using Walks.UI.Models.DTO;

namespace Walks.UI.Controllers {
    public class RegionsController : Controller {
        private readonly IHttpClientFactory httpClientFactory;

        public RegionsController(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }
        public async Task<IActionResult> Index() {

            List<RegionDto> response = new List<RegionDto>();

            try {
                var client = httpClientFactory.CreateClient();
                // base url should come ideally from appsettings.json
                // (url can differ for debug and release)
                var httpResponseMessage = await client.GetAsync("https://localhost:7275/api/regions");

                //throws an exception if we dont get successful response
                httpResponseMessage.EnsureSuccessStatusCode();

                response.AddRange(await httpResponseMessage.Content.ReadFromJsonAsync<IEnumerable<RegionDto>>());
            }
            catch (Exception ex) {
                // log the exception
                throw;
            }

            return View(response);
        }
        [HttpGet]
        public IActionResult Add() {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(AddRegionViewModel addRegionViewModel) {
            var client = httpClientFactory.CreateClient();
            var httpRequestMessage = new HttpRequestMessage() {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://localhost:7275/api/regions"),
                Content = new StringContent(JsonSerializer.Serialize(addRegionViewModel), Encoding.UTF8, "application/json")
            };
            var httpResponseMessage = await client.SendAsync(httpRequestMessage);

            httpResponseMessage.EnsureSuccessStatusCode();

            var response = await httpResponseMessage.Content.ReadFromJsonAsync<RegionDto>();
            if(response != null) {
                return RedirectToAction("Index", "Regions");
            }
            return View();
		}
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id) {
            var client = httpClientFactory.CreateClient();
            var response = await client.GetFromJsonAsync<RegionDto>($"https://localhost:7275/api/regions/{id.ToString()}");
            if(response is not null) {
                return View(response);
            }
            
            return View(null);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(RegionDto regionDto) {
            var client = httpClientFactory.CreateClient();
            var httpRequestMessage = new HttpRequestMessage() {
                Method = HttpMethod.Put,
                RequestUri = new Uri($"https://localhost:7275/api/regions/{regionDto.Id}"),
                Content = new StringContent(JsonSerializer.Serialize(regionDto), Encoding.UTF8, "application/json")
            };

            var httpResponseMessage = await client.SendAsync(httpRequestMessage);
            httpResponseMessage.EnsureSuccessStatusCode();
            var response = await httpRequestMessage.Content.ReadFromJsonAsync<RegionDto>();
            if(response != null) {
                return RedirectToAction("Edit", "Regions");
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Delete(RegionDto regionDto) {
			var client = httpClientFactory.CreateClient();
            var httpResponseMessage = await client
                .DeleteAsync($"https://localhost:7275/api/regions/{regionDto.Id}");
            httpResponseMessage.EnsureSuccessStatusCode();
            return RedirectToAction("Index", "Regions");
		}
    }
}
