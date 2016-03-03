' ========================================================================================================================
'  Copyrights © 2016 - Farkhnda Rao.. All rights reserved.

Imports System
Imports System.Text
Imports System.Data
Imports System.IO
Imports System.Web.UI

Namespace MySQL_DAL

    Public Class ExceptionLogger

        Public Shared Sub LogException(ByVal ex As Exception)
            'LogException("Exception" + DMSConstants.LineBreak + ex.Message + DMSConstants.LineBreak + ex.StackTrace)
        End Sub

        Public Shared Sub LogException(ByVal message As String)
            'Try
            '    Dim path As String = DMSConstants.Path + DMSConstants.TextLogFolder + "/" + DMSConstants.LogFile + Now.ToString("yyyyddMMHH")
            '    Dim writer As StreamWriter = File.AppendText(path)
            '    writer.WriteLine(Now.ToString("MMM dd, yyyy HH:mm:ss") + DMSConstants.LineBreak + message + DMSConstants.LineBreak)
            '    writer.Close()
            'Catch
            'End Try
        End Sub

    End Class

End Namespace