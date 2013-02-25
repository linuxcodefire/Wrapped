Imports System.Net
Imports System.Net.Sockets
Imports System.Text
Imports System.IO
Imports System.Security.Cryptography

Public Class Wrapped

    'Thank you to SirCmpwn for encryption, taken from SMProxy.
    'Which got this from Pdelvo.Minecraft, by _x68x.

    Public _stream As NetworkStream
    Public crypto As AesStream
    Public EncEnabled As Boolean = False

    Public Sub New(ByVal stream As NetworkStream)
        _stream = stream
    End Sub

    Public Sub InitEncryption(ByVal key() As Byte)
        crypto = New AesStream(_stream, key)
    End Sub

    '=====================================
    '            Strings
    '=====================================

    Public Function readString() As String
        Try
            Dim length As Short = readShort()
            Dim Stringbytes() As Byte = readByteArray(length * 2)

            Return Encoding.BigEndianUnicode.GetChars(Stringbytes)

        Catch ex As Exception
            Exit Function
        End Try

    End Function

    Public Sub writeString(ByVal message As String)
        Try
            Dim length() As Byte = BitConverter.GetBytes(CType(message.Length, Short))
            Array.Reverse(length)

            Dim final((message.Length * 2) + 1) As Byte
            Array.Copy(length, final, 2)
            Array.Copy(Encoding.BigEndianUnicode.GetBytes(message), 0, final, 2, message.Length * 2)

            Send(final)
        Catch ex As Exception
            Exit Sub
        End Try

    End Sub
    '=====================================
    '          Shorts
    '=====================================
    Public Function readShort() As Short
        Try
            Dim bytes() As Byte = readByteArray(2)

            Array.Reverse(bytes)
            Return BitConverter.ToInt16(bytes, 0)

        Catch ex As Exception
            Exit Function
        End Try
    End Function

    Public Sub writeShort(ByVal message As Short)
        Try
            Dim bytes() As Byte = BitConverter.GetBytes(message)
            Array.Reverse(bytes) 'Convert to bigendian

            Send(bytes)
        Catch ex As Exception
            Exit Sub
        End Try

    End Sub

    '=========================
    '     Integer
    '=========================

    Public Function readInt() As Integer
        Try
            Dim bytes() As Byte = readByteArray(4)
            Array.Reverse(bytes)

            Return BitConverter.ToInt32(bytes, 0)
        Catch ex As Exception
            Exit Function
        End Try
    End Function

    Public Sub writeInt(ByVal number As Integer)
        Try
            Dim bytes() As Byte = BitConverter.GetBytes(number)
            Array.Reverse(bytes)

            Send(bytes)
        Catch ex As Exception
            Exit Sub
        End Try

    End Sub
    '===========================
    '      Long!
    '===========================
    Public Function readLong() As Long
        Try
            Dim bytes() As Byte = readByteArray(8)
            Array.Reverse(bytes)

            Return BitConverter.ToInt64(bytes, 0)
        Catch ex As Exception
            Exit Function
        End Try
    End Function

    Public Sub writeLong(ByVal what As Long)
        Try
            Dim bytes() As Byte = BitConverter.GetBytes(what)
            Array.Reverse(bytes)

            Send(bytes)
        Catch ex As Exception
            Exit Sub
        End Try

    End Sub

    '===========================
    'Doubles!
    '===========================

    Public Function readDouble() As Double
        Try
            Dim bytes() As Byte = readByteArray(8)
            Array.Reverse(bytes)

            Return BitConverter.ToDouble(bytes, 0)
        Catch ex As Exception
            Exit Function
        End Try
    End Function

    Public Sub writeDouble(ByVal what As Double)
        Try
            Dim bytes() As Byte = BitConverter.GetBytes(what)
            Array.Reverse(bytes)

            Send(bytes)
        Catch ex As Exception
            Exit Sub
        End Try

    End Sub

    '=========================
    '   Floats! (Single)
    '=========================

    Public Function readFloat() As Single
        Try
            Dim bytes() As Byte = readByteArray(4)
            Array.Reverse(bytes)

            Return BitConverter.ToSingle(bytes, 0)
        Catch ex As Exception
            Exit Function
        End Try

    End Function

    Public Sub writeFloat(ByVal number As Single)
        Try
            Dim bytes() As Byte = BitConverter.GetBytes(number)
            Array.Reverse(bytes)

            Send(bytes)
        Catch ex As Exception
            Exit Sub
        End Try

    End Sub


#Region "Bytes"
    '==========================
    '      Bytes!
    '==========================

    Public Function readByte() As Byte
        Try
            Return readSingleByte()
        Catch ex As Exception
            Exit Function
        End Try

    End Function

    Public Sub writeByte(ByVal mybyte As Byte)
        Try
            SendByte(mybyte)
        Catch ex As Exception
            Exit Sub
        End Try

    End Sub

    Public Function readSByte() As SByte
        Try
            Return Convert.ToSByte(readSingleByte())
        Catch ex As Exception
            Exit Function
        End Try
    End Function

    Public Sub writeSByte(ByVal mybyte As SByte)
        Try
            SendByte(Convert.ToByte(mybyte))
        Catch ex As Exception
            Exit Sub
        End Try
    End Sub
    '=========================
    '     Bool!
    '=========================

    Public Function readBool() As Boolean
        Try
            Return Convert.ToBoolean(readSingleByte())
        Catch ex As Exception
            Exit Function
        End Try

    End Function

    Public Sub writeBool(ByVal mybool As Boolean)
        Try
            SendByte(Convert.ToByte(mybool))
        Catch ex As Exception
            Exit Sub
        End Try
    End Sub
#End Region
    '------------------------------------------------
    '-- A part not actually copied from SinZ,
    '-- Recursive byte reads!!!
    '------------------------------------------------
#Region "Send and Receive"

    Public Function readSingleByte() As Byte
        If EncEnabled Then
            Return crypto.decryptStream.ReadByte()
        Else
            Return _stream.ReadByte()
        End If
    End Function
    Public Function readByteArray(ByVal size As Integer) As Byte()
        If EncEnabled = False Then
            Dim MyBytes(size - 1) As Byte
            Dim BytesRead1 As Integer

            BytesRead1 = _stream.Read(MyBytes, 0, size)

            While 1 = 1
                If Not BytesRead1 = size Then
                    Dim newsize As Integer = size - BytesRead1
                    Dim Bytesread As Integer
                    Bytesread = _stream.Read(MyBytes, BytesRead1 - 1, newsize)
                    If Not Bytesread = newsize Then
                        size = newsize
                        BytesRead1 = Bytesread
                    Else
                        Exit While
                    End If
                Else
                    Exit While
                End If
            End While

            Return MyBytes
        Else
            Dim MyBytes(size - 1) As Byte
            Dim BytesRead1 As Integer

            BytesRead1 = crypto.decryptStream.Read(MyBytes, 0, size)

            While 1 = 1
                If Not BytesRead1 = size Then
                    Dim newsize As Integer = size - BytesRead1
                    Dim Bytesread As Integer
                    Bytesread = crypto.decryptStream.Read(MyBytes, BytesRead1 - 1, newsize)
                    If Not Bytesread = newsize Then
                        size = newsize
                        BytesRead1 = Bytesread
                    Else
                        Exit While
                    End If
                Else
                    Exit While
                End If
            End While

            Return MyBytes
        End If

    End Function

    Sub Send(ByVal Buffer As Byte())

        If EncEnabled Then
            crypto.encryptStream.Write(Buffer, 0, Buffer.Length)
        Else
            _stream.Write(Buffer, 0, Buffer.Length)
        End If

    End Sub

    Sub SendByte(ByVal thisByte As Byte)
        If EncEnabled Then
            crypto.encryptStream.WriteByte(thisByte)
        Else
            _stream.WriteByte(thisByte)
        End If
    End Sub
#End Region
End Class
