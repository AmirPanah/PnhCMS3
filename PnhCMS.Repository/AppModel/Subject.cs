using PnhCMS.Repository.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PnhCMS.Repository.AppModel
{
    [Table("Subject")]
    public class Subject : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SubjectId { get; set; }
        [Required(ErrorMessage = "Subject code is required", AllowEmptyStrings = false)]
        public string SubjectCode { get; set; }

        [Required(ErrorMessage = "Subject name is required", AllowEmptyStrings = false)]
        public string SubjectName { get; set; }

        [Required(ErrorMessage = "Subject credit is required", AllowEmptyStrings = false)]
        public double SubjectCredit { get; set; }
        public int CourseId { get; set; }
        public virtual Course Courses { get; set; }
        public virtual ICollection<StudentEnrollment> StudentEnrollments { get; set; }
        public virtual ICollection<TeacherEnrollment> TeacherEnrollments { get; set; }
    }
    
}
