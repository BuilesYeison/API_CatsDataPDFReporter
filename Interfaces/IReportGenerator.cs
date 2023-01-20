using PdfReporter.DataTransferObjects;

namespace PdfReporter.Interfaces;

public interface IReportGenerator
{
    /// <summary>
    /// Allows to receive cat breeds information and converting it in a pdf report from html document
    /// </summary>
    /// <param name="catBreedData">cat breeds information</param>
    /// <returns>Url that allows to download the pdf report</returns>
    string GetCatBreedReport(List<CatBreedDTO> catBreedData);
}