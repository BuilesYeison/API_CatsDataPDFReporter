using PdfReporter.DataTransferObjects;

namespace PdfReporter.Interfaces;

/// <summary>
/// Principio de inversión de dependencias, los objetos que hereden esta interfáz serán una fuente valida de información, independiente del motor de base de datos
/// </summary>
public interface IDataPersistence{
    /// <summary>
    /// Get data from repository
    /// </summary>
    /// <param name="connection">the url where the data is available</param>    
    /// <returns></returns>
    Task<List<T>> GetData<T>(string connection);
}