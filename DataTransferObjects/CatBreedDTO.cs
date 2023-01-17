using Newtonsoft.Json;
using PdfReporter.Interfaces;

namespace PdfReporter.DataTransferObjects;

public class CatBreedDTO{
    [JsonProperty(PropertyName = "breed")]
    public string Breed { get; set; }

    [JsonProperty(PropertyName = "country")]
    public string Country { get; set; }

    [JsonProperty(PropertyName = "origin")]
    public string Origin { get; set; }

    [JsonProperty(PropertyName = "coat")]
    public string Coat { get; set; }

    [JsonProperty(PropertyName = "pattern")]
    public string Pattern { get; set; }
}