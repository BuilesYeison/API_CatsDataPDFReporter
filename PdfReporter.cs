using PdfReporter.Repositories;
using PdfReporter.Interfaces;
using PdfReporter.DataAccesObjects;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// principio de inversion de dependencias, cualquier objeto que herede la interfáz servirá como fuente de datos, así si se cambia de api a sql como fuente de datos por ejemplo
IDataPersistence persistence = new ApiRepository(); 

app.MapGet("/", async () => {
    var catDb = new CatDAO(persistence);
    return await catDb.GetCatBreeds();
});

app.Run();
