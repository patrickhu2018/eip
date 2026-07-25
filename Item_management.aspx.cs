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

public partial class Item_management : System.Web.UI.Page
{
    string eip = WebConfigurationManager.ConnectionStrings["eip"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        Session["right"] = "1";

        if (!IsPostBack)
        {
            MasterPage master = (MasterPage)this.Master;
            Label masterLabel = (Label)master.FindControl("page_title");
            masterLabel.Text = "品名及科目管理";
            Literal link_li = (Literal)master.FindControl("link_li");
            if (link_li != null) link_li.Text += "<li>品名及科目管理</li>";
            p_BindData();
            b_BindData();
        }

    }
    private void p_BindData()
    {
        p_gv.DataSource = GetProduct();
        p_gv.DataBind();
        lblPageIndex.Text = (p_gv.PageIndex + 1) + " / " + p_gv.PageCount;
        UpdatePagerControls();
    }
    private void b_BindData()
    {
        b_gv.DataSource = GetBudget();
        b_gv.DataBind();
        lblPageIndex2.Text = (b_gv.PageIndex + 1) + " / " + b_gv.PageCount;
        UpdatePagerControls2();
    }
    private DataTable GetProduct()
    {

        using (SqlConnection cn = new SqlConnection(eip))
        {
            cn.Open();
            string sql = @"SELECT [id],[name]  FROM [Item_Product] order by id ASC";
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
    private DataTable GetBudget()
    {

        using (SqlConnection cn = new SqlConnection(eip))
        {
            cn.Open();
            string sql = @"SELECT [id],[name]  FROM [Item_Budget] order by id ASC";
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                lblDataCount2.Text = dt.Rows.Count.ToString();
                return dt;
            }
        }
    }
    protected void p_gv_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Remove")
        {
            // 取得要移除的項目ID
            string id = e.CommandArgument.ToString();
            DeleteItem(id);
            p_BindData();
        }
    }
    protected void p_gv_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        // 更新當前頁索引
        p_gv.PageIndex = e.NewPageIndex;

        // 重新綁定資料
        p_BindData();
        
    }
    protected void b_gv_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Remove")
        {
            // 取得要移除的項目ID
            string id = e.CommandArgument.ToString();
            DeleteBudget(id);
            b_BindData();
        }
    }
    protected void b_gv_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        // 更新當前頁索引
        b_gv.PageIndex = e.NewPageIndex;

        // 重新綁定資料
        b_BindData();
       
    }
    private void DeleteItem(string id)
    {

        using (SqlConnection cn = new SqlConnection(eip))
        {
            cn.Open();
            string sql = @"DELETE FROM Item_Product WHERE id = @id";
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.Add(new SqlParameter("@id", id));

                cmd.ExecuteNonQuery();
            }
        }
    }
    private void DeleteBudget(string id)
    {

        using (SqlConnection cn = new SqlConnection(eip))
        {
            cn.Open();
            string sql = @"DELETE FROM Item_Budget WHERE id = @id";
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.Add(new SqlParameter("@id", id));

                cmd.ExecuteNonQuery();
            }
        }
    }
    protected void AddProduct_Click(object sender, EventArgs e)
    {
        string Product_name = "";
        if (string.IsNullOrWhiteSpace(Product.Text))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('未填品名');", true);
            return;
        }
        using (SqlConnection cn = new SqlConnection(eip))
        {
            cn.Open();
            string sql = @"select name from  Item_Product where name=@name";

            SqlCommand cmd = new SqlCommand(sql, cn);
            cmd.Parameters.AddWithValue("@name", Product.Text);

            SqlDataReader dr = cmd.ExecuteReader();

            if (dr.Read())
            {
                Product_name = dr["name"].ToString();
            }
            cn.Close();
        }
        if (Product_name == Product.Text)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('品名重複');", true);
            return;
        }
        using (SqlConnection cn = new SqlConnection(eip))
        {
            string sql = @"insert into Item_Product (name) values (@name)";

            SqlCommand cmd = new SqlCommand(sql, cn);

            cmd.Parameters.AddWithValue("@name", Product.Text);

            cn.Open();
            cmd.ExecuteNonQuery();
            cn.Close();

        }
        Product.Text = "";
        p_BindData();
    }

    protected void AddBudget_Click(object sender, EventArgs e)
    {
        string Budget_name = "";
        if (string.IsNullOrWhiteSpace(Budget.Text))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('未填品名');", true);
            return;
        }
        using (SqlConnection cn = new SqlConnection(eip))
        {
            cn.Open();
            string sql = @"select name from  Item_Budget where name=@name";

            SqlCommand cmd = new SqlCommand(sql, cn);
            cmd.Parameters.AddWithValue("@name", Budget.Text);

            SqlDataReader dr = cmd.ExecuteReader();

            if (dr.Read())
            {
                Budget_name = dr["name"].ToString();
            }
            cn.Close();
        }
        if (Budget_name == Budget.Text)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('品名重複');", true);
            return;
        }
        using (SqlConnection cn = new SqlConnection(eip))
        {
            string sql = @"insert into Item_Budget (name) values (@name)";

            SqlCommand cmd = new SqlCommand(sql, cn);

            cmd.Parameters.AddWithValue("@name", Budget.Text);

            cn.Open();
            cmd.ExecuteNonQuery();
            cn.Close();
        }
        Budget.Text = "";
        b_BindData();
    }
    private void UpdatePagerControls()
    {
        // 更新分頁顯示
        lblPageIndex.Text = (p_gv.PageIndex + 1) + "/" + p_gv.PageCount;

       

        // 更新分頁控制按鈕狀態
        lkbPagePrev.Enabled = p_gv.PageIndex > 0;
        lkbPageNext.Enabled = p_gv.PageIndex < p_gv.PageCount - 1;
    }
    protected void ddlPageIndex_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList d = (DropDownList)sender;
        p_gv.PageIndex = int.Parse(d.SelectedValue);
        p_BindData();

    }

    protected void lkbPagePrev_Click(object sender, EventArgs e)
    {
        if (p_gv.PageIndex > 0)
        {
            p_gv.PageIndex--;
            p_BindData();
        }
    }


    protected void lkbPageNext_Click(object sender, EventArgs e)
    {
        if (p_gv.PageIndex < p_gv.PageCount - 1)
        {
            p_gv.PageIndex++;
            p_BindData();
        }

    }
    private void UpdatePagerControls2()
    {
        // 更新分頁顯示
        lblPageIndex2.Text = (b_gv.PageIndex + 1) + "/" + b_gv.PageCount;

       

        // 更新分頁控制按鈕狀態
        lkbPagePrev2.Enabled = b_gv.PageIndex > 0;
        lkbPageNext2.Enabled = b_gv.PageIndex < b_gv.PageCount - 1;
    }
    protected void ddlPageIndex2_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList d = (DropDownList)sender;
        b_gv.PageIndex = int.Parse(d.SelectedValue);
        b_BindData();

    }

    protected void lkbPagePrev2_Click(object sender, EventArgs e)
    {
        if (b_gv.PageIndex > 0)
        {
            b_gv.PageIndex--;
            b_BindData();
        }
    }


    protected void lkbPageNext2_Click(object sender, EventArgs e)
    {
        if (b_gv.PageIndex < b_gv.PageCount - 1)
        {
            b_gv.PageIndex++;
            b_BindData();
        }

    }
}