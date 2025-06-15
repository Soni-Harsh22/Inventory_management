using InventoryManagementSystem.Common.Constants;
using InventoryManagementSystem.Common.Enums;
using InventoryManagementSystem.Models.Responces;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagementSystem.Controllers
{
    public class BaseController : ControllerBase
    {
        /// <summary>
        /// Generates a standardized API response based on the service response status code.
        /// </summary>
        /// <param name="apiResponse">The response object containing status code and data.</param>
        /// <returns>An IActionResult representing the HTTP response.</returns>
        protected IActionResult GenerateResponse<T>(ApiResponse<T> apiResponse)
        {
            return apiResponse.ResponseCode switch
            {
                ResponseCode.Ok => Ok(apiResponse),
                ResponseCode.Created => StatusCode(201, apiResponse),
                ResponseCode.NoContent => NoContent(),
                ResponseCode.BadRequest => BadRequest(apiResponse),
                ResponseCode.Unauthorized => Unauthorized(apiResponse),
                ResponseCode.NotFound => NotFound(apiResponse),
                ResponseCode.Conflict => Conflict(apiResponse),
                ResponseCode.InternalServerError => StatusCode(500, apiResponse),
                _ => StatusCode(500, new ApiResponse<string>(ResponseCode.InternalServerError, ErrorCodes.ERROR_OCCURRED, null))
            };
        }
    }
}
