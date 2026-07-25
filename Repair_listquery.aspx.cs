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

public partial class Repair_listquery : System.Web.UI.Page
{
    string eip = WebConfigurationManager.ConnectionStrings["eip"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        MasterPage2 master = (MasterPage2)this.Master;
        Label masterLabel = (Label)master.FindControl("page_title");
        masterLabel.Text = "修繕申請查詢";
        Literal link_li = (Literal)master.FindControl("link_li");
        if (!IsPostBack)
        {
            if (link_li != null) link_li.Text += "<li>修繕申請查詢</li>";
            gv_BindData();
            getddl();
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
                      ";

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
    protected DataTable search()
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
            if (keyword.Text != "")
            {
                sql += " and (apply_user like '%" + keyword.Text + "%' or t4.Location_name like '%" + keyword.Text + "%' or apply_reason like '%" + keyword.Text + "%')  ";
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
}