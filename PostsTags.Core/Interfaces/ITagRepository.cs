using PostsTags.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostsTags.Core.Interfaces
{
    public interface ITagRepository
    {
        Task<List<Tag>> GetAllAsync();

        Task<Tag?> GetByIdAsync(int id);

        Task<bool> ExistsByNameAsync(string name);

        Task<bool> ExistsByNameAsync(string name, int excludedId);

        Task AddAsync(Tag tag);

        void Delete(Tag tag);

        Task SaveChangesAsync();
    }
}
