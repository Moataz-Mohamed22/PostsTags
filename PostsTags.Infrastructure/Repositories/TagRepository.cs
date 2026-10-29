using Microsoft.EntityFrameworkCore;
using PostsTags.Core.Entities;
using PostsTags.Core.Interfaces;
using PostsTags.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostsTags.Infrastructure.Repositories
{
    public class TagRepository : ITagRepository
    {
        private readonly AppDbContext _context;

        public TagRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Tag>> GetAllAsync()
        {
            var tags =await _context.Tags.AsNoTracking().ToListAsync();
            return tags;
        }

        public async Task<Tag?> GetByIdAsync(int id)
        {
           var tag =await _context.Tags.AsNoTracking().Include(t => t.PostTags)
                .FirstOrDefaultAsync(t => t.Id == id);
            return tag;
        }
        public async Task AddAsync(Tag tag)
        {
           await _context.
                Tags.AddAsync(tag);
        }

        public void Delete(Tag tag)
        {
            _context.Tags.Remove(tag);
        }

        public async Task<bool> ExistsByNameAsync(string name)
        {
            return await _context.Tags.AnyAsync(t => t.Name == name);
        }

        public async Task<bool> ExistsByNameAsync(string name, int excludedId)
        {
            return await _context.Tags.AnyAsync(t => t.Name == name && t.Id != excludedId);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
