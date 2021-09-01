﻿using BlizzardWind.Desktop.Business.Entities;
using BlizzardWind.Desktop.Business.Interfaces;
using BlizzardWind.Desktop.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<List<MarkTextFileModel>> AddArticleFileAsync(int type, List<string> fileNames, Guid? articleID = null)
        {
            List<MarkTextFileModel> models = new();
            List<MarkResource> entities = new();
            if (fileNames == null || !fileNames.Any())
                return models;
            if(!Directory.Exists(TEXT_DIRECTORY))
                Directory.CreateDirectory(TEXT_DIRECTORY);
            foreach (string fileName in fileNames)
            {
                FileInfo file = new FileInfo(fileName);
                if (!file.Exists)
                    continue;
                Guid id = Guid.NewGuid();
                MarkTextFileModel model = new()
                {
                    ID = id,
                    FileName = file.Name,
                    Type = type,
                    Extension = file.Extension,
                    FilePath = Path.Combine(TEXT_DIRECTORY, $"{id.ToString()}{file.Extension}")
                };
                file.CopyTo(model.FilePath);

                var entity = new MarkResource()
                {
                    ID = id,
                    Name = model.FileName,
                    Extension = model.Extension,
                    FileName = model.FilePath,
                    Length = file.Length,
                    Type = type,
                    ArticleID = articleID,
                    CreatedAt = DateTime.Now
                };

                entities.Add(entity);
                models.Add(model);
            }
            var db = await _dbService.GetConnectionAsync();
            await db.InsertAllAsync(entities);
            return models;
        }

        public async Task<List<MarkTextFileModel>> GetArticleFilesAsync(Guid articleID, int type = -1)
        {
            var db = await _dbService.GetConnectionAsync();
            var query = db.Table<MarkResource>()
                .Where(x=>x.ArticleID == articleID)
                .OrderByDescending(s => s.CreatedAt);
            if (type != -1)
                query.Where(x => x.Type == type);
            var entities = await query.ToListAsync();
            List<MarkTextFileModel> models = new();
            foreach (var entity in entities)
            {
                MarkTextFileModel model = new()
                {
                    ID = entity.ID,
                    Type = entity.Type,
                    FileName = entity.Name,
                    Extension = entity.Extension,
                    FilePath = Path.Combine(TEXT_DIRECTORY, entity.FileName)
                };
                models.Add(model);
            }
            return models;
        }
    }
}
