<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Principal
    Inherits System.Windows.Forms.Form

    'Form reemplaza a Dispose para limpiar la lista de componentes.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Requerido por el Diseñador de Windows Forms
    Private components As System.ComponentModel.IContainer

    'NOTA: el Diseñador de Windows Forms necesita el siguiente procedimiento
    'Se puede modificar usando el Diseñador de Windows Forms.  
    'No lo modifique con el editor de código.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Principal))
        Me.lblPalabraDescubierta = New System.Windows.Forms.Label()
        Me.txtIntroduceLetra = New System.Windows.Forms.TextBox()
        Me.btnAñadir = New System.Windows.Forms.Button()
        Me.lblEstado = New System.Windows.Forms.Label()
        Me.lstListaLetras = New System.Windows.Forms.ListBox()
        Me.Navegador = New System.Windows.Forms.WebBrowser()
        Me.picImagen = New System.Windows.Forms.PictureBox()
        CType(Me.picImagen, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblPalabraDescubierta
        '
        Me.lblPalabraDescubierta.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblPalabraDescubierta.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPalabraDescubierta.Location = New System.Drawing.Point(12, 16)
        Me.lblPalabraDescubierta.Name = "lblPalabraDescubierta"
        Me.lblPalabraDescubierta.Size = New System.Drawing.Size(310, 23)
        Me.lblPalabraDescubierta.TabIndex = 1
        Me.lblPalabraDescubierta.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtIntroduceLetra
        '
        Me.txtIntroduceLetra.Location = New System.Drawing.Point(12, 364)
        Me.txtIntroduceLetra.Name = "txtIntroduceLetra"
        Me.txtIntroduceLetra.Size = New System.Drawing.Size(137, 20)
        Me.txtIntroduceLetra.TabIndex = 2
        '
        'btnAñadir
        '
        Me.btnAñadir.Location = New System.Drawing.Point(155, 362)
        Me.btnAñadir.Name = "btnAñadir"
        Me.btnAñadir.Size = New System.Drawing.Size(167, 23)
        Me.btnAñadir.TabIndex = 3
        Me.btnAñadir.Text = "Introducir"
        Me.btnAñadir.UseVisualStyleBackColor = True
        '
        'lblEstado
        '
        Me.lblEstado.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblEstado.Location = New System.Drawing.Point(12, 398)
        Me.lblEstado.Name = "lblEstado"
        Me.lblEstado.Size = New System.Drawing.Size(310, 23)
        Me.lblEstado.TabIndex = 4
        Me.lblEstado.Text = "Label1"
        Me.lblEstado.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lstListaLetras
        '
        Me.lstListaLetras.FormattingEnabled = True
        Me.lstListaLetras.Location = New System.Drawing.Point(328, 58)
        Me.lstListaLetras.Name = "lstListaLetras"
        Me.lstListaLetras.Size = New System.Drawing.Size(41, 329)
        Me.lstListaLetras.TabIndex = 5
        '
        'Navegador
        '
        Me.Navegador.Location = New System.Drawing.Point(445, 37)
        Me.Navegador.MinimumSize = New System.Drawing.Size(20, 20)
        Me.Navegador.Name = "Navegador"
        Me.Navegador.Size = New System.Drawing.Size(250, 250)
        Me.Navegador.TabIndex = 6
        '
        'picImagen
        '
        Me.picImagen.Image = Global.WindowsApplication1.My.Resources.Resources._00
        Me.picImagen.Location = New System.Drawing.Point(12, 58)
        Me.picImagen.Name = "picImagen"
        Me.picImagen.Size = New System.Drawing.Size(310, 283)
        Me.picImagen.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.picImagen.TabIndex = 0
        Me.picImagen.TabStop = False
        '
        'Principal
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(375, 392)
        Me.Controls.Add(Me.Navegador)
        Me.Controls.Add(Me.lstListaLetras)
        Me.Controls.Add(Me.lblEstado)
        Me.Controls.Add(Me.btnAñadir)
        Me.Controls.Add(Me.txtIntroduceLetra)
        Me.Controls.Add(Me.lblPalabraDescubierta)
        Me.Controls.Add(Me.picImagen)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "Principal"
        Me.Text = "Ahorcado"
        CType(Me.picImagen, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents picImagen As System.Windows.Forms.PictureBox
    Friend WithEvents lblPalabraDescubierta As System.Windows.Forms.Label
    Friend WithEvents txtIntroduceLetra As System.Windows.Forms.TextBox
    Friend WithEvents btnAñadir As System.Windows.Forms.Button
    Friend WithEvents lblEstado As System.Windows.Forms.Label
    Friend WithEvents lstListaLetras As System.Windows.Forms.ListBox
    Friend WithEvents Navegador As System.Windows.Forms.WebBrowser

End Class
