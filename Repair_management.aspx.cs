using NPOI.HSSF.Record.Formula.Functions;
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

public partial class Repair_management : System.Web.UI.Page
{
    string eip = WebConfigurationManager.ConnectionStrings["eip"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        Session["right"] = "1";

        if (!IsPostBack)
        {
            MasterPage2 master = (MasterPage2)this.Master;
            Label masterLabel = (Label)master.FindControl("page_title");
            masterLabel.Text = "<div class='TitleFrame'> 位置管理  <div style='margin-left: 20px;' class=\"subTitleMarkText\">註：樓層及位置可交叉使用，如行政大樓11F(樓層)電梯口(位置)</div></div>";
            Literal link_li = (Literal)master.FindControl("link_li");
            if (link_li != null) link_li.Text += "<li>位置管理</li>";
            p_BindData();
            f1_BindData();
            f2_BindData();
            l_BindData();
        }

    }
    private void p_BindData()
    {
        p_gv.DataSource = Getplace();
        p_gv.DataBind();
    }
    private void f1_BindData()
    {
        f1_gv.DataSource = Getfloor("1");
        f1_gv.DataBind();
    }
    private void f2_BindData()
    {
        f2_gv.DataSource = Getfloor("2");
        f2_gv.DataBind();
    }
    private void l_BindData()
    {
        l_gv.DataSource = Getlocation();
        l_gv.DataBind();
    }
    private DataTable Getplace()
    {

        using (SqlConnection cn = new SqlConnection(eip))
        {
            cn.Open();
            string sql = @"SELECT [place_id],[place_name]  FROM [repair_place] order by place_id ASC";
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }
    }
    private DataTable Getfloor(string floor)
    {

        using (SqlConnection cn = new SqlConnection(eip))
        {
            cn.Open();
            string sql = @"SELECT [id],[place_id],[floor_name]  FROM [repair_floor] where place_id=@place_id order by id ASC";
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.AddWithValue("@place_id", floor);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }
    }
    private DataTable Getlocation()
    {

        using (SqlConnection cn = new SqlConnection(eip))
        {
            cn.Open();
            string sql = @"SELECT [id],[Location_name]  FROM [repair_location] order by id ASC";
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
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
    protected void f1_gv_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Remove")
        {
            // 取得要移除的項目ID
            string id = e.CommandArgument.ToString();
            Deletefloor(id);
            f1_BindData();
        }
    }
    protected void f1_gv_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        // 更新當前頁索引
        f1_gv.PageIndex = e.NewPageIndex;

        // 重新綁定資料
        f1_BindData();
    }
    protected void f2_gv_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Remove")
        {
            // 取得要移除的項目ID
            string id = e.CommandArgument.ToString();
            Deletefloor(id);
            f2_BindData();
        }
    }
    protected void f2_gv_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        // 更新當前頁索引
        f2_gv.PageIndex = e.NewPageIndex;

        // 重新綁定資料
        f2_BindData();
    }
    protected void l_gv_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Remove")
        {
            // 取得要移除的項目ID
            string id = e.CommandArgument.ToString();
            Deletelocation(id);
            l_BindData();
        }
    }
    protected void l_gv_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        // 更新當前頁索引
        l_gv.PageIndex = e.NewPageIndex;

        // 重新綁定資料
        l_BindData();
    }
    private void DeleteItem(string id)
    {

        using (SqlConnection cn = new SqlConnection(eip))
        {
            cn.Open();
            string sql = @"DELETE FROM repair_place WHERE place_id = @id";
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.Add(new SqlParameter("@id", id));

                cmd.ExecuteNonQuery();
            }
        }
    }
    private void Deletefloor(string id)
    {

        using (SqlConnection cn = new SqlConnection(eip))
        {
            cn.Open();
            string sql = @"DELETE FROM repair_floor WHERE id = @id";
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.Add(new SqlParameter("@id", id));

                cmd.ExecuteNonQuery();
            }
        }
    }

    private void Deletelocation(string id)
    {

        using (SqlConnection cn = new SqlConnection(eip))
        {
            cn.Open();
            string sql = @"DELETE FROM repair_location WHERE id = @id";
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.Add(new SqlParameter("@id", id));

                cmd.ExecuteNonQuery();
            }
        }
    }
    //protected void Addplace_Click(object sender, EventArgs e)
    //{
    //    string place_name = "";
    //    if (string.IsNullOrWhiteSpace(place.Text))
    //    {
    //        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('未填品名');", true);
    //        return;
    //    }
    //    using (SqlConnection cn = new SqlConnection(eip))
    //    {
    //        cn.Open();
    //        string sql = @"select place_name from  repair_place where place_name=@place_name";

    //        SqlCommand cmd = new SqlCommand(sql, cn);
    //        cmd.Parameters.AddWithValue("@place_name", place.Text);

    //        SqlDataReader dr = cmd.ExecuteReader();

    //        if (dr.Read())
    //        {
    //            place_name = dr["place_name"].ToString();
    //        }
    //        cn.Close();
    //    }
    //    if (place_name == place.Text)
    //    {
    //        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('品名重複');", true);
    //        return;
    //    }
    //    using (SqlConnection cn = new SqlConnection(eip))
    //    {
    //        string sql = @"insert into repair_place (place_name) values (@place_name)";

    //        SqlCommand cmd = new SqlCommand(sql, cn);

    //        cmd.Parameters.AddWithValue("@place_name", place.Text);

    //        cn.Open();
    //        cmd.ExecuteNonQuery();
    //        cn.Close();

    //    }
    //    place.Text = "";
    //    p_BindData();
    //}

    protected void Addfloor1_Click(object sender, EventArgs e)
    {
        string floor_name = "";
        if (string.IsNullOrWhiteSpace(floor.Text))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('未填樓層');", true);
            return;
        }
        else
        {
            //int length = GetMixedStringLength(floor.Text);
            if (floor.Text.Length > 20)
            {
                ErrorFormatContent.Text = "超出字數限制，最多為20個字";
                floor.Text = "";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "", "$('#ErrorFormatShow_alert').modal('show');", true);
                return;
            }
        }

        using (SqlConnection cn = new SqlConnection(eip))
        {
            cn.Open();
            string sql = @"select floor_name from  repair_floor where floor_name=@floor_name and place_id=1";

            SqlCommand cmd = new SqlCommand(sql, cn);
            cmd.Parameters.AddWithValue("@floor_name", floor.Text);

            SqlDataReader dr = cmd.ExecuteReader();

            if (dr.Read())
            {
                floor_name = dr["floor_name"].ToString();
            }
            cn.Close();
        }
        if (floor_name == floor.Text)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('樓層重複');", true);
            return;
        }
        using (SqlConnection cn = new SqlConnection(eip))
        {
            string sql = @"insert into repair_floor (floor_name,place_id) values (@floor_name,'1')";

            SqlCommand cmd = new SqlCommand(sql, cn);

            cmd.Parameters.AddWithValue("@floor_name", floor.Text);

            cn.Open();
            cmd.ExecuteNonQuery();
            cn.Close();
        }
        floor.Text = "";
        f1_BindData();
    }
    protected void Addfloor2_Click(object sender, EventArgs e)
    {
        string floor_name = "";
        if (string.IsNullOrWhiteSpace(floor2.Text))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('未填樓層');", true);
            return;
        }
        else
        {
            //int length = GetMixedStringLength(floor2.Text);
            if (floor2.Text.Length > 20)
            {
                ErrorFormatContent.Text = "超出字數限制，最多為20個字";
                floor2.Text = "";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "", "$('#ErrorFormatShow_alert').modal('show');", true);
                return;
            }
        }
        using (SqlConnection cn = new SqlConnection(eip))
        {
            cn.Open();
            string sql = @"select floor_name from  repair_floor where floor_name=@floor_name and place_id=2";

            SqlCommand cmd = new SqlCommand(sql, cn);
            cmd.Parameters.AddWithValue("@floor_name", floor2.Text);

            SqlDataReader dr = cmd.ExecuteReader();

            if (dr.Read())
            {
                floor_name = dr["floor_name"].ToString();
            }
            cn.Close();
        }
        if (floor_name == floor2.Text)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('樓層重複');", true);
            return;
        }
        using (SqlConnection cn = new SqlConnection(eip))
        {
            string sql = @"insert into repair_floor (floor_name,place_id) values (@floor_name,'2')";

            SqlCommand cmd = new SqlCommand(sql, cn);

            cmd.Parameters.AddWithValue("@floor_name", floor2.Text);

            cn.Open();
            cmd.ExecuteNonQuery();
            cn.Close();
        }
        floor2.Text = "";
        f2_BindData();
    }
    protected void Addlocation_Click(object sender, EventArgs e)
    {
        string location_name = "";
        if (string.IsNullOrWhiteSpace(location.Text))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('未填位置');", true);
            return;
        }
        else
        {
            //int length = GetMixedStringLength(location.Text);
            if (location.Text.Length > 20)
            {
                ErrorFormatContent.Text = "超出字數限制，最多為20個字";
                location.Text = "";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "", "$('#ErrorFormatShow_alert').modal('show');", true);
                return;
            }
        }
        using (SqlConnection cn = new SqlConnection(eip))
        {
            cn.Open();
            string sql = @"select Location_name from  repair_location where Location_name=@Location_name";

            SqlCommand cmd = new SqlCommand(sql, cn);
            cmd.Parameters.AddWithValue("@Location_name", location.Text);

            SqlDataReader dr = cmd.ExecuteReader();

            if (dr.Read())
            {
                location_name = dr["Location_name"].ToString();
            }
            cn.Close();
        }
        if (location_name == location.Text)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('位置重複');", true);
            return;
        }
        using (SqlConnection cn = new SqlConnection(eip))
        {
            string sql = @"insert into repair_location (Location_name) values (@Location_name)";

            SqlCommand cmd = new SqlCommand(sql, cn);

            cmd.Parameters.AddWithValue("@Location_name", location.Text);

            cn.Open();
            cmd.ExecuteNonQuery();
            cn.Close();

        }
        location.Text = "";
        l_BindData();
    }

    private int GetMixedStringLength(string input)
    {
        int length = 0;

        foreach (char c in input)
        {
            if (char.IsLetterOrDigit(c) || c == '-' || c == '_' || c == ' ')
            {
                length += 1; // 英文、數字、`-`、`_` 計 0.5 字
            }
            else if (char.GetUnicodeCategory(c) == System.Globalization.UnicodeCategory.OtherLetter)
            {
                length += 2; // 中文計 1 字
            }
        }

        return length / 2; // 因為英文 & 符號是 0.5 字，所以除以 2
    }

    protected string search_count(string mode, string id)
    {
        string result = "";
        if (mode == "floor")
        {
            using (SqlConnection cn = new SqlConnection(eip))
            {
                cn.Open();
                string sql = @"select t2.place_name from repair_floor t1 left join repair_place t2 on t1.place_id = t2.place_id where t1.id=@id";

                SqlCommand cmd = new SqlCommand(sql, cn);
                cmd.Parameters.AddWithValue("@id", id);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    result += dr["place_name"].ToString() + ",";
                }
                cn.Close();
            }
        }

        using (SqlConnection cn = new SqlConnection(eip))
        {
            cn.Open();
            string sql = @"select MAX(t2.place_name) as place_name,MAX(t3.floor_name) as floor_name,MAX(t4.Location_name) as Location_name,count(t1.id) as count 
                  FROM [repair_apply] t1 
                  left join repair_place t2 on t1.place_id = t2.place_id 
                  left join repair_floor as t3 on t1.floor_id=t3.id 
                  left join [repair_location] as t4 on t1.location_id=t4.id  where ";
            sql += (mode == "floor") ? "floor_id" : "location_id";
            sql += "=@id";

            SqlCommand cmd = new SqlCommand(sql, cn);
            cmd.Parameters.AddWithValue("@id", id);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                result += dr["count"].ToString();
            }
            cn.Close();
        }
        return result;
    }

    protected void modal2_alert_del_Click(object sender, EventArgs e)
    {
        Button button = (Button)sender;
        search_count("floor", button.CommandArgument);
        modal2_building.Text = search_count("floor", button.CommandArgument).Split(',')[0];
        modal2_floor.Text = button.CommandName;
        modal2_num.Text = search_count("floor", button.CommandArgument).Split(',')[1];
        modal2_del.CommandArgument = button.CommandArgument;
        if (modal2_num.Text != "0")
        {
            modal2_del.Enabled = false;
            modal2_del.Attributes.Add("style", "background-color:#777777;");
        }
        else
        {
            modal2_del.Enabled = true;
            modal2_del.Attributes.Add("style", "background-color:#B83F1F;");
        }
        ScriptManager.RegisterStartupScript(this, this.GetType(), "", "$('#modal2').modal('show');", true);
    }

    protected void modal3_alert_del_Click(object sender, EventArgs e)
    {
        Button button = (Button)sender;
        search_count("location", button.CommandArgument);
        modal3_location.Text = button.CommandName;
        modal3_num.Text = search_count("location", button.CommandArgument).Split(',')[0];
        modal3_del.CommandArgument = button.CommandArgument;
        if (modal3_num.Text != "0")
        {
            modal3_del.Enabled = false;
            modal3_del.Attributes.Add("style", "background-color:#777777;");
        }
        else
        {
            modal3_del.Enabled = true;
            modal3_del.Attributes.Add("style", "background-color:#B83F1F;");
        }
        ScriptManager.RegisterStartupScript(this, this.GetType(), "", "$('#modal3').modal('show');", true);
    }

    protected void modal2_del_Click(object sender, EventArgs e)
    {
        Button button = (Button)sender;
        using (SqlConnection cn = new SqlConnection(eip))
        {
            cn.Open();
            string sql = @"DELETE FROM repair_floor WHERE id = @id";
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.Add(new SqlParameter("@id", button.CommandArgument));

                cmd.ExecuteNonQuery();
            }
        }
        modal2_del.CommandArgument = "";
        f1_gv.DataSource = Getfloor("1");
        f1_gv.DataBind();
        f2_gv.DataSource = Getfloor("2");
        f2_gv.DataBind();
    }
    protected void modal3_del_Click(object sender, EventArgs e)
    {
        Button button = (Button)sender;
        using (SqlConnection cn = new SqlConnection(eip))
        {
            cn.Open();
            string sql = @"DELETE FROM repair_location WHERE id = @id";
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.Add(new SqlParameter("@id", button.CommandArgument));

                cmd.ExecuteNonQuery();
            }
        }
        modal3_del.CommandArgument = "";
        l_gv.DataSource = Getlocation();
        l_gv.DataBind();
    }
}