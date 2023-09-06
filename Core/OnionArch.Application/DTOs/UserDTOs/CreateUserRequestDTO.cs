using System;
namespace OnionArch.Application.DTOs.UserDTOs
{
    public class CreateUserRequestDTO
    {
        //  "CreateUserCommandRequest " içinde bulunan props değerlerini içeriyor
        //  Kullanıcı oluşturuken gelen verileri aktarmak için
        public string NameSurname { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}

