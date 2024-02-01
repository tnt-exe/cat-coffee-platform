using DAO.Helper;
using Microsoft.AspNetCore.Mvc;

namespace CatCoffeePlatformAPI.Controllers.Base
{
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected IActionResult HandleErrorResponse(List<ErrorHelper> errors)
        {
            if (errors.Any(e => e.Code == ErrorCode.UnAuthorize))
            {
                var error = errors.FirstOrDefault(e => e.Code == ErrorCode.UnAuthorize);
                return Unauthorized(new ErrorResponse(401, "UnAuthorize", error.Message, DateTime.Now));
            }
            if (errors.Any(e => e.Code == ErrorCode.NotFound))
            {
                var error = errors.FirstOrDefault(e => e.Code == ErrorCode.NotFound);
                return NotFound(new ErrorResponse(404, "Not Found", error.Message, DateTime.Now));
            }
            if (errors.Any(e => e.Code == ErrorCode.ServerError))
            {
                var error = errors.FirstOrDefault(e => e.Code == ErrorCode.ServerError);
                return StatusCode(500, new ErrorResponse(500, "Server Error", error.Message, DateTime.Now));
            }
            return StatusCode(400, new ErrorResponse(400, "Bad Request", errors, DateTime.Now));
        }
    }
}
