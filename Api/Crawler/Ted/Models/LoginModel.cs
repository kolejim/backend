namespace Api.Crawler.Ted.Models
{
    public class LoginModel
    {
        public string _token;
        public string kimlikno;
        public string sifre;
    
        public LoginModel(string kimlikno, string sifre, string token)
        {
            this._token = token;
            this.kimlikno = kimlikno;
            this.sifre = sifre;
        }
    }
}

