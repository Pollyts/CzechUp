using CzechUp.EF.Models;
using CzechUp.Services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CzechUp.Services.Interfaces
{
    public interface IWordService
    {
        Task<List<UserOriginalWord>> GetWords(Guid userGuid, CancellationToken cancellationToken);
        Task<List<UserOriginalWord>> GetWordsWithFilter(Guid userGuid, FilterWordDto filter, CancellationToken cancellationToken);
        Task<WordDto> GetWord(Guid userGuid, Guid wordGuid, CancellationToken cancellationToken);
        Task<SearchedWordDto> SearchWord(string word, Guid userGuid, CancellationToken cancellationToken);
        Task<Guid> CreateWord(WordDto wordDto, Guid userGuid, CancellationToken cancellationToken);
        Task<Guid> UpdateWord(WordDto wordDto, Guid userGuid, CancellationToken cancellationToken);
        Task DeleteWord(Guid wordGuid, Guid userGuid, CancellationToken cancellationToken);
    }
}
