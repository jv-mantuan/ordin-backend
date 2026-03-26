namespace Ordin.Api.Contracts
{
    public class Meta
    {
        public DateTimeOffset Timestamp { get; set; }
        public string RequestId { get; init; } = null!;
    }

    public class ApiResponse<T> where T : class
    {
        public T? Data { get; set; }

        public Meta Metadata { get; init; }

        public ApiResponse(T? data, Meta metaData)
        {
            Data = data;
            Metadata = metaData;
        }

        public ApiResponse(T? data, string requestId, DateTimeOffset timestamp)
        {
            Data = data;
            Metadata = new Meta { RequestId = requestId, Timestamp = timestamp };
        }
    }
}