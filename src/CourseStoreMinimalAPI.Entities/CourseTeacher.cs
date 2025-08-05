using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseStoreMinimalAPI.Entities
{
    public class CourseTeacher
    {
        public int CourseID { get; set; }
        public int TeacherID { get; set; }
        public Course Course { get; set; }
        public Teacher Teacher { get; set; }
        public int SortOrder { get; set; }
    }
}
