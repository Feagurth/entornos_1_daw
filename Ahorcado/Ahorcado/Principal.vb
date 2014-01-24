Imports System.Net

''' <summary>
''' Clase principal del juego del ahorcado
''' By.- Luis Cabrerizo Gomez
''' LuisCabrerizoGomez@gmail.com
''' </summary>
''' <remarks></remarks>
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

    ' Variables para llevar la puntuación del juego
    Dim partidasJugadas As Integer = 0
    Dim partidasGanadas As Integer = 0

    ' Variable para almacenar lo que lleva el jugador adivinado de la 
    ' palabra a adivinar
    Dim strPalabraAdivinada As String = String.Empty

    ' Variable para almacenar la cantidad de errores que tiene
    ' el usuario
    Dim intNumErrores As Integer = 0

#End Region

#Region "Funciones y métodos de apoyo"

    ''' <summary>
    ''' Función para verificar que la palabra introducida no tiene caracteres no válidos
    ''' </summary>
    ''' <param name="strPalabra">Palabra a validar</param>
    ''' <returns>Verdadero si todos sus caracteres son válidos y Falso si no lo son</returns>
    ''' <remarks></remarks>
    Private Function validacionPalabrasRaras(ByVal strPalabra)

        ' Iteramos por cada caracter de la palabra
        For Each caracter As Char In strPalabra
            ' Comprobamos si el caracter no se encuentra en la lista de 
            ' caracteres permitidos
            If ("abcdefghijklmnñopqrstuvwxyz".IndexOf(caracter) = -1) Then
                ' Si no aparece, la palabra tiene caracteres inválidos y se
                ' devuelve False para descartarla
                Return False
            End If
        Next

        ' Se devuelve True, puesto que la palabra está validada
        Return True

    End Function


    ''' <summary>
    ''' Método para buscar una palabra por internet e introducirla en el juego del ahorcado
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub BuscarPalabra()

        ' Comprobamos si hay conexión a Internet para usar el servicio de validación o la generación de palabras aleatorias
        If (VerificarConexionInternet()) Then

            ' Hay conexión. Pedimos una palabra para adivinar al usuario
            strPalabra = InputBox("Introduzca una palabra para adivinar. Pulse cancelar o no introduzca nada para generarla aleatoriamente", "Petición de palabra", String.Empty)

            ' Si el usuario no introduce palabra
            If (strPalabra = String.Empty) Then
                ' Cargamos en el navegador la página web que genera palabras aleatorias
                Dim cadena = getWebPage("http://www.palabrasque.com/palabra-aleatoria.php", 5000)

                If (cadena <> String.Empty) Then
                    parserPalabraAleatoria(cadena)
                Else
                    MsgBox("No se ha podido generar la palabra para empezar el juego." + vbCrLf + "Póngase en contacto con el administrardor", MsgBoxStyle.Critical, "Error!")
                End If
            Else
                If (validacionPalabrasRaras(strPalabra)) Then
                    ValidacionPalabra()
                Else
                    MsgBox("Se han detectado caracteres no válidos" + vbCrLf + "Introduzca una nueva palabra", MsgBoxStyle.Exclamation, "Atención!")
                    BuscarPalabra()
                End If

            End If
        Else
            ' No hay conexión. Mostramos un mensaje al respecto avisando al usuario
            MsgBox("No se ha derectado conexión a internet" + vbCrLf + "No se podrán validar palabras ni generarlas aleatoriamente", MsgBoxStyle.Exclamation, "Atención!")

            ' Pedimos una palabra 
            strPalabra = InputBox("Introduzca una palabra para adivinar. Pulse cancelar o no introduzca nada para finalizar el juego", "Petición de palabra", String.Empty)

            ' Comprobamos si la palabra no es vacia
            If (strPalabra <> String.Empty) Then

                ' Limpiamos la palabra de caracteres indeseados
                strPalabra = LimpiarPalabra(strPalabra)

                ' Verificamos si la palabra contiene caracteres no válidos
                If (validacionPalabrasRaras(strPalabra)) Then
                    ' Si todo está bien iniciamos el juego
                    IniciarJuego()
                Else
                    MsgBox("Se han detectado caracteres no válidos" + vbCrLf + "Buscando una nueva palabra", MsgBoxStyle.Exclamation, "Atención!")
                    ' Si no, buscamos nueva palabra
                    BuscarPalabra()
                End If
            Else
                ' Si lo es, buscamos nueva palabra
                BuscarPalabra()
            End If
        End If
    End Sub

    ''' <summary>
    ''' Función que nos permite recuperar el contenido de una página web
    ''' </summary>
    ''' <param name="url">URL de la página cuyo contenido se quiere obtener</param>
    ''' <param name="timeOutMs">Tiempo de carga máximo de espera en milisegundos</param>
    ''' <returns>El HTML de la página o una cadena vacia</returns>
    ''' <remarks></remarks>
    Private Function getWebPage(ByVal url, ByVal timeOutMs)

        ' Creamos el objeto para realizar la petición web
        Dim req As Net.HttpWebRequest = Net.WebRequest.Create(url)

        ' Establecemos un timeout para que, en el caso de no haber conexión a internet se de un aviso
        req.Timeout = timeOutMs

        ' Intentamos recuperar la página web
        Try
            ' Hacemos la petición y recogemos la respuesta
            Dim response As Net.WebResponse = req.GetResponse

            ' Definimos un flujo de datos y volamos el flujo de datos de la respuesta
            Dim stream As System.IO.Stream = response.GetResponseStream

            ' Creamos un buffer de bytes para leer la resupesta
            Dim buffer As Byte() = New Byte(1000) {}

            ' Creamos una lista de bytes para almacenar la respuesta
            Dim data As New List(Of Byte)

            ' Comenzamos a leer el flujo de datos
            Dim bytesRead = stream.Read(buffer, 0, buffer.Length)

            ' Iteramos hasta que no tengamos nada más que leer
            Do Until bytesRead = 0
                For i = 0 To bytesRead - 1
                    ' A cada iteración vamos añadiendo la información
                    ' A la lista de bytes
                    data.Add(buffer(i))
                Next

                bytesRead = stream.Read(buffer, 0, buffer.Length)
            Loop

            ' Volcamos la información recopilada en una cadena para su 
            ' tratamiento posterior
            Dim cadena = System.Text.Encoding.UTF8.GetString(data.ToArray)

            ' Cerramos objetos
            response.Close()
            stream.Close()

            Return cadena
        Catch ex As Exception
            ' En caso de fallar, se devuelve cadena vacia
            Return String.Empty
        End Try

    End Function

    ''' <summary>
    ''' Función que nos permite validar una palabra introducida por el usuario
    ''' contra el API de apalabrados
    ''' </summary>
    ''' <remarks>
    ''' Usamos HttpWebRequest en lugar del WebBrowser que hay en la aplicación por la
    ''' imposibilidad que tiene de cargar datos JSON, puesto que en lugar de mostrarlos intenta descargarlos
    ''' </remarks>
    Private Sub ValidacionPalabra()

        ' Limpiamos la palabra
        strPalabra = LimpiarPalabra(strPalabra)

        ' Creamos un objeto HttpWebRequest para poder comunicarnos con el API de la web de apalabrados
        ' puesto que la web devuelve un fichero JSON y no es tratable con el webbrowser
        Dim cadena = getWebPage("http://api.apalabrados.com/api/dictionaries/ES?words=" + strPalabra, 5000)

        ' Validamos si la petición de datos web ha tenido respuesta
        If (cadena <> String.Empty) Then
            ' Definimos los puntos entre los que se encuentra la palabra aleatoria en la página
            Dim fijacion1 As String = "{""ok"":["
            Dim fijacion2 As String = """],""wrong"":["

            ' Definimos la posición de la fijación 1
            Dim valor1 As Integer = cadena.IndexOf(fijacion1)

            ' Si valor1 es igual a -1 no se ha podido cargar la página de validación de forma correcta
            If (valor1 <> -1) Then
                ' Definimos la posición de la fijación 2 que estára siempre a continuación de la fijación 1
                Dim valor2 As Integer = cadena.IndexOf(fijacion2, valor1)


                If (valor2 <> -1) Then
                    ' La palabra será la cadena entre la posición de la fijacación 1 + su tamaño y 
                    ' la posición de la fijación 2 menos su tamaño y menos la posición de la fijación1
                    ' Asignamos la palabra parseada a la variable global que guarda la palabra a adivinar
                    If cadena.Substring(valor1 + fijacion1.Length, valor2 - (valor1 + fijacion1.Length)) <> String.Empty Then
                        ' Si dentro de los rangos de las fijaciones hay texto, la palabra es correcta
                        ' e iniciamos el juego
                        IniciarJuego()
                    Else
                        ' Si sale cualquier otra cosa rara, es un fallo en la comunicación con la web
                        MsgBox("No se ha podido validar la palabra para empezar el juego." + vbCrLf + "Póngase en contacto con el administrardor", MsgBoxStyle.Critical, "Error!")
                    End If
                Else
                    MsgBox("La palabra introducida no es válida" + vbCrLf + "Escriba una nueva palabra a continuación", MsgBoxStyle.Exclamation, "Palabra incorrecta")
                    BuscarPalabra()
                End If
            Else
                MsgBox("No se ha podido validar la palabra para empezar el juego." + vbCrLf + "Póngase en contacto con el administrardor", MsgBoxStyle.Critical, "Error!")
            End If
        Else
            MsgBox("No se ha podido establecer conexión con el servicio de validación" + vbCrLf + "Póngase en contacto con el administrador", MsgBoxStyle.Critical, "Error!")
        End If


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

    ''' <summary>
    ''' Método que reinicia los controles gráficos antes de iniciar un  nuevo juego
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ReiniciarJuego()
        ' Limpiamos la lista de letras introducidas
        lstListaLetras.Items.Clear()

        ' Cargamos la imagen inicial
        picImagen.Image = My.Resources._00

        ' Reiniciamos la variable que controla el número de errores
        intNumErrores = 0

        ' Limpiamos el label que muestra la palabra descubierta en el juego anterior
        lblPalabraDescubierta.Text = String.Empty

        ' Deshabilitamos el textbox que sirve para introducir letras
        txtIntroduceLetra.Enabled = False

        ' Deshabilitamos el boton para añadir letras
        btnAñadir.Enabled = False

        ' Buscamos una palabra nueva
        BuscarPalabra()
    End Sub

    ''' <summary>
    ''' Función que nos permite limpiar de acentos y pasar a minúscula la palabra a adivinar
    ''' </summary>
    ''' <param name="strValor">Palabra a limpiar</param>
    ''' <returns>Palabra limpia</returns>
    ''' <remarks></remarks>
    Private Function LimpiarPalabra(ByVal strValor)
        strValor = strValor.Replace("á", "a").Replace("é", "e").Replace("í", "i").Replace("ó", "o").Replace("ú", "u")

        Return strValor.ToString().ToLower()
    End Function

    ''' <summary>
    ''' Método que nos permite iniciar los controles del juego y preparar la palabra para adivinarla
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub IniciarJuego()

        ' Cambiamos los carácteres con acentos, por los mismos sin acentos para facilitar el juego
        strPalabra = LimpiarPalabra(strPalabra)

        ' Limpiamos controles y variables
        lblPalabraDescubierta.Text = String.Empty
        strPalabraAdivinada = String.Empty
        lstListaLetras.Items.Clear()

        ' Creamos la variables que almacenará lo que lleva el jugador adivinado de la palabra
        ' iterando los caracteres de la palabra a adivinar e añadiendo el caracter _ por cada posición
        For Each letra As String In strPalabra

            ' Comprobamos si hay n espacio para introducirlo en la palabra a adivinar
            If letra = " " Then
                strPalabraAdivinada = strPalabraAdivinada + " "
            Else
                strPalabraAdivinada = strPalabraAdivinada + "_"
            End If
        Next

        ' En el caso del label que muestra la palabra que lleva adivinada el usuario hacemos lo mismo
        ' con un bucle for, pero añadimos _ con un espacio
        For i As Integer = 0 To strPalabraAdivinada.Length - 1
            If strPalabraAdivinada.Chars(i) = " " Then
                lblPalabraDescubierta.Text = lblPalabraDescubierta.Text + "  "
            Else
                lblPalabraDescubierta.Text = lblPalabraDescubierta.Text + "_ "
            End If

        Next

        ' Cambiamos el estado del juego a continuar
        estadoJuego = Estado.CONTINUAR

        ' Cambiamos la etiqueta de estado
        lblEstado.Text = estadoJuego.ToString

        ' Habiliamos el boton y el textbox que permite jugar al usuario y pasamos el foco al cuadro de texto
        btnAñadir.Enabled = True
        txtIntroduceLetra.Enabled = True
        txtIntroduceLetra.Focus()

    End Sub

    ''' <summary>
    ''' Función para verificar si hay una conexión activa a Interent
    ''' </summary>
    ''' <returns>Verdadero si hay conexión, falso si no la hay</returns>
    ''' <remarks>Se usa un objeto WebClient que intenta cargar la página Google.com</remarks>
    Public Function VerificarConexionInternet() As Boolean
        ' Basamos la verificación en si podemos acceder a Google.com.
        Return getWebPage("http://www.google.com", 5000) <> String.Empty
    End Function

    ''' <summary>
    ''' Método para recoger la información de la web de generación de palabras aleatorias y 
    ''' recuperar la palabra para adivinar
    ''' </summary>
    ''' <param name="strCadena">Contenido HTML de la web de palabras alatorias</param>
    ''' <remarks></remarks>
    Private Sub parserPalabraAleatoria(ByRef strCadena)
        ' Variables para almacenar los puntos de fijación para el tratamiento de la web
        Dim fijacion1 As String
        Dim fijacion2 As String

        ' Definimos los puntos entre los que se encuentra la palabra aleatoria en la página
        fijacion1 = "<br><font size=" + Chr(34) + "6" + Chr(34) + " /><strong>"
        fijacion2 = "</strong></font>  </font></p>"

        ' Definimos la posición de la fijación 1
        Dim valor1 As Integer = strCadena.IndexOf(fijacion1)

        ' Verificamos si la página de donde cargamos las nuevas palabras se ha cargado correctamente
        ' Si no encontramos la primera fijación podemos asegurar que la página no se ha cargado tal y 
        ' como esperábamos...
        If (valor1 <> -1) Then
            ' Definimos la posición de la fijación 2 que estára siempre a continuación de la fijación 1
            Dim valor2 As Integer = strCadena.IndexOf(fijacion2, valor1)

            ' La palabra será la cadena entre la posición de la fijacación 1 + su tamaño y 
            ' la posición de la fijación 2 menos su tamaño y menos la posición de la fijación1
            ' Asignamos la palabra parseada a la variable global que guarda la palabra a adivinar
            strPalabra = strCadena.Substring(valor1 + fijacion1.Length, valor2 - fijacion2.Length - valor1).ToLower

            IniciarJuego()
        Else
            MsgBox("No se ha podido generar la palabra para empezar el juego." + vbCrLf + "Póngase en contacto con el administrardor", MsgBoxStyle.Critical, "Error!")
        End If
    End Sub

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

        'Inicializamos el marcador
        lblMarcador.Text = partidasGanadas.ToString() + " \ " + partidasJugadas.ToString()

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

            ' Validamos si el caracter es una letra
            If ValidacionLetra(strLetra) Then

                ' Verificamos si la letra introducida ya ha sido jugada anteriormente
                If lstListaLetras.Items.Contains(strLetra.ToUpper) = False Then

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

                            ' En el caso de que haya un espacio en blanco, se introduce como respuesta
                            If letras = " " Then
                                apoyo(intApoyo) = " "
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
                    ' Mostramos un mensaje avisando que el caracter introducido ya se ha usado anteriomente
                    MsgBox("Esa letra ya ha sido introducida", MsgBoxStyle.Exclamation, "Atención")
                End If
            Else
                ' Mostramos un mensaje avisando que el carácter introducido no es valido
                MsgBox("No es un caracter válido", MsgBoxStyle.Exclamation, "Atención")
            End If
        Else
            MsgBox("Debe introducir una letra para poder jugar", MsgBoxStyle.Exclamation, "Atención")
        End If

        ' Cambiamos el label que muestra el estado del juego
        lblEstado.Text = estadoJuego.ToString

        'Limpiamos el textbox
        txtIntroduceLetra.Text = String.Empty

        ' Pasamos el foco al textbox
        txtIntroduceLetra.Focus()


        ' Si el estado del juego es GANO, el jugador ha ganado
        If estadoJuego = Estado.GANO Then

            ' Aumentamos el número de partidas jugadas y ganadas
            partidasGanadas = partidasGanadas + 1
            partidasJugadas = partidasJugadas + 1

            'Actualizamos el marcador
            lblMarcador.Text = partidasGanadas.ToString() + " \ " + partidasJugadas.ToString()

            ' Mostramos un mensaje de información
            MsgBox("Ganaste", MsgBoxStyle.Information, "You Win")

            ' Reiniciamos el juego
            ReiniciarJuego()
        End If

        If estadoJuego = Estado.PIERDO Then

            ' Aumentamos el número de partidas jugadas
            partidasJugadas = partidasJugadas + 1

            'Actualizamos el marcador
            lblMarcador.Text = partidasGanadas.ToString() + " \ " + partidasJugadas.ToString()

            ' Mostramos un mensaje de información
            MsgBox("Perdiste" + vbCrLf + "La palabra a buscar era: " + strPalabra, MsgBoxStyle.Critical, "You Lose")

            ' Reiniciamos el juego
            ReiniciarJuego()
        End If
    End Sub

    ''' <summary>
    ''' Evento que controla la pulsación de teclas en el cuadro de texto para introducir letras
    ''' </summary>
    ''' <param name="sender">Objeto que envia la petición</param>
    ''' <param name="e">Argumentos del evento</param>
    ''' <remarks></remarks>
    Private Sub txtIntroduceLetra_KeyDown(sender As Object, e As KeyEventArgs) Handles txtIntroduceLetra.KeyDown
        ' Comprobamos si la tecla que se suelta es el intro, 
        ' de ser así ejecutamso el evento del click del botón
        If e.KeyData = Keys.Enter Then
            btnAñadir_Click(Nothing, Nothing)
        End If
    End Sub

#End Region

End Class
