namespace Demomann.common.Models;

public enum PaginationOrderByEnum{
    Asc=1,
    Desc = 2
}
public class PaginationParams {
    public int Skip { get; set; } = 0;
    public int Take { get; set; } = 10;
    public string? SortBy { get; set; }
    public PaginationOrderByEnum OrderBy { get; set; } = PaginationOrderByEnum.Asc;
}
public class Pagination<T> :  PaginationParams {
    public List<T> Data { get; set; } = [];
    public int TotalCount {get; set;} = 0;
}
