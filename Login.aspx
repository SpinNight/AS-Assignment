<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="AS_Assignment.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="https://www.google.com/recaptcha/api.js?render=6LdkzmkeAAAAALzsmTlc-G6pIpZgDW_-SjcP4ieT"></script>
</head>
<body>
    <form id="form1" runat="server">
        <h2>
            <br />
            <asp:Label ID="Label1" runat="server" Text="Login"></asp:Label>
            <br />
            <br />
        </h2>
        <table>
            <tr>
                <td>
                    <asp:Label ID="Label2" runat="server" Text="User ID/Email"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="tb_userid" runat="server" Height="16px" Width="280px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label3" runat="server" Text="Password"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="tb_pwd" runat="server" Height="16px" Width="281px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td></td>
                <td>
                    <asp:Button ID="btn_Submit" runat="server" Height="48px"
                        OnClick="btn_Submit_Click" Text="Submit" Width="288px" />
                </td>
            </tr>
        </table>
        &nbsp;&nbsp;&nbsp;
    <br />
        <br />
        <asp:Label ID="lb_error" runat="server"></asp:Label>
        <br />
        <br />
        <br />
        <br />
        <br />
        <input type="hidden" id="g-recaptcha-response" name="g-recaptcha-response"/>
        <asp:Label ID="lbl_gScore" runat="server"></asp:Label>
        <div>
        </div>
    </form>
    <script>
        grecaptcha.ready(function () {
            grecaptcha.execute('6LdkzmkeAAAAALzsmTlc-G6pIpZgDW_-SjcP4ieT', { action: 'Login' }).then(function (token) {
                document.getElementById("g-recaptcha-response").value = token;
            });
        });
    </script>
</body>
</html>
