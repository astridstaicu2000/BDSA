using MagazinOnline.Models.Entities;
using MagazinOnline.Models.VMs;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace MagazinOnline.Controllers
{
    public class HomeController : Controller
    {
        MagazinOnlineContext context;
        public HomeController(MagazinOnlineContext context)
        {
            this.context = context;
        }

        public IActionResult Index()
        {
            var list = context.Products.Select(p => new ProductVM().ProdToProdVM(p)).ToList();
            return View(list);
        }

        [HttpGet]
        [Route("Details/{id}")]
        public IActionResult Details(int id)
        {
            var product = context.Products.FirstOrDefault(p => p.Id == id);
            return View(product);
        }

        [HttpPost]
        [Route("Add/{id}")]
        public IActionResult Add(int id)
        {
            var product = context.Products.FirstOrDefault(p => p.Id == id);
            var cartItem = new CartItem(product.Id, product.Name, product.Price);

            var shopList = HttpContext.Session.Get<List<CartItem>>(SessionHelper.ShoppingCart);

            if (shopList == null)
                shopList = new List<CartItem>();

            if (!shopList.Select(item => item.Id).ToList().Contains(id))
                shopList.Add(cartItem);

            HttpContext.Session.Set(SessionHelper.ShoppingCart, shopList);

            return RedirectToAction("Index", "Home", Product.GetAll());
        }

        [HttpPost]
        [Route("Remove/{id}")]
        public IActionResult Remove(int id)
        {
            var product = context.Products.FirstOrDefault(p => p.Id == id);
            var cartItem = new CartItem(product.Id, product.Name, product.Price);
            var shopList = HttpContext.Session.Get<List<CartItem>>(SessionHelper.ShoppingCart);

            if (shopList == null)
                return RedirectToAction("Index", "Home", context.Products);

            if (shopList.Select(item => item.Id).ToList().Contains(id))
                shopList.RemoveAll(item => item.Id == id);

            HttpContext.Session.Set(SessionHelper.ShoppingCart, shopList);

            return RedirectToAction("Index", "Home", context.Products);
        }
    }
}