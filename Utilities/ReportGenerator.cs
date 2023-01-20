using System.Net;
using DinkToPdf;
using DinkToPdf.Contracts;
using PdfReporter.DataTransferObjects;
using PdfReporter.Interfaces;

namespace PdfReporter.Utilities;

/// <summary>
/// Allows to convert html content to pdf reports
/// </summary>
public class ReportGenerator : IReportGenerator
{
    /// <summary>
    /// Dependency that allows to convert html content to pdf files
    /// </summary>
    private IConverter converter;
    /// <summary>
    /// Server path where is running api
    /// </summary>
    private readonly string rootPath;

    public ReportGenerator(IConverter Converter, string RootPath)
    {
        this.converter = Converter;
        this.rootPath = RootPath;
    }

    /// <summary>
    /// Allows to receive cat breeds information and converting it in a pdf report from html document
    /// </summary>
    /// <param name="catBreedData">cat breeds information</param>
    /// <returns>Url that allows to download the pdf report</returns>
    public string GetCatBreedReport(List<CatBreedDTO> catBreedData)
    {
        string reportPath = Path.Combine(rootPath, $"Utilities/Reports/CatBreedReport{DateTime.Now.ToString("yyyyMMddhhmmss")}.pdf");
        try
        {
            var htmlContent = string.Empty;
            using (WebClient wbc = new WebClient())
            {
                htmlContent = wbc.DownloadString(Path.Combine(rootPath, "Utilities/HtmlBases/cat-breed-report.html"));
            }

            string variableToPutRows = "*pRows";//in html document
            foreach (var catBreed in catBreedData)
            {

                string tableHtmlRow = $"<tr>" +
                $"<td>{catBreed.Breed}</td>" +
                $"<td>{catBreed.Country}</td>" +
                $"<td>{catBreed.Origin}</td>" +
                $"<td>{catBreed.Coat}</td>" +
                $"<td>{catBreed.Pattern}</td>" +
                $"</tr>\n" +
                $"{variableToPutRows}";
                htmlContent = htmlContent.Replace(variableToPutRows, tableHtmlRow);
            }
            htmlContent = htmlContent.Replace(variableToPutRows, string.Empty);
            GeneratePdf(reportPath, htmlContent,Path.Combine(rootPath, "Utilities/HtmlBases/header.html"));
        }
        catch (Exception ex)
        {
            return String.Empty;
        }

        return reportPath;
    }

    /// <summary>
    /// Create a pdf document from a html content
    /// </summary>
    /// <param name="reportPath">Where pdf will be saved</param>
    /// <param name="htmlContent">Content to be converted to pdf</param>
    /// <param name="headerHtml">Url where html header is</param>
    private void GeneratePdf(string reportPath, string htmlContent, string headerHtml = null)
    {
        var doc = new HtmlToPdfDocument()
        {
            GlobalSettings ={
                ColorMode=ColorMode.Color,
                Orientation =Orientation.Portrait,
                PaperSize=PaperKind.A4,
                Out=reportPath
            },
            Objects ={
                new ObjectSettings(){
                    PagesCount =true,
                    HtmlContent=htmlContent,
                    HeaderSettings={HtmUrl=string.IsNullOrEmpty(headerHtml)?string.Empty:headerHtml}
                }
            }
        };
        converter.Convert(doc);
    }
}