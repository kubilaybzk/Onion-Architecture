using Microsoft.EntityFrameworkCore;
using OnionArch.Application.Abstractions.AttributeServices;
using OnionArch.Application.Abstractions.ProductCrud;
using OnionArch.Application.Repositories.AttributeCrud.AttributeCrud;
using OnionArch.Application.Repositories.AttributeCrud.AttributeValueCrud;
using OnionArch.Application.Repositories.AttributeCrud.ProductAttributeCrud;
using OnionArch.Application.View_Models.Attribute;
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


        // Servis yapılma sebebi ilerleyen süreçte buranın ürün eklerken kullanılabilme ihtimali.


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
            var attribute = await _attributeReadRepository.GetWhere(a => a.Name == attributeName).Select(p=>new Domain.Entities.Attribute()
            {
                //AttributeValues=p.AttributeValues,
                //CategoryAttributes=p.CategoryAttributes,
                ID=p.ID,
                CreateTime=p.CreateTime,
                UpdateTime=p.UpdateTime,
                Name=p.Name,
            }).FirstOrDefaultAsync();
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
            var attributeValue = await _attributeValueReadRepository.GetWhere(av => av.AttributeId == attributeId && av.Value == value).Select(p=>new AttributeValue()
            {
                AttributeId=p.AttributeId,
                Value=p.Value,
                ID=p.ID,
            }).FirstOrDefaultAsync();
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

        public async Task<IEnumerable<VM_Product_Attributes>> GetProductAttributesAsync(Guid productId)
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

            var productAttributes = product.ProductAttributes.Select(pa => new VM_Product_Attributes
            {
                AttributeName = pa.AttributeValue.Attribute.Name,
                AttributeId= pa.AttributeValue.Attribute.ID.ToString(),
                AttributeValue = pa.AttributeValue.Value,
                AttributeValueId= pa.AttributeValueId.ToString(),
            }).ToList();

            return productAttributes;
        }

        public async Task<bool> AssignAttributesToProductAsync(Guid productId, Guid attributeValueIds)
        {
            var product = await _productReadRepository.GetWhere(p=>p.ID==productId).Include(p => p.ProductAttributes).ThenInclude(p=>p.AttributeValue).FirstOrDefaultAsync();
            if (product == null)
            {
                return false;
            }

            product.ProductAttributes ??= new List<ProductAttribute>();

            var existingProductAttributes = product.ProductAttributes.ToList();

           
                var attributeValue = await _attributeValueReadRepository.GetByIdAsync(attributeValueIds.ToString());
                if (attributeValue == null)
                {
                return false;
                }

                var existingProductAttribute = existingProductAttributes
                    .FirstOrDefault(pa => pa.AttributeValue.AttributeId == attributeValue.AttributeId);

                if (existingProductAttribute != null)
                {
                    // Eğer aynı Attribute adı altında bir AttributeValue zaten mevcutsa, sadece değerini güncelle
                    existingProductAttribute.AttributeValue = attributeValue;
                    _productAttributeWriteRepository.Update(existingProductAttribute);
                }
                else
                {
                    // Eğer aynı Attribute adı altında bir AttributeValue mevcut değilse, yeni bir özellik ekle
                    var productAttribute = new ProductAttribute
                    {
                        ProductId = productId,
                        AttributeValueId = attributeValueIds,
                        AttributeValue = attributeValue
                    };
                    await _productAttributeWriteRepository.AddAsync(productAttribute);
                }
            

            await _productWriteRepository.SaveAsync();
            return true;
        }

        public async Task<bool> RemoveAttributeFromProductAsync(Guid productId, Guid attributeValueId)
        {
            var product = await _productReadRepository.GetWhere(p => p.ID == productId)
                .Include(p => p.ProductAttributes)
                .FirstOrDefaultAsync();

            if (product == null)
            {
                return false;
            }

            var productAttribute = product.ProductAttributes.FirstOrDefault(pa => pa.AttributeValueId == attributeValueId);
            if (productAttribute != null)
            {
                product.ProductAttributes.Remove(productAttribute);
                _productAttributeWriteRepository.Remove(productAttribute);
                await _productWriteRepository.SaveAsync();
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> DeleteAttributeAsync(Guid attributeId)
        {
            var attribute = await _attributeReadRepository.GetByIdAsync(attributeId.ToString());
            if (attribute == null)
            {
                return false;
            }

            _attributeWriteRepository.Remove(attribute);
            await _attributeWriteRepository.SaveAsync();
            return true;
        }

        public async Task<bool> DeleteAttributeValueAsync(Guid attributeValueId)
        {
            var attributeValue = await _attributeValueReadRepository.GetByIdAsync(attributeValueId.ToString());
            if (attributeValue == null)
            {
                return false;
            }

            _attributeValueWriteRepository.Remove(attributeValue);
            await _attributeValueWriteRepository.SaveAsync();
            return true;
        }

        public async Task<bool> UpdateAttributeAsync(Guid attributeId, string AttributeName)
        {
            var attribute = await _attributeReadRepository.GetByIdAsync(attributeId.ToString());
            if (attribute == null)
            {
                return false;
            }
            else
            {
            attribute.Name = AttributeName;
            _attributeWriteRepository.Update(attribute);
            await _attributeValueWriteRepository.SaveAsync();
            return true;
            }
        }

        public async Task<bool> UpdateAttributeValueAsync(Guid attributeValueId, string newValue)
        {
            var attributeValue = await _attributeValueReadRepository.GetByIdAsync(attributeValueId.ToString());
            if (attributeValue == null)
            {
                return false;
            }

            attributeValue.Value = newValue;
            _attributeValueWriteRepository.Update(attributeValue);
            await _attributeValueWriteRepository.SaveAsync();
            return true;
        }

    }
}
