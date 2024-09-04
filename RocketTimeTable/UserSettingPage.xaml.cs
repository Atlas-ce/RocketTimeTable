using Microsoft.Maui.Controls;
using PublicMethods;
using System.Drawing;
using System.Text.Json;
using UserSemesterSettingDataModel;
using UserSettingModule;

namespace RocketTimeTable;

public partial class UserSettingPage : ContentPage
{
    public UserSettingPage()
	{
		InitializeComponent();	
		Awake();
    }
	private void Awake()
	{
		SetStudentInformation();
		SetSemesterInformation();
    }
	private void SetStudentInformation()
	{
		if (DecideStudentID(DayPage.userSettingData.StudentNumber))
		{
            StudentID_Button.Text = "点击登录";
        }
		else
		{
            StudentID_Button.Text=DayPage.userSettingData.StudentNumber;
        }
	}
	private bool DecideStudentID(string result)
	{
		return result == "" || result == null ? true : false;	
    }
    private void SetSemesterInformation()
	{
        SemesterStartTime_Label.Text=DayPage.userSemesteData.SemesterStartDate.ToString("d");
		SemesterEndTime_Label.Text = DayPage.userSemesteData.SemesterStartDate.AddDays(16 * 7).ToString("d");
		ExamWeek_Label.Text = DayPage.userSemesteData.SemesterStartDate.AddDays(16 * 7).ToString("d");
		WeekToExamWeek_Label.Text = TestWeek(GetNowToExamWeek(DateTime.Now));
        PerDayCourse_Label.Text= DayPage.userSemesteData.PerDayClass.ToString();
        CourseTime_Label.Text= DayPage.userSemesteData.CourseTime.ToString()+"分钟";
    }
	private string TestWeek(int result)
	{
		if (result >= 0)
		{
			return "距离考试周还有" + result.ToString() + "周";

        }
		else if (result == -1)
		{
			return "正在考试周";
		}
		else
		{
			return "学期结束";

        }
	}
	private int GetNowToExamWeek(DateTime now)
	{
		return 16 - DayPage.GetWeekConut(now);
	}
	private void ShowEachCourseTime(UserSemesterSettingAttributes user)
	{
		for(int i = 1;i < user.PerDayClass+1; i++) 
		{
			StackLayout views = new StackLayout()
			{
				Orientation = StackOrientation.Horizontal,
				HorizontalOptions = LayoutOptions.Center,
				Padding = new Thickness(0,10,0,0),
				VerticalOptions= LayoutOptions.Center,
				Children =
				{
					new Label
					{
						Text="第"+i.ToString()+"天",
					},
					new Button
					{
						BackgroundColor=Microsoft.Maui.Graphics.Color.FromArgb("#FFBF00"),
						CornerRadius=5,
						Margin=new Thickness(5,0,0,0),
						HeightRequest=20,
						WidthRequest=40,
						Text=user.EachCourseStartTime[i-1].ToString(),
						Padding=new Thickness(0)
					},
                    new Label
                    {
                        Text="—",
						FontAttributes= FontAttributes.Bold
                    },
                    new Button
                    {
                        BackgroundColor=Microsoft.Maui.Graphics.Color.FromArgb("#FFBF00"),
                        CornerRadius=5,
                        Margin=new Thickness(5,0,0,0),
                        HeightRequest=20,
                        WidthRequest=40,
                        Text=user.EachCourseEndTime[i-1].ToString(),
                        Padding=new Thickness(0)
                    }
                }
            };
			CourseTime_StackLayout.Children.Add(views);
        }
	}
	private void RemoveEachCourseTime()
	{
		CourseTime_StackLayout.Children.Clear();
    }

    private async void StudentID_Button_Clicked(object sender, EventArgs e)
    {
		string result = await DisplayPromptAsync("", "请输入学号");
		if (DecideStudentID(result))
		{
            await DisplayAlert("提示", "不能为空", "确认");
        }
		else
		{
            DayPage.userSettingData.StudentNumber = result;
            StudentID_Button.Text = DayPage.userSettingData.StudentNumber;
			ChangeSaved("UserSettingDataS", JsonSerializer.Serialize(DayPage.userSettingData));
			EventClass eventClass=new EventClass();
            eventClass.SetValue();
        }
    }
    //private void SettingsChangeSaved()
    //{
    //    ModuleDataStorage.UseFileStream("UserSettingData", JsonSerializer.Serialize(DayPage.userSettingData));
    //}
	private void ChangeSaved(string TxtName,string SavedData)
	{
		ModuleDataStorage.UseFileStream(TxtName, SavedData);
	}
    #region 学期设置修改
	private void SemesterSettingChange(DateTime SemesterStartTime , int DayliyCourses )
	{
		DayPage.userSemesteData.SemesterStartDate = SemesterStartTime;
		ChangeSaved("UserSemesterSettingData", JsonSerializer.Serialize(DayPage.userSemesteData));
	}
    #endregion
    private async void AddNewCourse_Clicked(object sender, EventArgs e)
    {
	     await Navigation.PushModalAsync(new LandingNewCoursePage());
    }
}