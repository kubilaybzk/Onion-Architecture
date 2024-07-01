using OnionArch.Application.GlobalResponse;
using OnionArch.Application.View_Models.Attribute;
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
        Task<IEnumerable<VM_Product_Attributes>> GetProductAttributesAsync(Guid productId);
        Task <Boolean> AssignAttributesToProductAsync(Guid productId, Guid attributeValueIds);
        Task<Boolean> RemoveAttributeFromProductAsync(Guid productId, Guid attributeValueId);
        Task<Boolean> DeleteAttributeAsync (Guid attributeId);
        Task<Boolean> DeleteAttributeValueAsync(Guid attributeId);
        Task<Boolean> UpdateAttributeAsync(Guid attributeId,string AttributeName);
        Task<Boolean> UpdateAttributeValueAsync(Guid attributeValueId, string newValue);
    }


}
