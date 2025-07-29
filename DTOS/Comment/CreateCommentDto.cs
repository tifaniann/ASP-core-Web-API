using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.DTOS.Comment
{
    public class CreateCommentDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Title must be 100 characters or less.")]
        public string Title { get; set; } = string.Empty;
        [Required]
        [StringLength(500, ErrorMessage = "Content must be 500 characters or less")]
        public string Content { get; set; } = string.Empty;
      
    }
}