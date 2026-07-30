using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Text;

public partial class login_Repair : System.Web.UI.Page
{
    string eip = WebConfigurationManager.ConnectionStrings["eip"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        string currentUrl = HttpContext.Current.Request.Url.AbsoluteUri;
        Uri uri = new Uri(currentUrl);
        string queryString = uri.Query;
        Response.Write(queryString);

        string xmlurl = "https://intra.ctsp.gov.tw/SSO/getSession.jsp" + queryString;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        using (WebClient client = new WebClient())
        {
            try
            {
                // 發送 GET 請求並取得 XML 字串
                string xmlResponse = client.DownloadString(xmlurl);

                // 解析 XML 資料
                XDocument xmlDoc = XDocument.Parse(xmlResponse);

                // 檢查 checkSession 的值是否為 "true"
                bool isValidSession = (string)xmlDoc.Root.Element("checkSession") == "true";

                // 如果有效登入，抓取 sn, unit, cname
                if (isValidSession)
                {
                    string sn = (string)xmlDoc.Root.Element("sn");
                    string unit = (string)xmlDoc.Root.Element("unit");
                    string cname_ori = (string)xmlDoc.Root.Element("cname");
                    string cname = WebUtility.HtmlDecode(cname_ori);
                    string uid = (string)xmlDoc.Root.Element("uid") + ".ctspb";
                    string uid2 = (string)xmlDoc.Root.Element("uid");
                    string job = Getjob(uid);
                    string gid = get_unit((string)xmlDoc.Root.Element("uid"));///科室id 20250819 louis 新增用API接科室ID
                    string groupid = "";//科室

                    //20250819 louis 不使用
                    //using (SqlConnection cn = new SqlConnection(eip))
                    //{
                    //    cn.Open();
                    //    string sql = @"SELECT  [id],[name],[parent_id]  FROM [group_name] where name=@name";
                    //    using (SqlCommand cmd = new SqlCommand(sql, cn))
                    //    {
                    //        cmd.Parameters.AddWithValue("@name", unit);
                    //        SqlDataReader dr = cmd.ExecuteReader();
                    //        if (dr.Read())
                    //        {
                    //            gid = dr["parent_id"] == DBNull.Value ? dr["id"].ToString() : dr["parent_id"].ToString();
                    //            groupid = dr["id"].ToString();
                    //        }
                    //    }
                    //}

                    using (SqlConnection cn = new SqlConnection(eip))
                    {
                        cn.Open();
                        string sql = @"select user_id,t1.name,repair_user_right_id,user_group,t2.name as group_name,t1.job,HomePage from [eip_user] as t1
                                      left join group_name as t2 on t2.id=t1.user_group
                                      left join repair_UserRightSetting on t1.repair_user_right_id = repair_UserRightSetting.user_right_id
                                      where t1.sn=@sn and user_group like @user_group order by user_id desc";
									  
                        //where user_group=@user_group and t1.name=@name and 
                        //20250819單位判斷修改為and user_group=@user_group order by user_id desc
						//20250820單位判斷改為保警隊、局本部以外的帳號可判斷user_group，保警隊、局本部因API無帶入科室故不判斷
						//20250821單位判斷改為模糊查詢

                        using (SqlCommand cmd = new SqlCommand(sql, cn))
                        {
                            cmd.Parameters.AddWithValue("@sn", sn);
                            cmd.Parameters.AddWithValue("@user_group", gid + "%");
                            //cmd.Parameters.AddWithValue("@user_group", groupid);
                            //cmd.Parameters.AddWithValue("@name", cname);
                            Response.Write(sql);
                            SqlDataReader dr = cmd.ExecuteReader();
                            if (dr.Read())
                            {

                                Session["user_id"] = dr["user_id"].ToString();
                                Session["user_right_id"] = dr["repair_user_right_id"].ToString();
                                Session["user_name"] = dr["name"].ToString();
                                Session["user_group"] = dr["user_group"].ToString();
                                Session["group_name"] = dr["group_name"].ToString();
                                Session["user_job"] = job;
                                Session["login_t"] = DateTime.Now.ToString("yyyy/MM/dd HH:mm"); ;

                                // 同步職位 20260730
                                using (SqlConnection cn2 = new SqlConnection(eip))
                                {
                                    string sql2 = @"Update eip_user set job = @job where user_id = @user_id";
                                    using (SqlCommand cmd2 = new SqlCommand(sql2, cn2))
                                    {
                                        cmd2.Parameters.AddWithValue("@job", job);
                                        cmd2.Parameters.AddWithValue("@user_id", dr["user_id"].ToString());
                                        cn2.Open();
                                        cmd2.ExecuteNonQuery();
                                    }
                                }




                                //if (Session["user_right_id"].ToString() == "1")
                                //{
                                //    Response.Redirect("Repair_management_query.aspx");
                                //}
                                //else
                                //{
                                //    Response.Redirect("Repair_myapply.aspx");
                                //}
                                if (dr["HomePage"].ToString() != "0" && dr["HomePage"] != DBNull.Value)
                                {
                                    if (dr["HomePage"].ToString() == "1")
                                        Response.Redirect("Repair_myapply.aspx");
                                    else if (dr["HomePage"].ToString() == "2")
                                        Response.Redirect("Repair_management_query.aspx");
                                    else if (dr["HomePage"].ToString() == "3")
                                        Response.Redirect("Repair_inventory.aspx");
                                    else if (dr["HomePage"].ToString() == "4")
                                        Response.Redirect("Repair_inventory_list.aspx");
                                    else if (dr["HomePage"].ToString() == "5")
                                        Response.Redirect("Repair_management.aspx");
                                    else if (dr["HomePage"].ToString() == "6")
                                        Response.Redirect("Repair_user_setting.aspx");
                                    else Response.Redirect("Repair_myapply.aspx");//HomePage無對應首頁預設導向
                                }
                                else if (dr["HomePage"] == DBNull.Value)
                                    Response.Redirect("Repair_myapply.aspx");//無設定帳號權限預設導向
                                else
                                    Response.Redirect("systemBusy.aspx");

                            }
                        }
                    }

                    using (SqlConnection cn = new SqlConnection(eip))
                    {
                        cn.Open();
                        string sql = @"INSERT INTO [eip_user] (sn,account,name,user_group,job,user_right_id,purchase_user_right_id,supply_user_right_id,metting_user_right_id,specil_user_right_id,repair_user_right_id,state) 
								   VALUES (@sn,@account,@name,@user_group,@job,@user_right_id,@purchase_user_right_id,@supply_user_right_id,@metting_user_right_id,@specil_user_right_id,@repair_user_right_id,@state)";
                        using (SqlCommand cmd = new SqlCommand(sql, cn))
                        {
                            cmd.Parameters.AddWithValue("@account", uid2);
                            cmd.Parameters.AddWithValue("@sn", sn);
                            cmd.Parameters.AddWithValue("@name", cname);
                            cmd.Parameters.AddWithValue("@user_group", gid);
                            cmd.Parameters.AddWithValue("@job", job);
                            cmd.Parameters.AddWithValue("@user_right_id", 0);
                            cmd.Parameters.AddWithValue("@purchase_user_right_id", 0);
                            cmd.Parameters.AddWithValue("@supply_user_right_id", 0);
                            cmd.Parameters.AddWithValue("@metting_user_right_id", 0);
                            cmd.Parameters.AddWithValue("@specil_user_right_id", 0);
                            cmd.Parameters.AddWithValue("@repair_user_right_id", 0);
                            cmd.Parameters.AddWithValue("@state", 1);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    using (SqlConnection cn = new SqlConnection(eip))
                    {
                        cn.Open();
                        string sql = @"select user_id,t1.name,repair_user_right_id,user_group,t2.name as group_name,t1.job,HomePage from [eip_user] as t1
                                      left join group_name as t2 on t2.id=t1.user_group
                                      left join repair_UserRightSetting on t1.repair_user_right_id = repair_UserRightSetting.user_right_id
                                      where t1.sn=@sn and user_group like @user_group order by user_id desc";

                        //where user_group=@user_group and t1.name=@name and 
                        //20250819單位判斷修改為and user_group=@user_group order by user_id desc
						//20250820單位判斷改為保警隊、局本部以外的帳號可判斷user_group，保警隊、局本部因API無帶入科室故不判斷
						//20250821單位判斷改為模糊查詢

                        using (SqlCommand cmd = new SqlCommand(sql, cn))
                        {
                            cmd.Parameters.AddWithValue("@sn", sn);
                            cmd.Parameters.AddWithValue("@user_group", gid + "%");
                            //cmd.Parameters.AddWithValue("@user_group", groupid);
                            //cmd.Parameters.AddWithValue("@name", cname);
                            //Response.Write(sql);
                            SqlDataReader dr = cmd.ExecuteReader();
                            if (dr.Read())
                            {

                                Session["user_id"] = dr["user_id"].ToString();
                                Session["user_right_id"] = dr["repair_user_right_id"].ToString();
                                Session["user_name"] = dr["name"].ToString();
                                Session["user_group"] = dr["user_group"].ToString();
                                Session["group_name"] = dr["group_name"].ToString();
                                Session["user_job"] = dr["job"].ToString();
                                Session["login_t"] = DateTime.Now.ToString("yyyy/MM/dd HH:mm"); ;
                                //if (Session["user_right_id"].ToString() == "1")
                                //{
                                //    Response.Redirect("Repair_management_query.aspx");
                                //}
                                //else
                                //{
                                //    Response.Redirect("Repair_myapply.aspx");
                                //}
                                if (dr["HomePage"].ToString() != "0" && dr["HomePage"] != DBNull.Value)
                                {
                                    if (dr["HomePage"].ToString() == "1")
                                        Response.Redirect("Repair_myapply.aspx");
                                    else if (dr["HomePage"].ToString() == "2")
                                        Response.Redirect("Repair_management_query.aspx");
                                    else if (dr["HomePage"].ToString() == "3")
                                        Response.Redirect("Repair_inventory.aspx");
                                    else if (dr["HomePage"].ToString() == "4")
                                        Response.Redirect("Repair_inventory_list.aspx");
                                    else if (dr["HomePage"].ToString() == "5")
                                        Response.Redirect("Repair_management.aspx");
                                    else if (dr["HomePage"].ToString() == "6")
                                        Response.Redirect("Repair_user_setting.aspx");
									else Response.Redirect("Repair_myapply.aspx");//HomePage無對應首頁預設導向
                                }
								else if (dr["HomePage"] == DBNull.Value)
                                    Response.Redirect("Repair_myapply.aspx");//無設定帳號權限預設導向
                                else
                                    Response.Redirect("systemBusy.aspx");

                            }
                        }
                    }
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                Response.Write("<p><b>An error occurred:</b> " + ex.Message + "</p>");
            }

        }
    }

    protected string get_unit(string sn) //20250819 louis 用uid帶入API查科室代號
    {
        string unit = "";
        string url = "https://soa.nstc.gov.tw/SOA/api/Users/" + sn + ".ctspb";

        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        request.Method = "GET";
        request.ContentType = "application/json; charset=utf-8";

        // 加入 SOA-Token header
        request.Headers.Add("SOA-Token", "SOASIMPLETOKEN");

        using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
        using (Stream stream = response.GetResponseStream())
        using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
        {
            string json = reader.ReadToEnd();

            JObject obj = JObject.Parse(json);

            string orgCode = obj["data"]["orgCode"].ToString();
            unit = orgCode;
        }

        return unit;
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
}