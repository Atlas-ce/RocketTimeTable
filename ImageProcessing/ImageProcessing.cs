using CourseDataModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkiaSharp;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;
using System.Xml.Linq;
using System.Runtime.CompilerServices;

namespace ImageProcessing.Platforms
{
    public class ImageProcessing
    {
        private static List<CourseAttributes> newCourse = new List<CourseAttributes>();
        public ImageProcessing(string imagePath, string processingMoudle)
        {

        }
        private void ImageCropping(SKBitmap bitmap, SKCanvas canvas, int height, int width, int i, int j)
        {
            SKRect cropRect = new SKRect(width * i, height * j, width * i + width, height * j + height);
            canvas.ClipRect(cropRect);
            canvas.DrawBitmap(bitmap, 0, 0);
        }
        private async Task<string> GetTextOfTheImage(SKData data)
        {
            MemoryStream ms = new MemoryStream();
            data.SaveTo(ms);
            byte[] arrs = new byte[ms.Length];
            ms.Position = 0;
            ms.Read(arrs, 0, (int)ms.Length);
            return await AccessTheInterface(arrs);
        }
        private async Task<string> AccessTheInterface(byte[] arrs)
        {
            HttpClient client = new HttpClient();
            var content = new StringContent(JsonSerializer.Serialize(Convert.ToBase64String(arrs)), Encoding.UTF8, "application/json");
            var res = await client.PostAsync("http://1.94.20.112:5000/picture", content);
            return await res.Content.ReadAsStringAsync();
        }
        private async Task CourseNameRecognition(string imagePath)
        {
            using (SKBitmap bitmap = SKBitmap.Decode(File.ReadAllBytes(imagePath)))
            {
                for (int i = 0; i < 5; i++)
                {
                    for (int j = 1; j < 6; j++)
                    {
                        using (SKBitmap outputBitmap = new SKBitmap(bitmap.Width, bitmap.Height))
                        {
                            using (SKCanvas canvas = new SKCanvas(outputBitmap))
                            {
                                ImageCropping(bitmap, canvas, bitmap.Height / 6, bitmap.Width / 5, i, j);
                                using (SKImage image = SKImage.FromBitmap(outputBitmap))
                                using (SKData data = image.Encode())
                                {
                                    List<ImageText> imageText = new List<ImageText>
                                    {
                                        JsonConvert.DeserializeObject<ImageText>(await GetTextOfTheImage(data))
                                    };
                                    string coursName = null;
                                    foreach (var item in imageText)
                                    {
                                        foreach (var results in item.results)
                                        {
                                            foreach (var imageTextResults in results)
                                            {
                                                coursName += imageTextResults.text;
                                            }
                                        }
                                    }
                                    if (!string.IsNullOrEmpty(coursName))
                                    {
                                        newCourse.Add(new CourseAttributes
                                        {
                                            courseID = newCourse.Count + 1,
                                            courseName = coursName,
                                            courseClassRoom = null,
                                            courseInstructor = null,
                                            courseStartRanking = default,
                                            courseEndRanking = default,
                                            CourseStartTime = null,
                                            CourseEndTime = null,
                                            courseWeek = i + 1,
                                            courseDuration = null
                                        });
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        private List<string> DataProcessing(List<ImageText> imageText)
        {
            List<string> courseResults = new List<string>();
            foreach (var item in imageText)
            {
                foreach (var results in item.results)
                {
                    foreach (var imageTextResults in results)
                    {
                        if (imageTextResults != null)
                        {
                            courseResults.Add(imageTextResults.text.ToString());
                        }
                    }
                }
            }
            return courseResults;
        }
        private void SetcourseDuration(string courseResults, int count)
        {
            try
            {
                string courseWeekString = courseResults.Remove(courseResults.Length - 1);
                string[] partss = courseWeekString.Split('-');
                string courseWeek = null;
                for (int g = Convert.ToInt32(partss[0]); g <= Convert.ToInt32(partss[1]); g++)
                {
                    courseWeek += g.ToString() + ",";
                }
                newCourse[count].courseDuration = courseWeek.Remove(courseWeek.Length - 1);
            }
            catch { }
        }
        private void SetCourseStartAndEndResult(string courseResult, int count, string[] EachCourseStartTime, string[] EachCourseEndTime)
        {
            string courseRanking = courseResult.Remove(courseResult.Length - 1);
            string[] parts = courseRanking.Split('-');
            newCourse[count].courseStartRanking = Convert.ToInt32(parts[0]);
            newCourse[count].courseEndRanking = Convert.ToInt32(parts[1]);
            newCourse[count].CourseStartTime = EachCourseStartTime[Convert.ToInt32(parts[0]) - 1];
            newCourse[count].CourseEndTime = EachCourseEndTime[Convert.ToInt32(parts[1]) - 1];
        }
        private int ChangeNewCourses(string courseResultFirst, string courseResultSecond, int count, string[] EachCourseStartTime, string[] EachCourseEndTime)
        {
            SetcourseDuration(courseResultFirst, count);
            SetCourseStartAndEndResult(courseResultSecond, count, EachCourseStartTime, EachCourseEndTime);
            return count + 1;
        }
        private async Task CourseResultsRecognition(string imagePath, string[] EachCourseStartTime, string[] EachCourseEndTime)
        {
            int count = 0;
            using (SKBitmap bitmap = SKBitmap.Decode(File.ReadAllBytes(imagePath)))
            {
                for (int i = 0; i < 5; i++)
                {
                    for (int j = 1; j < 6; j++)
                    {
                        using (SKBitmap outputBitmap = new SKBitmap(bitmap.Width, bitmap.Height))
                        {
                            using (SKCanvas canvas = new SKCanvas(outputBitmap))
                            {
                                ImageCropping(bitmap, canvas, bitmap.Height / 6, bitmap.Width / 5, i, j);
                                using (SKImage image = SKImage.FromBitmap(outputBitmap))
                                using (SKData data = image.Encode())
                                {
                                    List<ImageText> imageText = new List<ImageText>
                                    {
                                        JsonConvert.DeserializeObject<ImageText>(await GetTextOfTheImage(data))
                                    };
                                    List<string> courseResults = DataProcessing(imageText);
                                    if (courseResults.Count == 3)
                                    {
                                        newCourse[count].courseClassRoom = courseResults[0];
                                        count = ChangeNewCourses(courseResults[1], courseResults[2], count, EachCourseStartTime, EachCourseEndTime);
                                    }
                                    if (courseResults.Count == 2)
                                    {
                                        count = ChangeNewCourses(courseResults[0], courseResults[1], count, EachCourseStartTime, EachCourseEndTime);
                                    }
                                    if (courseResults.Count == 6)
                                    {
                                        newCourse[count].courseClassRoom = courseResults[0];
                                        count = ChangeNewCourses(courseResults[1], courseResults[2], count, EachCourseStartTime, EachCourseEndTime);
                                        newCourse[count].courseClassRoom = courseResults[3];
                                        count = ChangeNewCourses(courseResults[3], courseResults[4], count, EachCourseStartTime, EachCourseEndTime);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        private async Task CourseTeacherRecognition(string imagePath)
        {
            List<string> courseTeacher = new List<string>();
            using (SKBitmap bitmap = SKBitmap.Decode(File.ReadAllBytes(imagePath)))
            {
                for (int i = 0; i < 5; i++)
                {
                    for (int j = 1; j < 6; j++)
                    {
                        using (SKBitmap outputBitmap = new SKBitmap(bitmap.Width, bitmap.Height))
                        {
                            using (SKCanvas canvas = new SKCanvas(outputBitmap))
                            {
                                ImageCropping(bitmap, canvas, bitmap.Height / 6, bitmap.Width / 5, i, j);
                                using (SKImage image = SKImage.FromBitmap(outputBitmap))
                                using (SKData data = image.Encode())
                                {
                                    List<ImageText> imageText = new List<ImageText>
                                    {
                                        JsonConvert.DeserializeObject<ImageText>(await GetTextOfTheImage(data))
                                    };
                                    courseTeacher.AddRange((DataProcessing(imageText)));
                                }
                            }
                        }
                    }
                }
            }
            for (int i = 0; i < newCourse.Count(); i++)
            {
                newCourse[i].courseInstructor = courseTeacher[i];
            }
        }
    }
}
