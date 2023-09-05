using System;
namespace OnionArch.Application.DTOs
{
	public class UserInfo
	{
		//Eposta ve Şifre ile giriş yapan Kullanıcının bilgilerini burada geri yollamak için ihtiyacımız var.

		public string UserName { get; set; }
		public string Email { get; set; }
		public string NameSurname { get; set; }

	}
}

