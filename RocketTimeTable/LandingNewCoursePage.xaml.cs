using System;

namespace RocketTimeTable;

public partial class LandingNewCoursePage : ContentPage
{
	public LandingNewCoursePage()
	{
		InitializeComponent();
        //Awake();
	}
    #region ÇÐ»»Ä£¿é
    private async void BootstrapNextChange(Border border,Label boderLabel , Label label ,Label nextLabel ,double progress)
    {
        border.Stroke = Color.FromArgb("#DCDCDC");
        boderLabel.TextColor = Color.FromArgb("#DCDCDC");
        label.IsVisible = false;
        nextLabel.IsVisible = true;
        await progressBar.ProgressTo(progress, 500, Easing.Linear);
    }
    private async void BootstrapBlackChange(Border border, Label boderLabel, Label label, Label backLabel, double progress)
    {
        border.Stroke = Color.FromArgb("#000000");
        boderLabel.TextColor = Color.FromArgb("#000000");
        label.IsVisible = false;
        backLabel.IsVisible = true;
        await progressBar.ProgressTo(progress, 500, Easing.Linear);
    }
    private void Next_Button_Clicked(object sender, EventArgs e)
    {
        BootstrapNextChange(crop_Boder,crop_Boder_Label,crop_Label,course_Label,0.4);
        black_Button.IsVisible=true;
        next_Button.Clicked -= Next_Button_Clicked;
        next_Button.Clicked += Step3_Clicked;
    }
    private void Step3_Clicked(object sender, EventArgs e)
    {
        BootstrapNextChange(course_Boder, course_Boder_Label, course_Label, adjust_Label,0.6);
        next_Button.Clicked -= Step3_Clicked;
        next_Button.Clicked += Step4_Clicked;
        black_Button.Clicked -= Black_Button_Clicked;
        black_Button.Clicked += BlackSep2_Clicked;
    }
    private void Step4_Clicked(object sender, EventArgs e)
    {
        BootstrapNextChange(adjust_Boder, adjust_Boder_Label, adjust_Label, information_Label,0.8);
        next_Button.Clicked -= Step4_Clicked;
        next_Button.Clicked += Step5_Clicked;
        black_Button.Clicked -= BlackSep2_Clicked;
        black_Button.Clicked += BlackSep3_Clicked;
    }
    private void Step5_Clicked(object sender, EventArgs e)
    {
        BootstrapNextChange(information_Boder,information_Boder_Label, information_Label,teacher_Label,1.0);
        black_Button.Clicked -= BlackSep3_Clicked;
        black_Button.Clicked += BlackSep4_Clicked;
    }

    private void Black_Button_Clicked(object sender, EventArgs e)
    {
        BootstrapBlackChange(crop_Boder, crop_Boder_Label, course_Label, crop_Label, 0.2);
        next_Button.Clicked -= Step3_Clicked;
        next_Button.Clicked += Next_Button_Clicked;
        black_Button.IsVisible = false;
    }
    private void BlackSep2_Clicked(object sender, EventArgs e)
    {
        BootstrapBlackChange(course_Boder, course_Boder_Label, adjust_Label, course_Label, 0.4);
        black_Button.Clicked -= BlackSep2_Clicked;
        black_Button.Clicked += Black_Button_Clicked;
        next_Button.Clicked -= Step4_Clicked;
        next_Button.Clicked += Step3_Clicked;

    }
    private void BlackSep3_Clicked(object sender, EventArgs e)
    {
        BootstrapBlackChange(adjust_Boder, adjust_Boder_Label, information_Label, adjust_Label, 0.6);
        black_Button.Clicked -= BlackSep3_Clicked;
        black_Button.Clicked += BlackSep2_Clicked;
        next_Button.Clicked -= Step5_Clicked;
        next_Button.Clicked += Step4_Clicked;
    }
    private void BlackSep4_Clicked(object sender, EventArgs e)
    {
        BootstrapBlackChange(information_Boder, information_Boder_Label, teacher_Label, information_Label, 0.8);
        black_Button.Clicked -= BlackSep4_Clicked;
        black_Button.Clicked += BlackSep3_Clicked;
    }
#endregion
    private async void Quit_Button_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }
    private void Awake()
    {
        Grid views = new()
        {
            Margin =new Thickness(5)
        };
        for (int i = 0; i < 4; i++)
        {
            views.AddRowDefinition(new RowDefinition());
        }
        Label label = new()
        {
            Text= "ÔÚÏà²áÖÐ½«½ØÍ¼²Ã¼ô",
            Margin = new Thickness(5,0,0,0),
            FontSize = 18
        };
        views.SetRow(label, 0);
        views.Children.Add(label);
        for (int i = 1; i < 4; i++)
        {
            AwakeGridSet(views, AwakeShowPhoto($"unmodified{i}",$"revised{i}"),i);
        }
        scrollView.Content= views;
    }
    private void AwakeGridSet(Grid views,StackLayout stack,int row)
    {
        views.SetRow(stack,row);
        views.Children.Add(stack);
    }
    private StackLayout AwakeShowPhoto(string fileOne,string fileTwo)
    {
        StackLayout result = new()
        {
            Orientation = StackOrientation.Horizontal,
            Margin = new Thickness(6, 15, 0, 0),
            Children =
            {
                new Image()
                {
                    Source = ImageSource.FromFile(fileOne),
                    HeightRequest=280
                },
                new StackLayout()
                {
                    HorizontalOptions=LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                    Margin = new Thickness(10, 0, 10, 0),
                    Children =
                    {
                        new Label()
                        {
                            Text="²Ã¼ô",
                            FontSize = 12,
                            HorizontalOptions = LayoutOptions.Center,
                        },
                        new Image()
                        {
                            Source = ImageSource.FromFile("arrowhead"),
                            HeightRequest=20,
                            HorizontalOptions = LayoutOptions.Center,
                        }
                    }
                },
                new Image()
                {
                    Source = ImageSource.FromFile(fileTwo),
                    HeightRequest=280
                }
            }
        };
        return result;
    }




}