using ETicaretAPI.Infrastructure.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Infrastructure.Sevices.Storage
{
    public class Storage
    {
        protected async Task<string> FileRenameAsync(string fileName)
        {
            string newFileName = await Task.Run<string>(async () =>
            {
                string fileId = Guid.NewGuid().ToString();
                string extension = Path.GetExtension(fileName);
                string oldName = Path.GetFileNameWithoutExtension(fileName);
                string updatedName = $"{fileId}_{NameEditor.CharacterRequlatory(oldName)}{extension}";
                return updatedName;
            });
            return newFileName;
        }
    }
}
