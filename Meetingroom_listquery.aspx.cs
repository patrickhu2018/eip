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
using System.Activities.Statements;
using System.Security.Cryptography;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Web.Services;

public partial class Meetingroom_listquery : System.Web.UI.Page
{
    string eip = WebConfigurationManager.ConnectionStrings["eip"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        MasterPage3 master = (MasterPage3)this.Master;
        Label masterLabel = (Label)master.FindControl("page_title");
        masterLabel.Text = "會議室使用狀況";


        Literal link_li = (Literal)master.FindControl("link_li");

        if (!IsPostBack)
        {

            //用部門名稱 取得部門編號
            using (SqlConnection cn = new SqlConnection(eip))
            {
                string sql_matchpeeID = @"select [no] FROM [group_name]   where [name]=@name ";
                SqlCommand cmd = new SqlCommand(sql_matchpeeID, cn);
                cmd.Parameters.AddWithValue("@name", Session["group_name"].ToString());
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    ViewState["group_no"] = dr["no"].ToString();
                }
                cn.Close();
            }

         


            ViewState["SortExpression"] = "use_start";
            ViewState["SortDirection"] = "DESC";
            if (link_li != null) link_li.Text += "<li>清單檢視</li>";
            
            if (Session["user_right_id"].ToString() != "1" && Session["user_right_id"].ToString() != "2" && Session["user_right_id"].ToString() != "3")
            {

                //export.Visible = false;
                group.Visible = true;
                key.Visible = false;

            }
            else
            {
                
                export.Visible = true;
                group.Visible = true;
                key.Visible = true;
            }
            

            meeting_type_bt.Visible = Session["user_right_id"].ToString() == "1"; //系統管理者可變更會議類型
            used_type_bt.Visible = Session["user_right_id"].ToString() == "1"; //系統管理者可變更使用類型



            getneetroom();
            gv_BindData();
            getddl();
            GenerateTimeOptions(timeSelectStart);

            GenerateTimeOptions(timeSelectEnd);

            #region 會議類型&使用類型
            ViewState["meeting_type"] = "meetingtype";
            show_meeting_item();
            #endregion

            if (Session["sunday"] != null && Session["sunday"].ToString()!="")
            {
                rb_customRange.Checked = true;
                receive_start.Text = DateTime.Parse(Session["sunday"].ToString()).ToString("yyyy-MM-dd");
                receive_end.Text = DateTime.Parse(Session["saturday"].ToString()).ToString("yyyy-MM-dd");
                receive_start.Enabled = true;
                receive_end.Enabled = true;

                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", " document.getElementById('ContentPlaceHolder1_searchbt').click();", true);
                //dsearch();
            }

            
        }

        if (ViewState["meeting_type"] != null)//要有物件才能觸發會議類型&使用類型的功能
        {
            show_meeting_item();
        }
        show_qualified_meeting_room();
    }
    #region 會議類型&使用類型
    protected void meeting_item_Click(object sender, ImageClickEventArgs e)
    {
        ImageButton ib = (ImageButton)sender;
        switch (ib.CommandArgument)
        {
            case "meetingtype":
                item_tile.InnerText = "會議類型";
                ViewState["meeting_type"] = "meetingtype";
                break;
            case "usedtype":
                item_tile.InnerText = "使用類型";
                ViewState["meeting_type"] = "usedtype";
                break;
        }

        show_meeting_item();
        ScriptManager.RegisterStartupScript(this, this.GetType(), "modal1", "$('#modal_item_add').modal('show');", true);
    }

    protected void show_meeting_item() //顯示選項
    {      
        string type = ViewState["meeting_type"].ToString();
        modal_item_pl.Controls.Clear();
        int i = 1;
        using (SqlConnection cn = new SqlConnection(eip))
        {
            cn.Open();
            string sql = @"SELECT * FROM meeting_apply_" + type;
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    Button delbt = new Button();
                    delbt.Text = "刪除";
                    delbt.ID = "del_" + type + "_" + i;
                    delbt.CssClass = "btn BT_red ml-4";
                    delbt.Attributes.Add("style", "padding: 2px 8px;height:60%;border-radius:5px;background-color: #B83F1F;");
                    delbt.CommandName = type;
                    delbt.CommandArgument = dr[0].ToString();
                    //delbt.Attributes.Add("onclick", "return confirm('您確定要刪除此項目嗎？')");
                    delbt.OnClientClick = "return confirm('您確定要刪除此項目嗎？')";
                    delbt.Click += delete_meeting_item;
                    ScriptManager.GetCurrent(this.Page).RegisterAsyncPostBackControl(delbt);

                    Label fav_bt = new Label();

                    //Button fav_bt = new Button();
                    fav_bt.Text = i + ".   " + dr[1];
                    fav_bt.CssClass = "gray_box";

                    modal_item_pl.Controls.Add(new LiteralControl("<div style='padding: 0px 20px 20px 20px;display: flex;align-items: center; '>"));
                    modal_item_pl.Controls.Add(fav_bt);
                    modal_item_pl.Controls.Add(delbt);
                    modal_item_pl.Controls.Add(new LiteralControl("</div>"));

                    i++;
                }
            }
        }

    }

    protected void add_meeting_item_Click(object sender, EventArgs e)
    {
        string type = ViewState["meeting_type"].ToString();
        using (SqlConnection cn = new SqlConnection(eip))
        {
            string sql1 = @"insert into meeting_apply_" + type + " (" + type + "_name) values (@content)";
            SqlCommand cmd = new SqlCommand(sql1, cn);
            cmd.Parameters.AddWithValue("@content", item_tb.Text);
            cn.Open();
            cmd.ExecuteNonQuery();
            cn.Close();
        }

        show_meeting_item();

        meetclass.Items.Clear();
        meetclass.DataBind();
        useclass.Items.Clear();
        useclass.DataBind();
    }

    protected void delete_meeting_item(object sender, EventArgs e)
    {
        Button bt = (Button)sender;
        string type = bt.CommandName;  // 這是用來標識要刪除的類型
        string index = bt.CommandArgument;  // 這是要刪除的名稱
        
        using (SqlConnection cn = new SqlConnection(eip))
        {
            string sql1 = @"delete meeting_apply_" + type + " where meeting_" + type + "_id=@index";
            SqlCommand cmd = new SqlCommand(sql1, cn);
            cmd.Parameters.AddWithValue("@index", index);
            cn.Open();
            cmd.ExecuteNonQuery();
            cn.Close();
        }

        show_meeting_item();

        meetclass.Items.Clear();
        meetclass.DataBind();
        useclass.Items.Clear();
        useclass.DataBind();
    }

    #endregion
    private void getddl()
    {
        using (SqlConnection cn = new SqlConnection(eip))
        {
            cn.Open();
            string sql = @"SELECT [no],[name]  FROM [group_name] where parent_id is null order by id ASC";
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    ddl3.Items.Add(new ListItem(dr["name"].ToString(), dr["no"].ToString()));
                }

            }
        }

    }
    private void getneetroom()
    {
        using (SqlConnection cn = new SqlConnection(eip))
        {
            cn.Open();
            mtroom.Items.Clear();
            mtroom.Items.Add(new ListItem("請選擇", "0"));
            ddl2.Items.Add(new ListItem("全部", "0"));
            string sql = @"SELECT  [id],[meet_name] FROM [meeting_equipment]";
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    mtroom.Items.Add(new ListItem(dr["meet_name"].ToString(), dr["id"].ToString()));
                    ddl2.Items.Add(new ListItem(dr["meet_name"].ToString(), dr["id"].ToString()));
                }

            }
        }

        if (!IsPostBack && Session["ddl3"] != null)
        {
           
            ddl2.SelectedValue = Session["ddl3"].ToString();
        }

    }
    private DataTable showdata(string sortExpression = null, string sortDirection = "ASC")
    {
        using (SqlConnection cn = new SqlConnection(eip))
        {
            cn.Open();
            string sql = @"SELECT t1.[id]
                                  ,[use_start]
                                  ,[use_end]
                                  ,FORMAT(t1.use_start, 'HH:mm') + '~' + FORMAT(t1.use_end, 'HH:mm') AS time_range
	                              ,t2.meet_name
	                              ,[appr_group]
                                  ,[meeting_name]
	                              ,t2.number
	                              ,appr_user
                                  ,[host]
	                              ,t1.[number] as Attendnumber
                                  ,[meetclass]
                                  ,[note]
                                  ,[state]
                              FROM [meeting_apprly] as t1
                              left join meeting_equipment as t2 
                              on t1.appr_meet_id=t2.id 
                              where  use_start >= GETDATE() AND use_start <= DATEADD(MONTH, 1, GETDATE())";

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
            sql = @"SELECT t1.[id]
                                  ,[use_start]
                                  ,[use_end]
                                  ,FORMAT(t1.use_start, 'HH:mm:ss') + '~' + FORMAT(t1.use_end, 'HH:mm:ss') AS time_range
	                              ,t2.meet_name
	                              ,[appr_group]
                                  ,[meeting_name]
	                              ,t2.number
	                              ,appr_user
                                  ,[host]
	                              ,t1.[number] as Attendnumber
                                  ,[meetclass]
                                  ,[note]
                                  ,[state],online
                              FROM [meeting_apprly] as t1
                              left join meeting_equipment as t2 
                              on t1.appr_meet_id=t2.id
                             left join group_name as t3 
                              on t1.appr_group=t3.name

                                where 1=1";
            if (chk1.Checked)
            {
                sql += " AND CHARINDEX('1', t2.equipment) > 0"; // 篩選包含 '1' 的設備
            }
            if (chk2.Checked)
            {
                sql += " AND CHARINDEX('2', t2.equipment) > 0"; // 篩選包含 '2' 的設備
            }
            if (chk3.Checked)
            {
                sql += " AND CHARINDEX('3', t2.equipment) > 0"; // 篩選包含 '3' 的設備
            }
            if (chk4.Checked)
            {
                sql += " AND CHARINDEX('4', t2.equipment) > 0"; // 篩選包含 '4' 的設備
            }
            if (chk5.Checked)
            {
                sql += " AND CHARINDEX('5', t2.equipment) > 0"; // 篩選包含 '5' 的設備
            }
            if (chk6.Checked)
            {
                sql += " AND CHARINDEX('6', t2.equipment) > 0"; // 篩選包含 '6' 的設備
            }
            if (chk7.Checked)
            {
                sql += " AND CHARINDEX('7', t2.equipment) > 0"; // 篩選包含 '7' 的設備
            }
            if (chk8.Checked)
            {
                sql += " AND CHARINDEX('8', t2.equipment) > 0"; // 篩選包含 '8' 的設備
            }
            if (safe_num.Text != "")
            {
                sql += " AND t2.number >= @number "; 
            }
            if (ddl1.SelectedValue != "0")
            {
                if (ddl1.SelectedValue == "1")
                {
                    sql += " and appr_user=@appr_user";
                }
                else if (ddl1.SelectedValue == "2")
                {
                    sql += " and t3.no like  '%' + @appr_group + '%'";
                }
            }
            if (ddl2.SelectedValue != "0")
            {
                sql += " and appr_meet_id=@appr_meet_id";
            }
            if (ddl3.SelectedValue != "0")
            {
                sql += " and t3.no like  '%' + @appr_group + '%'";
            }
            if (rb2.Checked)
            {
                sql += " and online=1";
            }
            if (rb3.Checked)
            {
                sql += " and online=0";
            }
            if (keyword.Text != "")
            {
                sql += " and (appr_user like '%" + keyword.Text + "%' or host like '%" + keyword.Text + "%' or meeting_name like '%" + keyword.Text + "%' or meet_name like '%" + keyword.Text + "%')  ";
            }
            if (rb_today.Checked)
            {
                // 時間是今天
                sql += " AND CAST(use_start AS DATE) = CAST(GETDATE() AS DATE)";
            }
            else if (rb_thisWeek.Checked)
            {
                // 時間是本週
                sql += " AND use_start >= DATEADD(WEEK, DATEDIFF(WEEK, 0, GETDATE()), 0)";  // 本週的第一天
                sql += " AND use_start < DATEADD(WEEK, DATEDIFF(WEEK, 0, GETDATE()), 7)";    // 本週的最後一天
            }
            else if (rb_nextMonth.Checked)
            {
                // 時間是未來一個月
                sql += " AND use_start >= GETDATE()";
                sql += " AND use_start <= DATEADD(MONTH, 1, GETDATE())";
            }
            else if (rb_lastMonth.Checked)
            {
                // 時間是前一個月
                sql += " AND use_start >= DATEADD(MONTH, -1, GETDATE())";
                sql += " AND use_start < GETDATE()";
            }
            else if (rb_customRange.Checked)
            {
                //sql += " AND use_start >= DATEADD(MONTH, -6, GETDATE())";
                //sql += " AND use_start < GETDATE()";
                // 自訂區間
                if (receive_start.Text != "")
                {
                    sql += " AND use_start >= '" + receive_start.Text + "'";
                }
                if (receive_end.Text != "")
                {
                    sql += " AND use_start <= '" + receive_end.Text + "'";
                }
            }
            if (!string.IsNullOrEmpty(sortExpression))
            {
                sql += @" ORDER BY " + sortExpression + " " + sortDirection;
            }
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                if (ddl3.SelectedValue != "0")
                {
                    cmd.Parameters.AddWithValue("@appr_group", ddl3.SelectedValue);
                }
                if (ddl1.SelectedValue != "0")
                {
                    if (ddl1.SelectedValue == "1")
                    {
                        cmd.Parameters.AddWithValue("@appr_user", Session["user_name"].ToString());
                    }
                    else if (ddl1.SelectedValue == "2")
                    {
                        cmd.Parameters.AddWithValue("@appr_group", ViewState["group_no"].ToString());
                    }
                }
                if (ddl2.SelectedValue != "0")
                {
                    cmd.Parameters.AddWithValue("@appr_meet_id", ddl2.SelectedValue);
                }
                if (safe_num.Text != "")
                {
                    cmd.Parameters.AddWithValue("@number", safe_num.Text);
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
        dsearch();
    }

    protected void RecommandMeetingRoom_search()
    {
        string sql = "";
        string MeetingRoomName = "";
        string MeetingRoomID = "";
        string MeetingRoomColor = "";
        using (SqlConnection cn = new SqlConnection(eip))
        {
            sql = @"select meet_name, id, color FROM meeting_equipment where 1=1";
            if (keyword.Text != "")
                sql += @" and meet_name like '%" + keyword.Text + "%'";
            if (ddl2.SelectedValue != "0")
                sql += " and id=@id";
            if (safe_num.Text != "")
                sql += " and number >= @safe_number ";
            if (chk1.Checked)
                sql += " AND CHARINDEX('1', equipment) > 0"; // 篩選包含 '1' 的設備
            if (chk2.Checked)
                sql += " AND CHARINDEX('2', equipment) > 0"; // 篩選包含 '2' 的設備
            if (chk3.Checked)
                sql += " AND CHARINDEX('3', equipment) > 0"; // 篩選包含 '3' 的設備
            if (chk4.Checked)
                sql += " AND CHARINDEX('4', equipment) > 0"; // 篩選包含 '4' 的設備
            if (chk5.Checked)
                sql += " AND CHARINDEX('5', equipment) > 0"; // 篩選包含 '5' 的設備
            if (chk6.Checked)
                sql += " AND CHARINDEX('6', equipment) > 0"; // 篩選包含 '6' 的設備
            if (chk7.Checked)
                sql += " AND CHARINDEX('7', equipment) > 0"; // 篩選包含 '7' 的設備
            if (chk8.Checked)
                sql += " AND CHARINDEX('8', equipment) > 0"; // 篩選包含 '8' 的設備

            sql += @" and meet_name not in (SELECT t2.meet_name      
                              FROM [meeting_apprly] as t1
                              left join meeting_equipment as t2 
                              on t1.appr_meet_id=t2.id
                             left join group_name as t3 
                              on t1.appr_group=t3.name

                                where 1=1";
            if (chk1.Checked)
            {
                sql += " AND CHARINDEX('1', t2.equipment) > 0"; // 篩選包含 '1' 的設備
            }
            if (chk2.Checked)
            {
                sql += " AND CHARINDEX('2', t2.equipment) > 0"; // 篩選包含 '2' 的設備
            }
            if (chk3.Checked)
            {
                sql += " AND CHARINDEX('3', t2.equipment) > 0"; // 篩選包含 '3' 的設備
            }
            if (chk4.Checked)
            {
                sql += " AND CHARINDEX('4', t2.equipment) > 0"; // 篩選包含 '4' 的設備
            }
            if (chk5.Checked)
            {
                sql += " AND CHARINDEX('5', t2.equipment) > 0"; // 篩選包含 '5' 的設備
            }
            if (chk6.Checked)
            {
                sql += " AND CHARINDEX('6', t2.equipment) > 0"; // 篩選包含 '6' 的設備
            }
            if (chk7.Checked)
            {
                sql += " AND CHARINDEX('7', t2.equipment) > 0"; // 篩選包含 '7' 的設備
            }
            if (chk8.Checked)
            {
                sql += " AND CHARINDEX('8', t2.equipment) > 0"; // 篩選包含 '8' 的設備
            }
            if (safe_num.Text != "")
            {
                sql += " AND t2.number >= @number "; 
            }
            if (ddl1.SelectedValue != "0")
            {
                if (ddl1.SelectedValue == "1")
                {
                    sql += " and appr_user=@appr_user";
                }
                else if (ddl1.SelectedValue == "2")
                {
                    sql += " and t3.no like  '%' + @appr_group + '%'";
                }
            }
            if (ddl2.SelectedValue != "0")
            {
                sql += " and appr_meet_id=@appr_meet_id";
            }
            if (ddl3.SelectedValue != "0")
            {
                sql += " and t3.no like  '%' + @appr_group + '%'";
            }
            if (rb2.Checked)
            {
                sql += " and online=1";
            }
            if (rb3.Checked)
            {
                sql += " and online=0";
            }
            if (keyword.Text != "")
            {
                sql += " and (appr_user like '%" + keyword.Text + "%' or host like '%" + keyword.Text + "%' or meeting_name like '%" + keyword.Text + "%' or meet_name like '%" + keyword.Text + "%')  ";
            }
            if (rb_today.Checked)
            {
                // 時間是今天
                sql += " AND CAST(use_start AS DATE) = CAST(GETDATE() AS DATE)";
            }
            else if (rb_thisWeek.Checked)
            {
                // 時間是本週
                sql += " AND use_start >= DATEADD(WEEK, DATEDIFF(WEEK, 0, GETDATE()), 0)";  // 本週的第一天
                sql += " AND use_start < DATEADD(WEEK, DATEDIFF(WEEK, 0, GETDATE()), 7)";    // 本週的最後一天
            }
            else if (rb_nextMonth.Checked)
            {
                // 時間是未來一個月
                sql += " AND use_start >= GETDATE()";
                sql += " AND use_start <= DATEADD(MONTH, 1, GETDATE())";
            }
            else if (rb_lastMonth.Checked)
            {
                // 時間是前一個月
                sql += " AND use_start >= DATEADD(MONTH, -1, GETDATE())";
                sql += " AND use_start < GETDATE()";
            }
            else if (rb_customRange.Checked)
            {
                //sql += " AND use_start >= DATEADD(MONTH, -6, GETDATE())";
                //sql += " AND use_start < GETDATE()";
                // 自訂區間
                if (receive_start.Text != "")
                {
                    sql += " AND use_start >= '" + receive_start.Text + "'";
                }
                if (receive_end.Text != "")
                {
                    sql += " AND use_start <= '" + receive_end.Text + "'";
                }
            }
            sql += @")";
            SqlCommand cmd = new SqlCommand(sql, cn);
            if (ddl3.SelectedValue != "0")
            {
                cmd.Parameters.AddWithValue("@appr_group", ddl3.SelectedValue);
            }
            if (ddl1.SelectedValue != "0")
            {
                if (ddl1.SelectedValue == "1")
                {
                    cmd.Parameters.AddWithValue("@appr_user", Session["user_name"].ToString());
                }
                else if (ddl1.SelectedValue == "2")
                {
                    cmd.Parameters.AddWithValue("@appr_group", ViewState["group_no"].ToString());
                }
            }
            if (ddl2.SelectedValue != "0")
            {
                cmd.Parameters.AddWithValue("@appr_meet_id", ddl2.SelectedValue);
                cmd.Parameters.AddWithValue("@id", ddl2.SelectedValue);
            }
            if (safe_num.Text != "")
            {
                cmd.Parameters.AddWithValue("@number", safe_num.Text);
                cmd.Parameters.AddWithValue("@safe_number", safe_num.Text);
            }
            cn.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                MeetingRoomName += dr[0].ToString() + ",";
                MeetingRoomID += dr[1].ToString() + ",";
                MeetingRoomColor += dr[2].ToString() + ",";
            }
            cn.Close();
            ViewState["MeetingRoomName"] = MeetingRoomName;
            ViewState["MeetingRoomID"] = MeetingRoomID;
            ViewState["MeetingRoomColor"] = MeetingRoomColor;
            show_qualified_meeting_room();
        }
    }

    protected void RecommandMeetingRoom_Click(object sender, EventArgs e)
    {
        RecommandMeetingRoom_search();
        ScriptManager.RegisterStartupScript(this, this.GetType(), "showmodal", "showRecommandMeetingRoomModal();", true);
    }

    protected void show_qualified_meeting_room()
    {
        if (ViewState["MeetingRoomName"] != null && ViewState["MeetingRoomName"].ToString() != "" && ViewState["MeetingRoomName"].ToString() != ",")
        {
            QualifiedMeetingRoom.Controls.Clear();
            List<string> namelist = ViewState["MeetingRoomName"].ToString().TrimEnd(',').Split(',').ToList();
            string[] IDList = ViewState["MeetingRoomID"].ToString().TrimEnd(',').Split(',');
            string[] ColorList = ViewState["MeetingRoomColor"].ToString().TrimEnd(',').Split(',');
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('"+ IDlist.Length +"');", true);
            int count = 0;
            foreach (string name in namelist)
            {
                Button bt = new Button();
                bt.Text = name;
                bt.CommandName = name;
                bt.CssClass = "E_box";
                bt.ID = "bt" + count;
                bt.Click += GoToSelectMeetingRoom;
                bt.CommandArgument = IDList[count];
                bt.UseSubmitBehavior = false;
                bt.Style["background-color"] = ColorList[count];
                QualifiedMeetingRoom.Controls.Add(bt);
                count++;
            }
        }
    }

    protected void GoToSelectMeetingRoom(object sender, EventArgs e)
    {
        Button bt = (Button)sender;
        mtroom.SelectedValue = bt.CommandArgument;
        show_mtroom_Selected(bt.CommandArgument);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "showmodal", "showModal1();", true);
    }

    protected void dsearch()
    {
        string sortExpression = ViewState["SortExpression"] as string;
        string sortDirection = ViewState["SortDirection"] as string;

        // 搜尋資料並將結果保存在 ViewState 中
        DataTable dt = search(sortExpression, sortDirection);
        ViewState["SearchResults"] = dt;
        Session["ddl3"] = ddl2.SelectedValue;

        Session["receive_start"] = receive_start.Text;
        Session["receive_end"] = receive_end.Text;

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
            Label meetclass = (Label)e.Row.FindControl("meetclass");
            Dictionary<string, string> meetingTypes = new Dictionary<string, string>
            {
                { "1", "研討會" },
                { "2", "發表會" },
                { "3", "股東大會" },
                { "4", "訓練講習會" },
                { "5", "聯誼活動" },
                { "6", "記者會" },
                { "7", "其他" },
                { "8", "招(開)標" },
                { "9", "電影欣賞" },
                { "10", "例行性會議" }
            };
            if (meetingTypes.ContainsKey(meetclass.Text))
            {
                string meetingName = meetingTypes[meetclass.Text];
                meetclass.Text = meetingName;
            }
            ///////////////////////////////
            Label use_start = (Label)e.Row.FindControl("use_start");
            Label use_end = (Label)e.Row.FindControl("use_end");
            if (Session["user_right_id"] != null && Session["user_right_id"].ToString() != "1" && Session["user_right_id"].ToString() != "2" && Session["user_right_id"].ToString() != "3")
            {
                // 顯示 "功能" 欄位
                gv.Columns[11].Visible = false; // 最後一個欄位為功能欄位
            }
            else
            {
                // 隱藏 "功能" 欄位
                gv.Columns[11].Visible = true;
            }


            Button btn = (Button)e.Row.FindControl("check");

            // 為每個按鈕動態設置 AsyncPostBackTrigger
            AsyncPostBackTrigger trigger = new AsyncPostBackTrigger();
            trigger.ControlID = btn.UniqueID;  // 使用 UniqueID，這樣能確保 ID 正確
            trigger.EventName = "Click";
            UpdatePanel1.Triggers.Add(trigger);
            UpdatePanel2.Triggers.Add(trigger);
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
            string id = ((Label)row.FindControl("id")).Text;
            del_id.Value = id;
            using (SqlConnection cn = new SqlConnection(eip))
            {
                cn.Open();
                string sql = @"SELECT  [id],[meeting_name],[use_start],[use_end],[appr_group],[appr_user],[appr_meet_id],[host]
                                ,[number],[meetclass],[useclass],[note],[online],[lunch_box],[Takeaway],[disposable],[other_reason]  FROM [meeting_apprly] where id=@id";
                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            m_no.Text = dr["meeting_name"].ToString();
                            DateTime useStart = dr.GetDateTime(dr.GetOrdinal("use_start"));
                            DateTime useEnd = dr.GetDateTime(dr.GetOrdinal("use_end"));
                            startdate.Text = useStart.ToString("yyyy-MM-dd");
                            starthour.Text = useStart.ToString("HH:mm");
                            enddate.Text = useEnd.ToString("yyyy-MM-dd");
                            endhour.Text = useEnd.ToString("HH:mm");
                            appr_group.Text = dr["appr_group"].ToString();
                            appr_user.Text = dr["appr_user"].ToString();
                            mtroom.SelectedValue = dr["appr_meet_id"].ToString();
                            show_mtroom_Selected(dr["appr_meet_id"].ToString());
                            host.Text = dr["host"].ToString();
                            number.Text = dr["number"].ToString();
                            meetclass.Text = dr["meetclass"].ToString();
                            useclass.Text = dr["useclass"].ToString();
                            note.Text = dr["note"].ToString();
                            ck_yes.Checked = dr["online"].ToString() == "1" ? true : false;
                            lunch_box.Text = dr["lunch_box"].ToString();
                            Takeaway.Text = dr["Takeaway"].ToString();
                            disposable.Text = dr["disposable"].ToString();
                            string otherReason = dr["other_reason"].ToString(); // 使用 null 合併運算子來避免 null 引發錯誤
                            var reasons = new List<string>();

                            if (!string.IsNullOrEmpty(otherReason)) // 檢查是否為 null 或空字串
                            {
                                reasons = otherReason.Split(',')
                                                      .Select(r => r.Trim()) // 去除每個選項的前後空格
                                                      .ToList();
                            }

                            List<string> otherOptions = new List<string>();
                            reason_ck1.Checked = false;
                            reason_ck2.Checked = false;
                            reason_ck3.Checked = false;
                            other_reason.Checked = false;

                            foreach (var item in reasons)
                            {
                                if (item == "訂購數量")
                                {
                                    reason_ck1.Checked = true; // 勾選第一個選項
                                }
                                else if (item == "收送時間")
                                {
                                    reason_ck2.Checked = true; // 勾選第二個選項
                                }
                                else if (item == "辦理場地")
                                {
                                    reason_ck3.Checked = true; // 勾選第三個選項
                                }
                                else
                                {
                                    otherOptions.Add(item); // 其他選項加入列表
                                }
                            }

                            // 檢查是否有 "其他" 選項，並返回給前端
                            string otherReasonInputValue = string.Empty;
                            if (otherOptions.Count > 0)
                            {
                                // 將最後一個 "其他" 選項設為輸入框的值
                                otherReasonInputValue = otherOptions.Last();
                                other_reason.Checked = true;
                                other_reason_txt.Text = otherReasonInputValue;
                            }
                            else
                            {
                                // 如果沒有 "其他" 選項，禁用輸入框
                                other_reason_txt.Text = string.Empty;
                                other_reason.Checked = false;
                            }


                        }
                    }
                }
            }
            submitbt.Visible = false;
            del.Style.Remove("display");
            modify.Style.Remove("display");

            ScriptManager.RegisterStartupScript(this, this.GetType(), "showmodal", "showModal1();", true);
        }
        if (e.CommandName == "del")
        {
            Button btn = (Button)e.CommandSource;
            GridViewRow row = (GridViewRow)btn.NamingContainer;
            string id = ((Label)row.FindControl("id")).Text;
            using (SqlConnection cn = new SqlConnection(eip))
            {
                cn.Open();
                string sql = @"delete [meeting_apprly] where id=@id";
                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
                cn.Close();
            }
            searchbt_Click(sender, e);
        }
    }

    protected void show_mtroom_Selected(string mtroom_value)
    {
        using (SqlConnection cn = new SqlConnection(eip))
        {
            cn.Open();
            string sql = @"SELECT  [equipment],[other],[number] FROM [meeting_equipment] where id=@id";
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.AddWithValue("@id", mtroom_value);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    mrdevices.Visible = true;
                    var equipmentMap = new Dictionary<int, string>()
                        {
                            { 1, "視訊會議攝影機" },
                            { 2, "大型顯示螢幕" },
                            { 3, "便條紙和筆" },
                            { 4, "白板" },
                            { 5, "麥克風" },
                            { 6, "電腦" },
                            { 7, "投影機" },
                            { 8, "音響系統" },
                            { 9, dr["other"].ToString()}
                        };

                    string equipmentIds = dr["equipment"].ToString();
                    string[] ids = equipmentIds.Split(',');

                    // 創建一個列表來儲存對應的設備名稱
                    List<string> equipmentNames = new List<string>();
                    foreach (string id in ids)
                    {
                        int equipmentId;
                        if (int.TryParse(id, out equipmentId) && equipmentMap.ContainsKey(equipmentId))
                        {
                            equipmentNames.Add(equipmentMap[equipmentId]);  // 根據設備 ID 找到對應的名稱
                        }
                    }
                    room_equipment.Text = "(" + string.Join(", ", equipmentNames) + ")";
                    room_number.Text = "(建議人數" + dr["number"].ToString() + "人)";
                }

            }
        }
        if (mtroom_value == "0")
        {
            room_equipment.Text = room_number.Text = "";
            mrdevices.Visible = false;
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
            receive_start.Text = string.Empty;
            receive_end.Text = string.Empty;
        }

    }
    protected void export_Click(object sender, EventArgs e)
    {
        List<string> use_start = new List<string>();
        List<string> time_range = new List<string>();
        List<string> meet_name = new List<string>();
        List<string> appr_group = new List<string>();
        List<string> meeting_name = new List<string>();
        List<string> number = new List<string>();
        List<string> appr_user = new List<string>();
        List<string> host = new List<string>();
        List<string> Attendnumber = new List<string>();
        List<string> meetclass = new List<string>();



        HSSFWorkbook workbook = new HSSFWorkbook();
        MemoryStream MemoryStream = new MemoryStream();
        // 新增試算表。 
        HSSFSheet sheet = (HSSFSheet)workbook.CreateSheet("會議室使用申請總表");
        HSSFFont font = (HSSFFont)workbook.CreateFont();
        font.FontName = "新細明體";
        font.FontHeightInPoints = 12;
        font.Boldweight = (short)FontBoldWeight.BOLD;
        HSSFFont font2 = (HSSFFont)workbook.CreateFont();
        font2.FontName = "新細明體";
        font2.FontHeightInPoints = 12;
        HSSFCellStyle font_style = (HSSFCellStyle)workbook.CreateCellStyle();
        font_style.SetFont(font2);

        sheet.CreateRow(0).CreateCell(0).SetCellValue("使用日期");
        sheet.GetRow(0).CreateCell(1).SetCellValue("使用時間");
        sheet.GetRow(0).CreateCell(2).SetCellValue("會議室");
        sheet.GetRow(0).CreateCell(3).SetCellValue("申請組室");
        sheet.GetRow(0).CreateCell(4).SetCellValue("會議名稱");
        sheet.GetRow(0).CreateCell(5).SetCellValue("可容納人數");
        sheet.GetRow(0).CreateCell(6).SetCellValue("申請者");
        sheet.GetRow(0).CreateCell(7).SetCellValue("主持人");
        sheet.GetRow(0).CreateCell(8).SetCellValue("出席人數");
        sheet.GetRow(0).CreateCell(9).SetCellValue("會議類型");
        HSSFRow row = (HSSFRow)sheet.GetRow(0);
        for (int i = 0; i <= 9; i++)
        {
            HSSFCell cell = (HSSFCell)row.GetCell(i, MissingCellPolicy.CREATE_NULL_AS_BLANK);
            cell.SetCellValue(cell.StringCellValue);
            HSSFCellStyle style = (HSSFCellStyle)workbook.CreateCellStyle();
            style.SetFont(font);
            cell.CellStyle = style;
            sheet.SetColumnWidth(i, 4000);
        }
        sheet.SetColumnWidth(1, 8000);
        sheet.SetColumnWidth(4, 8000);
        sheet.SetColumnWidth(9, 8000);
        using (SqlConnection cn = new SqlConnection(eip))
        {
            string sql = @"SELECT t1.[id]
                                  ,[use_start]
                                  ,[use_end]
                                  ,FORMAT(t1.use_start, 'HH:mm:ss') + '~' + FORMAT(t1.use_end, 'HH:mm:ss') AS time_range
	                              ,t2.meet_name
	                              ,[appr_group]
                                  ,[meeting_name]
	                              ,t2.number
	                              ,appr_user
                                  ,[host]
	                              ,t1.[number] as Attendnumber
                                  ,[meetclass]
                                  ,[note]
                                  ,[state]
                              FROM [meeting_apprly] as t1
                              left join meeting_equipment as t2 
                              on t1.appr_meet_id=t2.id
                              left join group_name as t3 
                              on t1.appr_group=t3.name
                              where 1=1";
            if (ddl1.SelectedValue != "0")
            {
                if (ddl1.SelectedValue == "1")
                {
                    sql += " and appr_user=@appr_user";
                }
                else if (ddl1.SelectedValue == "2")
                {
                    sql += " and t3.no like  '%' + @appr_group + '%'";
                }
            }
            if (ddl2.SelectedValue != "0")
            {
                sql += " and appr_meet_id=@appr_meet_id";
            }
            if (ddl3.SelectedValue != "0")
            {
                sql += " and t3.no like  '%' + @appr_group + '%'";
            }
            if (keyword.Text != "")
            {
                sql += " and (appr_user like '%" + keyword.Text + "%' or host like '%" + keyword.Text + "%' or meeting_name like '%" + keyword.Text + "%' or meet_name like '%" + keyword.Text + "%')  ";
            }
            if (rb_today.Checked)
            {
                // 時間是今天
                sql += " AND CAST(use_start AS DATE) = CAST(GETDATE() AS DATE)";
            }
            else if (rb_thisWeek.Checked)
            {
                // 時間是本週
                sql += " AND use_start >= DATEADD(WEEK, DATEDIFF(WEEK, 0, GETDATE()), 0)";  // 本週的第一天
                sql += " AND use_start < DATEADD(WEEK, DATEDIFF(WEEK, 0, GETDATE()), 7)";    // 本週的最後一天
            }
            else if (rb_nextMonth.Checked)
            {
                // 時間是未來一個月
                sql += " AND use_start >= GETDATE()";
                sql += " AND use_start <= DATEADD(MONTH, 1, GETDATE())";
            }
            else if (rb_lastMonth.Checked)
            {
                // 時間是前一個月
                sql += " AND use_start >= DATEADD(MONTH, -1, GETDATE())";
                sql += " AND use_start < GETDATE()";
            }
            else if (rb_customRange.Checked)
            {
                //sql += " AND use_start >= DATEADD(MONTH, -6, GETDATE())";
                //sql += " AND use_start < GETDATE()";
                // 自訂區間
                if (receive_start.Text != "")
                {
                    sql += " AND use_start >= '" + receive_start.Text + "'";
                }
                if (receive_end.Text != "")
                {
                    sql += " AND use_start <= '" + receive_end.Text + "'";
                }
            }
            sql += @" order by use_start DESC";
            SqlCommand cmd = new SqlCommand(sql, cn);
            if (ddl1.SelectedValue != "0")
            {
                if (ddl1.SelectedValue == "1")
                {
                    cmd.Parameters.AddWithValue("@appr_user", Session["user_name"].ToString());
                }
                else if (ddl1.SelectedValue == "2")
                {
                    cmd.Parameters.AddWithValue("@appr_group", ViewState["group_no"].ToString());
                }
            }
            if (ddl2.SelectedValue != "0")
            {
                cmd.Parameters.AddWithValue("@appr_meet_id", ddl2.SelectedValue);
            }
            if (ddl3.SelectedValue != "0")
            {
                cmd.Parameters.AddWithValue("@appr_group", ddl3.SelectedValue);
            }
            cn.Open();
            int rowIndex = 1;
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                string use_startstr = dr["use_start"].ToString();
                DateTime usestart = DateTime.Parse(use_startstr);
                use_start.Add(usestart.ToString("yyyy/MM/dd"));
                time_range.Add(dr["time_range"].ToString());
                meet_name.Add(dr["meet_name"].ToString());
                appr_group.Add(dr["appr_group"].ToString());
                meeting_name.Add(dr["meeting_name"].ToString());
                number.Add(dr["number"].ToString());
                appr_user.Add(dr["appr_user"].ToString());
                host.Add(dr["host"].ToString());
                Attendnumber.Add(dr["Attendnumber"].ToString());
                Dictionary<string, string> meetingTypes = new Dictionary<string, string>
                {
                    { "1", "研討會" },
                    { "2", "發表會" },
                    { "3", "股東大會" },
                    { "4", "訓練講習會" },
                    { "5", "聯誼活動" },
                    { "6", "記者會" },
                    { "7", "其他" },
                    { "8", "招(開)標" },
                    { "9", "電影欣賞" },
                    { "10", "例行性會議" }
                };
                if (meetingTypes.ContainsKey(dr["meetclass"].ToString()))
                {
                    string meetingName = meetingTypes[dr["meetclass"].ToString()];
                    meetclass.Add(meetingName);
                }


                row = (HSSFRow)sheet.CreateRow(rowIndex); // 在這裡創建新行，注意加1以避免覆蓋標題行

                row.CreateCell(0).SetCellValue(use_start[use_start.Count - 1]);
                row.CreateCell(1).SetCellValue(time_range[time_range.Count - 1]);
                row.CreateCell(2).SetCellValue(meet_name[meet_name.Count - 1]);
                row.CreateCell(3).SetCellValue(appr_group[appr_group.Count - 1]);
                row.CreateCell(4).SetCellValue(meeting_name[meeting_name.Count - 1]);
                row.CreateCell(5).SetCellValue(number[number.Count - 1]);
                row.CreateCell(6).SetCellValue(appr_user[appr_user.Count - 1]);
                row.CreateCell(7).SetCellValue(host[host.Count - 1]);
                row.CreateCell(8).SetCellValue(Attendnumber[Attendnumber.Count - 1]);


                if(meetclass.Count!=0)
                  row.CreateCell(9).SetCellValue(meetclass[meetclass.Count - 1]);
else 
  row.CreateCell(9).SetCellValue("");

                for (int i = 0; i <= 9; i++)
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
        Response.AddHeader("Content-Disposition", string.Format("attachment; filename=會議室使用申請總表.xls"));
        Response.BinaryWrite(MemoryStream.ToArray());


        workbook = null;
        // 釋放資源
        MemoryStream.Close();
        MemoryStream.Dispose();
    }

    protected void export_Reduce_Click(object sender, EventArgs e)
    {
        List<string> use_start = new List<string>();
        List<string> time_range = new List<string>();
        List<string> meeting_name = new List<string>();
        List<string> meet_name = new List<string>();
        List<string> appr_user = new List<string>();
        List<string> meetclass = new List<string>();
        List<string> lunch_box = new List<string>();
        List<string> Takeaway = new List<string>();
        List<string> disposable = new List<string>();
        List<string> other_reason = new List<string>();




        HSSFWorkbook workbook = new HSSFWorkbook();
        MemoryStream MemoryStream = new MemoryStream();
        // 新增試算表。 
        HSSFSheet sheet = (HSSFSheet)workbook.CreateSheet("免洗餐具及包裝因用水減量情形表");
        HSSFFont font = (HSSFFont)workbook.CreateFont();
        font.FontName = "新細明體";
        font.FontHeightInPoints = 12;
        font.Boldweight = (short)FontBoldWeight.BOLD;
        HSSFFont font2 = (HSSFFont)workbook.CreateFont();
        font2.FontName = "新細明體";
        font2.FontHeightInPoints = 12;
        HSSFCellStyle font_style = (HSSFCellStyle)workbook.CreateCellStyle();
        font_style.SetFont(font2);

        sheet.CreateRow(0).CreateCell(0).SetCellValue("使用日期");
        sheet.GetRow(0).CreateCell(1).SetCellValue("使用時間");
        sheet.GetRow(0).CreateCell(2).SetCellValue("會議室");
        sheet.GetRow(0).CreateCell(3).SetCellValue("會議名稱");
        sheet.GetRow(0).CreateCell(4).SetCellValue("申請者");
        sheet.GetRow(0).CreateCell(5).SetCellValue("會議類型");
        sheet.GetRow(0).CreateCell(6).SetCellValue("環保餐盒數量");
        sheet.GetRow(0).CreateCell(7).SetCellValue("外帶數量");
        sheet.GetRow(0).CreateCell(8).SetCellValue("一次性產品數量");
        sheet.GetRow(0).CreateCell(9).SetCellValue("無法配合原因");

        HSSFRow row = (HSSFRow)sheet.GetRow(0);
        for (int i = 0; i <= 9; i++)
        {
            HSSFCell cell = (HSSFCell)row.GetCell(i, MissingCellPolicy.CREATE_NULL_AS_BLANK);
            cell.SetCellValue(cell.StringCellValue);
            HSSFCellStyle style = (HSSFCellStyle)workbook.CreateCellStyle();
            style.SetFont(font);
            cell.CellStyle = style;
            sheet.SetColumnWidth(i, 4000);
        }
        sheet.SetColumnWidth(1, 8000);
        sheet.SetColumnWidth(8, 6000);
        sheet.SetColumnWidth(9, 12000);
        using (SqlConnection cn = new SqlConnection(eip))
        {
            string sql = @"SELECT t1.[id]
                                  ,[use_start]
                                  ,FORMAT(t1.use_start, 'HH:mm:ss') + '~' + FORMAT(t1.use_end, 'HH:mm:ss') AS time_range
	                              ,t2.meet_name
                                  ,[meeting_name]
	                              ,t2.number
	                              ,appr_user
	                              ,t1.[number] as Attendnumber
                                  ,[meetclass]
								  ,[online]
								  ,[lunch_box]
								  ,[Takeaway]
								  ,[disposable]
								  ,[other_reason]
								  ,t2.[other]
                              FROM [meeting_apprly] as t1
                              left join meeting_equipment as t2 
                             on t1.appr_meet_id=t2.id where lunch_box is not null or Takeaway is not null or disposable is not null or other_reason is not null";

            SqlCommand cmd = new SqlCommand(sql, cn);
            DateTime startDate;
            DateTime endDate;
            bool isStartDateValid = DateTime.TryParseExact(Reduce_start.Text, "yyyy-MM", null, System.Globalization.DateTimeStyles.None, out startDate);
            bool isEndDateValid = DateTime.TryParseExact(Reduce_end.Text, "yyyy-MM", null, System.Globalization.DateTimeStyles.None, out endDate);
            if (isStartDateValid && isEndDateValid && startDate > endDate)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('開始日期不可大於結束日期');", true);
                return;
            }
            if (isStartDateValid)
            {
                sql += " AND FORMAT(use_start, 'yyyy-MM') >= '" + startDate.ToString("yyyy-MM") + "'";
            }
            if (isEndDateValid)
            {
                sql += " AND FORMAT(use_start, 'yyyy-MM') <= '" + endDate.ToString("yyyy-MM") + "'";
            }
            sql += @" order by use_start DESC";
            cn.Open();
            int rowIndex = 1;
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                string use_startstr = dr["use_start"].ToString();
                DateTime usestart = DateTime.Parse(use_startstr);
                use_start.Add(usestart.ToString("yyyy/MM/dd"));
                time_range.Add(dr["time_range"].ToString());
                meet_name.Add(dr["meet_name"].ToString());
                meeting_name.Add(dr["meeting_name"].ToString());
                appr_user.Add(dr["appr_user"].ToString());
                Dictionary<string, string> meetingTypes = new Dictionary<string, string>
                {
                    { "1", "研討會" },
                    { "2", "發表會" },
                    { "3", "股東大會" },
                    { "4", "訓練講習會" },
                    { "5", "聯誼活動" },
                    { "6", "記者會" },
                    { "7", "其他" },
                    { "8", "招(開)標" },
                    { "9", "電影欣賞" },
                    { "10", "例行性會議" }
                };
                if (meetingTypes.ContainsKey(dr["meetclass"].ToString()))
                {
                    string meetingName = meetingTypes[dr["meetclass"].ToString()];
                    meetclass.Add(meetingName);
                }
                if (dr["lunch_box"] != DBNull.Value)
                {
                    lunch_box.Add(dr["lunch_box"].ToString());
                }
                else
                {
                    lunch_box.Add("0");
                }
                if (dr["Takeaway"] != DBNull.Value)
                {
                    Takeaway.Add(dr["Takeaway"].ToString());
                }
                else
                {
                    Takeaway.Add("0");
                }
                if (dr["disposable"] != DBNull.Value)
                {
                    disposable.Add(dr["disposable"].ToString());
                }
                else
                {
                    disposable.Add("0");
                }
                if (dr["other_reason"] != DBNull.Value)
                {
                    other_reason.Add(dr["other_reason"].ToString());
                }
                else
                {
                    other_reason.Add("無");
                }




                row = (HSSFRow)sheet.CreateRow(rowIndex); // 在這裡創建新行，注意加1以避免覆蓋標題行

                row.CreateCell(0).SetCellValue(use_start[use_start.Count - 1]);
                row.CreateCell(1).SetCellValue(time_range[time_range.Count - 1]);
                row.CreateCell(2).SetCellValue(meet_name[meet_name.Count - 1]);
                row.CreateCell(3).SetCellValue(meeting_name[meeting_name.Count - 1]);
                row.CreateCell(4).SetCellValue(appr_user[appr_user.Count - 1]);
                row.CreateCell(5).SetCellValue(meetclass[meetclass.Count - 1]);
                row.CreateCell(6).SetCellValue(lunch_box[lunch_box.Count - 1]);
                row.CreateCell(7).SetCellValue(Takeaway[Takeaway.Count - 1]);
                row.CreateCell(8).SetCellValue(disposable[disposable.Count - 1]);
                row.CreateCell(9).SetCellValue(other_reason[other_reason.Count - 1]);


                for (int i = 0; i <= 9; i++)
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
        Response.AddHeader("Content-Disposition", string.Format("attachment; filename=免洗餐具及包裝因用水減量情形表.xls"));
        Response.BinaryWrite(MemoryStream.ToArray());


        workbook = null;
        // 釋放資源
        MemoryStream.Close();
        MemoryStream.Dispose();
    }

    protected void chk0_CheckedChanged(object sender, EventArgs e)
    {
        if (chk0.Checked)
        {
            chk1.Checked = chk2.Checked = chk3.Checked = chk4.Checked = chk5.Checked = chk6.Checked = chk7.Checked = chk8.Checked = true;
        }
        else
        {
            chk1.Checked = chk2.Checked = chk3.Checked = chk4.Checked = chk5.Checked = chk6.Checked = chk7.Checked = chk8.Checked = false;
        }
    }

    //private void getneetroom()
    //{
    //    string favmt_id = "";
    //    List<int> favoriteMeetIds = new List<int>();
    //    using (SqlConnection cn = new SqlConnection(eip))
    //    {
    //        cn.Open();
    //        string sql = @"SELECT  [favorite_meet] FROM [eip_user] where user_id=@user_id";
    //        using (SqlCommand cmd = new SqlCommand(sql, cn))
    //        {
    //            cmd.Parameters.AddWithValue("@user_id", Session["user_id"].ToString());
    //            SqlDataReader dr = cmd.ExecuteReader();
    //            if (dr.Read())
    //            {
    //                favmt_id = dr["favorite_meet"].ToString();
    //            }

    //        }
    //    }
    //    string[] favmtArray = favmt_id.Split(',');
    //    foreach (string id in favmtArray)
    //    {
    //        if (id.Trim().Length > 0) // 確保字串不是空白
    //        {
    //            int result = int.Parse(id);
    //            favoriteMeetIds.Add(result);
    //        }
    //    }

    //    using (SqlConnection cn = new SqlConnection(eip))
    //    {
    //        cn.Open();
    //        mtroom.Items.Clear();
    //        mtroom.Items.Add(new ListItem("請選擇", "0"));
    //        ddl3.Items.Add(new ListItem("所有會議室", "0"));
    //        string sql = @"SELECT  [id],[meet_name] FROM [meeting_equipment]";
    //        using (SqlCommand cmd = new SqlCommand(sql, cn))
    //        {
    //            SqlDataReader dr = cmd.ExecuteReader();
    //            // 儲存最喜愛會議室項目
    //            List<ListItem> favoriteItems = new List<ListItem>();
    //            // 儲存其他會議室項目
    //            List<ListItem> otherItems = new List<ListItem>();

    //            while (dr.Read())
    //            {
    //                if (favoriteMeetIds.Contains(Convert.ToInt32(dr["id"])))
    //                {
    //                    favoriteItems.Add(new ListItem("★" + dr["meet_name"].ToString(), dr["id"].ToString()));
    //                }
    //                else
    //                {
    //                    otherItems.Add(new ListItem(dr["meet_name"].ToString(), dr["id"].ToString()));
    //                }

    //                ddl3.Items.Add(new ListItem(dr["meet_name"].ToString(), dr["id"].ToString()));
    //            }
    //            foreach (var item in favoriteItems)
    //            {
    //                mtroom.Items.Add(item);
    //            }

    //            foreach (var item in otherItems)
    //            {
    //                mtroom.Items.Add(item);
    //            }

    //        }
    //    }
    //}
    protected void mtroom_SelectedIndexChanged(object sender, EventArgs e)
    {
        using (SqlConnection cn = new SqlConnection(eip))
        {
            cn.Open();
            string sql = @"SELECT  [equipment],[other],[number] FROM [meeting_equipment] where id=@id";
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.AddWithValue("@id", mtroom.SelectedValue);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    mrdevices.Visible = true;
                    var equipmentMap = new Dictionary<int, string>()
                        {
                            { 1, "視訊會議攝影機" },
                            { 2, "大型顯示螢幕" },
                            { 3, "便條紙和筆" },
                            { 4, "白板" },
                            { 5, "麥克風" },
                            { 6, "電腦" },
                            { 7, "投影機" },
                            { 8, "音響系統" },
                            { 9, dr["other"].ToString()}
                        };

                    string equipmentIds = dr["equipment"].ToString();
                    string[] ids = equipmentIds.Split(',');

                    // 創建一個列表來儲存對應的設備名稱
                    List<string> equipmentNames = new List<string>();
                    foreach (string id in ids)
                    {
                        int equipmentId;
                        if (int.TryParse(id, out equipmentId) && equipmentMap.ContainsKey(equipmentId))
                        {
                            equipmentNames.Add(equipmentMap[equipmentId]);  // 根據設備 ID 找到對應的名稱
                        }
                    }
                    room_equipment.Text = "(" + string.Join(", ", equipmentNames) + ")";
                    room_number.Text = "(建議人數" + dr["number"].ToString() + "人)";
                }

            }
        }
        if (mtroom.SelectedValue == "0")
        {
            room_equipment.Text = room_number.Text = "";
            mrdevices.Visible = false;
        }

     //   ScriptManager.RegisterStartupScript(this, this.GetType(), "modal1", "$('#modal1').modal('show');", true);

    }
    protected void submitbt_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(m_no.Text))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('會議名稱尚未填寫');", true);
            return;
        }
        if (string.IsNullOrEmpty(startdate.Text) || string.IsNullOrEmpty(starthour.Text) || string.IsNullOrEmpty(endhour.Text) || string.IsNullOrEmpty(enddate.Text))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('時間尚未填寫');", true);
            return;
        }
        if (string.IsNullOrEmpty(m_no.Text))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('會議名稱尚未填寫');", true);
            return;
        }
        if (string.IsNullOrEmpty(mtroom.SelectedValue))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('會議室尚未選擇');", true);
            return;
        }
        if (string.IsNullOrEmpty(host.Text))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('主持人尚未填寫');", true);
            return;
        }
        if (string.IsNullOrEmpty(number.Text))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('出席人數尚未填寫');", true);
            return;
        }
        if (string.IsNullOrEmpty(meetclass.SelectedValue))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('會議類型尚未選擇');", true);
            return;
        }
        if (string.IsNullOrEmpty(useclass.SelectedValue))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('使用類型尚未選擇');", true);
            return;
        }
        DateTime startDateTime = DateTime.Parse(string.Concat(startdate.Text, " ", starthour.Text));
        DateTime endDateTime = DateTime.Parse(string.Concat(enddate.Text, " ", endhour.Text));
        using (SqlConnection cn = new SqlConnection(eip))
        {
            cn.Open();
            string sql = @"SELECT [id],[meeting_name],[use_start],[use_end],[appr_meet_id] FROM [meeting_apprly] 
                          where appr_meet_id=@appr_meet_id
                          AND ((use_start < @endDateTime AND use_end > @startDateTime))";


            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.AddWithValue("@appr_meet_id", mtroom.SelectedValue);
                cmd.Parameters.AddWithValue("@startDateTime", startDateTime);
                cmd.Parameters.AddWithValue("@endDateTime", endDateTime);

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.HasRows)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('此會議室選擇時段已有其他人申請請重新選擇時段或會議室');", true);
                        return;
                    }
                }
            }
        }
        int daysDifference = (endDateTime - startDateTime).Days;
        if (startdate.Text != enddate.Text)
        {
            using (SqlConnection cn = new SqlConnection(eip))
            {
                cn.Open();
                string sql = @"INSERT INTO [meeting_apprly] (meeting_name, use_start, use_end, appr_group, appr_user, appr_meet_id, host, number, meetclass, useclass, note, creat_date, creat_user, state,online,other,lunch_box,Takeaway,disposable,other_reason) 
                   VALUES (@meeting_name, @use_start, @use_end, @appr_group, @appr_user, @appr_meet_id, @host, @number, @meetclass, @useclass, @note, @creat_date, @creat_user, @state,@online,@other,@lunch_box,@Takeaway,@disposable,@other_reason)";

                for (int i = 0; i <= daysDifference; i++)
                {
                    DateTime currentStartTime = startDateTime.AddDays(i); // 當前日期
                    DateTime currentEndTime = (i == daysDifference) ? endDateTime : currentStartTime.AddDays(1).AddSeconds(-1);
                    if (currentStartTime.DayOfWeek == DayOfWeek.Saturday || currentStartTime.DayOfWeek == DayOfWeek.Sunday)
                    {
                        continue; // 跳過週六和週日
                    }
                    DateTime dayStartTime = new DateTime(currentStartTime.Year, currentStartTime.Month, currentStartTime.Day, 8, 30, 0);
                    DateTime dayEndTime = new DateTime(currentEndTime.Year, currentEndTime.Month, currentEndTime.Day, 17, 30, 0);


                    if (i == 0)
                    {
                        dayStartTime = new DateTime(currentStartTime.Year, currentStartTime.Month, currentStartTime.Day, startDateTime.Hour, startDateTime.Minute, 0);
                        dayEndTime = new DateTime(currentStartTime.Year, currentStartTime.Month, currentStartTime.Day, 17, 30, 0);
                    }
                    if (i == daysDifference)
                    {
                        dayStartTime = new DateTime(currentEndTime.Year, currentEndTime.Month, currentEndTime.Day, 8, 30, 0);

                        dayEndTime = new DateTime(currentEndTime.Year, currentEndTime.Month, currentEndTime.Day, endDateTime.Hour, endDateTime.Minute, 0);
                    }

                    if (i > 0 && i < daysDifference)
                    {
                        dayStartTime = new DateTime(currentStartTime.Year, currentStartTime.Month, currentStartTime.Day, 8, 30, 0);
                        dayEndTime = new DateTime(currentStartTime.Year, currentStartTime.Month, currentStartTime.Day, 17, 30, 0);
                    }

                    using (SqlCommand cmd = new SqlCommand(sql, cn))
                    {
                        cmd.Parameters.AddWithValue("@meeting_name", m_no.Text);
                        cmd.Parameters.AddWithValue("@use_start", dayStartTime);
                        cmd.Parameters.AddWithValue("@use_end", dayEndTime);
                        cmd.Parameters.AddWithValue("@appr_group", appr_group.Text);
                        cmd.Parameters.AddWithValue("@appr_user", appr_user.Text);
                        cmd.Parameters.AddWithValue("@appr_meet_id", mtroom.SelectedValue);
                        cmd.Parameters.AddWithValue("@host", host.Text);
                        cmd.Parameters.AddWithValue("@number", number.Text);
                        cmd.Parameters.AddWithValue("@meetclass", meetclass.SelectedValue);
                        cmd.Parameters.AddWithValue("@useclass", useclass.SelectedValue);
                        cmd.Parameters.AddWithValue("@note", note.Text);
                        cmd.Parameters.AddWithValue("@creat_date", DateTime.Now);
                        cmd.Parameters.AddWithValue("@creat_user", Session["user_name"].ToString());
                        cmd.Parameters.AddWithValue("@state", "0");
                        if (!ck_yes.Checked)
                        {
                            cmd.Parameters.AddWithValue("@online", "0");
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@online", "1");
                        }

                        cmd.Parameters.AddWithValue("@other", string.IsNullOrEmpty(other.Text) ? (object)DBNull.Value : (object)other.Text);
                        cmd.Parameters.AddWithValue("@lunch_box", string.IsNullOrEmpty(lunch_box.Text) ? (object)DBNull.Value : (object)lunch_box.Text);
                        cmd.Parameters.AddWithValue("@Takeaway", string.IsNullOrEmpty(Takeaway.Text) ? (object)DBNull.Value : (object)Takeaway.Text);
                        cmd.Parameters.AddWithValue("@disposable", string.IsNullOrEmpty(disposable.Text) ? (object)DBNull.Value : (object)disposable.Text);

                        string reason = "";
                        if (reason_ck1.Checked)
                        {
                            reason += "訂購數量,";
                        }
                        if (reason_ck2.Checked)
                        {
                            reason += "收送時間,";
                        }
                        if (reason_ck3.Checked)
                        {
                            reason += "辦理場地,";
                        }
                        if (other_reason.Checked && !string.IsNullOrEmpty(other_reason_txt.Text))
                        {
                            reason += other_reason_txt.Text + ",";
                        }

                        // 去除結尾的多餘逗號和空格
                        if (reason.EndsWith(", "))
                        {
                            reason = reason.Substring(0, reason.Length - 1);
                        }

                        // 如果沒有任何勾選框選中，reason 就會是空字符串
                        cmd.Parameters.AddWithValue("@other_reason", string.IsNullOrEmpty(reason) ? DBNull.Value : (object)reason);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }
        else
        {
            using (SqlConnection cn = new SqlConnection(eip))
            {
                cn.Open();
                string sql = @"INSERT INTO [meeting_apprly] (meeting_name,use_start, use_end,appr_group,appr_user,appr_meet_id,host,number,meetclass,useclass,note,creat_date,creat_user,state,online,other,lunch_box,Takeaway,disposable,other_reason) 
                                                VALUES (@meeting_name,@use_start, @use_end,@appr_group,@appr_user,@appr_meet_id,@host,@number,@meetclass,@useclass,@note,@creat_date,@creat_user,@state,@online,@other,@lunch_box,@Takeaway,@disposable,@other_reason)";

                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@meeting_name", m_no.Text);
                    cmd.Parameters.AddWithValue("@use_start", startDateTime);
                    cmd.Parameters.AddWithValue("@use_end", endDateTime);
                    cmd.Parameters.AddWithValue("@appr_group", appr_group.Text);
                    cmd.Parameters.AddWithValue("@appr_user", appr_user.Text);
                    cmd.Parameters.AddWithValue("@appr_meet_id", mtroom.SelectedValue);
                    cmd.Parameters.AddWithValue("@host", host.Text);
                    cmd.Parameters.AddWithValue("@number", number.Text);
                    cmd.Parameters.AddWithValue("@meetclass", meetclass.SelectedValue);
                    cmd.Parameters.AddWithValue("@useclass", useclass.SelectedValue);
                    cmd.Parameters.AddWithValue("@note", note.Text);
                    cmd.Parameters.AddWithValue("@creat_date", DateTime.Now);
                    cmd.Parameters.AddWithValue("@creat_user", Session["user_name"].ToString());
                    cmd.Parameters.AddWithValue("@state", "0");
                    if (!ck_yes.Checked)
                    {
                        cmd.Parameters.AddWithValue("@online", "0");
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@online", "1");
                    }
                    cmd.Parameters.AddWithValue("@other", string.IsNullOrEmpty(other.Text) ? (object)DBNull.Value : (object)other.Text);
                    cmd.Parameters.AddWithValue("@lunch_box", string.IsNullOrEmpty(lunch_box.Text) ? (object)DBNull.Value : (object)lunch_box.Text);
                    cmd.Parameters.AddWithValue("@Takeaway", string.IsNullOrEmpty(Takeaway.Text) ? (object)DBNull.Value : (object)Takeaway.Text);
                    cmd.Parameters.AddWithValue("@disposable", string.IsNullOrEmpty(disposable.Text) ? (object)DBNull.Value : (object)disposable.Text);

                    string reason = "";
                    if (reason_ck1.Checked)
                    {
                        reason += "訂購數量,";
                    }
                    if (reason_ck2.Checked)
                    {
                        reason += "收送時間,";
                    }
                    if (reason_ck3.Checked)
                    {
                        reason += "辦理場地,";
                    }
                    if (other_reason.Checked && !string.IsNullOrEmpty(other_reason_txt.Text))
                    {
                        reason += other_reason_txt.Text + ",";
                    }

                    // 去除結尾的多餘逗號和空格
                    if (reason.EndsWith(", "))
                    {
                        reason = reason.Substring(0, reason.Length - 1);
                    }

                    // 如果沒有任何勾選框選中，reason 就會是空字符串
                    cmd.Parameters.AddWithValue("@other_reason", string.IsNullOrEmpty(reason) ? DBNull.Value : (object)reason);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        ScriptManager.RegisterStartupScript(this, this.GetType(), "hideModal1", "hideModal1();", true);
        searchbt_Click(sender, e);
    }
    protected void modify_Click(object sender, EventArgs e)
    {

        if (string.IsNullOrEmpty(m_no.Text))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('會議名稱尚未填寫');", true);
            return;
        }
        if (string.IsNullOrEmpty(startdate.Text) || string.IsNullOrEmpty(starthour.Text) || string.IsNullOrEmpty(endhour.Text) || string.IsNullOrEmpty(enddate.Text))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('時間尚未填寫');", true);
            return;
        }
        if (string.IsNullOrEmpty(m_no.Text))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('會議名稱尚未填寫');", true);
            return;
        }
        if (string.IsNullOrEmpty(mtroom.SelectedValue))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('會議室尚未選擇');", true);
            return;
        }
        if (string.IsNullOrEmpty(host.Text))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('主持人尚未填寫');", true);
            return;
        }
        if (string.IsNullOrEmpty(number.Text))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('出席人數尚未填寫');", true);
            return;
        }
        if (string.IsNullOrEmpty(meetclass.SelectedValue))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('會議類型尚未選擇');", true);
            return;
        }
        if (string.IsNullOrEmpty(useclass.SelectedValue))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('使用類型尚未選擇');", true);
            return;
        }
        DateTime startDateTime = DateTime.Parse(string.Concat(startdate.Text, " ", starthour.Text));
        DateTime endDateTime = DateTime.Parse(string.Concat(enddate.Text, " ", endhour.Text));
        using (SqlConnection cn = new SqlConnection(eip))
        {
            cn.Open();
            string sql = @"SELECT [id],[meeting_name],[use_start],[use_end],[appr_meet_id] FROM [meeting_apprly] 
                          where appr_meet_id=@appr_meet_id
                          AND ((use_start < @endDateTime AND use_end > @startDateTime))";


            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.AddWithValue("@appr_meet_id", mtroom.SelectedValue);
                cmd.Parameters.AddWithValue("@startDateTime", startDateTime);
                cmd.Parameters.AddWithValue("@endDateTime", endDateTime);

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.HasRows)
                    {
                        if (dr.Read())
                        {
                            string id = dr["id"].ToString();
                            if (id != hf_id.Value)
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('此會議室選擇時段已有其他人申請請重新選擇時段或會議室');", true);
                                return;
                            }
                        }
                    }
                }
            }
        }
        using (SqlConnection cn = new SqlConnection(eip))
        {
            cn.Open();
            string sql = @"update [meeting_apprly] set meeting_name=@meeting_name,use_start=@use_start, use_end=@use_end,appr_group=@appr_group
                            ,appr_user=@appr_user,appr_meet_id=@appr_meet_id,host=@host,number=@number,meetclass=@meetclass,
                            useclass=@useclass,note=@note,creat_date=@creat_date,creat_user=@creat_user,online=@online,other=@other,lunch_box=@lunch_box,
                            Takeaway=@Takeaway,disposable=@disposable,other_reason=@other_reason where id=@id";
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.AddWithValue("@meeting_name", m_no.Text);
                cmd.Parameters.AddWithValue("@use_start", startDateTime);
                cmd.Parameters.AddWithValue("@use_end", endDateTime);
                cmd.Parameters.AddWithValue("@appr_group", appr_group.Text);
                cmd.Parameters.AddWithValue("@appr_user", appr_user.Text);
                cmd.Parameters.AddWithValue("@appr_meet_id", mtroom.SelectedValue);
                cmd.Parameters.AddWithValue("@host", host.Text);
                cmd.Parameters.AddWithValue("@number", number.Text);
                cmd.Parameters.AddWithValue("@meetclass", meetclass.SelectedValue);
                cmd.Parameters.AddWithValue("@useclass", useclass.SelectedValue);
                cmd.Parameters.AddWithValue("@note", note.Text);
                cmd.Parameters.AddWithValue("@creat_date", DateTime.Now);
                cmd.Parameters.AddWithValue("@creat_user", Session["user_name"].ToString());
                cmd.Parameters.AddWithValue("@id", del_id.Value);
                if (!ck_yes.Checked)
                {
                    cmd.Parameters.AddWithValue("@online", "0");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@online", "1");
                }
                cmd.Parameters.AddWithValue("@other", string.IsNullOrEmpty(other.Text) ? (object)DBNull.Value : (object)other.Text);
                cmd.Parameters.AddWithValue("@lunch_box", string.IsNullOrEmpty(lunch_box.Text) ? (object)DBNull.Value : (object)lunch_box.Text);
                cmd.Parameters.AddWithValue("@Takeaway", string.IsNullOrEmpty(Takeaway.Text) ? (object)DBNull.Value : (object)Takeaway.Text);
                cmd.Parameters.AddWithValue("@disposable", string.IsNullOrEmpty(disposable.Text) ? (object)DBNull.Value : (object)disposable.Text);
                string reason = "";
                if (reason_ck1.Checked)
                {
                    reason += "訂購數量,";
                }
                if (reason_ck2.Checked)
                {
                    reason += "收送時間,";
                }
                if (reason_ck3.Checked)
                {
                    reason += "辦理場地,";
                }
                if (other_reason.Checked && !string.IsNullOrEmpty(other_reason_txt.Text))
                {
                    reason += other_reason_txt.Text + ",";
                    // 去除結尾的多餘逗號和空格
                    if (reason.EndsWith(","))
                    {
                        reason = reason.Substring(0, reason.Length - 1);
                    }
                }



                // 如果沒有任何勾選框選中，reason 就會是空字符串
                cmd.Parameters.AddWithValue("@other_reason", string.IsNullOrEmpty(reason) ? DBNull.Value : (object)reason);
                cmd.ExecuteNonQuery();
            }
        }
        ScriptManager.RegisterStartupScript(this, this.GetType(), "hideModal1", "hideModal1();", true);
        searchbt_Click(sender, e);


    }
    protected void del_Click(object sender, EventArgs e)
    {
        string ap_id = "";
        string mclass = "";
        string uclass = "";
        string c_user = "";
        int mtcount = 0;
        List<string> id = new List<string>();
        using (SqlConnection cn = new SqlConnection(eip))
        {
            cn.Open();
            string sql = @"SELECT [appr_meet_id],[meetclass],[useclass],[creat_user] FROM [meeting_apprly] where @id=id ";

            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.AddWithValue("@id", del_id.Value);
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        ap_id = dr["appr_meet_id"].ToString();
                        mclass = dr["meetclass"].ToString();
                        uclass = dr["useclass"].ToString();
                        c_user = dr["creat_user"].ToString();
                    }
                }
            }
        }
        using (SqlConnection cn = new SqlConnection(eip))
        {
            cn.Open();
            string sql = @"SELECT id FROM [meeting_apprly] where appr_meet_id=@appr_meet_id and meetclass=@meetclass and useclass=@useclass and creat_user=@creat_user";

            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.AddWithValue("@appr_meet_id", ap_id);
                cmd.Parameters.AddWithValue("@meetclass", mclass);
                cmd.Parameters.AddWithValue("@useclass", uclass);
                cmd.Parameters.AddWithValue("@creat_user", c_user);
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        id.Add(dr["id"].ToString());
                    }
                    if (id.Count > 1)
                    {
                        mtcount = id.Count;  // 記錄資料筆數
                    }
                }
            }
        }
        if (mtcount > 1)
        {
            md2del_sing.Visible = false;
            md2del_let.Visible = true;
            md2del_other.Visible = true;
            deltxt.Text = "此筆申請還有額外" + (mtcount - 1) + " 筆申請資料，是否要一起刪除嗎？";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "showmoda2", "showModal2();", true);
        }
        else
        {
            md2del_sing.Visible = true;
            md2del_let.Visible = false;
            md2del_other.Visible = false;
            deltxt.Text = "確定要刪除嗎？";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "showmoda2", "showModal2();", true);
        }




    }
    protected void md2del_sing_Click(object sender, EventArgs e)
    {
        using (SqlConnection cn = new SqlConnection(eip))
        {
            cn.Open();
            string sql = @"delete [meeting_apprly] where @id=id ";

            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.AddWithValue("@id", del_id.Value);
                cmd.ExecuteNonQuery();
            }
        }
        string mod = Request.QueryString["mod"];
        int month = Request.QueryString["month"] != null ? Convert.ToInt32(Request.QueryString["month"]) : DateTime.Now.Month;
        int year = Request.QueryString["year"] != null ? Convert.ToInt32(Request.QueryString["year"]) : DateTime.Now.Year;
        Response.Redirect("Meetingroom_calendar.aspx?mod=w&month=" + month + "&year=" + year);
    }
    protected void md2del_let_Click(object sender, EventArgs e)
    {
        string ap_id = "";
        string mclass = "";
        string uclass = "";
        string c_user = "";
        List<string> id = new List<string>();
        using (SqlConnection cn = new SqlConnection(eip))
        {
            cn.Open();
            string sql = @"SELECT [appr_meet_id],[meetclass],[useclass],[creat_user] FROM [meeting_apprly] where @id=id ";

            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.AddWithValue("@id", del_id.Value);
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        ap_id = dr["appr_meet_id"].ToString();
                        mclass = dr["meetclass"].ToString();
                        uclass = dr["useclass"].ToString();
                        c_user = dr["creat_user"].ToString();
                    }
                }
            }
        }
        using (SqlConnection cn = new SqlConnection(eip))
        {
            cn.Open();
            string sql = @"SELECT id FROM [meeting_apprly] where appr_meet_id=@appr_meet_id and meetclass=@meetclass and useclass=@useclass and creat_user=@creat_user";

            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.AddWithValue("@appr_meet_id", ap_id);
                cmd.Parameters.AddWithValue("@meetclass", mclass);
                cmd.Parameters.AddWithValue("@useclass", uclass);
                cmd.Parameters.AddWithValue("@creat_user", c_user);
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        id.Add(dr["id"].ToString());
                    }
                }
            }
        }
        using (SqlConnection cn = new SqlConnection(eip))
        {
            cn.Open();
            foreach (string itemId in id)
            {
                string sql = @"DELETE FROM [meeting_apprly] WHERE id = @id";

                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@id", itemId);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        string mod = Request.QueryString["mod"];
        int month = Request.QueryString["month"] != null ? Convert.ToInt32(Request.QueryString["month"]) : DateTime.Now.Month;
        int year = Request.QueryString["year"] != null ? Convert.ToInt32(Request.QueryString["year"]) : DateTime.Now.Year;
        Response.Redirect("Meetingroom_calendar.aspx?mod=w&month=" + month + "&year=" + year);
    }
    protected void md2del_other_Click(object sender, EventArgs e)
    {
        using (SqlConnection cn = new SqlConnection(eip))
        {
            cn.Open();
            string sql = @"delete [meeting_apprly] where @id=id ";

            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.AddWithValue("@id", del_id.Value);
                cmd.ExecuteNonQuery();
            }
        }
        string mod = Request.QueryString["mod"];
        int month = Request.QueryString["month"] != null ? Convert.ToInt32(Request.QueryString["month"]) : DateTime.Now.Month;
        int year = Request.QueryString["year"] != null ? Convert.ToInt32(Request.QueryString["year"]) : DateTime.Now.Year;
        Response.Redirect("Meetingroom_calendar.aspx?mod=w&month=" + month + "&year=" + year);
    }

    protected void addmeeting_Click(object sender, EventArgs e)
    {
        m_no.Text = string.Empty;
        startdate.Text = string.Empty;
        starthour.Text = string.Empty;
        endhour.Text = string.Empty;
        enddate.Text = string.Empty;
        mtroom.SelectedValue = "0";
        meetclass.SelectedValue = "0";
        useclass.SelectedValue = "0";
        room_equipment.Text = string.Empty;
        room_number.Text = string.Empty;
        host.Text = string.Empty;
        number.Text = string.Empty;
        note.Text = string.Empty;
        del.Style.Add("display", "none");
        modify.Style.Add("display", "none");

        submitbt.Visible = true;
      
        ScriptManager.RegisterStartupScript(this, this.GetType(), "showmodal", "showModal1();", true);
    }
    [WebMethod]
    public static string GetHostNames()
    {
        string favoriteNames = string.Empty;
        string userId = HttpContext.Current.Session["user_id"] as string;
        if (string.IsNullOrEmpty(userId))
        {
            return favoriteNames;
        }
        string eip = WebConfigurationManager.ConnectionStrings["eip"].ConnectionString;

        // 資料庫查詢語句
        string query = "SELECT [favorite_host] FROM [eip_user] WHERE [user_id] = @userId";

        // 使用 SqlConnection 查詢資料
        using (SqlConnection conn = new SqlConnection(eip))
        {
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@userId", userId);

            conn.Open();

            var result = cmd.ExecuteScalar();
            if (result != null)
            {
                favoriteNames = result.ToString();  // 獲取 favorite_name 字段的值
            }
        }

        return favoriteNames;  // 返回用逗號隔開的 favorite_name 字符串
    }

    private void GenerateTimeOptions(DropDownList ddl)
    {
        ddl.Items.Clear();
        for (int hour = 8; hour < 18; hour++)
        {
            // 生成整點時間
            string time1 = FormatTime(hour, 0);
            ddl.Items.Add(new ListItem(time1, time1));

            // 生成半小時
            string time2 = FormatTime(hour, 30);
            ddl.Items.Add(new ListItem(time2, time2));
        }
    }

    // 格式化時間（將小時和分鐘格式化為 HH:MM 格式）
    private string FormatTime(int hour, int minute)
    {
        return string.Format("{0:D2}:{1:D2}", hour, minute);
    }
    protected void timeSelectStart_SelectedIndexChanged(object sender, EventArgs e)
    {
        starthour.Text = timeSelectStart.SelectedValue;
    }

    protected void timeSelectEnd_SelectedIndexChanged(object sender, EventArgs e)
    {
        endhour.Text = timeSelectEnd.SelectedValue;
    }

    protected void useclass_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (useclass.SelectedValue == "5")
        {
            other.Enabled = true;
        }
        else
        {
            other.Enabled = false;
        }

      //  ScriptManager.RegisterStartupScript(this, this.GetType(), "modal1", "showModal1();", true);
    }

    protected void ck_yes_CheckedChanged(object sender, EventArgs e)
    {

    }

    protected void Unnamed_CheckedChanged(object sender, EventArgs e)
    {

        other_reason_txt.Enabled = other_reason.Checked ? true : false;

    }


    protected void favorite_Click(object sender, ImageClickEventArgs e)
    {
        ImageButton ib = (ImageButton)sender;
        switch (ib.CommandArgument)
        {
            case "name":
                md3span.InnerText = "常用會議名稱";
                title_fav.InnerText = "註：最多可存3個常用名稱，限25字。";
                ViewState["show"] = "name";
                break;
            case "host":
                md3span.InnerText = "常用主持人";
                title_fav.InnerText = "註：最多可存3個常用名稱，限25字。";
                ViewState["show"] = "host";
                break;

        }
        show_favorite();

       ScriptManager.RegisterStartupScript(this, this.GetType(), "modal1", "$('#modal3').modal('show');", true);
    }
    protected void add_favorite_btn_Click(object sender, EventArgs e)
    {
        List<string> namelist = new List<string>();
        bool have_name = false;
        add_favorite_btn.CommandArgument = ViewState["show"].ToString();
        Button btn = (Button)sender;
        string num = "";
        if (btn.CommandArgument != "")
        {
            num = btn.CommandArgument == "name" ? "25" : "25";
            if (add_favorite_tb.Text != "")
            {
                using (SqlConnection cn = new SqlConnection(eip))
                {
                    string sql1 = @"select [favorite_" + btn.CommandArgument + @"] from [eip_user] where [user_id]=@user_id";
                    SqlCommand cmd = new SqlCommand(sql1, cn);
                    cmd.Parameters.AddWithValue("@user_id", Session["user_id"].ToString());
                    cn.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        foreach (string name in dr[0].ToString().Split(','))
                        {
                            namelist.Add(name);
                            if (name == add_favorite_tb.Text)
                                have_name = true;
                        }
                    }
                    cn.Close();
                }
                if (have_name)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert_other", "alert('已有相同常用備註請重新輸入!');", true);
                }
                else if (namelist.Count >= int.Parse(num))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert_other_count", "alert('常用備註最多只能" + num + "筆請先移除已建立備註再做新增!');", true);
                }
                else
                {
                    namelist.Add(add_favorite_tb.Text);
                    string str = string.Join(",", namelist.Where(s => s != ""));
                    using (SqlConnection cn = new SqlConnection(eip))
                    {
                        string sql = @"update [eip_user] set [favorite_" + btn.CommandArgument + @"]=@favorite_other where [user_id]=@user_id";
                        SqlCommand Cmd = new SqlCommand(sql, cn);
                        Cmd.Parameters.AddWithValue("@favorite_other", str);
                        Cmd.Parameters.AddWithValue("@user_id", Session["user_id"].ToString());
                        cn.Open();
                        Cmd.ExecuteScalar();
                        cn.Close();
                    }
                    add_favorite_tb.Text = "";
                }
            }

        }
        show_favorite();
    }
    protected void show_favorite()
    {
        string type = ViewState["show"].ToString();
        favorite_pl.Controls.Clear();
        int i = 1;
        using (SqlConnection cn = new SqlConnection(eip))
        {
            cn.Open();
            string sql = @"SELECT  [favorite_" + type + @"] FROM [eip_user] where [user_id]=" + Session["user_id"].ToString();
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    foreach (string name in dr[0].ToString().Split(','))
                    {
                        if (name != "")
                        {
                            Label fav_bt = new Label();

                            //Button fav_bt = new Button();
                            fav_bt.Text = i + ".   " + name;
                            fav_bt.CssClass = "gray_box";
                            if (type == "name")
                            {
                                fav_bt.Attributes.Add("onclick", "setmeetValue('" + name + "')");
                            }
                            if (type == "host")
                            {
                                fav_bt.Attributes.Add("onclick", "setHostValue('" + name + "')");
                            }

                            Button delbt = new Button();
                            delbt.Text = "刪除";
                            delbt.ID = "del_" + type + "_" + i;
                            delbt.CssClass = "btn BT_red ml-4";
                            delbt.Attributes.Add("style", "padding: 2px 8px;height:60%;border-radius:5px;background-color: #B83F1F;");
                            delbt.CommandName = type;
                            delbt.CommandArgument = name;
                            //delbt.Attributes.Add("onclick", "return confirm('您確定要刪除此項目嗎？')");
                            delbt.OnClientClick = "return confirm('您確定要刪除此項目嗎？')";
                            delbt.Click += delete_favorite;
                            ScriptManager.GetCurrent(this.Page).RegisterAsyncPostBackControl(delbt);
                            favorite_pl.Controls.Add(new LiteralControl("<div style='padding: 0px 20px 20px 20px;display: flex;align-items: center;'>"));
                            favorite_pl.Controls.Add(fav_bt);
                            favorite_pl.Controls.Add(delbt);
                            favorite_pl.Controls.Add(new LiteralControl("</div>"));
                        }
                        i++;
                    }
                }
            }
        }

    }


    protected void delete_favorite(object sender, EventArgs e)
    {

        Button bt = (Button)sender;
        string favoriteType = bt.CommandName;  // 這是用來標識要刪除的類型
        string favoriteName = bt.CommandArgument;  // 這是要刪除的名稱
        List<string> namelist = new List<string>();
        using (SqlConnection cn = new SqlConnection(eip))
        {
            string sql1 = @"select [favorite_" + bt.CommandName + @"] from [eip_user] where [user_id]=@user_id";
            SqlCommand cmd = new SqlCommand(sql1, cn);
            cmd.Parameters.AddWithValue("@user_id", Session["user_id"].ToString());
            cn.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                foreach (string name in dr[0].ToString().Split(','))
                {
                    namelist.Add(name);
                }
            }
            cn.Close();
        }
        string str = string.Join(",", namelist.Where(s => s != "").Where(s => s != bt.CommandArgument));
        using (SqlConnection cn = new SqlConnection(eip))
        {
            string sql = @"update [eip_user] set [favorite_" + bt.CommandName + @"]=@favorite_other where [user_id]=@user_id";
            SqlCommand Cmd = new SqlCommand(sql, cn);
            Cmd.Parameters.AddWithValue("@favorite_other", str);
            Cmd.Parameters.AddWithValue("@user_id", Session["user_id"].ToString());
            cn.Open();
            Cmd.ExecuteScalar();
            cn.Close();
        }

        show_favorite();

    }
}