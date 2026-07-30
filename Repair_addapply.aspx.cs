using NPOI.HSSF.Record.Formula.Functions;
using NPOI.OpenXmlFormats.Dml.Diagram;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Repair_addapply : System.Web.UI.Page
{
    string eip = WebConfigurationManager.ConnectionStrings["eip"].ConnectionString;
    private string id;
    private string m;
    private string r_no;
    private string state;
    private string inventory;
    protected void Page_Load(object sender, EventArgs e)
    {
        id = Request.QueryString["id"];
        m = Request.QueryString["m"];//0新增 1編輯 
        r_no = Request.QueryString["r_no"];
        state = Request.QueryString["state"];
        inventory = Request.QueryString["inventory"];
        string ctrlName = "";
        MasterPage2 master = (MasterPage2)this.Master;
        Label masterLabel = (Label)master.FindControl("page_title");
        masterLabel.Text = "我的申請";
        Literal link_li = (Literal)master.FindControl("link_li");


        if (Session["user_right_id"] == null)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "login_erro", "alert('登入逾時，請重新登入!');location.href='Login.aspx';", true);
        }

        //未知作用，在此程式中也沒其他地方引用，先註解掉避免登入逾時問題
        //if (Session["repair_show_page"] == null)
        //{
        //    ScriptManager.RegisterStartupScript(this, this.GetType(), "login_erro", "alert('登入逾時，請重新登入!');location.href='Login.aspx';", true);
        //}

        if (!IsPostBack)
        {

            if (link_li != null) link_li.Text += "<li>我的申請</li>";
            apply_date.Text = DateTime.Today.ToString("yyyy-MM-dd");
            apply_group.Text = Session["group_name"].ToString();
            apply_user.Text = Session["user_name"].ToString();

            getfloor("1");
            getlocation();
            getrepairno();

            if (m == "0")
            {
                statechange.Visible = false;
                treattime.Visible = false;
                Submit.Visible = true;
                save.Visible = false;
            }
            else if (m == "1")
            {
                masterLabel.Text = "申請審核";
                rb1.Enabled = false;
                rb2.Enabled = false;
                floor.Enabled = false;
                location.Enabled = false;
                reason.Enabled = false;
                if (state == "1" || state == "2")
                {

                    if (Session["user_right_id"].ToString() != "1")
                    {
                        Submit.Visible = false;
                        Cancel.Text = "返回";
                        rb1.Enabled = false;
                        rb2.Enabled = false;
                        floor.Enabled = false;
                        location.Enabled = false;
                        reason.Enabled = false;
                        Finish_date.Enabled = false;
                        repair_note.Enabled = false;
                        add_pl.Enabled = false;
                        add.Enabled = false;
                        save.Visible = false;
                    }
                    else
                    {
                        save.Visible = true;

                    }

                    if (state == "1")
                        TempSave.Visible = true;
                    else
                        TempSave.Visible = false;

                    if (state == "2")
                    {
                        save.Visible = false;
                        Finish_date.Enabled = false;
                        repair_note.Enabled = false;
                        add_pl.Enabled = false;
                        add.Visible = false;

                    }
                    statechange.Enabled = false;
                    treattime.Visible = true;
                    pl.Visible = true;
                    Submit.Visible = false;

                    showlog();
                }
                else
                {
                   
                    Submit.Visible = false;
                    Cancel.Text = "返回";
                    rb1.Enabled = false;
                    rb2.Enabled = false;
                    floor.Enabled = false;
                    location.Enabled = false;
                    reason.Enabled = false;
                    Finish_date.Enabled = false;
                    if (Session["user_right_id"].ToString() != "1")
                    {
                        statechange.Enabled = false;
                        
                    }
                    else { statechange.Enabled = true; save.Visible = true; }
                    save.Visible = false;
                    repair_note.Enabled = false;
                    add_pl.Enabled = false;
                    add.Enabled = false;
                  //  treattime.Visible = true;
                    pl.Visible = false;
                    Submit.Visible = false;

                    showlog();
                }
                if (state == "3")
                {
                    save.Visible = false;
                }


                statechange.Visible = true;

                if (Session["user_right_id"].ToString() != "1")
                {
                    statechange.Visible = false;
                    TempSave.Visible = false;
                }



                showdata();
            }
            statechange.Text = state == "0" ? "處理" : "處理中";
            if (state == "2")
            {
                statechange.Text = "已完成";
                statechange.Visible = false;
            }
            else if (state != "0") statechange.Visible = false;//處理中隱藏
        }
        else
        {
            ctrlName = this.Request.Form["__EVENTTARGET"].Replace("ctl00$", "");

            if (ctrlName == "ContentPlaceHolder1$add")
                hf_add.Value = (int.Parse(hf_add.Value) + 1).ToString();
            showdefect();

        }
    }
    private void showdata()
    {
        using (SqlConnection cn = new SqlConnection(eip))
        {
            cn.Open();
            string sql = @"SELECT  [id],[repair_no],[apply_group],[apply_user],[apply_date],[place_id],[floor_id],[location_id],[apply_reason],[filepath1],[filepath2],[state],[updateDate] 
                                    ,[finish_user],[finish_date],[Handle_note]
                                    FROM [repair_apply]
                                    where id=@id";
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.AddWithValue("@id", id);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    DateTime applyDate = dr.GetDateTime(dr.GetOrdinal("apply_date"));
                    apply_date.Text = applyDate.ToString("yyyy-MM-dd");
                    apply_group.Text = dr["apply_group"].ToString();
                    apply_user.Text = dr["apply_user"].ToString();
                    if (dr["place_id"].ToString() == "1")
                    {
                        rb1.Checked = true;
                    }
                    else
                    {
                        rb2.Checked = true;
                    }
                    floor.SelectedValue = dr["floor_id"].ToString();
                    location.SelectedValue = dr["location_id"].ToString();
                    reason.Text = dr["apply_reason"].ToString();
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
                    p1.Visible = false;
                    p2.Visible = false;


                    Finish_user.Text = !string.IsNullOrEmpty(dr["finish_user"].ToString()) ? dr["finish_user"].ToString() : Session["user_name"].ToString();
                    if (!string.IsNullOrEmpty(dr["finish_date"].ToString()))
                    {
                        DateTime finish_date = Convert.ToDateTime(dr["finish_date"]);
                        Finish_date.Text = finish_date.ToString("yyyy-MM-dd");
                    }
                    repair_note.Text = dr["Handle_note"].ToString();
                    if (!string.IsNullOrEmpty(dr["updateDate"].ToString()))
                    {
                        DateTime treat = Convert.ToDateTime(dr["updateDate"]);
                        treattime.Text += treat.ToString("yyyy/MM/dd hh:mm tt").ToLower();
                    }

                }
            }
        }
    }
    private void showlog()
    {
        int rowCount = 0;
        List<string> date = new List<string>();
        List<string> no = new List<string>();
        List<string> name = new List<string>();
        List<string> sp = new List<string>();
        List<string> nb = new List<string>();
        List<string> id = new List<string>();
        List<string> total = new List<string>();
        using (SqlConnection cn = new SqlConnection(eip))
        {
            cn.Open();
            string sql = @"SELECT t1.[id]
                              ,[repair_no]
                              ,[receivedate]
                              ,t1.[materials_no]
                              ,t1.[materials_name]
	                          ,t2.specification
                              ,[repair_place]
                              ,[number]
                          FROM [repair_materials_log] as t1
                          left join repair_materials as t2 on t1.materials_name=t2.materials_name
                      where repair_no=@repair_no";
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.AddWithValue("@repair_no", r_no);
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    DateTime receivedate = dr.GetDateTime(dr.GetOrdinal("receivedate"));
                    date.Add(receivedate.ToString("yyyy-MM-dd"));
                    no.Add(dr["materials_no"].ToString());
                    name.Add(dr["materials_name"].ToString());
                    sp.Add(dr["specification"].ToString());
                    nb.Add(dr["number"].ToString());
                    id.Add(dr["id"].ToString());
                    rowCount++;
                }
            }
        }
        hf_add.Value = rowCount.ToString();
        showdefect();
        for (int i = 0; i < rowCount; i++)
        {
            Panel pa_defects = (Panel)add_pl.FindControl("pa_defects_" + i);
            DropDownList materials_no = (DropDownList)pa_defects.FindControl("materials_no_" + i);
            TextBox adddate = (TextBox)pa_defects.FindControl("adddate" + i);
            TextBox materials_name = (TextBox)pa_defects.FindControl("materials_name" + i);
            TextBox specificationTextBox = (TextBox)pa_defects.FindControl("specification" + i);
            TextBox count = (TextBox)pa_defects.FindControl("count" + i);
            HiddenField hd_id = (HiddenField)pa_defects.FindControl("hd_id" + i);
            adddate.Text = date[i];
            materials_no.SelectedValue = no[i];
            materials_name.Text = name[i];
            specificationTextBox.Text = sp[i];
            count.Text = nb[i];
            hd_id.Value = id[i];
        }

    }
    private void showdefect()//領用物品
    {

        int sum = int.Parse(hf_add.Value);
        for (int i = 0; i < sum; i++)
        {
            Panel pa_defects = new Panel();
            pa_defects.ID = "pa_defects_" + i.ToString();
            pa_defects.CssClass = "d-flex align-items-baseline";
            TextBox adddate = new TextBox { ID = "adddate" + i, CssClass = "form-control", TextMode = TextBoxMode.Date };
            adddate.Attributes.Add("style", "width: 150px; height: 35px; margin:0px 10px;");
            DropDownList materials_no = new DropDownList { ID = "materials_no_" + i, CssClass = "form-control" };
            LoadMaterials(materials_no);
            materials_no.Attributes.Add("style", "width: 180px; height: 35px;margin:0px 10px;");
            materials_no.AutoPostBack = true;
            materials_no.SelectedIndexChanged += new EventHandler(MaterialsNo_SelectedIndexChanged);
            Label totallb = new Label { ID = "totallb" + i };
            totallb.Attributes.Add("style", "font-size:12px;position: absolute;");
            TextBox materials_name = new TextBox { ID = "materials_name" + i, CssClass = "form-control", Enabled = false };
            materials_name.Attributes.Add("style", "width: 100px; height: 35px; margin:10px 10px;");
            TextBox specification = new TextBox { ID = "specification" + i, CssClass = "form-control", Enabled = false };
            specification.Attributes.Add("style", "width: 100px; height: 35px; margin:10px 10px;");
            TextBox count = new TextBox { ID = "count" + i, CssClass = "form-control" };
            count.TextMode = TextBoxMode.Number;
            count.Attributes.Add("style", "width: 100px; height: 35px; margin:10px 10px;");
            HiddenField id = new HiddenField { ID = "hd_id" + i };
            Button del = new Button { ID = "del" + i, CssClass = "delbt", Text = "刪除", OnClientClick = "return confirm('確定要刪除嗎？');" };
            del.CommandName = pa_defects.ID;
            //del.UseSubmitBehavior = false;
            del.Click += bt_dele_Click;
            if (state == "2")
            {
                del.Visible = false;
            }
            pa_defects.Controls.Add(new LiteralControl("<div class='d-flex flex-wrap' style='width: 100%;align-items: center;'>"));
            pa_defects.Controls.Add(new LiteralControl("<div>"));
            pa_defects.Controls.Add(new LiteralControl("<span>日期</span>"));
            pa_defects.Controls.Add(adddate);
            pa_defects.Controls.Add(new LiteralControl("</div>"));
            pa_defects.Controls.Add(new LiteralControl("<div style='display: flex;align-items: center;'>"));
            pa_defects.Controls.Add(new LiteralControl("<span>料號</span>"));
            pa_defects.Controls.Add(new LiteralControl("<div class='d-flex flex-column align-items-center' style='width: 180px;margin: 0px 10px;'>"));
            pa_defects.Controls.Add(materials_no);
            pa_defects.Controls.Add(new LiteralControl("<span style='font-size:12px;align-self: flex-start;'>"));
            pa_defects.Controls.Add(totallb);
            pa_defects.Controls.Add(new LiteralControl("</span>"));
            pa_defects.Controls.Add(new LiteralControl("</div>"));
            pa_defects.Controls.Add(new LiteralControl("</div>"));
            pa_defects.Controls.Add(new LiteralControl("<div>"));
            pa_defects.Controls.Add(new LiteralControl("<span>名稱</span>"));
            pa_defects.Controls.Add(materials_name);
            pa_defects.Controls.Add(new LiteralControl("</div>"));
            pa_defects.Controls.Add(new LiteralControl("<div>"));
            pa_defects.Controls.Add(new LiteralControl("<span>規格</span>"));
            pa_defects.Controls.Add(specification);
            pa_defects.Controls.Add(new LiteralControl("</div>"));
            pa_defects.Controls.Add(new LiteralControl("<div>"));
            pa_defects.Controls.Add(new LiteralControl("<span>數量</span>"));
            pa_defects.Controls.Add(count);
            pa_defects.Controls.Add(id);
            pa_defects.Controls.Add(del);
            pa_defects.Controls.Add(new LiteralControl("</div>"));
            pa_defects.Controls.Add(new LiteralControl("</div>"));


            HtmlGenericControl hr = new HtmlGenericControl("hr");
            hr.ID = "hr" + i.ToString();
            hr.Attributes.Add("style", "width: 100%;");
            add_pl.Controls.Add(pa_defects);
            add_pl.Controls.Add(hr);

        }
    }
    protected void bt_dele_Click(object sender, EventArgs e)
    {
        Button bt = (Button)sender;
        string fullId = bt.CommandName;
        string[] parts = fullId.Split('_');
        string id = parts[parts.Length - 1];
        Panel pa_defects = (Panel)add_pl.FindControl(bt.CommandName);
        HtmlGenericControl hr = (HtmlGenericControl)add_pl.FindControl("hr" + id);
        DropDownList materials_no = (DropDownList)pa_defects.FindControl("materials_no_" + id);
        TextBox count = (TextBox)pa_defects.FindControl("count" + id);
        HiddenField hd_id = (HiddenField)pa_defects.FindControl("hd_id" + id);
        if (pa_defects != null)
        {
            if (state == "2")
            {
                if (string.IsNullOrEmpty(materials_no.Text) || string.IsNullOrEmpty(count.Text))
                {
                    pa_defects.Visible = false;
                    hr.Visible = false;
                    return;
                }
                using (SqlConnection cn = new SqlConnection(eip))
                {
                    int total = 0;
                    int p1 = 0; int p2 = 0;

                    cn.Open();
                    string sql = @"SELECT  [place1_inventory],[place2_inventory],[total_inventory] FROM [repair_materials] where materials_no=@materials_no ";
                    using (SqlCommand cmd = new SqlCommand(sql, cn))
                    {
                        cmd.Parameters.AddWithValue("@materials_no", materials_no.SelectedValue);
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr.Read())
                        {
                            p1 = int.Parse(dr["place1_inventory"].ToString());
                            p2 = int.Parse(dr["place2_inventory"].ToString());
                            total = int.Parse(dr["total_inventory"].ToString());
                        }
                        dr.Close();
                    }
                    string sql2 = "";
                    if (rb1.Checked)
                    {
                        p1 = p1 + int.Parse(count.Text);
                        total = total + int.Parse(count.Text);
                        sql2 = @"update [repair_materials] set place1_inventory=@place1_inventory,total_inventory=@total_inventory where materials_no=@materials_no ";
                        using (SqlCommand cmd = new SqlCommand(sql2, cn))
                        {
                            cmd.Parameters.AddWithValue("@materials_no", materials_no.SelectedValue);
                            cmd.Parameters.AddWithValue("@place1_inventory", p1);
                            cmd.Parameters.AddWithValue("@total_inventory", total);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    else if (rb2.Checked)
                    {
                        p2 = p2 + int.Parse(count.Text);
                        total = total + int.Parse(count.Text);
                        sql2 = @"update [repair_materials] set  place2_inventory=@place2_inventory,total_inventory=@total_inventory where materials_no=@materials_no ";
                        using (SqlCommand cmd = new SqlCommand(sql2, cn))
                        {
                            cmd.Parameters.AddWithValue("@materials_no", materials_no.SelectedValue);
                            cmd.Parameters.AddWithValue("@place2_inventory", p2);
                            cmd.Parameters.AddWithValue("@total_inventory", total);
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                using (SqlConnection cn = new SqlConnection(eip))
                {
                    cn.Open();
                    string sql = @"delete repair_materials_log where id=@id";
                    using (SqlCommand cmd = new SqlCommand(sql, cn))
                    {
                        cmd.Parameters.AddWithValue("@id", hd_id.Value);
                        cmd.ExecuteNonQuery();
                    }
                }
                Response.Redirect(Request.Url.ToString(), true);

            }
        }
        pa_defects.Visible = false;
        hr.Visible = false;
    }
    public void LoadMaterials(DropDownList ddlMaterials)
    {
        ddlMaterials.Items.Clear();
        ddlMaterials.Items.Add(new ListItem("請選擇", "0"));
        using (SqlConnection cn = new SqlConnection(eip))
        {
            cn.Open();
            string sql = @"SELECT  [id],[materials_no],[materials_name],[specification]  FROM [repair_materials]";
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    ddlMaterials.Items.Add(new ListItem(dr["materials_no"].ToString() + "/" + dr["materials_name"].ToString() + "/" + dr["specification"].ToString(), dr["materials_no"].ToString()));
                }

            }
        }
    }
    protected void MaterialsNo_SelectedIndexChanged(object sender, EventArgs e)
    {        
        DropDownList materials_no = (DropDownList)sender;
        string currentId = materials_no.ID.Substring(materials_no.ID.IndexOf("no_") + 3);
        
        Panel pa_defects = (Panel)add_pl.FindControl("pa_defects_" + currentId);
        
        if (pa_defects != null)
        {
            TextBox materialsName = (TextBox)pa_defects.FindControl("materials_name" + currentId);//領用物品名稱
            TextBox specificationTextBox = (TextBox)pa_defects.FindControl("specification" + currentId);//領用物品規格
            
            Label totallb = (Label)pa_defects.FindControl("totallb" + currentId);
            using (SqlConnection cn = new SqlConnection(eip))
            {
                cn.Open();
                string sql = @"SELECT [id]
                              ,[materials_no]
                              ,[materials_name]
                              ,[specification]
                              ,[safe_inventory]
                              ,[place1_inventory]
                              ,[place2_inventory]
                              ,[total_inventory]
                              ,[updateDate]
                          FROM [repair_materials]
                          where materials_no=@materials_no";
                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@materials_no", materials_no.SelectedValue);
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        materialsName.Text = dr["materials_name"].ToString();
                        specificationTextBox.Text = dr["specification"].ToString();
                        int total_inventory = !string.IsNullOrEmpty(dr["total_inventory"].ToString()) ? int.Parse(dr["total_inventory"].ToString()):0;
                        int pl1 = int.Parse(dr["place1_inventory"].ToString());
                        int pl2 = int.Parse(dr["place2_inventory"].ToString());

                        if (materials_no.SelectedValue != "0")
                        {
                            totallb.Text = "本物品庫存:<span style='color:#145597;'>" + total_inventory.ToString() + "</span>(" + dr["safe_inventory"].ToString() + ")";
                        }

                    }

                }

            }
            if (materials_no.SelectedValue == "0")
            {
                totallb.Text = " ";
            }
        }

    }
    private void getrepairno()
    {
        DateTime now = DateTime.Today;
        int twYear = now.Year - 1911; // 民國年

        int nextno = 1;
        using (SqlConnection cn = new SqlConnection(eip))
        {
            cn.Open();
            string sql = @"SELECT  RIGHT([repair_no], 4) AS current_serial FROM [repair_apply] 
                            WHERE 
                                [repair_no] LIKE 'R' + @date +'%'
                            ORDER BY 
                                current_serial DESC";
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.AddWithValue("@date", twYear.ToString("D3") + now.ToString("MMdd"));
                var result = cmd.ExecuteScalar();

                if (result != null)
                {
                    int currentSerial = Convert.ToInt32(result);
                    nextno = currentSerial + 1;
                }

            }
        }
        repairno.Text = "R" + twYear.ToString("D3") + now.ToString("MMdd") + nextno.ToString("D4");
    }
    private void getlocation()
    {
        location.Items.Clear();
        location.Items.Insert(0, new ListItem("請選擇", "0"));
        using (SqlConnection cn = new SqlConnection(eip))
        {
            cn.Open();
            string sql = @"SELECT [id],[Location_name]  FROM [repair_location] order by id ASC";
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    location.Items.Add(new ListItem(dr["Location_name"].ToString(), dr["id"].ToString()));
                }

            }
        }
    }
    private void getfloor(string place)
    {
        floor.Items.Clear();
        floor.Items.Insert(0, new ListItem("請選擇", "0"));
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
                    floor.Items.Add(new ListItem(dr["floor_name"].ToString(), dr["id"].ToString()));
                }

            }
        }
    }

    protected void rb_CheckedChanged(object sender, EventArgs e)
    {
        if (rb1.Checked)
        {
            getfloor("1");
        }
        else if (rb2.Checked)
        {
            getfloor("2");
        }
    }




    protected void md2submit_Click(object sender, EventArgs e)
    {
        string repair_no = repairno.Text;
        string applyGroup = apply_group.Text;
        string applyUser = apply_user.Text;
        string place = rb1.Checked ? "1" : "2";

        // 儲存檔案的路徑
        string filePath1 = string.Empty;
        string filePath2 = string.Empty;
        string virtualPath1 = string.Empty;
        string virtualPath2 = string.Empty;
        if (m == "0")
        {
            // 檢查第一個檔案上傳
            if (FileUpload1.HasFile)
            {
                string fileName1 = Path.GetFileName(FileUpload1.FileName);
                string folderPath = Server.MapPath("~/Repairphoto/");
                string fileExtension = Path.GetExtension(fileName1).ToLower();
                if (fileExtension != ".jpg" && fileExtension != ".png")
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('只允許上傳 JPG 或 PNG 檔案。');", true);
                    return;
                }
                filePath1 = Path.Combine(folderPath, fileName1);
                FileUpload1.SaveAs(filePath1);
                virtualPath1 = ResolveUrl("~/Repairphoto/" + fileName1);

            }

            // 檢查第二個檔案上傳
            if (FileUpload2.HasFile)
            {
                string fileName2 = Path.GetFileName(FileUpload2.FileName);
                string folderPath = Server.MapPath("~/Repairphoto/");
                string fileExtension = Path.GetExtension(fileName2).ToLower();
                if (fileExtension != ".jpg" && fileExtension != ".png")
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('只允許上傳 JPG 或 PNG 檔案。');", true);
                    return;
                }
                filePath2 = Path.Combine(folderPath, fileName2);
                FileUpload2.SaveAs(filePath2);
                virtualPath2 = ResolveUrl("~/Repairphoto/" + fileName2);
            }

            if (reason.Text.Length > 30)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "reason", "alert('修繕事由最多30字元');", true);
                return;
            }
            else
            {
                using (SqlConnection cn = new SqlConnection(eip))
                {
                    cn.Open();
                    string sql = @"INSERT INTO repair_apply (repair_no, apply_group, apply_user, apply_date, place_id, floor_id, location_id, apply_reason,filepath1,filepath2,creatDate,creatUser,state) 
                            VALUES (@repair_no, @apply_group, @apply_user, @apply_date, @place_id, @floor_id, @location_id, @apply_reason,@filepath1,@filepath2,@creatDate,@creatUser,@state)";

                    using (SqlCommand cmd = new SqlCommand(sql, cn))
                    {
                        cmd.Parameters.AddWithValue("@repair_no", repair_no);
                        cmd.Parameters.AddWithValue("@apply_group", applyGroup);
                        cmd.Parameters.AddWithValue("@apply_user", applyUser);
                        cmd.Parameters.AddWithValue("@apply_date", apply_date.Text);
                        cmd.Parameters.AddWithValue("@place_id", place);
                        cmd.Parameters.AddWithValue("@floor_id", floor.SelectedValue);
                        cmd.Parameters.AddWithValue("@location_id", location.SelectedValue);
                        cmd.Parameters.AddWithValue("@apply_reason", reason.Text);
                        cmd.Parameters.AddWithValue("@filepath1", virtualPath1);
                        cmd.Parameters.AddWithValue("@filepath2", virtualPath2);
                        cmd.Parameters.AddWithValue("@creatDate", DateTime.Now);
                        cmd.Parameters.AddWithValue("@creatUser", applyUser);
                        cmd.Parameters.AddWithValue("@state", 0);
                        cmd.ExecuteNonQuery();
                    }
                }
            }

        }
        else if (m == "1")
        {
            int rowCount = 0;
            using (SqlConnection cn = new SqlConnection(eip))
            {
                cn.Open();
                string sql = @"SELECT t1.[id]
                              ,[repair_no]
                              ,[receivedate]
                              ,t1.[materials_no]
                              ,t1.[materials_name]
	                          ,t2.specification
                              ,[repair_place]
                              ,[number]
                          FROM [repair_materials_log] as t1
                          left join repair_materials as t2 on t1.materials_name=t2.materials_name
                      where repair_no=@repair_no";
                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@repair_no", r_no);
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        rowCount++;
                    }
                }
            }



            for (int i = rowCount; i < int.Parse(hf_add.Value); i++)
            {

                Panel pa_defects = (Panel)add_pl.FindControl("pa_defects_" + i);
                TextBox adddate = (TextBox)pa_defects.FindControl("adddate" + i);
                DropDownList materials_no = (DropDownList)pa_defects.FindControl("materials_no_" + i);
                TextBox materialsName = (TextBox)pa_defects.FindControl("materials_name" + i);
                TextBox count = (TextBox)pa_defects.FindControl("count" + i);
                if (pa_defects.Visible)
                {
                    using (SqlConnection cn = new SqlConnection(eip))
                    {
                        int o = 0;
                        int savecount = 0;
                        int p1_count = 0, p2_count = 0;
                        int total_count = 0;
                        cn.Open();
                        string sql = @"SELECT [id]
                                  ,[materials_no]
                                  ,[materials_name]
                                  ,[safe_inventory]
                                  ,[place1_inventory]
                                  ,[place2_inventory]
                                  ,[total_inventory]
                                  ,[updateDate]
                              FROM [repair_materials] where materials_no=@materials_no and materials_name=@materials_name";
                        using (SqlCommand cmd = new SqlCommand(sql, cn))
                        {
                            cmd.Parameters.AddWithValue("@materials_no", materials_no.SelectedValue);
                            cmd.Parameters.AddWithValue("@materials_name", materialsName.Text);
                            SqlDataReader dr = cmd.ExecuteReader();
                            if (dr.Read())
                            {
                                savecount = !string.IsNullOrEmpty(dr["safe_inventory"].ToString()) ? int.Parse(dr["safe_inventory"].ToString()) : 0;
                                p1_count = int.Parse(dr["place1_inventory"].ToString());
                                p2_count = int.Parse(dr["place2_inventory"].ToString());
                                total_count = !string.IsNullOrEmpty(dr["total_inventory"].ToString()) ? int.Parse(dr["total_inventory"].ToString()) : 0; 
                            }
                            dr.Close();
                        }
                        if (place == "1" && int.TryParse(count.Text,out o))
                        {
                            p1_count = p1_count - int.Parse(count.Text);
                        }
                        else if (place == "2" && int.TryParse(count.Text, out o))
                        {
                            p2_count = p2_count - int.Parse(count.Text);
                        }
                        total_count = p1_count + p2_count;
                        if (savecount > total_count)
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('庫存不得小於安全庫存');", true);
                            return;
                        }
                        string sql2 = @"update [repair_materials] set place1_inventory=@place1_inventory,place2_inventory=@place2_inventory,total_inventory=@total_inventory where materials_no=@materials_no
                                    and materials_name=@materials_name";
                        using (SqlCommand cmd = new SqlCommand(sql2, cn))
                        {
                            cmd.Parameters.AddWithValue("@place1_inventory", p1_count);
                            cmd.Parameters.AddWithValue("@place2_inventory", p2_count);
                            cmd.Parameters.AddWithValue("@total_inventory", total_count);
                            cmd.Parameters.AddWithValue("@materials_no", materials_no.Text);
                            cmd.Parameters.AddWithValue("@materials_name", materialsName.Text);
                            cmd.ExecuteNonQuery();
                        }

                    }
                    using (SqlConnection cn = new SqlConnection(eip))
                    {
                        cn.Open();
                        string sql = @"INSERT INTO repair_materials_log (repair_no, receivedate, materials_no, materials_name, repair_place, number) 
                            VALUES (@repair_no, @receivedate, @materials_no, @materials_name, @repair_place, @number)";

                        using (SqlCommand cmd = new SqlCommand(sql, cn))
                        {
                            cmd.Parameters.AddWithValue("@repair_no", r_no);
                            cmd.Parameters.AddWithValue("@receivedate", adddate.Text);
                            cmd.Parameters.AddWithValue("@materials_no", materials_no.Text);
                            cmd.Parameters.AddWithValue("@materials_name", materialsName.Text);
                            cmd.Parameters.AddWithValue("@repair_place", place);
                            cmd.Parameters.AddWithValue("@number", count.Text);
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
            }

            if (repair_note.Text.Length > 30)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "reason", "alert('修繕處理說明最多80字元');", true);
                return;
            }
            else
            {
                using (SqlConnection cn = new SqlConnection(eip))
                {
                    cn.Open();

                    string sql = @"update [repair_apply] set finish_user=@finish_user,finish_date=@finish_date,Handle_note=@Handle_note,state=2 where id=@id";
                    using (SqlCommand cmd = new SqlCommand(sql, cn))
                    {
                        cmd.Parameters.AddWithValue("@finish_user", Finish_user.Text);
                        cmd.Parameters.AddWithValue("@finish_date", Finish_date.Text);
                        cmd.Parameters.AddWithValue("@Handle_note", repair_note.Text);
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.ExecuteNonQuery();
                    }

                }
            }

        }
        if (Session["user_right_id"].ToString() == "1")
        {
            Response.Redirect("Repair_management_query.aspx");
        }
        else
        {
            Response.Redirect("Repair_myapply.aspx");
        }
    }

    protected void Cancel_Click(object sender, EventArgs e)
    {
        if (inventory == "1")
        {
            Response.Redirect("Repair_inventory_list.aspx");
        }
        if (Session["user_right_id"].ToString() == "1")
        {
            Response.Redirect("Repair_management_query.aspx");
        }
        else
        {
            Response.Redirect("Repair_myapply.aspx");
        }

    }

    protected void add_Click(object sender, EventArgs e)
    {

    }

    protected void statechange_Click(object sender, EventArgs e)
    {
        using (SqlConnection cn = new SqlConnection(eip))
        {
            cn.Open();
            string sql = @"update [repair_apply] set state=1,updateDate=@updateDate,updateUser=@updateUser  where id=@id";

            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.AddWithValue("@updateDate", DateTime.Now);
                cmd.Parameters.AddWithValue("@updateUser", Session["user_name"].ToString());
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
        }
        string url = string.Format("Repair_addapply.aspx?m=1&id={0}&r_no={1}&state=1",
                                          HttpUtility.UrlEncode(id), HttpUtility.UrlEncode(r_no));
        Response.Redirect(url);
    }

    protected void TempSave_Click(object sender, EventArgs e)
    {
        using (SqlConnection cn = new SqlConnection(eip))
        {
            cn.Open();
            string sql = @"update [repair_apply] set finish_user = @finish_user, finish_date = @finish_date,Handle_note = @Handle_note  where id=@id";
            SqlCommand cmd = new SqlCommand(sql, cn);
            cmd.Parameters.AddWithValue("@finish_user", Finish_user.Text);
            cmd.Parameters.AddWithValue("@finish_date", Finish_date.Text);
            cmd.Parameters.AddWithValue("@Handle_note", repair_note.Text);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
        }
    }
}
