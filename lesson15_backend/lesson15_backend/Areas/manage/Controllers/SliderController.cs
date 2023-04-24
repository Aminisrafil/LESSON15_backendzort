using lesson11_backend.DAL;
using lesson11_backend.Helpers;
using lesson11_backend.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace lesson11_backend.Areas.manage.Controllers
{
    [Area("manage")]
    public class SliderController : Controller
    {
        private readonly PustokDbContext _context;
        private readonly IWebHostEnvironment _env;

        public SliderController(PustokDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index()
        {
            var data = _context.Sliders.OrderBy(x=>x.Order).ToList();
            return View(data);
        }
        public IActionResult Create()
        {
            var order = _context.Sliders.Max(x => x.Order);
            ViewBag.Order = order + 1;
            return View();
        }
        [HttpPost]
        public IActionResult Create(Slider slider)
        {
            if (slider.ImageFile == null || (slider.ImageFile.ContentType != "image/jpeg" && slider.ImageFile.ContentType != "image/png"))
            {
                if (slider.ImageFile.ContentType != "image/jpeg" && slider.ImageFile.ContentType != "image/png")
                    ModelState.AddModelError("ImageFile", "ImageFile must be image/png or image/jpeg");
            }
               

            if (slider.ImageFile.Length > 2097152)
                ModelState.AddModelError("ImageFile", "ImageFile must be less or equal than 2MB");

            slider.Image = FileManager.Save(slider.ImageFile, _env.WebRootPath + "/uploads/slider");

            if (!ModelState.IsValid) return View();

            _context.Sliders.Add(slider);
            _context.SaveChanges();

            return RedirectToAction("index");
        }
        public IActionResult Edit(int id)
        {
            Slider slider = _context.Sliders.Find(id);

            if (slider == null)
                return View("Error");

            return View(slider);
        }

        [HttpPost]
        public IActionResult Edit(Slider slider)
        {
            if (!ModelState.IsValid) return View();

            Slider existSlider = _context.Sliders.Find(slider.Id);

            if (existSlider == null)
                return View("Error");

            existSlider.Title1 = slider.Title1;
            existSlider.Title2 = slider.Title2;
            existSlider.Order = slider.Order;
            existSlider.BtnText = slider.BtnText;
            existSlider.BtnUrl = slider.BtnUrl;
            existSlider.Desc = slider.Desc;

            if (slider.ImageFile != null)
            {
                if (existSlider.Image != null)
                {
                    string imagePath = Path.Combine(_env.WebRootPath, "/uploads/slider", existSlider.Image);
                    System.IO.File.Delete(imagePath);
                }

                if (slider.ImageFile.ContentType != "image/jpeg" && slider.ImageFile.ContentType != "image/png")
                    ModelState.AddModelError("ImageFile", "ImageFile must be image/png or image/jpeg");

                if (slider.ImageFile.Length > 2097152)
                    ModelState.AddModelError("ImageFile", "ImageFile must be less or equal than 2MB");

                existSlider.Image = FileManager.Save(slider.ImageFile, _env.WebRootPath + "/uploads/slider");
                
                _context.SaveChanges();

            }

            if (!ModelState.IsValid) return View();
            return RedirectToAction("index");
        }
        public IActionResult Delete(int id)
        {
            Slider slider = _context.Sliders.Find(id);

            if (slider == null)
                return View("Error");

            return View(slider);
        }
        [HttpPost]
        public IActionResult Delete(Slider slider)
        {
            Slider sliders = _context.Sliders.Find(slider.Id);

            if (sliders == null)
                return View("Error");

            // Delete the image file from the file system
            if (sliders.Image != null)
            {
                string imagePath = Path.Combine(_env.WebRootPath, "uploads/slider", sliders.Image);
                System.IO.File.Delete(imagePath);
            }

            // Remove the Slider object from the database and save changes
            _context.Sliders.Remove(sliders);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
