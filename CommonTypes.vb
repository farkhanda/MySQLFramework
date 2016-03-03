' ========================================================================================================================
'  Copyrights © 2016 - Farkhnda Rao.. All rights reserved.
' ========================================================================================================================
'  File Name: DMS.Framework.CommonTypes
'  File Purpose: Common Structures & Types
' ========================================================================================================================

Imports System
Imports System.Text
Imports System.Data
Imports System.IO
Imports System.Web
Imports System.Web.UI.WebControls

Namespace MySQL_DAL


    Public Structure DMSResult
        Public Code As Integer
        Public Data As Object
    End Structure

    Public Structure DMSData
        Public ProcName As String
        Public TableName As String
        Public PrimaryKeyName As String
        Public PrimaryKeyValue As Integer
        Public NewIdName As String
        Public ForeignKeyName As String
        Public ForeignKeyValue As String
        Public Fields() As String
        Public Values() As Object
        Public ExtraFields() As String
        Public ExtraValues() As Object
    End Structure

    Public Structure DMSStoredProc
        Public ProcName As String
        Public TableName As String
        Public ParamFields() As String
        Public ParamValues() As String
    End Structure

    Public Structure DMSIPInfo
        Public CountryCode As String
        Public Country As String
        Public RegionCode As String
        Public Region As String
        Public City As String
        Public ZipCode As String
        Public Latitude As Double
        Public Longitude As Double
        Public MetroCode As Integer
        Public AreaCode As Integer
        Public ISP As String
        Public Organization As String
        Public Location As String
        Public Anthem As String
        Public IsNatDay As Boolean
    End Structure



    Public Structure DMSTime
        Public LocalTime As DateTime
        Public MeanTime As DateTime
        Public Period As String
        Public Display As String
    End Structure

End Namespace
