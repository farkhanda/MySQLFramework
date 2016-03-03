' ========================================================================================================================
'  Copyrights © 2016 - Farkhnda Rao.. All rights reserved.
' ========================================================================================================================
'  File Name: MySQLFramework.DbBroker
'  File Purpose: Controller of Data Access (Lower-most) Layer
' ========================================================================================================================

Imports System
Imports System.Text
Imports System.Data
Imports System.IO
Imports MySql.Data.MySqlClient

Imports MySQLFramework.MySQL_DAL.CommonEnums
Namespace MySQL_DAL

    Public Class DbBroker

#Region "Variables"

        Private Shared IDENTITY_TEXT As String = "SELECT IDENT_CURRENT('{TABLE_NAME}')"

        Private _connection As DbConnection
        Private _command As MySqlCommand
        Private _adapter As MySqlDataAdapter
        Private _parameter As MySqlParameter

        Private _data As DataSet
        Private _proc As String
        Private _result As DMSResult

#End Region

#Region "Properties"

        Public ReadOnly Property Connection() As MySqlConnection
            Get
                Return Me._connection.Connection
            End Get
        End Property

        Public ReadOnly Property Command() As MySqlCommand
            Get
                Return Me._command
            End Get
        End Property

#End Region

#Region "Functions"

        Public Sub New()
            Me.New("")
        End Sub

        Public Sub New(ByVal name As String)
            Me._connection = New DbConnection(name)
            Me._command = New MySqlCommand()
            Me._adapter = New MySqlDataAdapter()
            Me._parameter = New MySqlParameter()
            Me._data = New DataSet()
            Me._connection.Connect()
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            Me._adapter = Nothing
            Me._data = Nothing
            Me._command = Nothing
            Me._parameter = Nothing
            Me._connection = Nothing
        End Sub

        Private Sub MergeFields(ByRef data As DMSData, ByRef allFields() As String, ByRef allValues() As Object)
            Dim counter As Integer = 0
            If data.Fields.Count <> data.Values.Count Then Throw New Exception()
            If data.ExtraFields.Count <> data.ExtraValues.Count Then Throw New Exception()
            ReDim allFields(data.Fields.Count + data.ExtraFields.Count - 1)
            ReDim allValues(data.Fields.Count + data.ExtraFields.Count - 1)
            For counter = 0 To data.Fields.Count - 1
                allFields(counter) = data.Fields(counter)
                allValues(counter) = data.Values(counter)
            Next
            For counter = 0 To data.ExtraFields.Count - 1
                allFields(data.Fields.Count + counter) = data.ExtraFields(counter)
                allValues(data.Fields.Count + counter) = data.ExtraValues(counter)
            Next
        End Sub

#End Region

#Region "Transaction"

        Public Sub BeginTrans()
            Me._connection.BeginTrans()
        End Sub

        Public Sub CommitTrans()
            Me._connection.CommitTrans()
        End Sub

        Public Sub RollbackTrans()
            Me._connection.RollbackTrans()
        End Sub

#End Region

#Region "Select"

        Public Function GetBySP(ByVal tableName As String, ByVal procName As String, ByVal paramNames() As String, ByVal paramValues() As Object, ByVal rowCount As Integer) As DMSResult
            Try
                Me._result = New DMSResult()
                If Me._data.Tables.Contains(tableName) Then Me._data.Tables.Remove(tableName)
                Dim sql As String = "EXEC " + procName + " WITH "
                Me._command = New MySqlCommand(procName, Me._connection.Connection)
                Me._command.CommandType = CommandType.StoredProcedure
                If paramNames.Count <> paramValues.Count Then Throw New Exception()
                For counter As Integer = 0 To paramNames.Count - 1
                    Me._command.Parameters.AddWithValue("@" + paramNames(counter), paramValues(counter))
                    sql = sql + paramNames(counter) + "='" + CommonOps.GetSafeString(paramValues(counter)) + "' AND "
                Next
                sql = sql.Substring(0, sql.Length - 4)
                Me._adapter.SelectCommand = Me._command
                If rowCount = 0 Then
                    Me._adapter.Fill(Me._data, tableName)
                Else
                    Me._adapter.Fill(Me._data, 1, rowCount, tableName)
                End If
                'AuditTrail.LogData(sql, enmOperationType.Fetch)
                Me._result.Data = Me._data
                Me._result.Code = 1
            Catch ex As Exception
                Me._result.Code = 0
                Me._result.Data = ex.Message
            End Try
            Return Me._result
        End Function

        Public Function GetByTable(ByVal tableName As String, ByVal paramNames() As String, ByVal paramValues() As Object, ByVal rowCount As Integer) As DMSResult
            Try
                Me._result = New DMSResult()
                If Me._data.Tables.Contains(tableName) Then Me._data.Tables.Remove(tableName)
                Dim sql As String = "SELECT " + CStr(IIf(rowCount = 0, "*", "TOP " + CStr(rowCount) + "*")) + "  FROM " + tableName + " WHERE "
                If paramNames.Count <> paramValues.Count Then Throw New Exception()
                For counter As Integer = 0 To paramNames.Count - 1
                    sql = sql + paramNames(counter) + "='" + CommonOps.GetSafeString(paramValues(counter)) + "' AND "
                Next
                sql = sql.Substring(0, sql.Length - 4)
                Me._command = New MySqlCommand(sql, Me._connection.Connection)
                Me._command.CommandType = CommandType.Text
                Me._adapter.SelectCommand = Me._command
                Me._adapter.Fill(Me._data, tableName)
                'AuditTrail.LogData(sql, enmOperationType.Fetch)
                Me._result.Data = Me._data
                Me._result.Code = 1
            Catch ex As Exception
                Me._result.Code = 0
                Me._result.Data = ex.Message
            End Try
            Return Me._result
        End Function

        Public Function GetByWhere(ByVal tableName As String, ByVal whereClause As String, ByVal rowCount As Integer) As DMSResult
            Try
                Me._result = New DMSResult()
                If Me._data.Tables.Contains(tableName) Then Me._data.Tables.Remove(tableName)
                Dim sql As String = "SELECT " + CStr(IIf(rowCount = 0, "*", "TOP " + CStr(rowCount))) + " FROM " + tableName
                If whereClause.Trim() <> "" Then sql = sql + " WHERE " + whereClause.Trim()
                Me._command = New MySqlCommand(sql, Me._connection.Connection)
                Me._command.CommandType = CommandType.Text
                Me._adapter.SelectCommand = Me._command
                Me._adapter.Fill(Me._data, tableName)
                'AuditTrail.LogData(sql, enmOperationType.Fetch)
                Me._result.Data = Me._data
                Me._result.Code = 1
            Catch ex As Exception
                Me._result.Code = 0
                Me._result.Data = ex.Message
            End Try
            Return Me._result
        End Function

        Public Function GetBySQL(ByVal tableName As String, ByVal sql As String) As DMSResult
            Try
                Me._result = New DMSResult()
                If Me._data.Tables.Contains(tableName) Then Me._data.Tables.Remove(tableName)
                Me._command = New MySqlCommand(sql, Me._connection.Connection)
                Me._command.CommandType = CommandType.Text
                Me._adapter.SelectCommand = Me._command
                Me._adapter.Fill(Me._data, tableName)
                'AuditTrail.LogData(sql, enmOperationType.Fetch)
                Me._result.Data = Me._data
                Me._result.Code = 1
            Catch ex As Exception
                Me._result.Code = 0
                Me._result.Data = ex.Message
            End Try
            Return Me._result
        End Function

        Public Function GetMasterDetail(ByVal storedProcs() As DMSStoredProc, ByVal relations As ArrayList, ByVal keys As ArrayList) As DMSResult
            Try
                Me._result = New DMSResult()
                For counter As Integer = 0 To storedProcs.Length - 1
                    Me._proc = storedProcs(counter).ProcName
                    Me._command = New MySqlCommand(Me._proc, Me._connection.Connection)
                    Me._command.CommandType = CommandType.StoredProcedure
                    For param As Integer = 0 To storedProcs(counter).ParamFields.Length - 1
                        If storedProcs(counter).ParamFields(param) <> "" Then
                            Me._command.Parameters.AddWithValue("@" + storedProcs(counter).ParamFields(param), storedProcs(counter).ParamValues(param))
                        End If
                    Next
                    Me._adapter.SelectCommand = Me._command
                    Me._adapter.Fill(Me._data, storedProcs(counter).TableName)
                    Me._data.Tables(counter).TableName = storedProcs(counter).TableName
                Next
                Dim level As Integer = 0
                For counter As Integer = 0 To storedProcs.Length - 2
                    Dim relName As String = CStr(relations(counter))
                    Dim relCol1 As DataColumn = Me._data.Tables(storedProcs(counter).TableName).Columns(CStr(keys(counter)))
                    Dim relCol2 As DataColumn = Me._data.Tables(storedProcs(counter + 1).TableName).Columns(CStr(keys(counter)))
                    Me._data.Relations.Add(New DataRelation(relName, relCol1, relCol2))
                    level = level + 1
                Next
                Me._result.Data = Me._data
                Me._result.Code = 1
            Catch ex As System.Exception
                Me._result.Code = 0
                Me._result.Data = ex.Message
            End Try
            Return Me._result
        End Function

#End Region

#Region "Execute"

        Public Function ExecuteSP(ByVal procName As String, ByVal operation As enmOperationType, ByVal paramNames() As String, ByVal paramValues() As Object) As DMSResult
            Try
                Me._result = New DMSResult()
                Dim sql As String = "EXEC " + procName + " WITH "
                Me._command = New MySqlCommand(procName, Me._connection.Connection)
                Me._command.CommandType = CommandType.StoredProcedure
                If paramNames.Count <> paramValues.Count Then Throw New Exception()
                For counter As Integer = 0 To paramNames.Count - 1
                    Me._command.Parameters.AddWithValue("@" + paramNames(counter), paramValues(counter))
                    sql = sql + paramNames(counter) + "='" + CommonOps.GetSafeString(paramValues(counter)) + "' AND "
                Next
                sql = sql.Substring(0, sql.Length - 4)
                Dim result As Integer = Me._command.ExecuteNonQuery()
                'AuditTrail.LogData(sql, operation)
                Me._result.Data = result
                Me._result.Code = 1
            Catch ex As Exception
                Me._result.Code = 0
                Me._result.Data = ex.Message
            End Try
            Return Me._result
        End Function

        Public Function ExecuteNonQuery(ByVal sql As String, ByVal operation As enmOperationType) As DMSResult
            Try
                sql = sql.Replace("'null'", "null")
                Me._result = New DMSResult()
                Me._command = New MySqlCommand(sql, Me._connection.Connection)
                Me._command.CommandType = CommandType.Text
                Dim result As Integer = Me._command.ExecuteNonQuery()
                'AuditTrail.LogData(sql, operation)
                Me._result.Data = result
                Me._result.Code = 1
            Catch ex As Exception
                Me._result.Code = 0
                Me._result.Data = ex.Message
            End Try
            Return Me._result
        End Function

        Public Function ExecuteScalar(ByVal sql As String, ByVal operation As enmOperationType) As DMSResult
            Try
                sql = sql.Replace("'null'", "null")
                Me._result = New DMSResult()
                Me._command = New MySqlCommand(sql, Me._connection.Connection)
                Me._command.CommandType = CommandType.Text
                Dim result As Object = Me._command.ExecuteScalar()
                'AuditTrail.LogData(sql, operation)
                If result Is Nothing Then Throw New Exception()
                Me._result.Data = result
                Me._result.Code = 1
            Catch ex As Exception
                Me._result.Code = 0
                Me._result.Data = ex.Message
            End Try
            Return Me._result
        End Function

        Public Function ExecuteReader(ByVal sql As String, ByVal operation As enmOperationType) As DMSResult
            Try
                sql = sql.Replace("'null'", "null")
                Me._result = New DMSResult()
                Me._command = New MySqlCommand(sql, Me._connection.Connection)
                Me._command.CommandType = CommandType.Text
                Dim result As MySqlDataReader = Me._command.ExecuteReader()
                'AuditTrail.LogData(sql, operation)
                If result Is Nothing Then Throw New Exception()
                Me._result.Data = result
                Me._result.Code = 1
            Catch ex As Exception
                Me._result.Code = 0
                Me._result.Data = ex.Message
            End Try
            Return Me._result
        End Function

#End Region

#Region "Insert"

        Public Function InsertBySP(ByVal data As DMSData) As DMSResult
            Me._result = New DMSResult()
            Try
                Dim allFields() As String = {}
                Dim allValues() As Object = {}
                Me.MergeFields(data, allFields, allValues)
                Me._result = Me.ExecuteSP(data.ProcName, enmOperationType.Insert, allFields, allValues)
                If Me._result.Code = 0 Then Throw New Exception()
                Me._result = Me.ExecuteScalar(IDENTITY_TEXT.Replace("{TABLE_NAME}", data.TableName), enmOperationType.Internal)
                If Me._result.Code = 0 Then Throw New Exception()
                Return Me._result
            Catch
                Me._result.Code = 0
                Return Me._result
            End Try
        End Function

        Public Function InsertByTable(ByVal data As DMSData) As DMSResult
            Me._result = New DMSResult()
            Try
                Dim allFields() As String = {}
                Dim allValues() As Object = {}
                Me.MergeFields(data, allFields, allValues)
                Dim counter As Integer = 0
                Dim sql As String = "INSERT INTO " + data.TableName + " ( "
                For counter = 0 To allFields.Count - 2
                    sql = sql + allFields(counter) + ","
                Next
                sql = sql + allFields(counter) + ") VALUES ("
                For counter = 0 To allValues.Count - 2
                    sql = sql + "'" + allValues(counter).ToString() + "',"
                Next
                sql = sql + "'" + allValues(counter).ToString() + "') "
                'AuditTrail.LogData(sql, enmOperationType.Insert)
                Me._result = Me.ExecuteNonQuery(sql, enmOperationType.Insert)
                If Me._result.Code = 0 Then Throw New Exception()
                Me._result = Me.ExecuteScalar(IDENTITY_TEXT.Replace("{TABLE_NAME}", data.TableName), enmOperationType.Internal)
                If Me._result.Code = 0 Then Throw New Exception()
                Return Me._result
            Catch
                Me._result.Code = 0
                Return Me._result
            End Try
        End Function

        Public Function InsertByWhere(ByVal data As DMSData, ByVal whereClause As String) As DMSResult
            Me._result = New DMSResult()
            Try
                Dim allFields() As String = {}
                Dim allValues() As Object = {}
                Me.MergeFields(data, allFields, allValues)
                Dim counter As Integer = 0
                Dim sql As String = "INSERT INTO " + data.TableName + " ( "
                If allFields.Count <> allValues.Count Then Throw New Exception()
                For counter = 0 To allFields.Count - 1
                    sql = sql + allFields(counter) + ","
                Next
                sql = sql + allFields(counter) + ") VALUES ("
                For counter = 0 To allValues.Count - 1
                    sql = sql + "'" + allValues(counter).ToString() + "',"
                Next
                sql = sql + "'" + allValues(counter).ToString() + "') "
                If whereClause.Trim() <> "" Then sql = sql + " WHERE " + whereClause.Trim()
                'AuditTrail.LogData(sql, enmOperationType.Insert)
                Me._result = Me.ExecuteNonQuery(sql, enmOperationType.Insert)
                If Me._result.Code = 0 Then Throw New Exception()
                Me._result = Me.ExecuteScalar(IDENTITY_TEXT.Replace("{TABLE_NAME}", data.TableName), enmOperationType.Internal)
                If Me._result.Code = 0 Then Throw New Exception()
                Return Me._result
            Catch
                Me._result.Code = 0
                Return Me._result
            End Try
        End Function

        Public Function InsertBySQL(ByVal sql As String, ByVal tableName As String) As DMSResult
            Me._result = New DMSResult()
            Try
                Me._result = Me.ExecuteNonQuery(sql, enmOperationType.Insert)
                If Me._result.Code = 0 Then Throw New Exception()
                Me._result = Me.ExecuteScalar(IDENTITY_TEXT.Replace("{TABLE_NAME}", tableName), enmOperationType.Internal)
                If Me._result.Code = 0 Then Throw New Exception()
                Return Me._result
            Catch
                Me._result.Code = 0
                Return Me._result
            End Try
        End Function

#End Region

#Region "Update"

        Public Function UpdateBySP(ByVal data As DMSData) As DMSResult
            Me._result = New DMSResult()
            Try
                Dim allFields() As String = {}
                Dim allValues() As Object = {}
                Me.MergeFields(data, allFields, allValues)
                Return Me.ExecuteSP(data.ProcName, enmOperationType.Update, allFields, allValues)
            Catch
                Me._result.Code = 0
                Return Me._result
            End Try
        End Function

        Public Function UpdateByTable(ByVal data As DMSData, ByVal paramNames() As String, ByVal paramValues() As Object) As DMSResult
            Me._result = New DMSResult()
            Try
                Dim allFields() As String = {}
                Dim allValues() As Object = {}
                Me.MergeFields(data, allFields, allValues)
                Dim counter As Integer = 0
                Dim sql As String = "UPDATE " + data.TableName + " SET "
                For counter = 0 To allFields.Count - 2
                    sql = sql + allFields(counter) + "='" + allValues(counter).ToString() + "',"
                Next
                sql = sql + allFields(counter) + "='" + allValues(counter).ToString() + "' WHERE "
                If paramNames.Count <> paramValues.Count Then Throw New Exception()
                For counter = 0 To paramNames.Count - 2
                    sql = sql + paramNames(counter) + "='" + CommonOps.GetSafeString(paramValues(counter)) + "' AND "
                Next
                sql = sql + paramNames(counter) + "='" + CommonOps.GetSafeString(paramValues(counter)) + "' "
                'AuditTrail.LogData(sql, enmOperationType.Update)
                Return Me.ExecuteNonQuery(sql, enmOperationType.Update)
            Catch
                Me._result.Code = 0
                Return Me._result
            End Try
        End Function

        Public Function UpdateByWhere(ByVal data As DMSData, ByVal whereClause As String) As DMSResult
            Me._result = New DMSResult()
            Try
                Dim allFields() As String = {}
                Dim allValues() As Object = {}
                Me.MergeFields(data, allFields, allValues)
                Dim counter As Integer = 0
                Dim sql As String = "UPDATE " + data.TableName + " SET "
                For counter = 0 To allFields.Count - 2
                    sql = sql + allFields(counter) + "='" + allValues(counter).ToString() + "',"
                Next
                sql = sql + allFields(counter) + "='" + allValues(counter).ToString() + "' "
                If whereClause.Trim() <> "" Then sql = sql + " WHERE " + whereClause.Trim()
                'AuditTrail.LogData(sql, enmOperationType.Update)
                Return Me.ExecuteNonQuery(sql, enmOperationType.Update)
            Catch
                Me._result.Code = 0
                Return Me._result
            End Try
        End Function

        Public Function UpdateBySQL(ByVal sql As String) As DMSResult
            Me._result = New DMSResult()
            Try
                Return Me.ExecuteNonQuery(sql, enmOperationType.Update)
            Catch
                Me._result.Code = 0
                Return Me._result
            End Try
        End Function

#End Region

#Region "Delete"

        Public Function DeleteBySP(ByVal data As DMSData) As DMSResult
            Me._result = New DMSResult()
            Try
                Dim allFields() As String = {}
                Dim allValues() As Object = {}
                Me.MergeFields(data, allFields, allValues)
                Return Me.ExecuteSP(data.ProcName, enmOperationType.Delete, allFields, allValues)
            Catch
                Me._result.Code = 0
                Return Me._result
            End Try
        End Function

        Public Function DeleteByTable(ByVal tableName As String, ByVal paramNames() As String, ByVal paramValues() As Object) As DMSResult
            Me._result = New DMSResult()
            Try
                Dim sql As String = "DELETE FROM " + tableName + " WHERE "
                If paramNames.Count <> paramValues.Count Then Throw New Exception()
                For counter As Integer = 0 To paramNames.Count - 1
                    sql = sql + paramNames(counter) + "='" + CommonOps.GetSafeString(paramValues(counter)) + "' AND "
                Next
                sql = sql.Substring(0, sql.Length - 4)
                'AuditTrail.LogData(sql, enmOperationType.Delete)
                Return Me.ExecuteNonQuery(sql, enmOperationType.Delete)
            Catch
                Me._result.Code = 0
                Return Me._result
            End Try
        End Function

        Public Function DeleteByWhere(ByVal tableName As String, ByVal whereClause As String) As DMSResult
            Me._result = New DMSResult()
            Try
                Dim sql As String = "DELETE FROM " + tableName
                If whereClause.Trim() <> "" Then sql = sql + " WHERE " + whereClause.Trim()
                'AuditTrail.LogData(sql, enmOperationType.Delete)
                Return Me.ExecuteNonQuery(sql, enmOperationType.Delete)
            Catch
                Me._result.Code = 0
                Return Me._result
            End Try
        End Function

        Public Function DeleteBySQL(ByVal sql As String) As DMSResult
            Me._result = New DMSResult()
            Try
                Return Me.ExecuteNonQuery(sql, enmOperationType.Delete)
            Catch
                Me._result.Code = 0
                Return Me._result
            End Try
        End Function


#End Region

    End Class

End Namespace
