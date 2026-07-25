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


public partial class Repair_inventory : System.Web.UI.Page
{
    string eip = WebConfigurationManager.ConnectionStrings["eip"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        MasterPage2 master = (MasterPage2)this.Master;
        Label masterLabel = (Label)master.FindControl("page_title");
        masterLabel.Text = "物料管理-庫存";
        Literal link_li = (Literal)master.FindControl("link_li");

        if (!IsPostBack)
        {
            if (link_li != null) link_li.Text += "<li>物料管理-庫存</li>";
            getddl();
            gv_BindData();
        }
    }
    private void getddl()
    {
        materials.Items.Insert(0, new ListItem("請選擇", "0"));
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
                    materials.Items.Add(new ListItem(dr["materials_no"].ToString() + "/" + dr["materials_name"].ToString(), dr["id"].ToString()));
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

            if (!string.IsNullOrEmpty(sortExpression))
            {
                sql += @" ORDER BY " + sortExpression + " " + sortDirection;
            }
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.AddWithValue("@apply_user", Session["user_name"].ToString());
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                lblDataCount.Text = dt.Rows.Count.ToString();
                return dt;
            }
        }
    }
    protected DataTable search()
    {
        string sql = "";
        using (SqlConnection cn = new SqlConnection(eip))
        {
            cn.Open();
            sql = @"SELECT [id]
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

            if (materials.SelectedValue != "0")
            {
                sql += @" where id=@id";
            }


            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                if (materials.SelectedValue != "0")
                {
                    cmd.Parameters.AddWithValue("@id", materials.SelectedValue);
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
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

        }
    }
    protected void gv_RowCreated(object sender, GridViewRowEventArgs e)
    {

    }
    protected void gv_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "ShowImages")
        {
            string id = e.CommandArgument.ToString();
            int rowIndex = gv.DataKeys.Cast<DataKey>().ToList().FindIndex(d => d.Value.ToString() == id);
            if (rowIndex != -1)
            {
                GridViewRow row = gv.Rows[rowIndex];

                // 假設這裡用 id 去獲取圖片路徑
                string filepath1 = ((HiddenField)row.FindControl("file1")).Value.Replace("\\", "/");
                string filepath2 = ((HiddenField)row.FindControl("file2")).Value.Replace("\\", "/");
                string script = "";
                if (filepath1 == "" && filepath2 == "")
                {
                    script = @"
                var modal = $('#imageModal');
                modal.find('#modalImage1').attr('style', 'display:none');
                modal.find('#modalImage2').attr('style', 'display:none');
                modal.find('#modalMessage').attr('style', 'display:block');
                modal.modal('show');";
                }
                else
                {
                    script = @"
                var modal = $('#imageModal');
                modal.find('#modalImage1').attr('src', '" + ResolveUrl(filepath1) + @"');
                modal.find('#modalImage2').attr('src', '" + ResolveUrl(filepath2) + @"');
                modal.find('#modalMessage').attr('style', 'display:none');
                modal.modal('show');";
                }


                ClientScript.RegisterStartupScript(this.GetType(), "ShowModal3", script, true);
            }
        }
        if (e.CommandName == "add")
        {

            addpl.Visible = true;
            Button btn = (Button)e.CommandSource;
            GridViewRow row = (GridViewRow)btn.NamingContainer;
            string id = e.CommandArgument.ToString();
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
                      FROM [repair_materials] where id=@id";


                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        m_no.Text = dr["materials_no"].ToString();
                        m_name.Text = dr["materials_name"].ToString();
                        specification.Text = dr["specification"].ToString();
                        safe_count.Text = !string.IsNullOrEmpty(dr["safe_inventory"].ToString()) ? dr["safe_inventory"].ToString() : "0";
                        p1_count.Text = dr["place1_inventory"].ToString();
                        p2_count.Text = dr["place2_inventory"].ToString();
                        total.Text = !string.IsNullOrEmpty(dr["total_inventory"].ToString()) ? dr["total_inventory"].ToString() : "0"; 

                        if (dr["filepath1"].ToString() == "" && dr["filepath2"].ToString() == "")
                        {
                            //pic1.Attributes["style"] = pic2.Attributes["style"] = "display:none;";
                            //Message.Attributes["style"] = "display:block";
                            pic1.Visible = false;
                            pic2.Visible = false;
                            Message.Visible = true;
                        }
                        else if (dr["filepath1"].ToString() == "")
                        {
                            pic1.Visible = false;
                            Message.Visible = false;
                        }
                        else if (dr["filepath2"].ToString() == "")
                        {
                            pic2.Visible = false;
                            Message.Visible = false;
                        }
                        else
                        {
                            //pic1.Attributes["style"] = pic2.Attributes["style"] = "display:block;";
                            //Message.Attributes["style"] = "display:none";
                            pic1.Visible = true;
                            pic2.Visible = true;
                            Message.Visible = false;
                        }
                        preview1.Src = dr["filepath1"].ToString();
                        preview2.Src = dr["filepath2"].ToString();
                    }
                    cn.Close();

                }
            }

            m_no.Enabled = false;
            m_name.Enabled = false;
            specification.Enabled = false;
            safe_count.Enabled = false;
            p1_count.Enabled = false;
            p2_count.Enabled = false;
            //total.Enabled = false;
            modtitle.InnerText = "新增數量";
            FileUpload1.Visible = FileUpload2.Visible = false;
            upbt1.Visible = upbt2.Visible = false;
            submit.Visible = false;
            update.Visible = true;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenModal", "$('#modal1').modal('show');", true);
        }
        if (e.CommandName == "del")
        {
            Button btn = (Button)e.CommandSource;
            GridViewRow row = (GridViewRow)btn.NamingContainer;
            string id = e.CommandArgument.ToString();
            using (SqlConnection cn = new SqlConnection(eip))
            {
                cn.Open();
                string sql = @"delete [repair_materials] where id=@id";


                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("id", id);
                    cmd.ExecuteReader();
                }
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
    protected void p1_count_TextChanged(object sender, EventArgs e)
    {
        int place1;
        int place2;
        if (!string.IsNullOrWhiteSpace(p1_count.Text))
        {
            int.TryParse(p1_count.Text, out place1);
        }
        else
        {
            place1 = 0;
        }

        if (!string.IsNullOrWhiteSpace(p2_count.Text))
        {
            int.TryParse(p2_count.Text, out place2);
        }
        else
        {
            place2 = 0;
        }

        total.Text = (place1 + place2).ToString();
    }
    protected void submit_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(m_no.Text))
        {
            using (SqlConnection cn = new SqlConnection(eip))
            {
                cn.Open();
                string sql = @"SELECT [id],[materials_no]FROM [repair_materials] where materials_no=@materials_no";
                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("materials_no", m_no.Text);
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    if (count > 0)
                    {
                        alert_text.Text = "此物料代號已存在";
                        ClientScript.RegisterStartupScript(this.GetType(), "show_alert_modal", "show_alert_modal();", true);
                        
                    }
                }
            }
        }
        else
        {
            alert_text.Text = "物料代號未填寫";
            ClientScript.RegisterStartupScript(this.GetType(), "show_alert_modal", "show_alert_modal();", true);
        }
        if (!string.IsNullOrWhiteSpace(m_name.Text))
        {
            using (SqlConnection cn = new SqlConnection(eip))
            {
                cn.Open();
                string sql = @"SELECT [id],[materials_name] FROM [repair_materials] where materials_name=@materials_name";
                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("materials_name", m_name.Text);
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    if (count > 0)
                    {
                        alert_text.Text = "此物料名稱已存在";
                        ClientScript.RegisterStartupScript(this.GetType(), "show_alert_modal", "show_alert_modal();", true);
                      
                    }
                }
            }
        }
        else
        {
            alert_text.Text = "物料名稱未填寫";
            ClientScript.RegisterStartupScript(this.GetType(), "show_alert_modal", "show_alert_modal();", true);
        }
        // 儲存檔案的路徑
        string filePath1 = string.Empty;
        string filePath2 = string.Empty;
        string virtualPath1 = string.Empty;
        string virtualPath2 = string.Empty;
        // 檢查第一個檔案上傳
        if (FileUpload1.HasFile)
        {
            string fileName1 = Path.GetFileName(FileUpload1.FileName);
            string folderPath = Server.MapPath("~/Repairinventor/");
            string fileExtension = Path.GetExtension(fileName1).ToLower();
            if (fileExtension != ".jpg" && fileExtension != ".png")
            {
                alert_text.Text = "只允許上傳 JPG 或 PNG 檔案。";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "", "$('#modal1').modal('show');$('#alert_modal').modal('show');", true);
                //ClientScript.RegisterStartupScript(this.GetType(), "show_alert_modal", "show_alert_modal();", true);
                return;
            }
            filePath1 = Path.Combine(folderPath, fileName1);
            FileUpload1.SaveAs(filePath1);
            virtualPath1 = ResolveUrl("~/Repairinventor/" + fileName1);
        }

        // 檢查第二個檔案上傳
        if (FileUpload2.HasFile)
        {
            string fileName2 = Path.GetFileName(FileUpload2.FileName);
            string folderPath = Server.MapPath("~/Repairinventor/");
            string fileExtension = Path.GetExtension(fileName2).ToLower();
            if (fileExtension != ".jpg" && fileExtension != ".png")
            {
                alert_text.Text = "只允許上傳 JPG 或 PNG 檔案。";
                ClientScript.RegisterStartupScript(this.GetType(), "show_alert_modal", "show_alert_modal();", true);
                return;
            }
            filePath2 = Path.Combine(folderPath, fileName2);
            FileUpload2.SaveAs(filePath2);
            virtualPath2 = ResolveUrl("~/Repairinventor/" + fileName2);
        }

        using (SqlConnection cn = new SqlConnection(eip))
        {
            cn.Open();
            string sql = @"INSERT INTO repair_materials (materials_no, materials_name, specification,filepath1,filepath2,safe_inventory,place1_inventory,place2_inventory,total_inventory,updateDate) 
                            VALUES (@materials_no, @materials_name, @specification, @filepath1, @filepath2, @safe_inventory, @place1_inventory, @place2_inventory,@total_inventory,@updateDate)";

            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.AddWithValue("@materials_no", m_no.Text);
                cmd.Parameters.AddWithValue("@materials_name", m_name.Text);
                cmd.Parameters.AddWithValue("@specification", specification.Text);
                cmd.Parameters.AddWithValue("@filepath1", virtualPath1);
                cmd.Parameters.AddWithValue("@filepath2", virtualPath2);
                cmd.Parameters.AddWithValue("@safe_inventory", safe_count.Text);
                cmd.Parameters.AddWithValue("@place1_inventory", p1_count.Text);
                cmd.Parameters.AddWithValue("@place2_inventory", p2_count.Text);
                cmd.Parameters.AddWithValue("@total_inventory", total.Text);
                cmd.Parameters.AddWithValue("@updateDate", DateTime.Now);

                cmd.ExecuteNonQuery();
            }
        }
        Response.Redirect("Repair_inventory.aspx"); // 重定向到成功頁面
    }
    protected void add_Click(object sender, EventArgs e)
    {
        m_no.Text = string.Empty;
        m_name.Text = string.Empty;
        specification.Text = string.Empty;
        safe_count.Text = string.Empty;
        p1_count.Text = string.Empty;
        p2_count.Text = string.Empty;
        total.Text = string.Empty;
        m_no.Enabled = true;
        m_name.Enabled = true;
        specification.Enabled = true;
        safe_count.Enabled = true;
        p1_count.Enabled = true;
        p2_count.Enabled = true;
        total.Enabled = true;
        preview1.Src = preview2.Src = "image/image.png";
        FileUpload1.Visible = FileUpload2.Visible = true;
        upbt1.Visible = upbt2.Visible = true;
        submit.Visible = true;
        addpl.Visible = false;
        update.Visible = false;
        modtitle.InnerText = "新增物料";
        pic1.Visible = true;
        pic2.Visible = true;
        Message.Visible = false;
        ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal1", "$('#modal1').modal('show');", true);
    }


    protected void update_Click(object sender, EventArgs e)
    {
        using (SqlConnection cn = new SqlConnection(eip))
        {
            cn.Open();
            string sql = @"update repair_materials set 
                           [place1_inventory]=@place1_inventory
                          ,[place2_inventory]=@place2_inventory
                          ,[total_inventory]=@total_inventory
                          ,[updateDate]=@updateDate
                      where materials_no=@materials_no and materials_name=@materials_name";


            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                int addcount = int.Parse(addnumber.Text);
                int pl1count = int.Parse(p1_count.Text);
                int pl2count = int.Parse(p2_count.Text);
                int totalcount = int.Parse(total.Text);
                int savecount = int.Parse(safe_count.Text);

                total.Text = (totalcount + addcount).ToString();
                p1_count.Text = (addcount + pl1count).ToString();

                if ((totalcount + addcount) < savecount)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('總庫存不得小於安全庫存');", true);
                    return;
                }

                cmd.Parameters.AddWithValue("@materials_no", m_no.Text);
                cmd.Parameters.AddWithValue("@materials_name", m_name.Text);
                cmd.Parameters.AddWithValue("@place1_inventory", p1_count.Text);
                cmd.Parameters.AddWithValue("@place2_inventory", p2_count.Text);
                cmd.Parameters.AddWithValue("@total_inventory", total.Text);
                cmd.Parameters.AddWithValue("@updateDate", DateTime.Today);
                cmd.ExecuteNonQuery();
                cn.Close();

            }
            addnumber.Text = "0";
        }
        gv_BindData();
    }

    protected void export_Click(object sender, EventArgs e)
    {
        List<string> materials_no = new List<string>();
        List<string> materials_name = new List<string>();
        List<string> specification = new List<string>();
        List<string> safe_inventory = new List<string>();
        List<string> place1_inventory = new List<string>();
        List<string> place2_inventory = new List<string>();
        List<string> total_inventory = new List<string>();
        List<string> updateDate = new List<string>();

        HSSFWorkbook workbook = new HSSFWorkbook();
        MemoryStream MemoryStream = new MemoryStream();
        // 新增試算表。 
        HSSFSheet sheet = (HSSFSheet)workbook.CreateSheet("修繕管理物料庫存總表");
        HSSFFont font = (HSSFFont)workbook.CreateFont();
        font.FontName = "新細明體";
        font.FontHeightInPoints = 12;
        font.Boldweight = (short)FontBoldWeight.BOLD;
        HSSFFont font2 = (HSSFFont)workbook.CreateFont();
        font2.FontName = "新細明體";
        font2.FontHeightInPoints = 12;
        HSSFCellStyle font_style = (HSSFCellStyle)workbook.CreateCellStyle();
        font_style.SetFont(font2);

        sheet.CreateRow(0).CreateCell(0).SetCellValue("物料代碼");
        sheet.GetRow(0).CreateCell(1).SetCellValue("物料名稱");
        sheet.GetRow(0).CreateCell(2).SetCellValue("規格");
        sheet.GetRow(0).CreateCell(3).SetCellValue("總庫存量");
        sheet.GetRow(0).CreateCell(4).SetCellValue("行政大樓");
        sheet.GetRow(0).CreateCell(5).SetCellValue("工商大樓");
        sheet.GetRow(0).CreateCell(6).SetCellValue("安全庫存量");
        sheet.GetRow(0).CreateCell(7).SetCellValue("最後編修時間");

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
        using (SqlConnection cn = new SqlConnection(eip))
        {
            string sql = @"SELECT [id]
                          ,[materials_no]
                          ,[materials_name]
                          ,[specification]
                          ,[safe_inventory]
                          ,[place1_inventory]
                          ,[place2_inventory]
                          ,[total_inventory]
                          ,[updateDate]
                      FROM [repair_materials]";

            SqlCommand cmd = new SqlCommand(sql, cn);

            cn.Open();
            int rowIndex = 1;
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                materials_no.Add(dr["materials_no"].ToString());
                materials_name.Add(dr["materials_name"].ToString());
                specification.Add(dr["specification"].ToString());
                safe_inventory.Add(dr["safe_inventory"].ToString());
                place1_inventory.Add(dr["place1_inventory"].ToString());
                place2_inventory.Add(dr["place2_inventory"].ToString());
                total_inventory.Add(dr["total_inventory"].ToString());
                string update_Date = dr["updateDate"].ToString();
                DateTime updatedate = DateTime.Parse(update_Date);
                updateDate.Add(updatedate.ToString("yyyy/MM/dd"));

                row = (HSSFRow)sheet.CreateRow(rowIndex); // 在這裡創建新行，注意加1以避免覆蓋標題行

                row.CreateCell(0).SetCellValue(materials_no[materials_no.Count - 1]);
                row.CreateCell(1).SetCellValue(materials_name[materials_name.Count - 1]);
                row.CreateCell(2).SetCellValue(specification[specification.Count - 1]);
                row.CreateCell(3).SetCellValue(total_inventory[total_inventory.Count - 1]);
                row.CreateCell(4).SetCellValue(place1_inventory[place1_inventory.Count - 1]);
                row.CreateCell(5).SetCellValue(place2_inventory[place2_inventory.Count - 1]);
                row.CreateCell(6).SetCellValue(safe_inventory[safe_inventory.Count - 1]);
                row.CreateCell(7).SetCellValue(updateDate[updateDate.Count - 1]);

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
        Response.AddHeader("Content-Disposition", string.Format("attachment; filename=修繕管理物料庫存總表.xls"));
        Response.BinaryWrite(MemoryStream.ToArray());


        workbook = null;
        // 釋放資源
        MemoryStream.Close();
        MemoryStream.Dispose();
    }
}