using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cw3.Models
{
    public class Enrollment
    {
        public string Semester { get; set; }
        public string StudyName { get; set; }
        public string StartDate { get; set; }

        public Enrollment(string semester, string studyname, string startdate)
        {
            Semester = semester;
            StudyName = studyname;
            StartDate = startdate;
        }

        public Enrollment()
        {

        }

    }
}
