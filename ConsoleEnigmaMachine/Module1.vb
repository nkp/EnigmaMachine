Module Module1

    '' Manual testing interface.

    Dim RotorsXML As String = My.Resources.German123
    Dim ReflectorsXML As String = My.Resources.ReflectorB

    Sub Main()
        Dim Rotors() As Rotor
        Dim Reflector As Dictionary(Of Char, Char)
        Dim Machine As EnigmaMachine
        Rotors = MachineBuilder.RotorsFromXMLData(RotorsXML)
        Reflector = MachineBuilder.ReflectorFromXMLData(ReflectorsXML)

        Machine = New EnigmaMachine(Rotors, Reflector)
        PreRotateRotors(Machine)

        While 1
            Dim PlainText As String

            PlainText = Console.ReadLine().ToUpper()
            If PlainText = "--Q" Then
                Console.WriteLine("Quitting...")
                Exit While
            ElseIf PlainText = "--R" Then
                Console.WriteLine("Resetting...")
                Rotors = MachineBuilder.RotorsFromXMLData(RotorsXML)
                Reflector = MachineBuilder.ReflectorFromXMLData(ReflectorsXML)
                Machine = New EnigmaMachine(Rotors, Reflector)
                PreRotateRotors(Machine)
                Continue While
            End If
            Console.WriteLine(Machine.EncryptText(PlainText))
        End While
    End Sub

    Public Sub PreRotateRotors(ByRef Machine As EnigmaMachine)
        For i As Integer = 0 To UBound(Machine.Rotors)
            Dim PreRotations As String
            Console.Write("Pre rotate rotor " & i + 1 & ": ")
            PreRotations = Console.ReadLine()
            For j As Integer = 1 To Val(PreRotations) Mod Machine.Rotors(i).Mappings.Count
                Machine.Rotors(i).Rotate()
            Next
        Next
    End Sub

End Module
