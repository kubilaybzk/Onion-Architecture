using System;
using OnionArch.Application.DTOs.UserDTOs;
using OnionArch.Domain.Entities.Identity;

namespace OnionArch.Application.Abstractions.UserServices
{
    //Burada user işlemleri ile ilgili temel işlemleri yapacağız. CRud işlemleri.
    public interface IUserService
	{
        /*
         Şimdi biz kullanıcı oluştururken burada ilk olarak bize kullanıcı hakkında bilgiler gelecek değil mi ?
            Bir bu gelen bilgilere göre işlemler yapacağız yani gelen bilgilere göre kayıt işlemi gerçekleştireceğiz.
               Şimdi burada şöyle bir yöntem kullancağız ,
                        Bize bilgiler nereden geliyor  "CreateUserCommandRequest" içinden değil mi ?
                            bu bilgileri bizim bir şekilde CreateUser içine aktarmamız gerekiyor .
                                o zaman şöyle bir yöntem kullanacağız ,
                                Biz normal şartlarda bunları request nesneleri olarak alabiliriz ama bunu
                                    katmanlar arasında paylaşacağız katmanlar arasında bu bilgileri paylaşacağımız için
                                        yapmamız gereken işlem Bir DTO objesi oluşturmak ve bu Dto objesinin içinde bu bilgileri döndürmek.

                        Şimdi ilk olarak,
                            Application içinde bulunan DTO içine girelim.
                                Buraya User adında bir klasör oluşturalım
                                    Bu klasör içinde CreateUserDto olarak isimlendirelim ve Request içinde olan property değerlerini burada tanımlayalım.
                                        DTO altında UserDTO's adında bir klasör oluşturalım .
                                            Bu klasör altında CreateUserDTO adında bir class oluşturalım ve "CreateUserCommandRequest " içinde bulunan props değerlerini içersin
                        Aynı işlemi "CreateUserCommandResponse " için gerçekleştirelim.
                            Şimdi tüm bu işlemleri hallettikten sonra ,
                                Bunları parametre olarak ekleyelim.
                    
                    
         */
        //CreateUserResponseDTO döndürecek olan ve  CreateUserRequestDTO objesi sayesinde bilgilere erişebilecek olan
        //Fonksiyonumuz
        Task<CreateUserResponseDTO> CreateUser(CreateUserRequestDTO requestDTO);


        //Refresh Token'ın kullanıcı giriş yaptığı anda ,

        Task UpdateRefreshToken(string refreshToken, AppUser user, DateTime accessTokenDate, int addOnAccessTokenDate);



    }
}

