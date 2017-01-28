Imports System
Imports System.IO
Imports System.Web.HttpContext

Public Class DataAccess
    Dim users As DataTable

    Public Property usersTable() As DataTable
        Get
            Return users
        End Get
        Set(value As DataTable)
            users = value
        End Set
    End Property
    Public Function getUserStatus(ByVal userId As String)
        Dim MyDs As DataSet
        Dim MyDt As DataTable

        Try
            Dim sqlConnection As SqlClient.SqlConnection = New SqlClient.SqlConnection(ConfigurationManager.AppSettings("PhoneChatDB").ToString)
            Using cmd As New SqlClient.SqlCommand("getUserStatus")
                cmd.Parameters.AddWithValue("@webUserId", userId)
                cmd.CommandTimeout = 100
                sqlConnection.Open()
                Dim da As System.Data.SqlClient.SqlDataAdapter
                da = New System.Data.SqlClient.SqlDataAdapter(cmd)
                da.Fill(MyDs)
                sqlConnection.Close()
                MyDt = MyDs.Tables(0)

                Return MyDt(0)(0)

            End Using
        Catch ex As Exception
            WriteToLog("Error in lockUser Function: " + ex.Message)
            Return Nothing
        End Try
    End Function
    Public Sub getUserLogin(ByVal userId As String)
        Dim SQL
        'Dim DT As DataTable
        'Dim Param As SqlClient.SqlParameter
        Dim myDS As DataSet = New DataSet
        SQL = "getUserLogin"

        Try
            Dim sqlConnection As SqlClient.SqlConnection = New SqlClient.SqlConnection(ConfigurationManager.AppSettings("PhoneChatDB").ToString)

            Dim Command As New SqlClient.SqlCommand(SQL)
            Command.Connection = sqlConnection
            Command.CommandType = CommandType.StoredProcedure
            Command.Parameters.Add("@webUserId", SqlDbType.NVarChar)
            Command.Parameters(0).Value = userId
            Command.CommandTimeout = 100
            sqlConnection.Open()
            Dim da As System.Data.SqlClient.SqlDataAdapter
            da = New System.Data.SqlClient.SqlDataAdapter(Command)
            da.Fill(myDS)
            sqlConnection.Close()

            users = myDS.Tables(0)
        Catch ex As Exception
            WriteToLog("Error in getUserLogin Function: " + ex.Message)
        End Try
    End Sub
    Public Sub userSignUp(ByVal userId As String, ByVal userPW As String)
        Dim SQL
        Dim myDS As DataSet = New DataSet
        SQL = "newUser"

        Try
            Dim sqlConnection As SqlClient.SqlConnection = New SqlClient.SqlConnection(ConfigurationManager.AppSettings("PhoneChatDB").ToString)

            Dim Command As New SqlClient.SqlCommand(SQL)
            Command.Connection = sqlConnection
            Command.CommandType = CommandType.StoredProcedure
            Command.Parameters.Add("@webUserId", SqlDbType.NVarChar)
            Command.Parameters.Add("@userPW", SqlDbType.NVarChar)
            Command.Parameters(0).Value = userId
            Command.Parameters(1).Value = userPW
            Command.CommandTimeout = 100
            sqlConnection.Open()
            Command.ExecuteNonQuery()
            sqlConnection.Close()
        Catch ex As Exception
            WriteToLog("Error in UserSignUp Function: " + ex.Message)
        End Try

    End Sub
    Public Sub lockUser(ByVal userId As String)

        Try
            Dim sqlConnection As SqlClient.SqlConnection = New SqlClient.SqlConnection(ConfigurationManager.AppSettings("PhoneChatDB").ToString)
            Using cmd As New SqlClient.SqlCommand("LockUser")
                sqlConnection.Open()
                cmd.ExecuteNonQuery()
                sqlConnection.Close()
            End Using
        Catch ex As Exception
            WriteToLog("Error in lockUser Function: " + ex.Message)
        End Try
    End Sub
    Public Sub saveUserMessages(ByVal userMessagesDt As DataTable)
        Try
            For Each row As DataRow In userMessagesDt.Rows
                Dim sqlConnection As SqlClient.SqlConnection = New SqlClient.SqlConnection(ConfigurationManager.AppSettings("PhoneChatDB").ToString)
                Using cmd As New SqlClient.SqlCommand("newMessages")
                    cmd.Parameters.AddWithValue("@webUserId", row("webUserId").Value)
                    cmd.Parameters.AddWithValue("@phoneNum", row("phoneNum").Value)
                    cmd.Parameters.AddWithValue("@messageTxt", row("messageTxt").Value)
                    cmd.Parameters.AddWithValue("@messageTxt", row("Incoming").Value)
                    sqlConnection.Open()
                    cmd.ExecuteNonQuery()
                    sqlConnection.Close()
                End Using
            Next
        Catch ex As Exception
            WriteToLog("Error in saveUserMessages Function: " + ex.Message)
        End Try
    End Sub
    Public Sub saveUserContacts(ByVal userContactsDt As DataTable)
        Try
            For Each row As DataRow In userContactsDt.Rows
                Dim sqlConnection As SqlClient.SqlConnection = New SqlClient.SqlConnection(ConfigurationManager.AppSettings("PhoneChatDB").ToString)
                Using cmd As New SqlClient.SqlCommand("newContact")
                    cmd.Parameters.AddWithValue("@webUserId", row("webUserId"))
                    cmd.Parameters.AddWithValue("@phoneNum", row("phoneNum"))
                    sqlConnection.Open()
                    cmd.ExecuteNonQuery()
                    sqlConnection.Close()
                End Using
            Next
        Catch ex As Exception
            WriteToLog("Error in saveUserContacts Function: " + ex.Message)
        End Try
    End Sub
    Public Sub saveSingleContact(ByVal userId As String, ByVal phoneNumber As String)
        Try
            Dim sqlConnection As SqlClient.SqlConnection = New SqlClient.SqlConnection(ConfigurationManager.AppSettings("PhoneChatDB").ToString)
            Using cmd As New SqlClient.SqlCommand("newContact")
                cmd.Connection = sqlConnection
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.AddWithValue("@webUserId", userId)
                cmd.Parameters.AddWithValue("@phoneNum", phoneNumber)
                sqlConnection.Open()
                cmd.ExecuteNonQuery()
                sqlConnection.Close()
            End Using
        Catch ex As Exception
            WriteToLog("Error in saveSingleContacts Function: " + ex.Message)
        End Try
    End Sub
    Public Function getUserMessages(ByVal userId As String)
        Dim SQL
        Dim myDS As DataSet = New DataSet
        SQL = "getUserMessages"

        Try
            Dim sqlConnection As SqlClient.SqlConnection = New SqlClient.SqlConnection(ConfigurationManager.AppSettings("PhoneChatDB").ToString)

            Dim Command As New SqlClient.SqlCommand(SQL)
            Command.Connection = sqlConnection
            Command.CommandType = CommandType.StoredProcedure
            Command.Parameters.Add("@webUserId", SqlDbType.NVarChar)
            Command.Parameters(0).Value = userId
            Command.CommandTimeout = 100
            sqlConnection.Open()
            Dim da As System.Data.SqlClient.SqlDataAdapter
            da = New System.Data.SqlClient.SqlDataAdapter(Command)
            da.Fill(myDS)
            sqlConnection.Close()

            Return myDS.Tables(0)
        Catch ex As Exception
            WriteToLog("Error in getUserMessages Function: " + ex.Message)
            Return Nothing
        End Try
    End Function
    Public Function getUserContacts(ByVal userId As String)
        Dim SQL
        Dim myDS As DataSet = New DataSet
        SQL = "getUserContacts"

        Try
            Dim sqlConnection As SqlClient.SqlConnection = New SqlClient.SqlConnection(ConfigurationManager.AppSettings("PhoneChatDB").ToString)

            Dim Command As New SqlClient.SqlCommand(SQL)
            Command.Connection = sqlConnection
            Command.CommandType = CommandType.StoredProcedure
            Command.Parameters.Add("@webUserId", SqlDbType.NVarChar)
            Command.Parameters(0).Value = userId
            Command.CommandTimeout = 100
            sqlConnection.Open()
            Dim da As System.Data.SqlClient.SqlDataAdapter
            da = New System.Data.SqlClient.SqlDataAdapter(Command)
            da.Fill(myDS)
            sqlConnection.Close()

            Return myDS.Tables(0)
        Catch ex As Exception
            WriteToLog("Error in getUserContacts Function: " + ex.Message)
            Return Nothing
        End Try
    End Function
    Public Function getContactMessages(ByVal userId As String, ByVal phoneNumber As Long)
        Dim SQL
        Dim myDS As DataSet = New DataSet
        SQL = "getContactExchange"

        Try
            Dim sqlConnection As SqlClient.SqlConnection = New SqlClient.SqlConnection(ConfigurationManager.AppSettings("PhoneChatDB").ToString)

            Dim Command As New SqlClient.SqlCommand(SQL)
            Command.Connection = sqlConnection
            Command.CommandType = CommandType.StoredProcedure
            Command.Parameters.Add("@webUserId", SqlDbType.NVarChar)
            Command.Parameters.Add("@phoneNum", SqlDbType.BigInt)
            Command.Parameters(0).Value = userId
            Command.Parameters(1).Value = phoneNumber
            Command.CommandTimeout = 100
            sqlConnection.Open()
            Dim da As System.Data.SqlClient.SqlDataAdapter
            da = New System.Data.SqlClient.SqlDataAdapter(Command)
            da.Fill(myDS)
            sqlConnection.Close()

            Return myDS.Tables(0)
        Catch ex As Exception
            WriteToLog("Error in getContactMessages Function: " + ex.Message)
            Return Nothing
        End Try
    End Function
    Public Sub saveNewMessage(ByVal userId As String, ByVal phoneNumber As Long, ByVal messageTxt As String, ByVal incoming As Boolean, ByVal msgTime As DateTime)
        Try
            Dim sqlConnection As SqlClient.SqlConnection = New SqlClient.SqlConnection(ConfigurationManager.AppSettings("PhoneChatDB").ToString)
            Using cmd As New SqlClient.SqlCommand("newMessages")
                cmd.Connection = sqlConnection
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.AddWithValue("@webUserId", userId)
                cmd.Parameters.AddWithValue("@phoneNum", phoneNumber)
                cmd.Parameters.AddWithValue("@messageTxt", messageTxt)
                cmd.Parameters.AddWithValue("@incoming", incoming)
                cmd.Parameters.AddWithValue("@msgDateTime", msgTime)
                sqlConnection.Open()
                cmd.ExecuteNonQuery()
                sqlConnection.Close()
            End Using
        Catch ex As Exception
            WriteToLog("Error in saveUserContacts Function: " + ex.Message)
        End Try
    End Sub
    Public Sub DeleteContact(ByVal userId As String, ByVal phoneNumber As String)
        Try
            Dim sqlConnection As SqlClient.SqlConnection = New SqlClient.SqlConnection(ConfigurationManager.AppSettings("PhoneChatDB").ToString)
            Using cmd As New SqlClient.SqlCommand("removeContact")
                cmd.Connection = sqlConnection
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.AddWithValue("@webUserId", userId)
                cmd.Parameters.AddWithValue("@phoneNum", phoneNumber)
                sqlConnection.Open()
                cmd.ExecuteNonQuery()
                sqlConnection.Close()
            End Using
        Catch ex As Exception
            WriteToLog("Error in DeleteContact Function: " + ex.Message)
        End Try
    End Sub
    Public Sub WriteToLog(ByVal strLog As String)
        Try
            Dim oWrite As StreamWriter
            Dim strTime As String = Now.ToString()
            oWrite = New StreamWriter(Current.Server.MapPath("~") & "\LogFiles\PhoneChatErrorLog.txt", True)
            oWrite.WriteLine(strTime + " ---> " + strLog)
            oWrite.Flush()
            oWrite.Close()
        Catch ex As Exception
        End Try
    End Sub
End Class
