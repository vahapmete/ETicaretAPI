using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Services.Storage
{
    public interface IStorage
    {
        Task<List<(string fileName, string pathOrContainerName)>> UploadAsync(string pathOrConatinerName, IFormFileCollection files);
        Task DeleteAsync(string pathOrConatinerName,string fileName);
        List<string> GetFiles(string pathOrContainerName);

        bool HasFile(string pathOrContainerName,string fileName);
    }
}
