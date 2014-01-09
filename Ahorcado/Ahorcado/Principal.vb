Public Class Principal

#Region "Declaraciones"

    ''' <summary>
    ''' Enumerador que permite controlar el estado actual del juego
    ''' </summary>
    ''' <remarks></remarks>
    Enum Estado
        GANO = 1
        PIERDO = 2
        CONTINUAR = 0
    End Enum

    ' Varible para controlar el estado actual del juego
    Dim estadoJuego As Estado

    ' Variable para alamacenar la palabra a adivinar
    Dim strPalabra As String

    ' Variable para almacenar lo que lleva el jugador adivinado de la 
    ' palabra a adivinar
    Dim strPalabraAdivinada As String = String.Empty

    ' Variable para almacenar la cantidad de errores que tiene
    ' el usuario
    Dim intNumErrores As Integer = 0

#End Region

 #Region "Funciones y métodos de apoyo"

    ''' <summary>
    ''' Método para buscar una palabra por internet e introducirla en el juego del ahorcado
    ''' </summary>
    ''' <remarks>El tratamiento de la web se realiza en Navegador_DocumentCompleted</remarks>
    Private Sub BuscarPalabra()

        ' Cargamos en el navegador la página web que genera palabras aleatorias
        Navegador.Navigate("http://www.palabrasque.com/palabra-aleatoria.php")

        ' Definimos que no queremos que salten los posibles errores en la web
        Navegador.ScriptErrorsSuppressed = True

    End Sub

    ''' <summary>
    ''' Función para validar la letra introducida como que es una letra y no un caracter raro o un número
    ''' </summary>
    ''' <param name="strCadena">Letra a validar</param>
    ''' <returns>Verdadero si es una letra y falso si no lo es</returns>
    ''' <remarks></remarks>
    Private Function ValidacionLetra(ByVal strCadena As String)
        ' Variable que devolverá el resultado de la función
        Dim resultado As Boolean = True

        ' Si se ha introducido más de un caracter, la letra no es válida
        If strCadena.Length > 1 Then
            resultado = False
        End If

        ' Si el caracter es un número, no es válido
        If IsNumeric(strCadena) Then
            resultado = False
        End If

        ' Si el caracter no es parte del abecedario, no es válido
        If "abcdefghijklmnñopqrstuvwxyz".IndexOf(strCadena) = -1 Then
            resultado = False
        End If

        ' Devolvemos el resultado de la validación
        Return resultado
    End Function

#End Region

#Region "Eventos de los objetos del formulario"

    ''' <summary>
    ''' Evento del formulario principal
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Principal_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        ' Inhabilitamos el boton y el textbox donde se introducen las letras
        btnAñadir.Enabled = False
        txtIntroduceLetra.Enabled = False

        ' Buscamos una palabra por internet para adivinar
        BuscarPalabra()


    End Sub

    ''' <summary>
    ''' Evento click del boton
    ''' </summary>
    ''' <param name="sender">Objeto que envía el evento</param>
    ''' <param name="e">Parámetros del evento</param>
    ''' <remarks></remarks>
    Private Sub btnAñadir_Click(sender As Object, e As EventArgs) Handles btnAñadir.Click
        ' Variable para almacenar la letra  introducida por el jugador
        Dim strLetra As String

        ' Variable de apoyo para la iteración del array
        Dim intApoyo As Integer = 0

        ' Verificamos que se ha introducido algo en el textbox
        If txtIntroduceLetra.TextLength <> 0 Then
            ' Pasamos el valor introducido a minúsculas
            strLetra = txtIntroduceLetra.Text.ToLower

            ' Verificamos si la letra introducida ya ha sido jugada anteriormente
            If lstListaLetras.Items.Contains(strLetra.ToUpper) = False Then

                ' Validamos si el caracter es una letra
                If ValidacionLetra(strLetra) Then

                    ' Si es una letra y no se ha jugado antes la añadimos a la lista de
                    ' letras jugadas
                    lstListaLetras.Items.Add(strLetra.ToUpper)

                    ' Verificamos si la letra jugada está en la palabra a adivinar
                    If strPalabra.IndexOf(strLetra) <> -1 Then

                        ' Definimos un array de apoyo para tratar la cadena
                        Dim apoyo() As Char

                        ' Pasamos lo que lleva el jugador adivinado de la palabra
                        ' y lo convertimos en un array
                        apoyo = strPalabraAdivinada.ToCharArray()

                        ' Limpiamos el textbox que muestra lo que lleva el usuario adivinado
                        ' de la palabra y limpiamos la variable que la almacena
                        lblPalabraDescubierta.Text = String.Empty
                        strPalabraAdivinada = String.Empty

                        ' Iteramos por cada caracter de la pabra a adivinar
                        For Each letras As String In strPalabra
                            ' Si el caracter de la iteración es igual a la letra introducida
                            If letras = strLetra Then
                                ' En esa posición del array asignamos el valor de la letra introducida
                                apoyo(intApoyo) = strLetra
                            End If

                            ' Vamos generando la variable que controla lo que el jugador va adivinando
                            ' así como el label que muestra los progresos
                            strPalabraAdivinada = strPalabraAdivinada + apoyo(intApoyo)
                            lblPalabraDescubierta.Text = lblPalabraDescubierta.Text + apoyo(intApoyo) + " "

                            ' Aumentamos el contador
                            intApoyo = intApoyo + 1
                        Next

                        ' Si se da el caso que la palabra adivinada es igual a la palabra a adivinar
                        ' se ha ganado el juego
                        If strPalabra = strPalabraAdivinada Then
                            ' Cambiamos el estado del juego a Gano
                            estadoJuego = Estado.GANO
                        End If
                    Else
                        ' Si la letra introducida no está en la palabra a adivinar
                        ' aumentamos el número de errores
                        intNumErrores = intNumErrores + 1

                        ' Dependiendo del número de errores, mostramos una imágen u otra
                        ' almacenada en los recursos
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
                                ' Si se ha cargado la sexta imagen, el juego se ha perdido
                                estadoJuego = Estado.PIERDO
                        End Select

                    End If
                Else
                    ' Mostramos un mensaje avisando que el carácter introducido no es valido
                    MsgBox("No es un caracter válido", MsgBoxStyle.Exclamation, "Atención")
                End If
            Else
                ' Mostramos un mensaje avisando que el caracter introducido ya se ha usado anteriomente
                MsgBox("Esa letra ya ha sido introducida", MsgBoxStyle.Exclamation, "Atención")
            End If
        End If

        ' Cambiamos el label que muestra el estado del juego
        lblEstado.Text = estadoJuego.ToString

        'Limpiamos el textbox
        txtIntroduceLetra.Text = String.Empty

        ' Si el estado del juego es GANO, el jugador ha ganado
        If estadoJuego = Estado.GANO Then
            ' Mostramos un mensaje de información
            MsgBox("Ganaste", MsgBoxStyle.Information, "You Win")

            ' Limpiamos y buscamos una nueva palabra para jugar
            lstListaLetras.Items.Clear()
            picImagen.Image = My.Resources._00
            BuscarPalabra()
        End If

        If estadoJuego = Estado.PIERDO Then
            ' Mostramos un mensaje de información
            MsgBox("Perdiste", MsgBoxStyle.Critical, "You Lose")

            ' Limpiamos y buscamos una nueva palabra para jugar
            lstListaLetras.Items.Clear()
            picImagen.Image = My.Resources._00
            BuscarPalabra()
        End If
    End Sub

    ''' <summary>
    ''' Evento del webbrowser para cuando ha terminado de cargar una página web
    ''' </summary>
    ''' <param name="sender">Objeto que envía el evento</param>
    ''' <param name="e">Argumentos del evento</param>
    ''' <remarks></remarks>
    Private Sub Navegador_DocumentCompleted(sender As Object, e As WebBrowserDocumentCompletedEventArgs) Handles Navegador.DocumentCompleted

        ' Una vez cargada la página, volcamos la estructura de la misma en una cadena
        Dim cadena As String = Navegador.DocumentText.ToString()

        ' Definimos los puntos entre los que se encuentra la palabra aleatoria en la página
        Dim fijacion1 As String = "<br><font size=" + Chr(34) + "6" + Chr(34) + " /><strong>"
        Dim fijacion2 As String = "</strong></font>  </font></p>"

        ' Definimos la posición de la fijación 1
        Dim valor1 As Integer = cadena.IndexOf(fijacion1)
        ' Definimos la posición de la fijación 2 que irá despues de la fijación 1
        Dim valor2 As Integer = cadena.IndexOf(fijacion2, valor1)

        ' La palabra será la cadena entre la posición de la fijacación 1 + su tamaño y 
        ' la posición de la fijación 2 menos su tamaño y menos la posición de la fijación1
        ' Asignamos la palabra parseada a la variable global que guarda la palabra a adivinar
        strPalabra = cadena.Substring(valor1 + fijacion1.Length, valor2 - fijacion2.Length - valor1).ToLower

        ' Cambiamos los carácteres con acentos, por los mismos sin acentos para facilitar el juego
        strPalabra.Replace("á", "a").Replace("é", "e").Replace("í", "i").Replace("ó", "o").Replace("ú", "u")

        ' Limpiamos controles y variables
        lblPalabraDescubierta.Text = String.Empty
        strPalabraAdivinada = String.Empty
        lstListaLetras.Items.Clear()

        ' Creamos la variables que almacenará lo que lleva el jugador adivinado de la palabra
        ' iterando los caracteres de la palabra a adivinar e añadiendo el caracter _ por cada posición
        For Each letra As String In strPalabra
            strPalabraAdivinada = strPalabraAdivinada + "_"
        Next

        ' En el caso del label que muestra la palabra que lleva adivinada el usuario hacemos lo mismo
        ' con un bucle for, pero añadimos _ con un espacio
        For i As Integer = 0 To strPalabraAdivinada.Length - 1
            lblPalabraDescubierta.Text = lblPalabraDescubierta.Text + "_ "
        Next

        ' Cambiamos el estado del juego a continuar
        estadoJuego = Estado.CONTINUAR

        ' Cambiamos la etiqueta de estado
        lblEstado.Text = estadoJuego.ToString

        ' Habiliamos el boton y el textbox que permite jugar al usuario
        btnAñadir.Enabled = True
        txtIntroduceLetra.Enabled = True

    End Sub

#End Region

End Class
