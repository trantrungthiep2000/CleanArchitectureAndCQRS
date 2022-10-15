﻿using AutoMapper;
using CwkSocial.API.Contracts.UserProfiles.Requests;
using CwkSocial.API.Contracts.UserProfiles.Responses;
using CwkSocial.API.Filters;
using CwkSocial.APPLICATION.UserProfiles.Commands;
using CwkSocial.APPLICATION.UserProfiles.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CwkSocial.API.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route(ApiRoutes.BaseRoute)]
    [ApiController]
    [CwkSocialExceptionHandler]
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
        public async Task<IActionResult> GetAllUserProfiles()
        {
            var query = new GetAllUserProfileQuery();
            var response = await _mediator.Send(query);

            if (response.IsError)
                return HandlerErrorResponse(response.Errors);

            var userProfiles = _mapper.Map<IEnumerable<UserProfileResponse>>(response.PayLoad);

            return Ok(userProfiles);
        }
        [ValidateModel]
        [HttpPost]
        public async Task<IActionResult> CreateUserProfile([FromBody] UserProfileCreateUpdate userProfileCreate)
        {
            var command = _mapper.Map<CreateUserProfileCommand>(userProfileCreate);
            var response = await _mediator.Send(command);

            if (response.IsError)
                return HandlerErrorResponse(response.Errors);

            var userProfile = _mapper.Map<UserProfileResponse>(response.PayLoad);

            return CreatedAtAction(nameof(GetUserProfileById), new { id = userProfile.UserProfileId }, userProfile);
        }

        [HttpGet]
        [Route($"{ApiRoutes.UserProfiles.IdRoute}")]
        public async Task<IActionResult> GetUserProfileById(Guid id)
        {
            var query = new GetUserProfileByIdQuery { UserProfileId = id };
            var response = await _mediator.Send(query);

            if (response.IsError)
                return HandlerErrorResponse(response.Errors);

            var userProfile = _mapper.Map<UserProfileResponse>(response.PayLoad);

            return Ok(userProfile);
        }

        [ValidateModel]
        [HttpPut]
        [Route($"{ApiRoutes.UserProfiles.IdRoute}")]
        public async Task<IActionResult> UpdateUserProfile(Guid id, [FromBody] UserProfileCreateUpdate userProfile)
        {
            var command = _mapper.Map<UpdateUserProfileBasicInfoCommand>(userProfile);
            command.UserProfileId = id;
            var response = await _mediator.Send(command);

            return response.IsError ? HandlerErrorResponse(response.Errors) : Ok(response);
        }

        [HttpDelete]
        [Route($"{ApiRoutes.UserProfiles.IdRoute}")]
        public async Task<IActionResult> DeleteUserProfile(Guid id)
        {
            var command = new DeleteUserProfileCommand { UserProfileId = id };
            var response = await _mediator.Send(command);

            return response.IsError ? HandlerErrorResponse(response.Errors) : Ok(response);
        }
    }
}