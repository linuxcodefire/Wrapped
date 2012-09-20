Imports System.Net
Imports System.Net.Sockets
Imports System.Text
Imports System.IO
Imports System.Security.Cryptography
Public Class Wrapped
    Public _stream As NetworkStream

    Public Sub New(ByVal stream As NetworkStream)
        _stream = stream
    End Sub
#Region "TEMP"
    '=====================================
    '            Strings
    '=====================================

    Public Function readString() As String
        Try
            Dim length As Short = readShort()
            Dim Stringbytes((length * 2) - 1) As Byte
            Dim mylen As Integer = _stream.Read(Stringbytes, 0, length * 2)
            While 1 = 1
                If Not mylen = length * 2 Then
                    Dim NewSize As Integer = (length * 2) - mylen
                    Dim Bytesread As Integer
                    Bytesread = _stream.Read(Stringbytes, mylen - 1, NewSize)
                    If Not Bytesread = NewSize Then
                        length = NewSize / 2
                        mylen = Bytesread
                    Else
                        Exit While
                    End If
                Else
                    Exit While
                End If
            End While
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
            _stream.Write(final, 0, final.Length)
        Catch ex As Exception
            Exit Sub
        End Try

    End Sub
    '=====================================
    '          Shorts
    '=====================================
    Public Function readShort() As Short
        Try
            Dim bytes(1) As Byte
            Dim BytesRead As Integer = _stream.Read(bytes, 0, 2)
            While 1 = 1
                If Not BytesRead = 2 Then
                    Dim newsize As Integer = 2 - BytesRead
                    Dim BytesRead1 As Integer = _stream.Read(bytes, BytesRead - 1, newsize)
                    If Not BytesRead1 = newsize Then
                        BytesRead = BytesRead1 'Its quite sad that I even have to do this. <_<
                    Else : Exit While
                    End If
                Else
                    Exit While
                End If
            End While
            Array.Reverse(bytes) 'Converts to little-endian
            Return BitConverter.ToInt16(bytes, 0)
        Catch ex As Exception
            Exit Function
        End Try

    End Function

    Public Sub writeShort(ByVal message As Short)
        Try
            Dim bytes() As Byte = BitConverter.GetBytes(message)
            Array.Reverse(bytes) 'Convert to bigendian
            _stream.Write(bytes, 0, 2)
        Catch ex As Exception
            Exit Sub
        End Try

    End Sub

    '=========================
    '     Integer
    '=========================

    Public Function readInt() As Integer
        Try
            Dim bytes(3) As Byte
            Dim Bytesread As Integer = _stream.Read(bytes, 0, 4)
            While 1 = 1
                If Not Bytesread = 4 Then
                    Dim NewSize As Integer = 4 - Bytesread
                    Dim Bytesread1 As Integer = _stream.Read(bytes, Bytesread - 1, NewSize)
                    If Not Bytesread1 = NewSize Then
                        Bytesread = Bytesread1
                    Else
                        Exit While
                    End If
                Else
                    Exit While
                End If
            End While
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
            _stream.Write(bytes, 0, 4)
        Catch ex As Exception
            Exit Sub
        End Try

    End Sub
    '===========================
    '      Long!
    '===========================
    Public Function readLong() As Long
        Try
            Dim bytes(7) As Byte
            Dim Bytesread As Integer = _stream.Read(bytes, 0, 8)
            While 1 = 1
                If Not Bytesread = 8 Then
                    Dim NewSize As Integer = 8 - Bytesread
                    Dim Bytesread1 As Integer = _stream.Read(bytes, Bytesread - 1, NewSize)
                    If Not Bytesread1 = NewSize Then
                        Bytesread = Bytesread1
                    Else
                        Exit While
                    End If
                Else
                    Exit While
                End If
            End While
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
            _stream.Write(bytes, 0, 8)
        Catch ex As Exception
            Exit Sub
        End Try

    End Sub

    '===========================
    'Doubles!
    '===========================

    Public Function readDouble() As Double
        Try
            Dim bytes(7) As Byte
            Dim Bytesread As Integer = _stream.Read(bytes, 0, 8)
            While 1 = 1
                If Not Bytesread = 8 Then
                    Dim NewSize As Integer = 8 - Bytesread
                    Dim Bytesread1 As Integer = _stream.Read(bytes, Bytesread - 1, NewSize)
                    If Not Bytesread1 = NewSize Then
                        Bytesread = Bytesread1
                    Else
                        Exit While
                    End If
                Else
                    Exit While
                End If
            End While
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
            _stream.Write(bytes, 0, 8)
        Catch ex As Exception
            Exit Sub
        End Try

    End Sub

    '=========================
    '   Floats! (Single)
    '=========================

    Public Function readFloat() As Single
        Try
            Dim bytes(3) As Byte
            Dim Bytesread As Integer = _stream.Read(bytes, 0, 4)
            While 1 = 1
                If Not Bytesread = 4 Then
                    Dim NewSize As Integer = 4 - Bytesread
                    Dim Bytesread1 As Integer = _stream.Read(bytes, Bytesread - 1, NewSize)
                    If Not Bytesread1 = NewSize Then
                        Bytesread = Bytesread1
                    Else
                        Exit While
                    End If
                Else
                    Exit While
                End If
            End While
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
            _stream.Write(bytes, 0, 4)
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
            Return _stream.ReadByte()
        Catch ex As Exception
            Exit Function
        End Try

    End Function

    Public Sub writeByte(ByVal mybyte As Byte)
        Try
            _stream.WriteByte(mybyte)
        Catch ex As Exception
            Exit Sub
        End Try

    End Sub

    Public Function readSByte() As SByte
        Try
            Return Convert.ToSByte(_stream.ReadByte())
        Catch ex As Exception
            Exit Function
        End Try

    End Function

    Public Sub writeSByte(ByVal mybyte As SByte)
        Try
            _stream.WriteByte(Convert.ToByte(mybyte))
        Catch ex As Exception
            Exit Sub
        End Try

    End Sub
    '=========================
    '     Bool!
    '=========================

    Public Function readBool() As Boolean
        Try
            Return Convert.ToBoolean(_stream.ReadByte)
        Catch ex As Exception
            Exit Function
        End Try

    End Function

    Public Sub writeBool(ByVal mybool As Boolean)
        Try
            _stream.WriteByte(Convert.ToByte(mybool))
        Catch ex As Exception
            Exit Sub
        End Try

    End Sub
#End Region
    '------------------------------------------------
    '-- A part not actually copied from SinZ,
    '-- Recursive byte reads!!!
    '------------------------------------------------

    Public Function readByteArray(ByVal size As Integer) As Byte()
        Console.WriteLine("-- Umby24 Byte Array Processor --")
        Console.WriteLine("Size is " & size)
        Dim MyBytes(size - 1) As Byte
        Dim BytesRead1 As Integer
        BytesRead1 = _stream.Read(MyBytes, 0, size)
        Console.WriteLine("Read " & BytesRead1)
        While 1 = 1
            If Not BytesRead1 = size Then
                Dim newsize As Integer = size - BytesRead1
                Dim Bytesread As Integer
                Bytesread = _stream.Read(MyBytes, BytesRead1 - 1, newsize)
                Console.WriteLine("Read(2) " & Bytesread)
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
    End Function
#End Region
    Public enc As CryptoStream
    Public dec As CryptoStream
    Public Sub AesStream(ByVal key() As Byte, ByVal Stream As Stream)
        Dim Basestream = Stream
        enc = New CryptoStream(Stream, GenerateAES(key).CreateEncryptor, CryptoStreamMode.Write)
        dec = New CryptoStream(Stream, GenerateAES(key).CreateDecryptor, CryptoStreamMode.Read)
    End Sub
    Private Function GenerateAES(ByVal key() As Byte) As RijndaelManaged
        Dim ciper As RijndaelManaged = New RijndaelManaged
        ciper.Mode = CipherMode.CFB
        ciper.Padding = PaddingMode.None
        ciper.KeySize = 128
        ciper.FeedbackSize = 8
        ciper.Key = key
        ciper.IV = key
        Return ciper
    End Function

End Class
