<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage2.master" AutoEventWireup="true" CodeFile="Repair_management_query.aspx.cs" Inherits="Repair_management_query" MaintainScrollPositionOnPostback="true" %>

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

        .ItemContent label {
            margin-bottom: 0;
            margin-right: 20px;
        }

        .ItemContent input[type=radio] {
            margin-right: 5px;
        }

    </style>
    <script type="text/javascript">

        var applyDateRangeMonths = 6;

        function getApplyStartInput() {
            return document.getElementById('<%= apply_start.ClientID %>');
        }

        function getApplyEndInput() {
            return document.getElementById('<%= apply_end.ClientID %>');
        }

        function parseDateValue(value) {
            if (!value) {
                return null;
            }

            var parts = value.split("-");
            if (parts.length !== 3) {
                return null;
            }

            return new Date(Number(parts[0]), Number(parts[1]) - 1, Number(parts[2]));
        }

        function formatDateValue(date) {
            var year = date.getFullYear();
            var month = ("0" + (date.getMonth() + 1)).slice(-2);
            var day = ("0" + date.getDate()).slice(-2);
            return year + "-" + month + "-" + day;
        }

        function addMonthsClamped(date, months) {
            var target = new Date(date.getFullYear(), date.getMonth() + months, 1);
            var lastDay = new Date(target.getFullYear(), target.getMonth() + 1, 0).getDate();
            target.setDate(Math.min(date.getDate(), lastDay));
            return target;
        }

        function clearDateBounds(input) {
            input.removeAttribute("min");
            input.removeAttribute("max");
        }

        function setDateBounds(input, minValue, maxValue) {
            input.setAttribute("min", minValue);
            input.setAttribute("max", maxValue);
        }

        function isOutsideBounds(input) {
            return input.value && ((input.min && input.value < input.min) || (input.max && input.value > input.max));
        }

        function setEndBoundsFromStart(startInput, endInput) {
            var startDate = parseDateValue(startInput.value);
            if (!startDate) {
                return;
            }

            setDateBounds(endInput, startInput.value, formatDateValue(addMonthsClamped(startDate, applyDateRangeMonths)));
        }

        function setStartBoundsFromEnd(startInput, endInput) {
            var endDate = parseDateValue(endInput.value);
            if (!endDate) {
                return;
            }

            setDateBounds(startInput, formatDateValue(addMonthsClamped(endDate, -applyDateRangeMonths)), endInput.value);
        }

        function syncApplyDateRange(changedField) {
            var startInput = getApplyStartInput();
            var endInput = getApplyEndInput();

            clearDateBounds(startInput);
            clearDateBounds(endInput);

            if (changedField === "start") {
                setEndBoundsFromStart(startInput, endInput);
                if (isOutsideBounds(endInput)) {
                    endInput.value = "";
                }
            }
            else if (changedField === "end") {
                setStartBoundsFromEnd(startInput, endInput);
                if (isOutsideBounds(startInput)) {
                    startInput.value = "";
                }
            }
            else {
                setEndBoundsFromStart(startInput, endInput);
                if (isOutsideBounds(endInput)) {
                    endInput.value = "";
                }

                setStartBoundsFromEnd(startInput, endInput);
                if (isOutsideBounds(startInput)) {
                    startInput.value = "";
                }
            }

            clearDateBounds(startInput);
            clearDateBounds(endInput);
            setEndBoundsFromStart(startInput, endInput);
            setStartBoundsFromEnd(startInput, endInput);
        }

        function setEndDateRange() {
            syncApplyDateRange("start");
        }

        function setStartDateRange() {
            syncApplyDateRange("end");
        }

        document.addEventListener("DOMContentLoaded", function () {
            syncApplyDateRange();
        });
</script>
    <div class="d-flex">
        <div class="write_Box" style="margin-right: 4rem; width: 100%">
            <div class="write_Title">
                <h5>
                    <asp:Literal runat="server" ID="box_title">申請查詢</asp:Literal></h5>
            </div>
            <div class="write_textBox">
                <div class="dataBox row d-flex" style="margin-right: 50px; width: 100%">
                    <div class=" col-md-4 formItem title3">
                        <div class="ItemTitle"><span>修繕地點</span></div>
                        <div class="ItemContent">
                            <asp:DropDownList ID="apply_place" runat="server" CssClass="form-control" Style="width: 100%;" OnSelectedIndexChanged="apply_place_SelectedIndexChanged" AutoPostBack="true">
                                <%--<asp:ListItem Value="0">全部</asp:ListItem>--%>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class=" col-md-4 formItem title3" style="margin-left: 10px;">
                        <div class="ItemTitle">
                            <span>修繕樓層</span>
                        </div>
                        <div class="ItemContent">
                            <asp:DropDownList ID="apply_floor" runat="server" CssClass="form-control" Style="width: 100%">
                                <%--<asp:ListItem Value="0">全部</asp:ListItem>--%>
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="dataBox row" style="display: flex; align-items: center;">
                    <div id="src1" class="col-12 col-lg-12 formItem title3">
                        <div class="ItemTitle"><span>處理狀況</span></div>
                        <div class="ItemContent" style="display: flex; align-items: center;">
                            <div class="row" style="width: auto">
                                <div class="d-flex flex-wrap" style="margin-left: 15px;">
                                    <div class="d-flex align-items-center">
                                        <asp:CheckBox runat="server" ID="chk4" Style="margin-right: 5px;" OnCheckedChanged="chk4_CheckedChanged" AutoPostBack="true" /><span>全選</span>
                                    </div>
                                    <div class="d-flex  align-items-center">
                                        <asp:CheckBox runat="server" ID="chk1" Style="margin-right: 5px; margin-left: 5px;" /><span>待處理</span>
                                    </div>
                                    <div class="d-flex align-items-center">
                                        <asp:CheckBox runat="server" ID="chk2" Style="margin-right: 5px; margin-left: 5px;" /><span>處理中</span>
                                    </div>
                                    <div class="d-flex align-items-center">
                                        <asp:CheckBox runat="server" ID="chk3" Style="margin-right: 5px; margin-left: 5px;" /><span>已完成</span>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div style="display: flex; align-items: center; flex-wrap: wrap;">
                        <div class="col-12 formItem title3">
                            <div class="ItemTitle"><span>申請日期</span></div>
                            <div class="ItemContent" style="display: flex; align-items: center; flex-wrap: wrap;">
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
                    </div>
                </div>



            </div>
            <div class="write_textBox d-flex justify-content-center">
                <asp:Button ID="searchbt" runat="server" Text="查詢" CssClass="searchbt" OnClick="searchbt_Click" />
            </div>
        </div>
        <asp:Button ID="addapply" runat="server" Text="申請&#10;修繕項目" CssClass="ml-auto addbt" Style="white-space: pre-line;" OnClick="addapply_Click" />
    </div>
    <div class="write_Box">
        <div class="write_Title d-flex justify-content-between">
            <h5>
                <asp:Literal runat="server" ID="Literal1">查詢結果</asp:Literal></h5>




            <div>
                <asp:Button ID="export" runat="server" Text="匯出總表" CssClass="exportbt" OnClick="export_Click" />
            </div>

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
                    <asp:TemplateField HeaderText='修繕單編號<span class="fa fa-caret-down"/>' ItemStyle-HorizontalAlign="center" ItemStyle-Width="11%" SortExpression="repair_no">
                        <ItemTemplate>
                            <asp:Label ID="repair_no" runat="server" Text='<%# Eval("repair_no") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText='申請日期<span class="fa fa-caret-down"/>' ItemStyle-HorizontalAlign="center" ItemStyle-Width="11%" SortExpression="apply_date">
                        <ItemTemplate>
                            <asp:Label ID="apply_date" runat="server" Text='<%# Eval("apply_date","{0:yyyy/MM/dd}") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText='申請人<span class="fa fa-caret-down"/>' ItemStyle-HorizontalAlign="center" ItemStyle-Width="9%" SortExpression="apply_user">
                        <ItemTemplate>
                            <asp:Label ID="apply_user" runat="server" Text='<%# Eval("apply_user") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText='修繕地點<span class="fa fa-caret-down"/>' ItemStyle-HorizontalAlign="center" ItemStyle-Width="14.5%" SortExpression="place_id">
                        <ItemTemplate>
                            <asp:Label ID="place_id" runat="server" Text='<%# Eval("place_name") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText='修繕樓層<span class="fa fa-caret-down"/>' ItemStyle-HorizontalAlign="center" ItemStyle-Width="9%" SortExpression="floor_id">
                        <ItemTemplate>
                            <asp:Label ID="floor_id" runat="server" Text='<%# Eval("floor_name") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText='位置<span class="fa fa-caret-down"/>' ItemStyle-HorizontalAlign="center" ItemStyle-Width="13%" SortExpression="location_id">
                        <ItemTemplate>
                            <asp:Label ID="location_id" runat="server" Text='<%# Eval("Location_name") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText='事由<span class="fa fa-caret-down"/>' ItemStyle-HorizontalAlign="left" ItemStyle-Width="14.5%" SortExpression="apply_reason">
                        <ItemTemplate>
                            <asp:Label ID="apply_reason" runat="server" Text='<%# Eval("apply_reason") %>' class="grid-text"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText='處理狀況<span class="fa fa-caret-down"/>' ItemStyle-HorizontalAlign="center" ItemStyle-Width="8.5%" SortExpression="state">
                        <ItemTemplate>
                            <asp:Label ID="state" runat="server" Text='<%# Eval("state") %>' Visible="false"></asp:Label>
                            <asp:Label ID="stateDisplay" runat="server" Text='<%# Eval("state", "{0}") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-Width="9.5%">
                        <HeaderTemplate>
                            <span>功能</span>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Button runat="server" CssClass="gw_bt" ID="check" CommandName="check" CommandArgument='<%# Eval("id") %>' Text="檢視" Style="background-color: #1885C5" />
                            <asp:Button runat="server" CssClass="gw_bt" ID="modify" CommandName="modify" CommandArgument='<%# Eval("id") %>' Text="編輯" Style="background-color: #68B100" />
                            <asp:Button runat="server" CssClass="gw_bt" ID="del" CommandName="del" CommandArgument='<%# Eval("id") %>' Text="退件" Style="background-color: #B83F1F" OnClientClick="return confirm('確定要退件嗎？');" />
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

