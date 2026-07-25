<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage3.master" AutoEventWireup="true" CodeFile="Meetingroom_listquery.aspx.cs" Inherits="Meetingroom_listquery" MaintainScrollPositionOnPostback="true" %>

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
        function showModal3() {
            $('#modal3').modal('show');
            return false;
        }
        function hideModal3() {
            $('#modal3').modal('hide');
            return false;
        }

        function showModal4() {
            $('#modal4').modal('show');
            return false;
        }
        function hideModal4() {
            $('#modal4').modal('hide');
            return false;
        }



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
        function fetchMeetingNames() {
            $.ajax({
                type: 'POST',
                url: 'Meetingroom_calendar.aspx/GetMeetingNames',
                data: '{}',
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (response) {
                    var favoriteNames = response.d;
                    var namesArray = favoriteNames.split(',');
                    namesArray = namesArray.slice(0, 3);

                    var container = document.getElementById('favorite');
                    container.innerHTML = '';
                    var i = 0
                    namesArray.forEach(function (name) {
                        i++
                        var button = document.createElement('button');
                        button.innerHTML = i + ".　" + name.trim();
                        button.className = 'favoritebt';
                        button.onclick = function (event) {
                            setmeetValue(name.trim());
                            event.preventDefault();
                        };
                        container.appendChild(button);
                    });
                },
                error: function (xhr, status, error) {
                }
            });
            var mdtitle = document.getElementById('<%=md3span.ClientID%>');
            mdtitle.innerText = '常用會議名稱'

            showModal3();
            return false;
        }
        function fetchHostNames() {
            $.ajax({
                type: 'POST',
                url: 'Meetingroom_calendar.aspx/GetHostNames',
                data: '{}',
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (response) {
                    var favoriteNames = response.d;
                    var namesArray = favoriteNames.split(',');
                    namesArray = namesArray.slice(0, 3);

                    var container = document.getElementById('favorite');
                    container.innerHTML = '';
                    var i = 0
                    namesArray.forEach(function (name) {
                        i++
                        var button = document.createElement('button');
                        button.innerHTML = i + ".　" + name.trim();
                        button.className = 'favoritebt';
                        button.onclick = function (event) {
                            setHostValue(name.trim());
                            event.preventDefault();
                        };
                        container.appendChild(button);
                    });
                },
                error: function (xhr, status, error) {
                }
            });
            var mdtitle = document.getElementById('<%=md3span.ClientID%>');
            mdtitle.innerText = '常用主持人'




            showModal3();
            return false;
        }

        function setmeetValue(buttonText) {
            var textBox = document.getElementById('<%= m_no.ClientID %>');
            textBox.value = buttonText;
            $('#modal3').modal('hide');
            return false;
        }
        function setHostValue(buttonText) {
            var textBox = document.getElementById('<%= host.ClientID %>');
            textBox.value = buttonText;
            $('#modal3').modal('hide');
            return false;
        }

        // 點擊開始時間 TextBox 時顯示下拉選單
        function toggleTimeDropdown() {
            let dropdown = document.getElementById('timeDropdownStart');
            let textBox = document.getElementById('<%= starthour.ClientID %>');

            // 如果下拉選單已顯示，則隱藏它，否則顯示
            if (dropdown.style.display === 'block') {
                dropdown.style.display = 'none';
            } else {
                dropdown.style.display = 'block';

                // 確保下拉選單的位置與 TextBox 一致
                dropdown.style.top = textBox.offsetTop + textBox.offsetHeight + 'px';
                dropdown.style.left = textBox.offsetLeft + 'px';
            }
        }

        // 點擊結束時間 TextBox 時顯示下拉選單
        function toggleTimeDropdown2() {
            let dropdown = document.getElementById('timeDropdownEnd');
            let textBox = document.getElementById('<%= endhour.ClientID %>');

            // 如果下拉選單已顯示，則隱藏它，否則顯示
            if (dropdown.style.display === 'block') {
                dropdown.style.display = 'none';
            } else {
                dropdown.style.display = 'block';

                // 確保下拉選單的位置與 TextBox 一致
                dropdown.style.top = textBox.offsetTop + textBox.offsetHeight + 'px';
                dropdown.style.left = textBox.offsetLeft + 'px';
            }
        }

        // 當用戶選擇一個時間選項時，將選中的時間填入開始時間 TextBox
        function setTimeFromDropdown() {
            let select = document.getElementById('<%= timeSelectStart.ClientID %>');
            let selectedTime = select.value;
            let textBox = document.getElementById('<%= starthour.ClientID %>');
            // 將選擇的時間填入 TextBox
            textBox.value = selectedTime;

            // 隱藏下拉選單
            document.getElementById('timeDropdownStart').style.display = 'none';
        }

        // 當用戶選擇一個時間選項時，將選中的時間填入結束時間 TextBox
        function setTimeFromDropdown2() {
            let select = document.getElementById('<%= timeSelectEnd.ClientID %>');
            let selectedTime = select.value;
            let textBox = document.getElementById('<%= endhour.ClientID %>');

            // 將選擇的時間填入 TextBox
            textBox.value = selectedTime;

            // 隱藏下拉選單
            document.getElementById('timeDropdownEnd').style.display = 'none';
        }

        // 點擊下拉選單外部區域時，隱藏下拉選單
        document.addEventListener('click', function (event) {
            let dropdownStart = document.getElementById('timeDropdownStart');
            let dropdownEnd = document.getElementById('timeDropdownEnd');
            let textBoxStart = document.getElementById('<%= starthour.ClientID %>');
            let textBoxEnd = document.getElementById('<%= endhour.ClientID %>');

            // 如果點擊的區域不在 TextBox 或下拉選單內，則隱藏下拉選單
            if (!textBoxStart.contains(event.target) && !dropdownStart.contains(event.target)) {
                dropdownStart.style.display = 'none';
            }
            if (!textBoxEnd.contains(event.target) && !dropdownEnd.contains(event.target)) {
                dropdownEnd.style.display = 'none';
            }
        });


    </script>
    <style>
        .modal-backdrop {
            display: none;
        }

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

        /*    .formItem {
            margin-left: 0 !important;
        }*/

        /*input[readonly] {
            background-color: white !important;*/ /* 讓背景保持白色 */
        /*color: black !important;*/ /* 確保文字顏色是黑色 */
        /*}*/

        .ItemContent label {
            margin-bottom: 0;
            margin-right: 10px;
        }

        .ItemContent input[type=checkbox], input[type=radio] {
            margin-right: 5px;
        }

        #ContentPlaceHolder1_date_note
        {
            top:80%;
        }

        /* 日期時間區域設置 */
        .date-time-wrapper {
            display: flex;
            width: 45%; /* 可以調整寬度比例 */
            max-width: 45%;
        }
        /* 時間下拉選單的樣式 */
        .time-dropdown {
            display: none;
            position: absolute;
            z-index: 9999;
            width: 20%;
        }

            .time-dropdown select {
                width: 100%;
                height: auto;
                font-size: 14px;
                padding: 5px;
            }

        @media (max-width: 1680px) {
            .modal-lg {
                max-width: 70% !important; /* 屏幕較小時，讓元素垂直排列 */
            }

            .safe_num_wid
            {
                width:100% !important;
            }

            #ContentPlaceHolder1_date_note
            {
                top:100% !important;
            }
        }

        .custom-checkbox label {
            margin-right: 8px; /* 使用 !important 確保樣式生效 */
        }

        /*louis*/
        .safe_num_wid
        {
            width:86%;
        }

        .addbt {
            background: #EF9103 0% 0% no-repeat padding-box;
            box-shadow: 0px 3px 6px #00000029;
            border: 0px;
            border-radius: 10px;
            opacity: 1;
            color: #FFFFFF;
            position: absolute;
            top: 0;
            right: 0;
            white-space: pre-line;
            width: 80%;
        }

        .gray_box {
            width: 80%;
            border-radius: 3px;
            background-color: #EFEFEC;
            border: 0;
            color: #000000;
            padding: 5px 15px;
            height: 50px;
            display: flex;
            align-items: center;
            font-size: 1rem;
            font-weight: bold;
            text-align: left;
            align-items: center;
        }
        /*louis*/

        .pt {
          overflow: hidden;
            text-overflow: ellipsis;
            white-space: nowrap;
            max-width:6rem;
        }

        .pt:hover {
            overflow: initial;
            text-overflow: initial;
            white-space: initial;
            z-index: 5;
        }

        /*jack*/
        .recommandbt {
            width: 119px;
            height: 33px;
            background: #7C73E6 0% 0% no-repeat padding-box;
            border-radius: 3px;
            border: 0px;
            color: #FFFFFF;
            opacity: 1;
            background-image: url(../image/icon_46_30.png);
            background-position: left center;
            padding-left: 25px;
        }

        .button-container {
            position: relative; /* 確保浮動 div 可以相對於這個容器定位 */
            display: flex;
            align-items: center;
            margin-right:0.625rem;
        }

        .floating-div {
            display:none;
            position: absolute;
            right: 100%; /* 將浮動 div 的右邊固定在按鈕的左邊 */
            bottom: 0; /* 將浮動 div 的底部對齊按鈕的底部 */
            width: 400px; /* 設置浮動 div 的寬度 */
            background-color: #f0f0f0; /* 背景色，方便看到效果 */
            border: 1px solid #ccc;
            margin-right: 10px; /* 與按鈕之間的間距 */
            /*display: flex;*/
            align-items: center;
            justify-content: center;
            border-radius:5px;
        }

        .E_box {
            background: #56A6BF 0% 0% no-repeat padding-box;
            border-radius: 17px;
            height: 33px;
            font: normal normal bold 1em Microsoft JhengHei;
            color: #FFFFFF;
            align-content: center;
            padding-right: 10px;
            padding-left: 10px;
            border: 0;
            margin:0 0.625rem 0.625rem 0;
        }
        /*jack*/
   
    </style>
    <script>
        function toggleActiveAndDisplay(id) {
            var $el = $('#' + id);
            $el.hide();
        }
        function showRecommandMeetingRoomModal() {
            $('#ContentPlaceHolder1_RecommandMeetingRoomModal').css("display", "block");
        }
        function hideRecommandMeetingRoomModal() {
            $('#ContentPlaceHolder1_RecommandMeetingRoomModal').css("display", "none");
        }
    </script>
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div class="d-flex">
        <div class="write_Box" style="width: 87.6%;">
            <div class="write_Title">
                <h5>
                    <asp:Literal runat="server" ID="box_title">清單篩選</asp:Literal></h5>
            </div>
            <div class="write_textBox">
                <div class="dataBox row">
                          <div id="group" runat="server" class="col-xl-3 col-sm-6 formItem title4 ml-0 pr-4" style="margin: 0px;">
                        <div class="ItemTitle"><span>申請組室</span></div>
                        <div class="ItemContent">
                            <asp:DropDownList ID="ddl3" runat="server" CssClass="form-control" Style="width: 100%">
                                <asp:ListItem Value="0">全部</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>



                  <div id="key" runat="server" class=" col-xl-8 col-lg-10 col-md-11 formItem title4" style="margin-left: 0px;">
                        <div class="ItemTitle"><span>關鍵字查詢</span></div>
                        <div class="ItemContent" style="display: flex; align-items: center;">
                            <asp:TextBox ID="keyword" runat="server" CssClass="form-control" Style="width: 100%" placeholder="請輸入申請人、主持人、會議名稱、會議室或其他關鍵字來做查詢"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-xl-3 col-sm-4 formItem title4 pr-4" style="display:none">
                        <div class="ItemTitle"><span>呈現內容</span></div>
                        <div class="ItemContent">
                            <asp:DropDownList ID="ddl1" runat="server" CssClass="form-control" Style="width: 90%;">
                                <asp:ListItem Value="0">全部</asp:ListItem>
                                <asp:ListItem Value="1">我的申請</asp:ListItem>
                                <asp:ListItem Value="2">組室申請</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>

                </div>
                <hr>
                <div class="dataBox row">
                     <div class="col-xl-4  col-sm-6 formItem title4 pr-5 ml-0">
                        <div class="ItemTitle"><span>開會地點</span></div>
                        <div class="ItemContent">
                            <asp:DropDownList ID="ddl2" runat="server" CssClass="form-control" Style="width: 100%">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                   <div class="dataBox row">
                    <div class="col-xl-6 col-sm-12 formItem title4" style="margin: 0px; white-space: nowrap;">
                        <div class="ItemTitle"><span>電子會議</span></div>
                        <div class="ItemContent ">
                            <div class="d-flex flex-wrap">
                                <div style="flex-wrap: nowrap">
                                    <asp:RadioButton ID="rb1" Checked="true" GroupName="online" runat="server" Text="全選" />
                                </div>
                                <div style="flex-wrap: nowrap">
                                    <asp:RadioButton ID="rb2" GroupName="online" runat="server" Text="是" />
                                </div>
                                <div style="flex-wrap: nowrap">
                                    <asp:RadioButton ID="rb3" GroupName="online" runat="server" Text="否" />
                                </div>
                            </div>
                        </div>
                    </div>
                          </div>
                   <div class="dataBox row">
                    <div class="col-4 formItem title4" style="margin-left:0px">
                        <div class="ItemTitle"><span>可容納人數</span></div>
                        <div class="ItemContent" style="display: flex; align-items: center; flex-wrap: wrap;">
                            <asp:TextBox ID="safe_num" runat="server" CssClass="form-control safe_num_wid" placeholder="最多輸入4位數"></asp:TextBox>
                        </div>
                    </div>
  

                </div>
                  <div class="dataBox row">
                      <div class="col-12 formItem title4" style="margin-left: 0px!important;">
                        <div class="ItemTitle"><span>硬體設備</span></div>
                        <div class="ItemContent" style="display: flex; align-items: center; flex-wrap: wrap;">
                            <div class="d-flex flex-wrap  align-items-baseline">
                                <asp:UpdatePanel runat="server">
                                    <ContentTemplate>
                                        <asp:CheckBox ID="chk0" runat="server" Text="全部" CssClass="custom-checkbox" AutoPostBack="True" OnCheckedChanged="chk0_CheckedChanged" />
                                        <asp:CheckBox ID="chk1" runat="server" Text="視訊會議攝影機" CssClass="custom-checkbox" AutoPostBack="True" />
                                        <asp:CheckBox ID="chk2" runat="server" Text="便利紙和筆" CssClass="custom-checkbox" AutoPostBack="True" />
                                        <asp:CheckBox ID="chk3" runat="server" Text="麥克風" CssClass="custom-checkbox" AutoPostBack="True" />
                                        <asp:CheckBox ID="chk4" runat="server" Text="投影機" CssClass="custom-checkbox" AutoPostBack="True" />
                                        <asp:CheckBox ID="chk5" runat="server" Text="大型顯示螢幕" CssClass="custom-checkbox" AutoPostBack="True" />
                                        <asp:CheckBox ID="chk6" runat="server" Text="白板" CssClass="custom-checkbox" AutoPostBack="True" /><br>
                                        <asp:CheckBox ID="chk7" runat="server" Text="電腦" CssClass="custom-checkbox" AutoPostBack="True" />
                                        <asp:CheckBox ID="chk8" runat="server" Text="音響系統" CssClass="custom-checkbox" AutoPostBack="True" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                    </div>


                       </div>
                <div class="dataBox row" style="">
                    <div style="display: flex; align-items: center; flex-wrap: wrap;">
                        <div class="col-12 formItem title4">
                            <div class="ItemTitle"><span>使用日期</span></div>
                            <div class="ItemContent" style="position: relative;display: flex; align-items: center; flex-wrap: wrap;">
                                <asp:RadioButton ID="rb_today" runat="server" GroupName="receivedate" Text="今天" Style="margin-right: 5px;" AutoPostBack="true" OnCheckedChanged="rb_SelectedIndexChanged" />
                                <asp:RadioButton ID="rb_thisWeek" runat="server" GroupName="receivedate" Text="本周" Style="margin-right: 5px; margin-left: 10px;" AutoPostBack="true" OnCheckedChanged="rb_SelectedIndexChanged" />
                                <asp:RadioButton ID="rb_nextMonth" Checked="true" runat="server" GroupName="receivedate" Text="未來一個月" Style="margin-right: 5px; margin-left: 10px;" AutoPostBack="true" OnCheckedChanged="rb_SelectedIndexChanged" />
                                <asp:RadioButton ID="rb_lastMonth" runat="server" GroupName="receivedate" Text="前一個月" Style="margin-right: 10px; margin-left: 10px;" AutoPostBack="true" OnCheckedChanged="rb_SelectedIndexChanged" />
                                <div class="d-flex align-items-center">
                                    <asp:RadioButton ID="rb_customRange" runat="server" GroupName="receivedate" Text="自訂區間" Style="margin-right: 0px;" AutoPostBack="true" OnCheckedChanged="rb_SelectedIndexChanged" />
                                    <div style="flex: auto;">
                                        <asp:TextBox ID="receive_start" runat="server" TextMode="Date" CssClass="form-control" Style="width: 40%; height: 35px; margin-left: 0px;" Enabled="false" OnChange="setEndDateRange()"></asp:TextBox>
                                        <span class="right" style="font-size: 22px; margin: 0 5px;">～</span>
                                        <asp:TextBox ID="receive_end" runat="server" TextMode="Date" CssClass="form-control" Style="width: 40%; height: 35px; margin-left: 8px;" Enabled="false" OnChange="setStartDateRange()"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <asp:Label runat="server" ID="date_note" class="note" style="position: absolute; bottom: 0;margin-left:5.5rem;">註：使用日期查詢後可自動帶入日曆檢視</asp:Label>
                        </div>
                    </div>
                    
                </div>
                
                <div  class="dataBox row">
                
                </div>
            </div>
            <div class="write_textBox d-flex justify-content-center">
                <asp:Button ID="searchbt" runat="server" Text="查詢" CssClass="searchbt" OnClick="searchbt_Click" />
                <asp:Label runat="server" ID="test"></asp:Label>
            </div>
        </div>

        <div id="div_add" runat="server" style="width: 12.4%; position: relative;">
            <asp:Button ID="addpurchase" runat="server" Text="建立&#10;會議室申請"
                CssClass="ml-auto addbt" Style="white-space: pre-line;"
                OnClick="addmeeting_Click" UseSubmitBehavior="false" />
        </div>
    </div>

    <div class="write_Box">
        <div class="write_Title d-flex justify-content-between" style="align-items:center;">
            <h5>
                <asp:Literal runat="server" ID="Literal1">借用清單</asp:Literal></h5>
            <div style="display:flex;">
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <div class="button-container">
                            <asp:Panel runat="server" id="RecommandMeetingRoomModal" class="floating-div">
                                <div style="background:#7C73E6;text-align:center;">
                                    <span style="font-weight:bold;font-size:16px;color:#FFFFFF">推薦會議室</span>
                                    <button type="button" class="close" data-dismiss="modal" style="position:absolute;right:2%" onclick="hideRecommandMeetingRoomModal()">
                                        <img src="image/icon_47.png" style="position:absolute; top:10%; transform:translateY(25%); right:10%;" />
                                    </button>
                                </div>

                                <div style="padding:5px;">
                                    <asp:UpdatePanel runat="server">
                                        <ContentTemplate>
                                            <asp:Panel runat="server" ID="QualifiedMeetingRoom"></asp:Panel>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                                <div style="text-align:center;padding-bottom:5px;display: flex;justify-content: center;">
                                    <table style="font-size:14px;color:#777777">
                                        <tr>
                                            <td>註：</td><td style="text-align: left;">1.點選會議室名稱，可跳至申請畫面</td>
                                        </tr>
                                        <tr>
                                           <td></td> <td>2.推薦會議室將依據篩選條件而有所不同</td>
                                        </tr>
                                    </table>
                                </div>
                            </asp:Panel>
                            <asp:Button ID="RecommandMeetingRoom" runat="server" Text="推薦會議室" CssClass="recommandbt" OnClick="RecommandMeetingRoom_Click" />
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <asp:Button ID="export" runat="server" Text="匯出總表" style="margin-right:0.625rem;" CssClass="exportbt" OnClick="export_Click" />
                <asp:Button ID="export_decrement" runat="server" Text="匯出減量表" CssClass="exportbt" OnClientClick="return showModal4();" />
            </div>
        </div>
        <div class="write_textBox">
            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                <ContentTemplate>
                    <asp:GridView ID="gv" runat="server" CssClass="table table-bordered mt-2" AutoGenerateColumns="False" OnRowCommand="gv_RowCommand"
                        OnRowDataBound="gv_RowDataBound" OnPageIndexChanging="gv_PageIndexChanging" OnRowCreated="gv_RowCreated"
                        OnSorting="gv_Sorting" PageSize="10" AllowPaging="true" AllowSorting="True" DataKeyNames="id" PagerSettings-Visible="false">
                        <EmptyDataTemplate>無資料</EmptyDataTemplate>
                        <HeaderStyle CssClass="table-topbar" />
                        <Columns>
                            <asp:TemplateField HeaderText="id" ItemStyle-HorizontalAlign="center" ItemStyle-Width="0.5%" SortExpression="id" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="id" runat="server" Text='<%# Eval("id") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText='使用日期<span class="fa fa-caret-down"/>' ItemStyle-HorizontalAlign="center" ItemStyle-Width="10%" SortExpression="use_start">
                                <ItemTemplate>
                                    <asp:Label ID="use_start" runat="server" Text='<%# Eval("use_start","{0:yyyy/MM/dd}") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText='使用時間<span class="fa fa-caret-down"/>' ItemStyle-HorizontalAlign="center" ItemStyle-Width="10%" SortExpression="time_range">
                                <ItemTemplate>
                                    <asp:Label ID="time_range" runat="server" Text='<%# Eval("time_range") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText='會議室<span class="fa fa-caret-down"/>' ItemStyle-HorizontalAlign="center" ItemStyle-Width="10%" SortExpression="meet_name">
                                <ItemTemplate>
                                    <asp:Label ID="meet_name" runat="server" Text='<%# Eval("meet_name") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText='申請組室<span class="fa fa-caret-down"/>' ItemStyle-HorizontalAlign="center" ItemStyle-Width="10%" SortExpression="appr_group">
                                <ItemTemplate>
                                    <asp:Label ID="appr_group" runat="server" Text='<%# Eval("appr_group") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText='會議名稱<span class="fa fa-caret-down"/>' ItemStyle-HorizontalAlign="center" ItemStyle-Width="10%" SortExpression="meeting_name" ItemStyle-CssClass="pt">
                                <ItemTemplate>
                                    <asp:Label ID="meeting_name" runat="server" Text='<%# Eval("meeting_name") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText='可容納人數<span class="fa fa-caret-down"/>' ItemStyle-HorizontalAlign="center" ItemStyle-Width="10%" SortExpression="number">
                                <ItemTemplate>
                                    <asp:Label ID="number" runat="server" Text='<%# Eval("number") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText='申請者<span class="fa fa-caret-down"/>' ItemStyle-HorizontalAlign="center" ItemStyle-Width="10%" SortExpression="appr_user">
                                <ItemTemplate>
                                    <asp:Label ID="appr_user" runat="server" Text='<%# Eval("appr_user") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText='主持人<span class="fa fa-caret-down"/>' ItemStyle-HorizontalAlign="center" ItemStyle-Width="10%" SortExpression="host">
                                <ItemTemplate>
                                    <asp:Label ID="host" runat="server" Text='<%# Eval("host") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText='出席人數<span class="fa fa-caret-down"/>' ItemStyle-HorizontalAlign="center" ItemStyle-Width="10%" SortExpression="Attendnumber">
                                <ItemTemplate>
                                    <asp:Label ID="Attendnumber" runat="server" Text='<%# Eval("Attendnumber") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText='會議類型<span class="fa fa-caret-down"/>' ItemStyle-HorizontalAlign="center" ItemStyle-Width="10%" SortExpression="meetclass">
                                <ItemTemplate>
                                    <asp:Label ID="meetclass" runat="server" Text='<%# Eval("meetclass") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="10%">
                                <HeaderTemplate><span>功能</span></HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Button runat="server" CssClass="gw_bt" ID="check" CommandName="check" CommandArgument='<%# Eval("id") %>' Text="檢視" Style="background-color: #1885C5" UseSubmitBehavior="false" />
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
                </ContentTemplate>
                <Triggers>
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>


</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">

             
            <div class="modal fade bs-NewUser-modal-lg" id="modal1" tabindex="-1" role="dialog" aria-labelledby="myLargeModalLabel" aria-hidden="true" style="backdrop-filter: brightness(0.3);overflow-y: auto;">
                <div class="modal-dialog modal-lg modal-dialog-centered" style="max-width: 55%">
                    <div class="modal-content">
                        <div class="modal-header" style="background-color: #C5ECFF; font-size: 22px; display: flex; justify-content: center; align-items: center; position: relative;">
                            <b><span id="modtitle" runat="server" style="color: #003168">建立會議室申請</span></b>
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close" style="position: absolute; right: 10px;">
                                <image src="image/popup_close.png"></image>
                            </button>
                        </div>
                        <div class="modal-body">
                            <div class="MainClass">
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
                                <div id="content" role="form" class="form-horizontal">
                                    <div class="Box" style="padding: 0px 40px;">
                                        <div class="row" style="margin-bottom: 10px;">
                                            <div class=" col-8 formItem title3">
                                                <div class="ItemTitle"><span>會議名稱<span style="color: #ff0000">*</span></span></div>
                                                <div class="ItemContent d-flex align-items-center">
                                                    <asp:TextBox ID="m_no" runat="server" CssClass="form-control" Style="width: 85%; margin-right: 10px;" />
                                                    <asp:HiddenField ID="hf_id" runat="server" />
                                                    <asp:ImageButton ID="mnbt" ImageUrl="~/image/icon_24.png" ToolTip="常用會議名稱" runat="server" CommandArgument="name" OnClick="favorite_Click" />
                                                    <%-- <asp:ImageButton ID="mnbt" ImageUrl="~/image/icon_24.png" ToolTip="常用會議名稱" runat="server" UseSubmitBehavior="false" OnClientClick="return fetchMeetingNames()" />--%>
                                                    <%--<asp:Button ID="mnbt" runat="server" Text="常用會議名稱" CssClass="bt" Style="width: auto; height: auto; padding: 5px 10px; background-color: #1885C5" UseSubmitBehavior="false" OnClientClick="return fetchMeetingNames()" />--%>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row" style="margin-bottom: 10px; width: 100%">
                                            <div class=" col-12 formItem title3">
                                                <div class="ItemTitle"><span>會議時間<span style="color: #ff0000">*</span></span></div>
                                                <div class="ItemContent " style="max-width: 100%;">
                                                    <asp:UpdatePanel ID="UpdatePanel3" runat="server" class="d-flex justify-content-between align-items-center flex-wrap" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <div class="date-time-wrapper">
                                                                <asp:TextBox ID="startdate" runat="server" CssClass="form-control" Style="width: 100%; margin-right: 5px;" TextMode="Date" />
                                                                <asp:TextBox ID="starthour" runat="server" CssClass="form-control" Style="width: 100%;background:#FFFFFF;" TextMode="Time" ReadOnly="true" onclick="toggleTimeDropdown()" />
                                                                <div id="timeDropdownStart" class="time-dropdown" style="display: none;">
                                                                    <asp:DropDownList ID="timeSelectStart" runat="server" OnSelectedIndexChanged="timeSelectStart_SelectedIndexChanged" size="10" AutoPostBack="true" />
                                                                </div>
                                                            </div>
                                                            <span class="text-center">~</span>
                                                            <div class="date-time-wrapper">
                                                                <asp:TextBox ID="enddate" runat="server" CssClass="form-control" Style="width: 100%; margin-right: 5px;" TextMode="Date" />
                                                                <asp:TextBox ID="endhour" runat="server" CssClass="form-control" Style="width: 100%;background:#FFFFFF;" TextMode="Time" ReadOnly="true" onclick="toggleTimeDropdown2()" />
                                                                <div id="timeDropdownEnd" class="time-dropdown" style="display: none;">
                                                                    <asp:DropDownList ID="timeSelectEnd" runat="server" OnSelectedIndexChanged="timeSelectEnd_SelectedIndexChanged" size="10" AutoPostBack="true" />
                                                                </div>
                                                            </div>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="timeSelectStart" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="timeSelectEnd" EventName="SelectedIndexChanged" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                        <hr>
                                    </div>
                                    <div class="Box" style="padding: 0px 40px;">
                                        <div class="row" style="margin-bottom: 10px;">
                                            <div class=" col-5 formItem title3">
                                                <div class="ItemTitle"><span>申請組室<span style="color: #ff0000">*</span></span></div>
                                                <div class="ItemContent">
                                                    <asp:TextBox ID="appr_group" CssClass="form-control" runat="server" Style="width: 100%;" Enabled="false" />
                                                </div>
                                            </div>
                                            <div class=" col-5 formItem title3">
                                                <div class="ItemTitle"><span>申請人<span style="color: #ff0000">*</span></span></div>
                                                <div class="ItemContent">
                                                    <asp:TextBox ID="appr_user" CssClass="form-control" runat="server" Style="width: 80%;" Enabled="false" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row" style="margin-bottom: 10px;">
                                            <div class="col-8 formItem  title3">
                                                <div class="ItemTitle"><span>會議室<span style="color: #ff0000">*</span></span></div>
                                                <div class="ItemContent">
                                                    <div class=" d-flex align-items-center" style="width: 100%;">
                                                        <asp:UpdatePanel ID="UpdatePanel4" runat="server" style="width: 54%; display: flex; flex-direction: column">
                                                            <ContentTemplate>
                                                                <asp:DropDownList ID="mtroom" runat="server" class="form-control" Style="width: 100%;" OnSelectedIndexChanged="mtroom_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                        <span style="font-size: 14px; margin-left: 5px; white-space: nowrap;">註：標註★表示是我的最愛會議室</span>

                                                    </div>
                                                </div>
                                            </div>
                                            <div class=" col-4 formItem title3 ml-0">
                                                <div class="ItemTitle"><span>主持人<span style="color: #ff0000">*</span></span></div>
                                                <div class="ItemContent d-flex align-items-center">
                                                    <asp:TextBox ID="host" class="form-control" runat="server" Style="width: 85%; margin-right: 15px;" />
                                                    <asp:ImageButton ID="hostbt" ImageUrl="~/image/icon_24.png" ToolTip="常用主持人" runat="server" CommandArgument="host" OnClick="favorite_Click" />
                                                    <%--   <asp:ImageButton ID="hostbt" ImageUrl="~/image/icon_24.png" ToolTip="常用主持人" runat="server" UseSubmitBehavior="false" OnClientClick="return fetchHostNames()" />--%>
                                                    <%--  <asp:Button ID="hostbt" runat="server" Text="常用主持人" CssClass="bt" Style="width: auto; height: auto; padding: 5px 10px; background-color: #1885C5" UseSubmitBehavior="false" OnClientClick="return fetchHostNames()" />--%>
                                                </div>
                                            </div>
                                            <asp:Panel runat="server" ID="mrdevices" Visible="false" style="width:70%; padding-left:5.875rem;">
                                                <asp:Label ID="room_equipment" runat="server" Text="" Style="text-align: left; letter-spacing: 0px; color: #375471; font-size: 14px; " />
                                            </asp:Panel>
                                        </div>

                                        <hr>
                                    </div>

                                    <div class="Box" style="padding: 0px 40px;">
                                        <asp:UpdatePanel runat="server">
                                        <ContentTemplate>                              
                                        <div class="row" style="margin-bottom: 10px;">
                                            <div class=" col-5 formItem title3">
                                                <div class="ItemTitle"><span>會議類型<span style="color: #ff0000">*</span></span></div>
                                                <div class="ItemContent d-flex align-items-center">
                                                    <%--<asp:DropDownList ID="meetclass" runat="server" class="form-control" Style="width: 100%;">
                                                        <asp:ListItem Value="0">請選擇</asp:ListItem>
                                                        <asp:ListItem Value="1">研討會</asp:ListItem>
                                                        <asp:ListItem Value="2">發表會</asp:ListItem>
                                                        <asp:ListItem Value="3">股東大會</asp:ListItem>
                                                        <asp:ListItem Value="4">訓練講習會</asp:ListItem>
                                                        <asp:ListItem Value="5">聯誼活動</asp:ListItem>
                                                        <asp:ListItem Value="6">記者會</asp:ListItem>
                                                        <asp:ListItem Value="7">其他</asp:ListItem>
                                                        <asp:ListItem Value="8">招(開)標</asp:ListItem>
                                                        <asp:ListItem Value="9">電影欣賞</asp:ListItem>
                                                        <asp:ListItem Value="10">例行性會議</asp:ListItem>
                                                        <asp:ListItem Value="11">視訊會議</asp:ListItem>
                                                    </asp:DropDownList>--%>
                                                    <asp:DropDownList ID="meetclass" runat="server" CssClass="form-control" Style="width: 100%;"  DataSourceID="meetclass_sd"  DataTextField="meetingtype_name"  DataValueField="meeting_meetingtype_id"  AppendDataBoundItems="true"> 
                                                    </asp:DropDownList>
                                                    <asp:ImageButton ID="meeting_type_bt" Visible="false" ImageUrl="~/image/icon_24.png" ToolTip="會議類型" runat="server" CommandArgument="meetingtype" OnClick="meeting_item_Click" />
                                                    <asp:SqlDataSource runat="server" ID="meetclass_sd" ConnectionString="<%$ ConnectionStrings:eip %>" SelectCommand="SELECT 0 AS meeting_meetingtype_id, '請選擇' AS meetingtype_name UNION ALL select meeting_meetingtype_id,meetingtype_name from meeting_apply_meetingtype"></asp:SqlDataSource>
                                                </div>
                                            </div>
                                            <div class=" col-5 formItem title3">
                                                <div class="ItemTitle"><span>電子會議</span></div>
                                                <div class="ItemContent">
                                                    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                                        <ContentTemplate>
                                                            <asp:CheckBox ID="ck_yes" runat="server" Text="是" AutoPostBack="true" />
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                        </ContentTemplate>
                                        </asp:UpdatePanel>

                                         <div class="">
                                            <asp:UpdatePanel ID="UpdatePanel6" class="row" runat="server" style="margin-bottom: 10px;">
                                                <ContentTemplate>
                                                    <div class=" col-5 formItem title3">
                                                        <div class="ItemTitle"><span>使用類型<span style="color: #ff0000">*</span></span></div>
                                                        <div class="ItemContent d-flex align-items-center">
                                                            <asp:DropDownList ID="useclass" runat="server" CssClass="form-control" Style="width: 100%;" OnSelectedIndexChanged="useclass_SelectedIndexChanged" AutoPostBack="true" DataSourceID="useclass_sd"  DataTextField="usedtype_name"  DataValueField="meeting_usedtype_id"  AppendDataBoundItems="true">
                                                                <%--<asp:ListItem Value="0">請選擇</asp:ListItem>--%>
                                                               <%-- <asp:ListItem Value="1">自用</asp:ListItem>
                                                                <asp:ListItem Value="2">租用</asp:ListItem>
                                                                <asp:ListItem Value="3">共同辦理</asp:ListItem>
                                                                <asp:ListItem Value="4">協辦</asp:ListItem>
                                                                <asp:ListItem Value="5">其他</asp:ListItem>--%>
                                                            </asp:DropDownList>
                                                            <asp:ImageButton ID="used_type_bt" Visible="false" ImageUrl="~/image/icon_24.png" ToolTip="使用類型" runat="server" CommandArgument="usedtype" OnClick="meeting_item_Click" />
                                                            <asp:SqlDataSource runat="server" ID="useclass_sd" ConnectionString="<%$ ConnectionStrings:eip %>" SelectCommand="SELECT 0 AS meeting_usedtype_id, '請選擇' AS usedtype_name UNION ALL select meeting_usedtype_id,usedtype_name from meeting_apply_usedtype"></asp:SqlDataSource>
                                                        </div>
                                                    </div>
                                                    <div class=" col-5 formItem title3">
                                                        <div class="ItemTitle"><span>其他說明</span></div>
                                                        <div class="ItemContent">
                                                            <asp:TextBox ID="other" CssClass="form-control" runat="server" Style="width: 85%;" Enabled="false" />
                                                        </div>
                                                    </div>
                                                    </div>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                            <hr />
                                        </div>
                                        <div class="Box" style="padding: 0px 40px;">
                                            <div class="row" style="margin-bottom: 10px;">
                                                <div class=" col-12 formItem">
                                                    <div class="ItemTitle" style="width: auto;"><span>供餐</span></div>
                                                    <div class="ItemContent d-flex align-items-center" style="padding-left: 2.4rem;">
                                                        <span style="text-align: left; letter-spacing: 0px; color: #375471; font-size: 14px;">(請依實際是否供餐再做填寫；外帶為除便當以外，提供非塑膠包裝之餐點個數)</span>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row" style="margin-bottom: 10px;">
                                                <div class=" col-12 formItem title3">
                                                    <div class="ItemTitle"><span></span></div>
                                                    <div class="ItemContent d-flex align-items-center">
                                                        <div class="d-flex align-items-center"><span class="mr-2">使用環保餐盒</span><asp:TextBox runat="server" ID="lunch_box" CssClass="form-control mr-2" Style="width: 35%" TextMode="Number" oninput="checkPositiveNumber(this);" /><span>個</span></div>
                                                        <div class="d-flex align-items-center"><span class="mr-2">外帶</span><asp:TextBox runat="server" ID="Takeaway" CssClass="form-control mr-2" Style="width: 35%" TextMode="Number" oninput="checkPositiveNumber(this);" /><span>個</span></div>
                                                        <div class="d-flex align-items-center" style="margin-left: -2.5rem"><span class="mr-2" style="white-space: nowrap">使用一次性產品</span><asp:TextBox runat="server" ID="disposable" CssClass="form-control mr-2" Style="width: 35%" TextMode="Number" oninput="checkPositiveNumber(this);" /><span>個</span></div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row" style="margin-bottom: 10px;">
                                                <div class=" col-12 formItem title3">
                                                    <div class="ItemTitle"><span></span></div>
                                                    <div class="ItemContent d-flex align-items-center">

                                                        <asp:UpdatePanel ID="UpdatePanel7" runat="server" class="d-flex align-items-center">
                                                            <ContentTemplate>
                                                                <span style="white-space: nowrap; margin-right: 10px;">無法配合原因</span>
                                                                <asp:CheckBox Text="訂購數量" ID="reason_ck1" runat="server" Style="white-space: nowrap" AutoPostBack="true" />
                                                                <asp:CheckBox Text="收送時間" ID="reason_ck2" runat="server" Style="white-space: nowrap" AutoPostBack="true" />
                                                                <asp:CheckBox Text="辦理場地" ID="reason_ck3" runat="server" Style="white-space: nowrap" AutoPostBack="true" />
                                                                <asp:CheckBox ID="other_reason" Text="其他" runat="server" Style="white-space: nowrap" AutoPostBack="true" OnCheckedChanged="Unnamed_CheckedChanged" />
                                                                <asp:TextBox ID="other_reason_txt" runat="server" CssClass="form-control" Width="30%" Enabled="false"></asp:TextBox>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>

                                                    </div>
                                                </div>
                                            </div>
                                            <hr />
                                        </div>
                                        <div class="Box" style="padding: 0px 40px;">
                                            <div class="row" style="margin-bottom: 10px;">
                                                <div class=" col-5 formItem title3">
                                                    <div class="ItemTitle"><span>出席人數<span style="color: #ff0000">*</span></span></div>
                                                    <div class="ItemContent d-flex align-items-center">
                                                        <asp:TextBox ID="number" class="form-control" runat="server" Style="width: 70%;" />
                                                        <asp:UpdatePanel ID="UpdatePanel8" runat="server" style="width: 100%; display: flex; flex-direction: column">
                                                            <ContentTemplate>
                                                                <asp:Label ID="room_number" runat="server" Text="" Style="text-align: left; letter-spacing: 0px; color: #8F8F8F; font-size: 14px; width: 100%; flex-wrap: nowrap; margin-left: 5px;" />
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="row" style="margin-bottom: 10px;">
                                                <div class=" col-8  title3">
                                                    <div class="ItemTitle"><span>備註</span></div>
                                                    <div class="ItemContent">
                                                        <asp:TextBox ID="note" class="form-control" runat="server" TextMode="MultiLine" Columns="150" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <hr>
                                    <div style="text-align: center">
                                        <asp:Button ID="cancel" runat="server" Text="取消" class="bt" Style="background: #777777"
                                            OnClientClick="return hideModal1();" />
                                        <asp:Button ID="submitbt" runat="server" Text="儲存" class="bt" OnClick="submitbt_Click" />
                                        <asp:Button ID="del" runat="server" Text="刪除" class="bt" Style="background: #B83F1F" OnClick="del_Click" />
                                        <asp:Button ID="modify" runat="server" Text="送出" class="bt" OnClick="modify_Click" />
                                        <asp:HiddenField ID="del_id" runat="server" />
                                    </div>
                                </div>

                                  </ContentTemplate>
        <Triggers>
        </Triggers>
    </asp:UpdatePanel>
                            </div>
                        </div>
                    </div>
                </div>

            </div>
     
    <div class="modal fade bs-NewUser-modal-lg" id="modal4" tabindex="-1" role="dialog" aria-labelledby="myLargeModalLabel" aria-hidden="true" style="backdrop-filter: brightness(0.3);">
        <div class="modal-dialog modal-lg modal-dialog-centered" style="max-width: 25%;">
            <div class="modal-content">
                <div class="modal-header" style="background-color: #C5ECFF; font-size: 22px; display: flex; justify-content: center; align-items: center; position: relative;">
                    <b><span id="Span4" runat="server" style="color: #003168">匯出報表</span></b>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close" style="position: absolute; right: 10px;">
                        <image src="image/popup_close.png"></image>
                    </button>
                </div>
                <div class="modal-body">
                    <div class="MainClass">
                        <div id="content4" role="form" class="form-horizontal text-center" style="margin-top: 50px; margin-bottom: 50px;">
                            <b>免洗餐具及包裝因用水減量情形表</b>
                            <br>
                            <div class="d-flex align-items-center">
                                <asp:TextBox ID="Reduce_start" runat="server" TextMode="Month" CssClass="form-control" />
                                <span class="right" style="font-size: 22px; margin: 0 5px;">～</span>
                                <asp:TextBox ID="Reduce_end" runat="server" TextMode="Month" CssClass="form-control" />
                            </div>
                        </div>
                        <hr>
                        <div style="text-align: center">
                            <asp:Button ID="Button2" runat="server" Text="取消" class="bt" Style="background: #777777"
                                OnClientClick="return hideModal2();" />
                            <asp:Button ID="export_Reduce" runat="server" Text="匯出" class="bt" OnClick="export_Reduce_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="modal fade bs-NewUser-modal-lg" id="modal2" tabindex="-1" role="dialog" aria-labelledby="myLargeModalLabel" aria-hidden="true" style="backdrop-filter: brightness(0.3);">
        <div class="modal-dialog modal-lg modal-dialog-centered" style="width: 550px;">
            <div class="modal-content">
                <div class="modal-header" style="background-color: #C5ECFF; font-size: 22px; display: flex; justify-content: center; align-items: center; position: relative;">
                    <b><span id="Span2" runat="server" style="color: #003168">刪除確認</span></b>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close" style="position: absolute; right: 10px;">
                        <image src="image/popup_close.png"></image>
                    </button>
                </div>
                <div class="modal-body">
                    <div class="MainClass">
                        <div id="content2" role="form" class="form-horizontal text-center" style="margin-top: 50px; margin-bottom: 50px;">
                            <b>
                                <asp:Label ID="deltxt" runat="server" Text="" /></b>
                            <br>
                            <b>確認後無法修改</b>
                        </div>
                        <hr>
                        <div style="text-align: center">
                            <asp:Button ID="Button1" runat="server" Text="取消" class="bt" Style="background: #777777"
                                OnClientClick="return hideModal2();" />
                            <asp:Button ID="md2del_sing" runat="server" Text="確認" class="bt" OnClick="md2del_sing_Click" />
                            <asp:Button ID="md2del_let" runat="server" Text="全部刪除" class="bt" OnClick="md2del_let_Click" />
                            <asp:Button ID="md2del_other" runat="server" Text="僅刪除此筆" class="bt" OnClick="md2del_other_Click" Style="background: #B83F1F" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="modal fade bs-NewUser-modal-lg" id="modal3" tabindex="-1" role="dialog" aria-labelledby="myLargeModalLabel" aria-hidden="true" style="backdrop-filter: brightness(0.3);">
        <div class="modal-dialog modal-lg modal-dialog-centered" style="max-width: 350px !important;">
            <div class="modal-content">
                <asp:UpdatePanel ID="Updatepanel9" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="modal-header" style="background-color: #C5ECFF; font-size: 22px; display: flex; justify-content: center; align-items: center; position: relative;">
                            <b><span id="md3span" runat="server" style="color: #003168"></span></b>
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close" style="position: absolute; right: 10px;">
                                <image src="image/popup_close.png"></image>
                            </button>
                        </div>
                        <div class="modal-body">
                            <div class="MainClass">
                                <div id="content3" role="form" class="form-horizontal text-center" style="">
                                    <span id="title_fav" runat="server" style="color: #145597; font-size: 0.875rem;"></span>
                                    <div style="padding: 0px 20px; display: flex; margin-top: 10px;">
                                        <div style="width: 80%;">
                                            <asp:TextBox runat="server" ID="add_favorite_tb" CssClass="form-control" MaxLength="25" AutoPostBack="true"></asp:TextBox>
                                        </div>
                                        <asp:Button ID="add_favorite_btn" runat="server" Text="新增" class="btn BT_orange ml-4" Style="padding: 2px 8px;" OnClick="add_favorite_btn_Click" />
                                    </div>
                                    <hr />
                                    <asp:Panel ID="favorite_pl" runat="server" CssClass="scroll-container"></asp:Panel>
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="mnbt" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="hostbt" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="add_favorite_btn" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>

    <%--會議類型&使用類型 modal--%>
    <div class="modal fade bs-NewUser-modal-lg" id="modal_item_add" tabindex="-1" role="dialog" aria-labelledby="myLargeModalLabel" aria-hidden="true" style="backdrop-filter: brightness(0.3);">
         <div class="modal-dialog modal-lg modal-dialog-centered" style="max-width: 350px !important;">
             <div class="modal-content">
                 <asp:UpdatePanel ID="meeting_item_ul" runat="server" UpdateMode="Conditional">
                     <ContentTemplate>
                         <div class="modal-header" style="background-color: #C5ECFF; font-size: 22px; display: flex; justify-content: center; align-items: center; position: relative;">
                             <b><span id="item_tile" runat="server" style="color: #003168"></span></b>
                             <button type="button" class="close" data-dismiss="modal" aria-label="Close" style="position: absolute; right: 10px;">
                                 <image src="image/popup_close.png"></image>
                             </button>
                         </div>
                         <div class="modal-body">
                             <div class="MainClass">
                                 <div id="content5" role="form" class="form-horizontal text-center" style="">
                                     <span id="Span3" runat="server" style="color: #145597; font-size: 0.875rem;"></span>
                                     <div style="padding: 0px 20px; display: flex; margin-top: 10px;">
                                         <div style="width: 80%;">
                                             <asp:TextBox runat="server" ID="item_tb" CssClass="form-control" MaxLength="25" AutoPostBack="true"></asp:TextBox>
                                         </div>
                                         <asp:Button ID="add_item_btn" runat="server" Text="新增" class="btn BT_orange ml-4" Style="padding: 2px 8px;" OnClick="add_meeting_item_Click" />
                                     </div>
                                     <hr />
                                     <asp:Panel ID="modal_item_pl" runat="server" CssClass="scroll-container" style="max-height: 300px; overflow-y: auto;"></asp:Panel>
                                 </div>
                             </div>
                         </div>
                     </ContentTemplate>
                     <Triggers>
                         <asp:AsyncPostBackTrigger ControlID="meeting_type_bt" EventName="Click" />
                         <asp:AsyncPostBackTrigger ControlID="used_type_bt" EventName="Click" />
                         <asp:AsyncPostBackTrigger ControlID="add_item_btn" EventName="Click" />
                     </Triggers>
                 </asp:UpdatePanel>
             </div>
         </div>
    </div>
    <%--會議類型&使用類型modal--%>
</asp:Content>

