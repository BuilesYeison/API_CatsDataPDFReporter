using Newtonsoft.Json;
using PdfReporter.Interfaces;

namespace PdfReporter.DataTransferObjects;

public class CatFactDTO{
    [JsonProperty(PropertyName = "fact")]
    public string Fact { get; set; }

    [JsonProperty(PropertyName = "Length")]
    public string Length { get; set; }
}