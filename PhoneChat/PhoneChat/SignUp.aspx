<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="SignUp.aspx.vb" Inherits="PhoneChat.SignUp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>PhoneChat Sign Up!</title>
</head>
<body>
    <form id="form1" runat="server">
    <div id="RegistrationForm">
        <table style="margin:0 auto;">
                <tr style="background-color:Aqua;">
                    <th colspan="2">Welcome to PhoneChat!</th>
                </tr>
                <tr>
                    <th colspan="2" style="background-color:Orange;">Complete sign up and begin sending texts today!</th>
                </tr>
                <tr ID="ErrorMsg" visible="false" runat="server">
                    <th colspan="2"><span ID="errorTxt" runat="server" style="color:Red"></span></th>
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
                    <td>Confirm Password:</td>
                    <td>
                        <asp:TextBox ID="pwConf" runat="server" TextMode="Password"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="text-align:center;">
                        <asp:Button ID="submitBtn" runat="server" Text="Sign Up" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">Already have an account? <a href = "Login.aspx">Click here to sign in!</a></td>
                </tr>
            </table>
    </div>
    </form>
</body>
</html>
