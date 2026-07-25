
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
public partial class Repair_inventory_list : System.Web.UI.Page
{
    string eip = WebConfigurationManager.ConnectionStrings["eip"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        MasterPage2 master = (MasterPage2)this.Master;
        Label masterLabel = (Label)master.FindControl("page_title");
        masterLabel.Text = "物料管理-領用清單";
        Literal link_li = (Literal)master.FindControl("link_li");
     
        if (!IsPostBack)
        {
            if (link_li != null) link_li.Text += "<li>物料管理-領用清單</li>";
            ViewState["SortExpression"] = "receivedate";
            ViewState["SortDirection"] = "DESC";
            getddl();
            gv_BindData();

        }
    }
    private void getddl()
    {
        materials.Items.Insert(0, new ListItem("全部", "0"));
        using (SqlConnection cn = new SqlConnection(eip))
        {
            cn.Open();
            string sql = @"SELECT [id]
                          ,[materials_no]
                          ,[materials_name]
                          ,[specification]
                          ,[filepath1]
                          ,[filepath2]
                          ,[safe_inventory]
                          ,[place1_inventory]
                          ,[place2_inventory]
                          ,[total_inventory]
                          ,[updateDate]
                      FROM [repair_materials]";
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    materials.Items.Add(new ListItem(dr["materials_no"].ToString() + "/" + dr["materials_name"].ToString(), dr["materials_no"].ToString()));
                }

            }
        }

        place.Items.Insert(0, new ListItem("全部", "0"));
        using (SqlConnection cn = new SqlConnection(eip))
        {
            cn.Open();
            string sql = @"SELECT [place_id],[place_name]
                      FROM [repair_place]";
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    place.Items.Add(new ListItem(dr["place_name"].ToString(), dr["place_id"].ToString()));
                }

            }
        }
    }
    private DataTable showdata(string sortExpression = null, string sortDirection = "ASC")
    {
        using (SqlConnection cn = new SqlConnection(eip))
        {
            cn.Open();
            string sql = @"SELECT [id]
                          ,[repair_no]
                          ,[receivedate]
                          ,[materials_no]
                          ,[materials_name]
                          ,[repair_place]
                          ,[number]
                      FROM [repair_materials_log]";

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
            sql = @"SELECT [id]
                          ,[repair_no]
                          ,[receivedate]
                          ,[materials_no]
                          ,[materials_name]
                          ,[repair_place]
                          ,[number]
                      FROM [repair_materials_log] where 1=1";

            if (materials.SelectedValue != "0")
            {
                sql += @" and materials_no=@materials_no";
            }
            if (place.SelectedValue != "0")
            {
                sql += @" and repair_place=@repair_place";
            }
            if (rb_lastWeek.Checked)
            {
                sql += " and receivedate >= DATEADD(day, -7, GETDATE()) ";
            }
            else if (rb_lastMonth.Checked)
            {
                sql += " and receivedate >= DATEADD(month, -1, GETDATE()) ";
            }
            else if (rb_lastYear.Checked)
            {
                sql += " and receivedate >= DATEADD(year, -1, GETDATE()) ";
            }
            else if (rb_customRange.Checked)
            {
                if (receive_start.Text != "")
                {
                    sql += " and receivedate >= '" + receive_start.Text + "' ";
                }
                if (receive_end.Text != "")
                {
                    sql += " and receivedate <= '" +receive_end.Text +"'  ";
                }
            }
            if (!string.IsNullOrEmpty(sortExpression))
            {
                sql += @" ORDER BY " + sortExpression + " " + sortDirection;
            }
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                if (materials.SelectedValue != "0")
                {
                    cmd.Parameters.AddWithValue("@materials_no", materials.SelectedValue);
                }
                if (place.SelectedValue != "0")
                {
                    cmd.Parameters.AddWithValue("@repair_place", place.SelectedValue);
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
            Label place = (Label)e.Row.FindControl("repair_place");
            switch (place.Text)
            {
                case "1":
                    place.Text = "行政大樓";
                    break;
                case "2":
                    place.Text = "工商大樓";
                    break;

            }

        }
    }
    protected void gv_RowCreated(object sender, GridViewRowEventArgs e)
    {

    }
    protected void gv_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "check")
        {


            Button btn = (Button)e.CommandSource;
            GridViewRow row = (GridViewRow)btn.NamingContainer;
            string repair_no = ((Label)row.FindControl("repair_no")).Text;
            string id = "";
            string state = "";
            using (SqlConnection cn = new SqlConnection(eip))
            {
                cn.Open();
                string sql = @"SELECT t2.[id]
                              ,t1.[repair_no]
                              ,[receivedate]
                              ,t1.[materials_no]
                              ,t1.[materials_name]
                              ,[repair_place]
                              ,[number]
                              ,t2.state
                          FROM [repair_materials_log] as t1
                          left join repair_apply as t2 on t1.repair_no=t2.repair_no
                          where t1.repair_no=@repair_no";
                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@repair_no", repair_no);  
                    SqlDataReader dr=cmd.ExecuteReader();
                    if (dr.Read()){
                        id = dr["id"].ToString();
                        state = dr["state"].ToString();
                    }
                }
            }
            string url = string.Format("Repair_addapply.aspx?m=1&inventory=1&id={0}&r_no={1}&state={2}",
                                       HttpUtility.UrlEncode(id), HttpUtility.UrlEncode(repair_no), HttpUtility.UrlEncode(state));

            Response.Redirect(url);
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
            receive_start.Enabled = true;
            receive_end.Enabled = true;
        }
        else
        {
            receive_start.Enabled = false;
            receive_end.Enabled = false;
            receive_start.Text =string.Empty;
            receive_end.Text = string.Empty;
        }

    }


    protected void export_Click(object sender, EventArgs e)
    {
        List<string> repair_no = new List<string>();
        List<string> receivedate = new List<string>();
        List<string> materials_no = new List<string>();
        List<string> materials_name = new List<string>();
        List<string> repair_place = new List<string>();
        List<string> number = new List<string>();

        HSSFWorkbook workbook = new HSSFWorkbook();
        MemoryStream MemoryStream = new MemoryStream();
        // 新增試算表。 
        HSSFSheet sheet = (HSSFSheet)workbook.CreateSheet("修繕管理物料領用總表");
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
        sheet.GetRow(0).CreateCell(1).SetCellValue("領用日期");
        sheet.GetRow(0).CreateCell(2).SetCellValue("領用物料代碼");
        sheet.GetRow(0).CreateCell(3).SetCellValue("領用物料名稱");
        sheet.GetRow(0).CreateCell(4).SetCellValue("修繕地點");
        sheet.GetRow(0).CreateCell(5).SetCellValue("使用數量");
        HSSFRow row = (HSSFRow)sheet.GetRow(0);
        for (int i = 0; i <= 5; i++)
        {
            HSSFCell cell = (HSSFCell)row.GetCell(i, MissingCellPolicy.CREATE_NULL_AS_BLANK);
            cell.SetCellValue(cell.StringCellValue);
            HSSFCellStyle style = (HSSFCellStyle)workbook.CreateCellStyle();
            style.SetFont(font);
            cell.CellStyle = style;
            sheet.SetColumnWidth(i, 4000);
        }
        sheet.SetColumnWidth(0, 8000);
        using (SqlConnection cn = new SqlConnection(eip))
        {
            string sql = @"SELECT [id]
                          ,[repair_no]
                          ,[receivedate]
                          ,[materials_no]
                          ,[materials_name]
                          ,[repair_place]
	                      ,t2.place_name
                          ,[number]
                      FROM [repair_materials_log] as t1
                      left join repair_place as t2 on t1.repair_place=t2.place_id where 1=1";
            if (materials.SelectedValue != "0")
            {
                sql += @"and materials_no=@materials_no";
            }
            if (place.SelectedValue != "0")
            {
                sql += @"and repair_place=@repair_place";
            }
            if (rb_lastWeek.Checked)
            {
                sql += " and receivedate >= DATEADD(day, -7, GETDATE()) ";
            }
            else if (rb_lastMonth.Checked)
            {
                sql += " and receivedate >= DATEADD(month, -1, GETDATE()) ";
            }
            else if (rb_lastYear.Checked)
            {
                sql += " and receivedate >= DATEADD(year, -1, GETDATE()) ";
            }
            else if (rb_customRange.Checked)
            {
                if (receive_start.Text != "")
                {
                    sql += " and receivedate >= '" + receive_start.Text + "' ";
                }
                if (receive_end.Text != "")
                {
                    sql += " and receivedate <= '" + receive_end.Text + "'  ";
                }
            }
            SqlCommand cmd = new SqlCommand(sql, cn);
            if (materials.SelectedValue != "0")
            {
                cmd.Parameters.AddWithValue("@materials_no", materials.SelectedValue);
            }
            if (place.SelectedValue != "0")
            {
                cmd.Parameters.AddWithValue("@repair_place", place.SelectedValue);
            }
            cn.Open();
            int rowIndex = 1;
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                repair_no.Add(dr["repair_no"].ToString());
                string receive_date = dr["receivedate"].ToString();
                DateTime receiveDate = DateTime.Parse(receive_date);
                receivedate.Add(receiveDate.ToString("yyyy/MM/dd"));
                materials_no.Add(dr["materials_no"].ToString());
                materials_name.Add(dr["materials_name"].ToString());
                repair_place.Add(dr["place_name"].ToString());
                number.Add(dr["number"].ToString());

                row = (HSSFRow)sheet.CreateRow(rowIndex); // 在這裡創建新行，注意加1以避免覆蓋標題行

                row.CreateCell(0).SetCellValue(repair_no[repair_no.Count - 1]);
                row.CreateCell(1).SetCellValue(receivedate[receivedate.Count - 1]);
                row.CreateCell(2).SetCellValue(materials_no[materials_no.Count - 1]);
                row.CreateCell(3).SetCellValue(materials_name[materials_name.Count - 1]);
                row.CreateCell(4).SetCellValue(repair_place[repair_place.Count - 1]);
                row.CreateCell(5).SetCellValue(number[number.Count - 1]);

                for (int i = 0; i <= 5; i++)
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
        Response.AddHeader("Content-Disposition", string.Format("attachment; filename=修繕管理物料領用總表.xls"));
        Response.BinaryWrite(MemoryStream.ToArray());


        workbook = null;
        // 釋放資源
        MemoryStream.Close();
        MemoryStream.Dispose();
    }
}