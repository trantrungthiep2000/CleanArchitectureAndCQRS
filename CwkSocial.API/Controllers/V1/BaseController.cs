﻿using CwkSocial.API.Contracts.Common;
using CwkSocial.APPLICATION.Models;
using Microsoft.AspNetCore.Mvc;

namespace CwkSocial.API.Controllers.V1
{
    public class BaseController : ControllerBase
    {
        protected IActionResult HandlerErrorResponse(List<Error> errors)
        {
            var apiError = new ErrorResponse();

            if (errors.Any(error => error.Code == ErrorCode.NotFound))
            {
                var error = errors.FirstOrDefault(error => error.Code == ErrorCode.NotFound);

                apiError.StatusCode = 404;
                apiError.StatusPhrase = "Not Found";
                apiError.TimeStamp = DateTime.UtcNow;
                apiError.Errors.Add(error.Message);

                return NotFound(apiError);
            }

            apiError.StatusCode = 400;
            apiError.StatusPhrase = "Bad Request";
            apiError.TimeStamp = DateTime.UtcNow;
            foreach (var error in errors)
            {
                apiError.Errors.Add(error.Message);
            }

            return StatusCode(500, apiError);
        }
    }
}