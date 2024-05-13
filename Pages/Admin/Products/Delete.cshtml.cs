using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProductWebApplication.Services;

namespace ProductWebApplication.Pages.Admin.Products
{
    public class DeleteModel : PageModel
    {
        private readonly IWebHostEnvironment environment;
        private readonly ApplicationDbContext context;

        public DeleteModel(IWebHostEnvironment environment, ApplicationDbContext context)
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
            string ImageFullPath = environment.WebRootPath + "/Products/" + product.ImageFileName;
            System.IO.File.Delete(ImageFullPath);
            context.Products.Remove(product);
            context.SaveChanges();
            Response.Redirect("/Admin/Products/Index");
        }
    }
}
