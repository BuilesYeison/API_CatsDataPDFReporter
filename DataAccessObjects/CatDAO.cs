using PdfReporter.DataTransferObjects;
using PdfReporter.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace PdfReporter.DataAccesObjects;

/// <summary>
/// Logica de negocio, principio de responsabilidad unica: esta clase se dedica exclusivamente a la conexión con base de datos para obtención de información y su manipulación
/// según la logica del negocio
/// </summary>
public class CatDAO{
    private IDataPersistence dataAccess;

    public CatDAO(IDataPersistence persistence){
        //recibe la fuente de datos desde donde se consultará información: api, sql o mongo
        this.dataAccess = persistence;
    }

    /// <summary>
    /// get cat breeds with or without a filter
    /// </summary>
    /// <param name="filterByOrigin">origin to filter</param>
    /// <returns>List<CatBreed></returns>
    public async Task<List<CatBreedDTO>> GetCatBreeds(string filterByBreedOrigin = null){
        List<CatBreedDTO> catBreeds = await dataAccess.GetData<CatBreedDTO>("https://catfact.ninja/breeds");
        
        if(!String.IsNullOrEmpty(filterByBreedOrigin))
            catBreeds.Where(x=>x.Origin.ToLower() == filterByBreedOrigin.ToLower());
        return catBreeds;
    }
}