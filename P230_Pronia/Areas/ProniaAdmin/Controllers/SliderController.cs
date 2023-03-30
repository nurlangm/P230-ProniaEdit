using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using P230_Pronia.DAL;
using P230_Pronia.Entities;

namespace P230_Pronia.Areas.ProniaAdmin.Controllers
{
    [Area("ProniaAdmin")]
    public class SliderController : Controller
    {
        private readonly ProniaDbContext _context;

        public SliderController(ProniaDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            IEnumerable<Slider>slider=_context.Sliders.AsEnumerable();
            return View(slider);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Create(Slider newSlider)
        {
            if (newSlider==null)
            {
                ModelState.AddModelError("Image","Please choose image");
                return View();
            }
            if (!newSlider.Image.ContentType.Contains("image/"))
            {
                ModelState.AddModelError("Image", "Please choose image type file");
                return View();
            }
            if ((double)newSlider.Image.Length/1024/1024>1)
            {
                ModelState.AddModelError("Image", "Please size has to be max 1MB");
                return View();
            }

            var rootPath = "C:\\Users\\memme\\OneDrive\\Desktop\\Pronia-BackendAdminPanel\\P230_Pronia\\wwwroot";
            var folderPath = Path.Combine(rootPath, "assets", "images", "website-images");
            Random r = new Random();
            int random = r.Next(0, 1000);

            var filaName = string.Concat(random,newSlider.Image.FileName);
            var path = Path.Combine(folderPath,filaName);



            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                await newSlider.Image.CopyToAsync(stream);
            }

            newSlider.ImagePath = filaName;
            _context.Sliders.Add(newSlider);
            _context.SaveChanges();



            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var slider = _context.Sliders.FirstOrDefault(c => c.Id == id);

            if (slider == null)
            {

                return NotFound();
            }


            return View(slider);
        }

        public IActionResult DeleteConfirmed(int id)
        {
            var slider = _context.Sliders.FirstOrDefault(c => c.Id == id);

            if (slider == null)
            {
                return NotFound();
            }

            _context.Sliders.Remove(slider);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id, Slider updatedSlider)
        {
            var slider = await _context.Sliders.FindAsync(id);

            if (slider == null)
            {
                return NotFound();
            }

            slider.PlantName = updatedSlider.PlantName;
            slider.Desc = updatedSlider.Desc;
            slider.Order = updatedSlider.Order;

            if (updatedSlider.Image == null)
            {
                ModelState.AddModelError("Image", "Please choose image");
                return View(slider);
            }

            if (!updatedSlider.Image.ContentType.Contains("image/"))
            {
                ModelState.AddModelError("Image", "Please choose image type file");
                return View(slider);
            }

            if ((double)updatedSlider.Image.Length / 1024 / 1024 > 1)
            {
                ModelState.AddModelError("Image", "Please size has to be max 1MB");
                return View(slider);
            }

            var rootPath = "C:\\Users\\memme\\OneDrive\\Desktop\\Pronia-BackendAdminPanel\\P230_Pronia\\wwwroot";
            var folderPath = Path.Combine(rootPath, "assets", "images", "website-images");
            Random r = new Random();
            int random = r.Next(0, 1000);

            var fileName = string.Concat(random, updatedSlider.Image.FileName);
            var path = Path.Combine(folderPath, fileName);

            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                await updatedSlider.Image.CopyToAsync(stream);
            }

            slider.ImagePath = fileName;

            _context.Entry(slider).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

    }
}
