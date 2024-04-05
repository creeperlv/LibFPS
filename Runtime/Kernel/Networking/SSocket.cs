using LibFPS.Kernel.stdcs.stdlib;
using System;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using static LibFPS.Kernel.stdcs.stdlib.stdlib;
namespace LibFPS.Kernel.Networking
{
    public unsafe class SSocket : IDisposable
    {
        byte[] Buffer;
        byte[] Buffer2;
        int BufferSize;
        Socket UnderlyingSocket;
        RSA RSAEnc;
        Aes aes;
        ICryptoTransform AESEnc;
        public void SetPublicKey(ReadOnlySpan<byte> PublicKey)
        {
            RSAEnc.ImportRSAPublicKey(PublicKey, out _);
        }
        public void SetPrivateKey(ReadOnlySpan<byte> PrivateKey)
        {
            RSAEnc.ImportRSAPrivateKey(PrivateKey, out _);
        }
        public void WriteBuffer()
        {
            AESEnc.TransformBlock(Buffer, 0, BufferSize, Buffer2, 0);
            UnderlyingSocket.Send(Buffer2);
        }
        public SSocket(int BufferSize)
        {
            this.BufferSize = BufferSize;
            Buffer = new byte[BufferSize];
            Buffer2 = new byte[BufferSize];
            RSAEnc = RSA.Create();
        }

        public void Dispose()
        {
            UnderlyingSocket?.Dispose();
            aes.Dispose();
        }
    }
}
