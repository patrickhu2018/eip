using NPOI.HSSF.UserModel;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.HSSF.Util;

using NPOI.XSSF.UserModel;
using System.DirectoryServices;
using System.IdentityModel.Protocols.WSTrust;
using System.Drawing;
using System.Text;

public partial class item_listquery : System.Web.UI.Page
{
    string eip = WebConfigurationManager.ConnectionStrings["eip"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        MasterPage master = (MasterPage)this.Master;
        Label masterLabel = (Label)master.FindControl("page_title");
        masterLabel.Text = "清單查詢";
        Literal link_li = (Literal)master.FindControl("link_li");
        if (!IsPostBack)
        {
            if (Session["apply_group"] != null)
                apply_group.SelectedValue = Session["apply_group"].ToString();

            if (Session["apply_product"] != null)
                apply_product.SelectedValue = Session["apply_product"].ToString();

            if (Session["apply_Buget"] != null)
                apply_Buget.SelectedValue = Session["apply_Buget"].ToString();

            if (Session["apply_state"] != null)
                apply_state.SelectedValue = Session["apply_state"].ToString();

            if (Session["apply_start"] != null)
                apply_start.Text = Session["apply_start"].ToString();

            if (Session["apply_end"] != null)
                apply_end.Text = Session["apply_end"].ToString();

            if (Session["pass_start"] != null)
                pass_start.Text = Session["pass_start"].ToString();

            if (Session["pass_end"] != null)
                pass_end.Text = Session["pass_end"].ToString();


            //用部門名稱 取得部門編號
            using (SqlConnection cn = new SqlConnection(eip))
            {
                string sql_matchpeeID = @"select [no] FROM [group_name]   where [name]=@name ";
                SqlCommand cmd = new SqlCommand(sql_matchpeeID, cn);
                cmd.Parameters.AddWithValue("@name", Session["group_name"].ToString());
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    ViewState["group_no"] = dr["no"].ToString();
                }
                cn.Close();
            }


            ViewState["SortExpression"] = "apply_date";
            ViewState["SortDirection"] = "DESC";
            getddl();
            searchbt_Click(sender, e);

            //gv_BindData();
            rb_lastMonth.Checked = true;


            if (link_li != null) link_li.Text += "<li>清單查詢</li>";
            //if (Session["user_right_id"].ToString() != "1")
            //{
            //    export.Visible = false;
            //}

        }


    }
    private void getddl()
    {
        using (SqlConnection cn = new SqlConnection(eip))
        {
            cn.Open();
            string sql = @"SELECT [no],[name]  FROM [group_name] where parent_id is null order by id ASC";
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    apply_group.Items.Add(new ListItem(dr["name"].ToString(), dr["no"].ToString()));
                }

            }
        }
        using (SqlConnection cn = new SqlConnection(eip))
        {
            cn.Open();
            string sql = @"SELECT [id],[name]  FROM [Item_Product] order by id ASC";
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    apply_product.Items.Add(new ListItem(dr["name"].ToString(), dr["id"].ToString()));
                }

            }
        }
        using (SqlConnection cn = new SqlConnection(eip))
        {
            cn.Open();
            string sql = @"SELECT [id],[name]  FROM [Item_Budget] order by id ASC";
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    apply_Buget.Items.Add(new ListItem(dr["name"].ToString(), dr["id"].ToString()));
                }

            }
        }
    }
    private DataTable showdata(string sortExpression = null, string sortDirection = "ASC")
    {
        using (SqlConnection cn = new SqlConnection(eip))
        {
            cn.Open();
            string sql = @"SELECT t1.[id]
                          ,[apply_group],[apply_user],[apply_date],t2.name　as product_id,[price],[number],[total],[pass_date],
                          t3.name  as　Budget_id,[state],[user_list],[custodian_list],[note]
                          FROM [Item_Product_apply] as t1
                          left join Item_Product as t2 on t2.id=t1.product_id
                          left join Item_Budget as t3 on t3.id=t1.Budget_id
                           left join group_name as t4   on t1.apply_group=t3.name
                          where state=1 and apply_date >= DATEADD(month, -1, GETDATE())";

            if (!string.IsNullOrEmpty(sortExpression))
            {
                sql += @"order by " + sortExpression + " " + sortDirection;
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
    protected DataTable search(string sortExpression = null, string sortDirection = "ASC")
    {
        string sql = "";
        using (SqlConnection cn = new SqlConnection(eip))
        {
            cn.Open();
            sql = @"SELECT t1.[id]
                          ,[apply_group],t4.[id],[apply_user],[apply_date],t2.name　as product_id,[price],[number],[total],[pass_date],
                          t3.name  as　Budget_id,[state],[user_list],[custodian_list],[note],t1.updateDate
                          FROM [Item_Product_apply] as t1
                          left join Item_Product as t2 on t2.id=t1.product_id
                          left join Item_Budget as t3 on t3.id=t1.Budget_id
                          left join group_name as t4 on t4.name=t1.apply_group
                          where 1=1 ";
            if (apply_group.SelectedValue != "0")
            {
                sql += " and t4.no like  '%' + @apply_group + '%'";
            }
            if (apply_product.SelectedValue != "0")
            {
                sql += " and t1.product_id=@product_id";
            }
            if (apply_Buget.SelectedValue != "0")
            {
                sql += " and t1.Budget_id=@Budget_id";
            }
            if (apply_state.SelectedValue != "0")
            {
                sql += " and state=@state";
            }


            if (rb_lastWeek.Checked)
            {
                sql += " and apply_date >= DATEADD(day, -7, GETDATE()) ";
            }
            else if (rb_lastMonth.Checked)
            {
                sql += " and apply_date >= DATEADD(month, -1, GETDATE()) ";
            }
            else if (rb_lastYear.Checked)
            {
                sql += " and apply_date >= DATEADD(year, -1, GETDATE()) ";
            }
            else if (rb_customRange.Checked)
            {
                if (apply_start.Text != "")
                {
                    sql += " and apply_date >= '" + apply_start.Text + "' ";
                }
                if (apply_end.Text != "")
                {
                    sql += " and apply_date <= '" + Convert.ToDateTime(apply_end.Text).AddDays(1).ToString("yyyy-MM-dd") + "'  ";
                }
            }


            if (rb2_lastWeek.Checked)
            {
                sql += " and pass_date >= DATEADD(day, -7, GETDATE()) ";
            }
            else if (rb2_lastMonth.Checked)
            {
                sql += " and pass_date >= DATEADD(month, -1, GETDATE()) ";
            }
            else if (rb2_lastYear.Checked)
            {
                sql += " and pass_date >= DATEADD(year, -1, GETDATE()) ";
            }
            else if (rb2_customRange.Checked)
            {
                if (pass_start.Text != "")
                {
                    sql += " and pass_date >= '" + pass_start.Text + "' ";
                }
                if (pass_end.Text != "")
                {
                    sql += " and pass_date <= '" + Convert.ToDateTime(pass_end.Text).AddDays(1).ToString("yyyy-MM-dd") + "'  ";
                }
            }

            if (keyword.Text != "")
            {
                sql += " and (user_list like '%" + keyword.Text + "%' or custodian_list like '%" + keyword.Text + "%')  ";
            }
            if (!string.IsNullOrEmpty(sortExpression))
            {
                sql += @" order by " + sortExpression + " " + sortDirection;
            }

            Session["p_result"] = sql;
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                if (apply_group.SelectedValue != "0")
                {
                    cmd.Parameters.AddWithValue("@apply_group", apply_group.SelectedValue);
                }
                if (apply_product.SelectedValue != "0")
                {
                    cmd.Parameters.AddWithValue("@product_id", apply_product.SelectedValue);
                }
                if (apply_Buget.SelectedValue != "0")
                {
                    cmd.Parameters.AddWithValue("@Budget_id", apply_Buget.SelectedValue);
                }
                if (apply_state.SelectedValue != "0")
                {
                    cmd.Parameters.AddWithValue("@state", apply_state.SelectedValue);
                }
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
        string sortExpression = ViewState["SortExpression"] as string;
        string sortDirection = ViewState["SortDirection"] as string;


        Session["apply_group"] = apply_group.SelectedValue;
        Session["apply_product"] = apply_product.SelectedValue;
        Session["apply_Buget"] = apply_Buget.SelectedValue;
        Session["apply_state"] = apply_state.SelectedValue;
        Session["apply_start"] = apply_start.Text;
        Session["apply_end"] = apply_end.Text;
        Session["pass_start"] = pass_start.Text;
        Session["pass_end"] = pass_end.Text;





        // 搜尋資料並將結果保存在 ViewState 中
        DataTable dt = search(sortExpression, sortDirection);
        ViewState["SearchResults"] = dt;

        gv.DataSource = dt;
        gv.DataBind();

        lblPageIndex.Text = (gv.PageIndex + 1) + " / " + gv.PageCount;
        UpdatePagerControls();


    }
    private void gv_BindData()
    {
        string sortExpression = ViewState["SortExpression"] as string;
        string sortDirection = ViewState["SortDirection"] as string;
        DataTable dt = ViewState["SearchResults"] as DataTable;

        if (dt != null)
        {
            gv.DataSource = dt;
        }
        else
        {
            // 如果沒有搜尋結果，則顯示所有資料
            gv.DataSource = showdata(sortExpression, sortDirection);
        }

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
        if (e.Row.RowType == DataControlRowType.DataRow)
        {


            string stateValue = DataBinder.Eval(e.Row.DataItem, "state").ToString();
            Label state = (Label)e.Row.FindControl("state");
            if (string.IsNullOrEmpty(stateValue))
            {
                state.Text = "暫存中";
            }
            else
            {
                switch (state.Text)
                {
                    case "1":
                        state.Text = "申請中";
                        break;
                    case "2":
                        state.Text = "已核銷";
                        break;
                    case "3":
                        state.Text = "退件";
                        break;

                }
            }


            ///////////////
            string pass_dateValue = DataBinder.Eval(e.Row.DataItem, "pass_date").ToString();
            Label pass_date = (Label)e.Row.FindControl("pass_date");
            if (string.IsNullOrEmpty(pass_dateValue))
            {
                pass_date.Text = "-";
            }
            string Budget_idValue = DataBinder.Eval(e.Row.DataItem, "Budget_id").ToString();
            Label Budget_id = (Label)e.Row.FindControl("Budget_id");
            if (string.IsNullOrEmpty(pass_dateValue))
            {
                Budget_id.Text = "-";
            }


            //string user_listValue = DataBinder.Eval(e.Row.DataItem, "user_list").ToString();
            //Label user_list = (Label)e.Row.FindControl("user_list");
            //if (string.IsNullOrEmpty(pass_dateValue))
            //{
            //    user_list.Text = "-";
            //}
            //string custodian_listValue = DataBinder.Eval(e.Row.DataItem, "custodian_list").ToString();
            //Label custodian_list = (Label)e.Row.FindControl("custodian_list");
            //if (string.IsNullOrEmpty(pass_dateValue))
            //{
            //    custodian_list.Text = "-";
            //}

            Button export_mag = (Button)e.Row.FindControl("export_mag");
            System.Diagnostics.Debug.WriteLine("GridView 共 " + e.Row.Cells.Count + " 欄位");
            if (Session["user_right_id"] != null && (Session["user_right_id"].ToString() == "1" || Session["user_right_id"].ToString() == "3"))
            {
                // 顯示 "功能" 欄位
                gv.Columns[13].Visible = true; // 最後一個欄位為功能欄位
                gv.Columns[12].Visible = false; // 最後一個欄位為功能欄位
                export_mag.Visible = true;
            }
            else
            {
                // 隱藏 "功能" 欄位
                gv.Columns[13].Visible = false;
            }


        }
    }
    protected void gv_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "check")
        {
            string id = e.CommandArgument.ToString();
            try
            {
                //Button bt = (Button)sender;            
                ViewState["CurrentId"] = id;
                user_gv_BindData(id);
                custodian_gv_BindData(id);
                showUpdateTime(id);
                AsyncPostBackTrigger trigger = new AsyncPostBackTrigger();
                //trigger.ControlID = bt.ID;
                trigger.EventName = "Click";
                UpdatePanel3.Triggers.Add(trigger);
                //UpdatePanel2.Triggers.Add(trigger);               
                ClientScript.RegisterStartupScript(this.GetType(), "showModal", "showusermd();", true);

            }
            catch (Exception ex)
            {
            }
        }
        if (e.CommandName == "modify")
        {
            try
            {
                string[] args = e.CommandArgument.ToString().Split(',');
                string id = args[0];
                string state = args[1];
                string url = "";

                if (args[1] == "1")
                {
                    url = string.Format("item_addapply.aspx?m=1&mode=review&id={0}",
                                                               HttpUtility.UrlEncode(id));
                }
                else if (args[1] == "2")
                {
                    url = string.Format("item_addapply.aspx?m=2&mode=review&id={0}",
                                                               HttpUtility.UrlEncode(id));
                }


                Response.Redirect(url);

            }
            catch (Exception ex)
            {
            }
        }
        if (e.CommandName == "del")
        {
            try
            {
                string id = e.CommandArgument.ToString();
                using (SqlConnection cn = new SqlConnection(eip))
                {
                    cn.Open();
                    string sql = @"delete [Item_Product_apply] where id=@id";
                    using (SqlCommand cmd = new SqlCommand(sql, cn))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.ExecuteNonQuery();
                    }
                }
                searchbt_Click(sender, e);

            }
            catch (Exception ex)
            {
            }
        }

    }
    protected void gv_Sorting(object sender, GridViewSortEventArgs e)
    {
        // 取得目前的排序方向
        string sortDirection = ViewState["SortDirection"] as string;

        // 反轉排序方向
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

        // 確保從 ViewState 中載入已搜尋的資料並排序
        DataTable dt = ViewState["SearchResults"] as DataTable;

        if (dt != null)
        {
            // 重新排序搜尋結果
            DataView dv = dt.DefaultView;
            dv.Sort = e.SortExpression + " " + sortDirection;
            gv.DataSource = dv;
            gv.DataBind();
        }
        else
        {
            // 沒有搜尋過的情況下，重新查詢資料並排序
            gv_BindData();
        }
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
    protected void rb_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rb_Nolimit.Checked)
        {
            //// 禁用 'rb' 部分
            //rb_lastWeek.Enabled = false;
            //rb_lastMonth.Enabled = false;
            //rb_lastYear.Enabled = false;
            //rb_customRange.Enabled = false;
            //rb_Nolimit.Enabled = false;
            //rb_lastWeek.Checked = false;
            //rb_lastMonth.Checked = false;
            //rb_lastYear.Checked = false;
            //rb_customRange.Checked = false;
            //rb_Nolimit.Checked = false;
            //// 啟用 'rb2' 部分
            //rb2_Nolimit.Checked = true;
            //rb2_Nolimit.Enabled = true;
            //rb2_lastWeek.Enabled = true;
            //rb2_lastMonth.Enabled = true;
            //rb2_lastYear.Enabled = true;
            //rb2_customRange.Enabled = true;
        }
        else if (rb2_Nolimit.Checked)
        {
            //禁用 'rb2' 部分
            //rb2_lastWeek.Enabled = false;
            //rb2_lastMonth.Enabled = false;
            //rb2_lastYear.Enabled = false;
            //rb2_customRange.Enabled = false;
            //rb2_Nolimit.Enabled = false;
            //rb2_lastWeek.Checked = false;
            //rb2_lastMonth.Checked = false;
            //rb2_lastYear.Checked = false;
            //rb2_customRange.Checked = false;
            //rb2_Nolimit.Checked = false;
            //啟用 'rb' 部分
            //rb_Nolimit.Checked = true;
            //rb_Nolimit.Enabled = true;
            //rb_lastWeek.Enabled = true;
            //rb_lastMonth.Enabled = true;
            //rb_lastYear.Enabled = true;
            //rb_customRange.Enabled = true;
        }

        if (rb_customRange.Checked)
        {
            apply_start.Enabled = true;
            apply_end.Enabled = true;
        }
        else
        {
            apply_start.Enabled = false;
            apply_end.Enabled = false;
            apply_start.Text = "";
            apply_end.Text = "";
        }
        if (rb2_customRange.Checked)
        {
            pass_start.Enabled = true;
            pass_end.Enabled = true;
        }
        else
        {
            pass_start.Enabled = false;
            pass_end.Enabled = false;
            pass_start.Text = "";
            pass_end.Text = "";
        }
    }

    private string showuserlist(string id)
    {
        string result = "";
        using (SqlConnection cn = new SqlConnection(eip))
        {
            cn.Open();
            string sql = @"SELECT [id], [user_name], [user_quantity] FROM [item_apply_userlist] WHERE apply_id = @apply_id";

            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.AddWithValue("@apply_id", id);
                SqlDataReader dr = cmd.ExecuteReader();
                StringBuilder userNames = new StringBuilder();
                while (dr.Read())
                {
                    if (userNames.Length > 0)
                    {
                        userNames.Append("、");
                    }
                    userNames.Append(dr["user_name"].ToString() + "(" + dr["user_quantity"] + ")");
                }
                result = userNames.ToString();
            }
        }

        return result;
    }

    private string showcustodianlist(string id)
    {
        string result = "";
        using (SqlConnection cn = new SqlConnection(eip))
        {
            cn.Open();
            string sql = @"SELECT [id], [custodian_name], [custodian_quantity] FROM [item_apply_custodianlist] WHERE apply_id = @apply_id";

            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.AddWithValue("@apply_id", id);
                SqlDataReader dr = cmd.ExecuteReader();
                StringBuilder userNames = new StringBuilder();
                while (dr.Read())
                {
                    if (userNames.Length > 0)
                    {
                        userNames.Append("、");
                    }
                    userNames.Append(dr["custodian_name"].ToString() + "(" + dr["custodian_quantity"] + ")");
                }
                result = userNames.ToString();
            }
        }

        return result;
    }

    protected void export_single_Click(object sender, EventArgs e)
    {
        Button bt = (Button)sender;

        HSSFWorkbook workbook = new HSSFWorkbook();
        MemoryStream MemoryStream = new MemoryStream();
        // 新增試算表。 
        HSSFSheet sheet = (HSSFSheet)workbook.CreateSheet("特殊用品登錄總表");
        HSSFFont font = (HSSFFont)workbook.CreateFont();
        font.FontName = "新細明體";
        font.FontHeightInPoints = 12;
        font.Boldweight = (short)FontBoldWeight.BOLD;
        HSSFFont font2 = (HSSFFont)workbook.CreateFont();
        font2.FontName = "新細明體";
        font2.FontHeightInPoints = 12;
        HSSFCellStyle font_style = (HSSFCellStyle)workbook.CreateCellStyle();
        font_style.SetFont(font2);

        sheet.CreateRow(0).CreateCell(0).SetCellValue("特殊用品登錄系統");
        HSSFRow row = (HSSFRow)sheet.GetRow(0);
        HSSFCell cell = (HSSFCell)row.GetCell(0, MissingCellPolicy.CREATE_NULL_AS_BLANK);
        cell.SetCellValue(cell.StringCellValue);
        HSSFCellStyle style = (HSSFCellStyle)workbook.CreateCellStyle();
        style.SetFont(font);
        cell.CellStyle = style;
        sheet.SetColumnWidth(0, 2000);

        sheet.CreateRow(2).CreateCell(0).SetCellValue("申請登錄");
        ((HSSFCell)((HSSFRow)sheet.GetRow(2)).GetCell(0, MissingCellPolicy.CREATE_NULL_AS_BLANK)).CellStyle = style;

        sheet.SetColumnWidth(0, 3500);
        sheet.SetColumnWidth(1, 3500);
        using (SqlConnection cn = new SqlConnection(eip))
        {
            string sql = @"SELECT  apply_date,apply_group,apply_user,t2.name,price,number,total,pass_date,Budget_id,t1.id,t1.id,note FROM [Item_Product_apply] as t1 left join Item_Product as t2 on t1.product_id = t2.id where t1.id=" + bt.CommandArgument;
            SqlCommand cmd = new SqlCommand(sql, cn);
            cn.Open();
            int rowIndex = 1;
            int count = 0;
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                string[] title_arr = { "申請日期：", "申請組室：", "申請人：", "品名：", "單價：", "數量：", "合計：", "採購日期：", "預算科目：", "使用人：", "保管人：", "備註：" };

                for (int i = 3; i <= 9; i++)
                {
                    row = (HSSFRow)sheet.CreateRow(i); // 在這裡創建新行，注意加1以避免覆蓋標題行
                    HSSFRow row3 = (HSSFRow)sheet.GetRow(i);
                    row.CreateCell(0).SetCellValue(title_arr[i - 3]);
                    row.CreateCell(1).SetCellValue(dr[i - 3].ToString());
                    for (int x = 0; x <= 1; x++)
                    {
                        HSSFCell cell3 = (HSSFCell)row3.GetCell(x, MissingCellPolicy.CREATE_NULL_AS_BLANK);
                        cell3.CellStyle = font_style;
                    }
                }

                sheet.CreateRow(11).CreateCell(0).SetCellValue("採購登錄");
                ((HSSFCell)((HSSFRow)sheet.GetRow(11)).GetCell(0, MissingCellPolicy.CREATE_NULL_AS_BLANK)).CellStyle = style;
                for (int i = 12; i <= 16; i++)
                {
                    row = (HSSFRow)sheet.CreateRow(i); // 在這裡創建新行，注意加1以避免覆蓋標題行
                    HSSFRow row3 = (HSSFRow)sheet.GetRow(i);
                    row.CreateCell(0).SetCellValue(title_arr[i - 5]);
                    if (i == 14)
                        row.CreateCell(1).SetCellValue(showuserlist(dr[i - 5].ToString()));
                    else if (i == 15)
                        row.CreateCell(1).SetCellValue(showcustodianlist(dr[i - 5].ToString()));
                    else row.CreateCell(1).SetCellValue(dr[i - 5].ToString());

                    for (int x = 0; x <= 1; x++)
                    {
                        HSSFCell cell3 = (HSSFCell)row3.GetCell(x, MissingCellPolicy.CREATE_NULL_AS_BLANK);
                        cell3.CellStyle = font_style;
                    }
                }
            }
        }

        workbook.Write(MemoryStream);


        // HSSFWorkbook >> .xls 副檔名
        Response.AddHeader("Content-Disposition", string.Format("attachment; filename=特殊用品申請檔案.xls"));
        Response.BinaryWrite(MemoryStream.ToArray());


        workbook = null;
        // 釋放資源
        MemoryStream.Close();
        MemoryStream.Dispose();
    }

    protected void export_Click(object sender, EventArgs e)
    {
        List<string> apply_group = new List<string>();
        List<string> apply_user = new List<string>();
        List<string> apply_date = new List<string>();
        List<string> product_name = new List<string>();
        List<string> number = new List<string>();
        List<string> price = new List<string>();
        List<string> total = new List<string>();
        List<string> pass_date = new List<string>();
        List<string> Budget = new List<string>();
        List<string> updateDate = new List<string>();
        List<string> state = new List<string>();
        List<string> user_list = new List<string>();
        List<string> custodian_list = new List<string>();
        List<string> note = new List<string>();

        HSSFWorkbook workbook = new HSSFWorkbook();
        MemoryStream MemoryStream = new MemoryStream();
        // 新增試算表。 
        HSSFSheet sheet = (HSSFSheet)workbook.CreateSheet("特殊用品登錄總表");
        HSSFFont font = (HSSFFont)workbook.CreateFont();
        font.FontName = "新細明體";
        font.FontHeightInPoints = 12;
        font.Boldweight = (short)FontBoldWeight.BOLD;
        HSSFFont font2 = (HSSFFont)workbook.CreateFont();
        font2.FontName = "新細明體";
        font2.FontHeightInPoints = 12;
        HSSFCellStyle font_style = (HSSFCellStyle)workbook.CreateCellStyle();
        font_style.SetFont(font2);

        sheet.CreateRow(0).CreateCell(0).SetCellValue("組室");
        sheet.GetRow(0).CreateCell(1).SetCellValue("申請人");
        sheet.GetRow(0).CreateCell(2).SetCellValue("申請日期");
        sheet.GetRow(0).CreateCell(3).SetCellValue("品名");
        sheet.GetRow(0).CreateCell(4).SetCellValue("數量");
        sheet.GetRow(0).CreateCell(5).SetCellValue("單價");
        sheet.GetRow(0).CreateCell(6).SetCellValue("合計");
        sheet.GetRow(0).CreateCell(7).SetCellValue("核銷日期");
        sheet.GetRow(0).CreateCell(8).SetCellValue("預算科目");
        sheet.GetRow(0).CreateCell(9).SetCellValue("修改時間");
        sheet.GetRow(0).CreateCell(10).SetCellValue("狀態");
        sheet.GetRow(0).CreateCell(11).SetCellValue("使用人");
        sheet.GetRow(0).CreateCell(12).SetCellValue("保管人");
        sheet.GetRow(0).CreateCell(13).SetCellValue("備註");
        HSSFRow row = (HSSFRow)sheet.GetRow(0);
        for (int i = 0; i <= 14; i++)
        {
            HSSFCell cell = (HSSFCell)row.GetCell(i, MissingCellPolicy.CREATE_NULL_AS_BLANK);
            cell.SetCellValue(cell.StringCellValue);
            HSSFCellStyle style = (HSSFCellStyle)workbook.CreateCellStyle();
            style.SetFont(font);
            cell.CellStyle = style;
            sheet.SetColumnWidth(i, 2500);
        }
        sheet.SetColumnWidth(2, 3500);
        sheet.SetColumnWidth(3, 6500);
        sheet.SetColumnWidth(7, 3500);
        sheet.SetColumnWidth(8, 10000);
        sheet.SetColumnWidth(9, 3500);
        sheet.SetColumnWidth(11, 10000);
        sheet.SetColumnWidth(12, 10000);
        sheet.SetColumnWidth(13, 10000);
        using (SqlConnection cn = new SqlConnection(eip))
        {
            //string sql = @"SELECT t1.[id]
            //              ,[apply_group],[apply_user],[apply_date],t2.name　as product_id,[price],[number],[total],[pass_date],
            //              t3.name  as　Budget_id,[state],[user_list],[custodian_list],t1.updateDate,[note]
            //              FROM [Item_Product_apply] as t1
            //              left join Item_Product as t2 on t2.id=t1.product_id
            //              left join Item_Budget as t3 on t3.id=t1.Budget_id";

            string sql = Session["p_result"].ToString();

            SqlCommand cmd = new SqlCommand(sql, cn);

            if (apply_product.SelectedValue != "0")
            {
                cmd.Parameters.AddWithValue("@product_id", apply_product.SelectedValue);
            }
            if (apply_Buget.SelectedValue != "0")
            {
                cmd.Parameters.AddWithValue("@Budget_id", apply_Buget.SelectedValue);
            }
            if (apply_state.SelectedValue != "0")
            {
                cmd.Parameters.AddWithValue("@state", apply_state.SelectedValue);
            }

            cn.Open();
            int rowIndex = 1;
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                apply_group.Add(dr["apply_group"].ToString());
                apply_user.Add(dr["apply_user"].ToString());

                string applyDateStr = dr["apply_date"].ToString();
                DateTime applyDate = DateTime.Parse(applyDateStr);
                apply_date.Add(applyDate.ToString("yyyy/MM/dd"));

                product_name.Add(dr["product_id"].ToString());
                number.Add(dr["number"].ToString());
                price.Add(dr["price"].ToString());
                total.Add(dr["total"].ToString());

                string passdateStr = dr["pass_date"].ToString();
                try
                {
                    DateTime passdate = DateTime.Parse(passdateStr);
                    pass_date.Add(passdate.ToString("yyyy/MM/dd"));
                }
                catch (FormatException)
                {

                    pass_date.Add("尚未核銷");
                }
                if (string.IsNullOrEmpty(dr["Budget_id"].ToString()))
                {
                    Budget.Add("尚未填寫");
                }
                else
                {
                    Budget.Add(dr["Budget_id"].ToString());
                }

                if (dr["updateDate"].ToString() != "")
                {
                    DateTime update = DateTime.Parse(dr["updateDate"].ToString());
                    updateDate.Add(update.ToString("yyyy/MM/dd"));
                }
                else
                {
                    updateDate.Add("");
                }

                switch (dr["state"].ToString())
                {

                    case "1":
                        state.Add("申請中");
                        break;
                    case "2":
                        state.Add("已核銷");
                        break;
                    case "3":
                        state.Add("退件");
                        break;


                }
                user_list.Add(str_Getuserlist(dr["id"].ToString()));
                note.Add(dr["note"].ToString());

                custodian_list.Add(str_Getcustodianlist(dr["id"].ToString()));
                row = (HSSFRow)sheet.CreateRow(rowIndex); // 在這裡創建新行，注意加1以避免覆蓋標題行

                row.CreateCell(0).SetCellValue(apply_group[apply_group.Count - 1]);
                row.CreateCell(1).SetCellValue(apply_user[apply_user.Count - 1]);
                row.CreateCell(2).SetCellValue(apply_date[apply_date.Count - 1]);
                row.CreateCell(3).SetCellValue(product_name[product_name.Count - 1]);
                row.CreateCell(4).SetCellValue(number[number.Count - 1]);
                row.CreateCell(5).SetCellValue(price[price.Count - 1]);
                row.CreateCell(6).SetCellValue(total[total.Count - 1]);
                row.CreateCell(7).SetCellValue(pass_date[pass_date.Count - 1]);
                row.CreateCell(8).SetCellValue(Budget[Budget.Count - 1]);
                row.CreateCell(9).SetCellValue(updateDate[updateDate.Count - 1]);
                row.CreateCell(10).SetCellValue(state[state.Count - 1]);
                row.CreateCell(11).SetCellValue(user_list[user_list.Count - 1]);
                row.CreateCell(12).SetCellValue(custodian_list[custodian_list.Count - 1]);
                row.CreateCell(13).SetCellValue(note[note.Count - 1]);

                for (int i = 0; i <= 13; i++)
                {
                    sheet.GetRow(0).HeightInPoints = 20;
                    sheet.GetRow(rowIndex).GetCell(i).CellStyle = font_style;
                }
                rowIndex++;
            }
            cn.Close();
        }
        workbook.Write(MemoryStream);


        // HSSFWorkbook >> .xls 副檔名
        Response.AddHeader("Content-Disposition", string.Format("attachment; filename=特殊用品登錄總表.xls"));
        Response.BinaryWrite(MemoryStream.ToArray());


        workbook = null;
        // 釋放資源
        MemoryStream.Close();
        MemoryStream.Dispose();
    }


    private DataTable Getuserlist(string id)
    {

        using (SqlConnection cn = new SqlConnection(eip))
        {
            cn.Open();
            string sql = @"SELECT [id],[user_name],[user_quantity]  FROM [item_apply_userlist] where apply_id=@apply_id";
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.AddWithValue("@apply_id", id);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                lblDataCount3.Text = dt.Rows.Count.ToString();
                return dt;

            }
        }


    }



    private string str_Getuserlist(string id)
    {
        string res = "";
        using (SqlConnection cn = new SqlConnection(eip))
        {
            cn.Open();
            string sql = @"SELECT [id],[user_name],[user_quantity]  FROM [item_apply_userlist] where apply_id=@apply_id";
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.AddWithValue("@apply_id", id);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    res += dr["user_name"].ToString() + ",";
                }
                cn.Close();

            }
        }
        return res;

    }
    private DataTable Getcustodianlist(string id)
    {

        using (SqlConnection cn = new SqlConnection(eip))
        {
            cn.Open();
            string sql = @"SELECT [id],[custodian_name],[custodian_quantity]  FROM [item_apply_custodianlist] where apply_id=@apply_id";
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.AddWithValue("@apply_id", id);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                lblDataCount2.Text = dt.Rows.Count.ToString();
                return dt;

            }
        }

    }

    private string str_Getcustodianlist(string id)
    {
        string res = "";
        using (SqlConnection cn = new SqlConnection(eip))
        {
            cn.Open();
            string sql = @"SELECT [id],[custodian_name],[custodian_quantity]  FROM [item_apply_custodianlist] where apply_id=@apply_id";
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.AddWithValue("@apply_id", id);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    res += dr["custodian_name"].ToString() + ",";
                }
                cn.Close();

            }
        }
        return res;

    }


    private void user_gv_BindData(string id)
    {
        // 获取数据并绑定
        user_gv.DataSource = Getuserlist(id); // 传入 id
        user_gv.DataBind();
        //Response.Write(user_gv.DataSource);
        //ScriptManager.RegisterStartupScript(this, this.GetType(), "user_gv", "alert('" + user_gv.DataSource + "');", true);

        //SqlDataSource1.SelectCommand = "SELECT [id],[user_name],[user_quantity]  FROM [item_apply_userlist] where apply_id=19" ;
        //user_gv.DataBind();

        // 更新分页控件
        lblPageIndex3.Text = (user_gv.PageIndex + 1) + " / " + user_gv.PageCount;
        UpdatePagerControls3(); // 更新分页控件
    }
    protected void user_gv_RowCommand(object sender, GridViewCommandEventArgs e)
    {

    }
    protected void user_gv_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        // 更新當前頁索引
        user_gv.PageIndex = e.NewPageIndex;
        string id = ViewState["CurrentId"] as string;
        // 重新綁定資料
        user_gv_BindData(id);
    }
    private void UpdatePagerControls3()
    {
        // 更新分頁顯示
        lblPageIndex3.Text = (user_gv.PageIndex + 1) + "/" + user_gv.PageCount;


        // 更新分頁控制按鈕狀態
        lkbPagePrev3.Enabled = user_gv.PageIndex > 0;
        lkbPageNext3.Enabled = user_gv.PageIndex < user_gv.PageCount - 1;
    }
    protected void lkbPagePrev3_Click(object sender, EventArgs e)
    {
        string id = ViewState["CurrentId"] as string;
        if (user_gv.PageIndex > 0)
        {
            user_gv.PageIndex--;
            user_gv_BindData(id);
        }
    }
    protected void lkbPageNext3_Click(object sender, EventArgs e)
    {
        string id = ViewState["CurrentId"] as string;
        if (user_gv.PageIndex < user_gv.PageCount - 1)
        {
            user_gv.PageIndex++;
            user_gv_BindData(id);
        }

    }
    private void custodian_gv_BindData(string id)
    {
        custodian_gv.DataSource = Getcustodianlist(id); // 传入 id
        custodian_gv.DataBind();
        lblPageIndex2.Text = (custodian_gv.PageIndex + 1) + " / " + custodian_gv.PageCount;
        UpdatePagerControls2();

    }
    protected void custodian_gv_RowCommand(object sender, GridViewCommandEventArgs e)
    {

    }
    protected void custodian_gv_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        string id = ViewState["CurrentId"] as string;
        // 更新當前頁索引
        custodian_gv.PageIndex = e.NewPageIndex;

        // 重新綁定資料
        custodian_gv_BindData(id);
    }
    private void UpdatePagerControls2()
    {
        string id = ViewState["CurrentId"] as string;
        // 更新分頁顯示
        lblPageIndex2.Text = (custodian_gv.PageIndex + 1) + "/" + custodian_gv.PageCount;


        // 更新分頁控制按鈕狀態
        lkbPagePrev2.Enabled = custodian_gv.PageIndex > 0;
        lkbPageNext2.Enabled = custodian_gv.PageIndex < custodian_gv.PageCount - 1;
    }

    protected void lkbPagePrev2_Click(object sender, EventArgs e)
    {
        string id = ViewState["CurrentId"] as string;
        if (custodian_gv.PageIndex > 0)
        {
            custodian_gv.PageIndex--;
            custodian_gv_BindData(id);
        }
    }


    protected void lkbPageNext2_Click(object sender, EventArgs e)
    {
        string id = ViewState["CurrentId"] as string;
        if (custodian_gv.PageIndex < custodian_gv.PageCount - 1)
        {
            custodian_gv.PageIndex++;
            custodian_gv_BindData(id);
        }

    }

    protected void showUpdateTime(string id) //取得更新日期
    {       
        using (SqlConnection cn = new SqlConnection(eip))
        {
            cn.Open();
            string sql = @"SELECT updateDate  FROM [Item_Product_apply] where id=@id";
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.AddWithValue("@id", id);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    if (dr["updateDate"].ToString() != "")
                    {
                        DateTime update = DateTime.Parse(dr["updateDate"].ToString());
                        lbl_update_time.Text = update.ToString("yyyy/MM/dd HH:mm:ss");
                    }
                    
                }
                cn.Close();

            }
        }
    }
}