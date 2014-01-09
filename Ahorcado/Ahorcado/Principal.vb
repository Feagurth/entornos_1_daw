Public Class Principal

    Enum Estado
        GANO = 1
        PIERDO = 2
        CONTINUAR = 0
    End Enum

    Dim estadoJuego As Estado
    Dim strPalabra As String
    Dim strPalabraAdivinada As String = String.Empty
    Dim intNumErrores As Integer = 0

    Private Sub Principal_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Navegador.Navigate("http://www.palabrasque.com/palabra-aleatoria.php")

        While Navegador.IsBusy
            Debug.Print("Cargando...")
        End While


        strPalabra = BuscarPalabra()
        lblPalabraDescubierta.Text = String.Empty

        For Each letra As String In strPalabra
            strPalabraAdivinada = strPalabraAdivinada + "_"
        Next

        For i As Integer = 0 To strPalabraAdivinada.Length
            lblPalabraDescubierta.Text = lblPalabraDescubierta.Text + "_ "

        Next




        estadoJuego = Estado.CONTINUAR
        lblEstado.Text = estadoJuego.ToString

    End Sub

    Private Function BuscarPalabra()
        Dim resultado As String = "alcachofas"


        Return resultado

    End Function



    Private Function ValidacionLetra(ByVal strCadena As String)
        Dim resultado As Boolean = True

        If strCadena.Length > 1 Then
            resultado = False
        End If

        If IsNumeric(strCadena) Then
            resultado = False
        End If

        If "abcdefghijklmnñopqrstuvwxyz".IndexOf(strCadena) = -1 Then
            resultado = False
        End If

        Return resultado
    End Function


    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles btnAñadir.Click
        Dim strLetra As String
        Dim intApoyo As Integer = 0

        If txtIntroduceLetra.TextLength <> 0 Then
            strLetra = txtIntroduceLetra.Text.ToLower

            If lstListaLetras.Items.Contains(strLetra.ToUpper) = False Then

                If ValidacionLetra(strLetra) Then

                    lstListaLetras.Items.Add(strLetra.ToUpper)

                    If strPalabra.IndexOf(strLetra) <> -1 Then

                        Dim apoyo() As Char

                        apoyo = strPalabraAdivinada.ToCharArray()

                        lblPalabraDescubierta.Text = String.Empty
                        strPalabraAdivinada = String.Empty

                        For Each letras As String In strPalabra
                            If letras = strLetra Then
                                apoyo(intApoyo) = strLetra
                            End If

                            strPalabraAdivinada = strPalabraAdivinada + apoyo(intApoyo)
                            lblPalabraDescubierta.Text = lblPalabraDescubierta.Text + apoyo(intApoyo) + " "

                            intApoyo = intApoyo + 1
                        Next

                        If strPalabra = strPalabraAdivinada Then
                            estadoJuego = Estado.GANO
                        End If
                    Else
                        intNumErrores = intNumErrores + 1

                        Select Case intNumErrores
                            Case 1
                                picImagen.Image = My.Resources._01
                            Case 2
                                picImagen.Image = My.Resources._02
                            Case 3
                                picImagen.Image = My.Resources._03
                            Case 4
                                picImagen.Image = My.Resources._04
                            Case 5
                                picImagen.Image = My.Resources._05
                            Case 6
                                picImagen.Image = My.Resources._06
                                estadoJuego = Estado.PIERDO
                        End Select

                    End If
                Else
                    MsgBox("No es un caracter válido", MsgBoxStyle.Exclamation, "Atención")
                End If
            Else
                MsgBox("Esa letra ya ha sido introducida", MsgBoxStyle.Exclamation, "Atención")
            End If
        End If

        txtIntroduceLetra.Text = String.Empty
        txtIntroduceLetra.Focus()

        lblEstado.Text = estadoJuego.ToString

        If estadoJuego = Estado.GANO Then
            MsgBox("Ganaste", MsgBoxStyle.Information, "You Win")
            lstListaLetras.Items.Clear()
            picImagen.Image = My.Resources._00
        End If
        If estadoJuego = Estado.PIERDO Then
            MsgBox("Perdiste", MsgBoxStyle.Critical, "You Lose")
            lstListaLetras.Items.Clear()
            picImagen.Image = My.Resources._00
        End If


    End Sub

    Private Sub txtIntroduceLetra_KeyUp(sender As Object, e As KeyEventArgs) Handles txtIntroduceLetra.KeyUp
        btnAñadir.Focus()
    End Sub
End Class
