using Google.Cloud.Firestore;

namespace Api.Biz
{
    [FirestoreData]
    public class Student
    {
        [FirestoreProperty]
        public string Number { get; set; }
        
        [FirestoreProperty]
        public string Name { get; set; }
        
        [FirestoreProperty]
        public string Surname { get; set; }
        public List<Sorumlu> Sorumlular { get; set; }
        
        [FirestoreProperty]
        public Schedule Schedule { get; set; }

        [FirestoreProperty]
        public string Grade { get; set; }

        [FirestoreProperty]
        public string Section { get; set; }

        [FirestoreProperty]
        public string IdentityNumber { get; set; }

        [FirestoreProperty]
        public DateTime BirthDate { get; set; }
        
        [FirestoreProperty]
        public List<string> Users { get; set; }

        public Student()
        {
            Sorumlular = new List<Sorumlu>();
            Users = new List<string>();
        }
    }
}


