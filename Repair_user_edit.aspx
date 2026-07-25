<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage2.master" AutoEventWireup="true" CodeFile="Repair_user_edit.aspx.cs" Inherits="Repair_user_edit" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <link href="css/jquery-ui.min.css" rel="stylesheet" type="text/css" />
    <script src="js/jquery-ui.min.js" type="text/javascript"></script>
    <link href="css/jquery-ui-timepicker-addon.css" rel="stylesheet" type="text/css" />
    <script src="js/jquery-ui-sliderAccess.js" type="text/javascript"></script>
    <script src="js/jquery-ui-timepicker-addon.js" type="text/javascript"></script>
    <style>
        .bt {
            width: 92px;
            height: 44px;
            background: #4CAF1E 0% 0% no-repeat padding-box;
            border: 0px;
            border-radius: 5px;
            opacity: 1;
            text-align: center;
            color: #FFFFFF;
            margin-right: 10px;
            padding: 10px 30px;
        }
        .tab-titel {
            font-size: 1rem;
            font-weight: bold;
            background: #BEC9E1;
            padding: 10px;
            border-radius: 8px 8px 0px 0px;
        }
        .tab-titel > input[type=radio] {
            margin-right: 5px;
        }
        .tab-body {
            background-color: #FFFFFF;
            border-radius: 0px 0px 8px 8px;
            border: 1px solid #e0e0e0;
            padding: 10px;
            min-height: 190px;
            /*align-content: center;*/
        }
        .tab-body label{
            margin-bottom:0;
            margin-left:0.25rem;
        }
        .tab-body2 {
            background-color: #FFFFFF;
            border-radius: 0px 0px 8px 8px;
            border: 1px solid #e0e0e0;
            padding: 10px;
            /*min-height: 100px;*/
            /*align-content: center;*/
        }
        .tab-body2 label{
            margin-bottom:0;
            margin-left:0.25rem;
        }
        .cb_louis label {
            margin-bottom: 0 !important;
        }
        .mb-1r {
            margin-bottom:1rem;
        }
        .mb-05r {
            margin-bottom:0.5rem;
        }
        .mb-025r {
            margin-bottom:0.25rem;
        }
        .tb-bg {
            background: #E9FAFF;
            height:2rem;
        }
        .tb-bg.active {
            background: #FCF8BF;
            height:2rem;
        }
        .table1 tr td {
          border:0.25rem solid white;
          padding:0.5rem;
        }
        .table2 td {
            vertical-align:baseline;
        }
        .DDLwidth {
            width:10rem;
        }
        td > label {
            font-weight:bold;
        }
        .changeDDL {
            width:12rem;
        }
        @media (max-width: 1100px) {
            .changeDDL {
                width:9rem;
            }
        }
    </style>
    <script>
        var url = new URLSearchParams(window.location.search);
        var m = url.get('m');
        if (m !== "0") {
            document.getElementById("people_07").classList.add("active");
            document.getElementById("people_08").classList.remove("active");
        }
    </script>
    <div class="write_Box">
        <div class="write_Title">
            <h5>
                <asp:Literal runat="server" ID="Literal1">帳號資訊</asp:Literal></h5>
        </div>
        <div class="write_textBox">
            <div class="dataBox row" style="width: 80%;">
                <div class="col-3 formItem title6">
                    <div class="ItemTitle"><span>組室</span></div>
                    <div class="ItemContent">
                        <asp:DropDownList ID="group" runat="server" CssClass="form-control" Style="width: auto" Enabled="false">
                            <asp:ListItem Value="0">全選</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
            </div>
            <div class="dataBox row" style="width: 180%;">
                <div class="col-2 formItem title6">
                    <div class="ItemTitle"><span>使用者姓名</span></div>
                    <div class="ItemContent">
                        <asp:TextBox ID="username" runat="server" CssClass="form-control" Style="width: 100%" Enabled="false"></asp:TextBox>
                    </div>
                </div>
                <div class="col-2 formItem title4">
                    <div class="ItemTitle"><span>員工編號</span></div>
                    <div class="ItemContent">
                        <asp:TextBox ID="ac" runat="server" CssClass="form-control" Style="width: 60%" Enabled="false"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="dataBox row" style="width: 180%;">
                <div class="col-2 formItem title6">
                    <div class="ItemTitle"><span>職位</span></div>
                    <div class="ItemContent">
                        <asp:TextBox ID="job" runat="server" CssClass="form-control" Style="width: 100%" Enabled="false"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="dataBox row" style="width: 180%;">
                <div class="col-2 formItem title6">
                    <div class="ItemTitle"><span>備註</span></div>
                    <div class="ItemContent">
                        <asp:TextBox ID="note" TextMode="MultiLine" Columns="180" runat="server" CssClass="form-control" Style="width: 300%" Enabled="false" placeholder="無特殊備註"></asp:TextBox>
                    </div>
                </div>
            </div>
        </div>
        <asp:Panel ID="pl" runat="server" Visible="false">
            <div class="write_textBox">
                <div class="dataBox row" style="width: 180%;">
                    <div class="col-6 formItem title7" style="align-items:start;">
                        <div class="ItemTitle"><span>權限與身分設定</span></div>
                        <div class="ItemContent">
                            <div class="mb-1r">
                                <table class="table2" style="font-size:0.875rem;color:#777777;">
                                    <tr>
                                        <td>註：</td>
                                        <td colspan="2">權限與身分設定說明</td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>1. </td>
                                        <td>權限與身份會共同影響使用者可使用的功能，<span style="color:#FF0000;">權限設定將影響可使用的功能</span>，身分設定將影響可查閱或管理的資料範圍。</td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>2. </td>
                                        <td>權限與身份僅能各選擇其中一項。</td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>3. </td>
                                        <td>本局六大行政庶務系統將可各自設定權限與身份，且互不影響。</td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td><span style="color:#FF0000;">4. </span></td>
                                        <td><span style="color:#FF0000;">新增帳號的權限將預設「一般使用者」，身份將預設「 一般-自己」。</span></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td><span style="color:#FF0000;">5. </span></td>
                                        <td><span style="color:#FF0000;">如有職務調整，請於「3.人員異動」設定此帳號業務轉移對象及權限終止時間。</span></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>6. </td>
                                        <td>如本帳號為其他帳號授權之代理人，修正權限或身分後，<span style="color:#FF0000;">代理將自動取消</span>。</td>
                                    </tr>
                                </table>
                            </div>
                            <div class="mb-05r">
                                <span>最後一次修改日期：</span>
                                <asp:Label runat="server" ID="LastUpdateTime" style="color:#005A8F;"></asp:Label>
                            </div>
                            <div class="mb-1r">
                                <div class="tab-titel cb_louis"style="display:flex;justify-content:space-between;align-items:end;">
                                    <div>一、權限設定</div>
                                </div>
                                <div class="tab-body">
                                    <asp:UpdatePanel runat="server">
                                        <ContentTemplate>
                                            <table class="table1" style="width:100%;">
                                                <tr style="font-size:0.875rem;text-align:center;">
                                                    <th style="width:10rem;">類別</th>
                                                    <th>帳號權限</th>
                                                </tr>
                                                <tr runat="server" ID="RightSettingTr1" class="tb-bg">
                                                    <td><asp:RadioButton runat="server" ID="RightSetting1" AutoPostBack="true" OnCheckedChanged="RightSetting_SelectedIndexChanged" GroupName="RightSetting" Text="系統管理者" /></td>
                                                    <td>【全部功能】</td>
                                                </tr>
                                                <tr runat="server" ID="RightSettingTr2" class="tb-bg">
                                                    <td><asp:RadioButton runat="server" ID="RightSetting2" AutoPostBack="true" OnCheckedChanged="RightSetting_SelectedIndexChanged" GroupName="RightSetting" Text="主計業務管理者" /></td>
                                                    <td>【我的申請】、【管理查詢】</td>
                                                </tr>
                                                <tr runat="server" ID="RightSettingTr3" class="tb-bg">
                                                    <td><asp:RadioButton runat="server" ID="RightSetting3" AutoPostBack="true" OnCheckedChanged="RightSetting_SelectedIndexChanged" GroupName="RightSetting" Text="一般業務管理者" /></td>
                                                    <td>
                                                        <div style="display:inline-flex;">【我的申請】、【管理查詢】、【物料管理】、【位置管理】</div>
                                                    </td>
                                                </tr>
                                                <tr runat="server" ID="RightSettingTr4" class="tb-bg">
                                                    <td><asp:RadioButton runat="server" ID="RightSetting4" AutoPostBack="true" OnCheckedChanged="RightSetting_SelectedIndexChanged" GroupName="RightSetting" Text="一般使用者" /></td>
                                                    <td>【我的申請】、【管理查詢】</td>
                                                </tr>
                                                <tr runat="server" ID="RightSettingTr5" class="tb-bg">
                                                    <td><asp:RadioButton runat="server" ID="RightSetting5" AutoPostBack="true" OnCheckedChanged="RightSetting_SelectedIndexChanged" GroupName="RightSetting" Text="主計登記桌" /></td>
                                                    <td>【我的申請】、【管理查詢】</td>
                                                </tr>
                                                <tr runat="server" ID="RightSettingTr6" class="tb-bg">
                                                    <td><asp:RadioButton runat="server" ID="RightSetting6" AutoPostBack="true" OnCheckedChanged="RightSetting_SelectedIndexChanged" GroupName="RightSetting" Text="一般登記桌" /></td>
                                                    <td>【我的申請】、【管理查詢】</td>
                                                </tr>
                                                <tr runat="server" ID="RightSettingTr7" class="tb-bg">
                                                    <td><asp:RadioButton runat="server" ID="RightSetting7" AutoPostBack="true" OnCheckedChanged="RightSetting_SelectedIndexChanged" GroupName="RightSetting" Text="審核使用者" /></td>
                                                    <td>【我的申請】、【管理查詢】</td>
                                                </tr>
                                                <tr runat="server" ID="RightSettingTr8" class="tb-bg">
                                                    <td><asp:RadioButton runat="server" ID="RightSetting8" AutoPostBack="true" OnCheckedChanged="RightSetting_SelectedIndexChanged" GroupName="RightSetting" Text="免審核使用者" /></td>
                                                    <td>【我的申請】、【管理查詢】</td>
                                                </tr>
                                                <tr runat="server" ID="RightSettingTr0" class="tb-bg">
                                                    <td><asp:RadioButton runat="server" ID="RightSetting0" AutoPostBack="true" OnCheckedChanged="RightSetting_SelectedIndexChanged" GroupName="RightSetting" Text="停止使用者" /></td>
                                                    <td>無</td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                            <div class="mb-1r">
                                <div class="tab-titel cb_louis"style="display:flex;justify-content:space-between;align-items:end;">
                                    <div>二、身份設定</div>
                                </div>
                                <div class="tab-body">
                                    <table class="table2" style="font-size:0.875rem;color:#777777;">
                                        <tr>
                                            <td style="vertical-align:baseline;">註：</td>
                                            <td>1. </td>
                                            <td>身分代表登入後的預設首頁及可看到的資料範圍，點擊更換身分選項，儲存後，該帳號登入後即可看到不同的預設首頁及資料內容。</td>
                                        </tr>
                                        <tr>
                                            <td></td>
                                            <td>2. </td>
                                            <td>「可看到的首頁」及「可看到的資料範圍」兩個選項可在任何一組帳號的編輯功能中設定，
                                                <sapn style="color:#FF0000;">選擇後將會同步切換所有相同身分的帳號設定</sapn>，請謹慎選擇各個身分所對應的選項。</td>
                                        </tr>
                                    </table>
                                    <hr style="margin-top:0.5rem;margin-bottom:0.5rem;" />
                                    <asp:UpdatePanel runat="server">
                                        <ContentTemplate>

                        
                                            <table class="table1" style="width:100%;table-layout:fixed;">
                                                <tr style="font-size:0.875rem;text-align:center;">
                                                    <th style="width:10rem;">類別</th>
                                                    <th>可看到的首頁</th>
                                                    <th>可看到的資料範圍</th>
                                                </tr>
                                                <tr runat="server" ID="JobSettingTr1" class="tb-bg">
                                                    <td><asp:RadioButton runat="server" ID="JobSetting1" AutoPostBack="true" OnCheckedChanged="JobSetting_SelectedIndexChanged" GroupName="JobSetting" Text="局長" /></td>
                                                    <td>
                                                        <div style="display:flex;justify-content:center;">
                                                            <asp:DropDownList runat="server" Enabled="false" ID="JobSettingDDL1_1" CssClass="form-control DDLwidth">
                                                                <asp:ListItem Value="1" Selected="True">我的申請</asp:ListItem>
                                                                <asp:ListItem Value="2">管理查詢</asp:ListItem>
                                                                <asp:ListItem Value="3">庫存管理</asp:ListItem>
                                                                <asp:ListItem Value="4">領用清單</asp:ListItem>
                                                                <asp:ListItem Value="5">位置管理</asp:ListItem>
                                                                <asp:ListItem Value="6">使用者設定</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div style="display:flex;justify-content:center;">
                                                            <asp:DropDownList runat="server" Enabled="false" ID="JobSettingDDL1_2" CssClass="form-control DDLwidth">
                                                                <asp:ListItem Value="1" Selected="True">全局</asp:ListItem>
                                                                <asp:ListItem Value="2">該單位</asp:ListItem>
                                                                <asp:ListItem Value="3">該單位該科室</asp:ListItem>
                                                                <asp:ListItem Value="4">自己</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr runat="server" ID="JobSettingTr2" class="tb-bg">
                                                    <td><asp:RadioButton runat="server" ID="JobSetting2" AutoPostBack="true" OnCheckedChanged="JobSetting_SelectedIndexChanged" GroupName="JobSetting" Text="副局長" /></td>
                                                    <td>
                                                        <div style="display:flex;justify-content:center;">
                                                            <asp:DropDownList runat="server" Enabled="false" ID="JobSettingDDL2_1" CssClass="form-control DDLwidth">
                                                                <asp:ListItem Value="1" Selected="True">我的申請</asp:ListItem>
                                                                <asp:ListItem Value="2">管理查詢</asp:ListItem>
                                                                <asp:ListItem Value="3">庫存管理</asp:ListItem>
                                                                <asp:ListItem Value="4">領用清單</asp:ListItem>
                                                                <asp:ListItem Value="5">位置管理</asp:ListItem>
                                                                <asp:ListItem Value="6">使用者設定</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div style="display:flex;justify-content:center;">
                                                            <asp:DropDownList runat="server" Enabled="false" ID="JobSettingDDL2_2" CssClass="form-control DDLwidth">
                                                                <asp:ListItem Value="1" Selected="True">全局</asp:ListItem>
                                                                <asp:ListItem Value="2">該單位</asp:ListItem>
                                                                <asp:ListItem Value="3">該單位該科室</asp:ListItem>
                                                                <asp:ListItem Value="4">自己</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr runat="server" ID="JobSettingTr3" class="tb-bg">
                                                    <td><asp:RadioButton runat="server" ID="JobSetting3" AutoPostBack="true" OnCheckedChanged="JobSetting_SelectedIndexChanged" GroupName="JobSetting" Text="主任秘書" /></td>
                                                    <td>
                                                        <div style="display:flex;justify-content:center;">
                                                            <asp:DropDownList runat="server" Enabled="false" ID="JobSettingDDL3_1" CssClass="form-control DDLwidth">
                                                                <asp:ListItem Value="1" Selected="True">我的申請</asp:ListItem>
                                                                <asp:ListItem Value="2">管理查詢</asp:ListItem>
                                                                <asp:ListItem Value="3">庫存管理</asp:ListItem>
                                                                <asp:ListItem Value="4">領用清單</asp:ListItem>
                                                                <asp:ListItem Value="5">位置管理</asp:ListItem>
                                                                <asp:ListItem Value="6">使用者設定</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div style="display:flex;justify-content:center;">
                                                            <asp:DropDownList runat="server" Enabled="false" ID="JobSettingDDL3_2" CssClass="form-control DDLwidth">
                                                                <asp:ListItem Value="1" Selected="True">全局</asp:ListItem>
                                                                <asp:ListItem Value="2">該單位</asp:ListItem>
                                                                <asp:ListItem Value="3">該單位該科室</asp:ListItem>
                                                                <asp:ListItem Value="4">自己</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr runat="server" ID="JobSettingTr4" class="tb-bg">
                                                    <td><asp:RadioButton runat="server" ID="JobSetting4" AutoPostBack="true" OnCheckedChanged="JobSetting_SelectedIndexChanged" GroupName="JobSetting" Text="核稿秘書" /></td>
                                                    <td>
                                                        <div style="display:flex;justify-content:center;">
                                                            <asp:DropDownList runat="server" Enabled="false" ID="JobSettingDDL4_1" CssClass="form-control DDLwidth">
                                                                <asp:ListItem Value="1" Selected="True">我的申請</asp:ListItem>
                                                                <asp:ListItem Value="2">管理查詢</asp:ListItem>
                                                                <asp:ListItem Value="3">庫存管理</asp:ListItem>
                                                                <asp:ListItem Value="4">領用清單</asp:ListItem>
                                                                <asp:ListItem Value="5">位置管理</asp:ListItem>
                                                                <asp:ListItem Value="6">使用者設定</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div style="display:flex;justify-content:center;">
                                                            <asp:DropDownList runat="server" Enabled="false" ID="JobSettingDDL4_2" CssClass="form-control DDLwidth">
                                                                <asp:ListItem Value="1" Selected="True">全局</asp:ListItem>
                                                                <asp:ListItem Value="2">該單位</asp:ListItem>
                                                                <asp:ListItem Value="3">該單位該科室</asp:ListItem>
                                                                <asp:ListItem Value="4">自己</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr runat="server" ID="JobSettingTr5" class="tb-bg">
                                                    <td><asp:RadioButton runat="server" ID="JobSetting5" AutoPostBack="true" OnCheckedChanged="JobSetting_SelectedIndexChanged" GroupName="JobSetting" Text="組長" /></td>
                                                    <td>
                                                        <div style="display:flex;justify-content:center;">
                                                            <asp:DropDownList runat="server" Enabled="false" ID="JobSettingDDL5_1" CssClass="form-control DDLwidth">
                                                                <asp:ListItem Value="1" Selected="True">我的申請</asp:ListItem>
                                                                <asp:ListItem Value="2">管理查詢</asp:ListItem>
                                                                <asp:ListItem Value="3">庫存管理</asp:ListItem>
                                                                <asp:ListItem Value="4">領用清單</asp:ListItem>
                                                                <asp:ListItem Value="5">位置管理</asp:ListItem>
                                                                <asp:ListItem Value="6">使用者設定</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div style="display:flex;justify-content:center;">
                                                            <asp:DropDownList runat="server" Enabled="false" ID="JobSettingDDL5_2" CssClass="form-control DDLwidth">
                                                                <asp:ListItem Value="1" Selected="True">全局</asp:ListItem>
                                                                <asp:ListItem Value="2">該單位</asp:ListItem>
                                                                <asp:ListItem Value="3">該單位該科室</asp:ListItem>
                                                                <asp:ListItem Value="4">自己</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr runat="server" ID="JobSettingTr6" class="tb-bg">
                                                    <td><asp:RadioButton runat="server" ID="JobSetting6" AutoPostBack="true" OnCheckedChanged="JobSetting_SelectedIndexChanged" GroupName="JobSetting" Text="副組長" /></td>
                                                    <td>
                                                        <div style="display:flex;justify-content:center;">
                                                            <asp:DropDownList runat="server" Enabled="false" ID="JobSettingDDL6_1" CssClass="form-control DDLwidth">
                                                                <asp:ListItem Value="1" Selected="True">我的申請</asp:ListItem>
                                                                <asp:ListItem Value="2">管理查詢</asp:ListItem>
                                                                <asp:ListItem Value="3">庫存管理</asp:ListItem>
                                                                <asp:ListItem Value="4">領用清單</asp:ListItem>
                                                                <asp:ListItem Value="5">位置管理</asp:ListItem>
                                                                <asp:ListItem Value="6">使用者設定</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div style="display:flex;justify-content:center;">
                                                            <asp:DropDownList runat="server" Enabled="false" ID="JobSettingDDL6_2" CssClass="form-control DDLwidth">
                                                                <asp:ListItem Value="1" Selected="True">全局</asp:ListItem>
                                                                <asp:ListItem Value="2">該單位</asp:ListItem>
                                                                <asp:ListItem Value="3">該單位該科室</asp:ListItem>
                                                                <asp:ListItem Value="4">自己</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr runat="server" ID="JobSettingTr7" class="tb-bg">
                                                    <td><asp:RadioButton runat="server" ID="JobSetting7" AutoPostBack="true" OnCheckedChanged="JobSetting_SelectedIndexChanged" GroupName="JobSetting" Text="科長" /></td>
                                                    <td>
                                                        <div style="display:flex;justify-content:center;">
                                                            <asp:DropDownList runat="server" Enabled="false" ID="JobSettingDDL7_1" CssClass="form-control DDLwidth">
                                                                <asp:ListItem Value="1" Selected="True">我的申請</asp:ListItem>
                                                                <asp:ListItem Value="2">管理查詢</asp:ListItem>
                                                                <asp:ListItem Value="3">庫存管理</asp:ListItem>
                                                                <asp:ListItem Value="4">領用清單</asp:ListItem>
                                                                <asp:ListItem Value="5">位置管理</asp:ListItem>
                                                                <asp:ListItem Value="6">使用者設定</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div style="display:flex;justify-content:center;">
                                                            <asp:DropDownList runat="server" Enabled="false" ID="JobSettingDDL7_2" CssClass="form-control DDLwidth">
                                                                <asp:ListItem Value="1" Selected="True">全局</asp:ListItem>
                                                                <asp:ListItem Value="2">該單位</asp:ListItem>
                                                                <asp:ListItem Value="3">該單位該科室</asp:ListItem>
                                                                <asp:ListItem Value="4">自己</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr runat="server" ID="JobSettingTr8" class="tb-bg">
                                                    <td><asp:RadioButton runat="server" ID="JobSetting8" AutoPostBack="true" OnCheckedChanged="JobSetting_SelectedIndexChanged" GroupName="JobSetting" Text="登記桌" /></td>
                                                    <td>
                                                        <div style="display:flex;justify-content:center;">
                                                            <asp:DropDownList runat="server" Enabled="false" ID="JobSettingDDL8_1" CssClass="form-control DDLwidth">
                                                                <asp:ListItem Value="1" Selected="True">我的申請</asp:ListItem>
                                                                <asp:ListItem Value="2">管理查詢</asp:ListItem>
                                                                <asp:ListItem Value="3">庫存管理</asp:ListItem>
                                                                <asp:ListItem Value="4">領用清單</asp:ListItem>
                                                                <asp:ListItem Value="5">位置管理</asp:ListItem>
                                                                <asp:ListItem Value="6">使用者設定</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div style="display:flex;justify-content:center;">
                                                            <asp:DropDownList runat="server" Enabled="false" ID="JobSettingDDL8_2" CssClass="form-control DDLwidth">
                                                                <asp:ListItem Value="1" Selected="True">全局</asp:ListItem>
                                                                <asp:ListItem Value="2">該單位</asp:ListItem>
                                                                <asp:ListItem Value="3">該單位該科室</asp:ListItem>
                                                                <asp:ListItem Value="4">自己</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr runat="server" ID="JobSettingTr9" class="tb-bg">
                                                    <td><asp:RadioButton runat="server" ID="JobSetting9" AutoPostBack="true" OnCheckedChanged="JobSetting_SelectedIndexChanged" GroupName="JobSetting" Text="一般-全局" /></td>
                                                    <td>
                                                        <div style="display:flex;justify-content:center;">
                                                            <asp:DropDownList runat="server" Enabled="false" ID="JobSettingDDL9_1" CssClass="form-control DDLwidth">
                                                                <asp:ListItem Value="1" Selected="True">我的申請</asp:ListItem>
                                                                <asp:ListItem Value="2">管理查詢</asp:ListItem>
                                                                <asp:ListItem Value="3">庫存管理</asp:ListItem>
                                                                <asp:ListItem Value="4">領用清單</asp:ListItem>
                                                                <asp:ListItem Value="5">位置管理</asp:ListItem>
                                                                <asp:ListItem Value="6">使用者設定</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div style="display:flex;justify-content:center;">
                                                            <asp:DropDownList runat="server" Enabled="false" ID="JobSettingDDL9_2" CssClass="form-control DDLwidth">
                                                                <asp:ListItem Value="1" Selected="True">全局</asp:ListItem>
                                                                <asp:ListItem Value="2">該單位</asp:ListItem>
                                                                <asp:ListItem Value="3">該單位該科室</asp:ListItem>
                                                                <asp:ListItem Value="4">自己</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr runat="server" ID="JobSettingTr10" class="tb-bg">
                                                    <td><asp:RadioButton runat="server" ID="JobSetting10" AutoPostBack="true" OnCheckedChanged="JobSetting_SelectedIndexChanged" GroupName="JobSetting" Text="一般-全單位" /></td>
                                                    <td>
                                                        <div style="display:flex;justify-content:center;">
                                                            <asp:DropDownList runat="server" Enabled="false" ID="JobSettingDDL10_1" CssClass="form-control DDLwidth">
                                                                <asp:ListItem Value="1" Selected="True">我的申請</asp:ListItem>
                                                                <asp:ListItem Value="2">管理查詢</asp:ListItem>
                                                                <asp:ListItem Value="3">庫存管理</asp:ListItem>
                                                                <asp:ListItem Value="4">領用清單</asp:ListItem>
                                                                <asp:ListItem Value="5">位置管理</asp:ListItem>
                                                                <asp:ListItem Value="6">使用者設定</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div style="display:flex;justify-content:center;">
                                                            <asp:DropDownList runat="server" Enabled="false" ID="JobSettingDDL10_2" CssClass="form-control DDLwidth">
                                                                <asp:ListItem Value="1" Selected="True">全局</asp:ListItem>
                                                                <asp:ListItem Value="2">該單位</asp:ListItem>
                                                                <asp:ListItem Value="3">該單位該科室</asp:ListItem>
                                                                <asp:ListItem Value="4">自己</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr runat="server" ID="JobSettingTr11" class="tb-bg">
                                                    <td><asp:RadioButton runat="server" ID="JobSetting11" AutoPostBack="true" OnCheckedChanged="JobSetting_SelectedIndexChanged" GroupName="JobSetting" Text="一般-全科" /></td>
                                                    <td>
                                                        <div style="display:flex;justify-content:center;">
                                                            <asp:DropDownList runat="server" Enabled="false" ID="JobSettingDDL11_1" CssClass="form-control DDLwidth">
                                                                <asp:ListItem Value="1" Selected="True">我的申請</asp:ListItem>
                                                                <asp:ListItem Value="2">管理查詢</asp:ListItem>
                                                                <asp:ListItem Value="3">庫存管理</asp:ListItem>
                                                                <asp:ListItem Value="4">領用清單</asp:ListItem>
                                                                <asp:ListItem Value="5">位置管理</asp:ListItem>
                                                                <asp:ListItem Value="6">使用者設定</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div style="display:flex;justify-content:center;">
                                                            <asp:DropDownList runat="server" Enabled="false" ID="JobSettingDDL11_2" CssClass="form-control DDLwidth">
                                                                <asp:ListItem Value="1" Selected="True">全局</asp:ListItem>
                                                                <asp:ListItem Value="2">該單位</asp:ListItem>
                                                                <asp:ListItem Value="3">該單位該科室</asp:ListItem>
                                                                <asp:ListItem Value="4">自己</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr runat="server" ID="JobSettingTr12" class="tb-bg">
                                                    <td><asp:RadioButton runat="server" ID="JobSetting12" AutoPostBack="true" OnCheckedChanged="JobSetting_SelectedIndexChanged" GroupName="JobSetting" Text="一般-自己" /></td>
                                                    <td>
                                                        <div style="display:flex;justify-content:center;">
                                                            <asp:DropDownList runat="server" Enabled="false" ID="JobSettingDDL12_1" CssClass="form-control DDLwidth">
                                                                <asp:ListItem Value="1" Selected="True">我的申請</asp:ListItem>
                                                                <asp:ListItem Value="2">管理查詢</asp:ListItem>
                                                                <asp:ListItem Value="3">庫存管理</asp:ListItem>
                                                                <asp:ListItem Value="4">領用清單</asp:ListItem>
                                                                <asp:ListItem Value="5">位置管理</asp:ListItem>
                                                                <asp:ListItem Value="6">使用者設定</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div style="display:flex;justify-content:center;">
                                                            <asp:DropDownList runat="server" Enabled="false" ID="JobSettingDDL12_2" CssClass="form-control DDLwidth">
                                                                <asp:ListItem Value="1" Selected="True">全局</asp:ListItem>
                                                                <asp:ListItem Value="2">該單位</asp:ListItem>
                                                                <asp:ListItem Value="3">該單位該科室</asp:ListItem>
                                                                <asp:ListItem Value="4">自己</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                            <div class="mb-1r">
                                <div class="tab-titel cb_louis"style="display:flex;align-items:center;">
                                    <div>三、人員異動</div>
                                    <div style="font-size:0.875rem;margin-left:1rem;font-weight:normal;">註：如有職務調整，請於此設定此帳號業務轉移對象及權限終止時間。</div>
                                </div>
                                <div class="tab-body">
                                    <table class="table2" style="font-size:0.875rem;color:#777777;">
                                        <tr>
                                            <td>註：</td>
                                            <td>1. </td>
                                            <td>如有職務調整，請於此設定此帳號業務轉移對象及權限終止時間。</td>
                                        </tr>
                                        <tr>
                                            <td></td>
                                            <td>2. </td>
                                            <td><sapn style="color:#FF0000;">離職及異動的日期如設定在今日以前的日期，系統將即刻鎖定此帳號</sapn>，並依下方業務移轉對象進行業務資料轉移，
                                                如設定在未來的日期，將於設定日期23:29進行業務資料轉移，並將此帳號「權限」調整為「停止使用者」。</td>
                                        </tr>
                                    </table>
                                    <hr style="margin-top:0.5rem;margin-bottom:0.5rem;" />
                                    <table style="width:100%;">
                                        <tr style="font-size:0.875rem;text-align:center;">
                                            <th style="width:7rem;">方式</th>
                                            <th>異動內容</th>
                                        </tr>
                                    </table>
                                    <asp:UpdatePanel runat="server">
                                        <ContentTemplate>
                                            <script>
                                                $.datepicker.regional['zh-TW'] = {
                                                    dayNames: ["星期日", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六"],
                                                    dayNamesMin: ["日", "一", "二", "三", "四", "五", "六"],
                                                    monthNames: ["一月", "二月", "三月", "四月", "五月", "六月", "七月", "八月", "九月", "十月", "十一月", "十二月"],
                                                    monthNamesShort: ["一月", "二月", "三月", "四月", "五月", "六月", "七月", "八月", "九月", "十月", "十一月", "十二月"],
                                                    prevText: "上月",
                                                    nextText: "次月",
                                                    weekHeader: "週"
                                                };
                                                $.timepicker.regional['zh-TW'] = {
                                                    timeOnlyTitle: "選擇時分秒",
                                                    timeText: "時間",
                                                    hourText: "時",
                                                    minuteText: "分",
                                                    secondText: "秒",
                                                    millisecText: "毫秒",
                                                    timezoneText: "時區",
                                                    currentText: "現在時間",
                                                    closeText: "確定",
                                                    amNames: ["上午", "AM", "A"],
                                                    pmNames: ["下午", "PM", "P"]
                                                };
                                                $.datepicker.setDefaults($.datepicker.regional["zh-TW"]);
                                                $.timepicker.setDefaults($.timepicker.regional["zh-TW"]);
                                                Sys.Application.add_load(function () {
                                                    //$("#ui-datepicker-div").remove();
                                                    $('#ContentPlaceHolder1_date_change').datepicker({
                                                        changeYear: true,
                                                        changeMonth: true,
                                                        stepMinute: 10,
                                                        dateFormat: 'yy-mm-dd',
                                                        onSelect: function (selectedDate) {
                                                            var startDate = new Date(selectedDate);
                                                            var maxEndDate = new Date(startDate);
                                                        }
                                                    });
                                                    $('#ContentPlaceHolder1_date_leave').datepicker({
                                                        changeYear: true,
                                                        changeMonth: true,
                                                        stepMinute: 10,
                                                        dateFormat: 'yy-mm-dd',
                                                        onSelect: function (selectedDate) {
                                                            var endDate = new Date(selectedDate);
                                                            var minStartDate = new Date(endDate);
                                                        }
                                                    });
                                                    $("#ContentPlaceHolder1_date_change_icon").click(function (e) {
                                                        if ($("#ContentPlaceHolder1_date_change").is(":disabled"))
                                                            e.preventDefault(); // 阻止開啟日曆
                                                        else
                                                            $("#ContentPlaceHolder1_date_change").datepicker("show");
                                                    });
                                                    $("#ContentPlaceHolder1_date_leave_icon").click(function (e) {
                                                        if ($("#ContentPlaceHolder1_date_leave").is(":disabled"))
                                                            e.preventDefault(); // 阻止開啟日曆
                                                        else
                                                            $("#ContentPlaceHolder1_date_leave").datepicker("show");
                                                    });
                                                });
                                            </script>
                                            <div runat="server" ID="ChangeTr1" class="tb-bg mb-025r" style="display:flex;align-items:center;padding:0.5rem;">
                                                <asp:RadioButton runat="server" ID="Change1" AutoPostBack="true" OnCheckedChanged="Change_SelectedIndexChanged" 
                                                    style="font-weight:bold;" GroupName="Change" Text="無" />
                                            </div>
                                            <div runat="server" ID="ChangeTr2" class="tb-bg mb-025r" style="display:flex;align-items:center;height:auto;padding:0.5rem;">
                                                <table style="width:100%;">
                                                    <tr>
                                                        <td rowspan="2" style="width:7rem;border-right:solid 1px;">
                                                            <asp:RadioButton runat="server" ID="Change2" AutoPostBack="true" OnCheckedChanged="Change_SelectedIndexChanged" GroupName="Change" Text="離職" />
                                                        </td>
                                                        <td style="padding-left:1rem;">
                                                            <div style="display:flex;align-items:center;">
                                                                <div class="ItemTitle" style="font-weight:normal;"><span>離職日期設定</span></div>
                                                                <div class="ItemContent">
                                                                    <asp:TextBox AutoPostBack="false" CssClass="form-control changeDDL" runat="server" ID="date_leave" placeholder="年/月/日"  
                                                                        Enabled="false"></asp:TextBox>
                                                                    <asp:Image runat="server" ImageUrl="image/icon_14.png" ID="date_leave_icon" 
                                                                        style="top:-3px;left: -40px;position: relative;width:30px;height:30px"/>
                                                                </div>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="padding-left:1rem;">
                                                            <div style="display:flex;align-items:center;">
                                                                <div class="ItemTitle" style="font-weight:normal;"><span>業務轉移對象</span></div>
                                                                <div class="ItemContent" style="display:flex;">
                                                                    <asp:DropDownList runat="server" ID="leaveDDL_group" CssClass="form-control changeDDL" AutoPostBack="true" Enabled="false"
                                                                        DataTextField="name" DataValueField="id" DataSourceID="SqlDataSource1" OnSelectedIndexChanged="leaveDDL_group_SelectedIndexChanged">
                                                                    </asp:DropDownList>
                                                                    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:eip %>" 
                                                                        SelectCommand="Select id, name From group_name where parent_id is null order by LEN(name)"></asp:SqlDataSource>
                                                                    <asp:DropDownList runat="server" ID="leaveDDL_UserID" CssClass="form-control ml-1 changeDDL" Enabled="false"
                                                                        DataTextField="name" DataValueField="id" ></asp:DropDownList>
                                                                </div>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                            <div runat="server" ID="ChangeTr3" class="tb-bg mb-025r" style="display:flex;align-items:center;height:auto;padding:0.5rem;">
                                                <table style="width:100%;">
                                                    <tr>
                                                        <td rowspan="2" style="width:7rem;border-right:solid 1px;">
                                                            <asp:RadioButton runat="server" ID="Change3" AutoPostBack="true" OnCheckedChanged="Change_SelectedIndexChanged" GroupName="Change" Text="職務異動" />
                                                        </td>
                                                        <td style="padding-left:1rem;">
                                                            <div style="display:flex;align-items:center;">
                                                                <div class="ItemTitle" style="font-weight:normal;"><span>異動日期設定</span></div>
                                                                <div class="ItemContent">
                                                                    <asp:TextBox AutoPostBack="false" CssClass="form-control changeDDL" runat="server" ID="date_change" placeholder="年/月/日" 
                                                                        Enabled="false"></asp:TextBox>
                                                                    <asp:Image runat="server" ImageUrl="image/icon_14.png" ID="date_change_icon" 
                                                                        style="top:-3px;left: -40px;position: relative;width:30px;height:30px"/>
                                                                </div>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="padding-left:1rem;">
                                                            <div style="display:flex;align-items:center;">
                                                                <div class="ItemTitle" style="font-weight:normal;"><span>業務轉移對象</span></div>
                                                                <div class="ItemContent" style="display:flex;">
                                                                    <asp:DropDownList runat="server" ID="changeDDL_group" CssClass="form-control changeDDL" AutoPostBack="true" Enabled="false"
                                                                        DataTextField="name" DataValueField="id" DataSourceID="SqlDataSource1" OnSelectedIndexChanged="changeDDL_group_SelectedIndexChanged">
                                                                    </asp:DropDownList>
                                                                    <asp:DropDownList runat="server" ID="changeDDL_UserID" CssClass="form-control ml-1 changeDDL" Enabled="false"
                                                                        DataTextField="name" DataValueField="id" ></asp:DropDownList>
                                                                </div>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

            </div>
        </asp:Panel>
    </div>
    <div class="d-flex  justify-content-center">
        <asp:Button ID="Cancel" class="bt" runat="server" Text="返回" Style="background: #777777" OnClick="Cancel_Click" />
        <asp:Button ID="Submit" class="bt" runat="server" Text="儲存" Visible="false" OnClick="Submit_Click" />
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
</asp:Content>

