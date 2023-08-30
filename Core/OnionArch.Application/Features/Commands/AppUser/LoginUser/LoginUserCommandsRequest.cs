using System;
using MediatR;

namespace OnionArch.Application.Features.Commands.AppUser.LoginUser
{
	public class LoginUserCommandsRequest: IRequest<LoginUserCommandsResponse>
    {
		public string UserNameOrEmail { get; set; }
        public string Password { get; set; }

    }
}

