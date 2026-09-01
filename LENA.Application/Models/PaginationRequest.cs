namespace LENA.Application.Models;

public class PaginationRequest
{
    private static readonly HashSet<int> AllowedPageSizes = new() { 10, 25, 50, 100 };

    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 25;

    public static (int PageNumber, int PageSize) Clamp(int pageNumber, int pageSize)
    {
        if (pageNumber < 1) pageNumber = 1;
        if (!AllowedPageSizes.Contains(pageSize)) pageSize = 25;
        return (pageNumber, pageSize);
    }

    public void Normalize()
    {
        (PageNumber, PageSize) = Clamp(PageNumber, PageSize);
    }
}
