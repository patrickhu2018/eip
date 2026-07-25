using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

public partial class login_test : System.Web.UI.Page
{
    string eip = WebConfigurationManager.ConnectionStrings["eip"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {


        
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;


    
   using (SqlConnection cn = new SqlConnection(eip))
        {
            string sql = @"select [id] from [group_name] ";

            SqlCommand cmd = new SqlCommand(sql, cn);
            cn.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {

                try{  
                using (WebClient client = new WebClient())
                {
                    string apiUrl = "https://soa.nstc.gov.tw/SOA/api/Orgs?govCode=C&UserStatus=All&action=A&startTime=2000-01-01 00:00:00&endTime=2025-01-01 00:00:00&nscuCode=" + dr["id"].ToString();
                    string apiKey = "SOASIMPLETOKEN";
                    client.Headers.Add("SOA-Token", apiKey);////設定API密鑰
                    client.Encoding = Encoding.UTF8;
                    string userResponse = client.DownloadString(apiUrl);
                  //  Response.Write(userResponse);
                    JObject userJson = JObject.Parse(userResponse);
                    string users = userJson["data"]["users"].ToString();

            
                    JsonReader basicInformations = new JsonTextReader(new StringReader(users));
                    while (basicInformations.Read())
                    {
                        if (basicInformations.TokenType == JsonToken.StartObject)
                        {
                            JObject jObject2 = JObject.Load(basicInformations);
                            string userId = jObject2["sn"].ToString();
                            string userDuty = jObject2["userDuty"].ToString();
                            string userName = jObject2["userName"].ToString();
                            Response.Write(userId+":"+userDuty + ":"+ userName+"<br>");
                           insertuserdata(userId.Replace(".ctspb", ""), userName, userDuty, dr["id"].ToString());
                        }
                    }


                }
             }catch{}
            }
            cn.Close();
        }


    


      
    }

    private string Getjob(string uid)
    {
        string job = "";
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        using (WebClient client = new WebClient())
        {

            string apiUrl = "https://soa.nstc.gov.tw/SOA/api/Users/";
            string apiKey = "SOASIMPLETOKEN";
            client.Headers.Add("SOA-Token", apiKey);////設定API密鑰
            client.Encoding = Encoding.UTF8;
            string userResponse = client.DownloadString(apiUrl + uid);
            JObject userJson = JObject.Parse(userResponse);
            job = userJson["data"]["userDuty"].ToString();

        }

        return job;
    }

  private void insertuserdata(string userId, string userName, string userDuty,string gid)
    {

        using (SqlConnection cn = new SqlConnection(eip))
        {
            cn.Open();
            string sql = @"INSERT INTO [eip_user2] (account,name,user_group,job,user_right_id) 
                                       VALUES (@account,@name,@user_group,@job,@user_right_id2);
                                       insert into [Car_User2] ([Account],[Name],[UserGroup_id],[UserRight_Id],job) values (@account,@name,@user_group,@user_right_id,@job) 
                                       ";
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.AddWithValue("@account", userId);
                cmd.Parameters.AddWithValue("@name", userName);
                cmd.Parameters.AddWithValue("@user_group", gid);
                cmd.Parameters.AddWithValue("@job", userDuty);
                string job = userDuty;
                if (new List<string> { "局長", "副局長", "主任秘書" }.Contains(job))
                    cmd.Parameters.AddWithValue("@user_right_id", 9);
                else if (job == "副組長")
                    cmd.Parameters.AddWithValue("@user_right_id", 6);
                else if (job == "組長")
                    cmd.Parameters.AddWithValue("@user_right_id", 3);
                else if (job == "科長")
                    cmd.Parameters.AddWithValue("@user_right_id", 2);
                else
                    cmd.Parameters.AddWithValue("@user_right_id", 1);

               cmd.Parameters.AddWithValue("@user_right_id2", 0);
                cmd.Parameters.AddWithValue("@state", 0);
                cmd.ExecuteNonQuery();
            }
            cn.Close();
        }

    }
}

