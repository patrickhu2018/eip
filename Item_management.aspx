<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Item_management.aspx.cs" Inherits="Item_management" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager runat="server"></asp:ScriptManager>
    <style>
        .container {
            display: flex;
        }

        .write_Box {
            margin-right: 2rem;
            width: 35%;
            border-radius: 15px;
        }

        .addbt {
            width: 56px;
            height: 31px;
            border: 0px solid #ECECEC;
            background: #F7931E 0% 0% no-repeat padding-box;
            border-radius: 5px;
            opacity: 1;
            text-align: center;
            font: normal normal normal 16px/12px Microsoft JhengHei;
            letter-spacing: 0px;
            color: #FFFFFF;
            flex-basis: auto;
        }

        .del_btn {
            background-image: url(image/icon_17.png);
            background-repeat: no-repeat;
            background-position-y: 50%;
            background-position-x: 9%;
            padding-left: 26px;
            color: #FFFFFF;
            border-radius: 3px;
            opacity: 1;
            border: 0px;
        }

        @media (min-width: 768px) and (max-width: 1360px) {
            .write_Box {
                width: 40%;
            }
        }
    </style>
    <div class="d-flex"></div>
    <div class="write_Box">
        <div class="write_Title">
            <h5>
                <asp:Literal runat="server" ID="box_title">1.品名管理</asp:Literal></h5>
        </div>
        <div class="write_textBox">
            <div class="dataBox" style="background-color: #ECECEC; padding: 10px 30px; border-radius: 5px; width: 100%">
                <div style="width: 100%">
                    <span><b>新增品名</b></span>
                </div>
                <div style="width: 90%; display: flex;">
                    <asp:TextBox ID="Product" runat="server" placeholder="請輸入品名"
                        Style="width: 80%; box-sizing: border-box; margin-right: 10px;" />
                    <asp:Button ID="AddProduct" CssClass="addbt" runat="server" Text="新增" OnClick="AddProduct_Click" />
                </div>
            </div>
            <div class="dataBox">
                <asp:GridView ID="p_gv" class="table table-bordered mt-2" runat="server" AutoGenerateColumns="False" OnRowCommand="p_gv_RowCommand" PageSize="10" AllowPaging="true" OnPageIndexChanging="p_gv_PageIndexChanging" PagerSettings-Visible="false">
                    <EmptyDataTemplate>
                        無資料
                    </EmptyDataTemplate>
                    <HeaderStyle CssClass="table-topbar" />
                    <Columns>
                        <asp:TemplateField HeaderText="序號" ItemStyle-HorizontalAlign="center" ItemStyle-Width="1%">
                            <ItemTemplate>
                                <asp:Label ID="serial" runat="server"
                                    Text='<%# p_gv.Rows.Count + (p_gv.PageIndex * p_gv.PageSize) + 1 %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="id" HeaderText="id" ItemStyle-Width="1%" Visible="false" />
                        <asp:TemplateField HeaderText="品項" ItemStyle-HorizontalAlign="center" ItemStyle-Width="10%">
                            <ItemTemplate>
                                <asp:Label ID="name" runat="server" Text='<%# Eval("name") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-Width="2%">
                            <HeaderTemplate>
                                <span>功能</span>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Button runat="server" CssClass="del_btn" ID="del" CommandName="Remove" CommandArgument='<%# Eval("id") %>' Text="移除" Style="background-color: #B83F1F" OnClientClick="return confirm('確定要刪除嗎？');" />
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



                        <div class="NumberOfPages" style="padding: 5px; float: left">
                            <asp:LinkButton ID="lkbPagePrev" runat="server" OnClick="lkbPagePrev_Click" Style="color: #0071BC; text-decoration: underline;">上一頁</asp:LinkButton>
                            <asp:LinkButton ID="lkbPageNext" runat="server" OnClick="lkbPageNext_Click" Style="color: #0071BC; text-decoration: underline;">下一頁</asp:LinkButton>
                        </div>



                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="write_Box" style="flex-wrap: wrap">
        <div class="write_Title">
            <h5>
                <asp:Literal runat="server" ID="Literal1">2.預算科目管理</asp:Literal></h5>
        </div>
        <div class="write_textBox">
            <div class="dataBox" style="background-color: #ECECEC; padding: 10px 30px; border-radius: 5px; width: 100%">
                <div>
                    <span><b>新增預算科目</b></span>
                </div>
                <div style="display: flex;">
                    <asp:TextBox ID="Budget" runat="server" placeholder="請輸入預算科目" Style="width: 80%; margin-right: 10px; box-sizing: border-box;" />
                    <asp:Button ID="AddBudget" CssClass="addbt" runat="server" Text="新增" OnClick="AddBudget_Click" />
                </div>
            </div>
            <div class="dataBox">
                <asp:GridView ID="b_gv" class="table table-bordered mt-2" runat="server" AutoGenerateColumns="False" OnRowCommand="b_gv_RowCommand" PageSize="10" AllowPaging="true" OnPageIndexChanging="b_gv_PageIndexChanging" PagerSettings-Visible="false">
                    <EmptyDataTemplate>
                        無資料
                    </EmptyDataTemplate>
                    <HeaderStyle CssClass="table-topbar" />
                    <Columns>
                        <asp:TemplateField HeaderText="序號" ItemStyle-HorizontalAlign="center" ItemStyle-Width="1%">
                            <ItemTemplate>
                                <asp:Label ID="serial" runat="server"
                                    Text='<%#  b_gv.Rows.Count + (b_gv.PageIndex * b_gv.PageSize) + 1 %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="id" HeaderText="id" ItemStyle-Width="1%" Visible="false" />
                        <asp:TemplateField HeaderText="品項" ItemStyle-HorizontalAlign="center" ItemStyle-Width="10%">
                            <ItemTemplate>
                                <asp:Label ID="name" runat="server" Text='<%# Eval("name") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-Width="2%">
                            <HeaderTemplate>
                                <span>功能</span>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Button runat="server" CssClass="del_btn" ID="del" CommandName="Remove" CommandArgument='<%# Eval("id") %>' Text="移除" Style="background-color: #B83F1F" OnClientClick="return confirm('確定要刪除嗎？');" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <div id="down2" runat="server" class="btn-toolbar" role="toolbar" aria-label="Toolbar with button groups" style="justify-content: right;">
                    <div class="btn-group pull-right" role="group" aria-label="First group" style="align-items: center;">

                        <div class="NumberOfPages" style="padding: 5px; float: left">
                            第<asp:Label ID="lblPageIndex2" runat="server" Text="0" CssClass="Number" Style="color: #0071BC"></asp:Label>頁    
                        </div>

                        <div class="NumberOfPages" style="padding: 5px; float: left">
                            總計<asp:Label ID="lblDataCount2" runat="server" Text="0" CssClass="Number" Style="color: #0071BC"></asp:Label>筆
                        </div>



                        <div class="NumberOfPages" style="padding: 5px; float: left">
                            <asp:LinkButton ID="lkbPagePrev2" runat="server" OnClick="lkbPagePrev2_Click" Style="color: #0071BC; text-decoration: underline;">上一頁</asp:LinkButton>
                            <asp:LinkButton ID="lkbPageNext2" runat="server" OnClick="lkbPageNext2_Click" Style="color: #0071BC; text-decoration: underline;">下一頁</asp:LinkButton>
                        </div>



                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
</asp:Content>

