using PdfReporter.DataTransferObjects;

namespace PdfReporter.Interfaces;

/// <summary>
/// Allows to convert html content to pdf reports
/// </summary>
public interface IReportGenerator
{
    /// <summary>
    /// Allows to receive cat breeds information and converting it in a pdf report from html document
    /// </summary>
    /// <param name="catBreedData">cat breeds information</param>
    /// <returns>Url that allows to download the pdf report</returns>
    IResponse GetCatBreedReport(List<CatBreedDTO> catBreedData);

    /// <summary>
    /// Allows to receive cat facts information and converting it in a pdf report from html document
    /// </summary>
    /// <param name="catFactsData">cat facts information</param>
    /// <returns>Url that allows to download the pdf report</returns>
    IResponse GetCatFactsReport(List<CatFactDTO> catFactsData);
}