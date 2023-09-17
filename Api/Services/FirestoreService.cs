using Api.Biz;
using Google.Cloud.Firestore;

namespace Api.Services;

public class FirestoreService
{
    FirestoreDb db = FirestoreDb.Create("kolejim-398905");

    public async Task Save(Student student)
    {
        await db.Collection("students").AddAsync(student);
    }
}