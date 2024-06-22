using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class PostDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public PostDto? OriginalPost { get; set; }
    }
}
