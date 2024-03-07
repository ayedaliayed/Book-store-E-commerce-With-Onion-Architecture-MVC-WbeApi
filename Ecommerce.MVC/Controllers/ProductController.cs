using Ecommerce.Application.Services;
using Ecommerce.Dtos.Product;
using Ecommerce.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.MVC.Controllers
{
    public class ProductController : Controller
    {
        private IProductService _productService;
        private ICategoryService _categoryService;
       
        public ProductController(IProductService productService,ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
           
            
        }

        // GET: ProductController
        public async Task<ActionResult> Index()
        {
            var products = await (_productService.GetAllPagination(10,1));
            List< GetAllProductDTO > productsDTO = products.Entities.ToList();

            ViewBag.Count= products.Count;
            return View(productsDTO);
        }

        // GET: ProductController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ProductController/Create
        public async Task< ActionResult> Create()
        {
            var cat = await(_categoryService.GetAllPagination());
            ViewBag.Cat = cat;
            return View();
        }

        // POST: ProductController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateOrUpdateProductDTO product)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _productService.Create(product);

                    if (result.IsSuccess)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.Error = result.Message;
                    }
                }

                // If ModelState is not valid, return the view with the provided data
                return View(product);
            }
            catch (Exception ex)
            {
                // Return an error view or redirect to a generic error page
                ViewBag.Error = "An unexpected error occurred.";
                return View("Error");
            }
        }


        // GET: ProductController/Edit/5
        public async Task <ActionResult> Edit(int id)
        {
           var prd=await _productService.GetOne(id);
           var cat = await (_categoryService.GetAllPagination());
           ViewBag.Cat = cat;

            return View(prd.Enttiy);
        }

        // POST: ProductController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, CreateOrUpdateProductDTO product)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _productService.Update(product);

                    if (result.IsSuccess)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.Error = result.Message;
                    }
                }

                // If ModelState is not valid, return the view with the provided data
                return View(product);
            }
            catch (Exception ex)
            {
                // Return an error view or redirect to a generic error page
                ViewBag.Error = "An unexpected error occurred.";
                var cat = await (_categoryService.GetAllPagination());
                ViewBag.Cat = cat;
                return View();
            }
        }



        //Response.Cookies.Append("Name", Name);

        // HttpContext.Session.SetString("Name", Name);
        public async  Task<ActionResult> AddToCart(int id)
        {

            List<CreateOrUpdateProductDTO> cart = new List<CreateOrUpdateProductDTO>();
            var product = await _productService.GetOne(id);

            //use extantion method for adding list to Session
            var cartProducts = HttpContext.Session.GetObject<List<CreateOrUpdateProductDTO>>("Cart");
            if(cartProducts!=null)
            {
                foreach (var p in cartProducts)
                {
                    cart.Add(p);
                }
            }
           
            cart.Add(product.Enttiy);
            HttpContext.Session.SetObject("Cart", cart);
            return RedirectToAction("Index"); 
        }

        public IActionResult ShowCard()
        {
            // Retrieve the list from the session
            var cart = HttpContext.Session.GetObject<List<CreateOrUpdateProductDTO>>("Cart");

            // You may want to check for null or handle the case where "Cart" is not in the session

            // Pass the cart to the view
            return View(cart);
        }


        // GET: ProductController/Delete/5
        public async Task<ActionResult> HardDelete(int id)
        {
            var prd = await _productService.GetOne(id);
            return View(prd.Enttiy);
        }

        // POST: ProductController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> HardDelete(int id, CreateOrUpdateProductDTO product)
        {
            try
            {
               var p=await _productService.HardDelete(product);
                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.Error = "An unexpected error occurred.";
                return View();


            }
        }

        public async Task<ActionResult> SoftDelete(int id)
        {
            try
            {
                var prd = await _productService.GetOne(id);
                _productService.SoftDelete(prd.Enttiy);

                return RedirectToAction("Index");
            }

            catch
            {
                return View("Error");
            }
            

        }







    }
}
