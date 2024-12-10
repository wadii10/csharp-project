using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var connectionstring = builder.Configuration.GetConnectionString("conn");
builder.Services.AddDbContext<AppkicationContext>(
    options => options.UseSqlServer(connectionstring));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

//user
app.MapControllerRoute(
        name: "UserSignUp",
        pattern: "User/SignUp",
        defaults: new { controller = "User", action = "Signup" }
);

app.MapControllerRoute(
        name: "UserLogin",
        pattern: "User/Login",
        defaults: new { controller = "User", action = "Login" }
);

//Category
app.MapControllerRoute(
        name: "CategoryList",
        pattern: "Categorie/Index",
        defaults: new { controller = "Categorie", action = "Index" }
);

app.MapControllerRoute(
        name: "AddCategory",
        pattern: "Categorie/Create",
        defaults: new { controller = "Categorie", action = "Create" }
);

//Producer
app.MapControllerRoute(
        name: "ProduceryList",
        pattern: "Producer/Index",
        defaults: new { controller = "Producer", action = "Index" }
);

app.MapControllerRoute(
        name: "AddProducer",
        pattern: "Producer/Create",
        defaults: new { controller = "Producer", action = "Create" }
);

//film
app.MapControllerRoute(
    name: "filmList",
    pattern: "Film/Index",
    defaults: new { controller = "Film", action = "Index" }
);

app.MapControllerRoute(
    name: "AddFilm",
    pattern: "Film/Create",
    defaults: new { controller = "Film", action = "Create" }
);

app.MapControllerRoute(
    name: "FilmDetail",
    pattern: "Film/Details/{id}",
    defaults: new { controller = "Film", action = "Details" }
);

app.MapControllerRoute(
    name: "EditFilm",
    pattern: "Film/Edit/{id}",
    defaults: new { controller = "Film", action = "Edit" }
);

app.MapControllerRoute(
    name: "DeleteFilm",
    pattern: "Film/Delete/{id}",
    defaults: new { controller = "Film", action = "Delete" }
);



app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.Run();
