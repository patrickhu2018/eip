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
public partial class Repair_user_setting : System.Web.UI.Page
{
    string eip = WebConfigurationManager.ConnectionStrings["eip"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        MasterPage2 master = (MasterPage2)this.Master;
        Label masterLabel = (Label)master.FindControl("page_title");
        masterLabel.Text = "使用者設定";
        Literal link_li = (Literal)master.FindControl("link_li");
        if (!IsPostBack)
        {
            if (link_li != null) link_li.Text += "<li>使用者設定</li>";
            getgroup();
            search();
            gv_BindData();
        }
    }

    private DataTable showdata(string sortExpression = null, string sortDirection = "ASC")
    {
        using (SqlConnection cn = new SqlConnection(eip))
        {
            cn.Open();
            //string sql = @"SELECT  [user_id],[sn],t1.[name],t2.name as user_group ,[job],[user_right_id],[note],[state] FROM [eip_user] as t1
            //              left join group_name as t2 on t1.user_group=t2.id where user_right_id=1 ";

            string sql = @"SELECT  [user_id],[sn],t1.[name],t3.name as gid,t2.name as user_group ,[job],[user_right_id],[note],[state],repair_show_page
                    FROM [eip_user] as t1
                    left join group_name as t2 on t1.user_group=t2.id
					LEFT JOIN group_name AS t3  ON t3.id = ISNULL(t2.parent_id, t1.user_group)";            

            if (!string.IsNullOrEmpty(sortExpression))
            {
                sql += @" ORDER BY " + sortExpression + " " + sortDirection;
            }
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                lblDataCount.Text = dt.Rows.Count.ToString();
                return dt;
            }
        }
    }
    private void getgroup()
    {
        add_unit_class.Items.Clear();
        ListItem li_0 = new ListItem();
        li_0.Text = "請選擇";
        li_0.Value = "0";
        group.Items.Add(li_0);
        add_unit_class.Items.Add(li_0);

        using (SqlConnection cn = new SqlConnection(eip))
        {
            cn.Open();
            string sql = @"SELECT [id],[name]  FROM [group_name] where parent_id is null order by id ASC";
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
    protected DataTable search()
    {
        string sql = "";
        using (SqlConnection cn = new SqlConnection(eip))
        {
            cn.Open();
            //sql = @"SELECT  [user_id],[sn],t1.[name],t2.name as user_group ,[job],[user_right_id],[note],[state] FROM [eip_user] as t1
            //        left join group_name as t2 on t1.user_group=t2.id
            //        where 1=1 ";

            sql = @"SELECT  [user_id],[sn],t1.[name],t3.name as gid,t2.name as user_group ,[job],[user_right_id],[note],[state],repair_show_page
                    FROM [eip_user] as t1
                    left join group_name as t2 on t1.user_group=t2.id
					LEFT JOIN group_name AS t3  ON t3.id = ISNULL(t2.parent_id, t1.user_group)
                    where 1=1 ";

            if (group.SelectedValue != "0")
            {
                sql += " AND t3.id ='" + group.SelectedValue + "' ";
            }
            if (add_unit_class.SelectedValue != "0")
            {
                sql += " AND t1.user_group ='" + add_unit_class.SelectedValue + "' ";
            }
            if (rb_status2.Checked)
            {
                sql += " and repair_show_page=1";
            }
            else if (rb_status3.Checked)
            {
                sql += " and repair_show_page=2";
            }
            else if (rb_status4.Checked)
            {
                sql += " and repair_show_page=3";
            }
            else if (rb_status5.Checked)
            {
                sql += " and repair_show_page=4";
            }
            else if (rb_status6.Checked)
            {
                sql += " and repair_show_page=5";
            }
            else if (rb_status7.Checked)
            {
                sql += " and repair_show_page=6";
            }
            else if (rb_status8.Checked)
            {
                sql += " and repair_show_page=7";
            }
            else if (rb_status9.Checked)
            {
                sql += " and repair_show_page=8";
            }
            else if (rb_status0.Checked)
            {
                sql += " and repair_show_page=0";
            }
            if (keyword.Text != "")
            {
                sql += " and (sn like '%" + keyword.Text + "%' or t1.[name] like '%" + keyword.Text + "%' or t1.job like '%" + keyword.Text + "%')  ";
            }

            //if (group.SelectedValue != "0")
            //{
            //    sql += " and t1.user_group=@group";
            //}

            //if (search1_rb_up.Checked)
            //{
            //    sql += " and state=1";
            //}
            //else if (search1_rb_down.Checked)
            //{
            //    sql += " and state=0";
            //}

            //if (keyword.Text != "")
            //{
            //    sql += " and (sn like '%" + keyword.Text + "%' or t1.[name] like '%" + keyword.Text + "%')  ";
            //}

            ViewState["sql"] = sql;
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                if (group.SelectedValue != "0")
                    cmd.Parameters.AddWithValue("@group", group.SelectedValue);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                lblDataCount.Text = dt.Rows.Count.ToString();
                return dt;
            }
        }

    }
    protected void searchbt_Click(object sender, EventArgs e)
    {
        gv.DataSource = search();
        gv.DataBind();
        lblPageIndex.Text = (gv.PageIndex + 1) + " / " + gv.PageCount;
        UpdatePagerControls();

    }
    private void gv_BindData()
    {
        string sortExpression = ViewState["SortExpression"] as string;
        string sortDirection = ViewState["SortDirection"] as string;

        gv.DataSource = showdata(sortExpression, sortDirection);
        gv.DataBind();
        lblPageIndex.Text = (gv.PageIndex + 1) + " / " + gv.PageCount;
        UpdatePagerControls();
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
        //    string stateValue = DataBinder.Eval(e.Row.DataItem, "state").ToString();
        //    Label state = (Label)e.Row.FindControl("state");

        //    state.Text = stateValue == "1" ? "啟用" : "停用";
        //}
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string ShowPage = DataBinder.Eval(e.Row.DataItem, "repair_show_page").ToString();
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

                string url = string.Format("Repair_user_edit.aspx?m=1&id={0}",
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

        string sortDirection = ViewState["SortDirection"] as string;
        if (sortDirection == "ASC")
        {
            sortDirection = "DESC";
        }
        else
        {
            sortDirection = "ASC";
        }

        // 儲存排序的欄位和方向
        ViewState["SortExpression"] = e.SortExpression;
        ViewState["SortDirection"] = sortDirection;
        gv_BindData();
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
        gv_BindData();

    }
    protected void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList d = (DropDownList)sender;

        ddlPageSize.SelectedValue = d.SelectedValue;
        gv.PageSize = int.Parse(ddlPageSize.SelectedValue);
        gv_BindData();
    }
    protected void lkbPagePrev_Click(object sender, EventArgs e)
    {
        if (gv.PageIndex > 0)
        {
            gv.PageIndex--;
            gv_BindData();
        }
    }


    protected void lkbPageNext_Click(object sender, EventArgs e)
    {
        if (gv.PageIndex < gv.PageCount - 1)
        {
            gv.PageIndex++;
            gv_BindData();
        }

    }
    //protected void adduser_Click(object sender, EventArgs e)
    //{
    //    string url = string.Format("Repair_user_edit.aspx?m=2");

    //    Response.Redirect(url);
    //}

    protected void add_unit_group_SelectedIndexChanged(object sender, EventArgs e)
    {

        if (group.SelectedValue != "0")
        {
            add_unit_class.Items.Clear();
            ListItem li_0 = new ListItem();
            li_0.Text = "請選擇";
            li_0.Value = "0";
            add_unit_class.Items.Add(li_0);
            bool hasData = false; // 用來檢查是否有資料
            using (SqlConnection cn = new SqlConnection(eip))
            {
                string sql1 = @"select * from [group_name] where [parent_id]='" + group.SelectedValue + @"'";
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
            cn.Open();
            int rowIndex = 1;
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                Group.Add(dr["gid"].ToString());
                Class.Add(dr["user_group"].ToString());
                User.Add(dr["name"].ToString());
                Account.Add(dr["sn"].ToString());
                Job.Add(dr["job"].ToString());
                ManageState.Add(dr["repair_show_page"].ToString());

                HSSFRow row = (HSSFRow)sheet.CreateRow(rowIndex);
                if (Group.Last().Contains(""))
                    row.CreateCell(0).SetCellValue(Class.Last());
                else
                    row.CreateCell(0).SetCellValue(Group.Last());
                row.CreateCell(1).SetCellValue(Class.Last());
                row.CreateCell(2).SetCellValue(User.Last());
                row.CreateCell(3).SetCellValue(Account.Last());
                row.CreateCell(4).SetCellValue(Job.Last());
                if (ManageState.Last().Contains("0"))
                    row.CreateCell(5).SetCellValue("停用使用者");
                else if (ManageState.Last().Contains("1"))
                    row.CreateCell(5).SetCellValue("系統管理者");
                else if (ManageState.Last().Contains("2"))
                    row.CreateCell(5).SetCellValue("主計業務管理者");
                else if (ManageState.Last().Contains("3"))
                    row.CreateCell(5).SetCellValue("一般業務管理者");
                else if (ManageState.Last().Contains("4"))
                    row.CreateCell(5).SetCellValue("一般使用者");
                else if (ManageState.Last().Contains("5"))
                    row.CreateCell(5).SetCellValue("主計登記桌");
                else if (ManageState.Last().Contains("6"))
                    row.CreateCell(5).SetCellValue("一般登記桌");
                else if (ManageState.Last().Contains("7"))
                    row.CreateCell(5).SetCellValue("審核使用者");
                else if (ManageState.Last().Contains("8"))
                    row.CreateCell(5).SetCellValue("免審核使用者");
                else
                    row.CreateCell(5).SetCellValue("一般使用者");

                rowIndex++;
            }
            cn.Close();
        }

        workbook.Write(memoryStream);
        Response.AddHeader("Content-Disposition", "attachment; filename=修繕系統人員清單" + dateTime.ToString("yyyyMMddHHmmss") + ".xls");
        Response.BinaryWrite(memoryStream.ToArray());

        workbook = null;
        memoryStream.Close();
        memoryStream.Dispose();
    }
}