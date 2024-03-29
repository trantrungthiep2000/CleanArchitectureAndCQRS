﻿using CwkSocial.APPLICATION.Models;
using CwkSocial.DOMAIN.Aggregates.UserProfileAggregate;
using MediatR;

namespace CwkSocial.APPLICATION.UserProfiles.Commands
{
    public class UpdateUserProfileBasicInfoCommand : IRequest<OperationResult<bool>>
    {
        public Guid UserProfileId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string CurrentCity { get; set; }
    }
}