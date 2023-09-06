using System;
namespace OnionArch.Application.DTOs.UserDTOs
{
	


    public class LoginUserResponseDTO
    {


    }
    //Burada farklı bir kullanım yöntemi deneyimliyoruz .
    //İşlem başarılı ise sadece Token döndür .
    public class LoginUserSuccessResponseDTO : LoginUserResponseDTO
    {

        public Token token { get; set; }
        public UserInfo UserInfo { get; set; }
    }
    //Error var ise sadece  hata mesajı
    public class LoginUserErrorResponseDTO : LoginUserResponseDTO
    {

        public string Message { get; set; }
    }
}

