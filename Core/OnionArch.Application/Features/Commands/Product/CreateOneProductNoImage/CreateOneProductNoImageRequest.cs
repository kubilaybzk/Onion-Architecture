
using MediatR;
using OnionArch.Application.View_Models;

namespace OnionArch.Application.Features.Commands.Product.CreateOneProductNoImage
{
	public class CreateOneProductNoImageRequest: IRequest<CreateOneProductNoImageResponse>
    {
		public VM_Create_Product product { get; set; }
	}
}

