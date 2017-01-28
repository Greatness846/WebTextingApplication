Public Class SignUp
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub



    Protected Sub submitBtn_Click(sender As Object, e As EventArgs) Handles submitBtn.Click
        If Not Trim(usernameTxt.Text) = "" And Not Trim(pwTxt.Text) = "" And Not Trim(pwConf.Text) = "" Then
            If Trim(pwTxt.Text) = Trim(pwConf.Text) Then

                Dim user As New User
                Dim DA As New DataAccess
                Dim usersTableRow As DataRow

                DA.getUserLogin(Trim(usernameTxt.Text))
                If DA.usersTable.Rows.Count > 0 Then
                    errorTxt.InnerText = "User already exists"
                    ErrorMsg.Visible = True
                Else
                    DA.getUserLogin(Trim(usernameTxt.Text))

                    user.loggedIn = True
                    user.userName = Trim(usernameTxt.Text)
                    DA.userSignUp(user.userName, pwTxt.Text)
                    usersTableRow = DA.usersTable.NewRow()
                    usersTableRow("webUserId") = user.userName
                    usersTableRow("userPW") = pwTxt.Text
                    usersTableRow("userLocked") = "N"
                    DA.usersTable.Rows.Add(usersTableRow)

                    DA.userSignUp(user.userName, pwTxt.Text)

                    'DA.usersTable.Rows(DA.usersTable.Rows.Count)("webUserId") = user.userName
                    'DA.usersTable.Rows(DA.usersTable.Rows.Count)("userPW") = pwTxt.Text

                    Session("userObj") = user
                    Session("dataAccessObj") = DA

                    Response.Redirect("MessageBoard.aspx")
                End If
            Else
                errorTxt.InnerText = "Passwords do not match. Please enter matching passwords"
                ErrorMsg.Visible = True
            End If
        End If
    End Sub
End Class