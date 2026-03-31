using MockApi.Domain;

namespace MockApi.Application.Dto;

public class FieldConfig
{
    public FieldTypeEnum? Type { get; set; }

    public int? MinLength { get; set; }
    public int? MaxLength { get; set; }
    public int? MinValue { get; set; }
    public int? MaxValue { get; set; }
    
    public int? ArraySize { get; set; }
    public FieldConfig? Items { get; set; }
    
    public Dictionary<string, FieldConfig>? Properties { get; set; }
}

public class MockDataRequest
{
    public Dictionary<string, FieldConfig> Schema { get; set; }
}