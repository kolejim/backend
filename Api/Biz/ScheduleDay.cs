using Google.Cloud.Firestore;

namespace Api.Biz
{
    [FirestoreData]
    public class ScheduleDay
    {
        [FirestoreProperty] public string Name { get; set; }
        [FirestoreProperty] public List<Lesson> Lessons { get; set; }

        public ScheduleDay()
        {
            Lessons = new List<Lesson>();
        }
    }
}