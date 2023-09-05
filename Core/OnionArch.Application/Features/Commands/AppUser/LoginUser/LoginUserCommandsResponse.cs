using System;
using System.Numerics;
using OnionArch.Application.DTOs;

namespace OnionArch.Application.Features.Commands.AppUser.LoginUser
{
	public class LoginUserCommandsResponse
	{
        

    }
    //Burada farklı bir kullanım yöntemi deneyimliyoruz .
    //İşlem başarılı ise sadece Token döndür .
    public class LoginUserSuccessCommandsResponse: LoginUserCommandsResponse
    {

        public Token token { get; set; }
        public UserInfo UserInfo { get; set; }
    }
    //Error var ise sadece  hata mesajı
    public class LoginUserErrorCommandsResponse: LoginUserCommandsResponse
    {

        public string Message { get; set; }
    }
}

