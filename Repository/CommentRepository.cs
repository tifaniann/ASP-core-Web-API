using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.DTOS.Comment;
using api.Interfaces;
using api.Mappers;
using api.Models;
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

        public async Task<Comment> createAsync(Comment commentmodel)
        {
            int maxId = await _context.Comments.MaxAsync(c => (int?)c.Id) ?? 0;

            // Reset IDENTITY untuk ambil id maksimum yang ada di tabel Comments
            await _context.Database.ExecuteSqlInterpolatedAsync($"DBCC CHECKIDENT ('Comments', RESEED, {maxId})");
            await _context.Comments.AddAsync(commentmodel);
            await _context.SaveChangesAsync();
            return commentmodel;
        }

        public async Task<CommentDto> createAsyncDto(CreateCommentDto commentDtomodel, int stockId)
        {
            int maxId = await _context.Comments.MaxAsync(c => (int?)c.Id) ?? 0;

            // Reset IDENTITY untuk ambil id maksimum yang ada di tabel Comments
            await _context.Database.ExecuteSqlInterpolatedAsync($"DBCC CHECKIDENT ('Comments', RESEED, {maxId})");

            // Insert comment baru
            var commentModel = commentDtomodel.ToCommentFromCreate(stockId);
            await _context.Comments.AddAsync(commentModel);
            await _context.SaveChangesAsync();

            return commentModel.ToCommentDto();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var stock = await _context.Comments.FindAsync(id);
            if (stock == null)
            {
                return false; 
            }
            _context.Comments.Remove(stock); // Hapus data comments dari tabel comments di database
            await _context.SaveChangesAsync(); // Simpan perubahan ke database
            return true;
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

        public async Task<Comment?> UpdateAsync(int id, Comment Commentmodeldto)
        {
            var commentUpdate = await _context.Comments.FindAsync(id);
            if (commentUpdate == null)
            {
                return null;
            }

            commentUpdate.Title = Commentmodeldto.Title;
            commentUpdate.Content = Commentmodeldto.Content;
            await _context.SaveChangesAsync();
            return commentUpdate;
        }

        public async Task<CommentDto?> UpdateAsyncDto(int id, UpdateCommentDto Commentmodeldto)
        {
            var comModel = await _context.Comments.FirstOrDefaultAsync(s => s.Id == id);
            if (comModel == null)
            {
                return null;
            }
            Commentmodeldto.MapToExistingComment(comModel); 
            await _context.SaveChangesAsync();
            return comModel.ToCommentDto(); 
        }
    }
}