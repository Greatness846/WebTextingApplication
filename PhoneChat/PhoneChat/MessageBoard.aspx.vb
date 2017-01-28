Imports Twilio

Public Class MessageBoard
    Inherits System.Web.UI.Page

    Shared userClass As User
    Shared phoneChatDA As DataAccess
    Shared rowCount As Integer = 0
    Shared accountSid As String = ""
    Shared AuthToken As String = ""
    Shared TwilioClass As New TwilioRestClient(accountSid, AuthToken)
    Shared IncMessage As New Twilio.Message
    Shared messageSidArr As New DataTable
    Shared lastMessage As String
    Shared lastDateTime As DateTime
    Shared incMessageCount As Integer
    Shared outMessageCount As Integer = 0



    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        userClass = Session("userObj")
        phoneChatDA = Session("dataAccessObj")


        Dim contactMessages() As DataRow

        If Not IsNothing(userClass) And Not IsNothing(phoneChatDA) And userClass.loggedIn Then
            userNameLbl.Text = "Hi " & userClass.userName & "!"
            userStatusLink.Text = "Log Out"
            userStatusLink.PostBackUrl = "Login.aspx"
            If Not Page.IsPostBack Then
                userClass.acctPhoneNumber = TwilioClass.ListIncomingPhoneNumbers.IncomingPhoneNumbers(0).PhoneNumber
                incMessageCount = userClass.messagesTable.Select("Incoming = True").Count
            End If


            If Not IsNothing(userClass.contactsTable) And Not IsNothing(userClass.messagesTable) Then
                contactsList.DataSource = userClass.contactsTable
                contactsList.DataBind()

                If userClass.messagesTable.Rows.Count > 0 Then
                    If contactsList.Items.Count > 0 Then
                        Dim selectedRowIndex As Integer = -1
                        For i = 0 To contactsList.Items.Count - 1
                            Dim deselectRadioBtns As RadioButton = contactsList.FindControl("selectContact" & i)
                            If deselectRadioBtns.Checked Then
                                selectedRowIndex = i
                                Exit For
                            End If
                        Next
                        If Not selectedRowIndex = -1 And Not Page.IsPostBack Then


                            contactMessages = userClass.messagesTable.Select("phoneNum = '" & Trim(userClass.contactsTable(selectedRowIndex)("phoneNum")) & "'")
                            messageDiv.InnerHtml = ""
                            For i = 0 To contactMessages.Length - 1
                                If contactMessages(i)("Incoming") Then
                                    Dim HTML As String

                                    HTML = "<div class='incomingContainer'><span class='incoming'>"
                                    HTML += contactMessages(i)("phoneNum") & ": </span>"
                                    HTML += "<span style='float:left;overflow:auto;'>" & contactMessages(i)("messageTxt") & "</span>"
                                    HTML += "<br />" & "<span style='float:left;overflow:auto;'>" & contactMessages(i)("msgDateTime") & "</span></div>"

                                    messageDiv.InnerHtml += HTML
                                Else
                                    Dim HTML As String

                                    HTML = "<div class='outgoingContainer'><span class='outgoing'>"
                                    HTML += contactMessages(i)("webUserId") & ": </span>"
                                    HTML += "<span style='float:right;overflow:auto;'>" & contactMessages(i)("messageTxt") & "</span>"
                                    HTML += "<br />" & "<span style='float:right;overflow:auto;'>" & contactMessages(i)("msgDateTime") & "</span></div>"

                                    messageDiv.InnerHtml += HTML
                                End If
                            Next
                        End If
                    End If
                    'messagePanel.Update()
                End If
            Else
                userClass.contactsTable = phoneChatDA.getUserContacts(userClass.userName)
                userClass.messagesTable = phoneChatDA.getUserMessages(userClass.userName)

                'bind contacts data to listview
                contactsList.DataSource = userClass.contactsTable
                contactsList.DataBind()
            End If

        End If
    End Sub

    Protected Sub addContactsBtn_Click(sender As Object, e As EventArgs) Handles addContactsBtn.Click
        If Not IsNothing(userClass) And Not IsNothing(phoneChatDA) Then
            Dim newContactRow As DataRow

            If Not Trim(contactsTxtBx.Text) = "" Then
                phoneChatDA.saveSingleContact(userClass.userName, Trim(contactsTxtBx.Text))

                newContactRow = userClass.contactsTable.NewRow()
                newContactRow("webUserId") = userClass.userName
                newContactRow("phoneNum") = Trim(contactsTxtBx.Text)

                userClass.contactsTable.Rows.Add(newContactRow)

                contactsList.DataSource = Nothing
                contactsList.DataBind()

                contactsList.DataSource = userClass.contactsTable
                contactsList.DataBind()

                'contactsPanel.Update()
            End If
        End If
    End Sub

    Protected Sub sendBtn_Click(sender As Object, e As EventArgs) Handles sendBtn.Click
        Dim rowIndex As Integer

        If Trim(messageTxtBx.Text) = "" Then
            sendMsglbl.Text = "There is no message to be sent. Please enter words to send"
            sendMsglbl.Visible = True

            errorPanel.Update()

        Else
            sendMsglbl.Text = "label"
            sendMsglbl.Visible = False

            errorPanel.Update()

            For i = 0 To contactsList.Items.Count - 1
                Dim deselectRadioBtns As RadioButton = contactsList.FindControl("selectContact" & i)
                If deselectRadioBtns.Checked Then
                    rowIndex = i
                    Exit For
                End If
            Next

            phoneChatDA.saveNewMessage(userClass.userName, userClass.contactsTable(rowIndex)("phoneNum"), messageTxtBx.Text, False, Date.Now)
            userClass.messagesTable = phoneChatDA.getContactMessages(userClass.userName, userClass.contactsTable(rowIndex)("phoneNum"))

            'Dim newMessageRow As DataRow
            'newMessageRow = userClass.messagesTable.NewRow()

            'newMessageRow("webUserId") = userClass.userName
            'newMessageRow("phoneNum") = userClass.contactsTable(rowIndex)("phoneNum")
            'newMessageRow("messageTxt") = messageTxtBx.Text
            'newMessageRow("msgDateTime") = Date.Now
            'newMessageRow("Incoming") = False

            'userClass.messagesTable.Rows.Add(newMessageRow)
            messageDiv.InnerHtml = ""
            loadMessages(userClass.userName, CType(Trim(userClass.contactsTable(rowIndex)("phoneNum")), Long))
            messagePanel.Update()
            sendTxtMessage("+18638374013", Trim(userClass.contactsTable(rowIndex)("phoneNum")), messageTxtBx.Text) 'userClass.acctPhoneNumber
            messageTxtBx.Text = ""

        End If

    End Sub
    Public Function formatPhoneNumber(ByVal phoneNumber As String)
        Dim formattedNum As String
        If phoneNumber.Length = 10 Then
            formattedNum = Mid(phoneNumber, 1, 3) & "-" & Mid(phoneNumber, 4, 3) & "-" & Mid(phoneNumber, 7, 4)
            Return formattedNum
        ElseIf phoneNumber.Contains("-") Then
            formattedNum = phoneNumber.Replace("-", "")
            Return formattedNum
        Else
            Return phoneNumber
        End If

    End Function

    Protected Sub contactsList_SelectedIndexChanged(sender As Object, e As EventArgs) Handles contactsList.SelectedIndexChanged

        messageDiv.InnerHtml = ""
        loadMessages(userClass.userName, contactsList.SelectedValue)
        Timer1.Enabled = True

    End Sub

    Private Sub loadMessages(ByVal userId As String, ByVal phoneNumber As Long)
        Dim messagesDt As DataTable

        messagesDt = phoneChatDA.getContactMessages(userId, phoneNumber)

        For i = 0 To messagesDt.Rows.Count - 1
            If messagesDt(i)("Incoming") Then
                Dim HTML As String

                HTML = "<div class='incomingContainer'><span class='incoming'>"
                HTML += messagesDt(i)("phoneNum") & ": </span>"
                HTML += "<span style='float:left;overflow:auto;'>" & messagesDt(i)("messageTxt") & "</span>"
                HTML += "<br />" & "<span style='float:left;overflow:auto;'>" & messagesDt(i)("msgDateTime") & "</span></div>"

                messageDiv.InnerHtml += HTML
            Else
                Dim HTML As String

                HTML = "<div class='outgoingContainer'><span class='outgoing'>"
                HTML += messagesDt(i)("webUserId") & ": </span>"
                HTML += "<span style='float:right;overflow:auto;'>" & messagesDt(i)("messageTxt") & "</span>"
                HTML += "<br />" & "<span style='float:right;overflow:auto;'>" & messagesDt(i)("msgDateTime") & "</span></div>"

                messageDiv.InnerHtml += HTML
            End If
        Next


    End Sub

    Protected Sub contactsList_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.ListViewItemEventArgs) Handles contactsList.ItemDataBound

        If e.Item.ItemType = ListViewItemType.DataItem Then
            Dim rowView As DataRowView
            Dim itemTable As Table
            Dim tr As New TableRow
            Dim editCell As New TableCell
            Dim phoneNumberCell As New TableCell
            Dim selectCell As New TableCell
            Dim deleteCell As New TableCell

            'Build Row Controls
            Dim editCntl As New Button
            Dim phoneNumCntl As New Label
            Dim selectCntl As New RadioButton
            Dim deleteCntl As New Button

            editCntl.ID = "editContact" & e.Item.DataItemIndex
            phoneNumCntl.ID = "phoneNumContact" & e.Item.DataItemIndex
            selectCntl.ID = "selectContact" & e.Item.DataItemIndex
            deleteCntl.ID = "deleteContact" & e.Item.DataItemIndex

            editCntl.Text = "Edit"
            deleteCntl.Text = "Delete"

            'If rowCount = 0 Then
            '    Dim headerRow As New TableRow
            '    Dim ActionHeader As New TableHeaderCell
            '    Dim PhoneNumHeader As New TableHeaderCell
            '    Dim SelectHeader As New TableHeaderCell
            '    Dim DeleteHeader As New TableHeaderCell


            '    ActionHeader.Attributes.CssStyle.Add("border", "1px solid black")
            '    ActionHeader.Attributes.CssStyle.Add("background-color", "Aqua")
            '    ActionHeader.Text = "Action"
            '    headerRow.Cells.Add(ActionHeader)

            '    PhoneNumHeader.Attributes.CssStyle.Add("border", "1px solid black")
            '    PhoneNumHeader.Attributes.CssStyle.Add("background-color", "Aqua")
            '    PhoneNumHeader.Text = "Phone Number"
            '    headerRow.Cells.Add(PhoneNumHeader)

            '    SelectHeader.Attributes.CssStyle.Add("border", "1px solid black")
            '    SelectHeader.Attributes.CssStyle.Add("background-color", "Aqua")
            '    SelectHeader.Text = "Select"
            '    headerRow.Cells.Add(SelectHeader)

            '    DeleteHeader.Attributes.CssStyle.Add("border", "1px solid black")
            '    DeleteHeader.Attributes.CssStyle.Add("background-color", "Aqua")
            '    DeleteHeader.Text = "Delete"
            '    headerRow.Cells.Add(DeleteHeader)


            '    contactsList.Controls.Add(headerRow)

            'End If

            AddHandler editCntl.Click, AddressOf Me.itemEdit_Event
            AddHandler selectCntl.CheckedChanged, AddressOf Me.itemSelect_Event
            AddHandler deleteCntl.Click, AddressOf Me.itemDelete_Event

            selectCntl.AutoPostBack = True
            'deleteCntl.PostBackUrl = "MessageBoard.aspx"

            rowView = CType(e.Item.DataItem, DataRowView)
            phoneNumCntl.Text = formatPhoneNumber(Trim(rowView("phoneNum").ToString))

            'Style Table Cells
            editCell.Attributes.CssStyle.Add("border", "1px solid black")
            phoneNumberCell.Attributes.CssStyle.Add("border", "1px solid black")
            selectCell.Attributes.CssStyle.Add("border", "1px solid black")
            deleteCell.Attributes.CssStyle.Add("border", "1px solid black")

            'Add controls to table cells
            editCell.Controls.Add(editCntl)
            phoneNumberCell.Controls.Add(phoneNumCntl)
            selectCell.Controls.Add(selectCntl)
            deleteCell.Controls.Add(deleteCntl)

            'Add cells to table row
            tr.Cells.Add(editCell)
            tr.Cells.Add(phoneNumberCell)
            tr.Cells.Add(selectCell)
            tr.Cells.Add(deleteCell)


            'Check for itemTable add based on each
            If Not IsNothing(contactsList.FindControl("itemTable")) Then
                itemTable = CType(contactsList.FindControl("itemTable"), Table)
                itemTable.Controls.Add(tr)
            Else
                itemTable = New Table

                Dim headerRow As New TableRow
                Dim ActionHeader As New TableHeaderCell
                Dim PhoneNumHeader As New TableHeaderCell
                Dim SelectHeader As New TableHeaderCell
                Dim DeleteHeader As New TableHeaderCell


                ActionHeader.Attributes.CssStyle.Add("border", "1px solid black")
                ActionHeader.Attributes.CssStyle.Add("background-color", "Aqua")
                ActionHeader.Text = "Action"
                headerRow.Cells.Add(ActionHeader)

                PhoneNumHeader.Attributes.CssStyle.Add("border", "1px solid black")
                PhoneNumHeader.Attributes.CssStyle.Add("background-color", "Aqua")
                PhoneNumHeader.Text = "Phone Number"
                headerRow.Cells.Add(PhoneNumHeader)

                SelectHeader.Attributes.CssStyle.Add("border", "1px solid black")
                SelectHeader.Attributes.CssStyle.Add("background-color", "Aqua")
                SelectHeader.Text = "Select"
                headerRow.Cells.Add(SelectHeader)

                DeleteHeader.Attributes.CssStyle.Add("border", "1px solid black")
                DeleteHeader.Attributes.CssStyle.Add("background-color", "Aqua")
                DeleteHeader.Text = "Delete"
                headerRow.Cells.Add(DeleteHeader)


                itemTable.ID = "itemTable"
                itemTable.Controls.Add(headerRow)
                itemTable.Controls.Add(tr)
                contactsList.Controls.Add(itemTable)
            End If
            'Add row to ItemTemplate

            'contactsList.Controls.Add(itemTable)

        End If

    End Sub
    Protected Sub itemDelete_Event(sender As Object, e As System.EventArgs)
        Dim deleteBtn As Button = CType(sender, Button)
        Dim btnId As String = deleteBtn.ID
        Dim rowIndex As Integer = CType(btnId.Remove(btnId.IndexOf("deleteContact"), "deleteContact".Length), Integer)

        phoneChatDA.DeleteContact(userClass.userName, Trim(userClass.contactsTable(rowIndex)("phoneNum")))
        userClass.contactsTable.Rows.RemoveAt(rowIndex)

        rowCount = 0
        contactsList.DataSource = Nothing
        contactsList.DataBind()
        contactsList.DataSource = userClass.contactsTable
        contactsList.DataBind()

        contactsPanel.Update()
    End Sub
    Protected Sub itemSelect_Event(sender As Object, e As System.EventArgs)
        Dim selectRadioBtn As RadioButton = CType(sender, RadioButton)
        Dim btnId As String = selectRadioBtn.ID
        Dim rowIndex As Integer = CType(btnId.Remove(btnId.IndexOf("selectContact"), "selectContact".Length), Integer)

        If selectRadioBtn.Checked Then

            For i = 0 To contactsList.Items.Count - 1
                If Not i = rowIndex Then
                    Dim deselectRadioBtns As RadioButton = contactsList.FindControl("selectContact" & i)
                    deselectRadioBtns.Checked = False
                End If
            Next
            messageDiv.InnerHtml = ""
            loadMessages(userClass.userName, CType(Trim(userClass.contactsTable(rowIndex)("phoneNum")), Long))
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "updateMessageDiv", "updateMessageDiv()", True)
            messagePanel.Update()
        End If

    End Sub
    Protected Sub itemEdit_Event(sender As Object, e As System.EventArgs)

    End Sub
    Private Sub sendTxtMessage(ByVal userNumber As String, ByVal outgoingPhoneNumber As String, ByVal msgText As String)
        Dim message As Twilio.Message = TwilioClass.SendMessage(userNumber, "+1" & outgoingPhoneNumber, msgText)


        If Not IsNothing(message.RestException) Then
            sendMsglbl.Text = message.RestException.Message
            sendMsglbl.Visible = True

            errorPanel.Update()
        Else
            sendMsglbl.Text = "Label"
            sendMsglbl.Visible = False

            errorPanel.Update()
            listenerToReceiveMsg(message.Sid, outgoingPhoneNumber)
        End If
    End Sub
    Public Sub listenerToReceiveMsg(ByVal messageSid As String, ByVal phoneNum As String)
        'Dim timer As New System.Timers.Timer(1000)
        If messageSidArr.Rows.Count < 1 Then
            Dim phoneNumberCol As New DataColumn
            Dim messageSidCol As New DataColumn

            phoneNumberCol.ColumnName = "phoneNum"
            messageSidCol.ColumnName = "messageSid"
            messageSidArr.Columns.Add(phoneNumberCol)
            messageSidArr.Columns.Add(messageSidCol)

            Dim newIncMessageRow As DataRow
            newIncMessageRow = messageSidArr.NewRow()

            newIncMessageRow("phoneNum") = phoneNum
            newIncMessageRow("messageSid") = messageSid

            messageSidArr.Rows.Add(newIncMessageRow)

            'Session("messageSid" & messageCount) = messageSid
            'Session("phoneNum" & messageCount) = phoneNum
        End If

    End Sub
    Protected Sub newMessage_event(sender As Object, e As EventArgs)

    End Sub

    Protected Sub userStatusLink_Click(sender As Object, e As EventArgs) Handles userStatusLink.Click
        userClass.loggedIn = False

        Session("userObj") = Nothing
        Session("dataAccessObj") = Nothing

    End Sub

    Protected Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick

        incMessageCount = TwilioClass.ListMessages.Messages.FindAll(Function(m) m.Direction = "inbound").Count
        Dim missingMsgCount As Integer = TwilioClass.ListMessages(userClass.acctPhoneNumber).Messages.Count - incMessageCount

        If outMessageCount = 0 Then
            outMessageCount = TwilioClass.ListMessages.Messages.FindAll(Function(m) m.Direction = "outbound-api").Count
        End If
        'If Not outMessageCount = TwilioClass.ListMessages.Messages.FindAll(Function(m) m.Direction = "outbound-api").Count Then
        '    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "updateMessageDiv", "updateMessageDiv()", True)
        '    messagePanel.Update()
        '    outMessageCount += 1
        'End If

        If lastMessage <> TwilioClass.ListMessages.Messages.FindAll(Function(m) m.Direction = "inbound").Item(0).Body _
            And lastDateTime <> TwilioClass.ListMessages.Messages.FindAll(Function(m) m.Direction = "inbound").Item(0).DateSent Then
            IncMessage = TwilioClass.ListMessages(userClass.acctPhoneNumber).Messages(0)

            If Not IsNothing(IncMessage.RestException) Then
                sendMsglbl.Text = "Error Receiving message: " & IncMessage.RestException.Message
                sendMsglbl.Visible = True

                errorPanel.Update()
            ElseIf IncMessage.Direction = "inbound" Then
                sendMsglbl.Text = "Label"
                sendMsglbl.Visible = False


                errorPanel.Update()
                phoneChatDA.saveNewMessage(userClass.userName, Mid(IncMessage.From, 3), IncMessage.Body, True, Date.Now)
                incMessageCount += 1
                messageDiv.InnerHtml = ""
                loadMessages(userClass.userName, Mid(IncMessage.From, 3))
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "updateMessageDiv", "updateMessageDiv()", True)
                messagePanel.Update()
                lastMessage = IncMessage.Body
                lastDateTime = IncMessage.DateSent
            End If
            'ElseIf missingMsgCount > 1 Then
            '    For i = missingMsgCount - 1 To 0 Step -1
            '        IncMessage = TwilioClass.ListMessages(userClass.acctPhoneNumber).Messages(i)

            '        If Not IsNothing(IncMessage.RestException) Then
            '            sendMsglbl.Text = "Error Receiving message: " & IncMessage.RestException.Message
            '            sendMsglbl.Visible = True

            '            errorPanel.Update()
            '        ElseIf IncMessage.Direction = "inbound" Then
            '            sendMsglbl.Text = "Label"
            '            sendMsglbl.Visible = False


            '            errorPanel.Update()
            '            phoneChatDA.saveNewMessage(userClass.userName, Mid(IncMessage.From, 3), IncMessage.Body, True, Date.Now)
            '            incMessageCount += 1
            '        End If
            '    Next
            '    messageDiv.InnerHtml = ""
            '    loadMessages(userClass.userName, Mid(IncMessage.From, 3))
            '    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "updateMessageDiv", "updateMessageDiv()", True)
            '    messagePanel.Update()
        End If

    End Sub
End Class
