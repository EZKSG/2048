Public Class Form1
    Private B(,) As Button
    Private Score As Integer = 0
    Private beforeMoveStr As String = ""
    Private IsWin As Boolean = False
    Private IsStart As Boolean = False
#Region "WritePrivateProfileString"
    'Public Path As String = System.Windows.Forms.Application.StartupPath & "\2048_Save.dat"
    'Public Declare Function GetPrivateProfileString Lib "kernel32" Alias "GetPrivateProfileStringA" (ByVal lpApplicationName As String, ByVal lpKeyName As String, ByVal lpDefault As String, ByVal lpReturnedString As String, ByVal nSize As Int32, ByVal lpFileName As String) As Int32
    'Public Declare Function WritePrivateProfileString Lib "kernel32" Alias "WritePrivateProfileStringA" (ByVal lpApplicationName As String, ByVal lpKeyName As String, ByVal lpString As String, ByVal lpFileName As String) As Int32
    Protected Overrides Function ProcessDialogKey(ByVal keyData As Keys) As Boolean
        If keyData = Keys.Left Or keyData = Keys.Right Or keyData = Keys.Up Or keyData = Keys.Down Then
            Return False
        Else
            Return MyBase.ProcessDialogKey(keyData)
        End If
    End Function
    'Public Function GetINI(ByVal Section As String, ByVal AppName As String, ByVal lpDefault As String, ByVal FileName As String) As String
    '    Dim Str As String = LSet(Str, 256)
    '    GetPrivateProfileString(Section, AppName, lpDefault, Str, Len(Str), FileName)
    '    Return Microsoft.VisualBasic.Left(Str, InStr(Str, Chr(0)) - 1)
    'End Function
    'Public Function WriteINI(ByVal Section As String, ByVal AppName As String, ByVal lpDefault As String, ByVal FileName As String) As Long
    '    WriteINI = WritePrivateProfileString(Section, AppName, lpDefault, FileName)
    'End Function
#End Region

#Region "按下按鍵"
    ''' <summary>
    ''' 按下按鍵
    ''' 位移、相加、位移、新按钮、上色
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Form1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        IsNewbt()
        If e.KeyCode = Keys.Up Or e.KeyCode = Keys.W Then
            bt_Up()
            UpPlus()
            bt_Up()
        End If
        If e.KeyCode = Keys.Down Or e.KeyCode = Keys.S Then
            bt_Down()
            DownPlus()
            bt_Down()
        End If
        If e.KeyCode = Keys.Left Or e.KeyCode = Keys.A Then
            bt_Left()
            LeftPlus()
            bt_Left()
        End If
        If e.KeyCode = Keys.Right Or e.KeyCode = Keys.D Then
            bt_Right()
            RightPlus()
            bt_Right()
        End If
        Newbt()
        Bt_Color()
        'WriteINI("Game", "IsStart", IsStart, Path)
        If IsStart = True Then
            Dim a As String = ""
            For x = 0 To 3
                For y = 0 To 3
                    a = a & B(x, y).Text & "|"
                Next
            Next
            'WriteINI("Game", "Game", a, Path)
            'WriteINI("Game", "IsWin", IsWin, Path)
            'WriteINI("Game", "Score", Score, Path)
        End If
    End Sub

#End Region

#Region "加載窗體"
    ''' <summary>
    ''' 加載窗體
    ''' 加2個塊
    ''' 上色
    ''' 記錄舊記錄
    ''' IsWin = False
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Randomize() 'Initializes the random-number generator.
        Me.KeyPreview = True
        B = New Button(3, 3) {}
        For x As Integer = 0 To 3
            For y As Integer = 0 To 3
                B(x, y) = New Button()
                Panel1.Controls.Add(B(x, y))
                B(x, y).Left = 15 + 115 * x
                B(x, y).Top = 15 + 115 * y
                B(x, y).Width = 100
                B(x, y).Height = 100
                B(x, y).Font = New Font("黑体", 20.0F, FontStyle.Bold, GraphicsUnit.Point, CByte(134))
                B(x, y).Name = "B" & (x + y * 4).ToString()
                B(x, y).BackColor = Color.FromArgb(204, 192, 180)
                B(x, y).ForeColor = Color.FromArgb(187, 173, 160)
                B(x, y).FlatStyle = FlatStyle.Flat
                B(x, y).Enabled = False
            Next
        Next
        If Score = 0 Then        'GetINI("Game", "HighScore", "", Path) = ""
            'WriteINI("Game", "HighScore", 0, Path)
            For x As Integer = 0 To 3
                For y As Integer = 0 To 3
                    B(x, y).Text = ""
                    B(x, y).BackColor = Color.FromArgb(204, 192, 180)
                Next
            Next
            Dim row_i As Integer, col_j As Integer
            Dim random As New Random
            Dim a As Integer = 0
            While a < 2
                row_i = random.Next(4)
                col_j = random.Next(4)
                If B(row_i, col_j).Text = "" Then
                    B(row_i, col_j).Text = "2"
                    a += 1
                End If
            End While
            Score = 0
        Else
            'Label2.Text = GetINI("Game", "HighScore", "", Path)
            'If GetINI("Game", "IsStart", "", Path) = "True" Then
            '    Score = GetINI("Game", "Score", "", Path)
            '    Dim a, i
            '    a = Split(GetINI("Game", "Game", "", Path), "|")
            '    i = 0
            '    For x As Integer = 0 To 3
            '        For y As Integer = 0 To 3
            '            B(x, y).Text = a(i)
            '            i += 1
            '        Next
            '    Next
            '    IsStart = True
            'Else
            '    GameInit()
            'End If
        End If
        Bt_Color()
        IsNewbt()
        IsWin = False
        'If GetINI("Game", "IsWin", "", Path) = "True" Then
        '    IsWin = True
        'Else
        '    IsWin = False
        'End If
        ''added below
        Debug.Print("test")
    End Sub
#End Region

#Region "遊戲初始化"
    ''' <summary>
    ''' 遊戲初始化
    ''' 按鈕賦空值及上色、隨機2個數（小於4，1/6的概率为4,    5/6的概率为2）賦給隨機2個方塊、上色、分數賦值0
    ''' </summary>
    Private Sub GameInit()                                                                                          '遊戲初始化
        For x As Integer = 0 To 3                                                                                    '按钮全置灰色
            For y As Integer = 0 To 3
                B(x, y).Text = ""
                B(x, y).BackColor = Color.FromArgb(204, 192, 180)                                 '按钮全置灰色
            Next
        Next
        Dim row_i As Integer, col_j As Integer                                                                             '首次新給兩個方塊text賦值
        Dim random As New Random
        Dim a As Integer = 0
        While a < 2
            row_i = random.Next(4)
            col_j = random.Next(4)
            If B(row_i, col_j).Text = "" Then
                If random.Next(6) >= 1 Then                                                                                  '初始两个块，1/6的概率为4,    5/6的概率为2
                    B(row_i, col_j).Text = "2"
                Else
                    B(row_i, col_j).Text = "4"
                End If
                a += 1
            End If
        End While
        Bt_Color()
        Score = 0
        Label1.Text = "0"
        IsNewbt()
        IsWin = False
        IsStart = False
    End Sub
#End Region

#Region "按鈕上色"
    ''' <summary>
    ''' 按鈕上色
    ''' 上色、分數賦值
    ''' 判斷是否遊戲結束
    ''' </summary>
    Private Sub Bt_Color()                                                                                          '上色
        For x As Integer = 0 To 3                                                                                   '不同text值的按鈕上不同的顏色
            For y As Integer = 0 To 3
                B(x, y).BackColor = Color.FromArgb(204, 192, 180)
                Select Case B(x, y).Text
                    Case "2"
                        B(x, y).BackColor = Color.FromArgb(238, 228, 218)
                    Case "4"
                        B(x, y).BackColor = Color.FromArgb(236, 224, 200)
                    Case "8"
                        B(x, y).BackColor = Color.FromArgb(242, 177, 121)
                    Case "16"
                        B(x, y).BackColor = Color.FromArgb(236, 141, 83)
                    Case "32"
                        B(x, y).BackColor = Color.FromArgb(245, 124, 95)
                    Case "64"
                        B(x, y).BackColor = Color.FromArgb(233, 89, 55)
                    Case "128"
                        B(x, y).BackColor = Color.FromArgb(243, 217, 107)
                    Case "256"
                        B(x, y).BackColor = Color.FromArgb(241, 208, 75)
                    Case "512"
                        B(x, y).BackColor = Color.FromArgb(228, 192, 42)
                    Case "1024"
                        B(x, y).BackColor = Color.FromArgb(227, 186, 20)
                    Case "2048"
                        B(x, y).BackColor = Color.FromArgb(227, 186, 20)
                    Case "4096"
                        B(x, y).BackColor = Color.FromArgb(236, 196, 0)
                    Case "8192"
                        B(x, y).BackColor = Color.FromArgb(60, 58, 50)
                End Select
            Next
        Next                                                                                                                      '按钮上色
        Label1.Text = Score
        'If Score > GetINI("Game", "HighScore", "", Path) Then
        '    WriteINI("Game", "HighScore", Score, Path)
        '    Label2.Text = Score
        'End If
        IsGameOver()
    End Sub
#End Region

#Region "判斷遊戲結束"
    ''' <summary>
    ''' 判斷遊戲結束
    ''' 結束標誌 a = True
    ''' 如到2048，提示，並且IsWin = True
    ''' 如有空的塊則退出函數
    ''' 如有可合併（相鄰並相同）的塊則 a = false
    ''' a = True 則遊戲結束，顯示分數
    ''' </summary>
    Private Sub IsGameOver()
        Dim a As Boolean = True
        For x As Integer = 0 To 3
            For y As Integer = 0 To 3
                If B(x, y).Text = "2048" Then
                    If IsWin = False Then
                        MsgBox("恭喜你已玩到2048！", 64)
                        If MsgBox("是否继续游戏？", 32 + 4) = vbNo Then
                            MsgBox("游戏结束" & vbCrLf & "分数：" & Score, 64)
                            'If Score > GetINI("Game", "HighScore", "", Path) Then
                            '    WriteINI("Game", "HighScore", Score, Path)
                            'End If
                        End If
                        IsWin = True
                    End If
                End If
            Next
        Next
        For x As Integer = 0 To 3
            For y As Integer = 0 To 3
                If B(x, y).Text = "" Then
                    Exit Sub
                End If
            Next
        Next
        For x As Integer = 0 To 2
            For y As Integer = 0 To 2
                If B(x, y).Text = B(x, y + 1).Text Or B(x, y).Text = B(x + 1, y).Text Then
                    a = False
                End If
            Next
        Next
        For x As Integer = 0 To 2
            If B(x, 3).Text = B(x + 1, 3).Text Then
                a = False
            End If
        Next
        For y As Integer = 0 To 2
            If B(3, y).Text = B(3, y + 1).Text Then
                a = False
            End If
        Next
        If a = True Then
            MsgBox("游戏结束" & vbCrLf & "分数：" & Score, 64)
            'If Score > GetINI("game", "highscore", "", Path) Then
            '    WriteINI("game", "highscore", Score, Path)
            'End If
            GameInit()
        End If
    End Sub
#End Region

#Region "生成新塊"
    ''' <summary>
    ''' 生成新塊
    ''' 當前記錄（移动之后）和舊記錄比較（判斷）是否生成新塊，不同（有移动）則生成新塊併上色
    ''' </summary>
    Private Sub Newbt()
        Dim afterMoveStr As String
        afterMoveStr = ""
        For row_x As Integer = 0 To 3
            For col_y As Integer = 0 To 3
                afterMoveStr = afterMoveStr & row_x & col_y & B(row_x, col_y).Text
            Next
        Next
        If afterMoveStr <> beforeMoveStr Then
            Dim row_i As Integer, col_j As Integer
            Dim radom As New Random
            Dim a As Integer = 0
            While a < 1
                row_i = radom.Next(4)
                col_j = radom.Next(4)
                If B(row_i, col_j).Text = "" Then
                    If B(row_i, col_j).Text = "" Then
                        If radom.Next(6) >= 1 Then
                            B(row_i, col_j).Text = "2"
                            B(row_i, col_j).BackColor = Color.FromArgb(238, 228, 218)
                        Else
                            B(row_i, col_j).Text = "4"
                            B(row_i, col_j).BackColor = Color.FromArgb(236, 224, 200)
                        End If
                        a += 1
                    End If
                End If
            End While
#Region "other way to find empty block(need endless loop)"
            'Dim aa1 As Boolean = False
            'For x As Integer = 0 To 3
            '    For y As Integer = 0 To 3
            '        '需死循環
            '        If B(i, j).Text = "" And s.Next(4) >= 3 Then
            '            '新加塊
            '            If s.Next(6) >= 1 Then
            '                B(i, j).Text = "2"
            '                B(i, j).BackColor = Color.FromArgb(238, 228, 218)
            '                aa1 = True
            '                Exit For
            '            Else
            '                B(i, j).Text = "4"
            '                B(i, j).BackColor = Color.FromArgb(236, 224, 200)
            '                aa1 = True
            '                Exit For
            '            End If
            '        End If
            '    Next
            '    If aa1 = True Then
            '        Exit For
            '    End If
            'Next
#End Region
        End If
    End Sub
#End Region

#Region "保存舊記錄用於判斷（比較）是否需要新生成數給塊"
    ''' <summary>
    ''' 保存舊記錄用於判斷（比較）是否需要新生成數給塊
    ''' 打印字符串，坐標加內容，beforeMoveStr		
    ''' beforeMoveStr	"0001020310111213202122223303132433"	String
    ''' 保存当前的块状态位置及内容，用于判断挪完之后与之前是否相同，以便是否进行下一步的加随机新块。
    ''' </summary>
    Private Sub IsNewbt()
        beforeMoveStr = ""
        For row_x As Integer = 0 To 3
            For col_y As Integer = 0 To 3
                beforeMoveStr = beforeMoveStr & row_x & col_y & B(row_x, col_y).Text
            Next
        Next
    End Sub
#End Region

#Region "上移"
    ''' <summary>
    ''' 上/下/左/右移动等
    ''' 0空 123之中只有第一个遇到的上移
    ''' 1空 23之中只有第一个遇到的上移
    ''' 2空 3上移
    ''' 第四象限
    ''' IsStart = True
    ''' </summary>
    Private Sub bt_Up()
        Dim numOfMoves As Integer = 0
        While numOfMoves < 3
            For row_x As Integer = 0 To 3
                If B(row_x, 0).Text = "" Then
                    If B(row_x, 1).Text <> "" Then
                        B(row_x, 0).Text = B(row_x, 1).Text
                        B(row_x, 1).Text = ""
                    Else
                        If B(row_x, 2).Text <> "" Then
                            B(row_x, 0).Text = B(row_x, 2).Text
                            B(row_x, 2).Text = ""
                        Else
                            If B(row_x, 3).Text <> "" Then
                                B(row_x, 0).Text = B(row_x, 3).Text
                                B(row_x, 3).Text = ""
                            End If
                        End If
                    End If
                Else
                    If B(row_x, 1).Text = "" Then
                        If B(row_x, 2).Text <> "" Then
                            B(row_x, 1).Text = B(row_x, 2).Text
                            B(row_x, 2).Text = ""
                        Else
                            If B(row_x, 3).Text <> "" Then
                                B(row_x, 1).Text = B(row_x, 3).Text
                                B(row_x, 3).Text = ""
                            End If
                        End If
                    Else
                        If B(row_x, 2).Text = "" Then
                            If B(row_x, 3).Text <> "" Then
                                B(row_x, 2).Text = B(row_x, 3).Text
                                B(row_x, 3).Text = ""
                            End If
                        End If
                    End If
                End If
            Next
            numOfMoves += 1
        End While
        IsStart = True
    End Sub
#End Region

#Region "下移"
    Private Sub bt_Down()
        Dim numOfMoves As Integer = 0
        While numOfMoves < 3
            For row_x As Integer = 0 To 3
                If B(row_x, 3).Text = "" Then
                    If B(row_x, 2).Text <> "" Then
                        B(row_x, 3).Text = B(row_x, 2).Text
                        B(row_x, 2).Text = ""
                    Else
                        If B(row_x, 1).Text <> "" Then
                            B(row_x, 3).Text = B(row_x, 1).Text
                            B(row_x, 1).Text = ""
                        Else
                            If B(row_x, 0).Text <> "" Then
                                B(row_x, 3).Text = B(row_x, 0).Text
                                B(row_x, 0).Text = ""
                            End If
                        End If
                    End If
                Else
                    If B(row_x, 2).Text = "" Then
                        If B(row_x, 1).Text <> "" Then
                            B(row_x, 2).Text = B(row_x, 1).Text
                            B(row_x, 1).Text = ""
                        Else
                            If B(row_x, 0).Text <> "" Then
                                B(row_x, 2).Text = B(row_x, 0).Text
                                B(row_x, 0).Text = ""
                            End If
                        End If
                    Else
                        If B(row_x, 1).Text = "" Then
                            If B(row_x, 0).Text <> "" Then
                                B(row_x, 1).Text = B(row_x, 0).Text
                                B(row_x, 0).Text = ""
                            End If
                        End If
                    End If
                End If
            Next
            numOfMoves += 1
        End While
        IsStart = True
    End Sub
#End Region

#Region "左移"
    Private Sub bt_Left()
        Dim numOfMoves As Integer = 0
        While numOfMoves < 3
            For col_y As Integer = 0 To 3
                If B(0, col_y).Text = "" Then
                    If B(1, col_y).Text <> "" Then
                        B(0, col_y).Text = B(1, col_y).Text
                        B(1, col_y).Text = ""
                    Else
                        If B(2, col_y).Text <> "" Then
                            B(0, col_y).Text = B(2, col_y).Text
                            B(2, col_y).Text = ""
                        Else
                            If B(3, col_y).Text <> "" Then
                                B(0, col_y).Text = B(3, col_y).Text
                                B(3, col_y).Text = ""
                            End If
                        End If
                    End If
                Else
                    If B(1, col_y).Text = "" Then
                        If B(2, col_y).Text <> "" Then
                            B(1, col_y).Text = B(2, col_y).Text
                            B(2, col_y).Text = ""
                        Else
                            If B(3, col_y).Text <> "" Then
                                B(1, col_y).Text = B(3, col_y).Text
                                B(3, col_y).Text = ""
                            End If
                        End If
                    Else
                        If B(2, col_y).Text = "" Then
                            If B(3, col_y).Text <> "" Then
                                B(2, col_y).Text = B(3, col_y).Text
                                B(3, col_y).Text = ""
                            End If
                        End If
                    End If
                End If
            Next
            numOfMoves += 1
        End While
        IsStart = True
    End Sub
#End Region

#Region "右移"
    Private Sub bt_Right()
        Dim numOfMoves As Integer = 0
        While numOfMoves < 3
            For col_y As Integer = 0 To 3
                If B(3, col_y).Text = "" Then
                    If B(2, col_y).Text <> "" Then
                        B(3, col_y).Text = B(2, col_y).Text
                        B(2, col_y).Text = ""
                    Else
                        If B(1, col_y).Text <> "" Then
                            B(3, col_y).Text = B(1, col_y).Text
                            B(1, col_y).Text = ""
                        Else
                            If B(0, col_y).Text <> "" Then
                                B(3, col_y).Text = B(0, col_y).Text
                                B(0, col_y).Text = ""
                            End If
                        End If
                    End If
                Else
                    If B(2, col_y).Text = "" Then
                        If B(1, col_y).Text <> "" Then
                            B(2, col_y).Text = B(1, col_y).Text
                            B(1, col_y).Text = ""
                        Else
                            If B(0, col_y).Text <> "" Then
                                B(2, col_y).Text = B(0, col_y).Text
                                B(0, col_y).Text = ""
                            End If
                        End If
                    Else
                        If B(1, col_y).Text = "" Then
                            If B(0, col_y).Text <> "" Then
                                B(1, col_y).Text = B(0, col_y).Text
                                B(0, col_y).Text = ""
                            End If
                        End If
                    End If
                End If
            Next
            numOfMoves += 1
        End While
        IsStart = True
    End Sub
#End Region

#Region "上加"
    ''' <summary>
    ''' 每一个列中，向上加
    ''' 01非空，等则加
    ''' 12非空，等则加
    ''' 23非空，等则加
    ''' </summary>
    Private Sub UpPlus()
        For row_x As Integer = 0 To 3
            If B(row_x, 0).Text <> "" And B(row_x, 1).Text <> "" Then
                If B(row_x, 0).Text = B(row_x, 1).Text Then
                    B(row_x, 0).Text = 2 * B(row_x, 0).Text
                    B(row_x, 1).Text = ""
                    Score += B(row_x, 0).Text
                End If
            End If
            If B(row_x, 1).Text <> "" And B(row_x, 2).Text <> "" Then
                If B(row_x, 1).Text = B(row_x, 2).Text Then
                    B(row_x, 1).Text = 2 * B(row_x, 1).Text
                    B(row_x, 2).Text = ""
                    Score += B(row_x, 1).Text
                End If
            End If
            If B(row_x, 2).Text <> "" And B(row_x, 3).Text <> "" Then
                If B(row_x, 2).Text = B(row_x, 3).Text Then
                    B(row_x, 2).Text = 2 * B(row_x, 2).Text
                    B(row_x, 3).Text = ""
                    Score += B(row_x, 2).Text
                End If
            End If
        Next
    End Sub
#End Region

#Region "下加"
    Private Sub DownPlus()
        For row_x As Integer = 0 To 3
            If B(row_x, 3).Text <> "" And B(row_x, 2).Text <> "" Then
                If B(row_x, 3).Text = B(row_x, 2).Text Then
                    B(row_x, 3).Text = 2 * B(row_x, 3).Text
                    B(row_x, 2).Text = ""
                    Score += B(row_x, 3).Text
                End If
            End If
            If B(row_x, 2).Text <> "" And B(row_x, 1).Text <> "" Then
                If B(row_x, 2).Text = B(row_x, 1).Text Then
                    B(row_x, 2).Text = 2 * B(row_x, 2).Text
                    B(row_x, 1).Text = ""
                    Score += B(row_x, 2).Text
                End If
            End If
            If B(row_x, 1).Text <> "" And B(row_x, 0).Text <> "" Then
                If B(row_x, 1).Text = B(row_x, 0).Text Then
                    B(row_x, 1).Text = 2 * B(row_x, 1).Text
                    B(row_x, 0).Text = ""
                    Score += B(row_x, 1).Text
                End If
            End If
        Next
    End Sub
#End Region

#Region "左加"
    Private Sub LeftPlus()
        For col_y As Integer = 0 To 3
            If B(0, col_y).Text <> "" And B(1, col_y).Text <> "" Then
                If B(0, col_y).Text = B(1, col_y).Text Then
                    B(0, col_y).Text = 2 * B(0, col_y).Text
                    B(1, col_y).Text = ""
                    Score += B(0, col_y).Text
                End If
            End If
            If B(1, col_y).Text <> "" And B(2, col_y).Text <> "" Then
                If B(1, col_y).Text = B(2, col_y).Text Then
                    B(1, col_y).Text = 2 * B(1, col_y).Text
                    B(2, col_y).Text = ""
                    Score += B(1, col_y).Text
                End If
            End If
            If B(2, col_y).Text <> "" And B(3, col_y).Text <> "" Then
                If B(2, col_y).Text = B(3, col_y).Text Then
                    B(2, col_y).Text = 2 * B(2, col_y).Text
                    B(3, col_y).Text = ""
                    Score += B(2, col_y).Text
                End If
            End If
        Next
    End Sub
#End Region

#Region "右加"
    Private Sub RightPlus()
        For col_y As Integer = 0 To 3
            If B(3, col_y).Text <> "" And B(2, col_y).Text <> "" Then
                If B(3, col_y).Text = B(2, col_y).Text Then
                    B(3, col_y).Text = 2 * B(3, col_y).Text
                    B(2, col_y).Text = ""
                    Score += B(3, col_y).Text
                End If
            End If
            If B(2, col_y).Text <> "" And B(1, col_y).Text <> "" Then
                If B(2, col_y).Text = B(1, col_y).Text Then
                    B(2, col_y).Text = 2 * B(2, col_y).Text
                    B(1, col_y).Text = ""
                    Score += B(2, col_y).Text
                End If
            End If
            If B(1, col_y).Text <> "" And B(0, col_y).Text <> "" Then
                If B(1, col_y).Text = B(0, col_y).Text Then
                    B(1, col_y).Text = 2 * B(1, col_y).Text
                    B(0, col_y).Text = ""
                    Score += B(1, col_y).Text
                End If
            End If
        Next
    End Sub
#End Region

#Region "重新開始"
    ''' <summary>
    ''' 重新開始
    ''' 加載初始化函數
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btRestart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btRestart.Click
        'Beep()
        If MsgBox("确认重新开始？", 32 + 4) = vbYes Then
            GameInit()
        End If
        'WriteINI("Game", "IsStart", IsStart, Path)
        If IsStart = True Then
            Dim a As String = ""
            For row_x = 0 To 3
                For col_y = 0 To 3
                    a = a & B(row_x, col_y).Text & "|"
                Next
            Next
            'WriteINI("Game", "Game", a, Path)
            'WriteINI("Game", "IsWin", IsWin, Path)
            'WriteINI("Game", "Score", Score, Path)
        End If
    End Sub
#End Region

#Region "点击窗体自动移动"
    'Private Sub Form1_Click(sender As Object, e As EventArgs) Handles Me.Click
    '    Dim a2 As Integer = 0
    '    For a2 = 0 To 5
    '        bt_Down()
    '        DownPlus()
    '        bt_Down()
    '        Newbt()
    '        Bt_Color()

    '        bt_Up()
    '        UpPlus()
    '        bt_Up()
    '        Newbt()
    '        Bt_Color()

    '        bt_Left()
    '        LeftPlus()
    '        bt_Left()
    '        Newbt()
    '        Bt_Color()

    '        bt_Right()
    '        RightPlus()
    '        bt_Right()
    '        Newbt()
    '        Bt_Color()
    '    Next
    'End Sub
#End Region
End Class
