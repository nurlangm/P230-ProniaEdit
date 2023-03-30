using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NuGet.Versioning;
using P230_Pronia.DAL;
using P230_Pronia.Entities;
using P230_Pronia.ViewModels.Cookie;

namespace P230_Pronia.Controllers
{
    public class PlantController : Controller
    {
        private readonly ProniaDbContext _context;

        public PlantController(ProniaDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            List<Plant> plants = _context.Plants.
                Include(p => p.PlantTags).ThenInclude(tp => tp.Tag).
                Include(p => p.PlantCategories).ThenInclude(cp => cp.Category).
                Include(p => p.PlantDeliveryInformation).
                 Include(p => p.PlantImages)
                .ToList();
            return View(plants);
        }

        public IActionResult Detail(int id)
        {
            if (id == 0) return NotFound();
            Plant? plant = _context.Plants
                                .Include(p => p.PlantImages)
                                    .Include(p => p.PlantDeliveryInformation)
                                        .Include(p => p.PlantTags)
                                            .ThenInclude(pt => pt.Tag)
                                                .Include(p => p.PlantCategories)
                                                    .ThenInclude(pc => pc.Category).FirstOrDefault(p => p.Id == id);



            List<Plant> modified = new List<Plant>();
            if (plant is null) return NotFound();

            foreach (var item in _context.Plants.Include(p => p.PlantImages).Include(x => x.PlantCategories).ThenInclude(x => x.Category))
            {
                foreach (PlantCategory plantCategory in item.PlantCategories)
                {
                    foreach (var cat in plant.PlantCategories)
                    {
                        if (plantCategory.Category.Id == cat.Category.Id && !modified.Contains(item))
                        {

                            modified.Add(item);

                        }
                    }
                }
            }

            ViewBag.RelatedPlant = modified.Where(x => x.Id != id).Take(4).ToList();

            return View(plant);

        }
        public IActionResult AddBasket(int id)
        {
            if (id <= 0) return NotFound();
            Plant plant=_context.Plants.FirstOrDefault(p => p.Id == id);
            if (plant is null) return NotFound();
            var cookies = HttpContext.Request.Cookies["basket"];
            CookieBasketVM basket= new();
            if (cookies is null)
            {
                CookieBasketItemVM item = new CookieBasketItemVM
                {
                    Id = plant.Id,
                    Quantity = 1,
                    Price = (double)plant.Price
                };
                basket.CookieBasketItemVMs.Add(item);
                basket.TotalPrice = (double)plant.Price;
            }
            else
            {
                basket = JsonConvert.DeserializeObject<CookieBasketVM>(cookies);
                CookieBasketItemVM existed = basket.CookieBasketItemVMs.Find(c => c.Id == id);
                if(existed is null)
                {
                    CookieBasketItemVM newItem = new CookieBasketItemVM
                    {
                        Id = plant.Id,
                        Quantity = 1,
                        Price=(double)plant.Price
                    };
                    basket.CookieBasketItemVMs.Add(newItem);
                    basket.TotalPrice += newItem.Price;
                }
                else
                {
                    existed.Quantity++;
                    basket.TotalPrice += existed.Price;
                }
                //basket.TotalPrice = basket.CookieBasketItemVMs.Sum(c => c.Quantity * c.Price);
            }
            var basketStr=JsonConvert.SerializeObject(basket);
            HttpContext.Response.Cookies.Append("basket", basketStr);
            return RedirectToAction("Index","Home");
          

          
         
        }
        public IActionResult ShowBasket()
        {
            var cookies = HttpContext.Request.Cookies["basket"];
            return Json(JsonConvert.DeserializeObject<CookieBasketVM>(cookies));
        }
        public IActionResult RemoveBasketItem(int id)
        {
            var cookies = HttpContext.Request.Cookies["basket"];
            var basket = JsonConvert.DeserializeObject<CookieBasketVM>(cookies);
            var item = basket.CookieBasketItemVMs.FirstOrDefault(i => i.Id == id);
            if (item is not null)
            {
                basket.CookieBasketItemVMs.Remove(item);
                basket.TotalPrice -= item.Price * item.Quantity;

                var basketStr = JsonConvert.SerializeObject(basket);
                HttpContext.Response.Cookies.Append("basket", basketStr);
            }

            return RedirectToAction("Index", "Home");
        }

    }
}
