using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProductWebApplication.Models;
using ProductWebApplication.Services;

namespace ProductWebApplication.Pages.Admin.Products
{
    public class EditModel : PageModel
    {
        private readonly IWebHostEnvironment environment;
        private readonly ApplicationDbContext context;

        [BindProperty]
        public ProductDto ProductDto { get; set; } = new ProductDto();
        public Product Product { get; set; } = new Product();

        public string errorMessage = "";
        public string successMessage = "";
        public EditModel(IWebHostEnvironment environment, ApplicationDbContext context)
        {
            this.environment = environment;
            this.context = context;
        }
        public void OnGet(int? id)  
        {
            if (id == null)
            {
                Response.Redirect("/Admin/Products/Index");
                return;
            }
            var product = context.Products.Find(id);
            if (product == null)
            {
                Response.Redirect("/Admin/Products/Index");
                return;
            }
            ProductDto.Name = product.Name;
            ProductDto.Brand = product.Brand;
            ProductDto.Category = product.Category;
            ProductDto.Price = product.Price;
            ProductDto.Description = product.Description;
            Product = product;
        }
        public void OnPost(int? id)
        {
            if (id == null)
            {
                Response.Redirect("/Admin/Products/Index");
                return;
            }
            if (!ModelState.IsValid)
            {
                errorMessage = "Please Provide all the Required fields";
                return;
            }
            var product = context.Products.Find(id);
            if (product == null)
            {
                Response.Redirect("/Admin/Products/Index");
                return;
            }
            //Update the imagefile if we have a new image file
            string newFileName = product.ImageFileName;
            if(ProductDto.ImageFile !=  null)
            {
                newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                newFileName += Path.GetExtension(ProductDto.ImageFile!.FileName);
                string imageFullPath = environment.WebRootPath + "/Products/" + newFileName;
                using (var stream = System.IO.File.Create(imageFullPath))
                {
                    ProductDto.ImageFile.CopyTo(stream);
                }
                //delete the old image
                string oldImageFullPath = environment.WebRootPath + "/Products/" + product.ImageFileName;
                System.IO.File.Delete(oldImageFullPath);
            }



            //Update the product in the database

            product.Name = ProductDto.Name;
            product.Brand = ProductDto.Brand;
            product.Category = ProductDto.Category;
            product.Price = ProductDto.Price;
            product.Description = ProductDto.Description ?? "";
            product.ImageFileName = newFileName;
            context.SaveChanges();


            Product = product;
            successMessage = "Product Update successfully";
            Response.Redirect("/Admin/Products/Index");
        }
    }
}
