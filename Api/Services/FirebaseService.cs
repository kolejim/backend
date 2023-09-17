using Api.Biz;
using Google.Cloud.Firestore;

namespace Api.Services;

public class FirebaseService
{

    private ILogger<FirebaseService> _logger;

    FirestoreDb db = FirestoreDb.Create("kolejim-398905");

    public FirebaseService(ILogger<FirebaseService> logger)
    {
        _logger = logger;
    }

    public async Task Save(string user, Student student)
    {
        // todo save user into student

        var studentsCollection = db.Collection("students");
        // 
        var list = await studentsCollection.Where(Filter.EqualTo("IdentityNumber", student.Number)).GetSnapshotAsync();
        var dbStudents = list.Documents.Select(d=>d.ConvertTo<Student>()).ToList();

        var count = dbStudents.Count();
        if (count > 1)
        {
            //todo handle multiple students
            var errorMessage = "Multiple students found for number "+student.IdentityNumber;
            _logger.LogError(errorMessage);
            throw new Exception(errorMessage);
        }
        else if (count == 1)
        {
            student.Users = dbStudents[0].Users.Union(new []{user}).ToList();
            await studentsCollection.AddAsync(student);
        }
        else
        {
            // create user record
            student.Users.Add(user);
            await studentsCollection.AddAsync(student);
        }
    }
}