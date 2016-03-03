
' ========================================================================================================================
'  Copyrights © 2016 - Farkhnda Rao.. All rights reserved.

Imports System
Imports System.Text
Imports System.Data
Imports System.IO
Imports System.Configuration
Imports MySQLFramework.MySQL_DAL.CommonEnums

Namespace MySQL_DAL

    Public Class BLLBroker

#Region "Variables"

        Protected _dbBroker As DbBroker

        Public ReadOnly Property CurrentDAL() As DbBroker
            Get
                Return Me._dbBroker
            End Get
        End Property


#End Region

#Region "Functions"

        Public Sub New()
            Me.New("")
        End Sub

        Public Sub New(ByVal name As String)
            name = name.Trim()
            If name = "" Then
                If ConfigurationManager.ConnectionStrings.Count < 2 Then Throw New Exception()
                name = ConfigurationManager.ConnectionStrings(1).Name.Trim()
            End If
            Me._dbBroker = New DbBroker(name)
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            Me._dbBroker = Nothing
        End Sub



#End Region

#Region "Select"

        Public Function GetBySP(ByVal tableName As String, ByVal procName As String, ByVal paramNames() As String, ByVal paramValues() As Object, ByVal rowCount As Integer) As DMSResult
            Return Me._dbBroker.GetBySP(tableName, procName, paramNames, paramValues, rowCount)
        End Function

        Public Function GetByTable(ByVal tableName As String, ByVal paramNames() As String, ByVal paramValues() As Object, ByVal rowCount As Integer) As DMSResult
            Return Me._dbBroker.GetByTable(tableName, paramNames, paramValues, rowCount)
        End Function

        Public Function GetByWhere(ByVal tableName As String, ByVal whereClause As String, ByVal rowCount As Integer) As DMSResult
            Return Me._dbBroker.GetByWhere(tableName, whereClause, rowCount)
        End Function

        Public Function GetBySQL(ByVal tableName As String, ByVal sql As String) As DMSResult
            Return Me._dbBroker.GetBySQL(tableName, sql)
        End Function

        Public Function GetMasterDetail(ByVal storedProcs() As DMSStoredProc, ByVal relations As ArrayList, ByVal keys As ArrayList) As DMSResult
            Return Me._dbBroker.GetMasterDetail(storedProcs, relations, keys)
        End Function

#End Region

#Region "Execute"

        Public Function ExecuteSP(ByVal procName As String, ByVal operation As enmOperationType, ByVal paramNames() As String, ByVal paramValues() As Object) As DMSResult
            Return Me._dbBroker.ExecuteSP(procName, operation, paramNames, paramValues)
        End Function

        Public Function ExecuteNonQuery(ByVal sql As String, ByVal operation As enmOperationType) As DMSResult
            Return Me._dbBroker.ExecuteNonQuery(sql, operation)
        End Function

        Public Function ExecuteScalar(ByVal sql As String, ByVal operation As enmOperationType) As DMSResult
            Return Me._dbBroker.ExecuteScalar(sql, operation)
        End Function

        Public Function ExecuteReader(ByVal sql As String, ByVal operation As enmOperationType) As DMSResult
            Return Me._dbBroker.ExecuteReader(sql, operation)
        End Function

#End Region

#Region "Save"

        Public Overridable Function Save(ByVal data As DMSData, ByVal operation As enmOperationType) As DMSResult
            Dim result As New DMSResult()
            Try
                If data.Fields.Count <> data.Values.Count Then Throw New Exception()
                If data.ExtraFields.Count <> data.ExtraValues.Count Then Throw New Exception()
                Dim allFields(data.Fields.Count + data.ExtraFields.Count) As String
                Dim allValues(data.Fields.Count + data.ExtraFields.Count) As Object
                Dim offset As Integer = 0
                If operation = enmOperationType.Update Then
                    allFields(offset) = data.PrimaryKeyName
                    allValues(offset) = data.PrimaryKeyValue
                Else
                    allFields(offset) = "UNID"
                    If data.NewIdName <> "" Then allFields(offset) = data.NewIdName
                    allValues(offset) = "0"
                End If
                offset = 1
                For counter As Integer = 0 To data.Fields.Count - 1
                    allFields(counter + offset) = data.Fields(counter)
                    allValues(counter + offset) = data.Values(counter)
                Next
                For counter As Integer = 0 To data.ExtraFields.Count - 1
                    allFields(data.Fields.Count + counter + offset) = data.ExtraFields(counter)
                    allValues(data.Fields.Count + counter + offset) = data.ExtraValues(counter)
                Next
                data.Fields = allFields
                data.Values = allValues
                data.ExtraFields = New String() {}
                data.ExtraValues = New Object() {}
                If operation = enmOperationType.Insert Then
                    Return Me._dbBroker.InsertBySP(data)
                Else
                    Return Me._dbBroker.UpdateBySP(data)
                End If
            Catch ex As Exception
                ExceptionLogger.LogException(ex)
                result.Code = 0
                result.Data = ex.Message
            End Try
            Return result
        End Function

#End Region

#Region "Insert"

        Public Function InsertBySP(ByVal data As DMSData) As DMSResult
            Return Me._dbBroker.InsertBySP(data)
        End Function

        Public Function InsertByTable(ByVal data As DMSData) As DMSResult
            Return Me._dbBroker.InsertByTable(data)
        End Function

        Public Function InsertByWhere(ByVal data As DMSData, ByVal whereClause As String) As DMSResult
            Return Me._dbBroker.InsertByWhere(data, whereClause)
        End Function

        Public Function InsertBySQL(ByVal sql As String, ByVal tableName As String) As DMSResult
            Return Me._dbBroker.InsertBySQL(sql, tableName)
        End Function

#End Region

#Region "Update"

        Public Function UpdateBySP(ByVal data As DMSData) As DMSResult
            Return Me._dbBroker.UpdateBySP(data)
        End Function

        Public Function UpdateByTable(ByVal data As DMSData, ByVal paramNames() As String, ByVal paramValues() As Object) As DMSResult
            Return Me._dbBroker.UpdateByTable(data, paramNames, paramValues)
        End Function

        Public Function UpdateByWhere(ByVal data As DMSData, ByVal whereClause As String) As DMSResult
            Return Me._dbBroker.UpdateByWhere(data, whereClause)
        End Function

        Public Function UpdateBySQL(ByVal sql As String) As DMSResult
            Return Me._dbBroker.UpdateBySQL(sql)
        End Function

#End Region

#Region "Delete"

        Public Function DeleteBySP(ByVal data As DMSData) As DMSResult
            Return Me._dbBroker.DeleteBySP(data)
        End Function

        Public Function DeleteByTable(ByVal tableName As String, ByVal paramNames() As String, ByVal paramValues() As Object) As DMSResult
            Return Me._dbBroker.DeleteByTable(tableName, paramNames, paramValues)
        End Function

        Public Function DeleteByWhere(ByVal tableName As String, ByVal whereClause As String) As DMSResult
            Return Me._dbBroker.DeleteByWhere(tableName, whereClause)
        End Function

        Public Function DeleteBySQL(ByVal sql As String) As DMSResult
            Return Me._dbBroker.DeleteBySQL(sql)
        End Function

#End Region

    End Class

End Namespace
