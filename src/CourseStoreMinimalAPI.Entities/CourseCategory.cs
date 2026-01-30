using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseStoreMinimalAPI.Entities;

public class CourseCategory
{
    public int CoursId { get; set; }
    public int CategoryId { get; set; }
    public Course Course { get; set; }
    public Category Category { get; set; }
    public int SortOrder { get; set; }
}

