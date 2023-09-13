using Google.Cloud.Firestore;

namespace Api.Biz
{
    [FirestoreData]
    public class Schedule
    {
        [FirestoreProperty]

        public List<ScheduleDay> ScheduleDays { get; set; }
        
        public Schedule()
        {
            ScheduleDays = new List<ScheduleDay>();
        }
    }
}

