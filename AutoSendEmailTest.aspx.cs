using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class AutoSendEmailTest : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        {
            sendEmail("louis@fundot.tw;曹幼霖 <aasd321452@ctsp.gov.tw>", "TEST","for ein test");
        }
    }

    protected void sendEmail(string email, string title, string content)
    {
        //Email 收件人
        //string str_ConsigneeList = GridView1.Rows[i].Cells[5].Text + ";";
        //分號後面可自填寄信備份到哪
        string Email = email;

        //系統建立日期
        string str_CreateDate = System.DateTime.Now.ToString("MM月dd日");
        //標題
        string str_title = title;
        string url = Request.Url.ToString().Split('/')[0] + "//" + Request.Url.ToString().Split('/')[2] + "/login.aspx?c=m";
        //內容
        string str_Contnet = content;
        #region 發通知連結郵件
        OtherAPI2 API = new OtherAPI2();
        //-----------------------------------------------------寄發Email 開始------------------------------------------------
        int issend = 0;
        string result = "";
        result = API.SendEmail(title, str_Contnet, Email);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "xx", "console.log('"+ result + "');", true);
        //-----------------------------------------------------寄發Email 結束------------------------------------------------
        #endregion

    }
}