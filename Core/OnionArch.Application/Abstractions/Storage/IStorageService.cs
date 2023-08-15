using System;
namespace OnionArch.Application.Abstractions.Storage
{
	public interface IStorageService:IStorage
	{
		public string StorageType { get; }
	}
}

