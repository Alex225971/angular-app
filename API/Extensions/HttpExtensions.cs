using System.Text.Json;
using API.Configurations;

namespace API;

public static class HttpExtensions
{
    public static void AddPaginationHeader<T>(this HttpResponse response, PagedList<T> data) 
    {
        var paginationHeader = new PaginationHeaders(data.CurrentPage, data.PageSize, data.TotalCount, data.TotalPages);
        var jsonOptions = new JsonSerializerOptions{PropertyNamingPolicy = JsonNamingPolicy.CamelCase};

        response.Headers.Append("Pagination", JsonSerializer.Serialize(paginationHeader, jsonOptions));
        response.Headers.AccessControlExposeHeaders = "Pagination";
    }
}
