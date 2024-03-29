﻿using ETicaretAPI.Application.Services.Storage.Local;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using ETicaretAPI.Application.Services;
using ETicaretAPI.Infrastructure.Tools;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Infrastructure.Sevices.Storage.Local
{
    public class LocalStorage :Storage, ILocalStorage
    {
        readonly IWebHostEnvironment _webHostEnvironment;
        public LocalStorage(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task DeleteAsync(string path, string fileName)
            =>File.Delete(Path.Combine(path,fileName));
        

        public List<string> GetFiles(string path)
        {
            DirectoryInfo directory = new(path);
            return directory.GetFiles().Select(f => f.Name).ToList();
        }

        public bool HasFile(string path, string fileName)
        {
            return File.Exists(Path.Combine(path, fileName));
        }
        //async Task<string> FileRenameAsync(string fileName)
        //{
        //    string newFileName = await Task.Run<string>(async () =>
        //    {
        //        string fileId = Guid.NewGuid().ToString();
        //        string extension = Path.GetExtension(fileName);
        //        string oldName = Path.GetFileNameWithoutExtension(fileName);
        //        string updatedName = $"{fileId}_{NameEditor.CharacterRequlatory(oldName)}{extension}";
        //        return updatedName;
        //    });
        //    return newFileName;
        //}
         async Task<bool> CopyFileAsync(string path, IFormFile file)
        {
            try
            {
                using FileStream fileStream = new(path, FileMode.Create, FileAccess.Write, FileShare.None, 1024 * 1024, useAsync: false);
                await file.CopyToAsync(fileStream);
                await fileStream.FlushAsync();
                return true;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public async Task<List<(string fileName, string pathOrContainerName)>> UploadAsync(string path, IFormFileCollection files)
        {
            string uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, path);
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }
            List<(string fileName, string path)> filesList = new();
            List<bool> results = new();

            foreach (IFormFile file in files)
            {
                string newFileName = await FileRenameAsync(file.FileName);
                string fullPath = Path.Combine(uploadPath, newFileName);
                bool result = await CopyFileAsync(fullPath, file);
                results.Add(result);
                filesList.Add((newFileName, path));
            }
            if (results.TrueForAll(r => r.Equals(true)))
                return filesList;

            return null;
        }

    }
}
