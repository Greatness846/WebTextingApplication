<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="MessageBoard.aspx.vb" Inherits="PhoneChat.MessageBoard" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="Stylesheet" type="text/css" href="css/Styles.css" />
    <script type="text/javascript" src="Javascript/messageScript.js"></script>
</head>
<body>
    <form id="form1" runat="server">        
    <asp:ScriptManager ID="ScriptManager1" runat="server">
  </asp:ScriptManager>
  <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
    <asp:Timer ID="Timer1" runat="server" Interval="1000" Enabled="True">
    </asp:Timer>
    </ContentTemplate>
    </asp:UpdatePanel>
    <div id="contactsDiv" class="left">
        <span style="font-weight:bold; color:Navy;">Contacts</span>
       <asp:UpdatePanel ID="contactsPanel" runat="server" UpdateMode="Conditional">
       <ContentTemplate>
        <asp:ListView ID="contactsList" runat="server"> 
        <LayoutTemplate>
            <table style="border: 1px solid black">
                <tr runat="server" id="itemPlaceHolder" />
            </table>
        </LayoutTemplate> 
        <ItemTemplate runat="server">
        </ItemTemplate>      

        <EmptyDataTemplate>
              <table class="emptyTable" cellpadding="5" cellspacing="5">
                <tr>
                  <td>
                  </td>
                  <td>
                    You currently have no Contacts. Get out there and PhoneChat!!!
                  </td>
                </tr>
              </table>
          </EmptyDataTemplate>
        </asp:ListView>
        </ContentTemplate>
       </asp:UpdatePanel>
    <asp:TextBox ID="contactsTxtBx" runat="server" Width="250px"></asp:TextBox>
    <asp:Button ID="addContactsBtn" Text="Add" runat="server" />
    </div>
    <asp:UpdatePanel ID="messagePanel" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
    <div ID="messageDiv" class="right" style="width: 600px; height: 500px; background-color: #ffffcc; 
                        font-size: 12px; overflow: auto;" runat="server">

    </div>
    </ContentTemplate>
    </asp:UpdatePanel>    
    <div id="messageBx" style="margin-bottom: 2px;text-align:center;">
        <table>
            <tr>
                <td>
                    <asp:TextBox ID="messageTxtBx" runat="server" Height="92px" Width="600px" 
                        TextMode="MultiLine"></asp:TextBox>
                </td>
                <td>
                    <asp:Button ID="sendBtn" runat="server" Text="Send" 
                         />
                </td>
                <td>
                <asp:UpdatePanel ID="errorPanel" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Label ID="sendMsglbl" runat="server" Text="Label" Visible="false" ForeColor="Red"></asp:Label>
                </ContentTemplate>
                </asp:UpdatePanel>
                </td>
            </tr>
        </table>
    </div>
    <div id="userStatus" style="position:absolute; top:0px; right:0px;">
        <asp:Label ID="userNameLbl" runat="server" Text="Label"></asp:Label>(<asp:LinkButton ID="userStatusLink" runat="server"></asp:LinkButton>)
    </div>
    </form>

    </body>
</html>
