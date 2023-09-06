using System;
namespace OnionArch.Application.DTOs.UserDTOs
{
	public class CreateUserResponseDTO
	{
        //Burası bizim için CreateUser çalıştıktan sonra döndüreceği sonucu temsil edecek olan nesne
        public string Message { get; set; }
        public bool Succeeded { get; set; }
    }
}

