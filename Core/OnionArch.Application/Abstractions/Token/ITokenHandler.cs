using System;
using p=OnionArch.Application.DTOs;
namespace OnionArch.Application.Abstractions.Token
{
	public interface ITokenHandler
	{
        //Giriş işleminde JWT token oluşturma işleminin yapılacağı alan .
        //void CreateAccessToken();
        /*
		 Token'ı biz geri döndüreceğiz bundan dolayı bu CreateAccessToken void olmamalı.

		 Şimdi burada bu Token 'ı temsil edecek bir objeye  ihtiyacımız olmalı.
		 */

        p.Token CreateAccessToken(int minutes);
    }
}

