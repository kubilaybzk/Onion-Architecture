using System;
using MediatR;

namespace OnionArch.Application.Features.Commands.AppUser.CreateUser
{
	public class CreateUserCommandRequest:IRequest<CreateUserCommandResponse>
	{
        //Mevcut olan yapının içinde bulunan parametrelerimizi burada belirtiyoruz.
        public string NameSurname { get; set; }
		public string UserName { get; set; }
		public string Email { get; set; }
		public string Password { get; set; }
    }
}

