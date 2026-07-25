using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {


    }



    protected void Button1_Click(object sender, EventArgs e)
    {
        string apiUrl = "https://soa.nstc.gov.tw/SOA/api/Users/"; // API URL
        string startTime = HttpUtility.UrlEncode("2000-01-01 00:00:00");
        string endTime = HttpUtility.UrlEncode("2099-01-01 00:00:00");
        string groupapi = "https://soa.nstc.gov.tw/SOA/api/Orgs/All?govCode=C&nscuCode=61&level=3&action=A&startTime=" + startTime + "&endTime= " + endTime;
        string apiKey = "SOASIMPLETOKEN"; // API Key
        string uid = TextBox1.Text;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        using (WebClient client = new WebClient())
        {

            client.Headers.Add("SOA-Token", apiKey);////設定API密鑰
            client.Encoding = Encoding.UTF8;
            string userResponse = client.DownloadString(apiUrl + uid);
            JObject userJson = JObject.Parse(userResponse);
            string orgCode = userJson["data"]["orgCode"].ToString();
            //////////////////
            string groupResponse = client.DownloadString(groupapi);
            JObject groupJson = JObject.Parse(groupResponse);
            //var department = groupJson["data"]
            //   .FirstOrDefault(d => d["nscuCode"].ToString() == orgCode);
            //string nscuDesc = "";//科室
            //if (department != null)
            //{

            //    nscuDesc = department["nscuDesc"].ToString();

            //}
            int count = groupJson["data"].Count();
            Dictionary<string, string> departmentDictionary = new Dictionary<string, string>();
            foreach (var department in groupJson["data"])
            {
                // 提取科室代碼和科室名稱
                string nscuCode = department["nscuCode"].ToString();
                string nscuDesc = department["nscuDesc"].ToString();
                string isv= department["isVirtual"].ToString();
                // 將代碼和科室名稱加入字典中
                if (!string.IsNullOrEmpty(nscuCode) && !string.IsNullOrEmpty(nscuDesc)&& !string.IsNullOrEmpty(nscuDesc)&& isv!="Y")
                {
                    departmentDictionary[nscuCode] = nscuDesc;
                }
            }
            foreach (var kvp in departmentDictionary)
            {
                Label1.Text += kvp.Key + ","+ kvp.Value+ "<br/>";  // 顯示科室代碼
            }
            //Response.Write(nscuDesc);
            //Response.Write(groupJson["data"]["nscuDesc"].ToString());




        }
    }
}