﻿using DS.Contracts.OperationResponse;
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
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public virtual async Task<ActionResult<IOperationResponse<GetDeliveryCostHandlerResponse>>> GetCost(string route)
        {
            return await CallHandlerAsync<GetDeliveryCostHandlerRequest, GetDeliveryCostHandlerResponse>
            (
                new GetDeliveryCostHandlerRequest(route)
            );
        }

        [HttpGet("{route}/Possible")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public virtual async Task<ActionResult<IOperationResponse<GetPossibleDeliveryRoutesHandlerResponse>>> GetPossible(string route, [FromQuery]int maxRouteRepeat = 1, [FromQuery]int? maxDeliveryCost = null, [FromQuery]int? maxStop = null)
        {
            return await CallHandlerAsync<GetPossibleDeliveryRoutesHandlerRequest, GetPossibleDeliveryRoutesHandlerResponse>
            (
                new GetPossibleDeliveryRoutesHandlerRequest(route, maxRouteRepeat, maxDeliveryCost, maxStop)
            );
        }

        [HttpGet("{route}/TheCheapestCost")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public virtual async Task<ActionResult<IOperationResponse<GetTheCheapestDeliveryCostHandlerResponse>>> GetTheCheapestCost(string route)
        {
            return await CallHandlerAsync<GetTheCheapestDeliveryCostHandlerRequest, GetTheCheapestDeliveryCostHandlerResponse>
            (
                new GetTheCheapestDeliveryCostHandlerRequest(route)
            );
        }
    }
}