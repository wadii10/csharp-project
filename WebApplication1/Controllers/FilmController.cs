using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting.Internal;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class FilmController : Controller
    {
        private readonly AppkicationContext context;
        private readonly IWebHostEnvironment hostingEnvironment;



        public FilmController(AppkicationContext context, IWebHostEnvironment hostingEnvironment)
        {
            this.context = context;
            this.hostingEnvironment = hostingEnvironment;
        }

        // GET: FilmController
        public async Task<IActionResult> Index(string search)
        {
            //var films = await context.Films.Include(f => f.Categorie).ToListAsync();
            var categories = context.Categories.ToList();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");

            // Retrieve films based on the search query or all films
            var filmsQuery = context.Films.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                filmsQuery = filmsQuery.Where(f => f.FilmName.Contains(search));
            }

            var filmss = filmsQuery.Include(f => f.Categorie).ToList();
            return View(filmss);
        }

        // GET: FilmController/Details/5
        public ActionResult Details(int id)
        {
            ViewBag.Categories = context.Categories.ToList();
            ViewBag.Producers = context.Producers.ToList();
            Film film = context.Films.Where(x => x.FilmId == id).FirstOrDefault();
            return View(film);
        }

        // GET: FilmController/Create
        public ActionResult Create()
        {
            ViewBag.Categories = context.Categories.ToList();
            ViewBag.Producers = context.Producers.ToList();
            return View();
        }

        // POST: FilmController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Film film, IFormFile Image)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (Image != null && Image.Length > 0)
                    {
                       
                        // Générer un nom unique pour le fichier
                        var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(Image.FileName);
                        var fileExtension = Path.GetExtension(Image.FileName);
                        var uniqueFileName = $"{Guid.NewGuid()}_{fileNameWithoutExtension}{fileExtension}";

                        // Chemin de stockage des images dans le dossier wwwroot/uploads
                        var uploadsPath = Path.Combine(hostingEnvironment.WebRootPath, "uploads");

                        // Chemin complet du fichier sur le serveur
                        var filePath = Path.Combine(uploadsPath, uniqueFileName);

                        // Enregistrer le fichier sur le serveur
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await Image.CopyToAsync(stream);
                        }

                        // Associer le nom unique de fichier à la propriété Image du film
                        film.Image = uniqueFileName;
                    }

                    // Sauvegarder le film en base de données
                    context.Films.Add(film);
                    await context.SaveChangesAsync();

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    // Logguer l'exception pour une investigation ultérieure
                    ModelState.AddModelError(string.Empty, "Une erreur s'est produite lors de l'enregistrement du film.");
                }
            }

            // Si le ModelState n'est pas valide ou une erreur s'est produite, recharger ViewBag.Categories et revenir à la vue Create
            ViewBag.Categories = context.Categories.ToList();
            ViewBag.Producers = context.Producers.ToList();
            return View(film);
        }

        // GET: FilmController/Edit/5
        public ActionResult Edit(int id)
        {
            ViewBag.Categories = context.Categories.ToList();
            ViewBag.Producers = context.Producers.ToList();
            Film film = context.Films.Find(id);
            return View(film);
        }

        // POST: FilmController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Film film, IFormFile Image)
        {
            
                try
                {
                    if (Image != null && Image.Length > 0)
                    {

                        // Générer un nom unique pour le fichier
                        var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(Image.FileName);
                        var fileExtension = Path.GetExtension(Image.FileName);
                        var uniqueFileName = $"{Guid.NewGuid()}_{fileNameWithoutExtension}{fileExtension}";

                        // Chemin de stockage des images dans le dossier wwwroot/uploads
                        var uploadsPath = Path.Combine(hostingEnvironment.WebRootPath, "uploads");

                        // Chemin complet du fichier sur le serveur
                        var filePath = Path.Combine(uploadsPath, uniqueFileName);

                        // Enregistrer le fichier sur le serveur
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await Image.CopyToAsync(stream);
                        }

                        // Associer le nom unique de fichier à la propriété Image du film
                        film.Image = uniqueFileName;
                    }

                    // Sauvegarder le film en base de données
                    context.Films.Update(film);
                    await context.SaveChangesAsync();

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    // Logguer l'exception pour une investigation ultérieure
                    ModelState.AddModelError(string.Empty, "Une erreur s'est produite lors de l'enregistrement du film.");
                }
            

            // Si le ModelState n'est pas valide ou une erreur s'est produite, recharger ViewBag.Categories et revenir à la vue Create
            ViewBag.Categories = context.Categories.ToList();
            ViewBag.Producers = context.Producers.ToList();
            return View(film);
        }

        // GET: FilmController/Delete/5
        public ActionResult Delete(int id)
        {
            Film film = context.Films.Find(id);
            return View(film);
        }

        // POST: FilmController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Film film)
        {
            try
            {
                context.Films.Remove(film);
                context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }


       
    }
}
