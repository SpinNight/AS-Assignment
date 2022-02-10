<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Verification.aspx.cs" Inherits="AS_Assignment.Verification" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <h2>
            <br />
            <asp:Label ID="Label1" runat="server" Text="Verification"></asp:Label>
            <br />
            <br />
        </h2>
        <table class="style1">
            <tr>
                <td>
                    <asp:Label ID="Label2" runat="server" Text="Verification Code:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="tb_code" runat="server" Height="36px" Width="280px"></asp:TextBox>
                </td>
                <td>
                    <asp:Label ID="lbl_code" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td></td>
                <td>
                    <asp:Button ID="btn_Submit" runat="server" Height="48px"
                        OnClick="btn_Submit_Click" Text="Verify" Width="288px" />
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
