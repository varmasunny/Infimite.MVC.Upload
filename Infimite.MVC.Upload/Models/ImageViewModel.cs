using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Infimite.MVC.Upload.Models
{
    public class ImageViewModel
    {
        public int Id { get; set; }
        [Required]
        public IFormFile Images { get; set; }
        public string ImageUrl { get; set; }
        public string ImageName { get; set; }

    }
}
