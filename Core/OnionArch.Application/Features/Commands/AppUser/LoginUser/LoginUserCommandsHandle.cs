using MediatR;
using OnionArch.Application.Abstractions.UserServices;
using OnionArch.Application.DTOs;
using OnionArch.Application.DTOs.UserDTOs;
using OnionArch.Application.Features.Commands.AppUser.LoginUser;

public class LoginUserCommandsHandle : IRequestHandler<LoginUserCommandsRequest, LoginUserCommandsResponse>
{
    readonly IAuthService _authService;

    public LoginUserCommandsHandle(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<LoginUserCommandsResponse> Handle(LoginUserCommandsRequest request, CancellationToken cancellationToken)
    {
        var GelenDeğer = await _authService.LoginAsync(request.UserNameOrEmail, request.Password, 15);

        if (GelenDeğer is LoginUserSuccessResponseDTO successResponse)
        {
            return new LoginUserSuccessCommandsResponse()
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
            var response = new LoginUserErrorCommandsResponse
            {
                Message=errorResponse.Message
            };
            return response;
        }

        throw new Exception("Beklenmeyen bir yanıt türü alındı.");
    }
}
