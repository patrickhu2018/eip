<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage2.master" AutoEventWireup="true" CodeFile="Repair_listquery.aspx.cs" Inherits="Repair_listquery" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style>
        .addbt {
            background: #EF9103 0% 0% no-repeat padding-box;
            box-shadow: 0px 3px 6px #00000029;
            border: 0px;
            border-radius: 10px;
            opacity: 1;
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

        .ItemContent label {
            margin-bottom: 0;
            margin-right: 20px;
        }

        .ItemContent input[type=radio] {
            margin-right: 5px;
        }
    </style>
    <div class="write_Box">
        <div class="write_Title">
            <h5>
                <asp:Literal runat="server" ID="box_title">申請查詢</asp:Literal></h5>
        </div>
        <div class="write_textBox">
            <div class="dataBox row d-flex" style="margin-right: 50px; width: 100%">
                <div class=" col-md-4 formItem title3">
                    <div class="ItemTitle"><span>修繕地點</span></div>
                    <div class="ItemContent">
                        <asp:DropDownList ID="apply_place" runat="server" CssClass="form-control" Style="width: 100%" OnSelectedIndexChanged="apply_place_SelectedIndexChanged" AutoPostBack="true">
                            <asp:ListItem Value="0">全部</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class=" col-md-4 formItem title3" style="margin-left:11px;">
                    <div class="ItemTitle">
                        <span>修繕樓層</span>
                    </div>
                    <div class="ItemContent">
                        <asp:DropDownList ID="apply_floor" runat="server" CssClass="form-control" Style="width: 100%">
                            <asp:ListItem Value="0">全部</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
            </div>
            <div class="dataBox row" style="display: flex; align-items: center;">
                <div id="src1" class="col-6 col-lg-8 col-xl-4 formItem title3">
                    <div class="ItemTitle"><span>處理狀況</span></div>
                    <div class="ItemContent" style="display: flex; align-items: center;">
                        <div class="row" style="width: auto">
                            <div style="margin-left: 15px;">
                                <asp:CheckBox runat="server" ID="chk1" Style="margin-right: 5px;" /><span>待處理</span>
                                <asp:CheckBox runat="server" ID="chk2" Style="margin-right: 5px; margin-left: 5px;" /><span>處理中</span>
                                <asp:CheckBox runat="server" ID="chk3" Style="margin-right: 5px; margin-left: 5px;" /><span>已完成</span>
                            </div>
                        </div>
                    </div>
                </div>
                <div style="display: flex; align-items: center; flex-wrap: wrap;">
                    <div class="col-12  formItem title3">
                        <div class="ItemTitle"><span>申請日期</span></div>
                        <div class="ItemContent" style="display: flex; align-items: center; flex-wrap: wrap;">
                            <asp:RadioButton ID="rb_lastWeek" runat="server" GroupName="applyDate" Text="近一周" Checked="true" Style="margin-right: 0px;" AutoPostBack="true" OnCheckedChanged="rb_SelectedIndexChanged" />
                            <asp:RadioButton ID="rb_lastMonth" runat="server" GroupName="applyDate" Text="近一個月" Style="margin-right: 5px; margin-left: 0px;" AutoPostBack="true" OnCheckedChanged="rb_SelectedIndexChanged" />
                            <asp:RadioButton ID="rb_lastYear" runat="server" GroupName="applyDate" Text="近一年" Style="margin-right: 5px; margin-left: 0px;" AutoPostBack="true" OnCheckedChanged="rb_SelectedIndexChanged" />
                            <div class="d-flex align-items-center">
                                <asp:RadioButton ID="rb_customRange" runat="server" GroupName="applyDate" Text="自訂區間" Style="margin-right: 0px; margin-left: 0px;" AutoPostBack="true" OnCheckedChanged="rb_SelectedIndexChanged" />
                                <div style="flex: auto">
                                    <asp:TextBox ID="apply_start" runat="server" TextMode="Date" CssClass="form-control" Style="width: 150px; height: 35px; margin-left: 10px;" Enabled="false" OnChange="setEndDateRange()"></asp:TextBox>
                                    <span class="right" style="font-size: 22px; margin: 0 5px;">～</span>
                                    <asp:TextBox ID="apply_end" runat="server" TextMode="Date" CssClass="form-control" Style="width: 150px; height: 35px; margin-left: 8px;" Enabled="false" OnChange="setStartDateRange()"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="dataBox row">
                <div class="col-6 formItem title3">
                    <div class="ItemTitle"><span>關鍵字</span></div>
                    <div class="ItemContent" style="display: flex; align-items: center;">
                        <asp:TextBox ID="keyword" runat="server" CssClass="form-control" placeholder="請輸入申請人、申請位置或事由來做查詢"></asp:TextBox>
                    </div>
                </div>
            </div>


        </div>
        <div class="write_textBox d-flex justify-content-center">
            <asp:Button ID="searchbt" runat="server" Text="查詢" CssClass="searchbt" OnClick="searchbt_Click" />
        </div>
    </div>
    <div class="write_Box">
        <div class="write_Title">
            <h5>
                <asp:Literal runat="server" ID="Literal1">查詢結果</asp:Literal></h5>
        </div>
        <div class="write_textBox">
            <asp:GridView ID="gv" runat="server" class="table table-bordered mt-2" AutoGenerateColumns="False" OnRowCommand="gv_RowCommand" OnRowDataBound="gv_RowDataBound"
                OnPageIndexChanging="gv_PageIndexChanging" OnSorting="gv_Sorting" PageSize="10" AllowPaging="true" AllowSorting="True" PagerSettings-Visible="false">
                <EmptyDataTemplate>
                    無資料
                </EmptyDataTemplate>
                <HeaderStyle CssClass="table-topbar" />
                <Columns>
                    <asp:BoundField DataField="id" HeaderText="id" ItemStyle-Width="1%" Visible="false" />
                    <asp:TemplateField HeaderText='修繕單編號<span class="fa fa-caret-down"/>' ItemStyle-HorizontalAlign="center" ItemStyle-Width="1%" SortExpression="repair_no">
                        <ItemTemplate>
                            <asp:Label ID="repair_no" runat="server" Text='<%# Eval("repair_no") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText='申請日期<span class="fa fa-caret-down"/>' ItemStyle-HorizontalAlign="center" ItemStyle-Width="1%" SortExpression="apply_date">
                        <ItemTemplate>
                            <asp:Label ID="apply_date" runat="server" Text='<%# Eval("apply_date","{0:yyyy/MM/dd}") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText='申請人<span class="fa fa-caret-down"/>' ItemStyle-HorizontalAlign="center" ItemStyle-Width="1%" SortExpression="apply_user">
                        <ItemTemplate>
                            <asp:Label ID="apply_user" runat="server" Text='<%# Eval("apply_user") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText='修繕地點<span class="fa fa-caret-down"/>' ItemStyle-HorizontalAlign="center" ItemStyle-Width="1%" SortExpression="place_id">
                        <ItemTemplate>
                            <asp:Label ID="place_id" runat="server" Text='<%# Eval("place_name") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText='修繕樓層<span class="fa fa-caret-down"/>' ItemStyle-HorizontalAlign="center" ItemStyle-Width="1%" SortExpression="floor_id">
                        <ItemTemplate>
                            <asp:Label ID="floor_id" runat="server" Text='<%# Eval("floor_name") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText='位置<span class="fa fa-caret-down"/>' ItemStyle-HorizontalAlign="center" ItemStyle-Width="3%" SortExpression="location_id">
                        <ItemTemplate>
                            <asp:Label ID="location_id" runat="server" Text='<%# Eval("Location_name") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText='事由<span class="fa fa-caret-down"/>' ItemStyle-HorizontalAlign="left" ItemStyle-Width="3%" SortExpression="apply_reason">
                        <ItemTemplate>
                            <asp:Label ID="apply_reason" runat="server" Text='<%# Eval("apply_reason") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText='處理狀況<span class="fa fa-caret-down"/>' ItemStyle-HorizontalAlign="center" ItemStyle-Width="1%" SortExpression="state">
                        <ItemTemplate>
                            <asp:Label ID="state" runat="server" Text='<%# Eval("state") %>' Visible="false"></asp:Label>
                            <asp:Label ID="stateDisplay" runat="server" Text='<%# Eval("state", "{0}") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-Width="2%">
                        <HeaderTemplate>
                            <span>功能</span>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Button runat="server" CssClass="gw_bt" ID="check" CommandName="check" CommandArgument='<%# Eval("id") %>' Text="檢視" Style="background-color: #1885C5" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
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
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
</asp:Content>

