using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System;
using Infimite.MVC.Upload.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Infimite.MVC.Upload.Controllers
{
    public class ImageController : Controller
    {
        private readonly IConfiguration _configuration;

        public ImageController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<IActionResult> Index()
        {
            List<ImageViewModel> images = new();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                var result = await client.GetAsync("Image/GetAllUploadImages");
                if (result.IsSuccessStatusCode)
                {
                    images = await result.Content.ReadAsAsync<List<ImageViewModel>>();
                }
            }
            return View(images);
        }
        [HttpGet]
        public IActionResult Create()
        {

            
            return View();
        }






        [HttpPost]
        public async Task<IActionResult> Create(ImageViewModel model)
        {
            if (ModelState.IsValid)
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images", model.Images.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.Images.CopyToAsync(stream);
                }

                model.ImageUrl = "/Images/" + model.Images.FileName;
            }
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_configuration["ApiUrl:api"]);
                var result = await client.PostAsJsonAsync("Image/CreateImage", model);
                if (result.StatusCode == System.Net.HttpStatusCode.Created)
                {
                    return RedirectToAction("Index", "Image");

                }
            }
            return View(model);

        }
    }
}
