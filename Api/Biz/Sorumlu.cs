using System.Text.Json.Serialization;
using Google.Cloud.Firestore;

namespace Api.Biz
{
    [FirestoreData]
    public class Sorumlu
    {
        [FirestoreProperty]
        [JsonPropertyName("ogryil")]
        public string Ogryil { get; set; }

        [FirestoreProperty]
        [JsonPropertyName("sinif")]
        public string Sinif { get; set; }
        
        [FirestoreProperty]
        [JsonPropertyName("sube")]
        public string Sube { get; set; }
        
        [FirestoreProperty]
        [JsonPropertyName("ogtkod")]
        public string Ogtkod { get; set; }
        
        [FirestoreProperty]
        [JsonPropertyName("mdyrd")]
        public string Mdyrd { get; set; }
        
        [FirestoreProperty]
        [JsonPropertyName("danisman")]
        public string Danisman { get; set; }
        [JsonPropertyName("pdrm")]
        public string Pdrm { get; set; }
        
        [FirestoreProperty]
        [JsonPropertyName("gorev")]
        public string Gorev { get; set; }
        
        [FirestoreProperty]
        [JsonPropertyName("baslik")]
        public string Baslik { get; set; }
        
        [FirestoreProperty]
        [JsonPropertyName("dahili")]
        public string Dahili { get; set; }
        
        [FirestoreProperty]
        [JsonPropertyName("e_mail")]
        public string EMail { get; set; }
    }
}

