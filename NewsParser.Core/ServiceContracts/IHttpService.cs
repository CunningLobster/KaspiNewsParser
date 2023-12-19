namespace NewsParser.Core.ServiceContracts
{
    public interface IHttpService
    {
        public Task<string> GetHttpResponse(string url);
    }
}