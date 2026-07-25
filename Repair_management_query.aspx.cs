
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
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using System.DirectoryServices;
using System.IdentityModel.Protocols.WSTrust;
using System.Drawing;
using System.Collections;
public partial class Repair_management_query : System.Web.UI.Page
{
    string eip = WebConfigurationManager.ConnectionStrings["eip"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        MasterPage2 master = (MasterPage2)this.Master;
        Label masterLabel = (Label)master.FindControl("page_title");
        masterLabel.Text = "管理查詢";
        Literal link_li = (Literal)master.FindControl("link_li");
        if (!IsPostBack)
        {
            ViewState["SortExpression"] = "apply_date";
            ViewState["SortDirection"] = "DESC";

            apply_place.DataBind();
            apply_place.Items.Insert(0, new ListItem("全部", "0"));
            apply_floor.DataBind();
            apply_floor.Items.Insert(0, new ListItem("全部", "0"));
            if (Session["chk"] != null)
            {
                List<int> storedList = (List<int>)Session["chk"];
                if (storedList.Count > 0)
                {
                    foreach (int item in storedList)
                    {
                        if (item == 0)
                            chk1.Checked = true;
                        if (item == 1)
                            chk2.Checked = true;
                        if (item == 2)
                            chk3.Checked = true;
                    }
                }
                else
                {
                    //List<int> selectedStates = new List<int>();
                    //selectedStates.Add(0);
                    //selectedStates.Add(1);
                    //chk1.Checked = true;
                    //chk2.Checked = true;
                    //Session["chk"] = selectedStates;
                }
            }
            else
            {
                chk1.Checked = true;
                chk2.Checked = true;
            }

            if (Session["rbChecked"] != null)
            {
                if (Session["rbChecked"].ToString() == "0")
                    rb_lastWeek.Checked = true;
                if (Session["rbChecked"].ToString() == "1")
                    rb_lastMonth.Checked = true;
                if (Session["rbChecked"].ToString() == "2")
                    rb_lastYear.Checked = true;
                if (Session["rbChecked"].ToString() == "3")
                {
                    rb_customRange.Checked = true;
                    apply_start.Text = Session["apply_start"].ToString();
                    apply_end.Text = Session["apply_end"].ToString();
                    apply_start.Enabled = true;
                    apply_end.Enabled = true;
                }
            }
            else
            {
                rb_lastMonth.Checked = true;
                Session["rbChecked"] = 1;
            }

            string sortExpression = ViewState["SortExpression"] as string;
            string sortDirection = ViewState["SortDirection"] as string;

            getddl();

            if (Session["apply_place"] != null)
            {
                //Response.Write(Session["apply_place"].ToString());
                apply_place.SelectedValue = Session["apply_place"].ToString();
                getfloor(Session["apply_place"].ToString());
                if (Session["apply_floor"] != null)
                {
                    apply_floor.SelectedValue = Session["apply_floor"].ToString();
                }
            }

            // 搜尋資料並將結果保存在 ViewState 中
            DataTable dt = search(sortExpression, sortDirection);
            ViewState["SearchResults"] = dt;

            gv.DataSource = dt;
            gv.DataBind();
            gv_BindData();

            if (link_li != null) link_li.Text += "<li>管理查詢</li>";

        }
    }
    private void getddl()
    {
        using (SqlConnection cn = new SqlConnection(eip))
        {
            cn.Open();
            string sql = @"SELECT [place_id],[place_name]  FROM [repair_place] order by place_id ASC";
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    apply_place.Items.Add(new ListItem(dr["place_name"].ToString(), dr["place_id"].ToString()));
                }

            }
        }
    }
    private void getfloor(string place)
    {
        apply_floor.Items.Clear();
        apply_floor.Items.Insert(0, new ListItem("全部", "0"));
        using (SqlConnection cn = new SqlConnection(eip))
        {
            cn.Open();
            string sql = @"SELECT [id],[floor_name]  FROM [repair_floor] where place_id=@place_id  order by id ASC";
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.AddWithValue("@place_id", place);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    apply_floor.Items.Add(new ListItem(dr["floor_name"].ToString(), dr["id"].ToString()));
                }

            }
        }
    }
    protected void apply_place_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (apply_place.SelectedValue == "1")
        {
            getfloor("1");
        }
        else if (apply_place.SelectedValue == "2")
        {
            getfloor("2");
        }
        else
        {
            apply_floor.Items.Clear();
            apply_floor.Items.Insert(0, new ListItem("全部", "0"));
        }
    }
    private DataTable showdata(string sortExpression = null, string sortDirection = "ASC")
    {
        using (SqlConnection cn = new SqlConnection(eip))
        {
            cn.Open();
            string sql = @"SELECT t1.[id]
                          ,[repair_no]
                          ,[apply_user]
                          ,[apply_date]
                          ,t1.[place_id],t2.place_name
                          ,[floor_id],t3.floor_name
                          ,[location_id],t4.Location_name
                          ,[apply_reason]
                          ,[state]
                      FROM [repair_apply] as t1
                      left join repair_place as t2 on t1.place_id=t2.place_id
                      left join repair_floor as t3 on t1.floor_id=t3.id
                      left join repair_location as t4 on t1.location_id=t4.id
                      where state=0 ";

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
    protected DataTable search(string sortExpression = null, string sortDirection = "ASC")
    {
        string sql = "";
        using (SqlConnection cn = new SqlConnection(eip))
        {
            cn.Open();
            sql = @"SELECT t1.[id]
                          ,[repair_no]
                          ,[apply_user]
                          ,[apply_date]
                          ,t1.[place_id],t2.place_name
                          ,[floor_id],t3.floor_name
                          ,[location_id],t4.Location_name
                          ,[apply_reason]
                          ,[state]
                      FROM [repair_apply] as t1
                      left join repair_place as t2 on t1.place_id=t2.place_id
                      left join repair_floor as t3 on t1.floor_id=t3.id
                      left join repair_location as t4 on t1.location_id=t4.id
                      where 1=1 ";

            if (apply_place.SelectedValue != "0")
            {
                sql += " and t1.place_id=@place_id";
                Session["apply_place"] = apply_place.SelectedValue;
            }
            else if (apply_place.SelectedValue == "0")
            {
                Session["apply_place"] = "0";
                Session["apply_floor"] = "0";
            }

            if (apply_floor.SelectedValue != "0")
            {
                sql += " and t1.floor_id=@floor_id ";
                Session["apply_floor"] = apply_floor.SelectedValue;
            }

            List<int> selectedStates = new List<int>();

            if (chk1.Checked)
            {
                selectedStates.Add(0);
            }
            if (chk2.Checked)
            {
                selectedStates.Add(1);
            }
            if (chk3.Checked)
            {
                selectedStates.Add(2);
            }
            Session["chk"] = selectedStates;

            if (selectedStates.Count > 0)
            {
                sql += " AND state IN (" + string.Join(",", selectedStates) + ")";
            }
            if ((rb_lastWeek.Checked))
            {
                sql += " and apply_date >= DATEADD(day, -7, GETDATE()) ";
                Session["rbChecked"] = 0;
            }
            else if (rb_lastMonth.Checked)
            {
                sql += " and apply_date >= DATEADD(month, -1, GETDATE()) ";
                Session["rbChecked"] = 1;
            }
            else if (rb_lastYear.Checked)
            {
                sql += " and apply_date >= DATEADD(year, -1, GETDATE()) ";
                Session["rbChecked"] = 2;
            }
            else if (rb_customRange.Checked)
            {
                if (apply_start.Text != "")
                {
                    sql += " and apply_date >= '" + apply_start.Text + "' ";
                    Session["apply_start"] = apply_start.Text;
                }
                if (apply_end.Text != "")
                {
                    sql += " and apply_date <= '" + Convert.ToDateTime(apply_end.Text).AddDays(1).ToString("yyyy-MM-dd") + "'  ";
                    Session["apply_end"] = apply_end.Text;
                }
                Session["rbChecked"] = 3;
            }
            if (!string.IsNullOrEmpty(sortExpression))
            {
                sql += @" ORDER BY " + sortExpression + " " + sortDirection;
            }
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                if (apply_place.SelectedValue != "0")
                {
                    cmd.Parameters.AddWithValue("@place_id", apply_place.SelectedValue);
                }
                if (apply_floor.SelectedValue != "0")
                {
                    cmd.Parameters.AddWithValue("@floor_id", apply_floor.SelectedValue);
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

        // 如果 ViewState 中有搜尋結果，則使用它
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
        gv.PageIndex = e.NewPageIndex;
        gv_BindData();
    }
    protected void gv_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string stateValue = DataBinder.Eval(e.Row.DataItem, "state").ToString();
            Label state = (Label)e.Row.FindControl("state");
            Label stateDisplay = (Label)e.Row.FindControl("stateDisplay");
            Button checkbt = (Button)e.Row.FindControl("check");
            Button modifybt = (Button)e.Row.FindControl("modify");
            Button delbt = (Button)e.Row.FindControl("del");
            switch (state.Text)
            {
                case "0":
                    stateDisplay.Text = "待處理";
                    checkbt.Visible = false;
                    modifybt.Visible = true;

                    break;
                case "1":
                    stateDisplay.Text = "處理中";
                    checkbt.Visible = false;
                    modifybt.Visible = true;
                    break;
                case "2":
                    stateDisplay.Text = "已完成";
                    checkbt.Visible = true;
                    modifybt.Visible = false;
                    break;
                case "3":
                    stateDisplay.Text = "退件";
                    checkbt.Visible = true;
                    modifybt.Visible = false;
                    delbt.Visible = false;
                    break;
            }



        }
    }
    protected void gv_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "check")
        {
            try
            {
                Button btn = (Button)e.CommandSource;
                GridViewRow row = (GridViewRow)btn.NamingContainer;
                string id = e.CommandArgument.ToString();
                string repair_no = ((Label)row.FindControl("repair_no")).Text;
                string state = ((Label)row.FindControl("state")).Text; // 獲取原始數值

                string url = string.Format("Repair_addapply.aspx?m=1&id={0}&r_no={1}&state={2}",
                                           HttpUtility.UrlEncode(id), HttpUtility.UrlEncode(repair_no), HttpUtility.UrlEncode(state));

                Response.Redirect(url);

            }
            catch (Exception ex)
            {
            }
        }
        if (e.CommandName == "modify")
        {
            try
            {
                Button btn = (Button)e.CommandSource;
                GridViewRow row = (GridViewRow)btn.NamingContainer;
                string id = e.CommandArgument.ToString();
                string repair_no = ((Label)row.FindControl("repair_no")).Text;
                string state = ((Label)row.FindControl("state")).Text; // 獲取原始數值

                string url = string.Format("Repair_addapply.aspx?m=1&id={0}&r_no={1}&state={2}",
                                           HttpUtility.UrlEncode(id), HttpUtility.UrlEncode(repair_no), HttpUtility.UrlEncode(state));

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
                Button btn = (Button)e.CommandSource;
                GridViewRow row = (GridViewRow)btn.NamingContainer;
                string id = e.CommandArgument.ToString();
                using (SqlConnection cn = new SqlConnection(eip))
                {
                    cn.Open();
                    string sql = @"update [repair_apply] set state=3 where id=@id";


                    using (SqlCommand cmd = new SqlCommand(sql, cn))
                    {
                        cmd.Parameters.AddWithValue("id", id);
                        cmd.ExecuteNonQuery();

                    }
                }

                gv_BindData();
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
        if (rb_customRange.Checked)
        {
            apply_start.Enabled = true;
            apply_end.Enabled = true;
        }
        else
        {
            apply_start.Enabled = false;
            apply_end.Enabled = false;
            apply_start.Text = string.Empty;
            apply_end.Text = string.Empty;

        }

    }
    protected void addapply_Click(object sender, EventArgs e)
    {
        Response.Redirect("Repair_addapply.aspx?m=0");
    }

    protected void export_Click(object sender, EventArgs e)
    {
        List<string> repair_no = new List<string>();
        List<string> apply_date = new List<string>();
        List<string> apply_user = new List<string>();
        List<string> place_id = new List<string>();
        List<string> floor_id = new List<string>();
        List<string> location_id = new List<string>();
        List<string> apply_reason = new List<string>();
        List<string> state = new List<string>();



        HSSFWorkbook workbook = new HSSFWorkbook();
        MemoryStream MemoryStream = new MemoryStream();
        // 新增試算表。 
        HSSFSheet sheet = (HSSFSheet)workbook.CreateSheet("修繕管理申請總表");
        HSSFFont font = (HSSFFont)workbook.CreateFont();
        font.FontName = "新細明體";
        font.FontHeightInPoints = 12;
        font.Boldweight = (short)FontBoldWeight.BOLD;
        HSSFFont font2 = (HSSFFont)workbook.CreateFont();
        font2.FontName = "新細明體";
        font2.FontHeightInPoints = 12;
        HSSFCellStyle font_style = (HSSFCellStyle)workbook.CreateCellStyle();
        font_style.SetFont(font2);

        sheet.CreateRow(0).CreateCell(0).SetCellValue("修繕單編號");
        sheet.GetRow(0).CreateCell(1).SetCellValue("申請日期");
        sheet.GetRow(0).CreateCell(2).SetCellValue("申請人");
        sheet.GetRow(0).CreateCell(3).SetCellValue("修繕地點");
        sheet.GetRow(0).CreateCell(4).SetCellValue("修繕樓層");
        sheet.GetRow(0).CreateCell(5).SetCellValue("位置");
        sheet.GetRow(0).CreateCell(6).SetCellValue("事由");
        sheet.GetRow(0).CreateCell(7).SetCellValue("處理狀況");
        HSSFRow row = (HSSFRow)sheet.GetRow(0);
        for (int i = 0; i <= 7; i++)
        {
            HSSFCell cell = (HSSFCell)row.GetCell(i, MissingCellPolicy.CREATE_NULL_AS_BLANK);
            cell.SetCellValue(cell.StringCellValue);
            HSSFCellStyle style = (HSSFCellStyle)workbook.CreateCellStyle();
            style.SetFont(font);
            cell.CellStyle = style;
            sheet.SetColumnWidth(i, 4000);
        }
        sheet.SetColumnWidth(0, 8000);
        sheet.SetColumnWidth(6, 8000);
        using (SqlConnection cn = new SqlConnection(eip))
        {
            string sql = @"SELECT t1.[id]
                          ,[repair_no]
                          ,[apply_user]
                          ,[apply_date]
                          ,t1.[place_id],t2.place_name
                          ,[floor_id],t3.floor_name
                          ,[location_id],t4.Location_name
                          ,[apply_reason]
                          ,[state]
                      FROM [repair_apply] as t1
                      left join repair_place as t2 on t1.place_id=t2.place_id
                      left join repair_floor as t3 on t1.floor_id=t3.id
                      left join repair_location as t4 on t1.location_id=t4.id
                      where 1=1";
            if (apply_place.SelectedValue != "0")
            {
                sql += " and t1.place_id=@place_id";
            }

            if (apply_floor.SelectedValue != "0")
            {
                sql += " and t1.floor_id=@floor_id ";
            }

            List<int> selectedStates = new List<int>();

            if (chk1.Checked)
            {
                selectedStates.Add(0);
            }
            if (chk2.Checked)
            {
                selectedStates.Add(1);
            }
            if (chk3.Checked)
            {
                selectedStates.Add(2);
            }

            if (selectedStates.Count > 0)
            {
                sql += " AND state IN (" + string.Join(",", selectedStates) + ")";
            }
            if ((rb_lastWeek.Checked))
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
            SqlCommand cmd = new SqlCommand(sql, cn);
            if (apply_place.SelectedValue != "0")
            {
                cmd.Parameters.AddWithValue("@place_id", apply_place.SelectedValue);
            }
            if (apply_floor.SelectedValue != "0")
            {
                cmd.Parameters.AddWithValue("@floor_id", apply_floor.SelectedValue);
            }
            cn.Open();
            int rowIndex = 1;
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                repair_no.Add(dr["repair_no"].ToString());
                string applyDateStr = dr["apply_date"].ToString();
                DateTime applyDate = DateTime.Parse(applyDateStr);
                apply_date.Add(applyDate.ToString("yyyy/MM/dd"));
                apply_user.Add(dr["apply_user"].ToString());
                place_id.Add(dr["place_name"].ToString());
                floor_id.Add(dr["floor_name"].ToString());
                location_id.Add(dr["Location_name"].ToString());
                apply_reason.Add(dr["apply_reason"].ToString());
                switch (dr["state"].ToString())
                {

                    case "0":
                        state.Add("待處理");
                        break;
                    case "1":
                        state.Add("處理中");
                        break;
                    case "2":
                        state.Add("已完成");
                        break;
                    case "3":
                        state.Add("退件");
                        break;
                }
                row = (HSSFRow)sheet.CreateRow(rowIndex); // 在這裡創建新行，注意加1以避免覆蓋標題行

                row.CreateCell(0).SetCellValue(repair_no[repair_no.Count - 1]);
                row.CreateCell(1).SetCellValue(apply_date[apply_date.Count - 1]);
                row.CreateCell(2).SetCellValue(apply_user[apply_user.Count - 1]);
                row.CreateCell(3).SetCellValue(place_id[place_id.Count - 1]);
                row.CreateCell(4).SetCellValue(floor_id[floor_id.Count - 1]);
                row.CreateCell(5).SetCellValue(location_id[location_id.Count - 1]);
                row.CreateCell(6).SetCellValue(apply_reason[apply_reason.Count - 1]);
                row.CreateCell(7).SetCellValue(state[state.Count - 1]);

                for (int i = 0; i <= 7; i++)
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
        Response.AddHeader("Content-Disposition", string.Format("attachment; filename=修繕管理申請總表.xls"));
        Response.BinaryWrite(MemoryStream.ToArray());


        workbook = null;
        // 釋放資源
        MemoryStream.Close();
        MemoryStream.Dispose();
    }

    protected void chk4_CheckedChanged(object sender, EventArgs e)
    {
        if (chk4.Checked)
        {
            chk1.Checked = true;
            chk2.Checked = true;
            chk3.Checked = true;
        }
        else
        {
            chk1.Checked = false;
            chk2.Checked = false;
            chk3.Checked = false;
        }
    }
}