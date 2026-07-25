using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Text;

public partial class item_addapply : System.Web.UI.Page
{
    string eip = WebConfigurationManager.ConnectionStrings["eip"].ConnectionString;
    private string id;
    private string m;
    private string mode;
    protected void Page_Load(object sender, EventArgs e)
    {
        id = Request.QueryString["id"];
        m = Request.QueryString["m"];//0新增 1編輯
        mode = Request.QueryString["mode"];
        MasterPage master = (MasterPage)this.Master;
        Label masterLabel = (Label)master.FindControl("page_title");
        Literal link_li = (Literal)master.FindControl("link_li");
        masterLabel.Text = "我的申請";
        if (!IsPostBack)
        {
            if (link_li != null) link_li.Text += "<li>我的申請</li>";
            getproduct();
            //getcount();
            getBudget();
            user_gv_BindData();
            custodian_gv_BindData();
            if (m != "0")
            {
                pl.Visible = true;
                product.Enabled = false;
                txtPrice.Enabled = false;
                txtCount.Enabled = false;
                total.Enabled = false;
                if (m == "2")  //20250502
                {
                    pass_date.Enabled = false;
                    Budget.Enabled = false;
                }
                showdata();
            }
            else
            {
                apply_date.Text = DateTime.Today.ToString("yyyy-MM-dd");
                apply_group.Text = Session["group_name"].ToString();
                apply_user.Text = Session["user_name"].ToString();
            }

        }
    }
    private void showdata()
    {
        using (SqlConnection cn = new SqlConnection(eip))
        {
            cn.Open();
            string sql = @"SELECT  [id],[apply_group],[apply_user],[apply_date],[product_id],[price],[number],[total],[pass_date]
                            ,[Budget_id],[state],[note]  FROM [Item_Product_apply] where id=@id";
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.AddWithValue("@id", id);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    DateTime applydate = Convert.ToDateTime(dr["apply_date"]);
                    apply_date.Text = applydate.ToString("yyyy-MM-dd");
                    apply_group.Text = dr["apply_group"].ToString();
                    apply_user.Text = dr["apply_user"].ToString();
                    product.SelectedValue = dr["product_id"].ToString();
                    txtPrice.Text = dr["price"].ToString();
                    txtCount.Text = dr["number"].ToString();
                    total.Text = dr["total"].ToString();
                    if (!string.IsNullOrEmpty(dr["pass_date"].ToString()))
                    {
                        DateTime passdate = Convert.ToDateTime(dr["pass_date"]);
                        pass_date.Text = passdate.ToString("yyyy-MM-dd");
                    }

                    note.Text = dr["note"].ToString();
                    Budget.SelectedValue = dr["Budget_id"].ToString();
                    note.Text = dr["note"].ToString();
                }
            }
        }
    }
    private void getproduct()
    {
        product.Items.Insert(0, new ListItem("請選擇", "0"));
        using (SqlConnection cn = new SqlConnection(eip))
        {
            cn.Open();
            string sql = @"SELECT [id],[name]  FROM [Item_Product] order by id ASC";
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    product.Items.Add(new ListItem(dr["name"].ToString(), dr["id"].ToString()));
                }

            }
        }
    }
    //private void getcount()
    //{
    //    txtCount.Items.Insert(0, new ListItem("請選擇", "0"));
    //    for (int i = 1; i <= 100; i++)
    //    {
    //        txtCount.Items.Add(new ListItem(i.ToString(), i.ToString()));
    //    }
    //}
    private void getBudget()
    {
        Budget.Items.Insert(0, new ListItem("請選擇", "0"));
        using (SqlConnection cn = new SqlConnection(eip))
        {
            cn.Open();
            string sql = @"SELECT [id],[name]  FROM [Item_Budget] order by id ASC";
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    Budget.Items.Add(new ListItem(dr["name"].ToString(), dr["id"].ToString()));
                }

            }
        }
    }
    //protected void count_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    int price;
    //    if (!string.IsNullOrWhiteSpace(txtPrice.Text))
    //    {
    //        int.TryParse(txtPrice.Text, out price);
    //    }
    //    else
    //    {
    //        price = 0;
    //    }
    //    int count = 0;
    //    count = txtCount.SelectedIndex;
    //    total.Text = (price * count).ToString();
    //}
    protected void count_TextChanged(object sender, EventArgs e)
    {
        int price;
        if (!string.IsNullOrWhiteSpace(txtPrice.Text))
        {
            int.TryParse(txtPrice.Text, out price);
        }
        else
        {
            price = 0;
        }
        int count = 0;
        if (!int.TryParse(txtCount.Text, out count))
        {
            count = 0;  // 如果轉換失敗，則設置為 0
        }
        total.Text = (price * count).ToString();
    }
    protected void txtPrice_TextChanged(object sender, EventArgs e)
    {
        int price;
        if (!string.IsNullOrWhiteSpace(txtPrice.Text))
        {
            int.TryParse(txtPrice.Text, out price);
        }
        else
        {
            price = 0;
        }
        int count = 0;
        if (!int.TryParse(txtCount.Text, out count))
        {
            count = 0;  // 如果轉換失敗，則設置為 0
        }
        total.Text = (price * count).ToString();
    }

    protected void Cancel_Click(object sender, EventArgs e)
    {
        //if (m != "0")
        //{
        //    Response.Redirect("item_listquery.aspx");
        //}
        //else
        //{
        //    Response.Redirect("item_myapply.aspx");
        //}

        if (mode == "review")
        {
            Response.Redirect("item_listquery.aspx");
        }
        else if (mode == "apply")
        {
            Response.Redirect("item_myapply.aspx");
        }

    }

    protected void Submit_Click(object sender, EventArgs e)
    {
        mdgroup.InnerText = apply_group.Text;
        mduser.InnerText = apply_user.Text;
        if (string.IsNullOrEmpty(product.SelectedValue))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('品名尚未選擇');", true);
            return;
        }
        else { mdproduct.InnerText = product.SelectedItem.ToString(); }

        if (string.IsNullOrEmpty(txtPrice.Text))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('單價尚未填寫');", true);
            return;
        }
        else { mdprice.InnerText = txtPrice.Text; }

        if (string.IsNullOrEmpty(txtCount.Text))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('數量尚未選擇');", true);
            return;
        }
        else { mdcount.InnerText = txtCount.Text; }

        if (string.IsNullOrEmpty(total.Text))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('合計尚未填寫');", true);
            return;
        }

        if (m=="1")
        {
            if (string.IsNullOrEmpty(pass_date.Text))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('採購日期尚未填寫');", true);
                return;
            }

            if (string.IsNullOrEmpty(Budget.Text))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('預算科目尚未填寫');", true);
                return;
            }
        }

        else { mdtotal.InnerText = total.Text; }

        string script = "";
        if (m == "0")
        {
            script = "showModal1();";
        }
        else
        {
            script = "showModal2();";
        }

        ClientScript.RegisterStartupScript(this.GetType(), "CallShowModa", script, true);
    }

    protected void Butsubmit_Click(object sender, EventArgs e)
    {
        if (m == "0")
        {
            using (SqlConnection cn = new SqlConnection(eip))
            {
                cn.Open();
                string sql = @"INSERT INTO [Item_Product_apply] ([apply_group],[apply_user],[apply_date],[product_id],[price],[number],[total],[state],[updateDate],[updateUser],[creatDate],[creatUser])
                           VALUES (@apply_group,@apply_user,@apply_date,@product_id,@price,@number,@total,@state,@updateDate,@updateUser,@creatDate,@creatUser)";
                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@apply_group", apply_group.Text);
                    cmd.Parameters.AddWithValue("@apply_user", apply_user.Text);
                    cmd.Parameters.AddWithValue("@apply_date", apply_date.Text);
                    cmd.Parameters.AddWithValue("@product_id", product.SelectedValue);
                    cmd.Parameters.AddWithValue("@price", txtPrice.Text);
                    cmd.Parameters.AddWithValue("@number", txtCount.Text);
                    cmd.Parameters.AddWithValue("@total", total.Text);
                    cmd.Parameters.AddWithValue("@state", "1");
                    cmd.Parameters.AddWithValue("@creatDate", DateTime.Now);
                    cmd.Parameters.AddWithValue("@creatUser", Session["user_name"].ToString());
                    cmd.Parameters.AddWithValue("@updateDate", DateTime.Now);
                    cmd.Parameters.AddWithValue("@updateUser", Session["user_name"].ToString());
                    cmd.ExecuteNonQuery();

                }
            }
        }
        else
        {
            using (SqlConnection cn = new SqlConnection(eip))
            {
                cn.Open();
                string sql = @"update Item_Product_apply set pass_date=@pass_date,Budget_id=@Budget_id,note=@note,state=@state,
                               updateDate=@updateDate,updateUser=@updateUser where id=@id";
                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@pass_date", pass_date.Text);
                    cmd.Parameters.AddWithValue("@Budget_id", Budget.SelectedValue);
                    cmd.Parameters.AddWithValue("@note", note.Text);
                    cmd.Parameters.AddWithValue("@state", "2");
                    cmd.Parameters.AddWithValue("@updateDate", DateTime.Now);
                    cmd.Parameters.AddWithValue("@updateUser", Session["user_name"].ToString());
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();

                }
            }
        }
        Response.Redirect("item_myapply.aspx");
    }


    protected void product_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (product.SelectedValue != "0")
        {
            using (SqlConnection cn = new SqlConnection(eip))
            {
                cn.Open();
                string sql = @"SELECT top 1 [id]
                          ,[apply_group]
                          ,[apply_user]
                          ,[apply_date]
                          ,[product_id]
                          ,[price]
                          ,[number]
                          ,[total]
                          ,[pass_date]
                      FROM [Item_Product_apply]
                      where product_id=@product_id and pass_date is not null
                      order by pass_date DESC";
                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@product_id", product.SelectedValue);
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        string pass_Date = dr["pass_date"].ToString();
                        DateTime pass_date = DateTime.Parse(pass_Date);
                        lastbuy.Text = "(上次購買日期" + pass_date.ToString("yyyy/MM/dd")+")";
                    }

                }
            }
        }
        else
        {
            lastbuy.Text = " ";
        }
    }
    private DataTable Getuserlist()
    {
        if (m != "0")
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
                    lblDataCount.Text = dt.Rows.Count.ToString();
                    return dt;

                }
            }
        }
        else
        {
            DataTable dt = new DataTable();
            return dt;
        }

    }
    private DataTable Getcustodianlist()
    {
        if (m != "0")
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
        else
        {
            DataTable dt = new DataTable();
            return dt;
        }

    }
    private void user_gv_BindData()
    {
        user_gv.DataSource = Getuserlist();
        user_gv.DataBind();
        lblPageIndex.Text = (user_gv.PageIndex + 1) + " / " + user_gv.PageCount;
        UpdatePagerControls();
        getstock();
        showuserlist();
    }
    protected void user_gv_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "del")
        {
            try
            {
                string id = e.CommandArgument.ToString();
                using (SqlConnection cn = new SqlConnection(eip))
                {
                    cn.Open();
                    string sql = @"delete [item_apply_userlist] where id=@id";
                    using (SqlCommand cmd = new SqlCommand(sql, cn))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.ExecuteNonQuery();
                    }
                }
                user_gv_BindData();

            }
            catch (Exception ex)
            {
            }
        }
    }
    protected void user_gv_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        // 更新當前頁索引
        user_gv.PageIndex = e.NewPageIndex;

        // 重新綁定資料
        user_gv_BindData();
    }
    private void UpdatePagerControls()
    {
        // 更新分頁顯示
        lblPageIndex.Text = (user_gv.PageIndex + 1) + "/" + user_gv.PageCount;


        // 更新分頁控制按鈕狀態
        lkbPagePrev.Enabled = user_gv.PageIndex > 0;
        lkbPageNext.Enabled = user_gv.PageIndex < user_gv.PageCount - 1;
    }
    protected void lkbPagePrev_Click(object sender, EventArgs e)
    {
        if (user_gv.PageIndex > 0)
        {
            user_gv.PageIndex--;
            user_gv_BindData();
        }
    }
    protected void lkbPageNext_Click(object sender, EventArgs e)
    {
        if (user_gv.PageIndex < user_gv.PageCount - 1)
        {
            user_gv.PageIndex++;
            user_gv_BindData();
        }

    }
    private void custodian_gv_BindData()
    {
        custodian_gv.DataSource = Getcustodianlist();
        custodian_gv.DataBind();
        lblPageIndex2.Text = (custodian_gv.PageIndex + 1) + " / " + custodian_gv.PageCount;
        UpdatePagerControls2();
        getstock();
        showcustodianlist();

    }
    protected void custodian_gv_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "del")
        {
            try
            {
                string id = e.CommandArgument.ToString();
                using (SqlConnection cn = new SqlConnection(eip))
                {
                    cn.Open();
                    string sql = @"delete [item_apply_custodianlist] where id=@id";
                    using (SqlCommand cmd = new SqlCommand(sql, cn))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.ExecuteNonQuery();
                    }
                }
                custodian_gv_BindData();

            }
            catch (Exception ex)
            {
            }
        }
    }
    protected void custodian_gv_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        // 更新當前頁索引
        custodian_gv.PageIndex = e.NewPageIndex;

        // 重新綁定資料
        custodian_gv_BindData();
    }
    private void UpdatePagerControls2()
    {
        // 更新分頁顯示
        lblPageIndex2.Text = (custodian_gv.PageIndex + 1) + "/" + custodian_gv.PageCount;


        // 更新分頁控制按鈕狀態
        lkbPagePrev2.Enabled = custodian_gv.PageIndex > 0;
        lkbPageNext2.Enabled = custodian_gv.PageIndex < custodian_gv.PageCount - 1;
    }

    protected void lkbPagePrev2_Click(object sender, EventArgs e)
    {
        if (custodian_gv.PageIndex > 0)
        {
            custodian_gv.PageIndex--;
            custodian_gv_BindData();
        }
    }


    protected void lkbPageNext2_Click(object sender, EventArgs e)
    {
        if (custodian_gv.PageIndex < custodian_gv.PageCount - 1)
        {
            custodian_gv.PageIndex++;
            custodian_gv_BindData();
        }

    }
    private void showuserlist()
    {
        if (m != "0")
        {
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
                    user_list.Text = userNames.ToString();
                }
            }
        }
    }
    private void showcustodianlist()
    {
        if (m != "0")
        {
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
                    custodian_list.Text = userNames.ToString();
                }
            }
        }
    }
    protected void getstock()
    {
        if (m != "0")
        {
            int total = 0;
            int user_count = 0;
            int custodian_count = 0;
            using (SqlConnection cn = new SqlConnection(eip))
            {
                string sql = @"SELECT id,number FROM [Item_Product_apply] where id=@id";
                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cn.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        total = dr["number"] != DBNull.Value ? Convert.ToInt32(dr["number"]) : 0;
                    }
                }
            }
            using (SqlConnection cn = new SqlConnection(eip))
            {
                string sql = @"SELECT sum(user_quantity) as quantity FROM [item_apply_userlist] where apply_id=@apply_id";

                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@apply_id", id);
                    cmd.Parameters.AddWithValue("@user_name", user_name.Text);
                    cn.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        user_count = dr["quantity"] != DBNull.Value ? Convert.ToInt32(dr["quantity"]) : 0;
                    }
                }
            }
            using (SqlConnection cn = new SqlConnection(eip))
            {
                string sql = @"SELECT sum(custodian_quantity) as quantity FROM [item_apply_custodianlist] where apply_id=@apply_id";

                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@apply_id", id);
                    cn.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        custodian_count = dr["quantity"] != DBNull.Value ? Convert.ToInt32(dr["quantity"]) : 0;
                    }
                }
            }
            total = total - user_count - custodian_count;
            stocks.Text = "注意:目前尚未紀錄使用人、保管人的數量" + total + "個";
            custodian_stocks.Text = "注意:目前尚未紀錄使用人、保管人的數量" + total + "個";

        }
    }
    protected void adduser_Click(object sender, EventArgs e)
    {
        if (m != "0")
        {
            string erro = string.Empty;
            int total = 0;
            int user_count = 0;
            int custodian_count = 0;
            if (user_name.Text == "") erro += "請填寫使用人!\\n";
            if (user_quantity.Text == "") erro += "請填寫數量!\\n";
            if (erro != "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('" + erro + "');", true);
                return;
            }
            using (SqlConnection cn = new SqlConnection(eip))
            {
                string sql = @"SELECT id,number FROM [Item_Product_apply] where id=@id";
                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cn.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        total = dr["number"] != DBNull.Value ? Convert.ToInt32(dr["number"]) : 0;
                    }
                }
            }
            using (SqlConnection cn = new SqlConnection(eip))
            {
                string sql = @"SELECT sum(user_quantity) as quantity FROM [item_apply_userlist] where apply_id=@apply_id";

                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@apply_id", id);
                    cn.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        user_count = dr["quantity"] != DBNull.Value ? Convert.ToInt32(dr["quantity"]) : 0;
                    }
                }
            }
            using (SqlConnection cn = new SqlConnection(eip))
            {
                string sql = @"SELECT sum(custodian_quantity) as quantity FROM [item_apply_custodianlist] where apply_id=@apply_id";

                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@apply_id", id);
                    cn.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        custodian_count = dr["quantity"] != DBNull.Value ? Convert.ToInt32(dr["quantity"]) : 0;
                    }
                }
            }
            if ((user_count + custodian_count + int.Parse(user_quantity.Text)) > total)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('超出剩餘量請重新輸入');", true);
                return;
            }
            using (SqlConnection cn = new SqlConnection(eip))
            {
                string sql = @"SELECT * FROM [item_apply_userlist] where apply_id=@apply_id and user_name=@user_name";

                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@apply_id", id);
                    cmd.Parameters.AddWithValue("@user_name", user_name.Text);
                    cn.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('此用戶已領取過');", true);
                        user_name.Text = "";
                        user_quantity.Text = "";
                        return;
                    }
                }
            }
            string userName = Session["user_name"].ToString();
            using (SqlConnection cn = new SqlConnection(eip))
            {
                string sql = @"INSERT INTO [item_apply_userlist] ([apply_id],[user_name],[user_quantity],[creatDate],[creatUser])
                               VALUES
                               (@apply_id,@user_name,@user_quantity,@creatDate,@creatUser)";

                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@apply_id", id);
                    cmd.Parameters.AddWithValue("@user_name", user_name.Text);
                    cmd.Parameters.AddWithValue("@user_quantity", user_quantity.Text);
                    cmd.Parameters.AddWithValue("@creatDate", DateTime.Now);
                    cmd.Parameters.AddWithValue("@creatUser", userName);
                    cn.Open();
                    cmd.ExecuteNonQuery();


                }
            }
            user_name.Text = "";
            user_quantity.Text = "";


            user_gv_BindData();

        }
    }

    protected void addquantity_Click(object sender, EventArgs e)
    {
        if (m != "0")
        {
            string erro = string.Empty;
            int total = 0;
            int user_count = 0;
            int custodian_count = 0;
            if (custodian_name.Text == "") erro += "請填寫保管人!\\n";
            if (custodian_quantity.Text == "") erro += "請填寫數量!\\n";
            if (erro != "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('" + erro + "');", true);
                return;
            }
            using (SqlConnection cn = new SqlConnection(eip))
            {
                string sql = @"SELECT id,number FROM [Item_Product_apply] where id=@id";
                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cn.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        total = dr["number"] != DBNull.Value ? Convert.ToInt32(dr["number"]) : 0;
                    }
                }
            }
            using (SqlConnection cn = new SqlConnection(eip))
            {
                string sql = @"SELECT sum(user_quantity) as quantity FROM [item_apply_userlist] where apply_id=@apply_id";

                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@apply_id", id);
                    cn.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        user_count = dr["quantity"] != DBNull.Value ? Convert.ToInt32(dr["quantity"]) : 0;
                    }
                }
            }
            using (SqlConnection cn = new SqlConnection(eip))
            {
                string sql = @"SELECT sum(custodian_quantity) as quantity FROM [item_apply_custodianlist] where apply_id=@apply_id";

                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@apply_id", id);
                    cn.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        custodian_count = dr["quantity"] != DBNull.Value ? Convert.ToInt32(dr["quantity"]) : 0;
                    }
                }
            }
            if ((user_count + custodian_count + int.Parse(custodian_quantity.Text)) > total)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('超出剩餘量請重新輸入');", true);
                return;
            }
            using (SqlConnection cn = new SqlConnection(eip))
            {
                string sql = @"SELECT * FROM [item_apply_custodianlist] where apply_id=@apply_id and custodian_name=@custodian_name";

                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@apply_id", id);
                    cmd.Parameters.AddWithValue("@custodian_name", custodian_name.Text);
                    cn.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('此用戶已領取過');", true);
                        custodian_name.Text = "";
                        custodian_quantity.Text = "";
                        return;
                    }
                }
            }
            string userName = Session["user_name"].ToString();
            using (SqlConnection cn = new SqlConnection(eip))
            {
                string sql = @"INSERT INTO [item_apply_custodianlist] ([apply_id],[custodian_name],[custodian_quantity],[creatDate],[creatUser])
                               VALUES
                               (@apply_id,@custodian_name,@custodian_quantity,@creatDate,@creatUser)";

                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@apply_id", id);
                    cmd.Parameters.AddWithValue("@custodian_name", custodian_name.Text);
                    cmd.Parameters.AddWithValue("@custodian_quantity", custodian_quantity.Text);
                    cmd.Parameters.AddWithValue("@creatDate", DateTime.Now);
                    cmd.Parameters.AddWithValue("@creatUser", userName);
                    cn.Open();
                    cmd.ExecuteNonQuery();


                }
            }
            custodian_name.Text = "";
            custodian_quantity.Text = "";


            custodian_gv_BindData();

        }
    }

   
}