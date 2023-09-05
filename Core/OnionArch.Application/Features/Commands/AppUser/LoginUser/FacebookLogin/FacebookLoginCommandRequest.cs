using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.Features.Commands.AppUser.LoginUser.FacebookLogin
{
    public class FacebookLoginCommandRequest : IRequest<FacebookLoginCommandResponse>
    {
        public string AuthToken { get; set; }
    }
}

