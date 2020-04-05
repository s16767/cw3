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
                new Student{IdStudent=1, Firstname="Jan", Lastname="Kowalski"},
                new Student{IdStudent=2, Firstname="Anna", Lastname="Malewski"},
                new Student{IdStudent=3, Firstname="Andrzej", Lastname="Andrzejewicz"}
            };
        }

        public IEnumerable<Student> GetStudents()
        {
            return _students;
        }

        
    }
}
