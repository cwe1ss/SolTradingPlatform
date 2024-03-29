﻿using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace MeiShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductListController : ControllerBase
    {
        public static string ProductCatalogServiceBaseAddress = "https://ieg-product-catalog.azurewebsites.net";

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(ProductCatalogServiceBaseAddress);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = client.GetAsync(ProductCatalogServiceBaseAddress + "/api/ProductList").Result;
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            return Ok(content);
        }
    }
}
