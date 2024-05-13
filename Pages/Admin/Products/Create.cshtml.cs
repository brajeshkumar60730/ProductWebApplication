using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProductWebApplication.Models;
using ProductWebApplication.Services;

namespace ProductWebApplication.Pages.Admin.Products
{
    public class CreateModel : PageModel
    {
        private readonly IWebHostEnvironment environment;
        private readonly ApplicationDbContext context;

        [BindProperty]
        public ProductDto ProductDto { get; set; } = new ProductDto();

        public CreateModel(IWebHostEnvironment environment, ApplicationDbContext context)
        {
            this.environment = environment;
            this.context = context;
        }
        public void OnGet()
        {
        }

        public string errorMessage = "";
        public string successMessage = "";
        public void OnPost()
        {
            if (ProductDto.ImageFile == null)
            {
                ModelState.AddModelError("ProductDto.ImageFile", "The Image File is Required");
            }
            if (!ModelState.IsValid)
            {
                errorMessage = "Please Provide all the Required fields";
                return;
            }

            //save the image file
            string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            newFileName +=  Path.GetExtension(ProductDto.ImageFile!.FileName);
            
            string imageFullPath =  environment.WebRootPath + "/Products/" + newFileName;
            using(var stream = System.IO.File.Create(imageFullPath)) 
            { 
                ProductDto.ImageFile.CopyTo(stream);    
            }



            //save the new product in database

            Product product = new Product()
            {
                Name = ProductDto.Name,
                Brand = ProductDto.Brand,
                Category = ProductDto.Category,
                Price = ProductDto.Price,
                Description = ProductDto.Description ?? "",
                ImageFileName = newFileName,
                CreatedAt = DateTime.Now,

            };
            context.Products.Add(product);
            context.SaveChanges();




            //clear the form
            ProductDto.Name = "";
            ProductDto.Brand = "";
            ProductDto.Category = "";
            ProductDto.Price = 0;
            ProductDto.Description = "";
            ProductDto.ImageFile = null;

            ModelState.Clear();
            successMessage = "Product Create Successfullay";
            Response.Redirect("/Admin/Products/Index");

        }
    }
}
