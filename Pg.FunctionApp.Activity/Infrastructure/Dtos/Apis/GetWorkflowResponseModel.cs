using System.Text.Json.Serialization;

namespace Pg.FunctionApp.Activity.Infrastructure.Dtos.Apis;

public class GetWorkflowResponseModel
{
    [JsonPropertyName("_id")] public string? Id { get; set; }
    public string? Operation { get; set; }
    public Payload? Payload { get; set; }
    public DateTime Date { get; set; }
}

public class Payload
{
    public string? Status { get; set; }
    [JsonPropertyName("_collection")] public string? Collection { get; set; }

    public string? Action { get; set; }
    public DateTime Date { get; set; }
    public string? EntryType { get; set; }
}