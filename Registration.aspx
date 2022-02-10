<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Registration.aspx.cs" Inherits="AS_Assignment.Registration" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>My Registration</title>
    <script type="text/javascript">
        function validatepwd() {
            var str = document.getElementById('<% = tb_pwd.ClientID %>').value.trim();
            document.getElementById('lbl_pwdchecker').style.color = "Red";
            if (str.length < 12) {
                document.getElementById('lbl_pwdchecker').innerHTML = " Password length Must be at least 12 characters";
                return ("too_short");
            }
            else if (str.search(/[0-9]/) == -1) {
                document.getElementById('lbl_pwdchecker').innerHTML = "Password require at least 1 number";
                return ("no_number");
            }
            else if (str.search(/[a-z]/) == -1) {
                document.getElementById('lbl_pwdchecker').innerHTML = "Password require at least 1 lowercase character";
                return ("no_lowercase")
            }
            else if (str.search(/[A-Z]/) == -1) {
                document.getElementById('lbl_pwdchecker').innerHTML = "Password require at least 1 uppercase character";
                return ("no_uppercase")
            }
            else if (str.search(/[!@#$%^&*()]/) == -1) {
                document.getElementById('lbl_pwdchecker').innerHTML = "Password require at least 1 special character";
                return ("no_special")
            }
            document.getElementById('lbl_pwdchecker').innerHTML = str;
            document.getElementById('lbl_pwdchecker').style.color = "Green";
        }

        function validatecreditcard() {
            var str = document.getElementById('<% = tb_creditcard.ClientID %>').value.trim();
            document.getElementById('lbl_creditcardchecker').style.color = "Red";
            if (str.length != 16) {
                document.getElementById('lbl_creditcardchecker').innerHTML = "Credit card number must be exactly 16 digit";
                return;
            }
            else if (!/^\d+$/.test(str)) {
                document.getElementById('lbl_creditcardchecker').innerHTML = "Credit card number must contain only digit";
                return;
            }
            document.getElementById('lbl_creditcardchecker').innerHTML = "Credit card is valid";
            document.getElementById('lbl_creditcardchecker').style.color = "Green";
        }

        function validateemail() {
            var str = document.getElementById('<% = tb_userid.ClientID %>').value.trim();
            document.getElementById('lbl_emailchecker').style.color = "Red";
            if (! /^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/.test(str)) {
                document.getElementById('lbl_emailchecker').innerHTML = "Email is invalid";
                return;
            }
            document.getElementById('lbl_emailchecker').innerHTML = "Email is valid";
            document.getElementById('lbl_emailchecker').style.color = "Green";
        }

    </script>
</head>
<body style="height: 163px">
    <form id="form1" runat="server">
         <h2>
        <br />
        <asp:Label ID="Label1" runat="server" Text="Account Registration"></asp:Label>
        <br />
        <br />
    </h2>
    <table class="style1">
        <tr>
            <td>
                <asp:Label ID="Label2" runat="server" Text="Email"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="tb_userid" runat="server" Height="36px" Width="280px" onkeyup="validateemail()"></asp:TextBox>
            </td>
            <td>
                <asp:Label ID="lbl_emailchecker" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label3" runat="server" Text="Password"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="tb_pwd" runat="server" Height="32px" Width="281px" onkeyup="validatepwd()"></asp:TextBox>
            </td>
            <td>
                <asp:Label ID="lbl_pwdchecker" runat="server" ></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label4" runat="server" Text="Confirm Password"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="tb_cfpwd" runat="server" Height="32px" Width="281px"></asp:TextBox>
            </td>
            <td>
                <asp:Label ID="lbl_cfpwd" runat="server" ></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label5" runat="server" Text="First Name"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="tb_firstname" runat="server" Height="32px" Width="281px"></asp:TextBox>
            </td>
            <td>
                <asp:Label ID="lbl_first" runat="server" ></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label6" runat="server" Text="Last Name"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="tb_lastname" runat="server" Height="32px" Width="281px"></asp:TextBox>
            </td>
            <td>
                <asp:Label ID="lbl_last" runat="server" ></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label7" runat="server" Text="Credit Card Number"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="tb_creditcard" runat="server" Height="32px" Width="281px" onkeyup="validatecreditcard()"></asp:TextBox>
            </td>
            <td>
                <asp:Label ID="lbl_creditcardchecker" runat="server" ></asp:Label>
            </td>
        </tr>
        <tr>
            <td></td>
            <td>
                <asp:Button ID="btn_Submit" runat="server" Height="48px" OnClick="btn_Submit_Click" Text="Submit" Width="288px" />
            </td>
        </tr>
    </table>
    </form>




</body>
</html>
