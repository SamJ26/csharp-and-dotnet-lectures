using School.DAL.Entities;

namespace School.DAL;

public static class Database
{
    public static ICollection<AddressEntity> Addresses { get; } = new List<AddressEntity>();
    public static ICollection<CourseEntity> Courses { get; } = new List<CourseEntity>();
    public static ICollection<ExamEntity> Exams { get; } = new List<ExamEntity>();
    public static ICollection<StudentEntity> Students { get; } = new List<StudentEntity>();

    public static void ShowData()
    {
        Console.WriteLine("--------------------------");
        
        Console.WriteLine("# Addresses:");
        foreach (var a in Addresses)
        {
            Console.WriteLine(a);
        }
        
        Console.WriteLine("# Courses:");
        foreach (var c in Courses)
        {
            Console.WriteLine(c);
        }
        
        Console.WriteLine("# Exams:");
        foreach (var e in Exams)
        {
            Console.WriteLine(e);
        }
        
        Console.WriteLine("# Students:");
        foreach (var s in Students)
        {
            Console.WriteLine(s);
        }
        
        Console.WriteLine("--------------------------");
    }
}