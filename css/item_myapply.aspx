<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="item_myapply.aspx.cs" Inherits="item_myapply" MaintainScrollPositionOnPostback="true" %>

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

        .gw_bt {
            width: auto;
            background: #68B100;
            border: 1px solid transparent;
            border-radius: 5px;
            color: #ffffff;
            opacity: 1;
        }

        .searchbt {
            padding:5px 15px;
            background: #FF9900 0% 0% no-repeat padding-box;
            border-radius: 3px;
            border: 0px;
            opacity: 1;
            text-align: center;
            letter-spacing: 0px;
            color: #FFFFFF;
        }

        @media (min-width: 768px) and (max-width: 1745px) {
            .dll {
                width: 100%;
                flex: 0 0 100% !important;
            }
            #date{
                width:60%!important;
            }
        }

        @media (min-width: 768px) and (max-width: 1080px) {

            .apply_Buget {
                width: 120% !important
            }
        }
    </style>
    <div class="d-flex">
        <div class="write_Box" style="margin-right: 4rem;">
            <div class="write_Title">
                <h5>
                    <asp:Literal runat="server" ID="box_title">申請查詢</asp:Literal></h5>
            </div>
            <div class="write_textBox">
                <div class="dataBox row d-flex" style="margin-right: 60px;">
                    <div class="dll" style="display: flex; align-items: center; flex: 0 0 45%">
                        <div class="col-5 formItem title3">
                            <div class="ItemTitle"><span>申請組室</span></div>
                            <div class="ItemContent">
                                <asp:DropDownList ID="apply_group" runat="server" CssClass="form-control" Style="width: 100%" Enabled="false">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-6 formItem title2">
                            <div class="ItemTitle">
                                <span>品名</span>
                            </div>
                            <div class="ItemContent">
                                <asp:DropDownList ID="apply_product" runat="server" CssClass="form-control" Style="width: 110%">
                                    <asp:ListItem Value="0">全選</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div class="dll" style="display: flex; align-items: center; flex-wrap: wrap; flex: 0 0 55%">
                        <div class="col-7 formItem title3">
                            <div class="ItemTitle"><span>預算科目</span></div>
                            <div class="ItemContent">
                                <asp:DropDownList ID="apply_Buget" runat="server" CssClass="form-control apply_Buget" Style="width: 110%">
                                    <asp:ListItem Value="0">全選</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-4 formItem title2">
                            <div class="ItemTitle">
                                <span>狀態</span>
                            </div>
                            <div class="ItemContent">
                                <asp:DropDownList ID="apply_state" runat="server" CssClass="form-control" Style="width: 100%">
                                    <asp:ListItem Value="0">全選</asp:ListItem>
                                    <asp:ListItem Value="1">申請中</asp:ListItem>
                                    <asp:ListItem Value="2">已核銷</asp:ListItem>
                                    <asp:ListItem Value="3">退件</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="dataBox row">
                    <div class="col-6 formItem title6">
                        <div class="ItemTitle" style="width: 30rem;"><span>申請及核銷查詢</span><span style="color: #375471; font-size: 14px;">*申請日期和核銷日期僅能二擇一做查詢</span></div>
                    </div>
                </div>
                <div id="date" style="border: 1px dashed #FBA2D5; padding: 10px; width: 40%;">
                    <div class="dataBox formItem">
                        <div style="width: auto">
                            <div class="ItemTitle"><span>申請日期</span></div>
                            <div class="ItemContent" style="display: flex; align-items: center; flex-wrap: wrap;">
                                <!-- RadioButtonList -->
                                <asp:RadioButtonList ID="rb_applyDate" runat="server" CssClass="rbListFlex_middle" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="rb_SelectedIndexChanged">
                                    <asp:ListItem Value="不限" Selected="True" style="margin-right: 10px;"></asp:ListItem>
                                    <asp:ListItem Value="近一週" style="margin-right: 10px;"></asp:ListItem>
                                    <asp:ListItem Value="近一個月內" style="margin-right: 10px;"></asp:ListItem>
                                    <asp:ListItem Value="近一年" style="margin-right: 10px;"></asp:ListItem>
                                    <%--<asp:ListItem Value="自訂區間" style="margin-right: 10px;"></asp:ListItem>--%>
                                </asp:RadioButtonList>
                              <%--  <div style="flex: auto">
                                    <!-- Start Date TextBox -->
                                    <asp:TextBox ID="apply_start" runat="server" TextMode="Date" CssClass="form-control"
                                        Style="width: 28%; height: 35px; margin-left: 10px;" Enabled="false"></asp:TextBox>

                                    <!-- Separator "～" -->
                                    <span class="right" style="font-size: 22px; margin: 0 5px;">～</span>

                                    <!-- End Date TextBox -->
                                    <asp:TextBox ID="apply_end" runat="server" TextMode="Date" CssClass="form-control"
                                        Style="width: 28%; height: 35px; margin-left: 8px;" Enabled="false"></asp:TextBox>
                                </div>--%>
                            </div>
                        </div>
                    </div>
                    <div class="dataBox formItem" style="margin-top: 10px; margin-left: 0;">
                        <div style="width: 100%">
                            <div class="ItemTitle"><span>核銷日期</span></div>
                            <div class="ItemContent" style="display: flex; align-items: center; flex-wrap: wrap;">
                                <asp:RadioButtonList ID="rb_passDate" runat="server" CssClass="rbListFlex_middle" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="rb_SelectedIndexChanged">
                                    <asp:ListItem Value="不限" Selected="True" style="margin-right: 10px;"></asp:ListItem>
                                    <asp:ListItem Value="近一週" style="margin-right: 10px;"></asp:ListItem>
                                    <asp:ListItem Value="近一個月內" style="margin-right: 10px;"></asp:ListItem>
                                    <asp:ListItem Value="近一年" style="margin-right: 10px;"></asp:ListItem>
                                  <%--  <asp:ListItem Value="自訂區間" style="margin-right: 10px;"></asp:ListItem>--%>
                                </asp:RadioButtonList>
                              <%--  <div style="flex: auto">
                                    <asp:TextBox ID="pass_start" runat="server" TextMode="Date" CssClass="form-control" Style="width: 28%; height: 35px; margin-left: 10px;" Enabled="false"></asp:TextBox>
                                    <span class="right" style="font-size: 22px; margin: 0 5px;">～</span>
                                    <asp:TextBox ID="pass_end" runat="server" TextMode="Date" CssClass="form-control" Style="width: 28%; height: 35px; margin-left: 8px;" Enabled="false"></asp:TextBox>
                                </div>--%>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="write_textBox d-flex justify-content-center">
                <asp:Button ID="searchbt" runat="server" Text="查詢" OnClick="searchbt_Click" CssClass="searchbt" />
            </div>
        </div>
        <asp:Button ID="addapply" runat="server" Text="申請&#10;特殊用品" CssClass="ml-auto addbt" Style="white-space: pre-line;" OnClick="addapply_Click" />
    </div>
    <div class="write_Box" style="width: 100%">
        <div class="write_Title">
            <h5>
                <asp:Literal runat="server" ID="Literal1">查詢結果</asp:Literal></h5>
        </div>
        <div class="write_textBox">
            <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <asp:GridView ID="gv" runat="server" class="table table-bordered mt-2" AutoGenerateColumns="False" OnRowCommand="gv_RowCommand" OnRowDataBound="gv_RowDataBound"
                        OnPageIndexChanging="gv_PageIndexChanging" OnSorting="gv_Sorting" PageSize="10" AllowPaging="true" AllowSorting="True" PagerSettings-Visible="False">
                        <EmptyDataTemplate>
                            無資料
                        </EmptyDataTemplate>
                        <HeaderStyle CssClass="table-topbar" />
                        <Columns>
                            <asp:BoundField DataField="id" HeaderText="id" ItemStyle-Width="1%" Visible="false" />
                            <asp:TemplateField HeaderText="組室" ItemStyle-HorizontalAlign="center" ItemStyle-Width="1%" SortExpression="apply_group">
                                <ItemTemplate>
                                    <asp:Label ID="apply_group" runat="server" Text='<%# Eval("apply_group") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="申請日期" ItemStyle-HorizontalAlign="center" ItemStyle-Width="1%">
                                <ItemTemplate>
                                    <asp:Label ID="apply_date" runat="server" Text='<%# Eval("apply_date","{0:yyyy/MM/dd}") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="品名" ItemStyle-HorizontalAlign="center" ItemStyle-Width="3%" SortExpression="product_id">
                                <ItemTemplate>
                                    <asp:Label ID="product_id" runat="server" Text='<%# Eval("product_id") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="數量" ItemStyle-HorizontalAlign="center" ItemStyle-Width="1%" SortExpression="number">
                                <ItemTemplate>
                                    <asp:Label ID="number" runat="server" Text='<%# Eval("number") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="單價" ItemStyle-HorizontalAlign="center" ItemStyle-Width="1%" SortExpression="price">
                                <ItemTemplate>
                                    <asp:Label ID="price" runat="server" Text='<%# Eval("price") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="合計" ItemStyle-HorizontalAlign="center" ItemStyle-Width="1%" SortExpression="total">
                                <ItemTemplate>
                                    <asp:Label ID="total" runat="server" Text='<%# Eval("total") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="核銷日期" ItemStyle-HorizontalAlign="center" ItemStyle-Width="3%" SortExpression="pass_date">
                                <ItemTemplate>
                                    <asp:Label ID="pass_date" runat="server" Text='<%# Eval("pass_date","{0:yyyy/MM/dd}") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="預算科目" ItemStyle-HorizontalAlign="center" ItemStyle-Width="5%" SortExpression="Budget_id">
                                <ItemTemplate>
                                    <asp:Label ID="Budget_id" runat="server" Text='<%# Eval("Budget_id") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="狀態" ItemStyle-HorizontalAlign="center" ItemStyle-Width="1%">
                                <ItemTemplate>
                                    <asp:Label ID="state" runat="server" Text='<%# Eval("state") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="2%">
                                <HeaderTemplate>
                                    <span>功能</span>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Button runat="server" CssClass="gw_bt" ID="modify" CommandName="modify" CommandArgument='<%# Eval("id") %>' Text="編輯" Style="background-color: #68B100;margin:1.5px 0;" />
                                    <asp:Button runat="server" CssClass="gw_bt" ID="del" CommandName="del" CommandArgument='<%# Eval("id") %>' Text="刪除" Style="background-color: #B83F1F;margin:1.5px 0;" OnClientClick="return confirm('確定要刪除嗎？');" />
                                    <asp:Label ID="nolimit" runat="server" Text="無權限" Visible="false" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="searchbt" EventName="" />
                </Triggers>
            </asp:UpdatePanel>
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

