using System.Text.RegularExpressions;
using Api.Biz;
using HtmlAgilityPack;

namespace Api.Crawler.Ted.Parser;

public class StudentParser
{
    /*
     * This method parses the student info from the given string.
     */
   
    public void Parse(string html, Student student)
    {
        var doc = new HtmlDocument();
        doc.LoadHtml(html);

        var grade = doc.DocumentNode.SelectSingleNode("//*[@id='sonuc']/div[1]/div/div[2]/div[2]/div/div[3]/text()").InnerText.Trim();
        string gradeYear = Regex.Match(grade, @"\d+").Value;
        string gradeSection = Regex.Match(grade,@"(?<=\/\s).+").Value;
        
        student.Grade = gradeYear;
        student.Section = gradeSection;

    }
}