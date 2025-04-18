using CzechUp.EF;
using CzechUp.EF.Models;
using CzechUp.Services.DTOs;
using CzechUp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CzechUp.Services.Services
{
    public class TagService : BaseService<UserTag>, ITagService
    {
        private readonly DatabaseContext _databaseContext;
        private readonly IWordService wordService;

        public TagService(DatabaseContext databaseContext, IWordService wordService) : base(databaseContext)
        {
            _databaseContext = databaseContext;
            this.wordService = wordService;
        }
        public async Task<List<UserTag>> GetTags(Guid userGuid, CancellationToken cancellationToken)
        {
            return await GetQuery().Include(t=>t.TagTypes).Where(w => w.UserGuid == userGuid).ToListAsync(cancellationToken);
        }

        public async Task<UserTag> GetTag(Guid topicGuid, CancellationToken cancellationToken)
        {
            return GetByGuid(topicGuid);
        }

        public async Task<Guid> CreateTag(UserTag tag, CancellationToken cancellationToken)
        {
            var tagTypeEnums = _databaseContext.TagTypes.ToList();
            var selectedTypes = tag.TagTypes.Select(t => t.TagTypeEnum);
            tag.TagTypes = new List<TagType>(tagTypeEnums.Where(t => selectedTypes.Contains(t.TagTypeEnum)));
            return Create(tag);
        }

        public async Task<UserTag> UpdateTag(UserTag tag, CancellationToken cancellationToken)
        {
            var dbTag = GetByGuid(tag.Guid);
            if (dbTag == null)
            {
                throw new Exception("Can not update tag");
            }

            dbTag.Name = tag.Name;

            // Загружаем связанные TagTypes
            _databaseContext.Entry(dbTag).Collection(t => t.TagTypes).Load();
            dbTag.TagTypes.Clear();

            var tagTypeEnums = _databaseContext.TagTypes.ToList();
            var selectedTypes = tag.TagTypes.Select(t => t.TagTypeEnum);
            dbTag.TagTypes = new List<TagType>(tagTypeEnums.Where(t => selectedTypes.Contains(t.TagTypeEnum)));

            await _databaseContext.SaveChangesAsync(cancellationToken);

            return dbTag;
        }

        public async Task RemoveTag(Guid userGuid, Guid tagGuid, CancellationToken cancellationToken)
        {
            var tag = GetByGuid(tagGuid);
            if (tag == null || tag.UserGuid != userGuid)
            {
                throw new Exception("Can not delete tag");
            }
            else
            {
                _databaseContext.Remove(tag);
                _databaseContext.SaveChanges();
            }
        }
    }
}
