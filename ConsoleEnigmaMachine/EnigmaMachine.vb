Public Class EnigmaMachine
    Public Rotors() As Rotor
    Public Reflector As Dictionary(Of Char, Char)

    Public Sub New(ByVal Rotors() As Rotor, ByVal Reflector As Dictionary(Of Char, Char))
        '' Initialises a new Engigma Machine class instance.
        '' Instance uses supplied array of Rotors and a dictionary containing
        '' the mappings used for the reflector.

        Me.Rotors = Rotors
        Me.Reflector = Reflector
    End Sub

    Public Sub RotateRotors()
        '' Rotates the first rotor in the machine once. Then rotates each rotor
        '' where the preceding rotor has made a complete revolution.

        ' Rotate the right-most rotor once.
        Me.Rotors.Last().Rotate()

        ' For all the remaining rotors, check to see if they have made a
        ' complete revolution (offset is 0) and then rotate.

        ' For the remaining rotors, check to see if they have just passed the
        ' notch char and then rotate.

        For i As Integer = UBound(Me.Rotors) - 1 To 0 Step -1
            If Me.Rotors(i + 1).Notches.Contains(Me.Rotors(i + 1).Mappings.Values.Last()) Then
                Me.Rotors(i).Rotate()
            End If
        Next

        'For i As Integer = 1 To UBound(Me.Rotors)
        '    If Me.Rotors(i - 1).GetOffset() = 0 Then
        '        Me.Rotors(i).Rotate()
        '    Else
        '        ' Rotors can only rotate if the preceding one has just rotated.
        '        Exit For
        '    End If
        'Next

    End Sub

    Public Function EncryptChar(ByVal PlainChar As Char)
        '' Encrypts a character by encrypting it with the first rotor and then
        '' passing the result into the next rotor.
        '' Calls EnigmaMachine.RotateRotors() before fully encrypting a character

        Dim CipherChar As Char = PlainChar

        ' First rotate the rotors so the next character uses a new alphabet.
        Me.RotateRotors()

        ' Pass the character through the rotors from right to left.
        For Each Rot As Rotor In Me.Rotors.Reverse()
            CipherChar = Rot.StandardEncrypt(CipherChar)
        Next

        ' Reflect the character once through all the rotors.
        CipherChar = Reflector(CipherChar)

        ' Pass the character through the rotors from left to right.
        For Each Rot As Rotor In Me.Rotors
            CipherChar = Rot.ReflectEncrypt(CipherChar)
        Next

        Return CipherChar
    End Function

    Public Function EncryptText(ByVal PlainText As String)
        '' Encrypts a plain text string and returns the cipher text.
        Dim CipherText As String = ""
        For Each PlainChar As Char In PlainText
            CipherText += Me.EncryptChar(PlainChar)
        Next
        Return CipherText
    End Function

End Class
