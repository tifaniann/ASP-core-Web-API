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
    [Route("api/[controller]")]
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
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var comments = await _commentRepo.GetAllAsyncDto();
            return Ok(comments);
        }

        // [HttpGet("{id}")]
        [HttpGet("{id}", Name = "CommentDetails")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var comment = await _commentRepo.GetByIdAsync(id);
            if (comment == null)
            {
                return NotFound($"Comment with ID {id} does not exist.");
            }
            return Ok(comment); //
        }

        [HttpPost("{stockId}")]

        public async Task<IActionResult> CreateComment([FromRoute] int stockId, [FromBody] CreateCommentDto commentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!await _stockRepo.IsStockExists(stockId))
            {
                return BadRequest($"Stock with ID {stockId} does not exist.");
            }

            // var commentModel = commentDto.ToCommentFromCreate(stockId);
            var commentModel = await _commentRepo.createAsyncDto(commentDto, stockId);
            return CreatedAtRoute("CommentDetails", new { id = commentModel.Id }, commentModel);

            // var commentMdl = commentDto.ToCommentFromCreate(stockId);
            // await _commentRepo.createAsync(commentMdl);
            // Console.WriteLine($"Created ID: {commentMdl.Id}");
            // return CreatedAtRoute("CommentDetails", new { id = commentMdl.Id }, commentMdl);
        }

        [HttpPut("{stockId}")]
        public async Task<IActionResult> UpdateComment([FromRoute] int stockId, [FromBody] UpdateCommentDto commentDto)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var update_comment = await _commentRepo.UpdateAsyncDto(stockId, commentDto);
            if (update_comment == null)
            {
                return NotFound($"Comment with ID {stockId} does not exist.");
            }
            return CreatedAtRoute("CommentDetails", new { id = update_comment.Id }, update_comment);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment([FromRoute] int id)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var isDeleted = await _commentRepo.DeleteAsync(id);
            if (!isDeleted)
            {
                return NotFound($"Comment with ID {id} does not exist.");
            }
            return NoContent(); // Mengembalikan status 204 No Content jika berhasil menghapus
        }

       
    }
}