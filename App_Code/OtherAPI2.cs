using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Net.Mail;
using System.Net;

/// <summary>
/// OtherAPI2 的摘要描述
/// </summary>
public class OtherAPI2
{
    StringBuilder str_object = new StringBuilder("");
	public OtherAPI2()
	{}
    
    public string SendEmail(string _Title, string _Content, string _Consignee)
    {
        string result = "";
        MailMessage xmail = new MailMessage();  //信件本體宣告

        //寄件者
        xmail.From = new MailAddress("pms@apmail.ctsp.gov.tw", "");

        //收件人
        foreach (var address in _Consignee.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
        {
            xmail.To.Add(address);
        }
        //優先等級
        xmail.Priority = MailPriority.Normal;

        //標題
        xmail.Subject = _Title;

        //內容
        StringBuilder SB_Content = new StringBuilder("");

        //Content 排版
        /*SB_Content.Append("<table width=\"800px\">");
        SB_Content.Append("<tr>");
        SB_Content.Append("<td align=\"center\">");
        SB_Content.Append("<div style=\"width:750px; text-align:left; font-size: 100%; font-family:Times New Roman,新細明體; line-height: 22px; letter-spacing: 1px;\">");
        SB_Content.Append(_Content.Replace("\r\n", "<br />").Replace("中部科學工業園區管理局環安組敬上", "<div style=\"text-align:right;\">中部科學工業園區管理局環安組敬上</div>").Replace("您好：", "您好：<br /><br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;").Replace("次日起","<span style=\"font-size: 100%; font-weight:bold; color:Red;\">次日起</span>"));
        SB_Content.Append("</div>");
        SB_Content.Append("</td>");
        SB_Content.Append("</tr>");
        SB_Content.Append("</table>");*/

        xmail.Body = _Content.ToString();

        // 設定Email 內容為HTML格式
        // 以 HTML 編碼可以預防使用者在文字框內輸入惡意的片段，但相對的
        // 你也無法用 html tag 預先做版面設計
        xmail.IsBodyHtml = true;

        //設定編碼為utf-8
        xmail.BodyEncoding = System.Text.Encoding.UTF8;
        xmail.HeadersEncoding = System.Text.Encoding.UTF8;

        //xmail.Attachment.NameEncoding = System.Text.Encoding.UTF8;

        // 設定SMTP伺服器
        ServicePointManager.SecurityProtocol = (SecurityProtocolType)768 | (SecurityProtocolType)3072 ;//強制使用TLS1.2，依情況設定
        SmtpClient smtpServer = new SmtpClient();
        // 設定帳號與密碼 (請替換為實際的帳號與密碼)
        smtpServer.Credentials = new System.Net.NetworkCredential("ebulletin@apmail.ctsp.gov.tw", "Syscom01");
        // youracc 為你的 mail 帳號
        // yourpass 為你 mail 帳號的密碼
        smtpServer.Port = 25;  
        smtpServer.Host = "apmail.ctsp.gov.tw";
        smtpServer.EnableSsl = false;  
        // 要不要啟用 SSL 要看你的 mail server，像 gmail 就必須啟用，而我現在用的 mail server 就不能用 SSL
        try
        {
            smtpServer.Send(xmail);
            result = "send";
        }
        catch (Exception ex)
        {
            return "error: " + ex.Message.Replace("'", "\\'").Replace(Environment.NewLine, " ");
        }
   

        return result;
    }
     
}