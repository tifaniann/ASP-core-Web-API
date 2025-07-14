using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.DTOS.Comment;
using api.Interfaces;
using api.Mappers;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDBContext _context;

        public CommentRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<List<CommentDto>> GetAllAsyncDto()
        {
            return await _context.Comments
                .Select(c => c.ToCommentDto())
                .ToListAsync();
            // Mengambil semua data dari tabel Comments di database, dan ubah jadi List<CommentDto>
        }

        public async Task<CommentDto?> GetByIdAsync(int id)
        {
            var comment = await _context.Comments.FindAsync(id);
            return comment?.ToCommentDto(); //? Jika ditemukan, ubah menjadi CommentDto
        }
    }
}