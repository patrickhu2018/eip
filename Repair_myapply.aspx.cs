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

public partial class Repair_myapply : System.Web.UI.Page
{
    string eip = WebConfigurationManager.ConnectionStrings["eip"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        MasterPage2 master = (MasterPage2)this.Master;
        Label masterLabel = (Label)master.FindControl("page_title");
        masterLabel.Text = "我的申請";
        Literal link_li = (Literal)master.FindControl("link_li");
        if (!IsPostBack)
        {
            if (link_li != null) link_li.Text += "<li>我的申請</li>";
            ViewState["SortExpression"] = "apply_date";
            ViewState["SortDirection"] = "DESC";
            gv_BindData();
            getddl();
            showTempData();
            searchbt_Click(null, null);
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
        apply_floor.Items.Insert(0, new ListItem("請選擇", "0"));
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
            apply_floor.Items.Insert(0, new ListItem("請選擇", "0"));
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
                      where apply_user=@apply_user";

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
                      where apply_user=@apply_user";

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
            //if (chk3.Checked)
            //{
            //    selectedStates.Add(2);
            //}
            //if (chk4.Checked)
            //{
            //    selectedStates.Add(3);
            //}

            if (selectedStates.Count > 0)
            {
                sql += " AND state IN (" + string.Join(",", selectedStates) + ")";
            }
            //if ((rb_lastWeek.Checked))
            //{
            //    sql += " and apply_date >= DATEADD(day, -7, GETDATE()) ";
            //}
            else if (rb_lastMonth.Checked)
            {
                sql += " and apply_date >= DATEADD(month, -1, GETDATE()) ";
            }
            //else if (rb_lastYear.Checked)
            //{
            //    sql += " and apply_date >= DATEADD(year, -1, GETDATE()) ";
            //}
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
            if (!string.IsNullOrEmpty(sortExpression))
            {
                sql += @" ORDER BY " + sortExpression + " " + sortDirection;
            }
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.AddWithValue("@apply_user", Session["user_name"].ToString());
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
        tempData();
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
            Label stateDisplay = (Label)e.Row.FindControl("stateDisplay");
            Button checkbt = (Button)e.Row.FindControl("check");

            switch (state.Text)
            {
                case "0":
                    stateDisplay.Text = "待處理";


                    break;
                case "1":
                    stateDisplay.Text = "處理中";

                    break;
                case "2":
                    stateDisplay.Text = "已完成";

                    break;
                case "3":
                    stateDisplay.Text = "退件";
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
    protected void addapply_Click(object sender, EventArgs e)
    {
        Response.Redirect("Repair_addapply.aspx?m=0");
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

        }

    }

    protected void tempData()
    {

        Session["temp_apply_place"] = apply_place.SelectedValue;
        Session["temp_apply_floor"] = apply_floor.SelectedValue;
        Session["temp_chk1"] = chk1.Checked;
        Session["temp_chk2"] = chk2.Checked;
        //Session["temp_chk3"] = chk3.Checked;
        //Session["temp_chk4"] = chk4.Checked;


       // Session["temp_rb_lastWeek"] = rb_lastWeek.Checked;
        Session["temp_rb_lastMonth"] = rb_lastMonth.Checked;
       // Session["temp_rb_lastYear"] = rb_lastYear.Checked;
        Session["temp_apply_start"] = apply_start.Text;
        Session["temp_apply_end"] = apply_end.Text;



    }

    protected void showTempData()
    {

        if (Session["temp_apply_place"] != null)
        {
            apply_place.SelectedValue = Session["temp_apply_place"].ToString();
        }
        if (Session["temp_apply_floor"] != null)
        {
            apply_floor.SelectedValue = Session["temp_apply_floor"].ToString();
        }

        if (Session["temp_chk1"] != null)
        {
            chk1.Checked = Convert.ToBoolean(Session["temp_chk1"].ToString());
        }
        if (Session["temp_chk2"] != null)
        {
            chk2.Checked = Convert.ToBoolean(Session["temp_chk2"].ToString());
        }
        //if (Session["temp_chk3"] != null)
        //{
        //    chk3.Checked = Convert.ToBoolean(Session["temp_chk3"].ToString());
        //}
        //if (Session["temp_chk4"] != null)
        //{
        //    chk4.Checked = Convert.ToBoolean(Session["temp_chk4"].ToString());
        //}


        if (Session["temp_apply_start"] != null)
        {
            apply_start.Text = Session["temp_apply_start"].ToString();
        }
        if (Session["temp_apply_end"] != null)
        {
            apply_end.Text = Session["temp_apply_end"].ToString();
        }

        //if (Session["temp_rb_lastWeek"] != null)
        //{
        //    rb_lastWeek.Checked = Convert.ToBoolean(Session["temp_rb_lastWeek"].ToString());
        //}
        if (Session["temp_rb_lastMonth"] != null)
        {
            rb_lastMonth.Checked = Convert.ToBoolean(Session["temp_rb_lastMonth"].ToString());
        }
        //if (Session["temp_rb_lastYear"] != null)
        //{
        //    rb_lastYear.Checked = Convert.ToBoolean(Session["temp_rb_lastYear"].ToString());
        //}



    }
}