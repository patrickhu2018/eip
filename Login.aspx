<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>中科行政系統</title>

    <link href="css/bootstrap.css" rel="stylesheet" type="text/css" media="screen" />
    <link href="css/font-awesome.min.css" rel="stylesheet" />
    <link href="css/simple-sidebar.css" rel="stylesheet" type="text/css" />
    <link href="css/SmartEMCStyle.css" rel="stylesheet" type="text/css" />

    <script src="js/jquery-3.5.1.min.js"></script>
    <script src="js/bootstrap.js"></script>
    <script src="js/bootstrap.min.js"></script>
</head>
<body class="LoginPage">
    <form id="form1" runat="server">
        <div class="" style="transform: translateY(70%);">
            <div class="d-flex justify-content-center h-100">
                <div class="user_card">
                    <div class="d-flex justify-content-center">
                        <div class="brand_logo_container"></div>
                    </div>
                    <div class="d-flex justify-content-center">
                        <div class="col-12">
                            <div class="input-group mb-2">
                                <asp:TextBox ID="account_tb" CssClass="form-control input_user" placeholder="用戶帳號" runat="server" title="輸入帳號"></asp:TextBox>
                            </div>

                            <div class="d-flex justify-content-center mt-3">
                                <asp:Panel runat="server" ID="Panel1" DefaultButton="login_item">
                                    <asp:Button runat="server" ID="login_item" CssClass="btn login_btn" Text="登入特殊用品" OnClick="login_btn_Click" style="margin-bottom:10px;background-color:#F37EC1!important"/>
                                    <asp:Button runat="server" ID="login_repair" CssClass="btn login_btn" Text="登入修繕申請" OnClick="login_btn_Click" style="margin-bottom:10px; background-color:#EFCF00!important"/>
                                    <asp:Button runat="server" ID="login_meet" CssClass="btn login_btn" Text="登入會議室管理" OnClick="login_btn_Click" style="margin-bottom:10px; background-color:#7C73E6!important"/>
                                </asp:Panel>
                            </div>
                        </div>
                    </div>
                    <div class="col-12 mt-2">
                        <div class="d-flex justify-content-end links">
                        </div>
                    </div>

                </div>
            </div>
        </div>
    </form>
</body>
</html>
