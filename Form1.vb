'/*
' *    ™T©ReMeTaL FileSorter.
' *    Copyright (C) 2022 ™T©ReMeTaL.
' *    Licensed under the Apache License, Version 2.0 (the "License");
' *    you may Not use this file except in compliance with the License.
' *
' *    You may obtain a copy of the License at
' *    http://www.apache.org/licenses/LICENSE-2.0
' *
' *    Unless required by applicable law Or agreed to in writing, software
' *    distributed under the License Is distributed on an "AS IS" BASIS,
' *    WITHOUT WARRANTIES Or CONDITIONS OF ANY KIND, either express Or implied.
' *    See the License for the specific language governing permissions And
' *    limitations under the License.
' *
' *   ************************************************************************
' *     build.gradle : Copyright(c) 2022 ™T©ReMeTaL.
' *   ************************************************************************
' *      Computer Scientist: David W. Rick
' *      Date: 2/5/22, 3:58 PM
' *      Program Name: FileSorter
' *      File: Form1.vb
' *      Last Modified: 2/24/22, 11:53 AM
' *   *************************************************************************/
Public Class Form1
    Public Event RunWorkerCompletedEventArgs(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs)
    Public Event RunWorkerCompleted As System.ComponentModel.RunWorkerCompletedEventHandler
    Public ReadOnly Property CurrentVersion As String = My.Application.Info.Version.Major & "." & My.Application.Info.Version.Minor & "." & My.Application.Info.Version.Build & "." & My.Application.Info.Version.Revision
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        TextBox4.Text = ""
        Button1.Enabled = False
        Refresh()
        BackgroundWorker1.RunWorkerAsync()
        Timer1.Start()
    End Sub
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Text = $"{Text}({CurrentVersion})"
    End Sub
    Private Sub TextBox1_Click(sender As Object, e As EventArgs) Handles TextBox1.Click
        Dim Mp As New OpenFileDialog With {
                .Filter = "*|*.*",
                .FileName = "",
                .CheckFileExists = True,
                .AutoUpgradeEnabled = True,
                .Multiselect = False,
                .InitialDirectory = FileIO.SpecialDirectories.Desktop
            }
        If Mp.ShowDialog = DialogResult.OK Then
            TextBox1.Text = Mp.FileName
            Button1.Enabled = True
        End If
    End Sub

    Private Sub BackgroundWorker1_RunWorkerCompleted(ByVal sender As Object,
   ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BackgroundWorker1.RunWorkerCompleted
        TextBox4.Text = "Done"
        Timer1.Stop()
        Button1.Enabled = True
    End Sub

    Private Sub BackgroundWorker1_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork
        If FileIO.FileSystem.FileExists(TextBox1.Text) Then
            Try
                Dim filReader As IO.StreamReader
                Dim filReader3 As IO.StreamReader
                Dim Text1 As IO.StreamReader
                Dim filWriter As IO.StreamWriter
                FileIO.FileSystem.CopyFile(TextBox1.Text, "tmp.tmp", True)
                Text1 = FileIO.FileSystem.OpenTextFileReader(TextBox1.Text)
                Dim tmpLine As String
                Dim Text1Line As String
                Dim count As Integer
                Dim tr As Boolean = False
                Do Until Text1.EndOfStream
                    count = 0
                    If CheckBox1.Checked Then
                        Text1Line = Text1.ReadLine.Replace(", ", ",").Replace(", ", ",")
                        Text1Line = Text1Line.Replace(",", ", ")
                    Else
                        Text1Line = Text1.ReadLine
                    End If
                    If CheckBox2.Checked Then
                        Text1Line = Text1Line.ToLower()
                    End If
                    If CheckBox3.Checked Then
                        Text1Line = Text1Line.Replace("reseller", "RESELLER").Replace("Reseller", "RESELLER").Replace("direct", "DIRECT").Replace("Direct", "DIRECT")
                    End If
                    'Text1Line = Text1.ReadLine.Replace(", ", ",").ToLower().Replace("reseller", "RESELLER").Replace("direct", "DIRECT")
                    filReader = FileIO.FileSystem.OpenTextFileReader("tmp.tmp")
                    Do Until filReader.EndOfStream
                        'tmpLine = filReader.ReadLine.Replace(", ", ",").ToLower().Replace("reseller", "RESELLER").Replace("direct", "DIRECT")
                        If CheckBox1.Checked Then
                            tmpLine = filReader.ReadLine.Replace(", ", ",").Replace(", ", ",")
                            tmpLine = tmpLine.Replace(",", ", ")
                        Else
                            tmpLine = filReader.ReadLine
                        End If
                        If CheckBox2.Checked Then
                            tmpLine = tmpLine.ToLower()
                        End If
                        If CheckBox3.Checked Then
                            tmpLine = tmpLine.Replace("reseller", "RESELLER").Replace("Reseller", "RESELLER").Replace("direct", "DIRECT").Replace("Direct", "DIRECT")
                        End If
                        If tmpLine = Text1Line Then
                            count += 1
                            If count = 1 Then
                                If FileIO.FileSystem.FileExists("tmp1.tmp") Then
                                    filReader3 = FileIO.FileSystem.OpenTextFileReader("tmp1.tmp")
                                    Do Until filReader3.EndOfStream
                                        If filReader3.ReadLine = Text1Line Then
                                            tr = True
                                        End If
                                    Loop
                                    filReader3.Close()
                                    If tr = True Then
                                        tr = False
                                    Else
                                        filWriter = FileIO.FileSystem.OpenTextFileWriter("tmp1.tmp", True)
                                        filWriter.WriteLine(tmpLine)
                                        filWriter.Close()
                                    End If
                                Else
                                    filWriter = FileIO.FileSystem.OpenTextFileWriter("tmp1.tmp", False)
                                    filWriter.WriteLine(tmpLine)
                                    filWriter.Close()
                                End If
                            End If
                        End If
                    Loop
                    filReader.Close()
                Loop
                Text1.Close()
            Catch ex As Exception
            End Try
            FileIO.FileSystem.DeleteFile("tmp.tmp")
            FileIO.FileSystem.MoveFile("tmp1.tmp", TextBox1.Text & "(New).txt", True)
        End If

    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        If ProgressBar1.Value = 100 Then
            ProgressBar1.Value = 0
        Else
            ProgressBar1.Value += 5
        End If
    End Sub

    Private Sub ToolTip1_Popup(sender As Object, e As PopupEventArgs) Handles ToolTip1.Popup

    End Sub
End Class
