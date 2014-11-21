using ContosoUniversity.Models;
using System;
using System.Collections.Generic;

namespace ContosoUniversity.Models
{
    public interface IStudentRepository : IDisposable
    {
        IEnumerable<Student> GetStudents();

        Student GetStudentByID(int studentId);

        void InsertStudent(Student student);

        void DeleteStudent(int studentID);

        void UpdateStudent(Student student);

        void Save();
    }
}