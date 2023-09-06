using System;
namespace OnionArch.Application.Abstractions.UserServices
{
	//Burada login işlemleri ile ilgili temel işlemleri yapacağız.

	//Şimdi burada 2 farklı yöntem kullanacağız ,
	//Sosyal Medya girişi ve Normal giriş için 2 farklı interface tanımlayalım.

	public interface IAuthService: IExternalAuthentication,IInternalAuthentication
    {

	}
}

