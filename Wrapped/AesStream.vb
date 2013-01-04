Imports System
Imports System.IO
Imports System.Security.Cryptography

Public Class AesStream
    Public decryptStream As CryptoStream
    Public encryptStream As CryptoStream
    Private _key() As Byte
    Public baseStream As Stream

    Public Sub New(ByVal stream As Stream, ByVal key() As Byte)
        baseStream = stream
        _key = key
        Dim raj = GenerateAES(key)
        Dim encTrans = raj.CreateEncryptor()
        Dim decTrans = raj.CreateDecryptor()

        encryptStream = New CryptoStream(baseStream, encTrans, CryptoStreamMode.Write)
        decryptStream = New CryptoStream(baseStream, decTrans, CryptoStreamMode.Read)

    End Sub
    Private Function GenerateAES(ByVal key() As Byte) As Rijndael
        Dim Cipher = New RijndaelManaged
        Cipher.Mode = CipherMode.CFB
        Cipher.Padding = PaddingMode.None
        Cipher.KeySize = 128
        Cipher.FeedbackSize = 8
        Cipher.Key = key
        Cipher.IV = key
        Return Cipher
    End Function

End Class
