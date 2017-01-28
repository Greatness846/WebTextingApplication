Public Class Login
    Inherits System.Web.UI.Page
    Dim phoneChatDA As DataAccess
    Dim userObj As User

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        phoneChatDA = New DataAccess

    End Sub

    Protected Sub loginBtn_Click(sender As Object, e As EventArgs) Handles loginBtn.Click

        If Not Trim(usernameTxt.Text) = "" And Not Trim(pwTxt.Text) = "" Then
            phoneChatDA.getUserLogin(usernameTxt.Text)
            If phoneChatDA.usersTable.Rows.Count > 0 Then
                If phoneChatDA.usersTable.Rows(0)("userLocked") = "Y" Then
                    ErrorMsg.Visible = True
                    errorTxt.InnerText = "Account is locked. Too many failed attempts occurred. Please contact admin to reset your account."

                Else
                    If phoneChatDA.usersTable.Rows(0)("userPW") = pwTxt.Text And phoneChatDA.usersTable.Rows.Count > 0 Then
                        userObj = New User
                        userObj.loggedIn = True
                        userObj.messagesTable = phoneChatDA.getUserMessages(usernameTxt.Text)
                        userObj.contactsTable = phoneChatDA.getUserContacts(usernameTxt.Text)
                        userObj.userName = Trim(usernameTxt.Text)
                        'userObj.acctPhoneNumber = "8638374013"
                        Session("userObj") = userObj
                        Session("dataAccessObj") = phoneChatDA

                        Response.Redirect("MessageBoard.aspx")
                    Else
                        If Not Trim(Session("userTriesCount")) = "" Then
                            Dim userTries As Integer = 0
                            ErrorMsg.Visible = True
                            userTries += 1
                            Session("userTriesCount") = userTries
                        Else
                            Dim userTries As Integer = Session("userTriesCount")
                            userTries += 1
                            If userTries > 5 Then
                                ErrorMsg.Visible = True
                                errorTxt.InnerText = "Account is locked. Too many failed attempts occurred. Please contact admin to reset your account."
                                If phoneChatDA.usersTable.Rows.Count > 0 Then
                                    phoneChatDA.lockUser(usernameTxt.Text)

                                End If
                            End If
                            ErrorMsg.Visible = True
                            Session("userTriesCount") = userTries
                        End If
                    End If
                End If
            Else
                ErrorMsg.Visible = True
            End If
        End If

    End Sub
End Class