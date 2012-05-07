Imports System.Xml
Module MachineBuilder

    Public Function ReflectorFromXMLFilePath(ByVal Path As String) As Dictionary(Of Char, Char)
        '' Given a file path, returns a dictionary containing the reflector 
        '' character mappings.
        Dim XMLDoc As New XmlDocument

        XMLDoc.Load(Path)

        Return ReflectorFromXMLDocument(XMLDoc)
    End Function

    Public Function ReflectorFromXMLData(ByVal XMLData As String) As Dictionary(Of Char, Char)
        '' Given raw XML data, returns a dictionary containing the reflector
        '' character mappings.
        Dim XMLDoc As New XmlDocument

        XMLDoc.LoadXml(XMLData)

        Return ReflectorFromXMLDocument(XMLDoc)
    End Function

    Public Function ReflectorFromXMLDocument(ByRef XMLDoc As XmlDocument) As Dictionary(Of Char, Char)
        '' Given a .NET XMLDocument object, returns a dictionary containing the
        '' reflector character mappings.
        Dim ReflectorNode As XmlNode = XMLDoc.Item("reflector")

        Return MappingsFromXML(ReflectorNode)
    End Function

    Public Function RotorsFromXMLFilePath(ByVal Path As String) As Rotor()
        '' Given a file path to an XML file, returns an array of rotors.
        Dim XMLDoc As New XmlDocument

        XMLDoc.Load(Path)

        Return RotorsFromXMLDocument(XMLDoc)
    End Function

    Public Function RotorsFromXMLData(ByVal XMLData As String) As Rotor()
        '' Given raw XML data, returns an array of rotors.
        Dim XMLDoc As New XmlDocument

        XMLDoc.LoadXml(XMLData)

        Return RotorsFromXMLDocument(XMLDoc)
    End Function

    Public Function RotorsFromXMLDocument(ByRef XMLDoc As XmlDocument) As Rotor()
        '' Given a .NET XMLDocument, constructs an array of rotors.
        Dim RotorNodes As XmlNodeList = XMLDoc.GetElementsByTagName("rotor")
        Dim Rotors As New List(Of Rotor)

        ' Find rotors which don't have a specific position set and insert them
        ' in order of appearance.
        For Each RotorNode As XmlNode In RotorNodes
            If IsNothing(RotorNode.Attributes.ItemOf("position")) Then
                Dim Notches() As Char = RotorNode.Attributes.ItemOf("notches").Value.ToCharArray()
                Rotors.Add(New Rotor(MappingsFromXML(RotorNode), Notches))
            End If
        Next

        ' Find rotors which do have a specific position set and insert them
        ' into the Rotors list, pushing unspecific positioned rotors backwards.
        For Each RotorNode As XmlNode In RotorNodes
            If Not IsNothing(RotorNode.Attributes.ItemOf("position")) Then
                Dim Notches() As Char = RotorNode.Attributes.ItemOf("notches").Value.ToCharArray()
                Dim Position As Integer = Val(RotorNode.Attributes.ItemOf("position").Value) - 1
                Rotors.Insert(Position, New Rotor(MappingsFromXML(RotorNode), Notches))
            End If
        Next
        Return Rotors.ToArray()
    End Function

    Private Function MappingsFromXML(ByRef ParentNode As XmlNode) As Dictionary(Of Char, Char)
        '' Given a XMLNode mapping node, splits the node's source and
        '' destination values into a Key/Value pair.
        Dim Mappings As New Dictionary(Of Char, Char)
        For Each MappingNode As XmlNode In ParentNode.ChildNodes
            Dim Source, Destination As Char
            Source = MappingNode.Attributes.ItemOf("source").Value
            Destination = MappingNode.Attributes.ItemOf("destination").Value
            Mappings(Source) = Destination
        Next
        Return Mappings
    End Function

End Module