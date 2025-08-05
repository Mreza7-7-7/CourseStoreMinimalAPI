using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseStoreMinimalAPI.Entities;

public class Course : BaseEntities
{
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsOnline { get; set; }
    public string ImageUrl { get; set; }
    public List<Comment> Comments { get; set; }
    public List<CourseTeacher> CourseTeachers { get; set; }
    public List<CourseCategory> courseCategories { get; set; }
}

