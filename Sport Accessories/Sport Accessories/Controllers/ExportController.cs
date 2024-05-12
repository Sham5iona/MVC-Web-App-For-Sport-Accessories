using DocumentFormat.OpenXml.Bibliography;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sport_Accessories.Areas.Identity.Models;
using Sport_Accessories.Data;
using Sport_Accessories.Models;

namespace Sport_Accessories.Controllers
{
    public class ExportController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<User> _userManager;

        public ExportController(ApplicationDbContext dbContext,
                                UserManager<User> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public async Task<IActionResult> ExportOffersToPdfAsync()
        {
            try
            {
                // Get all the available products and relations with the other tables
                IList<Product> products = await _dbContext.Products
                     .Include(p => p.Category)
                     .Include(p => p.User)
                     .ToListAsync();

                using (var pdfStream = new MemoryStream())
                {
                    // Create a new document
                    using (var document = new Document())
                    {
                        // Create a PdfWriter instance to write to the memory stream
                        PdfWriter.GetInstance(document, pdfStream);

                        // Open the document
                        document.Open();

                        // Add content to the document
                        foreach (var product in products)
                        {
                            // Add a new page for each product (except for the first one)
                            if (products.IndexOf(product) != 0)
                            {
                                document.NewPage();
                            }

                            // Add product details as paragraphs
                            document.Add(new Paragraph($"Product name: {product.ProductName}", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12)));
                            document.Add(new Paragraph("\n"));
                            document.Add(new Paragraph($"Category: {product.Category?.CategoryName}", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10)));
                            document.Add(new Paragraph("\n"));
                            document.Add(new Paragraph($"Price: {product.Price:C}", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10)));
                            document.Add(new Paragraph("\n"));
                            document.Add(new Paragraph($"Description: {product.ProductDescription}", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10)));
                            document.Add(new Paragraph("\n"));
                            document.Add(new Paragraph($"Updated at: {product.UpdatedAt}", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10)));
                            document.Add(new Paragraph("\n"));
                            document.Add(new Paragraph($"Viewers: {product.Viewers}", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10)));
                            document.Add(new Paragraph("\n"));
                            document.Add(new Paragraph($"Is promo?: {product.IsPromo}", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10)));
                            document.Add(new Paragraph("\n"));
                            document.Add(new Paragraph($"Username: {product.User?.UserName}", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10)));
                            document.Add(new Paragraph("\n"));
                            document.Add(new Paragraph($"User email: {product.User?.Email}", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10)));
                            document.Add(new Paragraph("\n"));
                        }

                        // Close the PdfWriter to finalize the document
                        document.Close();
                    }

                    var content = pdfStream.ToArray();
                    // Return the PDF file
                    return File(content, "application/pdf", "Offers.pdf");
                }
            }
            catch (DocumentException de)
            {
                // Handle DocumentException
                Console.Error.WriteLine(de.Message);
                // Return an appropriate error response
                return BadRequest();
            }
        }

        public async Task<IActionResult> ExportCategoriesToPdfAsync()
        {
            try
            {
                // Get all the available products and relations with the other tables
                IList<Category> categories = await _dbContext.Categories
                                            .ToListAsync();

                using (var pdfStream = new MemoryStream())
                {
                    // Create a new document
                    using (var document = new Document())
                    {
                        // Create a PdfWriter instance to write to the memory stream
                        PdfWriter.GetInstance(document, pdfStream);

                        // Open the document
                        document.Open();

                        // Add content to the document
                        foreach (var category in categories)
                        {
                            // Add a new page for each product (except for the first one)
                            if (categories.IndexOf(category) != 0)
                            {
                                document.NewPage();
                            }

                            // Add product details as paragraphs
                            document.Add(new Paragraph($"Category name: {category.CategoryName}", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12)));
                            document.Add(new Paragraph("\n"));
                            document.Add(new Paragraph($"Is category active: {category.IsActive}", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10)));
                            document.Add(new Paragraph("\n"));
                            document.Add(new Paragraph($"Last modified: {category.LastModified_20118018}", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10)));
                            document.Add(new Paragraph("\n"));

                        }

                        // Close the PdfWriter to finalize the document
                        document.Close();
                    }

                    var content = pdfStream.ToArray();
                    // Return the PDF file
                    return File(content, "application/pdf", "Categories.pdf");
                }
            }
            catch (DocumentException de)
            {
                // Handle DocumentException
                Console.Error.WriteLine(de.Message);
                // Return an appropriate error response
                return BadRequest();
            }
        }
        public async Task<IActionResult> ExportUsersToPdfAsync()
        {
            try
            {
                // Get all the available products and relations with the other tables
                IList<User> users = await _dbContext.Users
                                            .ToListAsync();

                using (var pdfStream = new MemoryStream())
                {
                    // Create a new document
                    using (var document = new Document())
                    {
                        // Create a PdfWriter instance to write to the memory stream
                        PdfWriter.GetInstance(document, pdfStream);

                        // Open the document
                        document.Open();

                        // Add content to the document
                        foreach (var user in users)
                        {
                            // Add a new page for each product (except for the first one)
                            if (users.IndexOf(user) != 0)
                            {
                                document.NewPage();
                            }

                            // Add product details as paragraphs
                            document.Add(new Paragraph($"Username: {user.UserName}", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12)));
                            document.Add(new Paragraph("\n"));
                            document.Add(new Paragraph($"Email address: {user.Email}", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10)));
                            document.Add(new Paragraph("\n"));
                            document.Add(new Paragraph($"Email confirmed: {user.EmailConfirmed}", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10)));
                            document.Add(new Paragraph("\n"));
                            document.Add(new Paragraph($"Access failed count: {user.AccessFailedCount}", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10)));
                            document.Add(new Paragraph("\n"));
                            document.Add(new Paragraph($"Lockout enabled: {user.LockoutEnabled}", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10)));
                            document.Add(new Paragraph("\n"));
                            document.Add(new Paragraph($"Lockout end: {user.LockoutEnd}", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10)));
                            document.Add(new Paragraph("\n"));
                            document.Add(new Paragraph($"Is 2FA enabled: {user.TwoFactorEnabled}", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10)));
                            document.Add(new Paragraph("\n"));
                            document.Add(new Paragraph($"Last modified: {user.LastModified_20118018}", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10)));
                            document.Add(new Paragraph("\n"));

                        }

                        // Close the PdfWriter to finalize the document
                        document.Close();
                    }

                    var content = pdfStream.ToArray();
                    // Return the PDF file
                    return File(content, "application/pdf", "Users.pdf");
                }
            }
            catch (DocumentException de)
            {
                // Handle DocumentException
                Console.Error.WriteLine(de.Message);
                // Return an appropriate error response
                return BadRequest();
            }
        }

        public async Task<IActionResult> ExportAdminsToPdfAsync()
        {
            try
            {
                // Get all the available products and relations with the other tables
                IList<User> users = await _userManager.GetUsersInRoleAsync("Admin");

                using (var pdfStream = new MemoryStream())
                {
                    // Create a new document
                    using (var document = new Document())
                    {
                        // Create a PdfWriter instance to write to the memory stream
                        PdfWriter.GetInstance(document, pdfStream);

                        // Open the document
                        document.Open();

                        // Add content to the document
                        foreach (var user in users)
                        {
                            // Add a new page for each product (except for the first one)
                            if (users.IndexOf(user) != 0)
                            {
                                document.NewPage();
                            }

                            // Add product details as paragraphs
                            document.Add(new Paragraph($"Username: {user.UserName}", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12)));
                            document.Add(new Paragraph("\n"));
                            document.Add(new Paragraph($"Email address: {user.Email}", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10)));
                            document.Add(new Paragraph("\n"));
                            document.Add(new Paragraph($"Email confirmed: {user.EmailConfirmed}", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10)));
                            document.Add(new Paragraph("\n"));
                            document.Add(new Paragraph($"Access failed count: {user.AccessFailedCount}", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10)));
                            document.Add(new Paragraph("\n"));
                            document.Add(new Paragraph($"Lockout enabled: {user.LockoutEnabled}", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10)));
                            document.Add(new Paragraph("\n"));
                            document.Add(new Paragraph($"Lockout end: {user.LockoutEnd}", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10)));
                            document.Add(new Paragraph("\n"));
                            document.Add(new Paragraph($"Is 2FA enabled: {user.TwoFactorEnabled}", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10)));
                            document.Add(new Paragraph("\n"));
                            document.Add(new Paragraph($"Last modified: {user.LastModified_20118018}", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10)));
                            document.Add(new Paragraph("\n"));

                        }

                        // Close the PdfWriter to finalize the document
                        document.Close();
                    }

                    var content = pdfStream.ToArray();
                    // Return the PDF file
                    return File(content, "application/pdf", "Admins.pdf");
                }
            }
            catch (DocumentException de)
            {
                // Handle DocumentException
                Console.Error.WriteLine(de.Message);
                // Return an appropriate error response
                return BadRequest();
            }
        }
    }
}
