using System.Globalization;
using Api.Biz;
using HtmlAgilityPack;

namespace Api.Crawler.Ted.Parser;

public class StudentIdentityParser
{
    public void Parse(string html, Student student)
    {
        var doc = new HtmlDocument();
        doc.LoadHtml(html);
        var name = doc.DocumentNode.SelectSingleNode("//th[text()='Ad']/following-sibling::td").InnerText;
        var surname = doc.DocumentNode.SelectSingleNode("//th[text()='Soyad']/following-sibling::td").InnerText;
        var identityNumber = doc.DocumentNode.SelectSingleNode("//th[text()='T.C. Kimlik No']/following-sibling::td").InnerText;
        var birthDate = doc.DocumentNode.SelectSingleNode("//th[text()='Doğum Tarihi']/following-sibling::td").InnerText;
        
        // set values to student
        student.Name = name;
        student.Surname = surname;
        student.IdentityNumber = identityNumber;
        
        // parse birthDate as DateTime
        DateTime birthDateAsDateTime;
        if (DateTime.TryParseExact(birthDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out birthDateAsDateTime))
		{
			student.BirthDate = birthDateAsDateTime;
		}
		else
		{
			// handle error
		}
        
        
        
    }
}