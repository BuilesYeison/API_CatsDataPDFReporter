using PdfReporter.Repositories;
using PdfReporter.Interfaces;
using PdfReporter.DataAccesObjects;

var builder = WebApplication.CreateBuilder(args);
// principio de inversion de dependencias, cualquier objeto que herede la interfáz servirá como fuente de datos, así si se cambia de api a sql como fuente de datos por ejemplo
// Inyección de dependencias
builder.Services.AddSingleton<ICatDAO>(new CatDAO(new ApiRepository()));
var app = builder.Build();

app.MapGet("/cat-breeds", async (ICatDAO catDb) => {    
    return await catDb.GetCatBreeds();
});

app.MapGet("/cat-breeds/{origin}", async (string origin,ICatDAO catDb) => {    
    return await catDb.GetCatBreeds(origin);
});

app.MapGet("/cat-facts", async (ICatDAO catDb) => {    
    return await catDb.GetCatFacts();
});

app.MapGet("/cat-facts/{limit}", async (int limit,ICatDAO catDb) => {    
    return await catDb.GetCatFacts(limit);
});

app.Run();
