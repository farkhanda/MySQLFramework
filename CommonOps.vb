' ========================================================================================================================
'  Copyrights © 2016 - Farkhnda Rao.. All rights reserved.
'  File Name: MySQLFramework.CommonOps
'  File Purpose: 
' ========================================================================================================================

Imports System
Imports System.Text
Imports System.Data
Imports System.Drawing
Imports System.IO
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports MySQLFramework.MySQL_DAL.CommonEnums

Namespace MySQL_DAL

    Public Class CommonOps

        Public Shared Function GetSafeString(ByVal currentItem As Object) As String
            Try
                If currentItem Is Nothing Then Throw New Exception()
                Return CStr(currentItem)
            Catch ex As Exception
                Return ""
            End Try
        End Function

        Public Shared Function GetSafeInteger(ByVal currentItem As Object) As Integer
            Try
                If currentItem Is Nothing Then Throw New Exception()
                Return CInt(currentItem)
            Catch ex As Exception
                Return 0
            End Try
        End Function

        Public Shared Function GetSafeDouble(ByVal currentItem As Object) As Double
            Try
                If currentItem Is Nothing Then Throw New Exception()
                Return CDbl(currentItem)
            Catch ex As Exception
                Return 0
            End Try
        End Function

        Public Shared Function GetSafeBoolean(ByVal currentItem As Object) As Boolean
            Try
                If currentItem Is Nothing Then Throw New Exception()
                Return CBool(currentItem)
            Catch ex As Exception
                Return False
            End Try
        End Function

        Public Shared Function GetSafeDBNull(ByVal currentItem As Object) As String
            If currentItem Is Nothing Then Return "NULL"
            If currentItem Is DBNull.Value Then Return "NULL"
            Return currentItem.ToString
        End Function

        Public Shared Function SetSafeDBNull(ByVal currentItem As Object) As Object
            If currentItem Is Nothing Then Return DBNull.Value
            If currentItem Is "" Then Return DBNull.Value
            Return currentItem.ToString
        End Function

        Public Shared Function ConvertBlankToDBNull(ByVal currentText As String) As String
            Return currentText.Replace("''", " NULL ")
        End Function

        Public Shared Function NormalizeString(ByVal text As String) As String
            text = text.Replace(vbCr, "")
            text = text.Replace(vbLf, "")
            text = text.Replace(vbCrLf, "")
            text = text.Replace(vbTab, "")
            Return text
        End Function

        Public Shared Function CleanString(ByVal text As String) As String
            Try
                text = text.Replace("'", "")
                text = text.Replace("""", "")
                text = text.Replace(";", "")
                text = text.Replace("|", "")
                text = text.Replace(")", "")
                text = text.Replace("(", "")
                text = text.Replace("<", "")
                text = text.Replace(">", "")
                text = text.Replace("--", "")
                text = text.Replace("exec", "")
                text = text.Replace("%3E", "")
                text = text.Replace("%22", "")
                text = text.Replace("%3C", "")
                text = text.Replace("%3D", "")
                text = text.Replace("%25", "")
                text = text.Replace("%27", "")
                text = text.Replace("%3B", "")
                text = text.Replace("%7C", "")
                text = text.Replace("%28", "")
                text = text.Replace("%29", "")
                text = text.Replace("%2D", "")
                text = text.Replace("%45", "")
                text = text.Replace("%65", "")
                Return text
            Catch
                Return text
            End Try
        End Function

        Public Shared Function ToProperCase(ByVal text As String) As String
            Try
                Dim buffer() As String
                Dim result As String
                text = Trim(text)
                If text <> "" Then
                    buffer = Split(text, " ")
                    result = ""
                    For counter As Integer = 0 To UBound(buffer)
                        If buffer(counter) <> "" Then
                            result = result & Replace(Left(buffer(counter), 1), Left(buffer(counter), 1), UCase(Left(buffer(counter), 1))) & LCase(Right(buffer(counter), Len(buffer(counter)) - 1)) & " "
                        End If
                    Next
                    Return result.Trim()
                Else
                    Return text
                End If
            Catch ex As Exception
                ExceptionLogger.LogException(ex)
                Return text
            End Try
        End Function



        Public Shared Function GenerateKey(ByVal lenght As Integer) As String
            Try
                Dim chars As String = "abcdefgijkmnopqrstwxyzABCDEFGHJKLMNPQRSTWXYZ0123456789*$-+?_=!%{}/"
                If lenght < 1 Then lenght = 8
                Dim generator As New Random()
                Dim key As String = ""
                For counter As Integer = 0 To lenght
                    key += chars.Substring(generator.Next(0, chars.Length - 1), 1)
                Next
                Return key
            Catch ex As Exception
                ExceptionLogger.LogException(ex)
                Return ""
            End Try
        End Function


        Public Shared Sub EnableFields(ByVal ParamArray controls() As WebControl)
            Try
                For Each currentControl As WebControl In controls
                    If TypeOf (currentControl) Is TextBox Then
                        CType(currentControl, TextBox).ReadOnly = False
                    ElseIf TypeOf (currentControl) Is HyperLink Then
                        currentControl.Enabled = True
                    ElseIf TypeOf (currentControl) Is RadioButtonList Then
                        currentControl.Enabled = True
                    ElseIf TypeOf (currentControl) Is Label Then
                        currentControl.Enabled = True
                    ElseIf TypeOf (currentControl) Is ImageButton Then
                        currentControl.Enabled = True
                    ElseIf TypeOf (currentControl) Is DropDownList Then
                        currentControl.Enabled = True
                    ElseIf TypeOf (currentControl) Is LinkButton Then
                        currentControl.Enabled = True
                    End If
                Next
            Catch ex As Exception
                ExceptionLogger.LogException(ex)
            End Try
        End Sub

        Public Shared Sub DisableFields(ByVal ParamArray controls() As WebControl)
            Try
                For Each currentControl As WebControl In controls
                    If TypeOf (currentControl) Is TextBox Then
                        CType(currentControl, TextBox).ReadOnly = True
                    ElseIf TypeOf (currentControl) Is HyperLink Then
                        currentControl.Enabled = False
                    ElseIf TypeOf (currentControl) Is RadioButtonList Then
                        currentControl.Enabled = False
                    ElseIf TypeOf (currentControl) Is Label Then
                        currentControl.Enabled = False
                    ElseIf TypeOf (currentControl) Is ImageButton Then
                        currentControl.Enabled = False
                    ElseIf TypeOf (currentControl) Is LinkButton Then
                        currentControl.Enabled = False
                    ElseIf TypeOf (currentControl) Is DropDownList Then
                        currentControl.Enabled = False
                    End If
                Next
            Catch ex As Exception
                ExceptionLogger.LogException(ex)
            End Try
        End Sub

        Public Shared Sub EnableControls(ByVal ParamArray controls() As WebControl)
            Try
                For Each currentControl As WebControl In controls
                    If TypeOf (currentControl) Is TextBox Then
                        CType(currentControl, TextBox).ReadOnly = False
                    ElseIf TypeOf (currentControl) Is HyperLink Then
                        currentControl.Enabled = True
                    ElseIf TypeOf (currentControl) Is RadioButtonList Then
                        currentControl.Enabled = True
                    ElseIf TypeOf (currentControl) Is Label Then
                        currentControl.Enabled = True
                    ElseIf TypeOf (currentControl) Is ImageButton Then
                        currentControl.Enabled = True
                    ElseIf TypeOf (currentControl) Is DropDownList Then
                        currentControl.Enabled = True
                    ElseIf TypeOf (currentControl) Is LinkButton Then
                        currentControl.Enabled = True
                    ElseIf TypeOf (currentControl) Is Button Then
                        currentControl.Enabled = True
                    ElseIf TypeOf (currentControl) Is CheckBox Then
                        currentControl.Enabled = True
                    ElseIf TypeOf (currentControl) Is ListBox Then
                        currentControl.Enabled = True
                    End If
                Next
            Catch ex As Exception
                ExceptionLogger.LogException(ex)
            End Try
        End Sub

        Public Shared Sub DisableControls(ByVal ParamArray controls() As WebControl)
            Try
                For Each currentControl As WebControl In controls
                    If TypeOf (currentControl) Is TextBox Then
                        CType(currentControl, TextBox).ReadOnly = True
                    ElseIf TypeOf (currentControl) Is HyperLink Then
                        currentControl.Enabled = False
                    ElseIf TypeOf (currentControl) Is RadioButtonList Then
                        currentControl.Enabled = False
                    ElseIf TypeOf (currentControl) Is DropDownList Then
                        currentControl.Enabled = False
                    ElseIf TypeOf (currentControl) Is Label Then
                        currentControl.Enabled = False
                    ElseIf TypeOf (currentControl) Is ImageButton Then
                        currentControl.Enabled = False = True
                    ElseIf TypeOf (currentControl) Is LinkButton Then
                        currentControl.Enabled = False
                    ElseIf TypeOf (currentControl) Is Button Then
                        currentControl.Enabled = False
                    ElseIf TypeOf (currentControl) Is CheckBox Then
                        currentControl.Enabled = False
                    ElseIf TypeOf (currentControl) Is ListBox Then
                        currentControl.Enabled = False
                    End If
                Next
            Catch ex As Exception
                ExceptionLogger.LogException(ex)
            End Try
        End Sub

        Public Shared Sub MakeVisibleFalse(ByVal ParamArray controls() As WebControl)
            Try
                For Each currentControl As WebControl In controls
                    Try
                        currentControl.Visible = False
                    Catch
                    End Try
                Next
            Catch ex As Exception
                ExceptionLogger.LogException(ex)
            End Try
        End Sub

        Public Shared Sub MakeVisibleTrue(ByVal ParamArray controls() As WebControl)
            Try
                For Each currentControl As WebControl In controls
                    Try
                        currentControl.Visible = True
                    Catch
                    End Try
                Next
            Catch ex As Exception
                ExceptionLogger.LogException(ex)
            End Try
        End Sub

    End Class

End Namespace