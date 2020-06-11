using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc;

namespace ApiTwo.Controllers
{
    public class HomeController : Controller
    {
		private readonly IHttpClientFactory _httpClient;

		public HomeController(IHttpClientFactory httpClientFactory)
		{
			_httpClient = httpClientFactory;
		}

		[Route("/home")]
        public async Task<IActionResult> Index()
        {
			// retrieve access token
			var serverClient = _httpClient.CreateClient();

			var discoveryDocument = await serverClient.GetDiscoveryDocumentAsync("https://localhost:44357/");

			var tokenresponse = await serverClient.RequestClientCredentialsTokenAsync(
				new ClientCredentialsTokenRequest 
				{
					Address =discoveryDocument.TokenEndpoint,
					ClientId = "client_id",
					ClientSecret ="client_secret",

					Scope = "ApiOne"
				});

			// retrieve secret data 
			var apiClient = _httpClient.CreateClient();

			apiClient.SetBearerToken(tokenresponse.AccessToken);

			var response = await apiClient.GetAsync("https://localhost:44360/secret");

			var content = await response.Content.ReadAsStringAsync();

            return Ok(new 
			{ 
				access_token = tokenresponse.AccessToken,
				message = content,
			});
        }
    }
}