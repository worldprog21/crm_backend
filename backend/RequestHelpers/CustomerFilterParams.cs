namespace backend.RequestHelpers;

public class CustomerFilterParams : PaginationParams
{
    public string? Name { get; set; }
    public string? Industry { get; set; }
    public string? Type { get; set; }

    public string? Sort { get; set; }   // e.g. "name_asc", "name_desc", "industry_asc"
}
