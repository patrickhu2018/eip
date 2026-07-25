<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage3.master" AutoEventWireup="true" CodeFile="Meetingroom_user_setting.aspx.cs" Inherits="Meetingroom_user_setting" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script>
        function showModal1() {
            $('#modal1').modal('show');
            return false;
        }
        function hideModal1() {
            $('#modal1').modal('hide');
            return false;
        }
    </script>
    <style>
        .bt {
            width: 100px;
            height: 40px;
            background: #4CAF1E 0% 0% no-repeat padding-box;
            border: 0px;
            border-radius: 5px;
            opacity: 1;
            text-align: center;
            color: #FFFFFF;
            margin-right: 10px;
        }

        .addbt {
            width: 120px;
            height: 120px;
            background: #EF9103 0% 0% no-repeat padding-box;
            box-shadow: 0px 3px 6px #00000029;
            border: 0px;
            border-radius: 10px;
            opacity: 1;
            text-align: center;
            font: normal normal bold 22px/28px Microsoft YaHei UI;
            letter-spacing: 0px;
            color: #FFFFFF;
        }

        .searchbt {
            width: 60px;
            height: 36px;
            background: #FF9900 0% 0% no-repeat padding-box;
            border-radius: 3px;
            border: 0px;
            opacity: 1;
            text-align: center;
            letter-spacing: 0px;
            color: #FFFFFF;
        }


        .modal-backdrop {
            z-index: 0;
            background-color: rgba(0, 0, 0, 1); /* 調整透明度 */
        }

        .table th, .table td {
            padding: 0.4rem 0.3rem;
        }

        .exportbt {
            padding: 5px 20px 5px 35px;
            background: #5C9C00 0% 0% no-repeat padding-box;
            border-radius: 3px;
            border: 0px;
            color: #FFFFFF;
            opacity: 1;
            background-image: url(../image/icon_13.png);
            background-position: left center;
            /*padding-left: 20px;*/
        }

        .exportbt {
            padding: 5px 20px 5px 35px;
            background: #5C9C00 0% 0% no-repeat padding-box;
            border-radius: 3px;
            border: 0px;
            color: #FFFFFF;
            opacity: 1;
            background-image: url(../image/icon_13.png);
            background-position: left center;
            /*padding-left: 20px;*/
        }
    </style>
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div class="d-flex">
        <div class="write_Box" style="flex: 0 0 100%;">
            <div class="write_Title">
                <h5>
                    <asp:Literal runat="server" ID="box_title">帳號查詢</asp:Literal></h5>
            </div>
            <div class="write_textBox">
                <div class="d-flex justify-content-between" style="width: 100%">
                    <asp:UpdatePanel ID="UpdatePanel2" class="dataBox row" style="width: 100%;" runat="server">
                        <ContentTemplate>
                            <table style="margin-right: 15px; margin-left: 15px;">
                                <tr>
                                    <td style="width: 100px; font-weight: bold;">組室</td>
                                    <td style="width: 200px;">
                                        <asp:DropDownList runat="server" ID="add_unit_group" CssClass="form-control" Style=""
                                            AutoPostBack="true" OnSelectedIndexChanged="add_unit_group_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 100px;"></td>
                                    <td style="width: 75px; font-weight: bold;">科室</td>
                                    <td style="width: 200px;">
                                        <asp:DropDownList runat="server" ID="add_unit_class" CssClass="form-control ml-1" Style="" AutoPostBack="true"></asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                            <!-- 組室欄位 -->
                            <%--<div class="col-6 col-lg-5 formItem title6" style="">
                                <div class="ItemTitle"><span>組室</span></div>
                                <div class="ItemContent">
                                    <asp:DropDownList runat="server" ID="add_unit_group" CssClass="form-control" Style="width: 200px; display: inline;" AutoPostBack="true" OnSelectedIndexChanged="add_unit_group_SelectedIndexChanged"></asp:DropDownList>
                                </div>
                            </div>

                            <div class="col-6 col-lg-5 formItem title2" style="margin-left: 0px;">
                                <div class="ItemTitle"><span>科室</span></div>
                                <div class="ItemContent">
                                    <asp:DropDownList runat="server" ID="add_unit_class" CssClass="form-control ml-1" Style="width: 200px; display: inline;" AutoPostBack="true"></asp:DropDownList>
                                </div>
                            </div>--%>
                            <!-- 狀態欄位 -->
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <%--<table style="margin-top: 10px">
                    <tr>
                        <td style="width: 100px; font-weight: bold;">狀態</td>
                        <td style="">
                            <asp:RadioButton runat="server" ID="search1_rb_all" GroupName="search1_rb" /><span>全選</span>
                            <asp:RadioButton runat="server" Checked="true" ID="search1_rb_up" GroupName="search1_rb" Style="margin-left: 25px;" /><span>系統管理者</span>
                            <asp:RadioButton runat="server" Checked="true" ID="search1_rb_up2" GroupName="search1_rb" Style="margin-left: 25px;" /><span>業務管理者</span>
                            <asp:RadioButton runat="server" ID="search1_rb_down" GroupName="search1_rb" Style="margin-left: 25px;" /><span>停用</span>
                        </td>
                    </tr>
                    <tr style="height: 10px;"></tr>
                    <tr style="">
                        <td style="width: 100px; font-weight: bold;">關鍵字查詢</td>
                        <td style="">
                            <asp:TextBox ID="keyword" runat="server" CssClass="form-control" Style="width: 185%" placeholder="請輸入使用者、帳號或職位來做查詢"></asp:TextBox>
                        </td>
                    </tr>
                </table>--%>
                <table style="margin-top: 10px">
                    <tr>
                        <td style="width:100px;font-weight: bold;">狀態</td>
                        <td style="">
                            <asp:RadioButton runat="server" ID="rb_status1" Checked="true" GroupName="search1_rb" /><span>全選</span>
                            <asp:RadioButton runat="server" ID="rb_status2" GroupName="search1_rb" Style="margin-left: 25px;" /><span>系統管理者</span>
                            <asp:RadioButton runat="server" ID="rb_status3" GroupName="search1_rb" Style="margin-left: 25px;" /><span>主計業務管理者</span>
                            <asp:RadioButton runat="server" ID="rb_status4" GroupName="search1_rb" Style="margin-left: 25px;" /><span>一般業務管理者</span>
                            <asp:RadioButton runat="server" ID="rb_status5" GroupName="search1_rb" Style="margin-left: 25px;" /><span>一般使用者</span>
                        </td>
                    </tr>
                    <tr>
                        <td style="width:100px;"></td>
                        <td>
                            <asp:RadioButton runat="server" ID="rb_status6" GroupName="search1_rb" /><span>主計登記桌</span>
                            <asp:RadioButton runat="server" ID="rb_status7" GroupName="search1_rb" Style="margin-left: 25px;" /><span>一般登記桌</span>
                            <asp:RadioButton runat="server" ID="rb_status8" GroupName="search1_rb" Style="margin-left: 25px;" /><span>審核使用者</span>
                            <asp:RadioButton runat="server" ID="rb_status9" GroupName="search1_rb" Style="margin-left: 25px;" /><span>免審核使用者</span>
                            <asp:RadioButton runat="server" ID="rb_status0" GroupName="search1_rb" Style="margin-left: 25px;" /><span>停止使用者</span>
                        </td>
                    </tr>
                    <tr style="height:10px;"></tr>
                    <tr style="">
                        <td style="width:100px;font-weight: bold;">關鍵字查詢</td>
                        <td style="">
                            <asp:TextBox ID="keyword" runat="server" CssClass="form-control" Style="width: 120%" placeholder="請輸入使用者、帳號或職位來做查詢"></asp:TextBox>
                        </td>
                    </tr>
                </table>

                <%--<div class="dataBox row" style="width: 80%;">
                    <div class="col-12 col-lg-12 formItem title5" style="flex-wrap: wrap; margin-left: 0px;">
                        <div class="ItemTitle"><span>狀態</span></div>
                        <div class="ItemContent">
                            <div class="row">
                                <div class="col-12 col-sm-8" style="margin-left: 15px; display: flex; flex-wrap: wrap;">
                                    <asp:RadioButton runat="server" ID="search1_rb_all" GroupName="search1_rb" /><span>全選</span>
                                    <asp:RadioButton runat="server" Checked="true" ID="search1_rb_up" GroupName="search1_rb" Style="margin-left: 25px;" /><span>啟用中</span>
                                    <asp:RadioButton runat="server" ID="search1_rb_down" GroupName="search1_rb" Style="margin-left: 25px;" /><span>停用</span>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="dataBox row" style="width: 80%;">
                    <div class="col-6 formItem title6">
                        <div class="ItemTitle"><span>關鍵字查詢</span></div>
                        <div class="ItemContent">
                            <asp:TextBox ID="keyword" runat="server" CssClass="form-control" Style="width: 150%" placeholder="請輸入使用者、帳號或其他關鍵字來做查詢"></asp:TextBox>
                        </div>
                    </div>
                </div>--%>
            </div>

            <div class="write_textBox d-flex justify-content-center">
                <asp:Button ID="searchbt" runat="server" Text="查詢" CssClass="searchbt" OnClick="searchbt_Click" />
            </div>
        </div>
        <%-- <asp:Button ID="adduser" runat="server" Text="新增&#10;帳號" CssClass="ml-auto addbt" Style="white-space: pre-line;" OnClick="adduser_Click" />--%>
    </div>
    <div class="write_Box">
        <div class="write_Title" style="display: flex; align-items: center; justify-content:space-between;">
            <h5><asp:Literal runat="server" ID="Literal1">查詢結果</asp:Literal></h5>
            <div style="width: auto; text-align: right;" id="btn_list" runat="server">
                <asp:Button ID="btn_listBtn" runat="server" Text="人員清單" CssClass="exportbt" OnClick="listBtn_Click"></asp:Button>
            </div>
        </div>
        <div class="write_textBox">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <asp:GridView ID="gv" runat="server" class="table table-bordered mt-2" AutoGenerateColumns="False" OnRowCommand="gv_RowCommand" DataSourceID="SqlDataSource1"
                        OnRowDataBound="gv_RowDataBound" OnPageIndexChanging="gv_PageIndexChanging" OnSorting="gv_Sorting" PageSize="10" AllowPaging="true" AllowSorting="True" PagerSettings-Visible="false">
                        <EmptyDataTemplate>
                            無資料
                        </EmptyDataTemplate>
                        <HeaderStyle CssClass="table-topbar" />
                        <Columns>
                            <asp:BoundField DataField="user_id" HeaderText="user_id" ItemStyle-Width="1%" Visible="false" />
                            <asp:TemplateField HeaderText='組室<span class="fa fa-caret-down"/>' ItemStyle-HorizontalAlign="center" ItemStyle-Width="1%" SortExpression="gid">
                                <ItemTemplate>
                                    <asp:Label ID="gid" runat="server" Text='<%# Eval("gid") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText='科室<span class="fa fa-caret-down"/>' ItemStyle-HorizontalAlign="center" ItemStyle-Width="1%" SortExpression="user_group">
                                <ItemTemplate>
                                    <asp:Label ID="user_group" runat="server" Text='<%# Eval("user_group") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText='使用者<span class="fa fa-caret-down"/>' ItemStyle-HorizontalAlign="center" ItemStyle-Width="1%" SortExpression="name">
                                <ItemTemplate>
                                    <asp:Label ID="name" runat="server" Text='<%# Eval("name") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText='員工編號<span class="fa fa-caret-down"/>' ItemStyle-HorizontalAlign="center" ItemStyle-Width="1%" SortExpression="sn">
                                <ItemTemplate>
                                    <asp:Label ID="sn" runat="server" Text='<%# Eval("sn") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText='職位<span class="fa fa-caret-down"/>' ItemStyle-HorizontalAlign="center" ItemStyle-Width="1%" SortExpression="job">
                                <ItemTemplate>
                                    <asp:Label ID="job" runat="server" Text='<%# Eval("job") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText='管理權限狀態<span class="fa fa-caret-down"/>' ItemStyle-HorizontalAlign="center" ItemStyle-Width="1%" SortExpression="meeting_show_page">
                                <ItemTemplate>
                                    <asp:Label ID="state" runat="server" Text='<%# Eval("meeting_show_page") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="0.5%">
                                <HeaderTemplate>
                                    <span>功能</span>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Button runat="server" CssClass="gw_bt" ID="modify" CommandName="modify" CommandArgument='<%# Eval("user_id") %>' Text="編輯" Style="background-color: #68B100; margin-right: 8px;" />
                                    <%--  <asp:Button runat="server" CssClass="gw_bt" ID="del" CommandName="del" CommandArgument='<%# Eval("user_id") %>' Text="移除" Style="background-color: #B83F1F" />--%>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:eip %>" SelectCommand=""></asp:SqlDataSource>
                    <div id="down" runat="server" class="btn-toolbar" role="toolbar" aria-label="Toolbar with button groups" style="justify-content: right;">
                        <div class="btn-group pull-right" role="group" aria-label="First group" style="align-items: center;">

                            <div class="NumberOfPages" style="padding: 5px; float: left">
                                第<asp:Label ID="lblPageIndex" runat="server" Text="0" CssClass="Number" Style="color: #0071BC"></asp:Label>頁    
                            </div>

                            <div class="NumberOfPages" style="padding: 5px; float: left">
                                總計<asp:Label ID="lblDataCount" runat="server" Text="0" CssClass="Number" Style="color: #0071BC"></asp:Label>筆
                            </div>

                            <div class="NumberOfPages" style="padding: 5px; float: left">跳至</div>

                            <asp:DropDownList ID="ddlPageIndex" runat="server" AutoPostBack="True" class="NumberOfSelect form-control" Style="width: auto; padding: 0px; float: left; height: 30px;"
                                OnSelectedIndexChanged="ddlPageIndex_SelectedIndexChanged">
                            </asp:DropDownList>

                            <div class="NumberOfPages" style="padding: 5px; float: left">頁 </div>

                            <div class="NumberOfPages" style="padding: 5px; float: left">
                                <asp:LinkButton ID="lkbPagePrev" runat="server" OnClick="lkbPagePrev_Click" Style="color: #0071BC; text-decoration: underline;">上一頁</asp:LinkButton>
                                <asp:LinkButton ID="lkbPageNext" runat="server" OnClick="lkbPageNext_Click" Style="color: #0071BC; text-decoration: underline;">下一頁</asp:LinkButton>
                            </div>

                            <div class="NumberOfPages" style="padding: 5px; float: left">每頁顯示</div>

                            <asp:DropDownList ID="ddlPageSize" runat="server" AutoPostBack="True" class="NumberOfSelect form-control" Style="padding: 0px; width: auto; float: left; height: 30px;" OnSelectedIndexChanged="ddlPageSize_SelectedIndexChanged">
                                <asp:ListItem Selected="True">10</asp:ListItem>
                                <asp:ListItem>20</asp:ListItem>
                                <asp:ListItem>50</asp:ListItem>
                                <asp:ListItem>100</asp:ListItem>
                            </asp:DropDownList>

                            <div class="NumberOfPages" style="padding: 5px; float: left">筆</div>

                        </div>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="searchbt" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
    <div class="modal fade bs-NewUser-modal-lg" id="modal1" tabindex="-1" role="dialog" aria-labelledby="myLargeModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg modal-dialog-centered" style="max-width: 300px;">
            <div class="modal-content">
                <div class="modal-header" style="background-color: #C5ECFF; justify-content: center; font-size: 22px;">
                    <b>刪除確認</b>
                </div>
                <div class="modal-body">
                    <div class="MainClass">
                        <div id="content" role="form" class="form-horizontal text-center" style="margin-top: 50px; margin-bottom: 50px;">
                            <b>確定要刪除嗎?</b>
                            <br>
                            <b>確認後無法復原</b>
                        </div>
                        <hr>
                        <div style="text-align: center">
                            <asp:Button ID="Cancel" runat="server" Text="取消" class="bt" Style="background: #777777"
                                OnClientClick="return hideModal1();" />
                            <asp:Button ID="delbt" runat="server" Text="提交" class="bt"
                                OnClick="delbt_Click" />
                        </div>
                        <asp:HiddenField ID="hf_del" runat="server" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
</asp:Content>

