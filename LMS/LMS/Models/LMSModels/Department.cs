﻿using System;
using System.Collections.Generic;

namespace LMS.Models.LMSModels
{
    public partial class Department
    {
        public Department()
        {
            Courses = new HashSet<Course>();
            Professors = new HashSet<Professor>();
            Students = new HashSet<Student>();
        }

        public string DepartmentName { get; set; } = null!;
        public string DepartmentCode { get; set; } = null!;

        public virtual ICollection<Course> Courses { get; set; }
        public virtual ICollection<Professor> Professors { get; set; }
        public virtual ICollection<Student> Students { get; set; }
    }
}
