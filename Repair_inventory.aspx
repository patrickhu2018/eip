<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage2.master" AutoEventWireup="true" CodeFile="Repair_inventory.aspx.cs" Inherits="Repair_inventory" MaintainScrollPositionOnPostback="true" %>

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
        function showModal2() {
            $('#modal2').modal('show');
            return false;
        }
        function hideModal2() {
            $('#modal2').modal('hide');
            return false;
        }
        function show_alert_modal() {
            $('#alert_modal').modal('show');
            return false;
        }

        function hide_alert_modal() {
            $('#alert_modal').modal('hide');
            return false;
        }


        function showError(message) {
            // 顯示錯誤訊息
            var errorMessageElement = document.getElementById('error-message');
            errorMessageElement.innerHTML = '<strong>錯誤:</strong> ' + message;
            errorMessageElement.style.display = 'block';

            // 保證 modal 保持開啟
            $('#modal1').modal('show');
        }
        document.addEventListener('DOMContentLoaded', function () {
            var uploadButton = document.getElementById('<%= upbt1.ClientID %>');
            if (uploadButton) {
                console.log("1");
                uploadButton.onclick = function () {
                    var fileUpload = document.getElementById('<%= FileUpload1.ClientID %>');
                    if (fileUpload) {
                        fileUpload.click();
                    }
                };
            }
        });
        document.addEventListener('DOMContentLoaded', function () {
            var uploadButton = document.getElementById('<%= upbt2.ClientID %>');
            if (uploadButton) {
                console.log("2");
                uploadButton.onclick = function () {
                    var fileUpload = document.getElementById('<%= FileUpload2.ClientID %>');
                    if (fileUpload) {
                        fileUpload.click();
                    }
                };
            }
        });
        function showPreview(fileUploadControl, previewId) {

            var previewElement = (previewId == 1) ? document.getElementById('<%= preview1.ClientID %>') : document.getElementById('<%= preview2.ClientID %>');
            if (fileUploadControl.files && fileUploadControl.files[0]) {
                var reader = new FileReader();

                reader.onload = function (e) {
                    previewElement.src = e.target.result;
                    previewElement.style.display = 'block';

                };

                reader.readAsDataURL(fileUploadControl.files[0]);
            }

        }
    </script>
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

        .addbtn {
            width: 119px;
            height: 33px;
            background: #DE7525 0% 0% no-repeat padding-box;
            border-radius: 3px;
            border: 0px;
            color: #FFFFFF;
            opacity: 1;
            background-image: url(../image/icon_16.png);
            background-position: left center;
            padding-left: 20px;
        }

        .exportbt {
            width: 119px;
            height: 33px;
            background: #5C9C00 0% 0% no-repeat padding-box;
            border-radius: 3px;
            border: 0px;
            color: #FFFFFF;
            opacity: 1;
            background-image: url(../image/icon_13.png);
            background-position: left center;
            padding-left: 20px;
        }

        .modal-backdrop {
            display: none;
        }


        .upload-container {
            position: relative;
            width: calc(var(--diameter)* 3 + var(--tipset)* 2.6);
            max-width: calc(var(--diameter)* 3 + var(--tipset)* 2.6);
            min-width: calc(var(--diameter)* 3 + var(--tipset)* 2.6);
            height: calc(var(--tipset)* 2.6 + var(--tipset)* 2.6);
            max-height: calc(var(--tipset)* 2.6 + var(--tipset)* 2.6);
            min-height: calc(var(--tipset)* 2.6 + var(--tipset)* 2.6);
            border: 1px solid #ccc;
            display: flex;
            align-items: center;
            justify-content: center;
            flex-direction: column;
            background-color: #f7f7f7;
        }

        .image-preview {
            width: auto;
            max-width: 100%;
            height: auto;
            max-height: 100%;
            object-fit: cover;
            border-radius: 5px;
        }

        .img-fluid {
            width: 50%;
            height: 50%;
            object-fit: cover;
            border-radius: 5px;
        }

        .upload-button {
            background-color: #00bfa5;
            color: white;
            padding: 5px 10px;
            border: 0px;
            border-radius: 5px;
            cursor: pointer;
            text-align: center;
            display: inline-block;
            margin-top: 10px;
        }

        .containertitle {
            height: 68px;
            background: #F5F5F5 0% 0% no-repeat padding-box;
            border: 1px solid #B9B9B9;
            border-radius: 10px;
            text-align: center;
            color: #000470;
            display: flex;
            align-self: center;
            font-size: 20px;
            font: normal normal bold 20px/24px Microsoft JhengHei;
            opacity: 1;
        }

        .porductbt {
            padding: 8px 30px;
            border-radius: 5px;
            color: white;
            width: auto;
            height: auto;
        }

        .table th {
            padding: 0; /* 設置標頭的 padding */
        }

        @media (min-width: 768px) and (max-width: 1745px) {
            #insert_modal {
                max-width: 75% !important;
            }
        }

        @media (max-width: 1587px) {
            .write_TitleMark {
                margin-bottom: 10px;
            }
        }

        .write_TitleMark {
            display: flex !important;
            justify-content: space-between !important;
        }
    </style>
    <div class="write_Box">
        <div class="write_Title write_TitleMark">
            <h5>
                <asp:Literal runat="server" ID="box_title">清單篩選</asp:Literal></h5>
            <span class="subTitleMarkText">
                <span>註：1. 物料代碼/名稱中，無資料時，可按[新增物料]，增加物料資料</span>
                <span class="leftDistance">2. 物料代碼/名稱中，有資料時，按[查詢]，待由物品庫存表中，帶出資料，按[新增]新增庫存量</span>
            </span>
        </div>
        <div class="write_textBox">
            <div class="dataBox row">
                <div class="col-3 formItem title6">
                    <div class="ItemTitle"><span>物料代碼/名稱</span></div>
                    <div class="ItemContent">
                        <asp:DropDownList ID="materials" runat="server" CssClass="form-control" Style="width: 180px;">
                        </asp:DropDownList>
                    </div>
                </div>
            </div>
        </div>
        <div class="write_textBox d-flex justify-content-center">
            <asp:Button ID="searchbt" runat="server" Text="查詢" CssClass="searchbt" OnClick="searchbt_Click" />
        </div>
    </div>
    <div class="write_Box">
        <div class="write_Title d-flex justify-content-between">
            <h5>
                <asp:Literal runat="server" ID="Literal1">物品庫存表</asp:Literal></h5>
            <div>
                <asp:Button ID="add" runat="server" Text="新增物料" CssClass="addbtn" OnClick="add_Click" />
                <asp:Button ID="export" runat="server" Text="匯出總表" CssClass="exportbt" OnClick="export_Click" />
            </div>

        </div>
        <div class="write_textBox">
            <asp:GridView ID="gv" runat="server" CssClass="table table-bordered mt-2" AutoGenerateColumns="False" OnRowCommand="gv_RowCommand"
                OnRowDataBound="gv_RowDataBound" OnPageIndexChanging="gv_PageIndexChanging" OnRowCreated="gv_RowCreated"
                OnSorting="gv_Sorting" PageSize="10" AllowPaging="true" AllowSorting="True" DataKeyNames="id" PagerSettings-Visible="false">
                <EmptyDataTemplate>無資料</EmptyDataTemplate>
                <HeaderStyle CssClass="table-topbar" />
                <Columns>
                    <asp:TemplateField HeaderText="id" ItemStyle-HorizontalAlign="center" ItemStyle-Width="1%" SortExpression="id" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="id" runat="server" Text='<%# Eval("id") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText='物料代碼<span class="fa fa-caret-down"/>' ItemStyle-HorizontalAlign="center" ItemStyle-Width="1%" SortExpression="materials_no">
                        <ItemTemplate>
                            <asp:Label ID="materials_no" runat="server" Text='<%# Eval("materials_no") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText='物料名稱<span class="fa fa-caret-down"/>' ItemStyle-HorizontalAlign="center" ItemStyle-Width="1%" SortExpression="materials_name">
                        <ItemTemplate>
                            <asp:Label ID="materials_name" runat="server" Text='<%# Eval("materials_name") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText='規格<span class="fa fa-caret-down"/>' ItemStyle-HorizontalAlign="center" ItemStyle-Width="1%" SortExpression="specification">
                        <ItemTemplate>
                            <asp:Label ID="specification" runat="server" Text='<%# Eval("specification") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText='總庫存量<span class="fa fa-caret-down"/>' ItemStyle-HorizontalAlign="center" ItemStyle-Width="1%" SortExpression="total_inventory">
                        <ItemTemplate>
                            <asp:Label ID="total_inventory" runat="server" Text='<%# Eval("total_inventory") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText='剩餘庫存量<span class="fa fa-caret-down"/>' ItemStyle-HorizontalAlign="center" ItemStyle-Width="2%">
                        <HeaderTemplate>
                            <table style="width: 100%; border: 2px solid rgba(0, 0, 0, 0); padding: 0;">
                                <tr>
                                    <th colspan="2" style="text-align: center;">剩餘庫存量</th>
                                </tr>
                                <tr>
                                    <th style="text-align: center; width: 50%;">行政大樓</th>
                                    <th style="text-align: center; width: 50%;">工商大樓</th>
                                </tr>
                            </table>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <table style="width: 100%; border-collapse: collapse; height: 100%">
                                <tr style="border: 2px solid rgba(0, 0, 0, 0)">
                                    <td style="text-align: center; width: 50%; padding: 0; height: 100%">
                                        <%# Eval("place1_inventory") %>
                                   </td>
                                    <td style="text-align: center; width: 50%; padding: 0; height: 100%">
                                        <%# Eval("place2_inventory") %>
                                   </td>
                                </tr>
                            </table>
                        </ItemTemplate>
                    </asp:TemplateField>


                    <asp:TemplateField HeaderText='安全庫存量<span class="fa fa-caret-down"/>' ItemStyle-HorizontalAlign="center" ItemStyle-Width="1%" SortExpression="safe_inventory">
                        <ItemTemplate>
                            <asp:Label ID="safe_inventory" runat="server" Text='<%# Eval("safe_inventory") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="filepath1" ItemStyle-HorizontalAlign="center" ItemStyle-Width="1%" SortExpression="safe_inventory" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="filepath1" runat="server" Text='<%# Eval("filepath1") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="filepath2" ItemStyle-HorizontalAlign="center" ItemStyle-Width="1%" SortExpression="safe_inventory" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="filepath2" runat="server" Text='<%# Eval("filepath2") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="物料圖檔" ItemStyle-HorizontalAlign="center" ItemStyle-Width="1%">
                        <ItemTemplate>
                            <%-- <asp:ImageButton ID="imgbt" runat="server" CommandName="ShowImages" CommandArgument='<%# Eval("id") %>' ImageUrl="image/icon_15.png" />--%>
                            <asp:Button ID="imgbt" CssClass="gw_bt" runat="server" Text="預覽" CommandArgument='<%# Eval("id") %>' CommandName="ShowImages" Style="background-color: #1885C5" />
                            <asp:HiddenField ID="file1" runat="server" Value='<%# Eval("filepath1") %>' />
                            <asp:HiddenField ID="file2" runat="server" Value='<%# Eval("filepath2") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>


                    <asp:TemplateField HeaderText='最後編修時間<span class="fa fa-caret-down"/>' ItemStyle-HorizontalAlign="center" ItemStyle-Width="1%" SortExpression="updateDate">
                        <ItemTemplate>
                            <asp:Label ID="updateDate" runat="server" Text='<%#Eval("updateDate","{0:yyyy/MM/dd}") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-Width="1%">
                        <HeaderTemplate><span>功能</span></HeaderTemplate>
                        <ItemTemplate>
                            <asp:Button runat="server" CssClass="gw_bt" ID="add" CommandName="add" CommandArgument='<%# Eval("id") %>' Text="新增" Style="background-color: #68B100" />
                            <asp:Button runat="server" CssClass="gw_bt" ID="del" CommandName="del" CommandArgument='<%# Eval("id") %>' Text="刪除" Style="background-color: #B83F1F" OnClientClick="return confirm('確定要刪除嗎？');" />
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
    <%--<asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>--%>
    <div class="modal fade bs-NewUser-modal-lg " id="modal1" tabindex="-1" role="dialog" aria-labelledby="myLargeModalLabel" aria-hidden="true" style="backdrop-filter: brightness(0.3);">
        <div class="modal-dialog modal-lg modal-dialog-centered" id="insert_modal" style="max-width: 50%;">
            <div class="modal-content">
                <div class="modal-header" style="background-color: #C5ECFF; font-size: 22px; display: flex; justify-content: center; align-items: center; position: relative;">
                    <b><span id="modtitle" runat="server" style="color: #003168">新增物料</span></b>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close" style="position: absolute; right: 10px;">
                        <image src="image/popup_close.png"></image>
                    </button>
                </div>
                <div class="modal-body">
                    <div class="MainClass">
                        <div id="content" role="form" class="form-horizontal">
                            <div class="Box" style="padding: 0px 40px;">
                                <div class="row" style="margin-bottom: 15px;">
                                    <div class="col-8 d-flex align-items-center">物料代碼<asp:TextBox ID="m_no" runat="server" CssClass="form-control" Style="width: 30%; margin-left: 10px;" oninput="this.value=this.value.replace(/[^0-9]/g, '')" placeholder="代碼僅限輸入數字" /></div>
                                </div>
                                <div class="row" style="margin-bottom: 15px;">
                                    <div class="col-8 d-flex align-items-center">物料名稱<asp:TextBox ID="m_name" runat="server" CssClass="form-control" Style="width: 60%; margin-left: 10px;" /></div>
                                </div>
                                <hr>
                            </div>

                        </div>
                        <div class="Box" style="padding: 0px 40px;">
                            <div class="row" style="margin-bottom: 15px;">
                                <div class="col-6 d-flex align-items-center">規格<asp:TextBox ID="specification" runat="server" CssClass="form-control" Style="width: 30%; margin-left: 30px;" /></div>
                            </div>
                            <div class="row" style="margin-bottom: 15px;">
                                <div class="col-8">照片上傳<span style="font-size: 14px; color: #707070">(限傳2張照片，格式限制jpg、png，檔案大小限制10mb以下)</span></div>
                            </div>
                            <div style="display: flex; gap: 20px; justify-content: center;">
                                <div class="upload-container mr-5" id="pic1" runat="server">
                                    <img id="preview1" src="image/image.png" alt="Image Preview" class="image-preview" runat="server" />
                                    <asp:FileUpload ID="FileUpload1" runat="server" OnChange="showPreview(this, '1')" Style="display: none;" />
                                    <button id="upbt1" runat="server" type="button" class="upload-button">選擇檔案</button>
                                </div>

                                <div class="upload-container ml-5" id="pic2" runat="server">
                                    <img id="preview2" src="image/image.png" alt="Image Preview" class="image-preview" runat="server" />
                                    <asp:FileUpload ID="FileUpload2" runat="server" OnChange="showPreview(this, '2')" Style="display: none;" />
                                    <button id="upbt2" runat="server" type="button" class="upload-button">選擇檔案</button>
                                </div>

                            </div>
                            <span id="Message" style="margin: 0; padding: 10px; width: 100%; display: block; font-size: 14px; background-color: #e7e7e7; color: #414141; text-align: center; border: 1px solid #cccccc" runat="server" visible="false">無上傳照片</span>
                            <hr>
                        </div>
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <div class="Box" style="padding: 0px 40px;">
                                    <div class="row" style="margin-bottom: 15px;">
                                        <div class="col-xl-4 col-md-6 d-flex align-items-center pr-0">安全庫存量<asp:TextBox ID="safe_count" runat="server" CssClass="form-control" Style="width: 26%; margin-left: 10px;" /></div>
                                    </div>
                                    <div class="row d-flex align-items-center" style="margin-bottom: 30px; position: relative;">
                                        <div class="col-xl-4 col-md-6 d-flex align-items-center pr-0 mb-3">
                                            <div class="d-flex align-items-center" style="white-space: nowrap">行政大樓量<asp:TextBox ID="p1_count" runat="server" CssClass="form-control" Style="width: 100%; margin-left: 10px;" AutoPostBack="true" OnTextChanged="p1_count_TextChanged" /></div>
                                            <div class="d-flex justify-content-center align-items-center ml-2">
                                                <span style="color: #0071BC;">＋</span>
                                            </div>
                                        </div>
                                        <div class="col-xl-4 col-md-6 d-flex align-items-center pr-0 pl-1 mb-3">
                                            <div class="d-flex align-items-center" style="white-space: nowrap">工商大樓量<asp:TextBox ID="p2_count" runat="server" CssClass="form-control" Style="width: 100%; margin-left: 10px;" AutoPostBack="true" OnTextChanged="p1_count_TextChanged" /></div>
                                            <div class="d-flex justify-content-center align-items-center ml-2">
                                                <span style="color: #0071BC;">＝</span>
                                            </div>
                                        </div>
                                        <div class="col-xl-3 col-md-6 d-flex align-items-right position-relative mb-3">
                                            <span style="white-space: nowrap">總庫存量</span>
                                            <asp:Label ID="total" runat="server" Text="20" Style="width: 40%; min-height: 100%; border-bottom: 1px solid #0071BC; display: flex; justify-content: end" />
                                            <span style="color: #0071BC; position: absolute; left: 6.8%; top: 97%; white-space: nowrap">自動計算：行政大樓+工商大樓量</span>
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <asp:Panel ID="addpl" runat="server" Visible="false">
                            <div class="Box" style="padding: 0px 40px;">
                                <hr>
                                <div class="row" style="margin-bottom: 15px;">
                                    <div class="col-6 d-flex align-items-center">增加量<asp:TextBox ID="addnumber" runat="server" CssClass="form-control" Style="width: 33%; margin-left: 10px;" Text="0" /></div>
                                </div>
                            </div>
                        </asp:Panel>
                        <hr>

                        <div style="text-align: center">
                            <asp:Button ID="cancel" runat="server" Text="取消" class="bt" Style="background: #777777"
                                OnClientClick="return hideModal1();" />
                            <asp:Button ID="submit" runat="server" Text="新增" class="bt" OnClick="submit_Click" />
                            <asp:Button ID="update" runat="server" Text="儲存" class="bt" Visible="false" OnClick="update_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <%-- 警告modal --%>
    <div class="modal fade bs-NewUser-modal-lg" id="alert_modal" tabindex="-1" role="dialog" aria-labelledby="myLargeModalLabel" style="backdrop-filter: brightness(0.3);" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered" role="document" style="border-radius: 5px; max-width: 20%">
            <div class="modal-content">
                <div class="modal-header write_Title" style="background-color: #C5ECFF; display: flex; justify-content: center;">
                    <h5 class="modal-title" style="font-size: 1.375rem; font-weight: bold; color: #003168;">注意</h5>
                    <button type="button" class="close" onclick="hide_alert_modal()" style="position: absolute; right: 2%">
                        <img src="image/popup_close.png">
                    </button>
                </div>
                <div class="modal-body" style="text-align: center; font-weight: bold;">
                    <asp:Label runat="server" ID="alert_text"></asp:Label>
                    <hr />
                    <div class="d-flex align-items-center justify-content-center" style="text-align: center;">
                        <button type="button" id="Cancel" class="bt" style="background-color: #777777" onclick="hide_alert_modal()">返回</button>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <%-- 警告modal --%>
    <%--</ContentTemplate>
    </asp:UpdatePanel>--%>
    <div class="modal fade bs-NewUser-modal-lg" id="imageModal" tabindex="-1" role="dialog" aria-labelledby="myLargeModalLabel" aria-hidden="true" style="backdrop-filter: brightness(0.3);">
        <div class="modal-dialog modal-lg modal-dialog-centered" style="max-width: 550px;">
            <div class="modal-content">
                <div class="modal-header" style="background-color: #C5ECFF; font-size: 22px; display: flex; justify-content: center; align-items: center; position: relative;">
                    <b><span id="Span1" runat="server" style="color: #003168">圖片預覽</span></b>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close" style="position: absolute; right: 10px;">
                        <image src="image/popup_close.png"></image>
                    </button>
                </div>
                <div class="modal-body d-flex" style="justify-content: center; font-size: 22px;">
                    <img id="modalImage1" src="" alt="Image 1" class="img-fluid" style="margin-right: 15px;" />
                    <img id="modalImage2" src="" alt="Image 2" class="img-fluid" />
                    <span id="modalMessage" style="margin: 0;">無上傳照片</span>
                </div>
                <div class="modal-footer justify-content-center">
                    <button type="button" class="btn porductbt" style="background-color: #777777" data-dismiss="modal">關閉</button>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

