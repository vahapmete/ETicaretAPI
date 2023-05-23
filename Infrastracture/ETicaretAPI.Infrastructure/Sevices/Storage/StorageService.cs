using ETicaretAPI.Application.Services.Storage;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Infrastructure.Sevices.Storage
{
    public class StorageService : IStorageService
    {
         readonly IStorage _storage;

        public StorageService(IStorage storage)
        {
            _storage = storage;
        }

        public string StorageName { get => _storage.GetType().Name; }

        public Task DeleteAsync(string pathOrConatinerName, string fileName)
            => _storage.DeleteAsync(pathOrConatinerName, fileName);

        public List<string> GetFiles(string pathOrContainerName)
            => _storage.GetFiles(pathOrContainerName);

        public bool HasFile(string pathOrContainerName, string fileName)
            => _storage.HasFile(pathOrContainerName, fileName);

        public Task<List<(string fileName, string pathOrContainerName)>> UploadAsync(string pathOrConatinerName, IFormFileCollection files)
            =>_storage.UploadAsync(pathOrConatinerName, files);
    }
}
 