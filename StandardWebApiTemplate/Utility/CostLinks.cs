using Contracts;
using Entities.LinkModels;
using Entities.Models;
using Interfaces;
using Microsoft.Net.Http.Headers;
using Shared.Dtos.Costs;

namespace StandardWebApiTemplate.Utility
{
    public class CostLinks : ICostLinks
    {
        private readonly LinkGenerator _linkGenerator;
        private readonly IDataShaper<CostDto> _dataShaper;
        public Dictionary<string, MediaTypeHeaderValue> AcceptHeader { get; set; } =
            new Dictionary<string, MediaTypeHeaderValue>();

        public CostLinks(LinkGenerator linkGenerator, IDataShaper<CostDto> dataShaper)
        { 
            _linkGenerator = linkGenerator;
            _dataShaper = dataShaper; 
        }

        public LinkResponse TryGenerateLinks(IEnumerable<CostDto> costsDto, string fields, Guid movieId,
            HttpContext httpContext)
        {
            var shapedcost = ShapeData(costsDto, fields);

            if (ShouldGenerateLinks(httpContext))
                return ReturnLinkdedCosts(costsDto, fields, movieId, httpContext, shapedcost);

            return ReturnShapedCosts(shapedcost);
        }

        private List<Entity> ShapeData(IEnumerable<CostDto> costDto, string fields) =>
            _dataShaper.ShapeData(costDto, fields)
                .Select(e => e.Entity)
                .ToList();

        private bool ShouldGenerateLinks(HttpContext httpContext)
        {
            var mediaType = (MediaTypeHeaderValue)httpContext.Items["AcceptHeaderMediaType"];

            return mediaType.SubTypeWithoutSuffix.EndsWith("hateoas", StringComparison.InvariantCultureIgnoreCase);
        }

        private LinkResponse ReturnShapedCosts(List<Entity> shapedcosts) =>
            new LinkResponse { ShapedEntities = shapedcosts };

        private LinkResponse ReturnLinkdedCosts(IEnumerable<CostDto> costsDto,
            string fields, Guid movieId, HttpContext httpContext, List<Entity> shapedCosts)
        {
            var costDtoList = costsDto.ToList();

            for (var index = 0; index < costDtoList.Count(); index++)
            {
                var costLinks = CreateLinksForCost(httpContext, movieId, costDtoList[index].Id, fields);
                shapedCosts[index].Add("Links", costLinks);
            }

            var costCollection = new LinkCollectionWrapper<Entity>(shapedCosts);
            var linkedcosts = CreateLinksForCosts(httpContext, costCollection);

            return new LinkResponse { HasLinks = true, LinkedEntities = linkedcosts };
        }

        private List<Link> CreateLinksForCost(HttpContext httpContext, Guid movieId, Guid id, string fields = "")
        {
            var links = new List<Link>
            {
                new Link(_linkGenerator.GetUriByAction(httpContext, "GetCostsForMovie", values: new { movieId, id, fields }),
                "self",
                "GET"),
                new Link(_linkGenerator.GetUriByAction(httpContext, "DeleteCost", values: new { movieId, id }),
                "delete_cost",
                "DELETE"),
                new Link(_linkGenerator.GetUriByAction(httpContext, "UpdateCost", values: new { movieId, id }),
                "update_cost",
                "PUT")
            };
            return links;
        }

        private LinkCollectionWrapper<Entity> CreateLinksForCosts(HttpContext httpContext,
            LinkCollectionWrapper<Entity> costsWrapper)
        {
            costsWrapper.Links.Add(new Link(_linkGenerator.GetUriByAction(httpContext, "GetCostsForMovie", values: new { }),
                    "self",
                    "GET"));

            return costsWrapper;
        }
    }
}
