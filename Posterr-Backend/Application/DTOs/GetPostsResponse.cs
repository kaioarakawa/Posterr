using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class GetPostsResponse
    {
        public int CurrentPage { get; set; }
        public int TotalPosts { get; set; }
        public List<PostDto> Posts { get; set; }
    }
}
