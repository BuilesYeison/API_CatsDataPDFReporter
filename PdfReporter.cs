using PdfReporter.Repositories;
using PdfReporter.Interfaces;
using PdfReporter.DataAccesObjects;
using DinkToPdf;
using PdfReporter.Utilities;
using Swashbuckle.AspNetCore.Annotations;

var builder = WebApplication.CreateBuilder(args);
// principle of dependency inversion, any object that inherits the interface will serve as a data source, so if you change from api to sql as a data source for example
// Dependecies injection
builder.Services.AddSingleton<ICatDAO>(new CatDAO(new ApiRepository()));
builder.Services.AddScoped<IReportGenerator>(provider=>new ReportGenerator(new SynchronizedConverter(new PdfTools()),builder.Environment.ContentRootPath));
builder.Services.AddScoped<IReportRemover>(provider=>new ReportRemover(builder.Environment.ContentRootPath));
builder.Services.AddEndpointsApiExplorer(); //de esta forma swashbuckle explorará todos los endpoints para mapearlos
builder.Services.AddSwaggerGen(c=>{
    c.SwaggerDoc("v1",
    new(){Title="Cat Pdf Reporter API",Version="v1"});
    c.EnableAnnotations();
});
var app = builder.Build();

app.MapGet("/",(IReportRemover reportRemover)=>{
    try{
        reportRemover.DeletePdfReports();   
        return Results.Redirect("/swagger");
    }catch(Exception ex){
        return Results.BadRequest($"Hubo un error al eliminar reportes antiguos: {ex.Message}");
    }
});

/// <summary>
/// Get pdf report link to download about cat breeds
/// </summary>
/// <param name="catDb">Where information is getted</param>
/// <param name="reportGenerator">Service that return urls to download pdf report</param>
/// <returns>url to download pdf report</returns>
app.MapGet("/cat-breeds", async (ICatDAO catDb, IReportGenerator reportGenerator) => {
    try{
        var catBreedData =  await catDb.GetCatBreeds();
        if(catBreedData == null || catBreedData.Count() == 0)
            throw new InvalidOperationException("No fue posible la consulta de información");
        IResponse response = reportGenerator.GetCatBreedReport(catBreedData);
        return Results.Ok(response); 
    }catch(Exception ex){
        return Results.BadRequest($"Error al crear reporte pdf: {ex.Message}");
    }
}).WithMetadata(new SwaggerOperationAttribute(summary: "Get pdf report link to download about cat breeds"));

/// <summary>
/// Get pdf report link to download about cat breeds giving a cat breed origin as a filter
/// </summary>
/// <param name="origin">Where the cats are from</param>
/// <param name="catDb">Where information is getted</param>
/// <param name="reportGenerator">Service that return urls to download pdf report</param>
/// <returns>url to download pdf report</returns>
app.MapGet("/cat-breeds/{origin}", async ([SwaggerParameter("Where the cats are from")]string origin,ICatDAO catDb, IReportGenerator reportGenerator) => {    
    try{
        var catBreedData =  await catDb.GetCatBreeds(origin);
        if(catBreedData == null || catBreedData.Count() == 0)
            throw new InvalidOperationException("No fue posible la consulta de información");
        var response = reportGenerator.GetCatBreedReport(catBreedData);
        return Results.Ok(response);
    }catch(Exception ex){
        return Results.BadRequest($"Error al crear reporte pdf: {ex.Message}");
    }
    
}).WithMetadata(new SwaggerOperationAttribute(summary: "Get pdf report link to download about cat breeds", 
description: "Get pdf report link to download about cat breeds giving a cat breed origin as a filter"));

/// <summary>
/// Get pdf report link to download about randomly cat facts
/// </summary>
/// <param name="catDb">Where information is getted</param>
/// <param name="reportGenerator">Service that return urls to download pdf report</param>
/// <returns>url to download pdf report</returns>
app.MapGet("/cat-facts", async (ICatDAO catDb, IReportGenerator reportGenerator) => {
    try{
        var catFactsData = await catDb.GetCatFacts();
        if(catFactsData == null || catFactsData.Count() == 0)
            throw new InvalidOperationException("No fue posible la consulta de información");
        var response = reportGenerator.GetCatFactsReport(catFactsData);
        return Results.Ok(response);
    }catch(Exception ex){
        return Results.BadRequest($"Error al crear reporte pdf: {ex.Message}");
    }
}).WithMetadata(new SwaggerOperationAttribute(summary: "Get pdf report link to download about randomly cat facts"));

/// <summary>
/// Get pdf report link to download about randomly cat facts, giving a maximum amount of facts to get
/// </summary>
/// <param name="limit">The maximum amount of facts to get</param>
/// <param name="catDb">Where information is getted</param>
/// <param name="reportGenerator">Service that return urls to download pdf report</param>
/// <returns>url to download pdf report</returns>
app.MapGet("/cat-facts/{limit}", async ([SwaggerParameter("The maximum amount of facts to get")]int limit,ICatDAO catDb, IReportGenerator reportGenerator) => {        
    try{
        var catFactsData = await catDb.GetCatFacts(limit);
        if(catFactsData == null || catFactsData.Count() == 0)
            throw new InvalidOperationException("No fue posible la consulta de información");
        var response = reportGenerator.GetCatFactsReport(catFactsData);
        return Results.Ok(response);
    }catch(Exception ex){
        return Results.BadRequest($"Error al crear reporte pdf: {ex.Message}");
    }
}).WithMetadata(new SwaggerOperationAttribute(summary: "Get pdf report link to download about randomly cat facts", 
description: "Get pdf report link to download about randomly cat facts, giving a maximum amount of facts to get"));;

app.UseSwagger();
app.UseSwaggerUI(c=>c.SwaggerEndpoint(
    "/swagger/v1/swagger.json",
    "v1"
));

app.Run();
