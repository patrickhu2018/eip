using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Meetingroom_user_setting : System.Web.UI.Page
{
    string eip = WebConfigurationManager.ConnectionStrings["eip"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        MasterPage3 master = (MasterPage3)this.Master;
        Label masterLabel = (Label)master.FindControl("page_title");
        masterLabel.Text = "使用者設定 ";
        Label masterLabel_comment = (Label)master.FindControl("page_title_comment");
        masterLabel_comment.Text = "註：本功能僅提供資訊小組使用，如有問題請洽相關人員";
        Literal link_li = (Literal)master.FindControl("link_li");
        if (!IsPostBack)
        {
            if (link_li != null) link_li.Text += "<li>使用者設定</li>";
            getgroup();
            showTempData();
            d_search();
        }
    }

    private void getgroup()
    {

        add_unit_class.Items.Clear();
        ListItem li_0 = new ListItem();
        li_0.Text = "請選擇";
        li_0.Value = "0";
        add_unit_group.Items.Add(li_0);
        add_unit_class.Items.Add(li_0);

        using (SqlConnection cn = new SqlConnection(eip))
        {
            string sql1 = @"select * from [group_name] where [parent_id] = '' or [parent_id] is null";
            SqlCommand cmd = new SqlCommand(sql1, cn);
            cn.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                ListItem item = new ListItem();
                item.Text = dr["name"].ToString();
                item.Value = dr["id"].ToString();
                add_unit_group.Items.Add(item);
            }
            cn.Close();
        }
        using (SqlConnection cn = new SqlConnection(eip))
        {
            string sql1 = @"select * from [group_name] where [parent_id] = '' or [parent_id] is null";
            SqlCommand cmd = new SqlCommand(sql1, cn);
            cn.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                ListItem item = new ListItem();
                item.Text = dr["name"].ToString();
                item.Value = dr["id"].ToString();

            }
            cn.Close();
        }
    }
    protected void add_unit_group_SelectedIndexChanged(object sender, EventArgs e)
    {

        if (add_unit_group.SelectedValue != "0")
        {
            add_unit_class.Items.Clear();
            ListItem li_0 = new ListItem();
            li_0.Text = "請選擇";
            li_0.Value = "0";
            add_unit_class.Items.Add(li_0);
            bool hasData = false; // 用來檢查是否有資料
            using (SqlConnection cn = new SqlConnection(eip))
            {
                string sql1 = @"select * from [group_name] where [parent_id]='" + add_unit_group.SelectedValue + @"'";
                SqlCommand cmd = new SqlCommand(sql1, cn);
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    ListItem item = new ListItem();
                    item.Text = dr["name"].ToString();
                    item.Value = dr["id"].ToString();
                    add_unit_class.Items.Add(item);
                    hasData = true; // 如果有資料，設置為 true
                }
                if (!hasData)
                {
                    add_unit_class.Items.Clear();
                    ListItem noItem = new ListItem();
                    noItem.Text = "無";
                    noItem.Value = "0";

                    add_unit_class.Items.Add(noItem);
                }
                cn.Close();
            }

        }
        else
        {
            add_unit_class.Items.Clear();
            ListItem li_0 = new ListItem();
            li_0.Text = "請選擇";
            li_0.Value = "0";
            add_unit_class.Items.Add(li_0);
            add_unit_class.SelectedValue = "0";

        }
    }


    protected void searchtb()
    {
        string sql = "";
        using (SqlConnection cn = new SqlConnection(eip))
        {
            cn.Open();
            sql = @"SELECT  [user_id],[sn],t1.[name],t3.name as gid,t2.name as user_group ,[job],[user_right_id],[note],[state],meeting_show_page FROM [eip_user] as t1
                    left join group_name as t2 on t1.user_group=t2.id
					LEFT JOIN group_name AS t3  ON t3.id = ISNULL(t2.parent_id, t1.user_group)
                    where 1=1 ";
            SqlDataSource1.SelectCommand = sql;
            SqlDataSource1.SelectParameters.Clear();
            if (add_unit_group.SelectedValue != "0")
            {
                sql += " AND t3.id = @gid";
                SqlDataSource1.SelectParameters.Add("gid", add_unit_group.SelectedValue);
            }
            if (add_unit_class.SelectedValue != "0")
            {
                sql += " AND t1.user_group = @user_group";
                SqlDataSource1.SelectParameters.Add("user_group", add_unit_class.SelectedValue);
            }
            if (rb_status2.Checked)
            {
                sql += " and meeting_show_page=1";
            }
            else if (rb_status3.Checked)
            {
                sql += " and meeting_show_page=2";
            }
            else if (rb_status4.Checked)
            {
                sql += " and meeting_show_page=3";
            }
            else if (rb_status5.Checked)
            {
                sql += " and meeting_show_page=4";
            }
            else if (rb_status6.Checked)
            {
                sql += " and meeting_show_page=5";
            }
            else if (rb_status7.Checked)
            {
                sql += " and meeting_show_page=6";
            }
            else if (rb_status8.Checked)
            {
                sql += " and meeting_show_page=7";
            }
            else if (rb_status9.Checked)
            {
                sql += " and meeting_show_page=8";
            }
            else if (rb_status0.Checked)
            {
                sql += " and meeting_show_page=0";
            }
            if (keyword.Text != "")
            {
                sql += " and (sn like '%" + keyword.Text + "%' or t1.[name] like '%" + keyword.Text + "%' or t1.job like '%" + keyword.Text + "%')  ";
            }
            SqlDataSource1.SelectCommand = sql;
            ViewState["sql"] = sql;
            DataView dt = (DataView)SqlDataSource1.Select(new DataSourceSelectArguments());
            lblDataCount.Text = dt.Table.Rows.Count.ToString();
            gv.DataBind();

        }

    }
    protected void d_search()
    {
        tempData();

        searchtb();

        lblPageIndex.Text = (gv.PageIndex + 1) + " / " + gv.PageCount;
        UpdatePagerControls();
    }
    protected void searchbt_Click(object sender, EventArgs e)
    {
        d_search();

    }
    private void gv_BindData()
    {
        d_search();
    }
    protected void gv_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        // 更新當前頁索引
        gv.PageIndex = e.NewPageIndex;

        // 重新綁定資料
        gv_BindData();
    }
    protected void gv_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //if (e.Row.RowType == DataControlRowType.DataRow)
        //{
        //    string user_right_idValue = DataBinder.Eval(e.Row.DataItem, "user_right_id").ToString();
        //    Label user_right_id = (Label)e.Row.FindControl("user_right_id");

        //    user_right_id.Text = user_right_idValue == "1" ? "啟用" : "停用";
        //}
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string ShowPage = DataBinder.Eval(e.Row.DataItem, "meeting_show_page").ToString();
            Label state = (Label)e.Row.FindControl("state");

            if (ShowPage == "1")
            {
                state.Text = "系統管理者";
            }
            else if (ShowPage == "2")
            {
                state.Text = "主計業務管理者";
            }
            else if (ShowPage == "3")
            {
                state.Text = "一般業務管理者";
            }
            else if (ShowPage == "4")
            {
                state.Text = "一般使用者";
            }
            else if (ShowPage == "5")
            {
                state.Text = "主計登記桌";
            }
            else if (ShowPage == "6")
            {
                state.Text = "一般登記桌";
            }
            else if (ShowPage == "7")
            {
                state.Text = "審核使用者";
            }
            else if (ShowPage == "8")
            {
                state.Text = "免審核使用者";
            }
            else if (ShowPage == "0")
            {
                state.Text = "停止使用者";
            }
        }
    }
    protected void gv_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "modify")
        {
            try
            {
                string id = e.CommandArgument.ToString();

                string url = string.Format("Meetingroom_user_edit.aspx?m=1&id={0}",
                                           HttpUtility.UrlEncode(id));

                Response.Redirect(url);

            }
            catch (Exception ex)
            {
            }
        }
        //if (e.CommandName == "del")
        //{
        //    try
        //    {
        //        string id = e.CommandArgument.ToString();
        //        hf_del.Value = id;
        //        string script = "showModal1();";
        //        ClientScript.RegisterStartupScript(this.GetType(), "CallShowModa", script, true);



        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}

    }
    protected void delbt_Click(object sender, EventArgs e)
    {
        using (SqlConnection cn = new SqlConnection(eip))
        {
            cn.Open();
            string sql = @"delete [eip_user] 
            where user_id=@user_id ";
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.AddWithValue("user_id", hf_del.Value);
                cmd.ExecuteNonQuery();
                cn.Close();

            }
            gv_BindData();

        }
    }
    protected void gv_Sorting(object sender, GridViewSortEventArgs e)
    {

        d_search();
    }
    private void UpdatePagerControls()
    {
        // 更新分頁顯示
        lblPageIndex.Text = (gv.PageIndex + 1) + "/" + gv.PageCount;

        // 更新頁碼下拉選單
        ddlPageIndex.Items.Clear();
        for (int i = 0; i < gv.PageCount; i++)
        {
            ddlPageIndex.Items.Add(new ListItem((i + 1).ToString(), i.ToString()));
        }
        ddlPageIndex.SelectedValue = gv.PageIndex.ToString();

        // 更新分頁控制按鈕狀態
        lkbPagePrev.Enabled = gv.PageIndex > 0;
        lkbPageNext.Enabled = gv.PageIndex < gv.PageCount - 1;
    }
    protected void ddlPageIndex_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList d = (DropDownList)sender;
        gv.PageIndex = int.Parse(d.SelectedValue);
        d_search();

    }
    protected void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList d = (DropDownList)sender;

        ddlPageSize.SelectedValue = d.SelectedValue;
        gv.PageSize = int.Parse(ddlPageSize.SelectedValue);
        d_search();
    }
    protected void lkbPagePrev_Click(object sender, EventArgs e)
    {
        if (gv.PageIndex > 0)
        {
            gv.PageIndex--;
            d_search();
        }
    }


    protected void lkbPageNext_Click(object sender, EventArgs e)
    {
        if (gv.PageIndex < gv.PageCount - 1)
        {
            gv.PageIndex++;
            d_search();
        }

    }
    protected void tempData()
    {

        //Session["l_temp_group2"] = group.SelectedValue;
        Session["l_temp_keyword2"] = keyword.Text;

        Session["l_temp_rb_status1"] = rb_status1.Checked;
        Session["l_temp_rb_status2"] = rb_status2.Checked;
        Session["l_temp_rb_status3"] = rb_status3.Checked;
        Session["l_temp_rb_status4"] = rb_status4.Checked;
        Session["l_temp_rb_status5"] = rb_status5.Checked;
        Session["l_temp_rb_status6"] = rb_status6.Checked;
        Session["l_temp_rb_status7"] = rb_status7.Checked;
        Session["l_temp_rb_status8"] = rb_status8.Checked;
        Session["l_temp_rb_status9"] = rb_status9.Checked;
        Session["l_temp_rb_status0"] = rb_status0.Checked;

    }

    protected void showTempData()
    {

        //if (Session["l_temp_group2"] != null)
        //{
        //    group.SelectedValue = Session["l_temp_group2"].ToString();
        //}
        if (Session["l_temp_keyword2"] != null)
        {
            keyword.Text = Session["l_temp_keyword2"].ToString();
        }

        if (Session["l_temp_rb_status1"] != null)
        {
            rb_status1.Checked = Convert.ToBoolean(Session["l_temp_rb_status1"].ToString());
        }
        if (Session["l_temp_rb_status2"] != null)
        {
            rb_status2.Checked = Convert.ToBoolean(Session["l_temp_rb_status2"].ToString());
        }
        if (Session["l_temp_rb_status3"] != null)
        {
            rb_status3.Checked = Convert.ToBoolean(Session["l_temp_rb_status3"].ToString());
        }
        if (Session["l_temp_rb_status4"] != null)
        {
            rb_status4.Checked = Convert.ToBoolean(Session["l_temp_rb_status4"].ToString());
        }
        if (Session["l_temp_rb_status5"] != null)
        {
            rb_status5.Checked = Convert.ToBoolean(Session["l_temp_rb_status5"].ToString());
        }
        if (Session["l_temp_rb_status6"] != null)
        {
            rb_status6.Checked = Convert.ToBoolean(Session["l_temp_rb_status6"].ToString());
        }
        if (Session["l_temp_rb_status7"] != null)
        {
            rb_status7.Checked = Convert.ToBoolean(Session["l_temp_rb_status7"].ToString());
        }
        if (Session["l_temp_rb_status8"] != null)
        {
            rb_status8.Checked = Convert.ToBoolean(Session["l_temp_rb_status8"].ToString());
        }
        if (Session["l_temp_rb_status9"] != null)
        {
            rb_status9.Checked = Convert.ToBoolean(Session["l_temp_rb_status9"].ToString());
        }
        if (Session["l_temp_rb_status0"] != null)
        {
            rb_status0.Checked = Convert.ToBoolean(Session["l_temp_rb_status0"].ToString());
        }

    }
    //protected void adduser_Click(object sender, EventArgs e)
    //{
    //    string url = string.Format("Meetingroom_user_edit.aspx?m=2");

    //    Response.Redirect(url);
    //}

    protected void listBtn_Click(object sender, EventArgs e)//人員清單下載
    {
        List<string> Group = new List<string>();
        List<string> Class = new List<string>();
        List<string> User = new List<string>();
        List<string> Account = new List<string>();
        List<string> Job = new List<string>();
        List<string> ManageState = new List<string>();

        HSSFWorkbook workbook = new HSSFWorkbook();
        MemoryStream memoryStream = new MemoryStream();
        DateTime dateTime = DateTime.Now;
        HSSFSheet sheet = (HSSFSheet)workbook.CreateSheet("資料匯出總表");

        HSSFFont font = (HSSFFont)workbook.CreateFont();
        font.FontName = "新細明體";
        font.FontHeightInPoints = 12;
        font.Boldweight = (short)FontBoldWeight.BOLD;

        HSSFCellStyle font_style = (HSSFCellStyle)workbook.CreateCellStyle();
        font_style.SetFont(font);

        // 設定標題
        string[] headers = { "組室", "科室", "使用者", "帳號", "職位", "管理權限狀態" };
        HSSFRow headerRow = (HSSFRow)sheet.CreateRow(0);
        for (int i = 0; i < headers.Length; i++)
        {
            HSSFCell cell = (HSSFCell)headerRow.CreateCell(i);
            cell.SetCellValue(headers[i]);
            cell.CellStyle = font_style;
            sheet.SetColumnWidth(i, 3000);
        }
        //sheet.SetColumnWidth(0, 5000);
        //sheet.SetColumnWidth(2, 7000);
        //sheet.SetColumnWidth(3, 4000);
        sheet.SetColumnWidth(5, 4000);

        using (SqlConnection cn = new SqlConnection(eip))
        {
            string sql = ViewState["sql"].ToString();
            SqlCommand cmd = new SqlCommand(sql, cn);

            //0905 Mike更新
            if (add_unit_group.SelectedValue != "0")
            {
                cmd.Parameters.AddWithValue("@gid", add_unit_group.SelectedValue);
            }
            if (add_unit_class.SelectedValue != "0")
            {
                cmd.Parameters.AddWithValue("@user_group", add_unit_class.SelectedValue);
            }

            cn.Open();
            int rowIndex = 1;
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                Group.Add(dr["gid"].ToString());
                Class.Add(dr["user_group"].ToString());
                User.Add(dr["name"].ToString());
                Account.Add(dr["sn"].ToString());
                Job.Add(dr["Job"].ToString());
                ManageState.Add(dr["meeting_show_page"].ToString());

                HSSFRow row = (HSSFRow)sheet.CreateRow(rowIndex);

                row.CreateCell(0).SetCellValue(Group.Last());
                row.CreateCell(1).SetCellValue(Class.Last());
                row.CreateCell(2).SetCellValue(User.Last());
                row.CreateCell(3).SetCellValue(Account.Last());
                row.CreateCell(4).SetCellValue(Job.Last());
                //if (ManageState.Last().Contains("1"))
                //    row.CreateCell(5).SetCellValue("系統管理員");
                //else if (ManageState.Last().Contains("2"))
                //    row.CreateCell(5).SetCellValue("業務管理員");
                //else if (ManageState.Last().Contains("-1"))
                //    row.CreateCell(5).SetCellValue("已離職");
                //else
                //    row.CreateCell(5).SetCellValue("一般使用者");
                if (ManageState.Last().Contains("1"))
                {
                    row.CreateCell(5).SetCellValue("系統管理員");
                }
                else if (ManageState.Last().Contains("2"))
                {
                    row.CreateCell(5).SetCellValue("主計業務管理者");
                }
                else if (ManageState.Last().Contains("3"))
                {
                    row.CreateCell(5).SetCellValue("一般業務管理者");
                }
                else if (ManageState.Last().Contains("4"))
                {
                    row.CreateCell(5).SetCellValue("一般使用者");
                }
                else if (ManageState.Last().Contains("5"))
                {
                    row.CreateCell(5).SetCellValue("主計登記桌");
                }
                else if (ManageState.Last().Contains("6"))
                {
                    row.CreateCell(5).SetCellValue("一般登記桌");
                }
                else if (ManageState.Last().Contains("7"))
                {
                    row.CreateCell(5).SetCellValue("審核使用者");
                }
                else if (ManageState.Last().Contains("8"))
                {
                    row.CreateCell(5).SetCellValue("免審核使用者");
                }
                else if (ManageState.Last().Contains("0"))
                {
                    row.CreateCell(5).SetCellValue("停止使用者");
                }

                rowIndex++;
            }
            cn.Close();
        }

        workbook.Write(memoryStream);
        Response.AddHeader("Content-Disposition", "attachment; filename=會議室系統人員清單" + dateTime.ToString("yyyyMMddHHmmss") + ".xls");
        Response.BinaryWrite(memoryStream.ToArray());

        workbook = null;
        memoryStream.Close();
        memoryStream.Dispose();
    }
}