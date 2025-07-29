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
            await _context.Comments.AddAsync(commentmodel);
            await _context.SaveChangesAsync();
            return commentmodel;
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
    }
}