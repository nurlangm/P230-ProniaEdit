using Microsoft.AspNetCore.Mvc;
using P230_Pronia.DAL;
using P230_Pronia.Entities;

namespace P230_Pronia.Areas.ProniaAdmin.Controllers
{
    [Area("ProniaAdmin")]
    public class TagController : Controller
    {
        
        private readonly ProniaDbContext _context;

        public TagController(ProniaDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            IEnumerable<Tag> tags = _context.Tags.AsEnumerable();
            return View(tags);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ActionName("Create")]
        [AutoValidateAntiforgeryToken]
        public IActionResult Creates(Tag newTag)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Name", "You cannot duplicate Tag name");
                return View(newTag);
            }
            _context.Tags.Add(newTag);
            _context.SaveChanges();
            Console.WriteLine("Hello World");
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Edit(int id)
        {
            if (id == 0) return NotFound();
            Tag tag = _context.Tags.FirstOrDefault(c => c.Id == id);
            if (tag is null) return NotFound();
            return View(tag);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Edit(int id, Tag edited)
        {
            if (id != edited.Id) return BadRequest();
            Tag tag = _context.Tags.FirstOrDefault(c => c.Id == id);
            if (tag is null) return NotFound();
            bool duplicate = _context.Tags.Any(c => c.Name == edited.Name);
            if (duplicate)
            {
                ModelState.AddModelError("", "You cannot duplicate Tag name");
                return View();
            }
            tag.Name = edited.Name;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }


        public IActionResult Delete(int id)
        {
            var tag = _context.Tags.FirstOrDefault(c => c.Id == id);

            if (tag == null)
            {

                return NotFound();
            }


            return View(tag);
        }
        public IActionResult DeleteConfirmed(int id)
        {
            var tag = _context.Tags.FirstOrDefault(c => c.Id == id);

            if (tag == null)
            {
                return NotFound();
            }

            _context.Tags.Remove(tag);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
        public IActionResult Details(int id)
        {

            var tag = _context.Tags.FirstOrDefault(c => c.Id == id);

            if (tag == null)
            {
                return NotFound();
            }

            return View(tag);
        }

    }
}
