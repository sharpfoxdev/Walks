using Microsoft.AspNetCore.Mvc;
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
    }
}
