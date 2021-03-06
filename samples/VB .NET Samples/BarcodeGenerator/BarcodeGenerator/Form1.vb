﻿Option Strict Off
Option Explicit On

Public Class Form1

    Dim barcodeformat As String

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Me.Icon = New Icon(GetType(Form), "wfc.ico")
        Me.cmbBarcodeFormat.DropDownStyle = ComboBoxStyle.DropDownList
        Me.cmbBarcodeFormat.Items.Add("CODE_39")
        Me.cmbBarcodeFormat.Items.Add("CODE_128")
        Me.cmbBarcodeFormat.Items.Add("PDF417")
        Me.cmbBarcodeFormat.Items.Add("QR_CODE")
        Me.cmbBarcodeFormat.SelectedIndex = 0

        Me.rdbBMP.Checked = True

        Me.txtBarcodeContent.Text = "Dynamsoft"
        Me.txtBarcodeLocationX.Text = "0"
        Me.txtBarcodeLocationY.Text = "0"
        Me.txtHumanReadableTxt.Text = "Dynamsoft"
        Me.txtBarcodeScale.Text = "1"

        Dim strDllPath As String
        Dim m_strCurrentDirectory As String
        m_strCurrentDirectory = Application.ExecutablePath
        Dim strPDFDllPath As String
        Dim pos As Integer
        pos = m_strCurrentDirectory.LastIndexOf("\Samples\")
        If (pos <> -1) Then
            m_strCurrentDirectory = m_strCurrentDirectory.Substring(0, m_strCurrentDirectory.IndexOf("\", pos)) + "\"
            strDllPath = m_strCurrentDirectory + "Redistributable\BarcodeResources\"
            strPDFDllPath = m_strCurrentDirectory + "Redistributable\PDFResources\"
        Else
            pos = m_strCurrentDirectory.LastIndexOf("\")
            m_strCurrentDirectory = m_strCurrentDirectory.Substring(0, m_strCurrentDirectory.IndexOf("\", pos)) + "\"
            strDllPath = m_strCurrentDirectory
            strPDFDllPath = m_strCurrentDirectory
        End If
        DynamicDotNetTwain1.LicenseKeys = "BAF81AB5515958BF519F7AAE2A318B3B;BAF81AB5515958BF6DA4299CBA3CC11D;BAF81AB5515958BF9C195A4722534974;BAF81AB5515958BFE96B7433DD28E75B;BAF81AB5515958BF3DBAF9AB37059787;BAF81AB5515958BF5291EEE0B030BD82"
        DynamicDotNetTwain1.BarcodeDllPath = strDllPath
        DynamicDotNetTwain1.PDFRasterizerDllPath = strPDFDllPath
        DynamicDotNetTwain1.IfShowCancelDialogWhenBarcodeOrOCR = True
        DynamicDotNetTwain1.MaxImagesInBuffer = 64
        DynamicDotNetTwain1.ScanInNewProcess = True

    End Sub

    Private Sub btnLoadImage_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLoadImage.Click
        Try
            Dim openfiledlg As New OpenFileDialog
            openfiledlg.Filter = "All Support Files|*.JPG;*.JPEG;*.JPE;*.JFIF;*.BMP;*.PNG;*.TIF;*.TIFF;*.PDF;*.GIF|JPEG|*.JPG;*.JPEG;*.JPE;*.Jfif|BMP|*.BMP|PNG|*.PNG|TIFF|*.TIF;*.TIFF|PDF|*.PDF|GIF|*.GIF"
            openfiledlg.FilterIndex = 0
            openfiledlg.Multiselect = True

            Dim strfilename As String
            If (openfiledlg.ShowDialog() = DialogResult.OK) Then
                For Each strfilename In openfiledlg.FileNames
                    Dim pos As Integer
                    pos = strfilename.LastIndexOf(".")
                    If (pos <> -1) Then
                        Dim strSuffix As String
                        strSuffix = strfilename.Substring(pos, strfilename.Length - pos).ToLower()
                        If (strSuffix.CompareTo(".pdf") = 0) Then
                            DynamicDotNetTwain1.ConvertPDFToImage(strfilename, 200)
                            Continue For
                        End If
                    End If
                    Me.DynamicDotNetTwain1.LoadImage(strfilename)
                Next
            End If
        Catch ex As Exception
            MessageBox.Show(Me.DynamicDotNetTwain1.ErrorString)
        End Try    
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Try
            If (Me.DynamicDotNetTwain1.HowManyImagesInBuffer > 0) Then

                Me.labMsg.Text = ""
                Me.labMsg2.Text = ""
                Dim dlgFileSave As New SaveFileDialog
                Dim strFile As String 'The file name use to save the acquired image

                If (rdbBMP.Checked = True) Then
                    dlgFileSave.Filter = "BMP File (*.bmp)|*.bmp"
                ElseIf (rdbJPEG.Checked = True) Then
                    dlgFileSave.Filter = "JPEG File (*.jpg)|*.jpg"
                ElseIf (rdbPNG.Checked = True) Then
                    dlgFileSave.Filter = "PNG File (*.png)|*.png"
                ElseIf (rdbTIFF.Checked = True) Then
                    dlgFileSave.Filter = "TIFF File (*.tif)|*.tif"
                Else
                    dlgFileSave.Filter = "PDF File (*.pdf)|*.pdf"
                End If

                dlgFileSave.InitialDirectory = CurDir()
                dlgFileSave.FileName = ""
                If (dlgFileSave.ShowDialog() = DialogResult.OK) Then
                    strFile = dlgFileSave.FileName
                    If (rdbBMP.Checked = True) Then
                        DynamicDotNetTwain1.SaveAsBMP(strFile, DynamicDotNetTwain1.CurrentImageIndexInBuffer)

                    ElseIf (rdbJPEG.Checked = True) Then
                        DynamicDotNetTwain1.SaveAsJPEG(strFile, DynamicDotNetTwain1.CurrentImageIndexInBuffer)

                    ElseIf (rdbPNG.Checked = True) Then
                        DynamicDotNetTwain1.SaveAsPNG(strFile, DynamicDotNetTwain1.CurrentImageIndexInBuffer)

                    ElseIf (rdbTIFF.Checked = True) Then
                        If (chbMultiPageTIFF.CheckState = 1) Then
                            DynamicDotNetTwain1.SaveAllAsMultiPageTIFF(strFile)
                        Else
                            DynamicDotNetTwain1.SaveAsTIFF(strFile, DynamicDotNetTwain1.CurrentImageIndexInBuffer)
                        End If
                    Else
                        If (chbMultiPagePDF.CheckState = 1) Then
                            DynamicDotNetTwain1.SaveAllAsPDF(strFile)
                        Else
                            DynamicDotNetTwain1.SaveAsPDF(strFile, DynamicDotNetTwain1.CurrentImageIndexInBuffer)
                        End If
                    End If
                End If
            Else
                Me.labMsg2.ForeColor = Color.Red
                Me.labMsg2.Text = "Please load an image first"
                Me.labMsg2.Location = New Point(Me.GroupBox4.Size.Width / 2 - Me.labMsg2.Size.Width / 2, Me.labMsg2.Location.Y)
            End If
        Catch ex As Exception
            MessageBox.Show(Me.DynamicDotNetTwain1.ErrorString)
        End Try 
    End Sub

    Private Sub btnAddBarcode_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddBarcode.Click
        Try
            If (Me.DynamicDotNetTwain1.HowManyImagesInBuffer > 0) Then
                Me.labMsg.Text = ""
                Me.labMsg2.Text = ""
                If (txtBarcodeContent.Text <> "" And txtBarcodeLocationX.Text <> "" And txtBarcodeLocationY.Text <> "" And txtBarcodeScale.Text <> "") Then
                    DynamicDotNetTwain1.AddBarcode(DynamicDotNetTwain1.CurrentImageIndexInBuffer, barcodeformat, txtBarcodeContent.Text, txtHumanReadableTxt.Text, CInt(txtBarcodeLocationX.Text), CInt(txtBarcodeLocationY.Text), CDbl(txtBarcodeScale.Text))
                Else
                    If (txtBarcodeContent.Text = "") Then
                        txtBarcodeContent.Focus()
                        Me.labMsg.ForeColor = Color.Red
                        Me.labMsg.Text = "BarcodeContent can not be empty"
                        Me.labMsg.Location = New Point(Me.GroupBox2.Size.Width / 2 - Me.labMsg.Size.Width / 2, Me.labMsg.Location.Y)
                    End If
                    If (txtBarcodeLocationX.Text = "") Then
                        txtBarcodeLocationX.Focus()
                        Me.labMsg.ForeColor = Color.Red
                        Me.labMsg.Text = "BarcodeLocationX can not be empty"
                        Me.labMsg.Location = New Point(Me.GroupBox2.Size.Width / 2 - Me.labMsg.Size.Width / 2, Me.labMsg.Location.Y)
                    End If
                    If (txtBarcodeLocationY.Text = "") Then
                        txtBarcodeLocationY.Focus()
                        Me.labMsg.ForeColor = Color.Red
                        Me.labMsg.Text = "BarcodeLocationY can not be empty"
                        Me.labMsg.Location = New Point(Me.GroupBox2.Size.Width / 2 - Me.labMsg.Size.Width / 2, Me.labMsg.Location.Y)
                    End If
                    If (txtBarcodeScale.Text = "") Then
                        txtBarcodeScale.Focus()
                        Me.labMsg.ForeColor = Color.Red
                        Me.labMsg.Text = "BarcodeScale can not be empty"
                        Me.labMsg.Location = New Point(Me.GroupBox2.Size.Width / 2 - Me.labMsg.Size.Width / 2, Me.labMsg.Location.Y)
                    End If
                End If
            Else
                Me.labMsg.ForeColor = Color.Red
                Me.labMsg.Text = "Please load an image first"
                Me.labMsg.Location = New Point(Me.GroupBox2.Size.Width / 2 - Me.labMsg.Size.Width / 2, Me.labMsg.Location.Y)
            End If
        Catch ex As Exception
            MessageBox.Show(Me.DynamicDotNetTwain1.ErrorString)
        End Try      
    End Sub

    Private Sub rdbBMP_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdbBMP.CheckedChanged
        chbMultiPagePDF.Checked = False
        chbMultiPagePDF.Enabled = False
        chbMultiPageTIFF.Checked = False
        chbMultiPageTIFF.Enabled = False
    End Sub

    Private Sub rdbJPEG_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdbJPEG.CheckedChanged
        chbMultiPagePDF.Checked = False
        chbMultiPagePDF.Enabled = False
        chbMultiPageTIFF.Checked = False
        chbMultiPageTIFF.Enabled = False
    End Sub

    Private Sub rdbPNG_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdbPNG.CheckedChanged
        chbMultiPagePDF.Checked = False
        chbMultiPagePDF.Enabled = False
        chbMultiPageTIFF.Checked = False
        chbMultiPageTIFF.Enabled = False
    End Sub

    Private Sub rdbTIFF_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdbTIFF.CheckedChanged
        chbMultiPagePDF.Checked = False
        chbMultiPagePDF.Enabled = False
        chbMultiPageTIFF.Checked = True
        chbMultiPageTIFF.Enabled = True
    End Sub

    Private Sub rdbPDF_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdbPDF.CheckedChanged
        chbMultiPagePDF.Checked = True
        chbMultiPagePDF.Enabled = True
        chbMultiPageTIFF.Checked = False
        chbMultiPageTIFF.Enabled = False
    End Sub

    Private Sub cmbBarcodeFormat_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbBarcodeFormat.SelectedIndexChanged
        Select Case (Me.cmbBarcodeFormat.SelectedIndex)
            Case 0
                barcodeformat = Dynamsoft.DotNet.TWAIN.Enums.Barcode.BarcodeFormat.CODE_39
            Case 1
                barcodeformat = Dynamsoft.DotNet.TWAIN.Enums.Barcode.BarcodeFormat.CODE_128
            Case 2
                barcodeformat = Dynamsoft.DotNet.TWAIN.Enums.Barcode.BarcodeFormat.PDF417
            Case 3
                barcodeformat = Dynamsoft.DotNet.TWAIN.Enums.Barcode.BarcodeFormat.QR_CODE
        End Select
    End Sub

    Private Sub txtBarcodeLocationX_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtBarcodeLocationX.KeyPress
        If Char.IsDigit(e.KeyChar) Or e.KeyChar = Chr(8) Then
            e.Handled = False
        Else
            e.Handled = True
        End If
    End Sub

    Private Sub txtBarcodeLocationY_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtBarcodeLocationY.KeyPress
        If Char.IsDigit(e.KeyChar) Or e.KeyChar = Chr(8) Then
            e.Handled = False
        Else
            e.Handled = True
        End If
    End Sub

    Private Sub txtBarcodeScale_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtBarcodeScale.KeyPress
        If Char.IsDigit(e.KeyChar) Or e.KeyChar = Chr(8) Or e.KeyChar = "." Then
            If e.KeyChar = "." And InStr(txtBarcodeScale.Text, ".") > 0 Then
                e.Handled = True
            Else
                e.Handled = False
            End If
        Else
            e.Handled = True
        End If
    End Sub
End Class
