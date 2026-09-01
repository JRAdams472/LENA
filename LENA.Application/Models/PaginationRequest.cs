namespace LENA.Application.Models;

public class PaginationRequest
{
    private static readonly HashSet<int> AllowedPageSizes = new() { 10, 25, 50, 100 };

    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 25;

    public void Normalize()
    {
        if (PageNumber < 1) PageNumber = 1;
        if (!AllowedPageSizes.Contains(PageSize)) PageSize = 25;
    }
}
