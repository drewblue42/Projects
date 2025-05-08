using System;
using System.Collections.Generic;

namespace LMS.Models.LMSModels
{
    public partial class Course
    {
        public Course()
        {
            Classes = new HashSet<Class>();
        }

        public string CourseName { get; set; } = null!;
        public string DepartmentCode { get; set; } = null!;
        public uint CourseId { get; set; }
        public uint CourseNumber { get; set; }

        public virtual Department DepartmentCodeNavigation { get; set; } = null!;
        public virtual ICollection<Class> Classes { get; set; }
    }
}
