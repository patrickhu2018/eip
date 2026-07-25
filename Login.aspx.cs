using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Security.Principal;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Login : System.Web.UI.Page
{
    string eip = WebConfigurationManager.ConnectionStrings["eip"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        Session.Clear();

    }



    protected void login_btn_Click(object sender, EventArgs e)
    {
        if (account_tb.Text != "")
        {
            using (SqlConnection cn = new SqlConnection(eip))
            {
                string sql_matchpeeID = @"select user_id,t1.name,t1.user_right_id,metting_user_right_id,specil_user_right_id,repair_user_right_id,user_group,t2.name as group_name,t1.job,t1.alert_allow,t1.alert_allow_utime,
                                            meeting_UserRightSetting.HomePage as meeting_Homepage,meeting_show_page,
                                            repair_UserRightSetting.HomePage as repair_Homepage,repair_show_page,
                                            item_UserRightSetting.HomePage as item_Homepage,specil_show_page
                                             from [eip_user]  as t1
                                              left join group_name as t2 on t2.id=t1.user_group
                                              left join meeting_UserRightSetting on meeting_UserRightSetting.user_right_id = t1.user_right_id
                                              left join repair_UserRightSetting on repair_UserRightSetting.user_right_id = t1.user_right_id
                                              left join item_UserRightSetting on item_UserRightSetting.user_right_id = t1.user_right_id
                                              where [account]=@account "; /*and state = 1*/
                SqlCommand cmd = new SqlCommand(sql_matchpeeID, cn);
                cmd.Parameters.AddWithValue("@account", account_tb.Text);
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    Session["user_id"] = dr["user_id"].ToString();
                    Session["user_right_id"] = dr["user_right_id"].ToString();
                    Session["metting_user_right_id"] = dr["metting_user_right_id"].ToString();
                    Session["specil_user_right_id"] = dr["specil_user_right_id"].ToString();
                    Session["repair_user_right_id"] = dr["repair_user_right_id"].ToString();
                    Session["user_name"] = dr["name"].ToString();
                    Session["user_group"] = dr["user_group"].ToString();
                    Session["group_name"] = dr["group_name"].ToString();
                    Session["user_job"] = dr["job"].ToString();
                    Session["login_t"] = DateTime.Now.ToString("yyyy/MM/dd HH:mm"); ;
                    Session["alert_allow"] = dr["alert_allow"].ToString();
                    Session["alert_allow_utime"] = dr["alert_allow_utime"].ToString();
                    if (dr["meeting_Homepage"].ToString() != "0" && dr["meeting_Homepage"] != DBNull.Value)
                        ViewState["meeting_Homepage"] = dr["meeting_Homepage"].ToString();

                    if (dr["repair_Homepage"].ToString() != "0" && dr["repair_Homepage"] != DBNull.Value)
                        ViewState["repair_Homepage"] = dr["repair_Homepage"].ToString();
                    if (dr["repair_show_page"].ToString() != "0" && dr["repair_show_page"] != DBNull.Value)
                        Session["repair_show_page"] = dr["repair_show_page"].ToString();

                    if (dr["item_Homepage"].ToString() != "0" && dr["item_Homepage"] != DBNull.Value)
                        ViewState["item_Homepage"] = dr["item_Homepage"].ToString();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "xx", "alert('查無此帳號!');", true);
                    return;
                }
                cn.Close();
            }

            Button clickedButton = (Button)sender;
            switch (clickedButton.ID)
            {
                case "login_item":
                    if (ViewState["item_Homepage"] != null)
                    {
                        if (ViewState["item_Homepage"].ToString() == "1")
                            Response.Redirect("item_myapply.aspx");
                        else if (ViewState["item_Homepage"].ToString() == "2")
                            Response.Redirect("item_listquery.aspx");
                        else if (ViewState["item_Homepage"].ToString() == "3")
                            Response.Redirect("Item_management.aspx");
                        else if (ViewState["item_Homepage"].ToString() == "4")
                            Response.Redirect("item_user_setting.aspx");
                    }
                    else
                        Response.Redirect("systemBusy.aspx");
                    break;

                case "login_repair":
                    if (ViewState["repair_Homepage"] != null)
                    {
                        if (ViewState["repair_Homepage"].ToString() == "1")
                            Response.Redirect("Repair_myapply.aspx");
                        else if (ViewState["repair_Homepage"].ToString() == "2")
                            Response.Redirect("Repair_management_query.aspx");
                        else if (ViewState["repair_Homepage"].ToString() == "3")
                            Response.Redirect("Repair_inventory.aspx");
                        else if (ViewState["repair_Homepage"].ToString() == "4")
                            Response.Redirect("Repair_inventory_list.aspx");
                        else if (ViewState["repair_Homepage"].ToString() == "5")
                            Response.Redirect("Repair_management.aspx");
                        else if (ViewState["repair_Homepage"].ToString() == "6")
                            Response.Redirect("Repair_user_setting.aspx");
                    }
                    else
                        Response.Redirect("systemBusy.aspx");
                    break;
                case "login_meet":
                    Session["user_right_id"] = Session["metting_user_right_id"].ToString();
                    DateTime today = DateTime.Today;
                    DateTime firstDayOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                    DateTime firstSundayOfMonth = firstDayOfMonth;
                    while (firstSundayOfMonth.DayOfWeek != DayOfWeek.Sunday)
                    {
                        firstSundayOfMonth = firstSundayOfMonth.AddDays(1);
                    }
                    int daysSinceFirstSunday = (today - firstSundayOfMonth).Days;
                    int currentWeek = daysSinceFirstSunday / 7 + 1;
                    if (ViewState["meeting_Homepage"] != null)
                    {
                        if (ViewState["meeting_Homepage"].ToString() == "1")
                        {
                            Response.Redirect("Meetingroom_calendar.aspx?mod=w&wk=" + currentWeek + "&month=" + DateTime.Now.Month + "&year=" + DateTime.Now.Year);
                        }
                        else if (ViewState["meeting_Homepage"].ToString() == "2")
                        {
                            Response.Redirect("Meetingroom_management.aspx");
                        }
                        else if (ViewState["meeting_Homepage"].ToString() == "3")
                        {
                            Response.Redirect("Meetingroom_user_setting.aspx");
                        }
                    }
                    else
                        Response.Redirect("systemBusy.aspx");

                    break;
                default:
                    break;
            }

        }
        else
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "account_erro", "alert('請重新輸入帳號!');", true);
        }

    }
}