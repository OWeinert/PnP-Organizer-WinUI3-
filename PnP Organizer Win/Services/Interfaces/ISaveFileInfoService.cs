using PnPOrganizer.Services.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PnPOrganizer.Services.Interfaces
{
    public interface ISaveFileInfoService
    {
        public List<SaveFileInfo?> SaveFileInfos { get; }

        public void AddSaveFileInfo(SaveFileInfo saveFileInfo);

        public Task<List<SaveFileInfo?>?> LoadSaveFileInfos();

        public Task SaveSaveFileInfos();
    }
}
