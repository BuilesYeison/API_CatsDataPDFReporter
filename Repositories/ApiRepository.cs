using PdfReporter.DataTransferObjects;
using PdfReporter.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PdfReporter.Repositories;

/// <summary>
/// Fuente de datos via Http rest api
/// </summary>
public class ApiRepository : IDataPersistence{
    static readonly HttpClient client = new HttpClient();

    /// <summary>
    /// get any information from api form api source
    /// </summary>
    /// <param name="connection">link where information is available</param>
    /// <returns>List<T>T is stablished</returns>
    public async Task<List<T>> GetData<T>(string connection){
        var result = new List<T>();
        try
        {
            string responseBody = await client.GetStringAsync(connection);
            dynamic json = JValue.Parse(responseBody);
            string data = JsonConvert.SerializeObject(json.data);
            result = JsonConvert.DeserializeObject<List<T>>(data);// convert json getted to type stablished T
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine("\nException Caught!");
            Console.WriteLine("Message :{0} ", e.Message);
        }
        catch(Exception ex){
            return null;
        }
        return result;
    }
}