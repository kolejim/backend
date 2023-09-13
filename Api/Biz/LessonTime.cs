using Google.Cloud.Firestore;

namespace Api.Biz
{
    [FirestoreData]  
    public class LessonTime
    {
        [FirestoreProperty]
        public string Name { get; set; }
        [FirestoreProperty]
        public string StartTime { get; set; }
        [FirestoreProperty]
        public string EndTime { get; set; }
    }
}
