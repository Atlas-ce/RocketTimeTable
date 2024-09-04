using CourseDataModule;
using UserSettingModule;
using CourseDataModule.NetWorkData;
using System.Security.Principal;
using UserSemesterSettingDataModel;
using System.Collections.Generic;
using Microsoft.Maui.Controls.Shapes;
using System.Text.Json;
using PublicMethods;
using CourseDataModule.LocalData;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.Maui.Controls.PlatformConfiguration;
using RocketTimeTable.ViewModel;
namespace RocketTimeTable;

public partial class DayPage : ContentPage
{
    public static List<CourseAttributes> allCourseData = new List<CourseAttributes>();
    public static UserSetiingAttributes userSettingData = new UserSetiingAttributes();
    public static UserSemesterSettingAttributes userSemesteData = new UserSemesterSettingAttributes();
    public static readonly string futureCourses = "#FFBF00";
    public static readonly string pastCourses = "#DCDCDC";
  
    public DayPage(MainModel vm)
    {
        InitializeComponent();
        Awake();
        StudenIdChange();
    }
    public void Awake()
    {
        if (IsLocalData() == false)
        {
            FistTimeOpen();
        }
        ShowCourses();
        
    }
    private void StudenIdChange()
    {
        EventClass.userSettingChange += new EventClass.UserStudentIdChange(ReFlushed);
    }
    private async void ReFlushed()
    {
        await GetCourseDataFromNet();
        ClearTimeGrid();
        ShowDailyCourse();
    }
    private void ClearTimeGrid()
    {
        AM_Grid.Children.Clear();
        PM_Grid.Children.Clear();
        Nighet_Grid.Children.Clear();
    }
    #region File
    private void FistTimeOpen()
    {
        SetUserSettingData();
        SetUserSemesterSettingData();
    }
    private bool IsLocalData()
    {
        return File.Exists(FileSystem.Current.AppDataDirectory + "\\" + "UserSettingData" + ".txt") &&
               File.Exists(FileSystem.Current.AppDataDirectory + "\\" + "UserSemesterSettingData" + ".txt");
    }
    private bool IsLocalCourseData()
    {
        return File.Exists(FileSystem.Current.AppDataDirectory + "\\" + "UserCourseData" + ".txt");
    }
    private void SetUserSettingData()
    {
        List<UserSetiingAttributes> userSetiingAttributes =
        [
            new UserSetiingAttributes
            {
                StudentNumber = default,
                NetworkPermissions = false,
                Defaultsemesterschedule = default,
                IsExamWeek = true,
                ExamWeekDuration = 2
            },
        ];
        ModuleDataStorage.UseFileStream("UserSettingData", JsonSerializer.Serialize(userSetiingAttributes));
    }
    private void SetUserSemesterSettingData()
    {
        List<UserSemesterSettingAttributes> userSemesterSettingAttributes = new List<UserSemesterSettingAttributes>();
        userSemesterSettingAttributes.Add(new UserSemesterSettingAttributes
        {
            SemesterName = "默认课表",
            SemesterStartDate = Convert.ToDateTime("2024-3-3"),
            SemesterLastsWeeks = 30,
            PerDayClass = 13,
            AM = 6,
            PM = 11,
            CourseTime = 40,
            EachCourseStartTime = new string[] { "8:20", "9:10", "10:00", "10:50", "11:40", "14:00", "14:50", "15:40", "16:30", "17:20", "19:00", "19:50", "20:40" },
            EachCourseEndTime = new string[] { "9:00", "9:50", "10:40", "11:30", "12:20", "14:40", "15:30", "16:20", "17:10", "18:00", "19:40", "20:30", "21:20" },
        });
        ModuleDataStorage.UseFileStream("UserSemesterSettingData", JsonSerializer.Serialize(userSemesterSettingAttributes));
    }
    #endregion
    private async void ShowCourses()
    {
        GetSettingData();
        GetSemesteData();
        ShowDailyInformation();
        await GetCourseData();
        ShowDailyCourse();
        ShowTomorrowCourse();
    }
	private void ShowDailyInformation()
	{
        DateTime dateTime = DateTime.Today;
        ShowDayOfWeek(dateTime);
        ShowWeekCounnt(dateTime);
    }
    private void ShowDailyCourse()
    {
        var dayCourses = (from Result in allCourseData where Result.courseWeek == Convert.ToInt32(DateTime.Now.DayOfWeek) select Result).ToList();
        List<CourseAttributes> todayWeekCourses = (List<CourseAttributes>)dayCourses;
        List<CourseAttributes> todayCourses = ReturnWeekCourse(GetWeekConut(DateTime.Now).ToString(), todayWeekCourses);
        var amCourses = (from Result in todayCourses where Result.courseStartRanking <= 5 select Result).ToList();
        List<CourseAttributes> am = (List<CourseAttributes>)amCourses;
        var pmCourses = (from Result in todayCourses where Result.courseStartRanking > 5 && Result.courseStartRanking <= 10 select Result).ToList();
        List<CourseAttributes> pm = (List<CourseAttributes>)pmCourses;
        var nightCourses = (from Result in todayCourses where Result.courseStartRanking >10 select Result).ToList();
        List<CourseAttributes> night = (List<CourseAttributes>)nightCourses;
        SetBorder(am, AM_Grid,false);
        SetBorder(pm, PM_Grid, false);
        SetBorder(night, Nighet_Grid, false);
    }
    private void ShowTomorrowCourse()
    {
        var tomorrowCourses = (from Result in allCourseData where Result.courseWeek == Convert.ToInt32(DateTime.Now.DayOfWeek + 1) select Result).ToList();
        List<CourseAttributes> tomorrowWeekCourses = (List<CourseAttributes>)tomorrowCourses;
        List<CourseAttributes> theTomorrowCourses = new List<CourseAttributes>();
        if (DateTime.Now.DayOfWeek != 0)
        {
            theTomorrowCourses = ReturnWeekCourse(GetWeekConut(DateTime.Now).ToString(), tomorrowWeekCourses);
        }
        else
        {
            theTomorrowCourses = ReturnWeekCourse((GetWeekConut(DateTime.Now) + 1).ToString(), theTomorrowCourses);
        }
        var amCourses = (from Result in theTomorrowCourses where Result.courseStartRanking <= 5 select Result).ToList();
        List<CourseAttributes> am = (List<CourseAttributes>)amCourses;
        var pmCourses = (from Result in theTomorrowCourses where Result.courseStartRanking > 5 && Result.courseStartRanking <= 10 select Result).ToList();
        List<CourseAttributes> pm = (List<CourseAttributes>)pmCourses;
        var nightCourses = (from Result in theTomorrowCourses where Result.courseStartRanking > 10 select Result).ToList();
        List<CourseAttributes> night = (List<CourseAttributes>)nightCourses;
        SetBorder(am, TomorrowAM_Grid,true);
        SetBorder(pm, TomorrowPM_Grid, true);
        SetBorder(night, TomorrowNighet_Grid, true);
    }
    private void SetBorder(List<CourseAttributes> courses, Grid grid,bool bean)
    {
        courses.Sort((course1,course2)=>course1.courseStartRanking.CompareTo(course2.courseStartRanking));
        for (int i = 0; i < courses.Count;i++)
        {
            Border courseBorder = new Border
            {
                Margin = new Thickness(0, 5, 0, 5),
                StrokeShape = new RoundRectangle
                {
                    CornerRadius = new CornerRadius(10, 10, 10, 10)
                },
                StrokeThickness = 0,
                Background = Color.FromArgb(TimeJudgment(courses[i].CourseEndTime)|| bean ? futureCourses : pastCourses),
                Content = new StackLayout
                {
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                    Children =
                    {
                        new Label
                        {
                            Text=courses[i].courseName,
                            LineBreakMode=LineBreakMode.TailTruncation,
                            WidthRequest=80,
                            //FontAttributes=FontAttributes.Bold

                        },
                         new Label
                        {
                            Text=courses[i].courseClassRoom,
                            //FontAttributes=FontAttributes.Bold
                        },
                          new Label
                        {
                            Text=courses[i].CourseStartTime+"-"+courses[i].CourseEndTime,
                            //FontAttributes=FontAttributes.Bold
                        },
                    }
                }
            };
            grid.SetColumn(courseBorder, i*2);
            grid.Children.Add(courseBorder);
        }
    }
    public static List<CourseAttributes> ReturnWeekCourse(string weekCount,List<CourseAttributes> allCourseData)
    {
        try
        {
            var result = (from Result in allCourseData
                          where ("," + Result.courseDuration + ",").Contains("," + weekCount + ",")
                          select Result).ToList();
            return (List<CourseAttributes>)result;
        }
        catch
        {
            return new List<CourseAttributes>();
        }
    }
    public static bool TimeJudgment(string endTime)
    {
        return Convert.ToDateTime(endTime)>DateTime.Now ? true : false;
    }
    #region GetUserData
    private void GetSettingData()
    {
        SettingLocalDataStorage settingLocalDataStorage = new SettingLocalDataStorage();
        userSettingData = settingLocalDataStorage.UserSettings()[0];
    }
    private void GetSemesteData()
    {
        SemesterSettingLocalDataStorage semesterSettingLocalDataStorage = new SemesterSettingLocalDataStorage();
        userSemesteData = semesterSettingLocalDataStorage.UserSettings()[0];
    }
    private async Task GetCourseData()
    {
        if (!IsLocalCourseData())
        {
            LocalDataRead localDataRead = new LocalDataRead();
            allCourseData = localDataRead.GetLocalCourseData();
            if (allCourseData.Count == 0)
            {
                await GetCourseDataFromNet();
            }
        }
        else
        {
           await GetCourseDataFromNet();
        }
    }
    private async Task GetCourseDataFromNet()
    {
        LocalDataStorage localDataStorage = new LocalDataStorage();
        NetWorkDataRead netWorkDataRead = new NetWorkDataRead();
        allCourseData = await netWorkDataRead.ResponeData(userSettingData.StudentNumber);
        if (allCourseData.Count() != 0)
        {
            localDataStorage.StorageCourseData(JsonSerializer.Serialize(allCourseData));
        }
    }
    #endregion
    #region ShowWeekCount
    private void ShowWeekCounnt(DateTime date)
    {
        WeekNumber_Label.Text ="第"+ GetWeekConut(date).ToString() + "周";
    }
    public static int GetWeekConut(DateTime now)
    {
        TimeSpan daycount = now.Subtract(userSemesteData.SemesterStartDate);
        int day = daycount.Days;
        if (day == 0)
        {
            day++;
        }
        int week = day / 7 + 1;
        return now.DayOfWeek !=0 ? week : week -1;
    }
    #endregion
    #region ShowDayOfWeek
    private void ShowDayOfWeek(DateTime date)
    {
        DayOfWeek_Label.Text = GetChineseDayOfWeek(date.DayOfWeek);
    }
    private string GetChineseDayOfWeek(DayOfWeek dayOfWeek)
    {
        switch (dayOfWeek)
        {
            case DayOfWeek.Sunday:
                return "星期日";
            case DayOfWeek.Monday:
                return "星期一";
            case DayOfWeek.Tuesday:
                return "星期二";
            case DayOfWeek.Wednesday:
                return "星期三";
            case DayOfWeek.Thursday:
                return "星期四";
            case DayOfWeek.Friday:
                return "星期五";
            case DayOfWeek.Saturday:
                return "星期六";
            default:
                throw new ArgumentOutOfRangeException(nameof(dayOfWeek));
        }
    }
    #endregion

    private  void SwipeGestureRecognizer_Swiped_1(object sender, SwipedEventArgs e)
    {
        ShowNextDay(400);
    }

    private  void SwipeGestureRecognizer_Swiped(object sender, SwipedEventArgs e)
    {
        ShowNextDay(-400);
    }
    private void ShowNextDay(int X)
    {
        Today_StackLayout.TranslateTo(0, 0, 100);
        shadow.TranslateTo(X, 0, 100);
    }
   
}