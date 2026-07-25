<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage3.master" AutoEventWireup="true" CodeFile="Meetingroom_management.aspx.cs" MaintainScrollPositionOnPostback="true" Inherits="Meetingroom_management" %>

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
            margin-right: 10px;
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
            border-radius: 4px;
            border: 0px;
            color: #FFFFFF;
            opacity: 1;
            background-image: url(../image/icon_16.png);
            background-position: left center;
            padding-left: 27px;
        }

        .modifybt {
            width: 119px;
            height: 33px;
            background: #7DC11B 0% 0% no-repeat padding-box;
            border-radius: 4px;
            border: 0px;
            color: #FFFFFF;
            opacity: 1;
            background-image: url(../image/icon_13.png);
            background-position: left center;
            padding-left: 25px;
        }

        .modal-backdrop {
            display: none;
        }

        #modal1 input[type="checkbox"] {
            margin-right: 10px; /* 增加框框和文字之間的間隔 */
        }

        #modal1 input[type="text"] {
            margin-left: 10px; /* 增加文本框和 "其他" 文字之間的間隔 */
        }

        .custom-checkbox input[type="checkbox"] {
            margin-right: 5px; /* 使用 !important 確保樣式生效 */
        }

        .custom-checkbox input[type="text"] {
            margin-left: 0px; /* 使用 !important 確保樣式生效 */
        }

        .color-box {
            width: 40px;
            height: 40px;
            margin: 10px;
            cursor: pointer;
            border: 2px solid transparent; /* 默認邊框是透明的 */
            position: relative; /* 讓勾勾圖示能夠定位在色塊上 */
            transition: border 0.3s ease;
        }

        /* 當選中時的樣式 */
        .selected {
            box-shadow: 0px 0px 10px rgba(0, 0, 0, 0.2); /* 加入陰影效果 */
        }

            .selected::after {
                content: "✔"; /* 使用勾勾符號 */
                font-size: 40px; /* 勾勾的大小 */
                color: white; /* 勾勾的顏色 */
                position: absolute; /* 使勾勾定位在色塊的中心 */
                top: 50%; /* 垂直居中 */
                left: 50%; /* 水平居中 */
                transform: translate(-50%, -50%); /* 使用 transform 使其完美居中 */
            }

        .nav-link {
            background-color: #CCCCCC;
            font-weight: bold;
            font-size: 22px;
            color: #777777;
            border-radius: 15px 15px 0px 0px !important;
        }

        .nav-tabs .nav-link:hover, .nav-tabs .nav-link:focus {
            border-color: transparent;
        }

        .nav-link:hover, .nav-link:focus {
            text-decoration: none;
        }

        .navactive {
            background-color: #FFFFFF !important;
            color: #643C19 !important;
            border-radius: 15px 15px 0px 0px !important;
            border-bottom: 1px solid #FFFFFF !important;
        }

        .box {
            background: #EFEFEC 0% 0% no-repeat padding-box;
            border-radius: 5px;
            width: 600px;
            padding-bottom: 30px;
        }

        .withebox {
            background: #FFFFFF 0% 0% no-repeat padding-box;
            border-radius: 3px;
            padding: 15px;
        }

        .write_Box hr {
            margin-bottom: 0.5rem !important;
        }
    </style>
    <script type="text/javascript">
        var selectedItemIds = [];
        var maxSelection = 3;  // 最大选中数量
        var selectedCount = 0;
        var userId = '<%= Session["user_id"] %>';
        document.addEventListener("DOMContentLoaded", function () {
            var checkboxes = document.querySelectorAll('input[id^="ContentPlaceHolder1_gv_favorite"]');
            checkboxes.forEach(function (checkbox) {
                if (checkbox.checked) {
                    selectedCount++;
                    var spanElement = checkbox.closest('span');
                    var itemId = spanElement.getAttribute('data-item-id');
                    if (itemId && !selectedItemIds.includes(itemId)) {
                        selectedItemIds.push(itemId);
                    }
                }
            });
        });


        function onFavoriteChanged(checkbox, event) {
            var isChecked = checkbox.checked;


            var row = checkbox.closest('tr');
            var rowIndex = row.rowIndex;
            var spanElement = checkbox.closest('span');
            var itemId = spanElement.getAttribute("data-item-id");

            if (isChecked) {
                selectedCount++;
                if (!selectedItemIds.includes(itemId)) {
                    selectedItemIds.push(itemId);
                }
            } else {
                selectedCount--;
                selectedItemIds = selectedItemIds.filter(id => id !== itemId);
            }
            if (selectedCount > maxSelection) {
                alert("最多只能選擇 " + maxSelection + " 項！");
                checkbox.checked = false;
                selectedCount--;
                selectedItemIds = selectedItemIds.filter(id => id !== itemId);
                return;

            }
            var selectedItemsStr = selectedItemIds.join(",");
            $.ajax({
                url: 'WebService.asmx/savefav',
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify({
                    id: userId,
                    selected_items: selectedItemsStr
                }),
                dataType: 'json',

            });
        }
        function rgbToHex(rgb) {
            var result = rgb.match(/\d+/g); // 提取 RGB 數字
            var r = parseInt(result[0]).toString(16);
            var g = parseInt(result[1]).toString(16);
            var b = parseInt(result[2]).toString(16);

            // 如果單位不足兩位，則補充零
            return "#" + (r.length == 1 ? "0" + r : r) + (g.length == 1 ? "0" + g : g) + (b.length == 1 ? "0" + b : b);
        }
        function toggleSelection(element, color) {
            var allBoxes = document.querySelectorAll('.color-box');
            allBoxes.forEach(function (box) {
                box.classList.remove('selected');
            });
            element.classList.add('selected');
            document.getElementById('<%= SelectedColor.ClientID %>').value = color;
            console.log(document.getElementById('<%= SelectedColor.ClientID %>').value)
        }
        function setSelectedColor(color) {
            var colorBoxes = document.querySelectorAll('.color-box');
            colorBoxes.forEach(function (box) {
                // 將顏色框的背景顏色轉換為十六進制
                var boxColorHex = rgbToHex(box.style.backgroundColor);
                // 如果顏色框的背景色與後端顏色相同，則選中該顏色框
                if (boxColorHex === color.toLowerCase()) {  // 轉換後端顏色為小寫進行比較
                    box.classList.add('selected');  // 設置該顏色框為選中狀態
                    document.getElementById('<%= SelectedColor.ClientID %>').value = color;  // 設置隱藏欄位的顏色值
                }
            });
        }
        document.addEventListener("DOMContentLoaded", function () {
            var addButton = document.getElementById("<%= add.ClientID %>");
            addButton.addEventListener("click", function (event) {
                var textboxes = document.querySelectorAll('#modal1 .form-control');
                textboxes.forEach(function (textbox) {
                    textbox.value = '';  // 清空文本框的值
                });

                // 清空 CheckBox
                var checkboxes = document.querySelectorAll('#modal1 input[type="checkbox"]');
                checkboxes.forEach(function (checkbox) {

                    checkbox.checked = false;  // 取消勾選框
                });

                // 清空 顏色選擇
                var colorBoxes = document.querySelectorAll('#modal1 .color-box');
                colorBoxes.forEach(function (colorBox) {
                    colorBox.classList.remove('selected')  // 移除選中的邊框顏色
                });
                document.getElementById('<%= SelectedColor.ClientID %>').value = '';  // 清空選擇顏色的 HiddenField
                document.getElementById('<%= hf_id.ClientID %>').value = '';  // 清空 id 的 HiddenField

                var submitbt = document.getElementById('<%= submit.ClientID %>')
                var updatebt = document.getElementById('<%= update.ClientID %>')
                submitbt.style.display = 'inline-block';
                updatebt.style.display = 'none';
                console.log(submitbt)
                console.log(updatebt)
                $('#modal1').modal('show');
            });
        });
</script>
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div class="d-flex">
        <div class="write_Box" style="flex: 0 0 100%;">
            <div class="write_Title" style="display: flex;">
                <h5 style="width: 20%; display:flex;align-items:end;">
                    <asp:Literal runat="server" ID="box_title">會議室設備清單</asp:Literal></h5>
                <div style="display: inline-block; width: 80%; text-align: right;">
                    <asp:Label runat="server" ID="Label2" class="note mr-3">註：當您點擊任一會議室，系統會導向會議室使用狀況頁面，並預設查詢所選會議室</asp:Label>
                    <asp:Button ID="add" runat="server" Text="新增會議室" CssClass="addbtn" OnClientClick=" return showModal1();" />
                </div>
            </div>
            <div style="padding: 0 31px;">
                <hr style="border-top: 1px solid #e0e0e0;margin: 0;" />
            </div>
            <ul class=" nav nav-tabs" role="tablist" style="width: 100%; background-color: #CCCCCC; border-radius: 15px 15px 0 0; display: none;">
                <li class="nav-item">
                    <asp:Button ID="tab1" runat="server" CssClass="nav-link" CommandArgument="tab1" OnClick="TabButton_Click" data-tab-id="tab1" Text="會議室設備清單" />
                </li>
                <li class="nav-item">
                    <asp:Button ID="tab2" runat="server" CssClass="nav-link" CommandArgument="tab2" OnClick="TabButton_Click" data-tab-id="tab2" Text="常用名稱設定" />
                </li>
            </ul>
            <asp:Panel ID="meetpl" runat="server" style="margin-top: 19px;">
                <%--<div class="write_Title d-flex justify-content-end  pb-1">
                    <asp:Button ID="add" runat="server" Text="新增會議室" CssClass="addbtn" OnClientClick=" return showModal1();" />
                </div>--%>
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <div class="write_textBox pt-0" style="border: 0">
                            <asp:GridView ID="gv" runat="server" CssClass="table table-bordered mt-2" AutoGenerateColumns="False" OnRowCommand="gv_RowCommand"
                                OnRowDataBound="gv_RowDataBound" OnPageIndexChanging="gv_PageIndexChanging" OnRowCreated="gv_RowCreated"
                                OnSorting="gv_Sorting" PageSize="10" AllowPaging="false" AllowSorting="True" DataKeyNames="id" PagerSettings-Visible="false">
                                <EmptyDataTemplate>無資料</EmptyDataTemplate>
                                <HeaderStyle CssClass="table-topbar" />
                                <Columns>
                                    <asp:TemplateField HeaderText="id" ItemStyle-HorizontalAlign="center" ItemStyle-Width="1%" SortExpression="id" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="id" runat="server" Text='<%# Eval("id") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="會議室" ItemStyle-HorizontalAlign="center" ItemStyle-Width="1%" SortExpression="meet_name">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="LinkButton1" runat="server" Text='<%# Eval("meet_name") %>' OnClick="LinkButton1_Click" CommandName='<%# Eval("id") %>'></asp:LinkButton>
                 
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="位置" ItemStyle-HorizontalAlign="center" ItemStyle-Width="1%" SortExpression="meet_location">
                                        <ItemTemplate>
                                            <asp:Label ID="meet_location" runat="server" Text='<%# Eval("meet_location") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="可容納人數" ItemStyle-HorizontalAlign="center" ItemStyle-Width="1%" SortExpression="number">
                                        <ItemTemplate>
                                            <asp:Label ID="number" runat="server" Text='<%# Eval("number") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="硬體設備明細" ItemStyle-HorizontalAlign="left" ItemStyle-Width="5%">
                                        <ItemTemplate>
                                            <asp:Panel ID="gv_p" runat="server" Enabled="false">
                                                <asp:Label ID="equipment" runat="server" Text='<%# Eval("equipment") %>' Visible="false" />
                                                <asp:Label ID="other" runat="server" Text='<%# Eval("other") %>' Visible="false" />
                                                <div class="d-flex flex-wrap  align-items-baseline">
                                                    <asp:CheckBox ID="CheckBox1" runat="server" Text="視訊會議攝影機" CssClass="custom-checkbox" Style="margin-right: 15px;" AutoPostBack="True" OnCheckedChanged="CheckBox_CheckedChanged" />
                                                    <asp:CheckBox ID="CheckBox2" runat="server" Text="便利紙和筆" CssClass="custom-checkbox" Style="margin-right: 15px;" AutoPostBack="True" OnCheckedChanged="CheckBox_CheckedChanged" />
                                                    <asp:CheckBox ID="CheckBox3" runat="server" Text="麥克風" CssClass="custom-checkbox" Style="margin-right: 15px;" AutoPostBack="True" OnCheckedChanged="CheckBox_CheckedChanged" />
                                                    <asp:CheckBox ID="CheckBox4" runat="server" Text="投影機" CssClass="custom-checkbox" Style="margin-right: 15px;" AutoPostBack="True" OnCheckedChanged="CheckBox_CheckedChanged" />
                                                    <asp:CheckBox ID="CheckBox5" runat="server" Text="大型顯示螢幕" CssClass="custom-checkbox" Style="margin-right: 15px;" AutoPostBack="True" OnCheckedChanged="CheckBox_CheckedChanged" />
                                                    <asp:CheckBox ID="CheckBox6" runat="server" Text="白板" CssClass="custom-checkbox" Style="margin-right: 15px;" AutoPostBack="True" OnCheckedChanged="CheckBox_CheckedChanged" />
                                                    <asp:CheckBox ID="CheckBox7" runat="server" Text="電腦" CssClass="custom-checkbox" Style="margin-right: 15px;" AutoPostBack="True" OnCheckedChanged="CheckBox_CheckedChanged" />
                                                    <asp:CheckBox ID="CheckBox8" runat="server" Text="音響系統" CssClass="custom-checkbox" Style="margin-right: 15px;" AutoPostBack="True" OnCheckedChanged="CheckBox_CheckedChanged" />
                                                    <div>
                                                        <asp:CheckBox ID="CheckBox9" runat="server" Text="其他" CssClass="custom-checkbox" AutoPostBack="True" OnCheckedChanged="CheckBox_CheckedChanged" />
                                                        <asp:TextBox ID="gv_other" runat="server" AutoPostBack="True" Style="margin-left: 5px; top: 5px;" OnTextChanged="gv_other_TextChanged" />
                                                    </div>
                                                </div>
                                            </asp:Panel>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="顏色" ItemStyle-HorizontalAlign="center" ItemStyle-Width="0.5%">
                                        <ItemTemplate>
                                            <div style="background-color: <%# Eval("color") %>; border-radius: 11px; cursor: default; width: 30px; height: 30px;"></div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="我的最愛<br><span style='font-size:14px;'>(最多三項)</span>" ItemStyle-HorizontalAlign="center" ItemStyle-Width="0.5%">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="favorite" runat="server" Text="" onclick="onFavoriteChanged(this, event)" />
                                            <asp:Label ID="is_favorite" runat="server" Text='<%# Eval("is_favorite") %>' Visible="false"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-Width="0.5%">
                                        <HeaderTemplate><span>功能</span></HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Button runat="server" CssClass="gw_bt" ID="modify" CommandName="modify" CommandArgument='<%# Eval("id") %>' Text="編輯" Style="background-color: #68B100" ClientIDMode="Static" />
                                            <asp:Button runat="server" CssClass="gw_bt" ID="del" CommandName="del" CommandArgument='<%# Eval("id") %>' Text="刪除" Style="background-color: #B83F1F" OnClientClick="return confirm('確定要刪除嗎？');" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>

                            <div id="down" runat="server" class="btn-toolbar" role="toolbar" aria-label="Toolbar with button groups" style="justify-content: right;display:none">
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
                    </ContentTemplate>
                </asp:UpdatePanel>
                

            </asp:Panel>
            <asp:Panel ID="setpl" runat="server" Visible="false">
                <div class="write_textBox pt-0" style="border: 0">
                    <div class="pt-4">
                        <span style="color: #145597; font-size: 0.9rem">註：各項常用名稱最多僅能設定3個，如有超過，需先刪除已建立的常用名稱，再做新增。</span>
                    </div>
                    <div class="pt-3">
                        <div class="d-flex justify-content-between" style="padding: 0 80px; gap: 80px;">
                            <div class="box">
                                <div class="pl-3 pt-3">
                                    <span style="color: #145597; font-size: 1.1rem; font-weight: bold;">常用會議名稱</span>
                                </div>
                                <div class="pl-3 pt-3 pr-4 d-flex justify-content-between align-items-center" style="gap: 20px;">
                                    <asp:TextBox ID="meet_name_txt" runat="server" Width="90%" CssClass="form-control" placeholder="請輸入常用會議名稱" />
                                    <asp:Button Text="新增" ID="add_name" Style="background: #1885C5" CssClass="gw_bt" runat="server" OnClick="add_name_Click" />
                                </div>
                                <hr style="border: 1px solid #FFFFFF" />
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate>
                                        <asp:Panel ID="name_pl" runat="server"></asp:Panel>
                                    </ContentTemplate>
                                    <Triggers>
                                    </Triggers>
                                </asp:UpdatePanel>
                            </div>
                            <div class="box">
                                <div class="pl-3 pt-3">
                                    <span style="color: #145597; font-size: 1.1rem; font-weight: bold;">常用主持人</span>
                                </div>
                                <div class="pl-3 pt-3 pr-4 d-flex justify-content-between align-items-center" style="gap: 20px;">
                                    <asp:TextBox ID="host_txt" runat="server" Width="90%" CssClass="form-control" placeholder="請輸入常用主持人" />
                                    <asp:Button Text="新增" ID="add_host" Style="background: #1885C5" CssClass="gw_bt" runat="server" OnClick="add_host_Click" />
                                </div>
                                <hr style="border: 1px solid #FFFFFF" />
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                    <ContentTemplate>
                                        <asp:Panel ID="host_pl" runat="server" />
                                    </ContentTemplate>
                                    <Triggers>
                                    </Triggers>
                                </asp:UpdatePanel>
                            </div>

                        </div>
                    </div>
                </div>
            </asp:Panel>
        </div>
    </div>
    <asp:HiddenField ID="HiddenScrollTop" runat="server" />
    <script type="text/javascript">
        window.onload = function () {
            // 頁面載入完，滾到之前的位置
            var hiddenField = document.getElementById('<%= HiddenScrollTop.ClientID %>');
        if (hiddenField && hiddenField.value) {
            window.scrollTo(0, hiddenField.value);
        }
    };

    window.onbeforeunload = function() {
        // 在離開頁面前，記下目前捲軸位置
        var hiddenField = document.getElementById('<%= HiddenScrollTop.ClientID %>');
            if (hiddenField) {
                hiddenField.value = window.pageYOffset || document.documentElement.scrollTop || document.body.scrollTop || 0;
            }
        };
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <div class="modal fade bs-NewUser-modal-lg" id="modal1" tabindex="-1" role="dialog" aria-labelledby="myLargeModalLabel" aria-hidden="true" style="backdrop-filter: brightness(0.3);">
                <div class="modal-dialog modal-lg modal-dialog-centered" style="max-width: 850px;">
                    <div class="modal-content">
                        <div class="modal-header" style="background-color: #C5ECFF; font-size: 22px; display: flex; justify-content: center; align-items: center; position: relative;">
                            <b><span id="modtitle" runat="server" style="color: #003168">新增會議室資料</span></b>
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close" style="position: absolute; right: 10px;">
                                <image src="image/popup_close.png"></image>
                            </button>
                        </div>
                        <div class="modal-body">
                            <div class="MainClass">
                                <div id="content" role="form" class="form-horizontal">
                                    <div class="Box" style="padding: 0px 40px;">
                                        <div class="row" style="margin-bottom: 15px;">
                                            <div class="col-8 d-flex align-items-center">會議室名稱<asp:TextBox ID="m_name" runat="server" CssClass="form-control" Style="width: 80%; margin-left: 10px;" placeholder="請輸入會議室名稱" /></div>
                                        </div>
                                        <div class="row" style="margin-bottom: 15px;">
                                            <div class="col-8 d-flex align-items-center">會議室位置<asp:TextBox ID="m_location" runat="server" CssClass="form-control" Style="width: 80%; margin-left: 10px;" placeholder="請輸入會議室位置" /></div>
                                        </div>
                                        <div class="row" style="margin-bottom: 15px;">
                                            <div class="col-8 d-flex align-items-center">可容納人數<asp:TextBox ID="m_number" runat="server" CssClass="form-control" Style="width: 80%; margin-left: 10px;" placeholder="請輸入人數數量" /></div>
                                        </div>
                                        <hr>
                                    </div>
                                </div>
                                <div class="Box" style="padding: 0px 40px;">
                                    <div class="row" style="margin-bottom: 15px;">
                                        <div class="col-6 d-flex align-items-center">設備明細</div>
                                    </div>
                                    <div class="Box" style="padding: 0px 40px;">
                                        <div class="row">
                                            <asp:CheckBox ID="chk1" runat="server" CssClass="col-4" Text="視訊會議攝影機" />
                                            <asp:CheckBox ID="chk5" runat="server" CssClass="col-4" Text="大型顯示螢幕" />
                                            <asp:CheckBox ID="chk2" runat="server" CssClass="col-4" Text="便條紙和筆" />
                                        </div>
                                        <div class="row">

                                            <asp:CheckBox ID="chk6" runat="server" CssClass="col-4" Text="白板" />
                                            <asp:CheckBox ID="chk3" runat="server" CssClass="col-4" Text="麥克風" />
                                            <asp:CheckBox ID="chk7" runat="server" CssClass="col-4" Text="電腦" />
                                        </div>
                                        <div class="row">
                                            <asp:CheckBox ID="chk4" runat="server" CssClass="col-4" Text="投影機" />
                                            <asp:CheckBox ID="chk8" runat="server" CssClass="col-4" Text="音響系統" />

                                        </div>
                                        <div class="row align-items-baseline">
                                            <asp:CheckBox ID="chk9" runat="server" CssClass="col-2" Text="其他" />
                                            <asp:TextBox ID="other" runat="server" CssClass="form-control" Style="margin-left: -30px; width: 80%" placeholder="請輸入設備名稱" />
                                        </div>
                                    </div>
                                    <hr>
                                </div>
                                <div class="Box" style="padding: 0px 40px;">
                                    <div class="row" style="margin-bottom: 15px;">
                                        <div class="col-6 d-flex align-items-center">會議室顏色</div>
                                    </div>
                                    <div class="row">
                                        <div class="Box d-flex" style="padding: 0px 40px; width: 100%">
                                            <div class="color-box" style="background-color: #3E3AA2;" onclick="toggleSelection(this,'#3E3AA2')"></div>
                                            <div class="color-box" style="background-color: #B24B90;" onclick="toggleSelection(this,'#B24B90')"></div>
                                            <div class="color-box" style="background-color: #744BB2;" onclick="toggleSelection(this,'#744BB2')"></div>
                                            <div class="color-box" style="background-color: #4B85B2;" onclick="toggleSelection(this,'#4B85B2')"></div>
                                            <div class="color-box" style="background-color: #49ACAC;" onclick="toggleSelection(this,'#49ACAC')"></div>
                                            <div class="color-box" style="background-color: #52B24B;" onclick="toggleSelection(this,'#52B24B')"></div>
                                            <div class="color-box" style="background-color: #B2894B;" onclick="toggleSelection(this,'#B2894B')"></div>
                                            <div class="color-box" style="background-color: #2C68B5;" onclick="toggleSelection(this,'#2C68B5')"></div>
                                            <div class="color-box" style="background-color: #797D00;" onclick="toggleSelection(this,'#797D00')"></div>
                                            <div class="color-box" style="background-color: #B26B4B;" onclick="toggleSelection(this,'#B26B4B')"></div>


                                        </div>
                                        <div class="Box d-flex" style="padding: 0px 40px; width: 100%">
                                            <div class="color-box" style="background-color: #2F2D4E;" onclick="toggleSelection(this,'#2F2D4E')"></div>
                                            <div class="color-box" style="background-color: #650245;" onclick="toggleSelection(this,'#650245')"></div>
                                            <div class="color-box" style="background-color: #39008E;" onclick="toggleSelection(this,'#39008E')"></div>
                                            <div class="color-box" style="background-color: #00457A;" onclick="toggleSelection(this,'#00457A')"></div>
                                            <div class="color-box" style="background-color: #346363;" onclick="toggleSelection(this,'#346363')"></div>
                                            <div class="color-box" style="background-color: #076100;" onclick="toggleSelection(this,'#076100')"></div>
                                            <div class="color-box" style="background-color: #7B4900;" onclick="toggleSelection(this,'#7B4900')"></div>
                                            <div class="color-box" style="background-color: #021F46;" onclick="toggleSelection(this,'#021F46')"></div>
                                            <div class="color-box" style="background-color: #383A00;" onclick="toggleSelection(this,'#383A00')"></div>
                                            <div class="color-box" style="background-color: #654638;" onclick="toggleSelection(this,'#654638')"></div>
                                            <asp:HiddenField ID="SelectedColor" runat="server" />
                                            <asp:HiddenField ID="hf_id" runat="server" />
                                        </div>
                                    </div>
                                    <hr>
                                </div>
                                <div style="text-align: center">
                                    <asp:Button ID="cancel" runat="server" Text="取消" class="bt" Style="background: #777777"
                                        OnClientClick="return hideModal1();" />
                                    <asp:Button ID="submit" runat="server" Text="確認新增" class="bt" OnClick="submit_Click" />
                                    <asp:Button ID="update" runat="server" Text="儲存" class="bt" OnClick="submit_Click" />
                                </div>

                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    
</asp:Content>
