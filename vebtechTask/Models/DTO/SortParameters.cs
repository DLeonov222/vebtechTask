using Swashbuckle.AspNetCore.Annotations;
using vebtechTask.Models.Enums;

namespace vebtechTask.Models.DTO;

public class SortParameters
{
    public string OrderBy { get; set; } = String.Empty;

    [SwaggerParameter("0 - ascending, 1 - descending", Required = false)]
    public SortDirection OrderAsc { get; set; }
}