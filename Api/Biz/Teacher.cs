using Google.Cloud.Firestore;

namespace Api.Biz
{
    [FirestoreData]
    public class Teacher
    {
        [FirestoreProperty]
        public string SicilNo { get; set; }
        
        [FirestoreProperty]
        public string Name { get; set; }
        
        [FirestoreProperty]
        public string Email { get; set; }
    }
}

