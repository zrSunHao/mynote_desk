﻿using BlizzardWind.Desktop.Business.Entities;
using BlizzardWind.Desktop.Business.Interfaces;
using BlizzardWind.Desktop.Business.Models;

namespace BlizzardWind.Desktop.Business.Services
{
    public class FileResourceService : IFileResourceService
    {
        private readonly IDatabaseService _dbService;
        private readonly string TEXT_DIRECTORY = @"E:\Blizzard\MarkTxt\Resources";

        public FileResourceService(IDatabaseService service)
        {
            _dbService = service;
        }

        public async Task<List<MarkTextFileModel>> AddArticleFileAsync(int type, List<string> fileNames, Guid? articleId = null)
        {
            List<MarkTextFileModel> models = new();
            List<MarkResource> entities = new();
            if (fileNames == null || !fileNames.Any())
                return models;
            if (!Directory.Exists(TEXT_DIRECTORY))
                Directory.CreateDirectory(TEXT_DIRECTORY);
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
                    FilePath = Path.Combine(TEXT_DIRECTORY, $"{id.ToString()}{file.Extension}")
                };
                file.CopyTo(model.FilePath);

                var entity = new MarkResource()
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
            var db = await _dbService.GetConnectionAsync();
            var query = db.Table<MarkResource>()
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
                    FilePath = Path.Combine(TEXT_DIRECTORY, entity.FileName)
                };
                models.Add(model);
            }
            return models;
        }

        public async Task<string> GetPathByIdAsync(Guid id)
        {
            var db = await _dbService.GetConnectionAsync();
            var entity = await db.Table<MarkResource>().FirstOrDefaultAsync(x => x.Id == id && !x.Deleted);
            if (entity == null)
                return string.Empty;
            return Path.Combine(TEXT_DIRECTORY, entity.FileName);
        }

        public async Task<bool> RelaceAsync(MarkTextFileModel model, string fileName)
        {
            var db = await _dbService.GetConnectionAsync();
            var entity = await db.Table<MarkResource>()
                .FirstOrDefaultAsync(x => x.Id == model.Id);
            FileInfo file = new FileInfo(fileName);
            if (entity == null)
                return false;
            File.Delete(Path.Combine(TEXT_DIRECTORY, entity.FileName));

            entity.UpdatedAt = DateTime.Now;
            entity.Extension = file.Extension;
            entity.FileName = $"{entity.Id.ToString()}{file.Extension}";
            model.FilePath = Path.Combine(TEXT_DIRECTORY, entity.FileName);
            model.Extension = file.Extension;
            if (!file.Exists)
                return false;
            file.CopyTo(model.FilePath);

            await db.UpdateAsync(entity);
            return true;
        }

        public async Task<bool> RenameAsync(Guid id, string name)
        {
            var db = await _dbService.GetConnectionAsync();
            var entity = await db.Table<MarkResource>()
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
            var entity = await db.Table<MarkResource>()
                .FirstOrDefaultAsync(x => x.Id == id);
            if (entity != null)
            {
                entity.DeletedAt = DateTime.Now;
                entity.Deleted = true;
                await db.UpdateAsync(entity);
            }
            return true;
        }
    }
}
