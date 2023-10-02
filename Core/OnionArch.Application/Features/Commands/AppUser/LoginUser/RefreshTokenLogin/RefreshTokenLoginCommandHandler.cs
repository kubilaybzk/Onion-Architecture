using System;
using MediatR;
using OnionArch.Application.Abstractions.UserServices;
using OnionArch.Application.DTOs.UserDTOs;

namespace OnionArch.Application.Features.Commands.AppUser.LoginUser.RefreshTokenLogin
{
	public class RefreshTokenLoginCommandHandler : IRequestHandler<RefreshTokenLoginCommandRequest, RefreshTokenLoginCommandResponse>
    {
        readonly IAuthService _authService;

        public RefreshTokenLoginCommandHandler(IAuthService authService)
		{
            _authService = authService;
        }

        public async Task<RefreshTokenLoginCommandResponse> Handle(RefreshTokenLoginCommandRequest request, CancellationToken cancellationToken)
        {
            var GelenDeğer = await _authService.RefreshTokenLoginAsync(request.RefreshToken);

            if (GelenDeğer is LoginUserSuccessResponseDTO successResponse)
            {
                return new RefreshTokenLoginSuccessCommandsResponse()
                {
                    token = successResponse.token,
                    UserInfo = new()
                    {
                        UserName = successResponse.UserInfo.UserName,
                        Email = successResponse.UserInfo.Email,
                        NameSurname = successResponse.UserInfo.NameSurname

                    }
                };
            }

            if (GelenDeğer is LoginUserErrorResponseDTO errorResponse)
            {
                var response = new RefreshTokenLoginErrorCommandsResponse
                {
                    Message = errorResponse.Message
                };
                return response;
            }

            throw new Exception("Beklenmeyen bir yanıt türü alındı.");
        }
    }
}

