using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookLib.Entity;
using BookLib.Interface;
using Microsoft.AspNetCore.Mvc;

namespace BookLib.Controllers
{
    public class LibraryController : Controller
    {
        private readonly ILibraryService _libraryService;

        public LibraryController(ILibraryService libraryService)
        {
            _libraryService = libraryService;
        }
        public IActionResult Home()
        {
            var libraries = _libraryService.GetLibraries();
            return View(libraries);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Library library)
        {
            _libraryService.Create(library);
            return RedirectToAction("Home");
        }
        [HttpGet]
        public IActionResult Update(int id)
        {
            var lib = _libraryService.GetLibrary(id);
            return View(lib);
        }
        [HttpPost]
        public IActionResult Update(Library library)
        {
            _libraryService.Update(library);
            return RedirectToAction("Home");
        }
    }
}