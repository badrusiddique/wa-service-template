namespace Wati.Template.Common.Configurations;

public class RequestConfiguration
{
    public string Uri { get; set; }
    public object Data { get; set; } = null;
    public HttpRequestMessage RequestMessage { get; set; }
    public IEnumerable<RequestHeader> Headers { get; set; }
    public IDictionary<string, string> QueryParameters { get; set; }
    public FormUrlEncodedContent FormContent { get; set; } = null;
}

public class RequestHeader
{
    public string Key { get; set; }
    public string Value { get; set; }
}