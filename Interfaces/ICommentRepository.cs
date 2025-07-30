using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOS.Comment;
using api.Models;

namespace api.Interfaces
{
    public interface ICommentRepository
    {
        Task<List<CommentDto>> GetAllAsyncDto();
        Task<CommentDto?> GetByIdAsync(int id);
        Task<Comment> createAsync(Comment commentDtomodel);
        Task<CommentDto> createAsyncDto(CreateCommentDto commentDtomodel, int stockId);
        Task<Comment?> UpdateAsync(int id, Comment Commentmodeldto);
        Task<CommentDto?> UpdateAsyncDto(int id, UpdateCommentDto Commentmodeldto);
        Task<bool> DeleteAsync(int id);
    }
}