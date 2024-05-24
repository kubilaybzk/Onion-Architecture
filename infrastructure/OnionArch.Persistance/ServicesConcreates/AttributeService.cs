using Microsoft.EntityFrameworkCore;
using OnionArch.Application.Abstractions.AttributeServices;
using OnionArch.Application.Abstractions.ProductCrud;
using OnionArch.Application.Repositories.AttributeCrud.AttributeCrud;
using OnionArch.Application.Repositories.AttributeCrud.AttributeValueCrud;
using OnionArch.Application.Repositories.AttributeCrud.ProductAttributeCrud;
using OnionArch.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnionArch.Persistance.ServicesConcreates
{
    public class AttributeService : IAttributeService
    {
        private readonly IAttributeWriteRepository _attributeWriteRepository;
        private readonly IAttributeReadRepository _attributeReadRepository;
        private readonly IAttributeValueWriteRepository _attributeValueWriteRepository;
        private readonly IAttributeValueReadRepository _attributeValueReadRepository;
        private readonly IProductReadRepository _productReadRepository;
        private readonly IProductWriteRepository _productWriteRepository;
        private readonly IProductAttributeWriteRepository _productAttributeWriteRepository;

        public AttributeService(
            IAttributeWriteRepository attributeWriteRepository,
            IAttributeReadRepository attributeReadRepository,
            IAttributeValueWriteRepository attributeValueWriteRepository,
            IAttributeValueReadRepository attributeValueReadRepository,
            IProductReadRepository productReadRepository,
            IProductWriteRepository productWriteRepository,
            IProductAttributeWriteRepository productAttributeWriteRepository)
        {
            _attributeWriteRepository = attributeWriteRepository;
            _attributeReadRepository = attributeReadRepository;
            _attributeValueWriteRepository = attributeValueWriteRepository;
            _attributeValueReadRepository = attributeValueReadRepository;
            _productReadRepository = productReadRepository;
            _productWriteRepository = productWriteRepository;
            _productAttributeWriteRepository = productAttributeWriteRepository;
        }

        public async Task<Domain.Entities.Attribute> AddOrGetAttributeAsync(string attributeName)
        {
            var attribute = await _attributeReadRepository.GetWhere(a => a.Name == attributeName).FirstOrDefaultAsync();
            if (attribute == null)
            {
                attribute = new Domain.Entities.Attribute { Name = attributeName };
                await _attributeWriteRepository.AddAsync(attribute);
                await _attributeWriteRepository.SaveAsync();
            }
            return attribute;
        }

        public async Task<AttributeValue> AddOrGetAttributeValueAsync(Guid attributeId, string value)
        {
            var attributeValue = await _attributeValueReadRepository.GetWhere(av => av.AttributeId == attributeId && av.Value == value).FirstOrDefaultAsync();
            if (attributeValue == null)
            {
                attributeValue = new AttributeValue { AttributeId = attributeId, Value = value };
                await _attributeValueWriteRepository.AddAsync(attributeValue);
                await _attributeValueWriteRepository.SaveAsync();
            }
            return attributeValue;
        }

        public async Task<IEnumerable<Domain.Entities.Attribute>> GetAllAttributesAsync()
        {
            return await _attributeReadRepository.GetAll().ToListAsync();
        }

        public async Task<IEnumerable<AttributeValue>> GetAttributeValuesAsync(Guid attributeId)
        {
            return await _attributeValueReadRepository.GetWhere(av => av.AttributeId == attributeId).ToListAsync();
        }

        public async Task<IEnumerable<ProductAttributeDto>> GetProductAttributesAsync(Guid productId)
        {
            var product = await _productReadRepository.GetWhere(p => p.ID == productId)
                .Include(p => p.ProductAttributes)
                .ThenInclude(pa => pa.AttributeValue)
                    .ThenInclude(av => av.Attribute)
                .FirstOrDefaultAsync();

            if (product == null)
            {
                throw new Exception("Product not found");
            }

            var productAttributes = product.ProductAttributes.Select(pa => new ProductAttributeDto
            {
                AttributeName = pa.AttributeValue.Attribute.Name,
                AttributeId= pa.AttributeValue.Attribute.ID.ToString(),
                AttributeValue = pa.AttributeValue.Value,
                AttributeValueId= pa.AttributeValueId.ToString(),
            }).ToList();

            return productAttributes;
        }

        public async Task AssignAttributesToProductAsync(Guid productId, IEnumerable<Guid> attributeValueIds)
        {
            var product = await _productReadRepository.GetByIdAsync(productId.ToString());
            if (product == null)
            {
                throw new Exception("Product not found");
            }

            product.ProductAttributes ??= new List<ProductAttribute>();

            var existingProductAttributes = product.ProductAttributes.ToList();

            foreach (var attributeValueId in attributeValueIds)
            {
                if (!existingProductAttributes.Any(pa => pa.AttributeValueId == attributeValueId))
                {
                    var attributeValue = await _attributeValueReadRepository.GetByIdAsync(attributeValueId.ToString());
                    if (attributeValue != null)
                    {
                        var productAttribute = new ProductAttribute
                        {
                            ProductId = productId,
                            AttributeValueId = attributeValueId,
                            AttributeValue = attributeValue
                        };
                        await _productAttributeWriteRepository.AddAsync(productAttribute);
                    }
                }
            }

            await _productWriteRepository.SaveAsync();
        }

        public async Task RemoveAttributeFromProductAsync(Guid productId, Guid attributeValueId)
        {
            var product = await _productReadRepository.GetWhere(p => p.ID == productId)
                .Include(p => p.ProductAttributes)
                .FirstOrDefaultAsync();

            if (product == null)
            {
                throw new Exception("Product not found");
            }

            var productAttribute = product.ProductAttributes.FirstOrDefault(pa => pa.AttributeValueId == attributeValueId);
            if (productAttribute != null)
            {
                product.ProductAttributes.Remove(productAttribute);
                _productAttributeWriteRepository.Remove(productAttribute);
                await _productWriteRepository.SaveAsync();
            }
        }

        public async Task UpdateAttributeValueAsync(Guid attributeValueId, string newValue)
        {
            var attributeValue = await _attributeValueReadRepository.GetByIdAsync(attributeValueId.ToString());
            if (attributeValue == null)
            {
                throw new Exception("Attribute value not found");
            }

            attributeValue.Value = newValue;
            _attributeValueWriteRepository.Update(attributeValue);
            await _attributeValueWriteRepository.SaveAsync();
        }
    }
}
