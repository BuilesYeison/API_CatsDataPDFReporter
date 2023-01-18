using PdfReporter.DataTransferObjects;

namespace PdfReporter.Interfaces;

public interface ICatDAO
{
    Task<List<CatBreedDTO>> GetCatBreeds(string filterByBreedOrigin = null);
    Task<List<CatFactDTO>> GetCatFacts(int? limit = null);
}
