using System;
using OnionArch.Application.DTOs;

namespace OnionArch.Application.Features.Commands.AppUser.LoginUser.RefreshTokenLogin
{
	


    public class RefreshTokenLoginCommandResponse
    {


    }
    //Burada farklı bir kullanım yöntemi deneyimliyoruz .
    //İşlem başarılı ise sadece Token döndür .
    public class RefreshTokenLoginSuccessCommandsResponse : RefreshTokenLoginCommandResponse
    {

        public Token token { get; set; }
        public UserInfo UserInfo { get; set; }
    }
    //Error var ise sadece  hata mesajı
    public class RefreshTokenLoginErrorCommandsResponse : RefreshTokenLoginCommandResponse
    {

        public string Message { get; set; }
    }
}

