<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="item_listquery.aspx.cs" Inherits="item_listquery" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
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
            background: #FF9900 0% 0% no-repeat padding-box;
            border-radius: 3px;
            border: 0px;
            opacity: 1;
            text-align: center;
            letter-spacing: 0px;
            color: #FFFFFF;
            width: 60px;
            height: 36px;
            padding: 1px 6px;
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

        .ItemContent label {
            margin-bottom: 0;
            margin-right: 10px;
        }

        .ItemContent input[type=radio] {
            margin-right: 5px;
        }

        @media (min-width: 768px) and (max-width: 1745px) {
            .dll {
                flex: 0 0 100% !important;
            }
        }

        .modal-backdrop {
            z-index: 0;
            background-color: rgba(0, 0, 0, 1); /* 調整透明度 */
        }

        .GridViewTitleText {
            font-size: 18pt;
            font-weight: bold;
            text-align: left;
        }

        @media (max-width: 1024px) {
            .write_TitleMark {
                margin-bottom: 10px;
            }
        }
    </style>
    <script>
        function showusermd() {
            $('#usermd').modal('show');
            return false;
        }
        function hideusermd() {
            $('#usermd').modal('hide');
            return false;
        }
    </script>
    <script type="text/javascript">

        function setEndDateRange() {
            var startDate = document.getElementById('<%= apply_start.ClientID %>').value;

            if (startDate) {
                // 計算日期格式：年月日 (yyyy-mm-dd)
                var startDateObj = new Date(startDate);
                startDateObj.setMonth(startDateObj.getMonth() + 6);  // 計算六個月後的日期
                // 格式化為 yyyy-mm-dd 格式
                var year = startDateObj.getFullYear();
                var month = ("0" + (startDateObj.getMonth() + 1)).slice(-2);  // 1-12月轉為兩位數
                var day = ("0" + startDateObj.getDate()).slice(-2);  // 日期轉為兩位數
                var newEndDate = year + "-" + month + "-" + day;  // 新的六個月後的日期

                document.getElementById('<%= apply_end.ClientID %>').setAttribute('max', newEndDate);
                document.getElementById('<%= apply_end.ClientID %>').setAttribute('min', startDate);
            }
        }

        function setEndDateRange2() {
            var passDate = document.getElementById('<%= pass_start.ClientID %>').value;

            if (passDate) {
                // 計算日期格式：年月日 (yyyy-mm-dd)
                var passDateObj = new Date(passDate);  // 更正這裡使用 passDate
                passDateObj.setMonth(passDateObj.getMonth() + 6);  // 計算六個月後的日期
                // 格式化為 yyyy-mm-dd 格式
                var year = passDateObj.getFullYear();
                var month = ("0" + (passDateObj.getMonth() + 1)).slice(-2);  // 1-12月轉為兩位數
                var day = ("0" + passDateObj.getDate()).slice(-2);  // 日期轉為兩位數
                var newEndDate = year + "-" + month + "-" + day;  // 新的六個月後的日期

                document.getElementById('<%= pass_end.ClientID %>').setAttribute('max', newEndDate);
                document.getElementById('<%= pass_end.ClientID %>').setAttribute('min', passDate);
            }
        }

        function setStartDateRange() {
            var startDate = document.getElementById('<%= apply_end.ClientID %>').value;
            if (startDate) {
                // 計算日期格式：年月日 (yyyy-mm-dd)
                var startDateObj = new Date(startDate);
                startDateObj.setMonth(startDateObj.getMonth() - 6);  // 計算六個月後的日期
                // 格式化為 yyyy-mm-dd 格式
                var year = startDateObj.getFullYear();
                var month = ("0" + (startDateObj.getMonth() + 1)).slice(-2);
                var day = ("0" + startDateObj.getDate()).slice(-2);
                var newEndDate = year + "-" + month + "-" + day;
                document.getElementById('<%= apply_start.ClientID %>').setAttribute('max', startDate);
                document.getElementById('<%= apply_start.ClientID %>').setAttribute('min', newEndDate);
            }
        }

        function setStartDateRange2() {
            var passDate = document.getElementById('<%= pass_end.ClientID %>').value;

            if (passDate) {
                // 計算日期格式：年月日 (yyyy-mm-dd)
                var passDateObj = new Date(passDate);  // 更正這裡使用 passDate
                passDateObj.setMonth(passDateObj.getMonth() - 6);  // 計算六個月後的日期
                // 格式化為 yyyy-mm-dd 格式
                var year = passDateObj.getFullYear();
                var month = ("0" + (passDateObj.getMonth() + 1)).slice(-2);  // 1-12月轉為兩位數
                var day = ("0" + passDateObj.getDate()).slice(-2);  // 日期轉為兩位數
                var newStartDate = year + "-" + month + "-" + day;  // 新的六個月後的日期

                document.getElementById('<%= pass_start.ClientID %>').setAttribute('max', passDate);
                document.getElementById('<%= pass_start.ClientID %>').setAttribute('min', newStartDate);
            }
        }

        function printScreen() {
            var content = document.getElementById('usermd').innerHTML;
            var printWindow = window.open('', '', 'height=600,width=800');
            printWindow.document.write('<html><head><title></title>');
            printWindow.document.write('<style>body { font-family: Arial, sans-serif; }</style>');
            printWindow.document.write('</head><body>');
            printWindow.document.write(content);
            printWindow.document.write('</body></html>');
            printWindow.document.close();
            printWindow.print();
        }
</script>
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div class="write_Box">
        <div class="write_Title" style="height: auto;">
            <h5>
                <asp:Literal runat="server" ID="box_title">查詢篩選</asp:Literal></h5>

        </div>
        <div class="write_textBox">
            <div class="dataBox row d-flex">
                <div class="dll" style="display: flex; align-items: center; flex: 0 0 40%">
                    <div class="col-5 formItem title3">
                        <div class="ItemTitle"><span>申請組室</span></div>
                        <div class="ItemContent">
                            <asp:DropDownList ID="apply_group" runat="server" CssClass="form-control" Style="min-width: 100%">
                                <asp:ListItem Value="0">全選</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-6 formItem title2">
                        <div class="ItemTitle">
                            <span>品名</span>
                        </div>
                        <div class="ItemContent">
                            <asp:DropDownList ID="apply_product" runat="server" CssClass="form-control" Style="min-width: 100%">
                                <asp:ListItem Value="0">全選</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="dll" style="display: flex; align-items: center; flex-wrap: wrap; flex: 0 0 60%">
                    <div class="col-xl-7 col-sm-5 formItem title3">
                        <div class="ItemTitle"><span>預算科目</span></div>
                        <div class="ItemContent">
                            <asp:DropDownList ID="apply_Buget" runat="server" CssClass="form-control" Style="min-width: 100%">
                                <asp:ListItem Value="0">全選</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-4 formItem title2">
                        <div class="ItemTitle">
                            <span>狀態</span>
                        </div>
                        <div class="ItemContent">
                            <asp:DropDownList ID="apply_state" runat="server" CssClass="form-control" Style="min-width: 100%">
                                <asp:ListItem Value="0">全選</asp:ListItem>
                                <asp:ListItem Value="1" Selected="True">申請中</asp:ListItem>
                                <asp:ListItem Value="2">已核銷</asp:ListItem>
                                <%--<asp:ListItem Value="3">退件</asp:ListItem>--%>
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
            </div>
            <div class="dataBox row" style="margin-bottom: -1rem">
                <div class="col-6 formItem title6">
                    <div class="ItemTitle" style="width: 30rem;"><span>申請及採購查詢</span><span style="color: #375471; font-size: 14px;">*申請日期和核銷日期僅能二擇一做查詢</span></div>
                </div>
            </div>
            <div class="dataBox row d-flex" style="padding: 10px;">
                <div class="pr-2" style="border: 1px dashed #FBA2D5; display: inline-block; flex-wrap: wrap;">
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                        <ContentTemplate>
                            <div class="formItem title5 pl-1" style="width: 100%; margin-left: 0px;">
                                <div class="ItemTitle"><span>申請日期</span></div>
                                <div class="ItemContent" style="display: flex; align-items: center; flex-wrap: wrap;">
                                    <asp:RadioButton ID="rb_Nolimit" runat="server" GroupName="applyDate" Text="不限" Style="margin-right: 5px; margin-left: 0px;" AutoPostBack="true" OnCheckedChanged="rb_SelectedIndexChanged" />
                                    <asp:RadioButton ID="rb_lastWeek" runat="server" GroupName="applyDate" Text="近一周" Style="margin-right: 0px;" AutoPostBack="true" OnCheckedChanged="rb_SelectedIndexChanged" />
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
                            <div class="formItem title5 pl-1" style="width: 100%; margin-left: 0px;">
                                <div class="ItemTitle"><span>採購日期</span></div>
                                <div class="ItemContent" style="display: flex; align-items: center; flex-wrap: wrap;">
                                    <asp:RadioButton ID="rb2_Nolimit" runat="server" GroupName="applyDate" Text="不限" Style="margin-right: 5px; margin-left: 0px;" AutoPostBack="true" OnCheckedChanged="rb_SelectedIndexChanged" />
                                    <asp:RadioButton ID="rb2_lastWeek" runat="server" GroupName="applyDate" Text="近一周" Style="margin-right: 0px;" AutoPostBack="true" OnCheckedChanged="rb_SelectedIndexChanged" />
                                    <asp:RadioButton ID="rb2_lastMonth" runat="server" GroupName="applyDate" Text="近一個月" Style="margin-right: 5px; margin-left: 0px;" AutoPostBack="true" OnCheckedChanged="rb_SelectedIndexChanged" />
                                    <asp:RadioButton ID="rb2_lastYear" runat="server" GroupName="applyDate" Text="近一年" Style="margin-right: 5px; margin-left: 0px;" AutoPostBack="true" OnCheckedChanged="rb_SelectedIndexChanged" />
                                    <div class="d-flex align-items-center">
                                        <asp:RadioButton ID="rb2_customRange" runat="server" GroupName="applyDate" Text="自訂區間" Style="margin-right: 0px; margin-left: 0px;" AutoPostBack="true" OnCheckedChanged="rb_SelectedIndexChanged" />
                                        <div style="flex: auto">
                                            <asp:TextBox ID="pass_start" runat="server" TextMode="Date" CssClass="form-control" Style="width: 150px; height: 35px; margin-left: 10px;" Enabled="false" OnChange="setEndDateRange()"></asp:TextBox>
                                            <span class="right" style="font-size: 22px; margin: 0 5px;">～</span>
                                            <asp:TextBox ID="pass_end" runat="server" TextMode="Date" CssClass="form-control" Style="width: 150px; height: 35px; margin-left: 8px;" Enabled="false" OnChange="setStartDateRange()"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>

                </div>
            </div>
            <div class="dataBox row" style="margin-top: 20px;">
                <div class="col-5 formItem title4">
                    <div class="ItemTitle"><span>關鍵字</span></div>
                    <div class="ItemContent" style="display: flex; align-items: center;">
                        <asp:TextBox ID="keyword" runat="server" CssClass="form-control" placeholder="請輸入使用人、保管人來做查詢"></asp:TextBox>
                    </div>
                </div>
            </div>
        </div>
        <div class="write_textBox d-flex justify-content-center">
            <asp:Button ID="searchbt" runat="server" Text="查詢" OnClick="searchbt_Click" CssClass="searchbt" />
        </div>
    </div>

    <div class="write_Box">
        <div class="write_Title d-flex justify-content-between write_TitleMark" style="padding-bottom: 0.5em">
            <h5>
                <asp:Literal runat="server" ID="Literal1">查詢結果</asp:Literal></h5>

            <span style="display: flex; align-items: center;">
                <span class="subTitleMarkText" style="margin-right: 10px;">
                    <span>註：1. 看物品使用人或保管人，可按[使用清單查看]</span>
                    <span class="leftDistance">2. 匯出單筆資料申請及採購登錄資料，按下功能中的[匯出]</span>
                </span>
                <span>
                    <asp:Button ID="export" runat="server" Text="匯出總表" CssClass="exportbt" OnClick="export_Click" />
                </span>
            </span>

        </div>
        <div class="write_textBox">
            <asp:UpdatePanel runat="server">
                <ContentTemplate>
                    <asp:GridView ID="gv" runat="server" class="table table-bordered mt-2" AutoGenerateColumns="False" OnRowCommand="gv_RowCommand" OnRowDataBound="gv_RowDataBound"
                        OnPageIndexChanging="gv_PageIndexChanging" OnSorting="gv_Sorting" PageSize="10" AllowPaging="true" AllowSorting="True" PagerSettings-Visible="False">
                        <EmptyDataTemplate>
                            無資料
                        </EmptyDataTemplate>
                        <HeaderStyle CssClass="table-topbar" />
                        <Columns>
                            <asp:BoundField DataField="id" HeaderText="id" ItemStyle-Width="1%" Visible="false" />
                            <asp:TemplateField HeaderText='組室<span class="fa fa-caret-down"/>' ItemStyle-HorizontalAlign="center" ItemStyle-Width="1%" SortExpression="apply_group">
                                <ItemTemplate>
                                    <asp:Label ID="apply_group" runat="server" Text='<%# Eval("apply_group") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText='申請人<span class="fa fa-caret-down"/>' ItemStyle-HorizontalAlign="center" ItemStyle-Width="1%" SortExpression="apply_user">
                                <ItemTemplate>
                                    <asp:Label ID="apply_user" runat="server" Text='<%# Eval("apply_user") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText='申請日期<span class="fa fa-caret-down"/>' ItemStyle-HorizontalAlign="center" ItemStyle-Width="0.5%" SortExpression="apply_date">
                                <ItemTemplate>
                                    <asp:Label ID="apply_date" runat="server" Text='<%# Eval("apply_date","{0:yyyy/MM/dd}") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText='品名<span class="fa fa-caret-down"/>' ItemStyle-HorizontalAlign="center" ItemStyle-Width="3%" SortExpression="product_id">
                                <ItemTemplate>
                                    <asp:Label ID="product_id" runat="server" Text='<%# Eval("product_id") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText='數量<span class="fa fa-caret-down"/>' ItemStyle-HorizontalAlign="center" ItemStyle-Width="1%" SortExpression="number">
                                <ItemTemplate>
                                    <asp:Label ID="number" runat="server" Text='<%# Eval("number") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText='單價<span class="fa fa-caret-down"/>' ItemStyle-HorizontalAlign="center" ItemStyle-Width="1%" SortExpression="price">
                                <ItemTemplate>
                                    <asp:Label ID="price" runat="server" Text='<%# Eval("price") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText='合計<span class="fa fa-caret-down"/>' ItemStyle-HorizontalAlign="center" ItemStyle-Width="1%" SortExpression="total">
                                <ItemTemplate>
                                    <asp:Label ID="total" runat="server" Text='<%# Eval("total") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText='採購日期<span class="fa fa-caret-down"/>' ItemStyle-HorizontalAlign="center" ItemStyle-Width="1.5%" SortExpression="pass_date">
                                <ItemTemplate>
                                    <asp:Label ID="pass_date" runat="server" Text='<%# Eval("pass_date","{0:yyyy/MM/dd}") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText='預算科目<span class="fa fa-caret-down"/>' ItemStyle-HorizontalAlign="center" ItemStyle-Width="5%" SortExpression="Budget_id">
                                <ItemTemplate>
                                    <asp:Label ID="Budget_id" runat="server" Text='<%# Eval("Budget_id") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText='狀態<span class="fa fa-caret-down"/>' ItemStyle-HorizontalAlign="center" ItemStyle-Width="1%" SortExpression="state">
                                <ItemTemplate>
                                    <asp:Label ID="state" runat="server" Text='<%# Eval("state") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="0.5%">
                                <HeaderTemplate>
                                    <span>使用清單</span>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Button runat="server" CssClass="gw_bt" ID="check" CommandName="check" CommandArgument='<%# Eval("id") %>' Text="查看" Style="background-color: #1885C5" OnClientClick=" showusermd();" />
                                    <asp:HiddenField ID="hf_list" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="0.5%">
                                <HeaderTemplate>
                                    <span>匯出</span>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Button ID="export" runat="server" Text="匯出" CommandArgument='<%# Eval("id") %>' CssClass="gw_bt" Style="background-color: #1C6B00; margin: 1.5px 0;" OnClientClick="printScreen(); return false;"></asp:Button>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="2%">
                                <HeaderTemplate>
                                    <span>功能</span>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Button ID="export_mag" runat="server" Text="匯出" CommandArgument='<%# Eval("id") %>' CssClass="gw_bt" Style="background-color: #1C6B00; margin: 1.5px 0;display:none;" Visible="false" OnClick="export_single_Click"></asp:Button>
                                    <asp:Button runat="server" CssClass="gw_bt" ID="modify" CommandName="modify" CommandArgument='<%# Eval("id") + "," + Eval("state") %>' Text="編輯" Style="background-color: #68B100" />
                                    <asp:Button runat="server" CssClass="gw_bt" ID="del" CommandName="del" CommandArgument='<%# Eval("id") %>' Text="刪除" Style="background-color: #B83F1F" OnClientClick="return confirm('確定要刪除嗎？');" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </ContentTemplate>
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
    <div class="modal fade bs-NewUser-modal-lg" id="usermd" tabindex="-1" role="dialog" aria-labelledby="myLargeModalLabel" aria-hidden="true" style="backdrop-filter: brightness(0.3);">
        <div class="modal-dialog modal-lg modal-dialog-centered" style="max-width: 500px;">
            <div class="modal-content">
                <div class="modal-header" style="background-color: #C5ECFF; font-size: 22px; display: flex; justify-content: center; align-items: center; position: relative;">
                    <b><span id="Span2" runat="server" style="color: #003168">查看清單</span></b>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close" style="position: absolute; right: 10px;">
                        <image src="image/popup_close.png"></image>
                    </button>
                </div>
                <div class="modal-body">
                    <div class="MainClass">
                        <div id="content3" role="form" class="form-horizontal text-center">

                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <div class="GridViewTitleText" style="font-size: 1.125rem;">使用者清單</div>
                                    <asp:GridView ID="user_gv" class="table table-bordered mt-2 position-relative" runat="server" AutoGenerateColumns="False" OnRowCommand="user_gv_RowCommand" PageSize="10" AllowPaging="true" OnPageIndexChanging="user_gv_PageIndexChanging" PagerSettings-Visible="false">
                                        <EmptyDataTemplate>
                                            無資料
                                        </EmptyDataTemplate>
                                        <HeaderStyle CssClass="table-topbar" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="序列" ItemStyle-HorizontalAlign="center" ItemStyle-Width="2%">
                                                <ItemTemplate>
                                                    <asp:Label ID="serial" runat="server"
                                                        Text='<%# (Container.DataItemIndex + 1) %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="id" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="id" runat="server" Text='<%# Eval("id") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="使用者" ItemStyle-HorizontalAlign="center" ItemStyle-Width="3%">
                                                <ItemTemplate>
                                                    <asp:Label ID="name" runat="server" Text='<%# Eval("user_name") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="使用數量" ItemStyle-HorizontalAlign="center" ItemStyle-Width="3%">
                                                <ItemTemplate>
                                                    <asp:Label ID="user_quantity" runat="server" Text='<%# Eval("user_quantity") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                    <%--<asp:SqlDataSource ID="SqlDataSource1" ConnectionString="<%$ ConnectionStrings:eip %>" runat="server" SelectCommand=""></asp:SqlDataSource>--%>
                                    <div id="Div1" runat="server" class="btn-toolbar" role="toolbar" aria-label="Toolbar with button groups" style="justify-content: center;">
                                        <div class="btn-group pull-right" role="group" aria-label="First group" style="align-items: center;">

                                            <div class="NumberOfPages" style="padding: 5px; float: left">
                                                第<asp:Label ID="lblPageIndex3" runat="server" Text="0" CssClass="Number" Style="color: #0071BC"></asp:Label>頁    
                                            </div>

                                            <div class="NumberOfPages" style="padding: 5px; float: left">
                                                總計<asp:Label ID="lblDataCount3" runat="server" Text="0" CssClass="Number" Style="color: #0071BC"></asp:Label>筆
                                            </div>

                                            <div class="NumberOfPages" style="padding: 5px; float: left">
                                                <asp:LinkButton ID="lkbPagePrev3" runat="server" OnClick="lkbPagePrev3_Click" Style="color: #0071BC; text-decoration: underline;">上一頁</asp:LinkButton>
                                                <asp:LinkButton ID="lkbPageNext3" runat="server" OnClick="lkbPageNext3_Click" Style="color: #0071BC; text-decoration: underline;">下一頁</asp:LinkButton>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="GridViewTitleText" style="font-size: 1.125rem;">保管人清單</div>
                                    <asp:GridView ID="custodian_gv" class="table table-bordered mt-2 position-relative" runat="server" AutoGenerateColumns="False" OnRowCommand="custodian_gv_RowCommand" PageSize="10" AllowPaging="true" OnPageIndexChanging="custodian_gv_PageIndexChanging" PagerSettings-Visible="false">
                                        <EmptyDataTemplate>
                                            無資料
                                        </EmptyDataTemplate>
                                        <HeaderStyle CssClass="table-topbar" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="序列" ItemStyle-HorizontalAlign="center" ItemStyle-Width="2%">
                                                <ItemTemplate>
                                                    <asp:Label ID="serial" runat="server"
                                                        Text='<%# (Container.DataItemIndex + 1) %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="id" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="id" runat="server" Text='<%# Eval("id") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="保管人" ItemStyle-HorizontalAlign="center" ItemStyle-Width="3%">
                                                <ItemTemplate>
                                                    <asp:Label ID="name" runat="server" Text='<%# Eval("custodian_name") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="保管數量" ItemStyle-HorizontalAlign="center" ItemStyle-Width="3%">
                                                <ItemTemplate>
                                                    <asp:Label ID="user_quantity" runat="server" Text='<%# Eval("custodian_quantity") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                    <asp:SqlDataSource ID="SqlDataSource2" ConnectionString="<%$ ConnectionStrings:eip %>" runat="server" SelectCommand="SELECT [id],[custodian_name],[custodian_quantity]  FROM [item_apply_custodianlist] where apply_id=8"></asp:SqlDataSource>
                                    <div id="down2" runat="server" class="btn-toolbar" role="toolbar" aria-label="Toolbar with button groups" style="justify-content: center;">
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
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <hr>
                        <asp:UpdatePanel runat="server">
                            <ContentTemplate>
                                <div style="display: flex; justify-content: center;">
                                    <span>最後修改時間：<asp:Label ID="lbl_update_time" runat="server" Text=""></asp:Label></span>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>

                        <div style="text-align: center; margin-top: 8px;">
                            <asp:Button runat="server" Text="返回" class="bt" Style="background: #777777"
                                OnClientClick="return hideusermd();" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>

