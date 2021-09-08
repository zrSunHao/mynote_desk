using BlizzardWind.App.Common.MarkText;
using BlizzardWind.App.Common.Tools;
using BlizzardWind.Desktop.Business.Entities;
using BlizzardWind.Desktop.Business.Interfaces;
using BlizzardWind.Desktop.Business.Models;

namespace BlizzardWind.Desktop.Business.Services
{
    public class FileResourceService : IFileResourceService
    {
        private readonly IDatabaseService _dbService;
        private readonly string TEXT_DIRECTORY = "Resources";

        public FileResourceService(IDatabaseService service)
        {
            _dbService = service;
        }

        public async Task<List<MarkTextFileModel>> AddArticleFileAsync(int type, List<string> fileNames, Guid? articleId = null)
        {
            List<MarkTextFileModel> models = new();
            List<FileResource> entities = new();
            if (fileNames == null || !fileNames.Any())
                return models;
            string baseAddress = await GetBaseAddress();

            foreach (string fileName in fileNames)
            {
                FileInfo file = new FileInfo(fileName);
                if (!file.Exists)
                    continue;
                Guid id = Guid.NewGuid();
                MarkTextFileModel model = new()
                {
                    Id = id,
                    FileName = file.Name.Replace(file.Extension, ""),
                    Type = type,
                    SecretKey = Guid.NewGuid(),
                    Extension = file.Extension,
                    FilePath = Path.Combine(baseAddress, $"{id.ToString()}{file.Extension}")
                };

                string key = FileEncryptTool.GuidToKey(model.SecretKey);
                if (type == MarkResourceType.Cover)//封面图片需裁剪
                    FileEncryptTool.EncryptCoverFile(fileName, model.FilePath, key);
                else
                    FileEncryptTool.EncryptFile(fileName, model.FilePath, key);

                var entity = new FileResource()
                {
                    Id = id,
                    Name = model.FileName,
                    Extension = model.Extension,
                    FileName = $"{id.ToString()}{file.Extension}",
                    Length = file.Length,
                    Type = type,
                    SecretKey = model.SecretKey,
                    ArticleId = articleId,
                    CreatedAt = DateTime.Now
                };

                entities.Add(entity);
                models.Add(model);
            }
            var db = await _dbService.GetConnectionAsync();
            await db.InsertAllAsync(entities);
            return models;
        }

        public async Task<List<MarkTextFileModel>> GetArticleFilesAsync(Guid articleId, int type = -1)
        {
            string baseAddress = await GetBaseAddress();
            var db = await _dbService.GetConnectionAsync();
            var query = db.Table<FileResource>()
                .Where(x => x.ArticleId == articleId && !x.Deleted)
                .OrderByDescending(s => s.CreatedAt);
            if (type != -1)
                query.Where(x => x.Type == type);
            var entities = await query.ToListAsync();
            List<MarkTextFileModel> models = new();
            foreach (var entity in entities)
            {
                MarkTextFileModel model = new()
                {
                    Id = entity.Id,
                    Type = entity.Type,
                    FileName = entity.Name,
                    Extension = entity.Extension,
                    SecretKey = entity.SecretKey,
                    FilePath = Path.Combine(baseAddress, entity.FileName)
                };
                models.Add(model);
            }
            return models;
        }

        public async Task<MarkTextFileModel?> GetByIdAsync(Guid id)
        {
            string baseAddress = await GetBaseAddress();
            var db = await _dbService.GetConnectionAsync();
            var entity = await db.Table<FileResource>().FirstOrDefaultAsync(x => x.Id == id && !x.Deleted);
            if (entity == null)
                return null;
            return new MarkTextFileModel
            {
                Id = entity.Id,
                Type = entity.Type,
                FileName = entity.Name,
                Extension = entity.Extension,
                SecretKey = entity.SecretKey,
                FilePath = Path.Combine(baseAddress, entity.FileName)
            };
        }

        public async Task<bool> RelaceAsync(MarkTextFileModel model, string fileName)
        {
            string baseAddress = await GetBaseAddress();
            var db = await _dbService.GetConnectionAsync();
            var entity = await db.Table<FileResource>()
                .FirstOrDefaultAsync(x => x.Id == model.Id);
            FileInfo file = new FileInfo(fileName);
            if (entity == null)
                return false;
            File.Delete(Path.Combine(baseAddress, entity.FileName));

            entity.UpdatedAt = DateTime.Now;
            entity.Extension = file.Extension;
            entity.FileName = $"{entity.Id.ToString()}{file.Extension}";
            model.FilePath = Path.Combine(baseAddress, entity.FileName);
            model.Extension = file.Extension;
            if (!file.Exists)
                return false;
            string key = FileEncryptTool.GuidToKey(model.SecretKey);
            FileEncryptTool.EncryptFile(fileName, model.FilePath, key);

            await db.UpdateAsync(entity);
            return true;
        }

        public async Task<bool> RenameAsync(Guid id, string name)
        {
            var db = await _dbService.GetConnectionAsync();
            var entity = await db.Table<FileResource>()
                .FirstOrDefaultAsync(x => x.Id == id);
            if (entity != null)
            {
                entity.Name = name;
                entity.UpdatedAt = DateTime.Now;
                await db.UpdateAsync(entity);
            }
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var db = await _dbService.GetConnectionAsync();
            var entity = await db.Table<FileResource>()
                .FirstOrDefaultAsync(x => x.Id == id);
            if (entity != null)
            {
                entity.DeletedAt = DateTime.Now;
                entity.Deleted = true;
                await db.UpdateAsync(entity);
            }
            return true;
        }


        private async Task<string> GetBaseAddress()
        {
            string baseAddress = await _dbService.GetBaseAddress();
            if (!Directory.Exists(baseAddress))
                throw new Exception("基础存储目录不存在");
            string address = Path.Combine(baseAddress, TEXT_DIRECTORY);
            if (!Directory.Exists(address))
                Directory.CreateDirectory(address);
            return address;
        }
    }
}
