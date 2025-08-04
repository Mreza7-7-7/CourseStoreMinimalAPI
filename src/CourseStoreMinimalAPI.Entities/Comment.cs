using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseStoreMinimalAPI.Entities;

public class Comment:BaseEntities
{
    public string  CommentBody { get; set; }
    public DateTime CommentDate { get; set; }
    public int CourseId { get; set; }
}

