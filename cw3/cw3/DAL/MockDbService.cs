using cw3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cw3.DAL
{
    public class MockDbService : IDbService
    {
        public static IEnumerable<Student> _students;

        static MockDbService()
        {
            _students = new List<Student>
            {
                new Student{IndexNumber="s00001", Firstname="Jan", Lastname="Kowalski"},
                new Student{IndexNumber="s00002", Firstname="Anna", Lastname="Malewski"},
                new Student{IndexNumber="s00003", Firstname="Andrzej", Lastname="Andrzejewicz"}
            };
        }

        public IEnumerable<Student> GetStudents()
        {
            return _students;
        }

        
    }
}
