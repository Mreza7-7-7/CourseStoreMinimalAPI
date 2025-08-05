using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseStoreMinimalAPI.Entities;

public sealed class Teacher : BaseEntities
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string ImageUrl { get; set; }
    public DateTime Birthdate { get; set; }
    public List<CourseTeacher> courseTeachers { get; set; }
}

