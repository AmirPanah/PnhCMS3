using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PnhCMS.Services.Course.ViewModels
{
   public class CourseListVM
    {
        public int CourseId { get; set; }
        public string CourseCode { get; set; }
        public string CourseName { get; set; }
        public int NoOfTeacher { get; set; }
        public int NoOfStudent { get; set; }
        public double Grade { get; set; }

        public double AvgOfGrade { get; set; }
    }
}
