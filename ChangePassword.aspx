<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs" Inherits="AS_Assignment.ChangePassword" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <h2>
            <br />
            <asp:Label ID="Label1" runat="server" Text="Password reset"></asp:Label>
            <br />
            <br />
        </h2>
        <table class="style1">
            <tr>
                <td>
                    <asp:Label ID="Label3" runat="server" Text="Password:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="tb_pwd" runat="server" Height="36px" Width="280px"></asp:TextBox>
                </td>
                <td>
                    <asp:Label ID="lbl_pwd" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label5" runat="server" Text="Confirm Password"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="tb_cfmpwd" runat="server" Height="36px" Width="280px"></asp:TextBox>
                </td>
                <td>
                    <asp:Label ID="lbl_cfmpwd" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td></td>
                <td>
                    <asp:Button ID="btn_Submit" runat="server" Height="48px"
                        OnClick="btn_Submit_Click" Text="Change Password" Width="288px" />
                </td>
            </tr>
            <tr>
                <td></td>
                <td>
                    <asp:Label ID="lbl_policy" runat="server"></asp:Label>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
