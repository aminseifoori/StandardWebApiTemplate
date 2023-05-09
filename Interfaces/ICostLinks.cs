using Entities.LinkModels;
using Shared.Dtos.Costs;
using Microsoft.AspNetCore.Http;


namespace Interfaces
{
    public interface ICostLinks
    {
        LinkResponse TryGenerateLinks(IEnumerable<CostDto> costsDto, string fields, Guid costId, HttpContext httpContext);
    }
}
