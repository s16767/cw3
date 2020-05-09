using cw3.DTOs;
using cw3.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cw3.Services
{
    interface IStudentsDbService
    {
        Enrollment EnrollStudent(EnrollStudentRequest request);
        string PromoteStudent(PromoteStudentRequest request);
    }
}
