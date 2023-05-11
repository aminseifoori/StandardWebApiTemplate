using Entities.LinkModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace StandardWebApiTemplate.Presentation.Controllers
{
    [Route("api")]
    [ApiController]
    public class RootController : ControllerBase
    {
        private readonly LinkGenerator linkGenerator;

        public RootController(LinkGenerator _linkGenerator)
        {
            linkGenerator = _linkGenerator;
        }
        [HttpGet(Name = "GetRoot")]
        public IActionResult GetRoot([FromHeader(Name = "Accept")] string mediaType)
        {
            if (mediaType.Contains("application/vnd.codemaze.apiroot"))
            {
                var list = new List<Link>
                {
                    new Link
                    {
                        Href = linkGenerator.GetUriByName(HttpContext, nameof(GetRoot), new{ }),
                        Rel = "self",
                        Method = "GET"
                    },
                    new Link
                    {
                        Href = linkGenerator.GetUriByName(HttpContext, "GetCompanies", new {}),
                        Rel = "companies",
                        Method = "GET"
                    },
                    new Link
                    {
                        Href = linkGenerator.GetUriByName(HttpContext, "CreateCompany", new {}),
                        Rel = "create_company",
                        Method = "POST"
                    }
                };
                return Ok(list);
            }
            return NoContent();
        }
    }
}
