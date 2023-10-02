using System;
using MediatR;

namespace OnionArch.Application.Features.Commands.AppUser.LoginUser.RefreshTokenLogin
{
	public class RefreshTokenLoginCommandRequest: IRequest<RefreshTokenLoginCommandResponse>
    {
        public string RefreshToken { get; set; }
    }
}

