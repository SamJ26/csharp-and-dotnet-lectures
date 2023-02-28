using School.DAL.Entities;

namespace School.DAL.Repositories;

public class StudentRepository : IRepository<StudentEntity>
{
    /// <summary>
    /// Method stores new student and his/her address to database
    /// </summary>
    public Guid Create(StudentEntity? entity)
    {
        if (entity is null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        entity.Id = Guid.NewGuid();
        entity.Address.Id = Guid.NewGuid();
        
        Database.Addresses.Add(entity.Address);
        Database.Students.Add(entity);

        return entity.Id;
    }

    /// <summary>
    /// Returns specified student from database
    /// </summary>
    public StudentEntity GetById(Guid id)
    {
        return Database.Students.Single(s => s.Id == id);
    }

    /// <summary>
    /// Updates modified properties of a student student
    /// Address is not updated!!!
    /// </summary>
    public StudentEntity Update(StudentEntity? entity)
    {
        if (entity is null)
        {
            throw new ArgumentNullException(nameof(entity));
        }
        var existingStudent = Database.Students.Single(s => s.Id == entity.Id);
        existingStudent.Name = entity.Name;
        return existingStudent;
    }

    /// <summary>
    /// Deletes specified student from database
    /// Related address is also deleted
    /// </summary>
    public void Delete(Guid id)
    {
        var student = Database.Students.Single(s => s.Id == id);
        Database.Addresses.Remove(student.Address);
        Database.Students.Remove(student);
    }
}