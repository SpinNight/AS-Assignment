<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Success.aspx.cs" Inherits="AS_Assignment.Success" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <h2>
            <br />
            <asp:Label ID="Label1" runat="server" Text="User Profile:"></asp:Label>
            <br />
            <br />
        </h2>        <br />
        <br />
        <asp:Label ID="Label2" runat="server" Text="Email:"></asp:Label>
        
        <asp:Label ID="lbl_userID" runat="server"></asp:Label>
        <br />
        <asp:Label ID="Label4" runat="server" Text="Credit Card Number:"></asp:Label>
        <asp:Label ID="lbl_creditcard" runat="server"></asp:Label>

        <br />

        <asp:Button ID="btn_changepwd" runat="server" Height="48px" OnClick="ChangePwd" Text="Change Password" Width="200px" />
        <asp:Button ID="btn_Logout" runat="server" Height="48px" OnClick="Logout" Text="Logout" Width="200px" />

    </form>
</body>
</html>
