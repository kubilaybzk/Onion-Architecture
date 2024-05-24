using OnionArch.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnionArch.Application.Abstractions.AttributeServices
{
    public interface IAttributeService
    {
        Task<Domain.Entities.Attribute> AddOrGetAttributeAsync(string attributeName);
        Task<AttributeValue> AddOrGetAttributeValueAsync(Guid attributeId, string value);
        Task<IEnumerable<Domain.Entities.Attribute>> GetAllAttributesAsync();
        Task<IEnumerable<AttributeValue>> GetAttributeValuesAsync(Guid attributeId);
        Task<IEnumerable<ProductAttributeDto>> GetProductAttributesAsync(Guid productId);
        Task AssignAttributesToProductAsync(Guid productId, IEnumerable<Guid> attributeValueIds);

        Task RemoveAttributeFromProductAsync(Guid productId, Guid attributeValueId);
        Task UpdateAttributeValueAsync(Guid attributeValueId, string newValue);
    }

    public class ProductAttributeDto
    {
        public string AttributeName { get; set; }
        public string AttributeValue { get; set; }
        public string AttributeId {  get; set; }
        public string AttributeValueId { get; set; }
    }
}
