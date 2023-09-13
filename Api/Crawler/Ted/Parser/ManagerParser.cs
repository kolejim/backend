using System.Text.Json;
using Api.Biz;

namespace Api.Crawler.Ted.Parser;

public class ManagerParser
{
    public void Parse(string html, Student student)
    {
        var sorumlular = JsonSerializer.Deserialize<Dictionary<string, List<Sorumlu>>>(html);

        var firstStudentNumber = sorumlular.First().Key;
        var firstStudentSorumlus = sorumlular.First().Value;

        // create student model
        student.Number = firstStudentNumber;
        student.Sorumlular = firstStudentSorumlus;
    }
}