using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Meetingroom_user_edit : System.Web.UI.Page
{
    string eip = WebConfigurationManager.ConnectionStrings["eip"].ConnectionString;
    private string m;
    private string id;
    protected void Page_Load(object sender, EventArgs e)
    {
        m = Request.QueryString["m"];//0查看 1編輯 2新增
        id = Request.QueryString["id"];
        MasterPage3 master = (MasterPage3)this.Master;
        Label masterLabel = (Label)master.FindControl("page_title");
        masterLabel.Text = "使用者資訊";
        Literal link_li = (Literal)master.FindControl("link_li");
        string right_id = Session["user_right_id"].ToString();
        if (!IsPostBack)
        {
            leaveDDL_group.DataBind();
            leaveDDL_group.Items.Insert(0, new ListItem("請選擇", "0"));
            changeDDL_group.DataBind();
            changeDDL_group.Items.Insert(0, new ListItem("請選擇", "0"));

            if (m == "0") { if (link_li != null) link_li.Text += "<li>使用者資訊</li>"; }
            getgroup();
            if (right_id == "1")
            {
                pl.Visible = true;
            }
            if (m == "0")
            {
                showdata();
            }
            else if (m == "1")
            {
                masterLabel.Text = "使用者設定";
                Literal1.Text = "編輯帳號";
                if (link_li != null) link_li.Text += "<li>使用者設定</li>";
                Submit.Visible = true;
                //group.Enabled = true;
                //username.Enabled = true;
                //ac.Enabled = true;
                //job.Enabled = true;
                //note.Enabled = true;
                Cancel.Text = "取消";
                showdata();
            }
            else if (m == "2")
            {
                masterLabel.Text = "使用者設定";
                Literal1.Text = "新增帳號";
                if (link_li != null) link_li.Text += "<li>使用者設定</li>";
                Submit.Visible = true;
                group.Enabled = true;
                username.Enabled = true;
                ac.Enabled = true;
                job.Enabled = true;
                note.Enabled = true;

            }
        }
    }
    private void getgroup()
    {
        using (SqlConnection cn = new SqlConnection(eip))
        {
            cn.Open();
            string sql = @"SELECT [id],[name]  FROM [group_name] order by id ASC";
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    group.Items.Add(new ListItem(dr["name"].ToString(), dr["id"].ToString()));
                }

            }
        }
    }
    private void showdata()
    {
        using (SqlConnection cn = new SqlConnection(eip))
        {
            cn.Open();
            string sql = @"SELECT [sn],[name],[user_group],[job],[user_right_id],[note],meeting_show_page,LeaveDate,JobChageDate,TransferToGroup_id,TransferToUser_id,LastUpdateTime
                            FROM [eip_user] where user_id=@user_id";
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                if (string.IsNullOrEmpty(id))
                {
                    cmd.Parameters.AddWithValue("@user_id", Session["user_id"]);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@user_id", id);
                }

                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    group.SelectedValue = dr["user_group"].ToString();
                    username.Text = dr["name"].ToString();
                    ac.Text = dr["sn"].ToString();
                    job.Text = dr["job"].ToString();
                    note.Text = dr["note"].ToString();

                    if (dr["meeting_show_page"].ToString() == "1")
                    {
                        RightSetting1.Checked = true;
                        RightSettingTr1.Attributes["class"] += " active";
                    }
                    else if (dr["meeting_show_page"].ToString() == "2")
                    {
                        RightSetting2.Checked = true;
                        RightSettingTr2.Attributes["class"] += " active";
                    }
                    else if (dr["meeting_show_page"].ToString() == "3")
                    {
                        RightSetting3.Checked = true;
                        RightSettingTr3.Attributes["class"] += " active";
                    }
                    else if (dr["meeting_show_page"].ToString() == "4")
                    {
                        RightSetting4.Checked = true;
                        RightSettingTr4.Attributes["class"] += " active";
                    }
                    else if (dr["meeting_show_page"].ToString() == "5")
                    {
                        RightSetting5.Checked = true;
                        RightSettingTr5.Attributes["class"] += " active";
                    }
                    else if (dr["meeting_show_page"].ToString() == "6")
                    {
                        RightSetting6.Checked = true;
                        RightSettingTr6.Attributes["class"] += " active";
                    }
                    else if (dr["meeting_show_page"].ToString() == "7")
                    {
                        RightSetting7.Checked = true;
                        RightSettingTr7.Attributes["class"] += " active";
                    }
                    else if (dr["meeting_show_page"].ToString() == "8")
                    {
                        RightSetting8.Checked = true;
                        RightSettingTr8.Attributes["class"] += " active";
                    }
                    else if (dr["meeting_show_page"].ToString() == "0")
                    {
                        RightSetting0.Checked = true;
                        RightSettingTr0.Attributes["class"] += " active";
                    }

                    if (dr["user_right_id"].ToString() == "3")
                    {
                        JobSetting1.Checked = true;
                        JobSettingDDL1_1.Enabled = true;
                        JobSettingDDL1_2.Enabled = true;
                        JobSettingTr1.Attributes["class"] += " active";
                    }
                    else if (dr["user_right_id"].ToString() == "4")
                    {
                        JobSetting2.Checked = true;
                        JobSettingDDL2_1.Enabled = true;
                        JobSettingDDL2_2.Enabled = true;
                        JobSettingTr2.Attributes["class"] += " active";
                    }
                    else if (dr["user_right_id"].ToString() == "5")
                    {
                        JobSetting3.Checked = true;
                        JobSettingDDL3_1.Enabled = true;
                        JobSettingDDL3_2.Enabled = true;
                        JobSettingTr3.Attributes["class"] += " active";
                    }
                    else if (dr["user_right_id"].ToString() == "6")
                    {
                        JobSetting4.Checked = true;
                        JobSettingDDL4_1.Enabled = true;
                        JobSettingDDL4_2.Enabled = true;
                        JobSettingTr4.Attributes["class"] += " active";
                    }
                    else if (dr["user_right_id"].ToString() == "7")
                    {
                        JobSetting5.Checked = true;
                        JobSettingDDL5_1.Enabled = true;
                        JobSettingDDL5_2.Enabled = true;
                        JobSettingTr5.Attributes["class"] += " active";
                    }
                    else if (dr["user_right_id"].ToString() == "8")
                    {
                        JobSetting6.Checked = true;
                        JobSettingDDL6_1.Enabled = true;
                        JobSettingDDL6_2.Enabled = true;
                        JobSettingTr6.Attributes["class"] += " active";
                    }
                    else if (dr["user_right_id"].ToString() == "9")
                    {
                        JobSetting7.Checked = true;
                        JobSettingDDL7_1.Enabled = true;
                        JobSettingDDL7_2.Enabled = true;
                        JobSettingTr7.Attributes["class"] += " active";
                    }
                    else if (dr["user_right_id"].ToString() == "2")
                    {
                        JobSetting8.Checked = true;
                        JobSettingDDL8_1.Enabled = true;
                        JobSettingDDL8_2.Enabled = true;
                        JobSettingTr8.Attributes["class"] += " active";
                    }
                    else if (dr["user_right_id"].ToString() == "1")
                    {
                        JobSetting9.Checked = true;
                        JobSettingDDL9_1.Enabled = true;
                        JobSettingDDL9_2.Enabled = true;
                        JobSettingTr9.Attributes["class"] += " active";
                    }
                    else if (dr["user_right_id"].ToString() == "10")
                    {
                        JobSetting10.Checked = true;
                        JobSettingDDL10_1.Enabled = true;
                        JobSettingDDL10_2.Enabled = true;
                        JobSettingTr10.Attributes["class"] += " active";
                    }
                    else if (dr["user_right_id"].ToString() == "11")
                    {
                        JobSetting11.Checked = true;
                        JobSettingDDL11_1.Enabled = true;
                        JobSettingDDL11_2.Enabled = true;
                        JobSettingTr11.Attributes["class"] += " active";
                    }
                    else if (dr["user_right_id"].ToString() == "0")
                    {
                        JobSetting12.Checked = true;
                        JobSettingDDL12_1.Enabled = true;
                        JobSettingDDL12_2.Enabled = true;
                        JobSettingTr12.Attributes["class"] += " active";
                    }

                    if (dr["LeaveDate"] == DBNull.Value && dr["JobChageDate"] == DBNull.Value)
                    {
                        Change1.Checked = true;
                        ChangeTr1.Attributes["class"] += " active";
                    }
                    else if (dr["LeaveDate"] != DBNull.Value)
                    {
                        Change2.Checked = true;
                        date_leave.Enabled = true;
                        leaveDDL_group.Enabled = true;
                        leaveDDL_UserID.Enabled = true;
                        date_leave.Text = Convert.ToDateTime(dr["LeaveDate"]).ToString("yyyy/MM/dd");
                        leaveDDL_group.SelectedValue = dr["TransferToGroup_id"].ToString();
                        LeaveDDL();
                        leaveDDL_UserID.SelectedValue = dr["TransferToUser_id"].ToString();
                        ChangeTr2.Attributes["class"] += " active";
                    }
                    else if (dr["JobChageDate"] != DBNull.Value)
                    {
                        Change3.Checked = true;
                        date_change.Enabled = true;
                        changeDDL_group.Enabled = true;
                        changeDDL_UserID.Enabled = true;
                        date_change.Text = Convert.ToDateTime(dr["JobChageDate"]).ToString("yyyy/MM/dd");
                        changeDDL_group.SelectedValue = dr["TransferToGroup_id"].ToString();
                        ChangeDDL();
                        changeDDL_UserID.SelectedValue = dr["TransferToUser_id"].ToString();
                        ChangeTr3.Attributes["class"] += " active";

                    }
                    if (dr["LastUpdateTime"] != DBNull.Value)
                        LastUpdateTime.Text = Convert.ToDateTime(dr["LastUpdateTime"]).ToString("yyyy/MM/dd HH:mm:ss");
                }
            }
            cn.Close();
        }
        using (SqlConnection cn = new SqlConnection(eip))
        {
            cn.Open();
            string sql = @"SELECT * FROM meeting_UserRightSetting";
            SqlCommand cmd = new SqlCommand(sql, cn);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                if (dr["user_right_id"].ToString() == "3")
                {
                    JobSettingDDL1_1.SelectedValue = dr["HomePage"].ToString();
                    JobSettingDDL1_2.SelectedValue = dr["DataRange"].ToString();
                }
                else if (dr["user_right_id"].ToString() == "4")
                {
                    JobSettingDDL2_1.SelectedValue = dr["HomePage"].ToString();
                    JobSettingDDL2_2.SelectedValue = dr["DataRange"].ToString();
                }
                else if (dr["user_right_id"].ToString() == "5")
                {
                    JobSettingDDL3_1.SelectedValue = dr["HomePage"].ToString();
                    JobSettingDDL3_2.SelectedValue = dr["DataRange"].ToString();
                }
                else if (dr["user_right_id"].ToString() == "6")
                {
                    JobSettingDDL4_1.SelectedValue = dr["HomePage"].ToString();
                    JobSettingDDL4_2.SelectedValue = dr["DataRange"].ToString();
                }
                else if (dr["user_right_id"].ToString() == "7")
                {
                    JobSettingDDL5_1.SelectedValue = dr["HomePage"].ToString();
                    JobSettingDDL5_2.SelectedValue = dr["DataRange"].ToString();
                }
                else if (dr["user_right_id"].ToString() == "8")
                {
                    JobSettingDDL6_1.SelectedValue = dr["HomePage"].ToString();
                    JobSettingDDL6_2.SelectedValue = dr["DataRange"].ToString();
                }
                else if (dr["user_right_id"].ToString() == "9")
                {
                    JobSettingDDL7_1.SelectedValue = dr["HomePage"].ToString();
                    JobSettingDDL7_2.SelectedValue = dr["DataRange"].ToString();
                }
                else if (dr["user_right_id"].ToString() == "2")
                {
                    JobSettingDDL8_1.SelectedValue = dr["HomePage"].ToString();
                    JobSettingDDL8_2.SelectedValue = dr["DataRange"].ToString();
                }
                else if (dr["user_right_id"].ToString() == "1")
                {
                    JobSettingDDL9_1.SelectedValue = dr["HomePage"].ToString();
                    JobSettingDDL9_2.SelectedValue = dr["DataRange"].ToString();
                }
                else if (dr["user_right_id"].ToString() == "10")
                {
                    JobSettingDDL10_1.SelectedValue = dr["HomePage"].ToString();
                    JobSettingDDL10_2.SelectedValue = dr["DataRange"].ToString();
                }
                else if (dr["user_right_id"].ToString() == "11")
                {
                    JobSettingDDL11_1.SelectedValue = dr["HomePage"].ToString();
                    JobSettingDDL11_2.SelectedValue = dr["DataRange"].ToString();
                }
                else if (dr["user_right_id"].ToString() == "0")
                {
                    JobSettingDDL12_1.SelectedValue = dr["HomePage"].ToString();
                    JobSettingDDL12_2.SelectedValue = dr["DataRange"].ToString();
                }
            }
            cn.Close();
        }
    }



    protected void Cancel_Click(object sender, EventArgs e)
    {
        string url = "";
        if (m != "0")
        {
            url = string.Format("Meetingroom_user_setting.aspx");
            Response.Redirect(url);
        }
        else
        {
            url = string.Format("Meetingroom_myapply.aspx?m=0");
            Response.Redirect(url);
        }


    }

    protected void Submit_Click(object sender, EventArgs e)
    {

        if (m == "1")
        {
            using (SqlConnection cn = new SqlConnection(eip))
            {
                cn.Open();
                string sql = @"update [eip_user] set [user_group]=@user_group,[name]=@name,[sn]=@sn,[job]=@job,[note]=@note,[metting_user_right_id]=@user_right_id,
                                meeting_show_page=@meeting_show_page,LeaveDate=@LeaveDate,JobChageDate=@JobChageDate,
                                TransferToGroup_id=@TransferToGroup_id,TransferToUser_id=@TransferToUser_id,LastUpdateTime=@LastUpdateTime
                                where user_id=@user_id";

                //更新後註解 20260731 by blue
                //meeting_show_page會被讀進session["user_right_id"]
                //增加metting_user_right_id的寫入
                //在特定頁會讀session["user_right_id"]，如變更會議類型要由系統管理員，系統管理員為1
                //DB內的metting_user_right_idd只是代表該使用者屬於哪個身分，身分設定內的更改會影響到所有同身分的人
                //DB內的meeting_show_page只是代表使用者的"權限狀態"，只影響顯示的Menu有哪些，對其他頁面功能，包含登入首頁是哪，沒有任何影響



                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@user_group", group.SelectedValue);
                    cmd.Parameters.AddWithValue("@name", username.Text);
                    cmd.Parameters.AddWithValue("@sn", ac.Text);
                    cmd.Parameters.AddWithValue("@job", job.Text);
                    cmd.Parameters.AddWithValue("@note", note.Text);
                    if (RightSetting1.Checked)
                        cmd.Parameters.AddWithValue("@meeting_show_page", "1");                
                    else if (RightSetting2.Checked)
                        cmd.Parameters.AddWithValue("@meeting_show_page", "2");
                    else if (RightSetting3.Checked)
                        cmd.Parameters.AddWithValue("@meeting_show_page", "3");
                    else if (RightSetting4.Checked)
                        cmd.Parameters.AddWithValue("@meeting_show_page", "4");
                    else if (RightSetting5.Checked)
                        cmd.Parameters.AddWithValue("@meeting_show_page", "5");
                    else if (RightSetting6.Checked)
                        cmd.Parameters.AddWithValue("@meeting_show_page", "6");
                    else if (RightSetting7.Checked)
                        cmd.Parameters.AddWithValue("@meeting_show_page", "7");
                    else if (RightSetting8.Checked)
                        cmd.Parameters.AddWithValue("@meeting_show_page", "8");
                    else if (RightSetting0.Checked)
                        cmd.Parameters.AddWithValue("@meeting_show_page", "0");

                   

                    if (JobSetting1.Checked)
                        cmd.Parameters.AddWithValue("@user_right_id", "3");
                    else if (JobSetting2.Checked)
                        cmd.Parameters.AddWithValue("@user_right_id", "4");
                    else if (JobSetting3.Checked)
                        cmd.Parameters.AddWithValue("@user_right_id", "5");
                    else if (JobSetting4.Checked)
                        cmd.Parameters.AddWithValue("@user_right_id", "6");
                    else if (JobSetting5.Checked)
                        cmd.Parameters.AddWithValue("@user_right_id", "7");
                    else if (JobSetting6.Checked)
                        cmd.Parameters.AddWithValue("@user_right_id", "8");
                    else if (JobSetting7.Checked)
                        cmd.Parameters.AddWithValue("@user_right_id", "9");
                    else if (JobSetting8.Checked)
                        cmd.Parameters.AddWithValue("@user_right_id", "2");
                    else if (JobSetting9.Checked)
                        cmd.Parameters.AddWithValue("@user_right_id", "1");
                    else if (JobSetting10.Checked)
                        cmd.Parameters.AddWithValue("@user_right_id", "10");
                    else if (JobSetting11.Checked)
                        cmd.Parameters.AddWithValue("@user_right_id", "11");
                    else if (JobSetting12.Checked)
                        cmd.Parameters.AddWithValue("@user_right_id", "0");

                    if (Change1.Checked)
                    {
                        cmd.Parameters.AddWithValue("@LeaveDate", DBNull.Value);
                        cmd.Parameters.AddWithValue("@JobChageDate", DBNull.Value);
                        cmd.Parameters.AddWithValue("@TransferToGroup_id", DBNull.Value);
                        cmd.Parameters.AddWithValue("@TransferToUser_id", DBNull.Value);
                    }
                    else if (Change2.Checked)
                    {
                        cmd.Parameters.AddWithValue("@LeaveDate", date_leave.Text);
                        cmd.Parameters.AddWithValue("@JobChageDate", DBNull.Value);
                        cmd.Parameters.AddWithValue("@TransferToGroup_id", leaveDDL_group.SelectedValue);
                        cmd.Parameters.AddWithValue("@TransferToUser_id", leaveDDL_UserID.SelectedValue);
                    }
                    else if (Change3.Checked)
                    {
                        cmd.Parameters.AddWithValue("@LeaveDate", DBNull.Value);
                        cmd.Parameters.AddWithValue("@JobChageDate", date_change.Text);
                        cmd.Parameters.AddWithValue("@TransferToGroup_id", changeDDL_group.SelectedValue);
                        cmd.Parameters.AddWithValue("@TransferToUser_id", changeDDL_UserID.SelectedValue);
                    }

                    cmd.Parameters.AddWithValue("@LastUpdateTime", DateTime.Now);
                    cmd.Parameters.AddWithValue("@user_id", id);
                    cmd.ExecuteNonQuery();
                    cn.Close();
                }
            }
            using (SqlConnection cn = new SqlConnection(eip))
            {
                cn.Open();
                string sql = @"update meeting_UserRightSetting set HomePage=@HomePage, DataRange=@DataRange where user_right_id=@user_right_id";
                SqlCommand cmd = new SqlCommand(sql, cn);
                if (JobSetting1.Checked)
                {
                    cmd.Parameters.AddWithValue("@HomePage", JobSettingDDL1_1.SelectedValue);
                    cmd.Parameters.AddWithValue("@DataRange", JobSettingDDL1_2.SelectedValue);
                    cmd.Parameters.AddWithValue("@user_right_id", "3");
                }
                else if (JobSetting2.Checked)
                {
                    cmd.Parameters.AddWithValue("@HomePage", JobSettingDDL2_1.SelectedValue);
                    cmd.Parameters.AddWithValue("@DataRange", JobSettingDDL2_2.SelectedValue);
                    cmd.Parameters.AddWithValue("@user_right_id", "4");
                }
                else if (JobSetting3.Checked)
                {
                    cmd.Parameters.AddWithValue("@HomePage", JobSettingDDL3_1.SelectedValue);
                    cmd.Parameters.AddWithValue("@DataRange", JobSettingDDL3_2.SelectedValue);
                    cmd.Parameters.AddWithValue("@user_right_id", "5");
                }
                else if (JobSetting4.Checked)
                {
                    cmd.Parameters.AddWithValue("@HomePage", JobSettingDDL4_1.SelectedValue);
                    cmd.Parameters.AddWithValue("@DataRange", JobSettingDDL4_2.SelectedValue);
                    cmd.Parameters.AddWithValue("@user_right_id", "6");
                }
                else if (JobSetting5.Checked)
                {
                    cmd.Parameters.AddWithValue("@HomePage", JobSettingDDL5_1.SelectedValue);
                    cmd.Parameters.AddWithValue("@DataRange", JobSettingDDL5_2.SelectedValue);
                    cmd.Parameters.AddWithValue("@user_right_id", "7");
                }
                else if (JobSetting6.Checked)
                {
                    cmd.Parameters.AddWithValue("@HomePage", JobSettingDDL6_1.SelectedValue);
                    cmd.Parameters.AddWithValue("@DataRange", JobSettingDDL6_2.SelectedValue);
                    cmd.Parameters.AddWithValue("@user_right_id", "8");
                }
                else if (JobSetting7.Checked)
                {
                    cmd.Parameters.AddWithValue("@HomePage", JobSettingDDL7_1.SelectedValue);
                    cmd.Parameters.AddWithValue("@DataRange", JobSettingDDL7_2.SelectedValue);
                    cmd.Parameters.AddWithValue("@user_right_id", "9");
                }
                else if (JobSetting8.Checked)
                {
                    cmd.Parameters.AddWithValue("@HomePage", JobSettingDDL8_1.SelectedValue);
                    cmd.Parameters.AddWithValue("@DataRange", JobSettingDDL8_2.SelectedValue);
                    cmd.Parameters.AddWithValue("@user_right_id", "2");
                }
                else if (JobSetting9.Checked)
                {
                    cmd.Parameters.AddWithValue("@HomePage", JobSettingDDL9_1.SelectedValue);
                    cmd.Parameters.AddWithValue("@DataRange", JobSettingDDL9_2.SelectedValue);
                    cmd.Parameters.AddWithValue("@user_right_id", "1");
                }
                else if (JobSetting10.Checked)
                {
                    cmd.Parameters.AddWithValue("@HomePage", JobSettingDDL10_1.SelectedValue);
                    cmd.Parameters.AddWithValue("@DataRange", JobSettingDDL10_2.SelectedValue);
                    cmd.Parameters.AddWithValue("@user_right_id", "10");
                }
                else if (JobSetting11.Checked)
                {
                    cmd.Parameters.AddWithValue("@HomePage", JobSettingDDL11_1.SelectedValue);
                    cmd.Parameters.AddWithValue("@DataRange", JobSettingDDL11_2.SelectedValue);
                    cmd.Parameters.AddWithValue("@user_right_id", "11");
                }
                else if (JobSetting12.Checked)
                {
                    cmd.Parameters.AddWithValue("@HomePage", JobSettingDDL12_1.SelectedValue);
                    cmd.Parameters.AddWithValue("@DataRange", JobSettingDDL12_2.SelectedValue);
                    cmd.Parameters.AddWithValue("@user_right_id", "0");
                }
                cmd.ExecuteNonQuery();
                cn.Close();
            }
            string url = string.Format("Meetingroom_user_setting.aspx?");
            Response.Redirect(url);
        }
        //else if (m == "2")
        //{
        //    using (SqlConnection cn = new SqlConnection(eip))
        //    {
        //        cn.Open();
        //        string sql = @"SELECT  [account] FROM [eip_user] where account=@account";
        //        using (SqlCommand cmd = new SqlCommand(sql, cn))
        //        {
        //            cmd.Parameters.AddWithValue("@account", ac.Text);
        //            SqlDataReader dr = cmd.ExecuteReader();
        //            if (dr.HasRows)
        //            {
        //                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('帳號已有人使用');", true);
        //                return;
        //            }
        //        }
        //    }
        //    using (SqlConnection cn = new SqlConnection(eip))
        //    {
        //        cn.Open();
        //        string sql = @"INSERT INTO [eip_user] ([user_group],[name],[account],[job],[note],[state],[user_right_id])
        //                   VALUES (@user_group,@name,@account,@job,@note,@state,@user_right_id)";
        //        using (SqlCommand cmd = new SqlCommand(sql, cn))
        //        {
        //            cmd.Parameters.AddWithValue("@user_group", group.SelectedValue);
        //            cmd.Parameters.AddWithValue("@name", username.Text);
        //            cmd.Parameters.AddWithValue("@account", ac.Text);
        //            cmd.Parameters.AddWithValue("@job", job.Text);
        //            cmd.Parameters.AddWithValue("@note", note.Text);
        //            cmd.Parameters.AddWithValue("@user_right_id", "0");
        //            cmd.Parameters.AddWithValue("@state", "1");
        //            cmd.ExecuteNonQuery();
        //            cn.Close();
        //        }
        //    }
        //    string url = string.Format("Meetingroom_user_setting.aspx?");
        //    Response.Redirect(url);
        //}
    }

    protected void leaveDDL_group_SelectedIndexChanged(object sender, EventArgs e)
    {
        LeaveDDL();
    }
    protected void changeDDL_group_SelectedIndexChanged(object sender, EventArgs e)
    {
        ChangeDDL();
    }

    protected void LeaveDDL()
    {
        string parent_id = leaveDDL_group.SelectedValue;
        if (leaveDDL_group.SelectedValue != "0")
        {
            leaveDDL_UserID.Items.Clear();
            ListItem li_0 = new ListItem();
            li_0.Text = "請選擇";
            li_0.Value = "0";
            leaveDDL_UserID.Items.Add(li_0);
            bool hasData = false; // 用來檢查是否有資料
            using (SqlConnection cn = new SqlConnection(eip))
            {
                string sql1 = @"SELECT 
                        g2.name AS groupName,
                            g1.name AS className,
                            c.Name AS Name,
                            c.User_id,
                            c.UserRight_Id
                        FROM Car_User AS c
                        LEFT JOIN group_name AS g1 ON c.UserGroup_id = g1.id
                        LEFT JOIN group_name AS g2 ON g1.parent_id = g2.id
						where 1=1
                        and (g1.parent_id =@gid or g1.id =@gid) and User_id <> @User_id
                        order by className";
                SqlCommand cmd = new SqlCommand(sql1, cn);
                cmd.Parameters.AddWithValue("@gid", parent_id);
                cmd.Parameters.AddWithValue("@User_id", id);
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    ListItem item = new ListItem();
                    item.Text = dr["Name"].ToString();
                    item.Value = dr["User_id"].ToString();
                    leaveDDL_UserID.Items.Add(item);
                    hasData = true; // 如果有資料，設置為 true
                }
                if (!hasData)
                {
                    leaveDDL_UserID.Items.Clear();
                    ListItem noItem = new ListItem();
                    noItem.Text = "無";
                    noItem.Value = "0";

                    leaveDDL_UserID.Items.Add(noItem);
                }
                cn.Close();
            }

        }
        else
        {
            leaveDDL_UserID.Items.Clear();
            ListItem li_0 = new ListItem();
            li_0.Text = "請選擇";
            li_0.Value = "0";
            leaveDDL_UserID.Items.Add(li_0);
            leaveDDL_UserID.SelectedValue = "0";

        }
    }

    protected void ChangeDDL()
    {
        string parent_id = changeDDL_group.SelectedValue;
        if (changeDDL_group.SelectedValue != "0")
        {
            changeDDL_UserID.Items.Clear();
            ListItem li_0 = new ListItem();
            li_0.Text = "請選擇";
            li_0.Value = "0";
            changeDDL_UserID.Items.Add(li_0);
            bool hasData = false; // 用來檢查是否有資料
            using (SqlConnection cn = new SqlConnection(eip))
            {
                string sql = @"SELECT 
                        g2.name AS groupName,
                            g1.name AS className,
                            c.Name AS Name,
                            c.User_id,
                            c.UserRight_Id
                        FROM Car_User AS c
                        LEFT JOIN group_name AS g1 ON c.UserGroup_id = g1.id
                        LEFT JOIN group_name AS g2 ON g1.parent_id = g2.id
						where 1=1
                        and (g1.parent_id =@gid or g1.id =@gid) and User_id <> @User_id
                        order by className";
                SqlCommand cmd = new SqlCommand(sql, cn);
                cmd.Parameters.AddWithValue("@gid", parent_id);
                cmd.Parameters.AddWithValue("@User_id", id);
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    ListItem item = new ListItem();
                    item.Text = dr["Name"].ToString();
                    item.Value = dr["User_id"].ToString();
                    changeDDL_UserID.Items.Add(item);
                    hasData = true; // 如果有資料，設置為 true
                }
                if (!hasData)
                {
                    changeDDL_UserID.Items.Clear();
                    ListItem noItem = new ListItem();
                    noItem.Text = "無";
                    noItem.Value = "0";

                    changeDDL_UserID.Items.Add(noItem);
                }
                cn.Close();
            }

        }
        else
        {
            changeDDL_UserID.Items.Clear();
            ListItem li_0 = new ListItem();
            li_0.Text = "請選擇";
            li_0.Value = "0";
            changeDDL_UserID.Items.Add(li_0);
            changeDDL_UserID.SelectedValue = "0";

        }
    }

    protected void Change_SelectedIndexChanged(object sender, EventArgs e) //開啟人員異動選擇中的DDL
    {
        ChangeTr1.Attributes["class"] = "tb-bg mb-025r";
        ChangeTr2.Attributes["class"] = "tb-bg mb-025r";
        ChangeTr3.Attributes["class"] = "tb-bg mb-025r";
        if (Change2.Checked)
        {
            date_leave.Enabled = true;
            leaveDDL_group.Enabled = true;
            leaveDDL_UserID.Enabled = true;
            date_change.Enabled = false;
            changeDDL_group.Enabled = false;
            changeDDL_UserID.Enabled = false;
            date_change.Text = "";
            changeDDL_group.SelectedValue = "0";
            changeDDL_UserID.SelectedValue = "0";
            ChangeTr2.Attributes["class"] += " active";
        }
        else if (Change3.Checked)
        {
            date_change.Enabled = true;
            changeDDL_group.Enabled = true;
            changeDDL_UserID.Enabled = true;
            date_leave.Enabled = false;
            leaveDDL_group.Enabled = false;
            leaveDDL_UserID.Enabled = false;
            date_leave.Text = "";
            leaveDDL_group.SelectedValue = "0";
            leaveDDL_UserID.SelectedValue = "0";
            ChangeTr3.Attributes["class"] += " active";
        }
        else
        {
            date_change.Enabled = false;
            date_leave.Enabled = false;
            leaveDDL_group.Enabled = false;
            leaveDDL_UserID.Enabled = false;
            changeDDL_group.Enabled = false;
            changeDDL_UserID.Enabled = false;
            date_change.Text = "";
            changeDDL_group.SelectedValue = "0";
            changeDDL_UserID.SelectedValue = "0";
            date_leave.Text = "";
            leaveDDL_group.SelectedValue = "0";
            leaveDDL_UserID.SelectedValue = "0";
            ChangeTr1.Attributes["class"] += " active";
        }

    }

    protected void JobSetting_SelectedIndexChanged(object sender, EventArgs e) //開啟身分設定選擇中的DDL
    {
        JobSettingTr1.Attributes["class"] = "tb-bg";
        JobSettingTr2.Attributes["class"] = "tb-bg";
        JobSettingTr3.Attributes["class"] = "tb-bg";
        JobSettingTr4.Attributes["class"] = "tb-bg";
        JobSettingTr5.Attributes["class"] = "tb-bg";
        JobSettingTr6.Attributes["class"] = "tb-bg";
        JobSettingTr7.Attributes["class"] = "tb-bg";
        JobSettingTr8.Attributes["class"] = "tb-bg";
        JobSettingTr9.Attributes["class"] = "tb-bg";
        JobSettingTr10.Attributes["class"] = "tb-bg";
        JobSettingTr11.Attributes["class"] = "tb-bg";
        JobSettingTr12.Attributes["class"] = "tb-bg";
        JobSettingDDL1_1.Enabled = false;
        JobSettingDDL1_2.Enabled = false;
        JobSettingDDL2_1.Enabled = false;
        JobSettingDDL2_2.Enabled = false;
        JobSettingDDL3_1.Enabled = false;
        JobSettingDDL3_2.Enabled = false;
        JobSettingDDL4_1.Enabled = false;
        JobSettingDDL4_2.Enabled = false;
        JobSettingDDL5_1.Enabled = false;
        JobSettingDDL5_2.Enabled = false;
        JobSettingDDL6_1.Enabled = false;
        JobSettingDDL6_2.Enabled = false;
        JobSettingDDL7_1.Enabled = false;
        JobSettingDDL7_2.Enabled = false;
        JobSettingDDL8_1.Enabled = false;
        JobSettingDDL8_2.Enabled = false;
        JobSettingDDL9_1.Enabled = false;
        JobSettingDDL9_2.Enabled = false;
        JobSettingDDL10_1.Enabled = false;
        JobSettingDDL10_2.Enabled = false;
        JobSettingDDL11_1.Enabled = false;
        JobSettingDDL11_2.Enabled = false;
        JobSettingDDL12_1.Enabled = false;
        JobSettingDDL12_2.Enabled = false;
        if (JobSetting1.Checked)
        {
            JobSettingDDL1_1.Enabled = true;
            JobSettingDDL1_2.Enabled = true;
            JobSettingTr1.Attributes["class"] += " active";
        }
        else if (JobSetting2.Checked)
        {
            JobSettingDDL2_1.Enabled = true;
            JobSettingDDL2_2.Enabled = true;
            JobSettingTr2.Attributes["class"] += " active";
        }
        else if (JobSetting3.Checked)
        {
            JobSettingDDL3_1.Enabled = true;
            JobSettingDDL3_2.Enabled = true;
            JobSettingTr3.Attributes["class"] += " active";
        }
        else if (JobSetting4.Checked)
        {
            JobSettingDDL4_1.Enabled = true;
            JobSettingDDL4_2.Enabled = true;
            JobSettingTr4.Attributes["class"] += " active";
        }
        else if (JobSetting5.Checked)
        {
            JobSettingDDL5_1.Enabled = true;
            JobSettingDDL5_2.Enabled = true;
            JobSettingTr5.Attributes["class"] += " active";
        }
        else if (JobSetting6.Checked)
        {
            JobSettingDDL6_1.Enabled = true;
            JobSettingDDL6_2.Enabled = true;
            JobSettingTr6.Attributes["class"] += " active";
        }
        else if (JobSetting7.Checked)
        {
            JobSettingDDL7_1.Enabled = true;
            JobSettingDDL7_2.Enabled = true;
            JobSettingTr7.Attributes["class"] += " active";
        }
        else if (JobSetting8.Checked)
        {
            JobSettingDDL8_1.Enabled = true;
            JobSettingDDL8_2.Enabled = true;
            JobSettingTr8.Attributes["class"] += " active";
        }
        else if (JobSetting9.Checked)
        {
            JobSettingDDL9_1.Enabled = true;
            JobSettingDDL9_2.Enabled = true;
            JobSettingTr9.Attributes["class"] += " active";
        }
        else if (JobSetting10.Checked)
        {
            JobSettingDDL10_1.Enabled = true;
            JobSettingDDL10_2.Enabled = true;
            JobSettingTr10.Attributes["class"] += " active";
        }
        else if (JobSetting11.Checked)
        {
            JobSettingDDL11_1.Enabled = true;
            JobSettingDDL11_2.Enabled = true;
            JobSettingTr11.Attributes["class"] += " active";
        }
        else if (JobSetting12.Checked)
        {
            JobSettingDDL12_1.Enabled = true;
            JobSettingDDL12_2.Enabled = true;
            JobSettingTr12.Attributes["class"] += " active";
        }
    }

    protected void RightSetting_SelectedIndexChanged(object sender, EventArgs e) //變更權限設定的底色
    {
        RightSettingTr1.Attributes["class"] = "tb-bg";
        RightSettingTr2.Attributes["class"] = "tb-bg";
        RightSettingTr3.Attributes["class"] = "tb-bg";
        RightSettingTr4.Attributes["class"] = "tb-bg";
        RightSettingTr5.Attributes["class"] = "tb-bg";
        RightSettingTr6.Attributes["class"] = "tb-bg";
        RightSettingTr7.Attributes["class"] = "tb-bg";
        RightSettingTr8.Attributes["class"] = "tb-bg";
        RightSettingTr0.Attributes["class"] = "tb-bg";
        if (RightSetting1.Checked)
            RightSettingTr1.Attributes["class"] += " active";
        else if (RightSetting2.Checked)
            RightSettingTr2.Attributes["class"] += " active";
        else if (RightSetting3.Checked)
            RightSettingTr3.Attributes["class"] += " active";
        else if (RightSetting4.Checked)
            RightSettingTr4.Attributes["class"] += " active";
        else if (RightSetting5.Checked)
            RightSettingTr5.Attributes["class"] += " active";
        else if (RightSetting6.Checked)
            RightSettingTr6.Attributes["class"] += " active";
        else if (RightSetting7.Checked)
            RightSettingTr7.Attributes["class"] += " active";
        else if (RightSetting8.Checked)
            RightSettingTr8.Attributes["class"] += " active";
        else if (RightSetting0.Checked)
            RightSettingTr0.Attributes["class"] += " active";
    }
}