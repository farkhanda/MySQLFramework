' ========================================================================================================================
'  Copyrights © 2016 - Farkhnda Rao.. All rights reserved.
' ========================================================================================================================
'  File Name: MySQLFramework.DbConnection
'  File Purpose: 
' ========================================================================================================================

Imports System
Imports System.Text
Imports System.Data
Imports System.IO
Imports MySql.Data.MySqlClient
Imports System.Configuration

Namespace MySQL_DAL

    Friend Class DbConnection

#Region "Variables"

        Private _connectionString As String
        Private _connection As MySqlConnection
        Private _transaction As MySqlTransaction

#End Region

#Region "Properties"

        Public Property Connection() As MySqlConnection
            Get
                Return Me._connection
            End Get
            Set(ByVal value As MySqlConnection)
                Me._connection = value
            End Set
        End Property

        Public Property Transaction() As MySqlTransaction
            Get
                Return Me._transaction
            End Get
            Set(ByVal value As MySqlTransaction)
                Me._transaction = value
            End Set
        End Property

#End Region

#Region "Functions"

        Public Sub New(ByVal name As String)
            Me._connectionString = ConfigurationManager.ConnectionStrings(name).ConnectionString
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            Me._connection = Nothing
        End Sub

        Public Sub Connect()
            Try
                Me._connection = New MySqlConnection(Me._connectionString)
                Me._connection.Open()
            Catch ex As Exception
                ExceptionLogger.LogException(ex)
            End Try
        End Sub

        Public Sub Disconnect()
            Me._connection.Close()
        End Sub

        Public Sub BeginTrans()
            If Me._transaction IsNot Nothing Then
                Me._transaction = Me._connection.BeginTransaction()
            End If
        End Sub

        Public Sub CommitTrans()
            If Me._transaction IsNot Nothing Then
                Me._transaction.Commit()
            End If
        End Sub

        Public Sub RollbackTrans()
            If Me._transaction IsNot Nothing Then
                Me._transaction.Rollback()
            End If
        End Sub

#End Region

    End Class

End Namespace