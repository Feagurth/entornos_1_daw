Module Module1

    Sub Main(ByVal sArgs() As String)

        If sArgs.Length = 0 Then
            Console.WriteLine("Sin valores")
        Else
            Dim i As Integer = 0
            Dim control As Boolean = True
            Dim salida As String = String.Empty

            While i < sArgs.Length

                control = True

                For j As Integer = 2 To (sArgs(i) - 1) Step 1
                    If sArgs(i) Mod j = 0 Then
                        control = False
                    End If
                Next

                If control = True And sArgs(i) <> 0 Then
                    salida = salida + sArgs(i)
                End If

                i = i + 1
            End While

            Console.WriteLine("Resultado: " + salida)

        End If
    End Sub

End Module
