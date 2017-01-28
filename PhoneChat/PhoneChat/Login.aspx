<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Login.aspx.vb" Inherits="PhoneChat.Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>PhoneChat Login</title>
</head>
<body>
    <form id="form1" runat="server">
    <div style="height:100%; width:100%; text-align:center;">
    <table style="margin:0 auto;">
        <tr style="background-color:Aqua;">
            <th colspan="2">Welcome to PhoneChat!</th>
        </tr>
        <tr>
            <th colspan="2" style="background-color:Orange;">Sign in to begin phone chatting!</th>
        </tr>
        <tr ID="ErrorMsg" visible="false" runat="server">
            <th colspan="2"><span ID="errorTxt" runat="server" style="color:Red">Invalid Username/Password</span></th>
        </tr>
        <tr>
            <td>UserName:</td>
            <td>
                <asp:TextBox ID="usernameTxt" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>Password:</td>
            <td>
                <asp:TextBox ID="pwTxt" runat="server" TextMode="Password"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="2" style="text-align:center;">
                <asp:Button ID="loginBtn" runat="server" Text="Login" />
            </td>
        </tr>
        <tr>
            <td colspan="2">Don't have an account? <a href = "SignUp.aspx">Sign up today!</a></td>
        </tr>
    </table>
    </div>
    </form>
</body>
</html>
