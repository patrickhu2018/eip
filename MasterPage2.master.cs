using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MasterPage2 : System.Web.UI.MasterPage
{
    string eip = WebConfigurationManager.ConnectionStrings["eip"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["user_id"] != null && Session["group_name"] != null && Session["user_name"] != null && Session["user_group"] != null)
        {

            if (!Page.IsPostBack)
            {
                acc_left_href();
                user.Text = Session["login_t"].ToString() + " 歡迎" + Session["user_name"].ToString() + "(" + Session["user_job"].ToString() + ")" + "登入";
            }

        }
        else
        {
            Response.Redirect("login.aspx");
            ScriptManager.RegisterStartupScript(this, this.GetType(), "login_erro", "alert('登入逾時，請重新登入!');location.href='login.aspx';", true);
        }
        if (Session["user_right_id"].ToString() == "1" || Session["user_right_id"].ToString() == "3")
        {
            homepic.HRef = "Repair_management_query.aspx";
            home_link.HRef = "Repair_management_query.aspx";
            home.HRef = "Repair_management_query.aspx";
        }
        else
        {
            homepic.HRef = "Repair_myapply.aspx";
            home_link.HRef = "Repair_myapply.aspx";
            home.HRef = "Repair_myapply.aspx";
        }

    }

    protected void acc_left_href()
    {
        string ShowPage = "";
        using (SqlConnection cn = new SqlConnection(eip))
        {
            string sql = @"select repair_show_page from eip_user where user_id =@user_id";
            SqlCommand cmd = new SqlCommand(sql, cn);
            cmd.Parameters.AddWithValue("@user_id", Session["user_id"].ToString());
            cn.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
                ShowPage = dr[0].ToString();
            cn.Close();
        }
        if (ShowPage != "0")
        {
            if (ShowPage != "1")
            {
                left_link.Text += @"<li class='menuIcon01 menuIcon_smail " + get_active(1) + @"' id='people_01'><a href = 'Repair_myapply.aspx'>我的申請</a></li>
								<li class='menuIcon02 menuIcon_smail mt-2 " + get_active(2) + @"' id='people_02'><a href ='Repair_listquery.aspx' > 修繕申請查詢 </a></li>";
            }
            if (ShowPage == "1" || ShowPage == "3")
            {
                left_link.Text += @"<li class='menuIcon03 menuIcon_smail mt-2 " + get_active(3) + @"' id='people_03'><a href = 'Repair_management_query.aspx' > 管理查詢 </a></li>";
                left_link.Text += @"<li class='menuIcon09 menuIcon_smail mt-2 " + get_active(9) + @"' id='people_09'><a href = 'Repair_inventory.aspx' > 物料管理 </a></li>";
                left_link.Text += @"<li class='menuIcon04 menuIcon_smail mt-2 " + get_active(4) + @"' id='people_04' style='margin-top: -0.5rem !important;'><a href = 'Repair_inventory.aspx'  style='font-size:18px;padding:10px 30px 10px 60px;' > 庫存 </a></li>";
                left_link.Text += @"<li class='menuIcon05 menuIcon_smail mt-2 " + get_active(5) + @"' id='people_05' style='margin-top: -0.5rem !important;'><a href = 'Repair_inventory_list.aspx'  style='font-size:18px;padding:10px 30px 10px 60px;'  > 領用清單 </a></li>";
                left_link.Text += @"<li class='menuIcon06 menuIcon_smail mt-2 " + get_active(6) + @"' id='people_06'><a href = 'Repair_management.aspx' > 位置管理 </a></li>";
            }

            //left_link.Text += @"<li class='menuIcon08 menuIcon_smail mt-2 " + get_active(8) + @"' id='people_08'><a href = 'Repair_user_edit.aspx?m=0' > 使用者資訊 </a></li>";
            if (ShowPage == "1")
            {
                left_link.Text += @"<li class='menuIcon07 menuIcon_smail mt-2 " + get_active(7) + @"' id='people_07'><a href = 'Repair_user_setting.aspx' > 使用者設定 </a></li>";
            }
        }


    }
    protected string get_active(int icon)
    {
        string str = "";
        if ((icon == 1 && System.IO.Path.GetFileName(Request.PhysicalPath) == "Repair_myapply.aspx")
        || (icon == 2 && System.IO.Path.GetFileName(Request.PhysicalPath) == "Repair_listquery.aspx")
        || (icon == 3 && System.IO.Path.GetFileName(Request.PhysicalPath) == "Repair_management_query.aspx")
        || (icon == 4 && System.IO.Path.GetFileName(Request.PhysicalPath) == "Repair_inventory.aspx")
        || (icon == 5 && System.IO.Path.GetFileName(Request.PhysicalPath) == "Repair_inventory_list.aspx")
        || (icon == 6 && System.IO.Path.GetFileName(Request.PhysicalPath) == "Repair_management.aspx")
        || (icon == 7 && System.IO.Path.GetFileName(Request.PhysicalPath) == "Repair_user_setting.aspx")
        /*|| (icon == 8 && System.IO.Path.GetFileName(Request.PhysicalPath) == "Repair_user_edit.aspx")*/)
            str = "active";

        if (icon == 9 && (System.IO.Path.GetFileName(Request.PhysicalPath) == "Repair_inventory.aspx" || System.IO.Path.GetFileName(Request.PhysicalPath) == "Repair_inventory_list.aspx"))
        {
            str = "active";
        }
        return str;
    }

    #region 切換多元身分
    protected void Change_role_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "ShowModal_check", "$('#exampleModalCenter_role').modal('show');", true);
    }

    protected void apply_group_SelectedIndexChanged(object sender, EventArgs e)//科室選項
    {
        sds_division.SelectCommand = "select '0' as id, '全部' as name union select id, name from group_name where parent_id is not null and parent_id='" + apply_group.SelectedValue + "'";
        ddl_division.DataBind();

        ddl_division_SelectedIndexChanged(null, null);//申請人選項
    }

    protected void ddl_division_SelectedIndexChanged(object sender, EventArgs e)//申請人選項
    {
        sds_applicant.SelectCommand = "select '0' as user_id, '全選' as name union select user_id,t1.name from eip_user t1 left join group_name t2 on t1.user_group = t2.id " +
                    "where t1.Name not like '%方達科技%' and t1.Name not like '%信箱%' and t1.Name not like '%通報%' and t1.Name " +
                    "not like '%同仁%' and t1.Name not like '%保全%' and t1.Name not like '%中科實中%' and t1.Name not like '%中科政風室%' and LeaveDate is null ";

        if (ddl_division.SelectedValue != "0")
        {
            sds_applicant.SelectCommand += "and t1.user_group ='" + ddl_division.SelectedValue + "'";
        }
        else if (apply_group.SelectedValue != "0")
        {
            sds_applicant.SelectCommand += "and t1.user_group ='" + apply_group.SelectedValue + "' or t2.parent_id='" + apply_group.SelectedValue + "'";
        }

        ddl_applicant.DataSourceID = "sds_applicant";
        ddl_applicant.DataBind();
    }

    protected void Change_role_comfirm(object sender, EventArgs e) /*切換使用者MODAL-確認*/
    {
        if (ddl_applicant.SelectedValue != "0") change_role_session();
        else ScriptManager.RegisterStartupScript(this, GetType(), "role_alert", "alert('未選擇正確的使用者');", true);
    }

    protected void change_role_session()/*切換為選取之使用者Session，並重新導向*/
    {
        using (SqlConnection cn = new SqlConnection(eip))
        {
            cn.Open();
            string sql = @"select user_id,t1.name,repair_user_right_id,user_group,t2.name as group_name,t1.job,HomePage from [eip_user] as t1
                                      left join group_name as t2 on t2.id=t1.user_group
                                      left join repair_UserRightSetting on t1.repair_user_right_id = repair_UserRightSetting.user_right_id
                                      where user_id=@user_id";

            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.AddWithValue("@user_id", ddl_applicant.SelectedValue);
                Response.Write(sql);
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

    protected string getgroupname(string user_group)
    {
        string name = "";
        using (SqlConnection cn = new SqlConnection(eip))
        {
            cn.Open();
            string sql = @"SELECT  [id],[name],[parent_id]  FROM [group_name] where id=@user_group";
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.AddWithValue("@user_group", user_group);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    name = dr["name"].ToString();
                }
            }
        }
        return name;
    }
    #endregion
}
