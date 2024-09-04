
using CourseDataModule;
using Microsoft.Maui.Layouts;
using System.Globalization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RocketTimeTable;

public partial class WeekCoursePage : ContentView
{
	public WeekCoursePage()
	{
		InitializeComponent();
		ShowWeekDay();
		ShowCourseTime();
		SetCourse();
    }
	
	private void ShowWeekDay()
	{
		for (int i = 0; i < 5; i++)
		{
			Frame weekCourseFrame = new Frame()
			{
				Margin = new Thickness(1,0,1,0),
				Padding = 0,
				CornerRadius = 3,
                Content =new Label()
				{
					Text=(i+1).ToString(),
					HorizontalOptions=LayoutOptions.Center, 
					VerticalOptions=LayoutOptions.Center
				},
				BorderColor=Color.FromArgb(WeekDayColor(i+1, GetDayOfWeek())),
				BackgroundColor= Color.FromArgb(WeekDayColor(i + 1, GetDayOfWeek()))
            };
			WeekCourse_Grid.SetColumn(weekCourseFrame, i);
			WeekCourse_Grid.Children.Add(weekCourseFrame);
        }
    }
	private int GetDayOfWeek()
	{
		return Convert.ToInt32(DateTime.Now.DayOfWeek);
    }
	private void ShowCourseTime()
	{
		for (int i = 0; i < DayPage.userSemesteData.PerDayClass; i++)
		{
            AbsoluteLayout coureTimeAbsoluteLayout = new AbsoluteLayout()
            {
                HeightRequest = 100        
            };
            Label startCourseTime = new Label()
            {
                Text = DayPage.userSemesteData.EachCourseStartTime[i],
                TextColor = Color.FromArgb("#DCDCDC")
            };
            coureTimeAbsoluteLayout.SetLayoutBounds(startCourseTime, new Rect(0.2, 0.1, -1, -1));
            coureTimeAbsoluteLayout.SetLayoutFlags(startCourseTime, AbsoluteLayoutFlags.PositionProportional);
            Label endCourseTime = new Label()
            {
                Text = DayPage.userSemesteData.EachCourseEndTime[i]+"     ",
                TextColor = Color.FromArgb("#DCDCDC"),
				TextDecorations=TextDecorations.Underline
            };
            coureTimeAbsoluteLayout.SetLayoutBounds(endCourseTime, new Rect(0.4, 0.9, -1, -1));
            coureTimeAbsoluteLayout.SetLayoutFlags(endCourseTime, AbsoluteLayoutFlags.PositionProportional);
            coureTimeAbsoluteLayout.Children.Add(startCourseTime);
			coureTimeAbsoluteLayout.Children.Add(endCourseTime);
            WeekCourse_Grid.SetRow(coureTimeAbsoluteLayout, i + 1);
            WeekCourse_Grid.Children.Add(coureTimeAbsoluteLayout);
        }	
	}
	private void SetCourse()
	{
		List<CourseAttributes> thisWeekCourses = DayPage.ReturnWeekCourse(DayPage.GetWeekConut(DateTime.Now).ToString(),DayPage.allCourseData);
		ShowCourse(thisWeekCourses);
    }
	private void ShowCourse(List<CourseAttributes> course)
	{
        TapGestureRecognizer tapGestureRecognizer = new TapGestureRecognizer();
        tapGestureRecognizer.Tapped += (s, e) =>
        {
            int FrameID =Convert.ToInt32(((Frame)s).ClassId);
        };
        for (int i = 0;i < course.Count; i++)
		{
			Frame weekCourse = new Frame()
			{
                ClassId = course[i].courseID.ToString(),
				BorderColor = Color.FromArgb(SetCourseColor(course[i].courseWeek, course[i].CourseEndTime)),
				BackgroundColor = Color.FromArgb(SetCourseColor(course[i].courseWeek, course[i].CourseEndTime)),
                Margin = new Thickness(1, 1, 1, 1),
                Padding = 0,
				Content = new StackLayout()
				{
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                    Margin = new Thickness(1, 0, 1, 0),
                    Children =
                    {
                        new Label
                        {
                            VerticalOptions=LayoutOptions.Center ,
                            Text=course[i].courseName
                        },
                        new Label
                        {
                            VerticalOptions=LayoutOptions.Center ,
                            Text=course[i].courseClassRoom
                        },
                        new Label
                        {
                            Text=course[i].courseInstructor
                        },
                        new Label
                        {
                            Text=course[i].CourseStartTime+"~"
                        },
                        new Label
                        {
                            Text=course[i].CourseEndTime
                        },
                    }
                }
            };
            WeekCourse_Grid.SetRow(weekCourse, course[i].courseStartRanking);
            WeekCourse_Grid.SetRowSpan(weekCourse, course[i].courseEndRanking - course[i].courseStartRanking + 1);
            WeekCourse_Grid.SetColumn(weekCourse, course[i].courseWeek - 1);
            WeekCourse_Grid.Children.Add(weekCourse);
        }
	}
	private string SetCourseColor(int weekDay , string courseEndTime)
	{
		if(weekDay < GetDayOfWeek()) 
		{
			return DayPage.pastCourses;
		}
        else if(weekDay == GetDayOfWeek())
		{
			return DayPage.TimeJudgment(courseEndTime) ? DayPage.futureCourses : DayPage.pastCourses;
        }
		else 
		{
			return DayPage.futureCourses;
		}
    }
	private string WeekDayColor(int weekDay,int today)
	{
		if(weekDay <= today)
		{
			return DayPage.pastCourses;
		}
		else
		{
			return DayPage.futureCourses;
		}
	}
	public async void CourseChange(int CourseID)
    {
       
    }
}