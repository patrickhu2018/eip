using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Script.Serialization;
using System.Web.Services;

/// <summary>
/// WebService 的摘要描述
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// 若要允許使用 ASP.NET AJAX 從指令碼呼叫此 Web 服務，請取消註解下列一行。
[System.Web.Script.Services.ScriptService]
public class WebService : System.Web.Services.WebService
{

    [WebMethod]

    public string getdate(string id)
    {
        string eip = WebConfigurationManager.ConnectionStrings["eip"].ConnectionString;
        string mid = null;
        string meeting_name = null;
        string use_start = null;
        string use_end = null;
        string appr_group = null;
        string appr_user = null;
        string appr_meet_id = null;
        string host = null;
        string number = null;
        string equipment = null;
        string count = null;
        string meetclass = null;
        string useclass = null;
        string note = null;
        string ck = null;
        string lunch_box = null;
        string Takeaway = null;
        string disposable = null;
        string other_reason = null;
        string other=null;

        using (SqlConnection cn = new SqlConnection(eip))
        {
            cn.Open();
            string sql = @"SELECT  t1.[id],[meeting_name],[use_start],[use_end],[appr_group],[appr_user],[appr_meet_id],[host]
                                ,t1.[number],t2.number as count,[meetclass],[useclass],[note],[online],[lunch_box],[Takeaway]
                                ,[disposable],[other_reason],t1.[other] FROM [meeting_apprly] as t1 
								left join meeting_equipment as t2 on  t1.appr_meet_id=t2.id where t1.id=@id";
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.AddWithValue("@id", id);
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        mid = dr["id"].ToString();
                        meeting_name = dr["meeting_name"].ToString();
                        use_start = dr["use_start"].ToString();
                        use_end = dr["use_end"].ToString();
                        appr_group = dr["appr_group"].ToString();
                        appr_user = dr["appr_user"].ToString();
                        appr_meet_id = dr["appr_meet_id"].ToString();
                        host = dr["host"].ToString();
                        number = dr["number"].ToString();
                        equipment = show_mtroom_Selected(dr["appr_meet_id"].ToString());
                        count = dr["count"].ToString();
                        meetclass = dr["meetclass"].ToString();
                        useclass = dr["useclass"].ToString();
                        note = dr["note"].ToString();
                        ck = dr["online"].ToString();
                        other = dr["other"] != DBNull.Value ? dr["other"].ToString() : null;
                        lunch_box = dr["lunch_box"] != DBNull.Value ? dr["lunch_box"].ToString() : null;
                        Takeaway = dr["Takeaway"] != DBNull.Value ? dr["Takeaway"].ToString() : null;
                        disposable = dr["disposable"] != DBNull.Value ? dr["disposable"].ToString() : null;
                        other_reason = dr["other_reason"] != DBNull.Value ? dr["other_reason"].ToString() : null;
                    }
                }
            }
        }
        var response = new
        {
            mid = mid,
            meeting_name = meeting_name,
            use_start = use_start,
            use_end = use_end,
            appr_group = appr_group,
            appr_user = appr_user,
            appr_meet_id = appr_meet_id,
            host = host,
            number = number,
            equipment = equipment,
            count = count,
            meetclass = meetclass,
            useclass = useclass,
            note = note,
            ck = ck,
            other = other,
            lunch_box = lunch_box,
            Takeaway = Takeaway,
            disposable = disposable,
            other_reason = other_reason
        };
        //return JsonConvert.SerializeObject(response);
        return new JavaScriptSerializer().Serialize(response);
    }

    protected string show_mtroom_Selected(string mtroom_value)
    {
        string eip = WebConfigurationManager.ConnectionStrings["eip"].ConnectionString;
        string result = "";
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
                    result = "(" + string.Join(", ", equipmentNames) + ")";
                }

            }
        }

        return result;
    }

    [WebMethod]
    public string savefav(string id, string selected_items)
    {
        string eip = WebConfigurationManager.ConnectionStrings["eip"].ConnectionString;

        using (SqlConnection cn = new SqlConnection(eip))
        {
            cn.Open();
            string sql = @"UPDATE [eip_user] SET favorite_meet = @favorite_meet WHERE user_id = @id";
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.AddWithValue("@favorite_meet", selected_items);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
        }
        return "{\"success\": true, \"message\": \"保存成功\"}";
    }

}
