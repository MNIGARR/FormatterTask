﻿using System.Linq.Expressions;
using Formatter_Task.Data;
using Formatter_Task.Entities;
using Formatter_Task.Repositories.Abstract;

namespace Formatter_Task.Repositories.Concrete
{
    public class StudentRepository : IStudentRepository
    {
        private readonly StudentDbContext _context;

        public StudentRepository(StudentDbContext context)
        {
            _context = context;
        }

        public void Add(Student entity)
        {
            _context.Students.Add(entity);
            _context.SaveChanges();
        }

        public void Delete(Student entity)
        {
            _context.Students.Remove(entity);
            _context.SaveChanges();
        }

        public Student Get(Expression<Func<Student, bool>> expression)
        {
            var student = _context.Students.SingleOrDefault(expression);
            return student;
        }

        public IEnumerable<Student> GetAll()
        {
            var students = _context.Students;
            return students;
        }

        public void Update(Student entity)
        {
            _context.Students.Update(entity);
            _context.SaveChanges();
        }
    }
}
