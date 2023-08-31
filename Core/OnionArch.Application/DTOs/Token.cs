using System;
namespace OnionArch.Application.DTOs
{
	public class Token
	{ 
		public string AccessToken { get; set; }  //Token'ın kendisini temsile edecek olan değer. 
		public DateTime Expiration { get; set; }  //Token'ın ömrünü temsil edecek olan değer. 
	}
}

