namespace backend.RequestHelpers;

public class CustomerFilterParams : PaginationParams
{
    public string? Name { get; set; }
    public string? Industry { get; set; }
    public string? Type { get; set; }
}
