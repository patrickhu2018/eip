<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage2.master" AutoEventWireup="true" CodeFile="Repair_inventory_list.aspx.cs" Inherits="Repair_inventory_list" MaintainScrollPositionOnPostback="true" %>

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
            margin-right: 10px;    padding: 10px 30px;
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

        .addbt {
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
            z-index: 0;
            background-color: rgba(0, 0, 0, 1); /* 調整透明度 */
        }

        .upload-container {
            position: relative;
            width: 200px;
            height: 150px;
            border: 1px solid #ccc;
            display: flex;
            align-items: center;
            justify-content: center;
            flex-direction: column;
            background-color: #f7f7f7;
        }

        .image-preview {
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
          .ItemContent label {
      margin-bottom: 0;
      margin-right: 20px;
  }

  .ItemContent input[type=radio] {
      margin-right: 5px;
  }
    </style>
    <script type="text/javascript">
        // 設置 apply_end 的日期範圍
        function setEndDateRange() {
            var startDate = document.getElementById('<%= receive_start.ClientID %>').value;
                if (startDate) {
                    // 計算日期格式：年月日 (yyyy-mm-dd)
                    var startDateObj = new Date(startDate);
                    startDateObj.setMonth(startDateObj.getMonth() + 6);  // 計算六個月後的日期
                    // 格式化為 yyyy-mm-dd 格式
                    var year = startDateObj.getFullYear();
                    var month = ("0" + (startDateObj.getMonth() + 1)).slice(-2);  // 1-12月轉為兩位數
                    var day = ("0" + startDateObj.getDate()).slice(-2);  // 日期轉為兩位數
                    var newEndDate = year + "-" + month + "-" + day;  // 新的六個月後的日期

                    document.getElementById('<%= receive_end.ClientID %>').setAttribute('max', newEndDate);
                document.getElementById('<%= receive_end.ClientID %>').setAttribute('min', startDate);
            }
        }
        function setStartDateRange() {
            var startDate = document.getElementById('<%= receive_end.ClientID %>').value;

            if (startDate) {
                // 計算日期格式：年月日 (yyyy-mm-dd)
                var startDateObj = new Date(startDate);
                startDateObj.setMonth(startDateObj.getMonth() - 6);  // 計算六個月後的日期
                // 格式化為 yyyy-mm-dd 格式
                var year = startDateObj.getFullYear();
                var month = ("0" + (startDateObj.getMonth() + 1)).slice(-2);
                var day = ("0" + startDateObj.getDate()).slice(-2);
                var newEndDate = year + "-" + month + "-" + day;
                document.getElementById('<%= receive_start.ClientID %>').setAttribute('max', startDate);
                document.getElementById('<%= receive_start.ClientID %>').setAttribute('min', newEndDate);
            }
        }
</script>
    <div class="write_Box">
        <div class="write_Title">
            <h5>
                <asp:Literal runat="server" ID="box_title">清單篩選</asp:Literal></h5>
        </div>
        <div class="write_textBox">
            <div class="dataBox row">
                <div class="col-4 formItem title6">
                    <div class="ItemTitle"><span>物料代碼/名稱</span></div>
                    <div class="ItemContent">
                        <asp:DropDownList ID="materials" runat="server" CssClass="form-control" Style="width: 130%">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="col-4 formItem title4" style="margin-left: 100px;">
                    <div class="ItemTitle"><span>修繕地點</span></div>
                    <div class="ItemContent">
                        <asp:DropDownList ID="place" runat="server" CssClass="form-control" Style="width: 100%">
                        </asp:DropDownList>
                    </div>
                </div>
            </div>
            <div class="dataBox row">
                <div style="display: flex; align-items: center; flex-wrap: wrap;">
                    <div class="col-12 formItem title6">
                        <div class="ItemTitle"><span>領用日期</span></div>
                        <div class="ItemContent" style="display: flex; align-items: center; flex-wrap: wrap;">
                            <asp:RadioButton ID="rb_lastWeek" runat="server" GroupName="applyDate" Text="近一周" Checked="true" Style="margin-right: 0px;" AutoPostBack="true" OnCheckedChanged="rb_SelectedIndexChanged" />
                            <asp:RadioButton ID="rb_lastMonth" runat="server" GroupName="applyDate" Text="近一個月" Style="margin-right: 5px; margin-left: 0px;" AutoPostBack="true" OnCheckedChanged="rb_SelectedIndexChanged" />
                            <asp:RadioButton ID="rb_lastYear" runat="server" GroupName="applyDate" Text="近一年" Style="margin-right: 5px; margin-left: 0px;" AutoPostBack="true" OnCheckedChanged="rb_SelectedIndexChanged" />
                            <div class="d-flex align-items-center">
                                <asp:RadioButton ID="rb_customRange" runat="server" GroupName="applyDate" Text="自訂區間" Style="margin-right: 0px; margin-left: 0px;" AutoPostBack="true" OnCheckedChanged="rb_SelectedIndexChanged" />
                                <div style="flex: auto">
                                    <asp:TextBox ID="receive_start" runat="server" TextMode="Date" CssClass="form-control" Style="width: 150px; height: 35px; margin-left: 10px;" Enabled="false" OnChange="setEndDateRange()"></asp:TextBox>
                                    <span class="right" style="font-size: 22px; margin: 0 5px;">～</span>
                                    <asp:TextBox ID="receive_end" runat="server" TextMode="Date" CssClass="form-control" Style="width: 150px; height: 35px; margin-left: 8px;" Enabled="false" OnChange="setStartDateRange()"></asp:TextBox>
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
    <div class="write_Box">
        <div class="write_Title d-flex justify-content-between">
            <h5>
                <asp:Literal runat="server" ID="Literal1">物品庫存表</asp:Literal></h5>
            <div>
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
                    <asp:TemplateField HeaderText='修繕單編號<span class="fa fa-caret-down"/>' ItemStyle-HorizontalAlign="center" ItemStyle-Width="1%" SortExpression="repair_no">
                        <ItemTemplate>
                            <asp:Label ID="repair_no" runat="server" Text='<%# Eval("repair_no") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText='領用日期<span class="fa fa-caret-down"/>' ItemStyle-HorizontalAlign="center" ItemStyle-Width="1%" SortExpression="receivedate">
                        <ItemTemplate>
                            <asp:Label ID="receivedate" runat="server" Text='<%# Eval("receivedate","{0:yyyy/MM/dd}") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText='領用物料代碼<span class="fa fa-caret-down"/>' ItemStyle-HorizontalAlign="center" ItemStyle-Width="1%" SortExpression="materials_no">
                        <ItemTemplate>
                            <asp:Label ID="materials_no" runat="server" Text='<%# Eval("materials_no") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText='領用物料名稱<span class="fa fa-caret-down"/>' ItemStyle-HorizontalAlign="center" ItemStyle-Width="1%" SortExpression="materials_name">
                        <ItemTemplate>
                            <asp:Label ID="materials_name" runat="server" Text='<%# Eval("materials_name") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText='修繕地點<span class="fa fa-caret-down"/>' ItemStyle-HorizontalAlign="center" ItemStyle-Width="1%" SortExpression="repair_place">
                        <ItemTemplate>
                            <asp:Label ID="repair_place" runat="server" Text='<%# Eval("repair_place") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText='使用數量<span class="fa fa-caret-down"/>' ItemStyle-HorizontalAlign="center" ItemStyle-Width="1%" SortExpression="number">
                        <ItemTemplate>
                            <asp:Label ID="number" runat="server" Text='<%# Eval("number") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-Width="0.5%">
                        <HeaderTemplate><span>功能</span></HeaderTemplate>
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

