Public Class User
    Dim signedIn As Boolean
    Dim userNameVar As String
    Dim messages As DataTable
    Dim contacts As DataTable
    Dim userLocked As Boolean
    Dim accountPhoneNumber As String

    Public Property userName As String
        Get
            Return userNameVar
        End Get
        Set(value As String)
            userNameVar = value
        End Set
    End Property
    Public Property acctPhoneNumber As String
        Get
            Return accountPhoneNumber
        End Get
        Set(value As String)
            accountPhoneNumber = value
        End Set
    End Property

    Public Property loggedIn() As Boolean
        Get
            Return signedIn
        End Get
        Set(value As Boolean)
            signedIn = value
        End Set
    End Property

    Public Property messagesTable() As DataTable
        Get
            Return messages
        End Get
        Set(value As DataTable)
            messages = value
        End Set
    End Property

    Public Property contactsTable() As DataTable
        Get
            Return contacts
        End Get
        Set(value As DataTable)
            contacts = value
        End Set
    End Property
    Public Property userStatus() As Boolean
        Get
            Return userLocked
        End Get
        Set(value As Boolean)
            userLocked = value
        End Set
    End Property

End Class
