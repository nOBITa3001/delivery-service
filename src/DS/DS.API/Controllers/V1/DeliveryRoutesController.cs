using DS.Contracts.OperationResponse;
using DS.Handlers.Requests;
using DS.Handlers.Responses;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace DS.API.Controllers.V1
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class DeliveryRoutesController : DSControllerBase
    {
        public DeliveryRoutesController(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
        }

        [HttpGet("{route}/Cost")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public virtual async Task<ActionResult<IOperationResponse<GetDeliveryCostHandlerResponse>>> GetCost(string route)
        {
            return await CallHandlerAsync<GetDeliveryCostHandlerRequest, GetDeliveryCostHandlerResponse>
            (
                new GetDeliveryCostHandlerRequest(route)
            );
        }
    }
}