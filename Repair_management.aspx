<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage2.master" AutoEventWireup="true" CodeFile="Repair_management.aspx.cs" Inherits="Repair_management" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <style>
        .container {
            display: flex;
        }

        .write_Box {
            margin-right: 2rem;
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

        @media(max-width:1360px) {
            .container {
                margin-right: 0px;
                margin-left: 100px;
                display: flex;
                flex-direction: column;
            }

            #floor_sec {
                width: 72% !important;
            }

            #floor_third {
                width: 72% !important;
            }
        }

        .bt {
            padding: 10px 30px;
            background: #4CAF1E 0% 0% no-repeat padding-box;
            border: 0px;
            border-radius: 5px;
            opacity: 1;
            text-align: center;
            color: #FFFFFF;
            margin-right: 10px;
            padding: 10px 30px;
        }

        .porductbt {
            padding: 8px 30px;
            border-radius: 5px;
            color: white;
            width: auto;
            height: auto;
        }

        .markText {
            position: absolute;
            top: 169px;
            z-index: 999;
        }
    </style>
    <script>
        function hideModal2() {
            $('#modal2').modal('hide');

            return false;
        }
        function hideModal3() {
            $('#modal3').modal('hide');

            return false;
        }
    </script>

   
    <div class="write_Box" style="width: 27%;">
        <div class="write_Title">
            <h5>
                <asp:Literal runat="server" ID="box_title">1.地點</asp:Literal></h5>
        </div>
        <div class="write_textBox">
            <%--            <div class="dataBox" style="background-color: #ECECEC; padding: 10px 30px; border-radius: 5px;">
                <div>
                    <span><b>新增地點</b></span>
                </div>
                <div>
                    <asp:TextBox ID="place" runat="server" placeholder="請輸入欲增加地點名稱" Style="width: 70%" />
                    <asp:Button ID="Addplace" CssClass="addbt" runat="server" Text="新增" OnClick="Addplace_Click" />
                </div>
            </div>--%>
            <div class="dataBox">
                <asp:GridView ID="p_gv" class="table table-bordered mt-2" runat="server" AutoGenerateColumns="False" OnRowCommand="p_gv_RowCommand" PageSize="10" AllowPaging="true" OnPageIndexChanging="p_gv_PageIndexChanging">
                    <EmptyDataTemplate>
                        無資料
                    </EmptyDataTemplate>
                    <HeaderStyle CssClass="table-topbar" />
                    <Columns>
                        <asp:BoundField DataField="place_id" HeaderText="place_id" ItemStyle-Width="1%" Visible="false" />
                        <asp:TemplateField HeaderText="地點" ItemStyle-HorizontalAlign="center" ItemStyle-Width="10%">
                            <ItemTemplate>
                                <asp:Label ID="place_name" runat="server" Style="color: #375471" Text='<%# Eval("place_name") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%--                        <asp:TemplateField ItemStyle-Width="2%">
                            <HeaderTemplate>
                                <span>功能</span>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Button runat="server" CssClass="del_btn" ID="del" CommandName="Remove" CommandArgument='<%# Eval("place_id") %>' Text="移除" Style="background-color: #B83F1F" OnClientClick="return confirm('確定要刪除嗎？');" />
                            </ItemTemplate>
                        </asp:TemplateField>--%>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>
    <div class="write_Box" style="width: 70%" id="floor_sec">
        <div class="write_Title">
            <h5>
                <asp:Literal runat="server" ID="Literal1">2.樓層</asp:Literal></h5>
        </div>
        <div class="write_textBox" style="display: flex;">
            <div style="margin-right: 30px;">
                <span style="font-size: 18px; color: #375471">行政大樓</span>
                <div class="dataBox" style="background-color: #ECECEC; padding: 10px 20px; border-radius: 5px;">
                    <div>
                        <span><b>新增樓層</b></span>
                    </div>
                    <div style="display: flex;">
                        <asp:TextBox ID="floor" runat="server" placeholder="最多可輸入20個字" Style="width: 70%; margin-right: 5px;" />
                        <asp:Button ID="addfloor1" CssClass="addbt" runat="server" Text="新增" OnClick="Addfloor1_Click" />
                    </div>
                </div>
                <div class="dataBox">
                    <asp:GridView ID="f1_gv" class="table table-bordered mt-2" runat="server" AutoGenerateColumns="False" PageSize="30" AllowPaging="true" OnPageIndexChanging="f1_gv_PageIndexChanging" PagerSettings-Visible="false">
                        <EmptyDataTemplate>
                            無資料
                        </EmptyDataTemplate>
                        <HeaderStyle CssClass="table-topbar" />
                        <Columns>
                            <asp:BoundField DataField="id" HeaderText="id" ItemStyle-Width="1%" Visible="false" />
                            <asp:TemplateField HeaderText="樓層" ItemStyle-HorizontalAlign="center" ItemStyle-Width="10%">
                                <ItemTemplate>
                                    <asp:Label ID="floor_name" runat="server" Text='<%# Eval("floor_name") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="2%">
                                <HeaderTemplate>
                                    <span>功能</span>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Button runat="server" CssClass="del_btn" ID="del" CommandName='<%# Eval("floor_name") %>' CommandArgument='<%# Eval("id") %>' Text="移除" Style="background-color: #B83F1F" OnClick="modal2_alert_del_Click" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
            <div>
                <span style="font-size: 18px; color: #375471">工商大樓</span>
                <div class="dataBox" style="background-color: #ECECEC; padding: 10px 20px; border-radius: 5px;">
                    <div>
                        <span><b>新增樓層</b></span>
                    </div>
                    <div style="display: flex;">
                        <asp:TextBox ID="floor2" runat="server" placeholder="最多可輸入20個字" Style="width: 70%; margin-right: 5px;" />
                        <asp:Button ID="addfloor2" CssClass="addbt" runat="server" Text="新增" OnClick="Addfloor2_Click" />
                    </div>
                </div>
                <div class="dataBox">
                    <asp:GridView ID="f2_gv" class="table table-bordered mt-2" runat="server" AutoGenerateColumns="False" PageSize="30" AllowPaging="true" OnPageIndexChanging="f2_gv_PageIndexChanging" PagerSettings-Visible="false">
                        <EmptyDataTemplate>
                            無資料
                        </EmptyDataTemplate>
                        <HeaderStyle CssClass="table-topbar" />
                        <Columns>
                            <asp:BoundField DataField="id" HeaderText="id" ItemStyle-Width="1%" Visible="false" />
                            <asp:TemplateField HeaderText="樓層" ItemStyle-HorizontalAlign="center" ItemStyle-Width="10%">
                                <ItemTemplate>
                                    <asp:Label ID="floor_name" runat="server" Text='<%# Eval("floor_name") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="2%">
                                <HeaderTemplate>
                                    <span>功能</span>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Button runat="server" CssClass="del_btn" ID="del" CommandName='<%# Eval("floor_name") %>' CommandArgument='<%# Eval("id") %>' Text="移除" Style="background-color: #B83F1F" OnClick="modal2_alert_del_Click" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>

        </div>
    </div>
    <div class="write_Box" style="width: 35%; margin-right: 0px;" id="floor_third">
        <div class="write_Title">
            <h5>
                <asp:Literal runat="server" ID="Literal2">3.位置</asp:Literal></h5>
        </div>
        <div class="write_textBox">
            <div class="dataBox" style="background-color: #ECECEC; padding: 10px 30px; border-radius: 5px;">
                <div>
                    <span><b>新增位置</b></span>
                </div>
                <div style="display: flex;">
                    <asp:TextBox ID="location" runat="server" placeholder="最多可輸入20個字" Style="width: 70%; margin-right: 5px;" />
                    <asp:Button ID="addlocation" CssClass="addbt" runat="server" Text="新增" OnClick="Addlocation_Click" />
                </div>
            </div>
            <div class="dataBox">
                <asp:GridView ID="l_gv" class="table table-bordered mt-2" runat="server" AutoGenerateColumns="False" PageSize="50" AllowPaging="true" OnPageIndexChanging="l_gv_PageIndexChanging">
                    <EmptyDataTemplate>
                        無資料
                    </EmptyDataTemplate>
                    <HeaderStyle CssClass="table-topbar" />
                    <Columns>
                        <asp:TemplateField HeaderText="序號" ItemStyle-HorizontalAlign="center" ItemStyle-Width="2%">
                            <ItemTemplate>
                                <asp:Label ID="serial" runat="server"
                                    Text='<%# (Container.DataItemIndex+1) + (l_gv.PageIndex)  %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="id" HeaderText="id" ItemStyle-Width="1%" Visible="false" />
                        <asp:TemplateField HeaderText="位置" ItemStyle-HorizontalAlign="center" ItemStyle-Width="10%">
                            <ItemTemplate>
                                <asp:Label ID="Location_name" runat="server" Text='<%# Eval("Location_name") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-Width="2%">
                            <HeaderTemplate>
                                <span>功能</span>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Button runat="server" CssClass="del_btn" ID="del" CommandName='<%# Eval("Location_name") %>' CommandArgument='<%# Eval("id") %>' Text="移除" Style="background-color: #B83F1F" OnClick="modal3_alert_del_Click" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
    <%--格式錯誤的modal--%>
    <div class="modal" id="ErrorFormatShow_alert" tabindex="-1" role="dialog" aria-labelledby="ModalTitle" aria-hidden="true" style="backdrop-filter: brightness(0.3);">
        <div class="modal-dialog modal-lg modal-dialog-centered" style="max-width: 500px;">
            <div class="modal-content">
                <div class="modal-header write_Title" style="background-color: #C5ECFF; font-size: 22px; display: flex; justify-content: center; align-items: center; position: relative;">
                    <h5 class="modal-title" runat="server" id="H2" style="font-size: 20px; font-weight: bold; color: #333333;">
                        <asp:Label ID="ErrorFormatTitle" runat="server" Style="color: #003168;">格式錯誤</asp:Label>
                    </h5>
                    <button type="button" class="close" data-dismiss="modal" style="position: absolute; right: 2%">
                        <image src="image/popup_close.png"></image>
                    </button>
                </div>
                <div class="modal-body" style="padding: 50px 0px 35px 0px;">
                    <div style="font-size: 20px; font-weight: bold; color: #333333; display: flex; justify-content: center;">
                        <asp:Label ID="ErrorFormatContent" runat="server">確定要刪除嗎?</asp:Label>
                    </div>
                </div>
                <div>
                    <hr />
                </div>
                <div class="mb-3" style="display: flex; justify-content: center;">
                    <asp:Button runat="server" CssClass="bt" Text="取消" Style="background: #777777" data-dismiss="modal" />
                    <asp:Button runat="server" CssClass="bt" Text="確定" data-dismiss="modal" />
                </div>
            </div>
        </div>
    </div>
    <%--格式錯誤的modal--%>
    <%-- modal2 start--%>
    <div class="modal fade bs-NewUser-modal-lg" id="modal2" tabindex="-1" role="dialog" aria-labelledby="myLargeModalLabel" aria-hidden="true" style="backdrop-filter: brightness(0.3);">
        <div class="modal-dialog modal-lg modal-dialog-centered" style="max-width: 50%;">
            <div class="modal-content">
                <div class="modal-header" style="background-color: #C5ECFF; font-size: 22px; display: flex; justify-content: center; align-items: center; position: relative;">
                    <b><span style="color: #003168">刪除確認</span></b>
                    <button type="button" class="close" data-dismiss="modal" style="position: absolute; right: 2%">
                        <image src="image/popup_close.png"></image>
                    </button>
                </div>
                <div class="modal-body">
                    <div class="MainClass">
                        <div id="content2" role="form" class="form-horizontal text-center">
                            <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                <ContentTemplate>
                                    <div class="d-flex justify-content-center">
                                        <table border="1" class="table table-bordered">
                                            <tr class="table-topbar">
                                                <th>大樓</th>
                                                <th>樓層</th>
                                                <th>參照數</th>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="modal2_building" runat="server" /></td>
                                                <td>
                                                    <asp:Label ID="modal2_floor" runat="server" /></td>
                                                <td>
                                                    <asp:Label ID="modal2_num" runat="server" /></td>
                                            </tr>
                                        </table>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <div class="align-items-center align-content-center">
                                <span style="color: #B83F1F; font-size: 1.25rem">*刪除後，下層資料與現有資料可能會遺失；有參照數時，請清除後再刪除資料</span>
                            </div>
                            <hr />
                            <asp:Button Text="返回" runat="server" CssClass="btn porductbt" Style="background-color: #777777" OnClientClick="return hideModal2()" />
                            <asp:Button Text="刪除" ID="modal2_del" runat="server" CssClass="btn porductbt" Style="background-color: #B83F1F" OnClick="modal2_del_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <%-- modal2 end--%>

    <%-- modal3 start--%>
    <div class="modal fade bs-NewUser-modal-lg" id="modal3" tabindex="-1" role="dialog" aria-labelledby="myLargeModalLabel" aria-hidden="true" style="backdrop-filter: brightness(0.3);">
        <div class="modal-dialog modal-lg modal-dialog-centered" style="max-width: 50%;">
            <div class="modal-content">
                <div class="modal-header" style="background-color: #C5ECFF; font-size: 22px; display: flex; justify-content: center; align-items: center; position: relative;">
                    <b><span style="color: #003168">刪除確認</span></b>
                    <button type="button" class="close" data-dismiss="modal" style="position: absolute; right: 2%">
                        <image src="image/popup_close.png"></image>
                    </button>
                </div>
                <div class="modal-body">
                    <div class="MainClass">
                        <div id="content3" role="form" class="form-horizontal text-center" style="margin-top: 25px; margin-bottom: 25px;">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <div class="d-flex justify-content-center">
                                        <table border="1" class="table table-bordered">
                                            <tr class="table-topbar">
                                                <th>位置</th>
                                                <th>參照數</th>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="modal3_location" runat="server" /></td>
                                                <td>
                                                    <asp:Label ID="modal3_num" runat="server" /></td>
                                            </tr>
                                        </table>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <div class="align-items-center align-content-center">
                                <span style="color: #B83F1F; font-size: 1.25rem">*刪除後，下層資料與現有資料可能會遺失；有參照數時，請清除後再刪除資料</span>
                            </div>
                            <hr />
                            <asp:Button Text="返回" runat="server" CssClass="btn porductbt" Style="background-color: #777777" OnClientClick="return hideModal3()" />
                            <asp:Button Text="刪除" ID="modal3_del" runat="server" CssClass="btn porductbt" Style="background-color: #B83F1F" OnClick="modal3_del_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <%-- modal3 end--%>
</asp:Content>

