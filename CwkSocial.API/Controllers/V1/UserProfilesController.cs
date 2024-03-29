﻿using AutoMapper;
using CwkSocial.API.Contracts.UserProfiles.Requests;
using CwkSocial.API.Contracts.UserProfiles.Responses;
using CwkSocial.API.Extensions;
using CwkSocial.API.Filters;
using CwkSocial.APPLICATION.UserProfiles.Commands;
using CwkSocial.APPLICATION.UserProfiles.Queries;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CwkSocial.API.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route(ApiRoutes.BaseRoute)]
    [ApiController]
    [CwkSocialExceptionHandler]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UserProfilesController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public UserProfilesController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUserProfiles(CancellationToken cancellationToken)
        {
            var query = new GetAllUserProfileQuery();

            var response = await _mediator.Send(query, cancellationToken);

            if (response.IsError)
                return HandlerErrorResponse(response.Errors);

            var userProfiles = _mapper.Map<IEnumerable<UserProfileResponse>>(response.PayLoad);

            return Ok(userProfiles);
        }

        [HttpGet]
        [Route($"{ApiRoutes.UserProfiles.IdRoute}")]
        [ValidateGuid("id")]
        public async Task<IActionResult> GetUserProfileById(Guid id, CancellationToken cancellationToken)
        {
            var query = new GetUserProfileByIdQuery { UserProfileId = id };

            var response = await _mediator.Send(query, cancellationToken);

            if (response.IsError)
                return HandlerErrorResponse(response.Errors);

            var userProfile = _mapper.Map<UserProfileResponse>(response.PayLoad);

            return Ok(userProfile);
        }

        [HttpPut]
        [Route($"{ApiRoutes.UserProfiles.IdRoute}")]
        [ValidateModel]
        public async Task<IActionResult> UpdateUserProfile([FromBody] UserProfileCreateUpdate userProfile,
            CancellationToken cancellationToken)
        {
            var userProfileId = HttpContext.GetUserProfileIdClaimValue();

            var command = _mapper.Map<UpdateUserProfileBasicInfoCommand>(userProfile);
            command.UserProfileId = userProfileId;

            var response = await _mediator.Send(command, cancellationToken);

            return response.IsError ? HandlerErrorResponse(response.Errors) : Ok(response);
        }
    }
}