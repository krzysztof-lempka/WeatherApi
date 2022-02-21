namespace WeatherApi.DataProviders;

public interface IDataProvider<TReq, TResponse> where TReq : class
{
    IDataProvider<TReq, TResponse> Next { get; }
    Task<TResponse> HandleAsync(TReq request);
}
