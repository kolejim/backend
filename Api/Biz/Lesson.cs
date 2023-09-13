using Google.Cloud.Firestore;

namespace Api.Biz
{
    [FirestoreData]
    public class Lesson
    {
        [FirestoreProperty]
        public string Name { get; set; }
        [FirestoreProperty]
        public Teacher Teacher { get; set; }
        [FirestoreProperty]
        public LessonTime Time { get; set; }
    }
}

