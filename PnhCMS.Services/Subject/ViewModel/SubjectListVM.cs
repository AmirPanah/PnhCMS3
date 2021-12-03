using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PnhCMS.Services.Subject.ViewModel
{
   public class SubjectListVM
    {
        public int SubjectId { get; set; }
        public string SubjectCode { get; set; }
        public string SubjectName { get; set; }
        public string CourseName { get; set; }
        public string TeacherName { get; set; }
        public double AvgOfGrades { get; set; }
        public int NoOfStudent { get; set; }
    }
}
