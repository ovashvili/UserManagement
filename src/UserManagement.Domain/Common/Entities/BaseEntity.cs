using System.Text.Json.Serialization;

namespace UserManagement.Domain.Common.Entities;

public class BaseEntity
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
}