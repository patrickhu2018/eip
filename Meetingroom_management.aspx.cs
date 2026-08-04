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
using System.Security.Cryptography;

public partial class Meetingroom_management : System.Web.UI.Page
{
    string eip = WebConfigurationManager.ConnectionStrings["eip"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        MasterPage3 master = (MasterPage3)this.Master;
        Label masterLabel = (Label)master.FindControl("page_title");
        masterLabel.Text = "會議室設備詳情";
        Literal link_li = (Literal)master.FindControl("link_li");

        if (!IsPostBack)
        {
            if (link_li != null) link_li.Text += "<li>會議室設備詳情</li>";
            if (Session["user_right_id"] != null && Session["user_right_id"].ToString() != "1" && Session["user_right_id"].ToString() != "2" && Session["user_right_id"].ToString() != "3")
            {
                add.Visible = false;
            }
            SetActiveTab("tab1");
            show_meetname();
            show_host();
            gv_BindData();
        }
        else
        {
            if (ViewState["tab"] != null)
            {
                SetActiveTab(ViewState["tab"].ToString());
            }
            show_meetname();
            show_host();
        }
    }
    private void SetActiveTab(string tabId)
    {
        if (tabId == "tab1")
        {
            tab1.CssClass = "nav-link navactive";  // Active tab class
            tab2.CssClass = "nav-link";            // Inactive tab class
        }
        else
        {
            tab1.CssClass = "nav-link";            // Inactive tab class
            tab2.CssClass = "nav-link navactive";  // Active tab class
        }

        if (tabId == "tab1")
        {
            meetpl.Visible = true;
            setpl.Visible = false;
        }
        else
        {
            meetpl.Visible = false;
            setpl.Visible = true;
        }
    }
    protected void TabButton_Click(object sender, EventArgs e)
    {
        var button = (Button)sender;
        if (button.ID == "tab1")
        {
            SetActiveTab("tab1");
            ViewState["tab"] = "tab1";
        }
        if (button.ID == "tab2")
        {
            SetActiveTab("tab2");
            ViewState["tab"] = "tab2";
        }
    }
    protected void add_name_Click(object sender, EventArgs e)
    {
        string meet_name = meet_name_txt.Text;
        string currentFavoriteName = string.Empty;
        List<string> namelist = new List<string>();
        using (SqlConnection cn = new SqlConnection(eip))
        {
            cn.Open();
            string sql = @"SELECT [favorite_name]  FROM [eip_user] where user_id=@user_id ";
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.AddWithValue("@user_id", Session["user_id"]);
                var result = cmd.ExecuteScalar();  // 取得單一結果
                if (result != DBNull.Value)
                {
                    currentFavoriteName = result.ToString();  // 取得原來的 favorite_name
                }
            }
        }
        if (!string.IsNullOrEmpty(currentFavoriteName))
        {
            namelist = currentFavoriteName.Split(',').ToList();  // 將已有名稱按逗號分割
        }
        if (namelist.Contains(meet_name))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('已有相同會議名稱請重新輸入');", true);
            return;
        }
        if (namelist.Count >= 3)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('會議名稱最多只能三筆請先移除已建立名稱再做新增');", true);
            return;
        }
        namelist.Add(meet_name);
        string updatedFavoriteName = string.Join(",", namelist);
        using (SqlConnection cn = new SqlConnection(eip))
        {
            cn.Open();
            string sql = @"update [eip_user] set  favorite_name=@favorite_name  where user_id=@user_id ";
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.AddWithValue("@user_id", Session["user_id"]);
                cmd.Parameters.AddWithValue("@favorite_name", updatedFavoriteName);  // 更新 favorite_name 欄位
                cmd.ExecuteNonQuery();  // 執行更新
            }
        }
        meet_name_txt.Text = "";

        show_meetname();
    }
    protected void show_meetname()
    {
        name_pl.Controls.Clear();  // 清除舊的控件
        List<string> namelist = new List<string>();

        // 取得資料庫中的 favorite_name
        using (SqlConnection cn = new SqlConnection(eip))
        {
            cn.Open();
            string sql = @"SELECT [favorite_name] FROM [eip_user] WHERE user_id=@user_id";
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.AddWithValue("@user_id", Session["user_id"]);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    string meet_name = dr["favorite_name"].ToString();
                    if (!string.IsNullOrEmpty(meet_name))
                    {
                        namelist = meet_name.Split(',').ToList();
                    }
                }
            }
        }

        // 動態生成控件並綁定事件
        for (int i = 0; i < namelist.Count; i++)
        {
            Label nameLabel = new Label();
            nameLabel.Text = (i + 1) + "&nbsp;&nbsp;&nbsp;&nbsp;" + namelist[i];
            nameLabel.CssClass = "withebox";
            nameLabel.Width = Unit.Percentage(90);

            Button delbt = new Button();
            delbt.Text = "刪除";
            delbt.CssClass = "gw_bt";
            delbt.ID = "del_" + (i+1);
            delbt.Style["background"] = "#B83F1F";
            delbt.CommandArgument = namelist[i]; // 設置要刪除的會議名稱為 CommandArgument
            delbt.Click += DeleteButton_Click;  // 綁定刪除事件
            AsyncPostBackTrigger trigger = new AsyncPostBackTrigger();
            trigger.ControlID = delbt.ID;  // 綁定控件 ID
            trigger.EventName = "Click";   // 綁定事件名稱
           
            // 使用 LiteralControl 加入布局
            name_pl.Controls.Add(new LiteralControl("<div class=\" pl-3 pt-3 pr-4 d-flex justify-content-between align-items-center\" style=\"gap: 20px;\">"));
            name_pl.Controls.Add(nameLabel);
            name_pl.Controls.Add(delbt);
            name_pl.Controls.Add(new LiteralControl("</div>"));
            UpdatePanel1.Triggers.Add(trigger);  // 將觸發器添加到 UpdatePanel
        }
    }
    protected void DeleteButton_Click(object sender, EventArgs e)
    {
        Button delbt = (Button)sender;
        string meetNameToDelete = delbt.CommandArgument; // 取得要刪除的會議名稱

        // 使用第一個 SQL 查詢，讀取 favorite_name
        string meetNames = string.Empty;
        using (SqlConnection cn = new SqlConnection(eip))
        {
            cn.Open();
            string sql = @"SELECT [favorite_name] FROM [eip_user] WHERE user_id=@user_id";
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.AddWithValue("@user_id", Session["user_id"]);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    meetNames = dr["favorite_name"].ToString();
                }
                dr.Close();
            }
        }

        // 如果讀取到了會議名稱，進行後續處理
        if (!string.IsNullOrEmpty(meetNames))
        {
            List<string> namelist = meetNames.Split(',').ToList();

            // 從列表中移除要刪除的會議名稱
            if (namelist.Contains(meetNameToDelete))
            {
                namelist.Remove(meetNameToDelete);

                // 使用第二個 SQL 查詢更新資料庫中的 favorite_name
                using (SqlConnection cn = new SqlConnection(eip))
                {
                    cn.Open();
                    string updatedNames = string.Join(",", namelist);
                    string updateSql = @"UPDATE [eip_user] SET [favorite_name]=@favorite_name WHERE user_id=@user_id";
                    using (SqlCommand updateCmd = new SqlCommand(updateSql, cn))
                    {
                        updateCmd.Parameters.AddWithValue("@favorite_name", updatedNames);
                        updateCmd.Parameters.AddWithValue("@user_id", Session["user_id"]);
                        updateCmd.ExecuteNonQuery();
                    }
                }
            }
        }

        // 顯示更新後的會議名稱
        show_meetname();
    }
    protected void add_host_Click(object sender, EventArgs e)
    {
        string meet_name = host_txt.Text;
        string currentFavoriteName = string.Empty;
        List<string> namelist = new List<string>();
        using (SqlConnection cn = new SqlConnection(eip))
        {
            cn.Open();
            string sql = @"SELECT [favorite_host]  FROM [eip_user] where user_id=@user_id ";
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.AddWithValue("@user_id", Session["user_id"]);
                var result = cmd.ExecuteScalar();  // 取得單一結果
                if (result != DBNull.Value)
                {
                    currentFavoriteName = result.ToString();  // 取得原來的 favorite_name
                }
            }
        }
        if (!string.IsNullOrEmpty(currentFavoriteName))
        {
            namelist = currentFavoriteName.Split(',').ToList();  // 將已有名稱按逗號分割
        }
        if (namelist.Contains(meet_name))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('已有相同主持人請重新輸入');", true);
            return;
        }
        if (namelist.Count >= 3)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('主持人最多只能三筆請先移除已建立名稱再做新增');", true);
            return;
        }
        namelist.Add(meet_name);
        string updatedFavoriteName = string.Join(",", namelist);
        using (SqlConnection cn = new SqlConnection(eip))
        {
            cn.Open();
            string sql = @"update [eip_user] set  favorite_host=@favorite_host  where user_id=@user_id ";
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.AddWithValue("@user_id", Session["user_id"]);
                cmd.Parameters.AddWithValue("@favorite_host", updatedFavoriteName);  // 更新 favorite_name 欄位
                cmd.ExecuteNonQuery();  // 執行更新
            }
        }
        host_txt.Text = "";

        show_host();
    }
    protected void show_host()
    {
        host_pl.Controls.Clear();  // 清除舊的控件
        List<string> namelist = new List<string>();

        using (SqlConnection cn = new SqlConnection(eip))
        {
            cn.Open();
            string sql = @"SELECT [favorite_host] FROM [eip_user] WHERE user_id=@user_id";
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.AddWithValue("@user_id", Session["user_id"]);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    string meet_name = dr["favorite_host"].ToString();
                    if (!string.IsNullOrEmpty(meet_name))
                    {
                        namelist = meet_name.Split(',').ToList();
                    }
                }
            }
        }

        // 動態生成控件並綁定事件
        for (int i = 0; i < namelist.Count; i++)
        {
            Label nameLabel = new Label();
            nameLabel.Text = (i + 1) + "&nbsp;&nbsp;&nbsp;&nbsp;" + namelist[i];
            nameLabel.CssClass = "withebox";
            nameLabel.Width = Unit.Percentage(90);

            Button delbt = new Button();
            delbt.Text = "刪除";
            delbt.CssClass = "gw_bt";
            delbt.ID = "delhost_" + (i + 1);
            delbt.Style["background"] = "#B83F1F";
            delbt.CommandArgument = namelist[i]; // 設置要刪除的會議名稱為 CommandArgument
            delbt.Click += DeleteButton2_Click;  // 綁定刪除事件
            AsyncPostBackTrigger trigger = new AsyncPostBackTrigger();
            trigger.ControlID = delbt.ID;  // 綁定控件 ID
            trigger.EventName = "Click";   // 綁定事件名稱

            // 使用 LiteralControl 加入布局
            host_pl.Controls.Add(new LiteralControl("<div class=\" pl-3 pt-3 pr-4 d-flex justify-content-between align-items-center\" style=\"gap: 20px;\">"));
            host_pl.Controls.Add(nameLabel);
            host_pl.Controls.Add(delbt);
            host_pl.Controls.Add(new LiteralControl("</div>"));
            UpdatePanel2.Triggers.Add(trigger);  // 將觸發器添加到 UpdatePanel
        }
    }
    protected void DeleteButton2_Click(object sender, EventArgs e)
    {
        Button delbt = (Button)sender;
        string meetNameToDelete = delbt.CommandArgument; // 取得要刪除的會議名稱

        // 使用第一個 SQL 查詢，讀取 favorite_name
        string meetNames = string.Empty;
        using (SqlConnection cn = new SqlConnection(eip))
        {
            cn.Open();
            string sql = @"SELECT [favorite_host] FROM [eip_user] WHERE user_id=@user_id";
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.AddWithValue("@user_id", Session["user_id"]);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    meetNames = dr["favorite_host"].ToString();
                }
                dr.Close();
            }
        }

        // 如果讀取到了會議名稱，進行後續處理
        if (!string.IsNullOrEmpty(meetNames))
        {
            List<string> namelist = meetNames.Split(',').ToList();

            // 從列表中移除要刪除的會議名稱
            if (namelist.Contains(meetNameToDelete))
            {
                namelist.Remove(meetNameToDelete);

                using (SqlConnection cn = new SqlConnection(eip))
                {
                    cn.Open();
                    string updatedNames = string.Join(",", namelist);
                    string updateSql = @"UPDATE [eip_user] SET [favorite_host]=@favorite_host WHERE user_id=@user_id";
                    using (SqlCommand updateCmd = new SqlCommand(updateSql, cn))
                    {
                        updateCmd.Parameters.AddWithValue("@favorite_host", updatedNames);
                        updateCmd.Parameters.AddWithValue("@user_id", Session["user_id"]);
                        updateCmd.ExecuteNonQuery();
                    }
                }
            }
        }

        // 顯示更新後的會議名稱
        show_host();
    }
    private DataTable showdata(string sortExpression = null, string sortDirection = "ASC")
    {
        using (SqlConnection cn = new SqlConnection(eip))
        {
            cn.Open();
            string sql = @"SELECT 
                            t1.id,
                            t1.meet_name,
                            t1.meet_location,
                            t1.number,
                            t1.equipment,
                            t1.other,
                            CASE 
                                WHEN CHARINDEX(CAST(t1.id AS VARCHAR), t2.favorite_meet) > 0 THEN 1
                                ELSE 0
                            END AS is_favorite,t1.color
                        FROM 
                            meeting_equipment AS t1
                        LEFT JOIN 
                            eip_user AS t2
                            ON t2.user_id = @user_id";

            if (!string.IsNullOrEmpty(sortExpression))
            {
                sql += @" ORDER BY " + sortExpression + " " + sortDirection;
            }
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.AddWithValue("@user_id", Session["user_id"]);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                lblDataCount.Text = dt.Rows.Count.ToString();
                return dt;
            }
        }
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
            // 獲取資料並確保非 null（如果為 null，則使用空字符串）
            string equipmentData = DataBinder.Eval(e.Row.DataItem, "equipment") != null ? DataBinder.Eval(e.Row.DataItem, "equipment").ToString() : "";
            string other = DataBinder.Eval(e.Row.DataItem, "other") != null ? DataBinder.Eval(e.Row.DataItem, "other").ToString() : "";

            // 將 equipmentData 轉換為 List<int>，確保不會拋出異常
            List<int> selectedEquipment = new List<int>();
            if (!string.IsNullOrEmpty(equipmentData))
            {
                selectedEquipment = equipmentData.Split(',').Select(int.Parse).ToList();
            }

            // 獲取對應的 CheckBox 控件
            CheckBox cb1 = e.Row.FindControl("CheckBox1") as CheckBox; // 視訊會議攝影機
            CheckBox cb2 = e.Row.FindControl("CheckBox2") as CheckBox; // 音響系統
            CheckBox cb3 = e.Row.FindControl("CheckBox3") as CheckBox; // 便利紙和筆
            CheckBox cb4 = e.Row.FindControl("CheckBox4") as CheckBox; // 麥克風
            CheckBox cb5 = e.Row.FindControl("CheckBox5") as CheckBox; // 投影機
            CheckBox cb6 = e.Row.FindControl("CheckBox6") as CheckBox; // 大型顯示螢幕
            CheckBox cb7 = e.Row.FindControl("CheckBox7") as CheckBox; // 白板
            CheckBox cb8 = e.Row.FindControl("CheckBox8") as CheckBox; // 電腦
            CheckBox cb9 = e.Row.FindControl("CheckBox9") as CheckBox; // 其他
            TextBox gv_other = e.Row.FindControl("gv_other") as TextBox; // 其他
            Panel gv_p = e.Row.FindControl("gv_p") as Panel;

            if (gv_p != null)
            {
                gv_p.Enabled = false;
            }

            // 根據 equipment 欄位的數據設置 CheckBox 勾選狀態
            if (cb1 != null) cb1.Checked = selectedEquipment.Contains(1);
            if (cb2 != null) cb2.Checked = selectedEquipment.Contains(2);
            if (cb3 != null) cb3.Checked = selectedEquipment.Contains(3);
            if (cb4 != null) cb4.Checked = selectedEquipment.Contains(4);
            if (cb5 != null) cb5.Checked = selectedEquipment.Contains(5);
            if (cb6 != null) cb6.Checked = selectedEquipment.Contains(6);
            if (cb7 != null) cb7.Checked = selectedEquipment.Contains(7);
            if (cb8 != null) cb8.Checked = selectedEquipment.Contains(8);
            if (cb9 != null) cb9.Checked = selectedEquipment.Contains(9);

            // 如果 "其他" 的 CheckBox 被選中，顯示對應的文本
            if (cb9 != null && cb9.Checked && gv_other != null)
            {
                gv_other.Text = other;
            }

            // 根據權限設置 "功能" 欄位的顯示
            if (Session["user_right_id"] != null && Session["user_right_id"].ToString() != "1" && Session["user_right_id"].ToString() != "2" && Session["user_right_id"].ToString() != "3")
            {
                // 隱藏功能欄位
                gv.Columns[7].Visible = false; // 最後一個欄位為功能欄位
            }
            else
            {
                // 顯示功能欄位
                gv.Columns[7].Visible = true;
            }

            Label fov = e.Row.FindControl("is_favorite") as Label;
            CheckBox favorite = e.Row.FindControl("favorite") as CheckBox;

            // 設置收藏狀態
            if (fov != null && fov.Text == "1" && favorite != null)
            {
                favorite.Checked = true;
            }

            // 為 favorite CheckBox 設置 "data-item-id" 屬性
            if (favorite != null)
            {
                // 获取当前行的 id 数据
                string itemId = DataBinder.Eval(e.Row.DataItem, "id") != null ? DataBinder.Eval(e.Row.DataItem, "id").ToString() : "";

                // 动态设置 data-item-id 属性
                favorite.Attributes["data-item-id"] = itemId;
            }
        }
    }
    protected void gv_RowCreated(object sender, GridViewRowEventArgs e)
    {
    }
    protected void gv_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "modify")
        {
            Button btn = (Button)e.CommandSource;
            GridViewRow row = (GridViewRow)btn.NamingContainer;
            string id = e.CommandArgument.ToString();
            string color = "";

            submit.Style["display"] = "none";
            update.Style["display"] = "inline-block";

            hf_id.Value = id;
            using (SqlConnection cn = new SqlConnection(eip))
            {
                cn.Open();
                string sql = @"SELECT [id],[meet_name],[meet_location],[number],[equipment],[other],[color]  FROM [meeting_equipment] where id=@id";
                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("id", id);
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        m_name.Text = dr["meet_name"].ToString();
                        m_location.Text = dr["meet_location"].ToString();
                        m_number.Text = dr["number"].ToString();
                        color = dr["color"].ToString();
                        string equipmentData = dr["equipment"].ToString();
                        List<int> selectedEquipment = new List<int>();
                        if (!string.IsNullOrEmpty(equipmentData))
                        {
                            selectedEquipment = equipmentData.Split(',').Select(int.Parse).ToList();
                        }
                        // 根據 equipment 欄位的數據設置 CheckBox 勾選狀態
                        if (chk1 != null) chk1.Checked = selectedEquipment.Contains(1);
                        if (chk2 != null) chk2.Checked = selectedEquipment.Contains(2);
                        if (chk3 != null) chk3.Checked = selectedEquipment.Contains(3);
                        if (chk4 != null) chk4.Checked = selectedEquipment.Contains(4);
                        if (chk5 != null) chk5.Checked = selectedEquipment.Contains(5);
                        if (chk6 != null) chk6.Checked = selectedEquipment.Contains(6);
                        if (chk7 != null) chk7.Checked = selectedEquipment.Contains(7);
                        if (chk8 != null) chk8.Checked = selectedEquipment.Contains(8);
                        if (chk9 != null) chk9.Checked = selectedEquipment.Contains(9);

                        // 如果 "其他" 的 CheckBox 被選中，顯示對應的文本
                        if (chk9 != null && chk9.Checked && other != null)
                        {
                            other.Text = dr["other"].ToString();
                        }
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "SetSelectedColor",
                      "setSelectedColor('" + color + "');", true);
                    }
                    //ClientScript.RegisterStartupScript(this.GetType(), "CallShowModa", "showModal1()", true);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "CallShowModa", "showModal1();", true);
                }
            }
            gv_BindData();

        }

        if (e.CommandName == "del")
        {
            Button btn = (Button)e.CommandSource;
            GridViewRow row = (GridViewRow)btn.NamingContainer;
            string id = e.CommandArgument.ToString();
            using (SqlConnection cn = new SqlConnection(eip))
            {
                cn.Open();
                string sql = @"delete [meeting_equipment] where id=@id";


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
    protected void CheckBox_CheckedChanged(object sender, EventArgs e)
    {
        // 確定是哪個 CheckBox 被點擊
        CheckBox checkBox = (CheckBox)sender;
        GridViewRow row = (GridViewRow)checkBox.NamingContainer;

        Label id_lb = (Label)row.FindControl("id");
        int id = Convert.ToInt32(id_lb.Text);
        // 獲取該行的 ID


        // 建立設備數據列表，用於將選中設備的數字代碼存入資料庫
        List<int> selectedEquipment = new List<int>();

        // 檢查每個 CheckBox 的勾選狀態
        if (((CheckBox)row.FindControl("CheckBox1")).Checked) selectedEquipment.Add(1);
        if (((CheckBox)row.FindControl("CheckBox2")).Checked) selectedEquipment.Add(2);
        if (((CheckBox)row.FindControl("CheckBox3")).Checked) selectedEquipment.Add(3);
        if (((CheckBox)row.FindControl("CheckBox4")).Checked) selectedEquipment.Add(4);
        if (((CheckBox)row.FindControl("CheckBox5")).Checked) selectedEquipment.Add(5);
        if (((CheckBox)row.FindControl("CheckBox6")).Checked) selectedEquipment.Add(6);
        if (((CheckBox)row.FindControl("CheckBox7")).Checked) selectedEquipment.Add(7);
        if (((CheckBox)row.FindControl("CheckBox8")).Checked) selectedEquipment.Add(8);
        if (((CheckBox)row.FindControl("CheckBox9")).Checked) selectedEquipment.Add(9);

        string equipmentData = string.Join(",", selectedEquipment);


        UpdateDatabase(id, equipmentData);
    }
    protected void gv_other_TextChanged(object sender, EventArgs e)
    {
        // 確定是哪個 CheckBox 被點擊
        TextBox gv_other = (TextBox)sender;
        GridViewRow row = (GridViewRow)gv_other.NamingContainer;

        Label id_lb = (Label)row.FindControl("id");
        int id = Convert.ToInt32(id_lb.Text);
        // 獲取該行的 ID
        using (SqlConnection cn = new SqlConnection(eip))
        {
            string sql = "UPDATE meeting_equipment SET other = @other WHERE id = @id";
            SqlCommand cmd = new SqlCommand(sql, cn);
            cmd.Parameters.AddWithValue("@other", gv_other.Text);
            cmd.Parameters.AddWithValue("@id", id);

            cn.Open();
            cmd.ExecuteNonQuery();
            cn.Close();
        }
    }
    private void UpdateDatabase(int id, string equipmentData)
    {
        using (SqlConnection cn = new SqlConnection(eip))
        {
            string sql = "UPDATE meeting_equipment SET equipment = @equipment WHERE id = @id";
            SqlCommand cmd = new SqlCommand(sql, cn);
            cmd.Parameters.AddWithValue("@equipment", equipmentData);
            cmd.Parameters.AddWithValue("@id", id);

            cn.Open();
            cmd.ExecuteNonQuery();
            cn.Close();
        }
    }
    protected void submit_Click(object sender, EventArgs e)
    {

        List<string> selectedItems = new List<string>();
        if (chk1.Checked)
            selectedItems.Add("1");
        if (chk2.Checked)
            selectedItems.Add("2");
        if (chk3.Checked)
            selectedItems.Add("3");
        if (chk4.Checked)
            selectedItems.Add("4");
        if (chk5.Checked)
            selectedItems.Add("5");
        if (chk6.Checked)
            selectedItems.Add("6");
        if (chk7.Checked)
            selectedItems.Add("7");
        if (chk8.Checked)
            selectedItems.Add("8");
        if (chk9.Checked)
            selectedItems.Add("9");
        string selectedValues = string.Join(",", selectedItems);
        string sql = "";
        using (SqlConnection cn = new SqlConnection(eip))
        {
            cn.Open();
            Button btn = (Button)sender; // 確定按鈕來源
            if (btn.ID == "submit")  // 判斷是否是新增按鈕
            {
                sql = @"INSERT INTO meeting_equipment (meet_name, meet_location, number,equipment,other,color) 
                            VALUES (@meet_name, @meet_location, @number, @equipment,@other,@color)";
            }
            else if (btn.ID == "update")  // 判斷是否是更新按鈕
            {
                sql = @"update meeting_equipment set meet_name=@meet_name, meet_location=@meet_location, number=@number,equipment=@equipment,other=@other,color=@color where id=@id";
            }


            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.AddWithValue("@meet_name", m_name.Text);
                cmd.Parameters.AddWithValue("@meet_location", m_location.Text);
                cmd.Parameters.AddWithValue("@number", m_number.Text);
                cmd.Parameters.AddWithValue("@equipment", selectedValues);
                if (chk9.Checked)
                {
                    cmd.Parameters.AddWithValue("@other", other.Text);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@other", DBNull.Value);
                }
                cmd.Parameters.AddWithValue("@color", SelectedColor.Value);
                if (btn.ID == "update")  // 判斷是否是更新按鈕
                {
                    cmd.Parameters.AddWithValue("@id", hf_id.Value);
                }
                cmd.ExecuteNonQuery();
            }
        }
        Response.Redirect("Meetingroom_management.aspx"); // 重定向到成功頁面
    }
    protected void update_Click(object sender, EventArgs e)
    {

    }


    protected void favorite_CheckedChanged(object sender, EventArgs e)
    {

    }







    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        LinkButton lb = (LinkButton)sender;
        Session["ddl3"] = lb.CommandName;
        Response.Redirect("Meetingroom_calendar.aspx?mod=w");

    }
}