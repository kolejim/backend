using System.Text.Json;
using Api.Biz;
using Api.Crawler.Ted.Models;
using Api.Crawler.Ted.Parser;
using Api.Services;
using HtmlAgilityPack;
using RestSharp;

namespace Api.Crawler.Ted
{
    public class TedClient
    {
        
        // Login using username and password
        public async Task<Student> LoadStudent(string username, string password)
        {
            Student student = new Student();

            var client = new RestClient("https://portal.tedankara.k12.tr/");

            RestResponse response;
            Login(username, password, client);

            // find program link
            // doc.LoadHtml(response.Content);

            // create request for dashboard/sorumlu
            response = client.ExecuteGet(new RestRequest("dashboard/sorumlu"));
            new ManagerParser().Parse(response.Content, student);

            // create request for ogrenci/{student.Number}/dersprog
            // parse response html using CourseScheduleParser
            // add schedule to student model
            response = client.ExecuteGet(new RestRequest($"ogrenci/{student.Number}/dersprog"));
            new CourseScheduleParser().Parse(response.Content, student);

            // create request for ogrenci/{student.Number} 
            // parse response html using StudentInfoParser
            response = client.ExecuteGet(new RestRequest($"ogrenci/{student.Number}"));
            new StudentParser().Parse(response.Content, student);
            
            // create request for ogrenci/{student.Number}/nufus/{student.Number}
            // parse response html using StudentIdentityParser
            response = client.ExecuteGet(new RestRequest($"ogrenci/{student.Number}/nufus/{student.Number}"));
            new StudentIdentityParser().Parse(response.Content, student);
            
            // serialize student model to json and console log
            Console.Out.WriteLine("Generated student response = {0}", JsonSerializer.Serialize(student));

            return student;
        }

        private static void Login(string username, string password, RestClient client)
        {
            var request = new RestRequest("login");
            var response = client.ExecuteGet(request);
            var doc = new HtmlDocument();
            doc.LoadHtml(response.Content);

            var token = doc.DocumentNode.SelectSingleNode("//meta[@name='_token']").Attributes["content"].Value;

            // todo body ya da parametre olarak gönderilebilir mi kontrol edilecek
            request.AddBody(new LoginModel(username, password, token));

            request.AddParameter("kimlikno", username);
            request.AddParameter("sifre", password);
            request.AddParameter("_token", token);
            response = client.ExecutePost(request);
        }
    }
}