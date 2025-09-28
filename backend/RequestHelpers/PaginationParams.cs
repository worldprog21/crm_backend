
namespace backend.RequestHelpers;

public class PaginationParams
{
    private const int MaxPageSize = 50; // hard limit to prevent abuse
    private int _pageSize = 2; // default page size
    public int PageNumber { get; set; } = 1;

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
    }
}
