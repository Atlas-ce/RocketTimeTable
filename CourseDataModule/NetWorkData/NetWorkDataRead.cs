using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseDataModule.NetWorkData
{
    public class NetWorkDataRead
    {
        readonly HttpClient client = new HttpClient();
        public static string? status;
        public async Task<List<CourseAttributes>> ResponeData(string studentNumber)
        {
            return await GetNetWorkData(studentNumber);
        }
        protected async Task<List<CourseAttributes>> GetNetWorkData(string studentNumber)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync("http://1.94.20.112:5001/getCourseByID?studentId="+studentNumber);
                response.EnsureSuccessStatusCode();
                string jsonContent = await response.Content.ReadAsStringAsync();
                ResponeDataAttributes responeDataAttributes = JsonConvert.DeserializeObject<ResponeDataAttributes>(jsonContent);
                status = responeDataAttributes.status;
                return responeDataAttributes.courseDatas;
            }
            catch 
            {
                ResponeDataAttributes responeDataAttributes = new ResponeDataAttributes()
                {
                    courseDatas = new List<CourseAttributes>()
                };
                status = "010";
                return responeDataAttributes.courseDatas;
            }
        }
    }
}
