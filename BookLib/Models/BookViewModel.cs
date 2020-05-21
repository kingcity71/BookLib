using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookLib.Models
{
    public class BookViewModel
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PhotoBase64 { get; set; }
        public IFormFile Photo{ get; set; }
        public int LibId { get; set; }
        public IEnumerable<SelectListItem> Libs{ get; set; }
    }
}
