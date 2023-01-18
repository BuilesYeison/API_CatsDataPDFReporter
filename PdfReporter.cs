using PdfReporter.Repositories;
using PdfReporter.Interfaces;
using PdfReporter.DataAccesObjects;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// principio de inversion de dependencias, cualquier objeto que herede la interfáz servirá como fuente de datos, así si se cambia de api a sql como fuente de datos por ejemplo
IDataPersistence persistence = new ApiRepository(); 
var catDb = new CatDAO(persistence);

app.MapGet("/cat-breeds", async () => {    
    return await catDb.GetCatBreeds();
});

app.MapGet("/cat-breeds/{origin}", async (string origin) => {    
    return await catDb.GetCatBreeds(origin);
});

app.MapGet("/cat-facts", async () => {    
    return await catDb.GetCatFacts();
});

app.MapGet("/cat-facts/{limit}", async (int limit) => {    
    return await catDb.GetCatFacts(limit);
});

app.Run();
