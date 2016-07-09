Public Class clsFileJunk

    Public Sub DirSearch(sPath As String)

        For Each d As String In Directory.GetDirectories(sPath)
            Console.WriteLine(d)
            For Each f As String In Directory.GetFiles(d)
                Console.WriteLine(f)
            Next
            DirSearch(d)
        Next

    End Sub

    Public Sub DirInfoSearch(sPath As String, sMask As String)
        Dim di = New DirectoryInfo(sPath)

        Dim rgFiles = di.GetFiles(sMask, IO.SearchOption.AllDirectories)
    End Sub

    Public Sub DirSearch2(sPath As String, sMask As String)
        Try
            Dim files = From chkFile In Directory.EnumerateFiles(sPath, sMask, SearchOption.AllDirectories)
                        From line In File.ReadLines(chkFile)
                        Where line.Contains("Microsoft")
                        Select New With {.curFile = chkFile, .curLine = line}

            For Each f In files
                Console.WriteLine("{0}\t{1}", f.curFile, f.curLine)
            Next
            Console.WriteLine("{0} files found.", files.Count.ToString())
        Catch UAEx As UnauthorizedAccessException
            Console.WriteLine(UAEx.Message)
        Catch PathEx As PathTooLongException
            Console.WriteLine(PathEx.Message)
        End Try

    End Sub

    Public Shared Sub DirSearch3(ByVal args As String())
        Dim diTop As New DirectoryInfo("d:\")
        Try
            For Each fi In diTop.EnumerateFiles()
                Try
                    ' Display each file over 10 MB;
                    If fi.Length > 10000000 Then
                        Console.WriteLine("{0}" & vbTab & vbTab & "{1}", fi.FullName, fi.Length.ToString("N0"))
                    End If
                Catch UnAuthTop As UnauthorizedAccessException
                    Console.WriteLine("{0}", UnAuthTop.Message)
                End Try
            Next

            For Each di In diTop.EnumerateDirectories("*")
                Try
                    For Each fi In di.EnumerateFiles("*", SearchOption.AllDirectories)
                        Try
                            ' // Display each file over 10 MB;
                            If fi.Length > 10000000 Then
                                Console.WriteLine("{0}" & vbTab &
                                vbTab & "{1}", fi.FullName, fi.Length.ToString("N0"))
                            End If
                        Catch UnAuthFile As UnauthorizedAccessException
                            Console.WriteLine("UnAuthFile: {0}", UnAuthFile.Message)
                        End Try
                    Next
                Catch UnAuthSubDir As UnauthorizedAccessException
                    Console.WriteLine("UnAuthSubDir: {0}", UnAuthSubDir.Message)
                End Try
            Next
        Catch DirNotFound As DirectoryNotFoundException
            Console.WriteLine("{0}", DirNotFound.Message)
        Catch UnAuthDir As UnauthorizedAccessException
            Console.WriteLine("UnAuthDir: {0}", UnAuthDir.Message)
        Catch LongPath As PathTooLongException
            Console.WriteLine("{0}", LongPath.Message)
        End Try
    End Sub

End Class
