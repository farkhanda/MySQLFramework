' ========================================================================================================================
'  Copyrights © 2016 - Farkhnda Rao.. All rights reserved.
'  File Name: MySQLFramework.CommonEnums
'  File Purpose: 
' ========================================================================================================================

Imports System
Imports System.Text
Imports System.Data
Imports System.IO

Namespace MySQL_DAL

    Public Class CommonEnums

        Public Enum enmOperationType
            Fetch = 1
            Insert = 2
            Update = 3
            Delete = 4
            Internal = 9
        End Enum

        Public Enum enmControlType
            Input = 1
            Display = 2
            Button = 3
            Link = 4
            Ignore = 8
            Other = 9
        End Enum

    End Class

End Namespace