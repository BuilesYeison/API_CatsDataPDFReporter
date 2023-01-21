using PdfReporter.Repositories;
using PdfReporter.Interfaces;
using PdfReporter.DataAccesObjects;
using DinkToPdf;
using PdfReporter.Utilities;

var builder = WebApplication.CreateBuilder(args);
// principio de inversion de dependencias, cualquier objeto que herede la interfáz servirá como fuente de datos, así si se cambia de api a sql como fuente de datos por ejemplo
// Inyección de dependencias
builder.Services.AddSingleton<ICatDAO>(new CatDAO(new ApiRepository()));
builder.Services.AddSingleton<IReportGenerator>(new ReportGenerator(new SynchronizedConverter(new PdfTools()),builder.Environment.ContentRootPath));
var app = builder.Build();

app.MapGet("/cat-breeds", async (ICatDAO catDb, IReportGenerator reportGenerator) => {
    var catBreedData =  await catDb.GetCatBreeds();
    if(catBreedData == null || catBreedData.Count() == 0)
        throw new InvalidOperationException("No fue posible la consulta de información");
    string url = reportGenerator.GetCatBreedReport(catBreedData);
    return string.IsNullOrEmpty(url)? Results.BadRequest("Error al crear reporte pdf") : Results.Ok(url);
});

app.MapGet("/cat-breeds/{origin}", async (string origin,ICatDAO catDb, IReportGenerator reportGenerator) => {    
    var catBreedData =  await catDb.GetCatBreeds(origin);
    if(catBreedData == null || catBreedData.Count() == 0)
        throw new InvalidOperationException("No fue posible la consulta de información");
    string url = reportGenerator.GetCatBreedReport(catBreedData);
    return string.IsNullOrEmpty(url)? Results.BadRequest("Error al crear reporte pdf") : Results.Ok(url);
});

app.MapGet("/cat-facts", async (ICatDAO catDb, IReportGenerator reportGenerator) => {
    var catFactsData = await catDb.GetCatFacts();
    if(catFactsData == null || catFactsData.Count() == 0)
        throw new InvalidOperationException("No fue posible la consulta de información");
    string url = reportGenerator.GetCatFactsReport(catFactsData);
    return string.IsNullOrEmpty(url)? Results.BadRequest("Error al crear reporte pdf") : Results.Ok(url);
});

app.MapGet("/cat-facts/{limit}", async (int limit,ICatDAO catDb, IReportGenerator reportGenerator) => {    
    var catFactsData = await catDb.GetCatFacts(limit);
    if(catFactsData == null || catFactsData.Count() == 0)
        throw new InvalidOperationException("No fue posible la consulta de información");
    string url = reportGenerator.GetCatFactsReport(catFactsData);
    return string.IsNullOrEmpty(url)? Results.BadRequest("Error al crear reporte pdf") : Results.Ok(url);
});

app.Run();
