using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Interfaces;
using Microsoft.AspNetCore.Mvc;
using api.Data;
using api.DTOS.Comment;
using api.Mappers;

namespace api.Controllers
{
    [Route("api/comments")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepo;
        private readonly IStockRepository _stockRepo;
        public CommentController(ICommentRepository commentRepo, IStockRepository stockRepo)
        {
            _commentRepo = commentRepo;
            _stockRepo = stockRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllComments()
        {
            var comments = await _commentRepo.GetAllAsyncDto();
            return Ok(comments);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var comment = await _commentRepo.GetByIdAsync(id);
            if (comment == null)
            {
                return NotFound($"Comment with ID {id} does not exist.");
            }
            return Ok(comment);
        }

        [HttpPost("{stockId}")]

        public async Task<IActionResult> CreateComment(int stockId, [FromBody] CreateCommentDto commentDto)
        {
            if (!await _stockRepo.IsStockExists(stockId))
            {
                return NotFound($"Stock with ID {stockId} does not exit.");
            }

            var comment = commentDto.ToCommentFromCreate(stockId);
            await _commentRepo.createAsync(comment);
            return CreatedAtAction(nameof(GetByIdAsync), new { id = comment.Id }, comment .ToCommentDto());        
        }

       
    }
}