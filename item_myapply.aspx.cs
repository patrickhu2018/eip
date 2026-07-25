using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.DirectoryServices;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class item_myapply : System.Web.UI.Page
{
    string eip = WebConfigurationManager.ConnectionStrings["eip"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        MasterPage master = (MasterPage)this.Master;
        Label masterLabel = (Label)master.FindControl("page_title");
        masterLabel.Text = "我的申請";
        Literal link_li = (Literal)master.FindControl("link_li");
        if (!IsPostBack)
        {
            ViewState["SortExpression"] = "apply_date";
            ViewState["SortDirection"] = "DESC";
            getddl();
            apply_group.SelectedValue = Session["user_group"].ToString();
            gv_BindData();
            if (link_li != null) link_li.Text += "<li>我的申請</li>";
            rb_lastWeek.Checked = true;
            apply_state.SelectedValue = "1";
            //apply_state.Enabled = false;
            gv.DataSource = search("", "");
            gv.DataBind();
        }
    }
    private void getddl()
    {
        using (SqlConnection cn = new SqlConnection(eip))
        {
            cn.Open();
            string sql = @"SELECT [id],[name]  FROM [group_name] order by id ASC";
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    apply_group.Items.Add(new ListItem(dr["name"].ToString(), dr["id"].ToString()));
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
                          where state=1 and (apply_group=@apply_group or apply_user=@apply_user) ";

            if (!string.IsNullOrEmpty(sortExpression))
            {
                sql += @"order by " + sortExpression + " " + sortDirection;
            }
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.AddWithValue("@apply_group", Session["group_name"].ToString());
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
                          ,[apply_group],[apply_user],[apply_date],t2.name　as product_id,[price],[number],[total],[pass_date],
                          t3.name  as　Budget_id,[state],[user_list],[custodian_list],[note]
                          FROM [Item_Product_apply] as t1
                          left join Item_Product as t2 on t2.id=t1.product_id
                          left join Item_Budget as t3 on t3.id=t1.Budget_id
                          where (apply_group=@apply_group or apply_user=@apply_user)";

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
            if (!string.IsNullOrEmpty(sortExpression))
            {
                sql += @" order by " + sortExpression + " " + sortDirection;
            }
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.AddWithValue("@apply_group", Session["group_name"].ToString());
                cmd.Parameters.AddWithValue("@apply_user", Session["user_name"].ToString());
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

            /////////////////非自己的申請顯示無權限
            Button modifyBtn = (Button)e.Row.FindControl("modify");
            Button delBtn = (Button)e.Row.FindControl("del");
            Label nolimit = (Label)e.Row.FindControl("nolimit");
            string apply_user = DataBinder.Eval(e.Row.DataItem, "apply_user").ToString();
            string currentUserName = Session["user_name"].ToString();
            if (apply_user == currentUserName)
            {
                modifyBtn.Visible = true;
                delBtn.Visible = true;
            }
            else
            {
                modifyBtn.Visible = false;
                delBtn.Visible = false;
                nolimit.Visible = true;
            }

            if (state.Text == "已核銷")  //20250502
            {
                delBtn.Visible = false;
            }
        }
    }
    protected void gv_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "modify")
        {
            try
            {
                string[] args = e.CommandArgument.ToString().Split(',');
                string id = args[0];
                string state = args[1];
                //string id = e.CommandArgument.ToString();

                string url = "";
                if (state == "2")
                {
                    url = string.Format("item_addapply.aspx?m=2&mode=apply&id={0}",
                                          HttpUtility.UrlEncode(id));
                }
                else
                {
                    url = string.Format("item_addapply.aspx?m=1&mode=apply&id={0}",
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
                //gv_BindData();
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
    protected void addapply_Click(object sender, EventArgs e)
    {
        string url = string.Format("item_addapply.aspx?m=0&mode=apply");
        Response.Redirect(url);
    }



    protected void rb_SelectedIndexChanged(object sender, EventArgs e)
    {


        if (rb_Nolimit.Checked || rb_lastWeek.Checked || rb_lastMonth.Checked || rb_lastYear.Checked || rb_customRange.Checked)
        {
            //// 禁用 'rb2' 部分
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
            //// 啟用 'rb' 部分
            //rb_Nolimit.Checked = true;
            //rb_Nolimit.Enabled = true;
            //rb_lastWeek.Enabled = true;
            //rb_lastMonth.Enabled = true;
            //rb_lastYear.Enabled = true;
            //rb_customRange.Enabled = true;

        }
        else
        {
            // 禁用 'rb' 部分
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
}