using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCoreExample
{
    public class Blog
    {
        public int Id { get; set; }
        public string url { get; set; }
        public List<Post> publicaciones { get; set; } = new();
    }
}
