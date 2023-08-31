using System;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using OnionArch.Application.Abstractions.Token;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;

namespace OnionArch.infrastructure.Services.Token
{
    public class TokenHandler : ITokenHandler
    {

        readonly IConfiguration _configuration;

        public TokenHandler(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Application.DTOs.Token CreateAccessToken(int minutes)
        {
           //ÖNCELİKLE TOKEN GERİ DÖNDÜRECEĞİMİZ İÇİN BİR TOKEN NESNESİNE İHTİYACIMIZ VAR
            Application.DTOs.Token token = new();
            //DAHA SONRA BİR ADET TOKEN OLUŞTURMAMIZ GEREKİYOR.

            //HER ŞEYDEN ÖNCE BİR ADET SCREET KEY OLUŞTURMAMIZ GEREKİYOR
            SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(_configuration["Token:SecurityKey"]));


            //Artık bu oluşturduğumuz securityKey için Şifrelenmiş olan kimliği oluşturuyoruz.
            SigningCredentials signingCredentials = new(securityKey, SecurityAlgorithms.HmacSha256);
                    //Burası bizden securityKey ve haslenecek yöntem olarak bir algoritma istiyor.


            //Oluşturulacak Token ayarlarını vereceğiz.

                //Burada artık Token'ın ne kadar süre çalışır durumda olacağını ayarlıyoruz.
                token.Expiration = DateTime.UtcNow.AddMinutes(minutes);

                JwtSecurityToken securityToken = new(
                audience: _configuration["Token:Audience"],
                issuer: _configuration["Token:Issuer"],
                expires: token.Expiration,
                notBefore: DateTime.UtcNow,
                signingCredentials: signingCredentials
                );

            //Token oluşturucu sınıfından bir örnek alalım.
                JwtSecurityTokenHandler tokenHandler = new();

            //Return edecek olan Token'ın içinde bulunan  AccessToken'a oluşturduğumuz token'ı assign edelim.
            token.AccessToken = tokenHandler.WriteToken(securityToken);
                return token;





        }
    }
}

