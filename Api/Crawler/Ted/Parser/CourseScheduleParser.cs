using Api.Biz;
using HtmlAgilityPack;

namespace Api.Crawler.Ted.Parser
{

	public class CourseScheduleParser
	{
		/*
		 * Parse the course schedule page and return a list of courses
		 */
		public void Parse(string scheduleHtml, Student student)
		{
			var lessonHours = ParseHours(scheduleHtml);
			Schedule schedule = ParseDays(lessonHours, scheduleHtml);
			student.Schedule = schedule;
		}

		private Schedule ParseDays(List<LessonTime> lessonHours, string scheduleHtml)
		{
			Schedule schedule = new Schedule();

			var doc = new HtmlDocument();
			doc.LoadHtml(scheduleHtml);

			var dayRows = doc.DocumentNode.SelectNodes("//table/tbody/tr");
			// iterate over each day row
			foreach (var dayRow in dayRows)
			{
				ScheduleDay day = new ScheduleDay();
				// find the day name
				var dayName = dayRow.SelectSingleNode("th[1]").InnerText;
				day.Name = dayName;
				// find the day's courses
				var courseRows = dayRow.SelectNodes("td");
				// iterate over each course
				for (var index = 0; index < courseRows.Count; index++)
				{
					try
					{
						var courseRow = courseRows[index];
						// find the course name
						var courseName = courseRow.SelectSingleNode("span[1]").InnerText;
						// find the course's teachers
						var teacherRows = courseRow.SelectNodes("small");
						Teacher teacher = new Teacher();
						foreach (var teacherRow in teacherRows)
						{
							/*
		 * teacher html:
		 * <small style="display:block" class="text-info personel" data-sclno="3352" data-title="BEATA KÜLPINAR">
		                        BEATA KÜLPINAR<br>
		                        bkulpinar@tedankara.k12.tr
		                    </small>
		 */
							teacher.Name = teacherRow.SelectSingleNode("text()[1]").InnerText.Trim();
							teacher.Email = teacherRow.SelectSingleNode("text()[2]").InnerText.Trim();
							teacher.SicilNo = teacherRow.Attributes["data-sclno"].Value;

						}

						LessonTime time = lessonHours[index];

						// create a new course and add it to the day
						day.Lessons.Add(new Lesson()
						{
							Time = time,
							Name = courseName,
							Teacher = teacher
						});
					}
					catch (Exception ex)
					{
						
					}
				}

				schedule.ScheduleDays.Add(day);
			}

			return schedule;
		}

		private List<LessonTime> ParseHours(string scheduleHtml)
		{
			// header names are "1. Ders", "2. Ders", etc.
			// we need to find the headers and then parse the table
			// that follows each header
			// the table will contain the course name, teacher, etc.
			// we need to parse the table and then add the course to the list
			// of courses
			var doc = new HtmlDocument();
			doc.LoadHtml(scheduleHtml);
			List<LessonTime> lessons = new List<LessonTime>();
			// find th cells in thead tag
			var ths = doc.DocumentNode.SelectNodes("//thead/tr/th");
			foreach (var th in ths)
			{
				// get the text of the th cell
				var thText = th.InnerText.Trim();
				// if the text is a header, parse the table that follows
				if (!string.IsNullOrWhiteSpace(thText))
				{
					/*
					 * th content is:
								<th style="min-width: 10%" class="text-center">
									1. Ders <br>
									<small class="warning">08.15-08.55</small>
								</th>
						// extract 1. Ders as the lesson name
						// extract 08.15  as the lesson start
						// extract 08.55 as the lesson end
					 */
					// 
					LessonTime time = new LessonTime();

					// text is "1. Ders \n                    08.15-08.55"
					// extract the lesson name "1. Ders"
					var lessonName = thText.Split(new string[] { "\n" }, StringSplitOptions.None)[0].Trim();
					time.Name = lessonName;
					var lessonTime = th.SelectSingleNode("small").InnerText;
					var strings = lessonTime.Split('-');
					time.StartTime = strings[0].Trim().Replace(".", ":");
					time.EndTime = strings[1].Trim().Replace(".", ":");
					lessons.Add(time);
				}
			}

			return lessons;
		}
	}
}