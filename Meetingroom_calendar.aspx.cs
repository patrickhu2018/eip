using Newtonsoft.Json;
using NPOI.HSSF.Record.Formula.Functions;
using NPOI.SS.Util;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using System;
using System.Activities.Statements;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.DirectoryServices;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Configuration;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Meetingroom_calendar : System.Web.UI.Page
{
    string eip = WebConfigurationManager.ConnectionStrings["eip"].ConnectionString;
    private DateTime currentDate = DateTime.Now;  // 當前日期
    private int currentWeekIndex = 1;
    protected void Page_Load(object sender, EventArgs e)
    {

        string mod = Request.QueryString["mod"];
        
        int month = Request.QueryString["month"] != null ? Convert.ToInt32(Request.QueryString["month"]) : DateTime.Now.Month;
        int year = Request.QueryString["year"] != null ? Convert.ToInt32(Request.QueryString["year"]) : DateTime.Now.Year;
        int day = Request.QueryString["day"] != null ? Convert.ToInt32(Request.QueryString["day"]) : DateTime.Now.Day;
        int week = Request.QueryString["wk"] != null ? Convert.ToInt32(Request.QueryString["wk"]) : 1;

        
        MasterPage3 master = (MasterPage3)this.Master;
        Label masterLabel = (Label)master.FindControl("page_title");
        masterLabel.Text = "會議室使用狀況";
        Literal link_li = (Literal)master.FindControl("link_li");


    

        if (!IsPostBack)
        {
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "modal1", "alert('" + Session["user_right_id"].ToString() + "');", true);
            meeting_type_bt.Visible = Session["user_right_id"].ToString() == "1"; //系統管理者可變更會議類型
            used_type_bt.Visible = Session["user_right_id"].ToString() == "1"; //系統管理者可變更使用類型

            if (Request.QueryString["month"] != null)
            {
                month = Request.QueryString["month"] != null ? Convert.ToInt32(Request.QueryString["month"]) : DateTime.Now.Month;
                year = Request.QueryString["year"] != null ? Convert.ToInt32(Request.QueryString["year"]) : DateTime.Now.Year;
                day = Request.QueryString["day"] != null ? Convert.ToInt32(Request.QueryString["day"]) : DateTime.Now.Day;
                week = Convert.ToInt32(Request.QueryString["wk"]);
            }
            else
            {
                currentWeekIndex = GetWeekOfMonth(DateTime.Today);
              


                if (week == 0)
                    week = currentWeekIndex;




                //  ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('" + Session["m"].ToString() + "');", true);

                if (Session["y"] != null && Session["y"].ToString() != "0")
                {
                    year = Convert.ToInt32(Session["y"].ToString());
                }
                else
                    year = DateTime.Now.Year;

                if (Session["m"] != null && Session["m"].ToString() != "0")
                {
                    month = Convert.ToInt32(Session["m"].ToString());
                }
                else
                    month = DateTime.Now.Month;

                if (Session["day"] != null)
                {
                    day = Convert.ToInt32(Session["day"].ToString());
                }
                if (Session["week"] != null)
                {
                    week = Convert.ToInt32(Session["week"].ToString());
                }
            }
            if (Session["receive_start"] != null && Request.QueryString["month"] == null && Session["receive_start"].ToString()!="")
            {
                year = DateTime.Parse(Session["receive_start"].ToString()).Year;
                month = DateTime.Parse(Session["receive_start"].ToString()).Month;
                Session["y"] = year;
                Session["m"] = month;
               
                currentWeekIndex = GetWeekOfMonth(DateTime.Parse(Session["receive_start"].ToString()));
                Session["week"] = currentWeekIndex;

                if (Session["receive_end"] != null && Session["receive_end"].ToString() != "")
                {
                    double days = (DateTime.Parse(Session["receive_end"].ToString()) - DateTime.Parse(Session["receive_start"].ToString())).TotalDays;
                    if (days > 7)
                        Session["ddl1"] = "0";
                    else
                        Session["ddl1"] = "1";

                }

            }

            if (Session["ddl1"] != null)
            {
                ddl1.SelectedValue = Session["ddl1"].ToString();
                if (ddl1.SelectedValue=="0")
                  mod ="m";
                if (ddl1.SelectedValue == "1")
                    mod = "w";
                if (ddl1.SelectedValue == "2")
                    mod = "d";
            }

         




            if (link_li != null) link_li.Text += "<li>日曆檢視</li>";
            getneetroom();
            appr_group.Text = Session["group_name"].ToString();
            appr_user.Text = Session["user_name"].ToString();
            ddl2.SelectedValue = Session["ddl2"] == null ? "" : Session["ddl2"].ToString();
            ddl3.SelectedValue = Session["ddl3"] == null ? "0" : Session["ddl3"].ToString();
        
            if (mod == "m")
            {
                ddl1.SelectedValue = "0";
                panle_month.Visible = true;
                panle_week.Visible = false;
                panle_day.Visible = false;
                GenerateCalendar(month, year, ddl2.SelectedValue, ddl3.SelectedValue);
            }
            if (mod == "w")
            {
                ddl1.SelectedValue = "1";
                panle_month.Visible = false;
                panle_week.Visible = true;
                panle_day.Visible = false;
                GenerateWeeklyCalendar(month, year, week, ddl2.SelectedValue, ddl3.SelectedValue);
            }
            if (mod == "d")
            {
                Calendar.Visible = true;
                ddl1.SelectedValue = "2";
                panle_month.Visible = false;
                panle_week.Visible = false;
                panle_day.Visible = true;
                GenerateDayCalendar(month, year, day, ddl2.SelectedValue, ddl3.SelectedValue);
            }
            GenerateTimeOptions(timeSelectStart);

            // 生成結束時間選項
            GenerateTimeOptions(timeSelectEnd);
            ViewState["show"] = "name";
            show_favorite();
            ViewState["meeting_type"] = "meetingtype";
            show_meeting_item();



            if (Session["ddl2"] != null)
            {
                ddl2.SelectedValue = Session["ddl2"].ToString();
                ddl3.SelectedValue = Session["ddl3"] == null ? "0" : Session["ddl3"].ToString();
                if (mod == "m")
                {
                    GenerateCalendar(month, year, ddl2.SelectedValue, ddl3.SelectedValue);
                }
                if (mod == "w")
                {
                    GenerateWeeklyCalendar(month, year, week, ddl2.SelectedValue, ddl3.SelectedValue);
                }
                if (mod == "d")
                {
                    GenerateDayCalendar(month, year, day, ddl2.SelectedValue, ddl3.SelectedValue);
                }
            }
            if (Session["ddl3"] != null)
            {
                ddl2.SelectedValue = Session["ddl2"] == null ? "" : Session["ddl2"].ToString();
                ddl3.SelectedValue = Session["ddl3"].ToString();
                if (mod == "m")
                {
                    GenerateCalendar(month, year, ddl2.SelectedValue, ddl3.SelectedValue);
                }
                if (mod == "w")
                {
                    GenerateWeeklyCalendar(month, year, week, ddl2.SelectedValue, ddl3.SelectedValue);
                }
                if (mod == "d")
                {
                    GenerateDayCalendar(month, year, day, ddl2.SelectedValue, ddl3.SelectedValue);
                }
            }

            
        }
        getmeet();
        if (ViewState["show"] != null)
        {
            show_favorite();
        }
        if (ViewState["meeting_type"] != null)
        {
            show_meeting_item();
        }

    }
    private void getmeet()
    {
        StringBuilder sb = new StringBuilder();
        using (SqlConnection cn = new SqlConnection(eip))
        {
            cn.Open();
            string sql = @"SELECT [id],[meet_name],[color] FROM [meeting_equipment] order by LEN(meet_name) ,CAST(meet_name as nvarchar)";
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {

                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    sb.Append("<div class='d-flex align-items-center mb-1 mt-1' style='min-width: 80px;'>");
                    sb.Append("<div class='meetname mr-1' style='background-color:" + dr["color"].ToString() + "'></div>");
                    sb.Append("<span style='font-size: 0.8rem;margin-right:0.7rem;'>" + dr["meet_name"].ToString() + "</span>");
                    sb.Append("</div>");
                }

            }
        }
        m_meet.InnerHtml = sb.ToString();
    }
    private void getneetroom()
    {
        string favmt_id = "";
        List<int> favoriteMeetIds = new List<int>();
        using (SqlConnection cn = new SqlConnection(eip))
        {
            cn.Open();
            string sql = @"SELECT  [favorite_meet] FROM [eip_user] where user_id=@user_id";
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.AddWithValue("@user_id", Session["user_id"].ToString());
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    favmt_id = dr["favorite_meet"].ToString();
                }

            }
        }
        string[] favmtArray = favmt_id.Split(',');
        foreach (string id in favmtArray)
        {
            if (id.Trim().Length > 0) // 確保字串不是空白
            {
                int result = int.Parse(id);
                favoriteMeetIds.Add(result);
            }
        }

        using (SqlConnection cn = new SqlConnection(eip))
        {
            cn.Open();
            mtroom.Items.Clear();
            ddl3.Items.Clear();
            mtroom.Items.Add(new ListItem("請選擇", "0"));
            ddl3.Items.Add(new ListItem("所有會議室", "0"));
            string sql = @"SELECT  [id],[meet_name] FROM [meeting_equipment]";
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                SqlDataReader dr = cmd.ExecuteReader();
                // 儲存最喜愛會議室項目
                List<ListItem> favoriteItems = new List<ListItem>();
                // 儲存其他會議室項目
                List<ListItem> otherItems = new List<ListItem>();

                while (dr.Read())
                {
                    if (favoriteMeetIds.Contains(Convert.ToInt32(dr["id"])))
                    {
                        favoriteItems.Add(new ListItem("★" + dr["meet_name"].ToString(), dr["id"].ToString()));
                    }
                    else
                    {
                        otherItems.Add(new ListItem(dr["meet_name"].ToString(), dr["id"].ToString()));
                    }

                    ddl3.Items.Add(new ListItem(dr["meet_name"].ToString(), dr["id"].ToString()));
                }
                foreach (var item in favoriteItems)
                {
                    mtroom.Items.Add(item);
                }

                foreach (var item in otherItems)
                {
                    mtroom.Items.Add(item);
                }

            }
        }
    }
    protected void mtroom_SelectedIndexChanged(object sender, EventArgs e)
    {
        show_mtroom_Selected(mtroom.SelectedValue);
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
                    mrdevices.Visible = true;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "showMrdevices", "document.getElementById('mrdevices').style.display='block'", true);
                }

            }
        }
        if (mtroom_value == "0")
        {
            room_equipment.Text = room_number.Text = "";
            //mrdevices.Visible = false;
        }
    }
    private void GenerateCalendar(int month, int year, string ddl2, string ddl3)
    {
        StringBuilder sb = new StringBuilder();
        DateTime firstDayOfMonth = new DateTime(year, month, 1);
        DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
        int startDayOfWeek = (int)firstDayOfMonth.DayOfWeek; // 0 = Sunday, 6 = Saturday
        int daysInMonth = DateTime.DaysInMonth(year, month);

        sb.Append("<table class='calendar'>");
        sb.Append("<thead><tr>");
        sb.Append("<th>週日</th><th>週一</th><th>週二</th><th>週三</th><th>週四</th><th>週五</th><th>週六</th>");
        sb.Append("</tr></thead>");
        sb.Append("<tbody>");

        int currentDay = 1;
        HiddenField yd = new HiddenField();

        for (int row = 0; row < 6; row++)
        {
            sb.Append("<tr>");
            for (int col = 0; col < 7; col++)
            {
                if (row == 0 && col < startDayOfWeek)
                {
                    sb.Append("<td class='empty'></td>");
                }
                else if (currentDay <= daysInMonth)
                {
                    bool isToday = currentDay == DateTime.Now.Day && month == DateTime.Now.Month && year == DateTime.Now.Year;
                    sb.Append("<td style='position:relative;'>");
                    if (isToday)
                    {
                        sb.Append("<div style='height: 30%;display: flex; align-items: center; justify-content: center;'>");
                        sb.Append("<div style='background-color: #1E76E8; color: white; border-radius: 50%; width: 25px; height: 25px; display: flex; align-items: center; justify-content: center;'>" + currentDay + "</div>");
                        sb.Append("</div>");
                    }
                    else
                    {
                        sb.Append("<div style='height: 30%;'>" + currentDay + "</div>");
                    }
                    sb.Append("<div style='height: 70%;display:flex;flex-direction: column;padding-bottom: 20px;'>");

                    // 用來儲存當天所有會議按鈕的 HTML（全部會顯示在浮窗中）
                    List<string> allButtons = new List<string>();

                    // 依日期查詢會議資料
                    using (SqlConnection cn = new SqlConnection(eip))
                    {
                        cn.Open();
                        string sql = @"SELECT t1.[id], [meeting_name], [use_start], [use_end], [state], t2.color, t2.meet_name
                                   FROM [meeting_apprly] as t1 
                                   LEFT JOIN [meeting_equipment] as t2 on t1.appr_meet_id = t2.id
                                   WHERE CONVERT(date, [use_start]) <= @currentDate AND CONVERT(date, [use_end]) >= @currentDate";

                        if (ddl2 == "1")
                        {
                            sql += " and appr_user = @appr_user";
                        }
                        else if (ddl2 == "2")
                        {
                            sql += " and appr_group = @appr_group";
                        }
                        if (ddl3 != "0")
                        {
                            sql += " and appr_meet_id = @appr_meet_id";
                        }
                        using (SqlCommand cmd = new SqlCommand(sql, cn))
                        {
                            cmd.Parameters.AddWithValue("@currentDate", new DateTime(year, month, currentDay));
                            if (ddl2 == "1")
                            {
                                cmd.Parameters.AddWithValue("@appr_user", Session["user_name"].ToString());
                            }
                            else if (ddl2 == "2")
                            {
                                cmd.Parameters.AddWithValue("@appr_group", Session["group_name"].ToString());
                            }
                            if (ddl3 != "0")
                            {
                                cmd.Parameters.AddWithValue("@appr_meet_id", ddl3);
                            }
                            using (SqlDataReader dr = cmd.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    int id = int.Parse(dr["id"].ToString());
                                    string meetingName = dr["meet_name"].ToString() + "_" + dr["meeting_name"].ToString();
                                    Button dateBtn = new Button
                                    {
                                        ID = "date_" + id,
                                        CssClass = "datebt",
                                        Text = meetingName,
                                        UseSubmitBehavior = false,
                                        ToolTip = meetingName
                                    };
                                    dateBtn.Style["background-color"] = dr["color"].ToString();

                                    StringWriter writer = new StringWriter();
                                    HtmlTextWriter htmlWriter = new HtmlTextWriter(writer);
                                    dateBtn.RenderControl(htmlWriter);
                                    string renderedButton = writer.ToString();
                                    allButtons.Add(renderedButton);
                                }
                            }
                        }
                    }

                    // 畫面上僅顯示前三個按鈕
                    int visibleCount = Math.Min(allButtons.Count, 3);
                    for (int i = 0; i < visibleCount; i++)
                    {
                        sb.Append(allButtons[i]);
                    }

                    // 不論會議數量多少，都建立一個浮窗 div，內含所有會議按鈕
                    if (allButtons.Count > 3)
                    {
                        sb.Append("<div class='more-meetings' style='display:none;position:absolute;top:0;left:0;background-color:white;border:1px solid #ccc;padding:10px;z-index:100;'>");
                        foreach (string btnHtml in allButtons)
                        {
                            sb.Append(btnHtml);
                        }
                        sb.Append("</div>");
                    }

                    // hiddenfield 儲存當天日期
                    yd.Value = year + "-" + month.ToString("00") + "-" + currentDay.ToString("00");
                    StringWriter ydwriter = new StringWriter();
                    HtmlTextWriter ydhtmlWriter = new HtmlTextWriter(ydwriter);
                    yd.RenderControl(ydhtmlWriter);
                    sb.Append(ydwriter.ToString());

                    sb.Append("</div>");
                    sb.Append("</td>");
                    currentDay++;
                }
                else
                {
                    sb.Append("<td class='empty'></td>");
                }
            }
            sb.Append("</tr>");
        }
        sb.Append("</tbody>");
        sb.Append("</table>");

        calendar_month.InnerHtml = sb.ToString();
    }
    protected void today_month_Click(object sender, EventArgs e)
    {
        int month = DateTime.Now.Month;
        int year = DateTime.Now.Year;
        string url = "Meetingroom_calendar.aspx?mod=m&month=" + month + "&year=" + year;
        Response.Redirect(url);

    }
    public string CurrentMonth()
    {
        int month = 0;
        int year = 0;
   

        if (Request.QueryString["month"] != null)
        {
            month = Request.QueryString["month"] != null ? Convert.ToInt32(Request.QueryString["month"]) : DateTime.Now.Month;
            year = Request.QueryString["year"] != null ? Convert.ToInt32(Request.QueryString["year"]) : DateTime.Now.Year;
        }
        else
        {
            if (Session["y"] != null && Session["y"].ToString()!="0")
            {
                year = Convert.ToInt32(Session["y"].ToString());
            }
            else
                year = DateTime.Now.Year;


            if (Session["m"] != null && Session["m"].ToString() != "0")
            {
                month = Convert.ToInt32(Session["m"].ToString());
            }
            else
                month = DateTime.Now.Month;
        }

            DateTime date = new DateTime(year, month, 1);
        return date.ToString("yyyy年MM月");
    }

    public string GetPreviousMonthUrl()
    {
        int month = 0;
        int year = 0;


        if (Request.QueryString["month"] != null)
        {
            month = Request.QueryString["month"] != null ? Convert.ToInt32(Request.QueryString["month"]) : DateTime.Now.Month;
            year = Request.QueryString["year"] != null ? Convert.ToInt32(Request.QueryString["year"]) : DateTime.Now.Year;
        }
        else
        {
            if (Session["y"] != null)
            {
                year = Convert.ToInt32(Session["y"].ToString());
            }
            if (Session["m"] != null)
            {
                month = Convert.ToInt32(Session["m"].ToString());
            }
        }


        if (month == 1)
        {
            month = 12;
            year--;
        }
        else
        {
            month--;
        }
        Session["m"] = month;
        Session["y"] = year;
        return "Meetingroom_calendar.aspx?mod=m&month=" + month + "&year=" + year;
    }

    public string GetNextMonthUrl()
    {
        int month = 0;
        int year = 0;


        if (Request.QueryString["month"] != null)
        {
            month = Request.QueryString["month"] != null ? Convert.ToInt32(Request.QueryString["month"]) : DateTime.Now.Month;
            year = Request.QueryString["year"] != null ? Convert.ToInt32(Request.QueryString["year"]) : DateTime.Now.Year;
        }
        else
        {
            if (Session["y"] != null)
            {
                year = Convert.ToInt32(Session["y"].ToString());
            }
            if (Session["m"] != null)
            {
                month = Convert.ToInt32(Session["m"].ToString());
            }
        }
        if (month == 12)
        {
            month = 1;
            year++;
        }
        else
        {
            month++;
        }
     
        Session["m"] = month;
        Session["y"] = year;
        return "Meetingroom_calendar.aspx?mod=m&month=" + month + "&year=" + year;
    }

    private void GenerateWeeklyCalendar(int month, int year, int weekIndex, string ddl2, string ddl3)
    {
        StringBuilder sb = new StringBuilder();
        DateTime firstDayOfMonth = new DateTime(year, month, 1);
        DateTime startOfWeek = firstDayOfMonth;
        while (startOfWeek.DayOfWeek != DayOfWeek.Sunday)
        {
            startOfWeek = startOfWeek.AddDays(1);
        }
        startOfWeek = startOfWeek.AddDays(7 * (weekIndex - 1));

        


        DateTime[] weekDates = new DateTime[7];
        for (int i = 0; i < 7; i++)
        {
            weekDates[i] = startOfWeek.AddDays(i);

            if (i == 0)
            {
                ViewState["sunday"] = weekDates[i];
                Session["sunday"] = weekDates[i];
            }
            if (i == 6)
            {
                ViewState["saturday"] = weekDates[i];
                Session["saturday"] = weekDates[i];
            }
        }

        sb.Append("<table class='calendar week'>");
        sb.Append("<thead><tr>");
        sb.Append("<th style='width:5%;border:0px;'></th>");
        string[] daysOfWeek = { "週日", "週一", "週二", "週三", "週四", "週五", "週六" };
        for (int i = 0; i < 7; i++)
        {
            bool isToday = weekDates[i].Day == DateTime.Now.Day && month == DateTime.Now.Month && year == DateTime.Now.Year;
            if (isToday)
            {
                sb.Append("<th style='width:10%;'>" + daysOfWeek[i] + "<br>");
                sb.Append("<div style='height: 30%;display: flex; align-items: center; justify-content: center;'>");
                sb.Append("<div style='background-color: #1E76E8; color: white; border-radius: 50%; width: 25px; height: 25px; display: flex; align-items: center; justify-content: center;'>" + weekDates[i].Day + "</div></div></th>");
            }
            else
            {
                sb.Append("<th style='width:10%;'>" + daysOfWeek[i] + "<br>" + weekDates[i].Day + "</th>");
            }
        }
        sb.Append("</tr></thead>");
        sb.Append("<tbody>");


        int[] dayscount = { 0, 0, 0, 0, 0, 0, 0 };

        // 每個時段從 8:00 到 23:00
        for (int hour = 8; hour <= 23; hour++)
        {
            sb.Append("<tr>");
            sb.Append("<td style='width:5%;border:0px; pointer-events: none;position: relative;'><span class='hourtd'>" + hour.ToString("D2") + ":00</span></td>");
             
            for (int day = 0; day < 7; day++)
            {
               
                DateTime currentDate = weekDates[day];
                sb.Append("<td style='width:10%;position: relative;'>");
                sb.Append("<div class='d-felx' style='width:100%;position:relative;'>");

                // 先取得當前時段的會議資料集合
                List<Dictionary<string, object>> events = new List<Dictionary<string, object>>();
                using (SqlConnection cn = new SqlConnection(eip))
                {
                    cn.Open();
                    string sql = @"SELECT t1.[id], [meeting_name], [use_start], [use_end], [state], t2.color, t2.meet_name
                               FROM [meeting_apprly] as t1 
                               LEFT JOIN [meeting_equipment] as t2 on t1.appr_meet_id = t2.id
                               WHERE CONVERT(date, [use_start]) <= @currentDate AND CONVERT(date, [use_end]) >= @currentDate";
                    if (ddl2 == "1")
                    {
                        sql += " and appr_user=@appr_user";
                    }
                    else if (ddl2 == "2")
                    {
                        sql += " and appr_group=@appr_group";
                    }
                  
                    if (ddl3 != "0")
                    {
                       
                        sql += " and appr_meet_id=@appr_meet_id";
                    }
                    sql += " order by use_start ASC";
                    using (SqlCommand cmd = new SqlCommand(sql, cn))
                    {
                        // 注意：這裡以當前日期的日期部分作為參數
                        cmd.Parameters.AddWithValue("@currentDate", new DateTime(currentDate.Year, currentDate.Month, currentDate.Day));
                        if (ddl2 == "1")
                        {
                            cmd.Parameters.AddWithValue("@appr_user", Session["user_name"].ToString());
                        }
                        else if (ddl2 == "2")
                        {
                            cmd.Parameters.AddWithValue("@appr_group", Session["group_name"].ToString());
                        }
                        if (ddl3 != "0")
                        {
                            cmd.Parameters.AddWithValue("@appr_meet_id", ddl3);
                        }
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                DateTime meetingStart = DateTime.Parse(dr["use_start"].ToString());
                                // 只處理在這個小時內開始的會議（你也可以根據需求調整判斷邏輯）  20250318 改一天
                                if (meetingStart.Hour == hour)
                                {
                                    Dictionary<string, object> evt = new Dictionary<string, object>();
                                    evt["id"] = dr["id"].ToString();
                                    evt["meeting_name"] = Regex.Replace(dr["meeting_name"].ToString(), "[()\\-_:]", " | ");
                                    evt["meet_name"] = dr["meet_name"].ToString();
                                    evt["color"] = dr["color"].ToString();
                                    evt["use_start"] = meetingStart;
                                    evt["use_end"] = DateTime.Parse(dr["use_end"].ToString());
                                    events.Add(evt);
                                }
                            }
                        }
                      
                    }
                }
               // if (meetingStart.Day == currentDate.Day) //20250318 改一天
             // 若有會議則按 Google 日曆風格依序錯開
                  int eventCount = events.Count;
              
                if (eventCount > 0)
                {
                    // 設定左右間距百分比（margin）
                    float margin = 3f;
                    // 計算每個事件的寬度

                    int datCount = eventCount;
                    datCount = getsamedatmettingcount(new DateTime(currentDate.Year, currentDate.Month, currentDate.Day), ddl2, ddl3);
                    float eventWidth = (100 - (datCount - 1) * margin) / datCount;

                   
                    for (int i = 0; i < eventCount; i++)
                    {
                        var evt = events[i];
                        DateTime meetingStart = (DateTime)evt["use_start"];
                        DateTime meetingEnd = (DateTime)evt["use_end"];
                        // 根據開始分鐘調整 top
                        string top = meetingStart.Minute != 0 ? "0" : "-25";
                        int eventHeight = (int)((meetingEnd - meetingStart).TotalHours * 50);
                        // 計算左邊距，依據事件在此時段的順序
                        float left = i * (eventWidth + margin);
                        left = dayscount[day] * (eventWidth + margin);


                        // 輸出 div，設定絕對定位，並加入錯開的效果
                        sb.Append("<div role='button' id='bt_" + evt["id"] + "' class='datebt' style='position:absolute; top:" + top + "px; left:" + left.ToString("F2") + "%; background-color:" + 
                            evt["color"] + "; width:" + eventWidth.ToString("F2") + "%; height:" + eventHeight + "px; z-index:1; overflow:hidden; writing-mode:vertical-lr; " +
                            "text-orientation:upright; display:flex; align-items:center;' title='" + evt["meet_name"] + " | " + evt["meeting_name"] + "' >");
                        sb.Append(evt["meet_name"] + " | " + evt["meeting_name"]);
                        sb.Append("</div>");

                        dayscount[day]++;
                    }
                }

                sb.Append("</div>");
                sb.Append("</td>");
            }
            sb.Append("</tr>");
        }

        sb.Append("</tbody>");
        sb.Append("</table>");

        calendar_week.InnerHtml = sb.ToString();
    
    }



    public string PreviousWeek()
    {
        int month = 0;
        int year = 0;


        if (Request.QueryString["month"] != null)
        {
            month = Request.QueryString["month"] != null ? Convert.ToInt32(Request.QueryString["month"]) : DateTime.Now.Month;
            year = Request.QueryString["year"] != null ? Convert.ToInt32(Request.QueryString["year"]) : DateTime.Now.Year;
        }
        else
        {
            if (Session["y"] != null)
            {
                year = Convert.ToInt32(Session["y"].ToString());
            }
            if (Session["m"] != null)
            {
                month = Convert.ToInt32(Session["m"].ToString());
            }
        }


        Session["m"] = month;
        Session["y"] = year;
        currentWeekIndex = GetWeekOfMonth(DateTime.Parse(ViewState["sunday"].ToString()));
        Session["week"] = currentWeekIndex;

        DateTime nextsunday = DateTime.Parse(ViewState["sunday"].ToString()).AddDays(-7);
        year = nextsunday.Year;
        month = nextsunday.Month;
        
        currentWeekIndex = GetWeekOfMonth(nextsunday);

        //currentWeekIndex = Convert.ToInt32(Request.QueryString["wk"]);




        //if (Request.QueryString["wk"] == null)
        //    currentWeekIndex = GetWeekOfMonth(DateTime.Today);

        //currentWeekIndex--;

        // if (currentWeekIndex > 4)
        //     currentWeekIndex = 3;

        //if (currentWeekIndex < 1)
        //{
        //    currentWeekIndex = 4; // 限制最小值
        //    if (currentDate.AddDays(-7).Month != month)
        //    {
        //        if (month == 1)
        //        {
        //            month = 12;
        //            year--;
        //        }
        //        else
        //        {
        //            month--;
        //        }
        //    }
        //}


        return "Meetingroom_calendar.aspx?mod=w&wk=" + currentWeekIndex + "&month=" + month + "&year=" + year;
    }
    public string NextWeek()
    {
        int month = 0;
        int year = 0;


        if (Request.QueryString["month"] != null)
        {
            month = Request.QueryString["month"] != null ? Convert.ToInt32(Request.QueryString["month"]) : DateTime.Now.Month;
            year = Request.QueryString["year"] != null ? Convert.ToInt32(Request.QueryString["year"]) : DateTime.Now.Year;
        }
        else
        {
            if (Session["y"] != null)
            {
                year = Convert.ToInt32(Session["y"].ToString());
            }
            if (Session["m"] != null)
            {
                month = Convert.ToInt32(Session["m"].ToString());
            }
        }
        Session["m"] = month;
        Session["y"] = year;
        currentWeekIndex = GetWeekOfMonth(DateTime.Parse(ViewState["sunday"].ToString()));
        Session["week"] = currentWeekIndex;
        DateTime nextsunday = DateTime.Parse(ViewState["sunday"].ToString()).AddDays(7);
        year = nextsunday.Year;
        month = nextsunday.Month;

      

         // ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('" + nextsunday.ToString() + "');", true);
        currentWeekIndex = GetWeekOfMonth(nextsunday);
      
        //currentWeekIndex = Convert.ToInt32(Request.QueryString["wk"]);

        //if(Request.QueryString["wk"]==null)
        //currentWeekIndex = GetWeekOfMonth(DateTime.Today);

        //currentWeekIndex++;


        //int month = Request.QueryString["month"] != null ? Convert.ToInt32(Request.QueryString["month"]) : DateTime.Now.Month;
        //int year = Request.QueryString["year"] != null ? Convert.ToInt32(Request.QueryString["year"]) : DateTime.Now.Year;

        // if (currentWeekIndex > 4)
        //    currentWeekIndex = 1; // 限制最小值


        //    if (currentDate.AddDays(7).Month != month)
        //    {
        //        if (month == 12)
        //        {
        //            month = 1;
        //            year++;
        //        }
        //        else
        //        {
        //            month++;

        //        }
        //    }
        //}
        return "Meetingroom_calendar.aspx?mod=w&wk=" + currentWeekIndex + "&month=" + month + "&year=" + year;
    }
    protected void today_week_Click(object sender, EventArgs e)
    {
        int month = DateTime.Now.Month;
        int year = DateTime.Now.Year;
        DateTime today = DateTime.Today;
        DateTime firstDayOfMonth = new DateTime(year, month, 1);
        DateTime firstSundayOfMonth = firstDayOfMonth;
        while (firstSundayOfMonth.DayOfWeek != DayOfWeek.Sunday)
        {
            firstSundayOfMonth = firstSundayOfMonth.AddDays(1);
        }
        int daysSinceFirstSunday = (today - firstSundayOfMonth).Days;
        int currentWeek = daysSinceFirstSunday / 7 + 1;
        string url = "Meetingroom_calendar.aspx?mod=w&wk=" + currentWeek + "&month=" + month + "&year=" + year;
        Response.Redirect(url);

    }
    private void GenerateDayCalendar(int month, int year, int day, string ddl2, string ddl3)
    {
        StringBuilder sb = new StringBuilder();
        DateTime targetDate = new DateTime(year, month, day);
        sb.Append("<table class='calendar day'>");
        sb.Append("<thead><tr>");
        sb.Append("<th style='width:6%;border:0px;'></th>");
        sb.Append("<th style='width:85%;border:0px;'>");
        sb.Append("<span style='font-size: 18px; font-weight: bold;display: flex;justify-content: start;'>");
        string dayOfWeek = targetDate.DayOfWeek.ToString();
        Dictionary<string, string> dayOfWeekCn = new Dictionary<string, string>
    {
        { "Sunday", "週日" },
        { "Monday", "週一" },
        { "Tuesday", "週二" },
        { "Wednesday", "週三" },
        { "Thursday", "週四" },
        { "Friday", "週五" },
        { "Saturday", "週六" }
    };
        string weekDayInChinese = dayOfWeekCn.ContainsKey(dayOfWeek) ? dayOfWeekCn[dayOfWeek] : dayOfWeek;
        int dayOfMonth = targetDate.Day;
        sb.Append(weekDayInChinese + "<br>" + dayOfMonth);
        sb.Append("</span>");
        sb.Append("</th>");
        sb.Append("</tr></thead>");
        sb.Append("<tbody>");

        Dictionary<int, List<Meeting>> meetingsPerHour = new Dictionary<int, List<Meeting>>();
        using (SqlConnection cn = new SqlConnection(eip))
        {
            cn.Open();
            string sql = @"SELECT t1.[id], [meeting_name], [use_start], [use_end], [state],t2.color,t2.meet_name
                       FROM [meeting_apprly]  as t1 
                       left join [meeting_equipment] as t2 on t1.appr_meet_id=t2.id
                       WHERE CONVERT(date, [use_start]) <= @currentDate 
                       AND CONVERT(date, [use_end]) >= @currentDate";

            if (ddl2 == "1")
            {
                sql += @" and appr_user=@appr_user";
            }
            else if (ddl2 == "2")
            {
                sql += @" and appr_group=@appr_group";
            }
            if (ddl3 != "0")
            {
                sql += @" and appr_meet_id=@appr_meet_id";
            }
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.AddWithValue("@currentDate", targetDate);
                if (ddl2 == "1")
                {
                    cmd.Parameters.AddWithValue("@appr_user", Session["user_name"].ToString());
                }
                else if (ddl2 == "2")
                {
                    cmd.Parameters.AddWithValue("@appr_group", Session["group_name"].ToString());
                }
                if (ddl3 != "0")
                {
                    cmd.Parameters.AddWithValue("@appr_meet_id", ddl3);
                }
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        DateTime meetingStart = DateTime.Parse(dr["use_start"].ToString());
                        DateTime meetingEnd = DateTime.Parse(dr["use_end"].ToString());
                        int meetingHour = meetingStart.Hour;
                        Meeting meeting = new Meeting
                        {
                            Id = (int)dr["id"],
                            Name = dr["meet_name"].ToString() + " | " + dr["meeting_name"].ToString(),
                            Start = meetingStart,
                            End = meetingEnd,
                            State = dr["state"].ToString(),
                            color = dr["color"].ToString(),
                        };

                        if (!meetingsPerHour.ContainsKey(meetingHour))
                        {
                            meetingsPerHour[meetingHour] = new List<Meeting>();
                        }
                        meetingsPerHour[meetingHour].Add(meeting);
                    }
                }
            }
        }
        int[] hourcount = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        int hours = 0;
        for (int hour = 8; hour <= 23; hour++)
        {
            sb.Append("<tr>");
            sb.Append("<td style='width:6%;border:0px; pointer-events: none;position: relative;'><span class='hourtd'>" + hour.ToString("D2") + ":00</span></td>");
            sb.Append("<td style='width:85%;position: relative;'>");

            if (meetingsPerHour.ContainsKey(hour))
            {
                var meetings = meetingsPerHour[hour];
                int meetingCount = meetings.Count();


                int datCount = meetingCount;
                datCount = getsamedatmettingcount(new DateTime(currentDate.Year, currentDate.Month, currentDate.Day), ddl2, ddl3);
                datCount = 8;
                int eventWidth = (int)(100 / datCount);


                for (int i = 0; i < meetingCount; i++)
                {
                    var meeting = meetings[i];

                    //string color = (meeting.State == "0") ? "#9089E9" : "#524BB2";

                    int startMinutes = (int)(meeting.Start.Minute / 60.0 * 50);
                    int eventHeight = (int)((meeting.End - meeting.Start).TotalMinutes / 60.0 * 50);

                    int leftPosition = i * eventWidth;
                    leftPosition = hours * eventWidth;//20250318 改
                   
                    sb.Append("<div role='button' id='bt_" + meeting.Id + "' class='datebt' style='position: absolute; top:" + startMinutes + "%; left:" + leftPosition + 
                        "%; background-color: " + meeting.color + "; width: " + (eventWidth - 1) + "%; height: " + eventHeight +
                        "px; z-index: 1;margin-right:5px; writing-mode:vertical-lr; text-orientation:upright; display:flex; align-items:center;' title='" + Regex.Replace(meeting.Name, "[()\\-_:]", " | ") + "'>");
                    sb.Append(Regex.Replace(meeting.Name, "[()\\-_:]", " | "));
                    sb.Append("</div>");

                    hours++;
                }
            }

            sb.Append("</td>");
            sb.Append("</tr>");
        }

        sb.Append("</tbody>");
        sb.Append("</table>");

        // Set the generated HTML as the inner HTML of the calendar container
        calendar_day.InnerHtml = sb.ToString();
    }
    public class Meeting
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string State { get; set; }
        public string color { get; set; }
    }

    public string Currentday()
    {
        int month = 0;
        int year = 0;


        if (Request.QueryString["month"] != null)
        {
            month = Request.QueryString["month"] != null ? Convert.ToInt32(Request.QueryString["month"]) : DateTime.Now.Month;
            year = Request.QueryString["year"] != null ? Convert.ToInt32(Request.QueryString["year"]) : DateTime.Now.Year;
        }
        else
        {
            if (Session["y"] != null)
            {
                year = Convert.ToInt32(Session["y"].ToString());
            }
            if (Session["m"] != null)
            {
                month = Convert.ToInt32(Session["m"].ToString());
            }
        }
        int day = Request.QueryString["day"] != null ? Convert.ToInt32(Request.QueryString["day"]) : DateTime.Now.Day;
        DateTime date = new DateTime(year, month, day);
        return date.ToString("yyyy年MM月dd日");
    }
    public string Previousday()
    {
        string mod = Request.QueryString["mod"];
        int month = 0;
        int year = 0;


        if (Request.QueryString["month"] != null)
        {
            month = Request.QueryString["month"] != null ? Convert.ToInt32(Request.QueryString["month"]) : DateTime.Now.Month;
            year = Request.QueryString["year"] != null ? Convert.ToInt32(Request.QueryString["year"]) : DateTime.Now.Year;
        }
        else
        {
            if (Session["y"] != null)
            {
                year = Convert.ToInt32(Session["y"].ToString());
            }
            if (Session["m"] != null)
            {
                month = Convert.ToInt32(Session["m"].ToString());
            }
        }
        int day = Request.QueryString["day"] != null ? Convert.ToInt32(Request.QueryString["day"]) : DateTime.Now.Day;

        // 先减去一天
        DateTime currentDate = new DateTime(year, month, day).AddDays(-1);

        // 获取上一天的年份、月份和日期
        year = currentDate.Year;
        month = currentDate.Month;
        day = currentDate.Day;

        return "Meetingroom_calendar.aspx?mod=d&day=" + day + "&month=" + month + "&year=" + year;
    }

    public string Nextday()
    {
        string mod = Request.QueryString["mod"];
        int month = 0;
        int year = 0;


        if (Request.QueryString["month"] != null)
        {
            month = Request.QueryString["month"] != null ? Convert.ToInt32(Request.QueryString["month"]) : DateTime.Now.Month;
            year = Request.QueryString["year"] != null ? Convert.ToInt32(Request.QueryString["year"]) : DateTime.Now.Year;
        }
        else
        {
            if (Session["y"] != null)
            {
                year = Convert.ToInt32(Session["y"].ToString());
            }
            if (Session["m"] != null)
            {
                month = Convert.ToInt32(Session["m"].ToString());
            }
        }
        int day = Request.QueryString["day"] != null ? Convert.ToInt32(Request.QueryString["day"]) : DateTime.Now.Day;

        // 先加上一天
        DateTime currentDate = new DateTime(year, month, day).AddDays(1);

        // 获取下一天的年份、月份和日期
        year = currentDate.Year;
        month = currentDate.Month;
        day = currentDate.Day;

        return "Meetingroom_calendar.aspx?mod=d&day=" + day + "&month=" + month + "&year=" + year;
    }
    protected void today_Click(object sender, EventArgs e)
    {
        int month = DateTime.Now.Month;
        int year = DateTime.Now.Year;
        int day = DateTime.Now.Day;
        string url = "Meetingroom_calendar.aspx?mod=d&day=" + day + "&month=" + month + "&year=" + year;
        Response.Redirect(url);
    }
    protected void submitbt_Click(object sender, EventArgs e)
    {

        ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal1", "showModal1();", true);


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
                    DateTime dayEndTime = new DateTime(currentEndTime.Year, currentEndTime.Month, currentEndTime.Day, 23, 30, 0);


                    if (i == 0)
                    {
                        dayStartTime = new DateTime(currentStartTime.Year, currentStartTime.Month, currentStartTime.Day, startDateTime.Hour, startDateTime.Minute, 0);
                        dayEndTime = new DateTime(currentStartTime.Year, currentStartTime.Month, currentStartTime.Day, 23, 30, 0);
                    }
                    if (i == daysDifference)
                    {
                        dayStartTime = new DateTime(currentEndTime.Year, currentEndTime.Month, currentEndTime.Day, 8, 30, 0);

                        dayEndTime = new DateTime(currentEndTime.Year, currentEndTime.Month, currentEndTime.Day, endDateTime.Hour, endDateTime.Minute, 0);
                    }

                    if (i > 0 && i < daysDifference)
                    {
                        dayStartTime = new DateTime(currentStartTime.Year, currentStartTime.Month, currentStartTime.Day, 8, 30, 0);
                        dayEndTime = new DateTime(currentStartTime.Year, currentStartTime.Month, currentStartTime.Day, 23, 30, 0);
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
        string mod = Request.QueryString["mod"];
        string wk = Request.QueryString["wk"];
        string day = Request.QueryString["day"];
        int month = 0;
        int year = 0;

        /*會議室如果 1.mode是w時 2.參數無值 => 預設登入時的時間*/
        if (mod == "w")
        {
            if (Request.QueryString["wk"] == null && Request.QueryString["month"] == null && Request.QueryString["year"] == null)
            {
                DateTime today = DateTime.Today;
                DateTime firstDayOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                DateTime firstSundayOfMonth = firstDayOfMonth;
                while (firstSundayOfMonth.DayOfWeek != DayOfWeek.Sunday)
                {
                    firstSundayOfMonth = firstSundayOfMonth.AddDays(1);
                }
                int daysSinceFirstSunday = (today - firstSundayOfMonth).Days;
                int currentWeek = daysSinceFirstSunday / 7 + 1;
                wk = Request.QueryString["wk"] != null ? currentWeek.ToString() : "1";
                month = Request.QueryString["month"] != null ? Convert.ToInt32(DateTime.Now.Month) : DateTime.Now.Month;
                year = Request.QueryString["year"] != null ? Convert.ToInt32(DateTime.Now.Year) : DateTime.Now.Year;
            }
        }
        /*會議室如果 1.mode是w時 2.參數無值 => 預設登入時的時間*/

        if (Request.QueryString["month"] != null)
        {
            month = Request.QueryString["month"] != null ? Convert.ToInt32(Request.QueryString["month"]) : DateTime.Now.Month;
            year = Request.QueryString["year"] != null ? Convert.ToInt32(Request.QueryString["year"]) : DateTime.Now.Year;
        }
        else
        {
            if (Session["y"] != null)
            {
                year = Convert.ToInt32(Session["y"].ToString());
            }
            if (Session["m"] != null)
            {
                month = Convert.ToInt32(Session["m"].ToString());
            }
        }
        if (mod == "m")
        {
            Response.Redirect("Meetingroom_calendar.aspx?mod=m&month=" + month + "&year=" + year);
        }
        else if (mod == "w")
        {
            Response.Redirect("Meetingroom_calendar.aspx?mod=w&wk=" + wk + "&month=" + month + "&year=" + year);

        }
        else if (mod == "d")
        {
            Response.Redirect("Meetingroom_calendar.aspx?mod=d&day=" + day + "&month=" + month + "&year=" + year);
        }
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
        string mod = Request.QueryString["mod"];
        string wk = Request.QueryString["wk"];
        string day = Request.QueryString["day"];
        int month = 0;
        int year = 0;


        if (Request.QueryString["month"] != null)
        {
            month = Request.QueryString["month"] != null ? Convert.ToInt32(Request.QueryString["month"]) : DateTime.Now.Month;
            year = Request.QueryString["year"] != null ? Convert.ToInt32(Request.QueryString["year"]) : DateTime.Now.Year;
        }
        else
        {
            if (Session["y"] != null)
            {
                year = Convert.ToInt32(Session["y"].ToString());
            }
            if (Session["m"] != null)
            {
                month = Convert.ToInt32(Session["m"].ToString());
            }
        }
        if (mod == "m")
        {
            Response.Redirect("Meetingroom_calendar.aspx?mod=m&month=" + month + "&year=" + year);
        }
        else if (mod == "w")
        {
            Response.Redirect("Meetingroom_calendar.aspx?mod=w&wk=" + wk + "&month=" + month + "&year=" + year);

        }
        else if (mod == "d")
        {
            Response.Redirect("Meetingroom_calendar.aspx?mod=d&day=" + day + "&month=" + month + "&year=" + year);
        }



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
            string sql = @"SELECT [appr_meet_id],[meetclass],[useclass],[creat_user] FROM [meeting_apprly] where id=@id ";

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
            ScriptManager.RegisterStartupScript(this, this.GetType(), "showmoda2", ";showModal2();", true);
        }
        else
        {
            md2del_sing.Visible = true;
            md2del_let.Visible = false;
            md2del_other.Visible = false;
            deltxt.Text = "確定要刪除嗎？";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "showmoda2", ";showModal2();", true);
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
        int month = 0;
        int year = 0;


        if (Request.QueryString["month"] != null)
        {
            month = Request.QueryString["month"] != null ? Convert.ToInt32(Request.QueryString["month"]) : DateTime.Now.Month;
            year = Request.QueryString["year"] != null ? Convert.ToInt32(Request.QueryString["year"]) : DateTime.Now.Year;
        }
        else
        {
            if (Session["y"] != null)
            {
                year = Convert.ToInt32(Session["y"].ToString());
            }
            if (Session["m"] != null)
            {
                month = Convert.ToInt32(Session["m"].ToString());
            }
        }
        Response.Redirect("Meetingroom_calendar.aspx?mod=m&month=" + month + "&year=" + year);
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
        int month = 0;
        int year = 0;


        if (Request.QueryString["month"] != null)
        {
            month = Request.QueryString["month"] != null ? Convert.ToInt32(Request.QueryString["month"]) : DateTime.Now.Month;
            year = Request.QueryString["year"] != null ? Convert.ToInt32(Request.QueryString["year"]) : DateTime.Now.Year;
        }
        else
        {
            if (Session["y"] != null)
            {
                year = Convert.ToInt32(Session["y"].ToString());
            }
            if (Session["m"] != null)
            {
                month = Convert.ToInt32(Session["m"].ToString());
            }
        }
        Response.Redirect("Meetingroom_calendar.aspx?mod=m&month=" + month + "&year=" + year);
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
        int month = 0;
        int year = 0;


        if (Request.QueryString["month"] != null)
        {
            month = Request.QueryString["month"] != null ? Convert.ToInt32(Request.QueryString["month"]) : DateTime.Now.Month;
            year = Request.QueryString["year"] != null ? Convert.ToInt32(Request.QueryString["year"]) : DateTime.Now.Year;
        }
        else
        {
            if (Session["y"] != null)
            {
                year = Convert.ToInt32(Session["y"].ToString());
            }
            if (Session["m"] != null)
            {
                month = Convert.ToInt32(Session["m"].ToString());
            }
        }
        Response.Redirect("Meetingroom_calendar.aspx?mod=m&month=" + month + "&year=" + year);
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
        ScriptManager.RegisterStartupScript(this, this.GetType(), "showmodal", "showModal1();", true);
    }
    protected void ddl1_SelectedIndexChanged(object sender, EventArgs e)
    {
        int month = DateTime.Now.Month;
        int year = DateTime.Now.Year;


        if (Request.QueryString["month"] != null)
        {
            month = Request.QueryString["month"] != null ? Convert.ToInt32(Request.QueryString["month"]) : DateTime.Now.Month;
            year = Request.QueryString["year"] != null ? Convert.ToInt32(Request.QueryString["year"]) : DateTime.Now.Year;
        }
        else
        {
            if (Session["y"] != null)
            {
                year = Convert.ToInt32(Session["y"].ToString());
            }
            if (Session["m"] != null)
            {
                month = Convert.ToInt32(Session["m"].ToString());
            }
        }
        Session["ddl1"] = ddl1.SelectedValue;
        if (ddl1.SelectedValue == "0")
        {
            ViewState["mod"] = "m";
            string url = "Meetingroom_calendar.aspx?mod=m&month="+ month + "&year=" + year;
            Response.Redirect(url);
        }
        else if (ddl1.SelectedValue == "1")
        {
            ViewState["mod"] = "w";
            DateTime today = DateTime.Today;
            DateTime firstDayOfMonth = new DateTime(year, month, 1);
            DateTime firstSundayOfMonth = firstDayOfMonth;
            while (firstSundayOfMonth.DayOfWeek != DayOfWeek.Sunday)
            {
                firstSundayOfMonth = firstSundayOfMonth.AddDays(1);
            }
            int daysSinceFirstSunday = (today - firstSundayOfMonth).Days;
            int currentWeek = daysSinceFirstSunday / 7 + 1;
            string url = "Meetingroom_calendar.aspx?mod=w&wk=" + currentWeek + "&month=" + month + "&year=" + year;
            Response.Redirect(url);
        }
        else if (ddl1.SelectedValue == "2")
        {
            ViewState["mod"] = "d";
            int day = Request.QueryString["day"] != null ? Convert.ToInt32(Request.QueryString["day"]) : DateTime.Now.Day;
            string url = "Meetingroom_calendar.aspx?mod=d&day=" + day + "&month=" + month + "&year=" + year;
            Response.Redirect(url);
        }
     


    }
    protected void ddl2_SelectedIndexChanged(object sender, EventArgs e)
    {
        string mod = Request.QueryString["mod"];
        int week = Convert.ToInt32(Request.QueryString["wk"]);
        int month = 0;
        int year = 0;


        if (Request.QueryString["month"] != null)
        {
            month = Request.QueryString["month"] != null ? Convert.ToInt32(Request.QueryString["month"]) : DateTime.Now.Month;
            year = Request.QueryString["year"] != null ? Convert.ToInt32(Request.QueryString["year"]) : DateTime.Now.Year;
        }
        else
        {
            if (Session["y"] != null && Session["y"].ToString() != "0")
            {
                year = Convert.ToInt32(Session["y"].ToString());
            }
            else
                year = DateTime.Now.Year;

            if (Session["m"] != null && Session["m"].ToString() != "0")
            {
                month = Convert.ToInt32(Session["m"].ToString());
            }
            else
                month = DateTime.Now.Month;

            if (Session["week"] != null && Session["week"].ToString() != "0")
            {
                week = Convert.ToInt32(Session["week"].ToString());
            }
            else
                week = 1;

        }
        int day = Request.QueryString["day"] != null ? Convert.ToInt32(Request.QueryString["day"]) : DateTime.Now.Day;
        Session["m"] = month;
        Session["y"] = year;
        Session["week"] = week;
        Session["day"] = day;
        Session["ddl2"] = ddl2.SelectedValue;
        ddl2.SelectedValue = Session["ddl2"].ToString();
        ddl3.SelectedValue = Session["ddl3"] == null ? "0" : Session["ddl3"].ToString();
        if (mod == "m")
        {
            GenerateCalendar(month, year, ddl2.SelectedValue, ddl3.SelectedValue);
        }
        if (mod == "w")
        {
            GenerateWeeklyCalendar(month, year, week, ddl2.SelectedValue, ddl3.SelectedValue);
        }
        if (mod == "d")
        {
            GenerateDayCalendar(month, year, day, ddl2.SelectedValue, ddl3.SelectedValue);
        }
       
    }

    protected void ddl3_SelectedIndexChanged(object sender, EventArgs e)
    {
        string mod = Request.QueryString["mod"];
        int week = Convert.ToInt32(Request.QueryString["wk"]);
        int month = 0;
        int year = 0;


        if (Request.QueryString["month"] != null)
        {
            month = Request.QueryString["month"] != null ? Convert.ToInt32(Request.QueryString["month"]) : DateTime.Now.Month;
            year = Request.QueryString["year"] != null ? Convert.ToInt32(Request.QueryString["year"]) : DateTime.Now.Year;
        }
        else
        {
            if (Session["y"] != null && Session["y"].ToString() != "0")
            {
                year = Convert.ToInt32(Session["y"].ToString());
            }
            else
                year = DateTime.Now.Year;

            if (Session["m"] != null && Session["m"].ToString() != "0")
            {
                month = Convert.ToInt32(Session["m"].ToString());
            }
            else
                month = DateTime.Now.Month;

            if (Session["week"] != null && Session["week"].ToString() != "0")
            {
                week = Convert.ToInt32(Session["week"].ToString());
            }
            else
                week = 1;

        }

        

        int day = Request.QueryString["day"] != null ? Convert.ToInt32(Request.QueryString["day"]) : DateTime.Now.Day;

        Session["m"] = month;
        Session["y"] = year;
        Session["week"] = week;
        Session["day"] = day;
        Session["ddl3"] = ddl3.SelectedValue;

        if(!IsPostBack  && Session["ddl2"] != null && (Session["ddl2"].ToString()=="0" || Session["ddl2"].ToString() == "1" || Session["ddl2"].ToString() == "2") )
          ddl2.SelectedValue = Session["ddl2"] == null ? "" : Session["ddl2"].ToString();

        if (!IsPostBack)
          ddl3.SelectedValue = Session["ddl3"].ToString();


      //  ScriptManager.RegisterStartupScript(this, this.GetType(), "alert_other", "alert('" + week + "');", true);
        if (mod == "m")
        {
            GenerateCalendar(month, year, ddl2.SelectedValue, ddl3.SelectedValue);
        }
        if (mod == "w")
        {
            GenerateWeeklyCalendar(month, year, week, ddl2.SelectedValue, ddl3.SelectedValue);
        }
        if (mod == "d")
        {
            GenerateDayCalendar(month, year, day, ddl2.SelectedValue, ddl3.SelectedValue);
        }


        
    }



    [WebMethod]
    public static string GetMeetingNames()
    {
        string favoriteNames = string.Empty;
        string userId = HttpContext.Current.Session["user_id"] as string;
        if (string.IsNullOrEmpty(userId))
        {
            return favoriteNames;
        }
        string eip = WebConfigurationManager.ConnectionStrings["eip"].ConnectionString;

        // 資料庫查詢語句
        string query = "SELECT [favorite_name] FROM [eip_user] WHERE [user_id] = @userId";

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
        //for (int hour = 8; hour < 18; hour++)
        //{
        //    // 生成整點時間
        //    string time1 = FormatTime(hour, 0);
        //    ddl.Items.Add(new ListItem(time1, time1));

        //    // 生成半小時
        //    string time2 = FormatTime(hour, 30);
        //    ddl.Items.Add(new ListItem(time2, time2));
        //}
        for (int hour = 8; hour < 24; hour++)
        {
            // 加入整點
            string time1 = FormatTime(hour, 0);
            ddl.Items.Add(new ListItem(time1, time1));

            // 加入半點（但11:30不要）
            if (hour != 23)
            {
                string time2 = FormatTime(hour, 30);
                ddl.Items.Add(new ListItem(time2, time2));
            }
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
        //ScriptManager.RegisterStartupScript(this, this.GetType(), "modal1", "alert('"+ ViewState["show"] .ToString()+ "');", true);
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
            string sql1 = @"insert into meeting_apply_"+ type + " ("+ type + "_name) values (@content)";
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
        //ScriptManager.RegisterStartupScript(this, this.GetType(), "apply_user", "alert('" + index + "');", true);
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
    protected int getsamedatmettingcount(DateTime dt,string ddl2, string ddl3)
    {
        int res = 0;

        using (SqlConnection cn = new SqlConnection(eip))
        {
            string sql1 = @"select count([id]) as c from [meeting_apprly] where convert(char(10),[use_start],121) =@use_start";

            if (ddl2 == "1")
            {
                sql1 += " and appr_user=@appr_user";
            }
            else if (ddl2 == "2")
            {
                sql1 += " and appr_group=@appr_group";
            }
            if (ddl3 != "0")
            {
                sql1 += " and appr_meet_id=@appr_meet_id";
            }

            SqlCommand cmd = new SqlCommand(sql1, cn);
            cmd.Parameters.AddWithValue("@use_start", dt.ToString("yyyy-MM-dd"));

            if (ddl2 == "1")
            {
                cmd.Parameters.AddWithValue("@appr_user", Session["user_name"].ToString());
            }
            else if (ddl2 == "2")
            {
                cmd.Parameters.AddWithValue("@appr_group", Session["group_name"].ToString());
            }
            if (ddl3 != "0")
            {
                cmd.Parameters.AddWithValue("@appr_meet_id", ddl3);
            }


            cn.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                res = int.Parse(dr["c"].ToString());
            }
            cn.Close();
        }


        return res;
    }


    static int GetWeekOfMonth(DateTime date)
    {
        DateTime firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
        int firstDayWeek = (int)firstDayOfMonth.DayOfWeek;
        if (firstDayWeek == 0) firstDayWeek = 7; // 將星期天視為 7
       
        // 計算當前日期是當月的第幾天
        int dayOfMonth = date.Day;

        // 計算是第幾周
        return (dayOfMonth + firstDayWeek - 1) / 7 ;
    }

    protected void Calendar_SelectionChanged(object sender, EventArgs e)
    {
        string mod = Request.QueryString["mod"];
        int week = Convert.ToInt32(Request.QueryString["wk"]);
        int month = 0;
        int year = 0;


        if (Request.QueryString["month"] != null)
        {
            month = Request.QueryString["month"] != null ? Convert.ToInt32(Request.QueryString["month"]) : DateTime.Now.Month;
            year = Request.QueryString["year"] != null ? Convert.ToInt32(Request.QueryString["year"]) : DateTime.Now.Year;
        }
        else
        {
            if (Session["y"] != null && Session["y"].ToString() != "0")
            {
                year = Convert.ToInt32(Session["y"].ToString());
            }
            else
                year = DateTime.Now.Year;

            if (Session["m"] != null && Session["m"].ToString() != "0")
            {
                month = Convert.ToInt32(Session["m"].ToString());
            }
            else
                month = DateTime.Now.Month;

            if (Session["week"] != null && Session["week"].ToString() != "0")
            {
                week = Convert.ToInt32(Session["week"].ToString());
            }
            else
                week = 1;

        }
        int day = Convert.ToInt32(Calendar.SelectedDate.ToString("yyyy/MM/dd").Substring(8));
        //Response.Write(Calendar.SelectedDate.ToString("yyyy/MM/dd").Substring(8));
        Session["m"] = month;
        Session["y"] = year;
        Session["week"] = week;
        Session["day"] = day;
        Session["ddl2"] = ddl2.SelectedValue;
        ddl2.SelectedValue = Session["ddl2"].ToString();
        ddl3.SelectedValue = Session["ddl3"] == null ? "0" : Session["ddl3"].ToString();
        GenerateDayCalendar(month, year, day, ddl2.SelectedValue, ddl3.SelectedValue);
    }
}
