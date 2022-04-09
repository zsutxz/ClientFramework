using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace SocketClient
{
    /// <summary>
    /// �ַ���Э��
    /// </summary>
    public sealed class ProtocolString : ProtocolBase
    {
        /// <summary>
        /// Э��
        /// </summary>
        public string protoStr;

        public override ProtocolBase DeCode(byte[] pBuffer, int pStart, int pLen)
        {
            ProtocolString _proto = new ProtocolString();
            _proto.protoStr = Encoding.UTF8.GetString(pBuffer, pStart, pLen);
            return _proto;
        }

        public override byte[] EnCode()
        {
            byte[] _buffer = Encoding.UTF8.GetBytes(protoStr);
            return _buffer;
        }

        public override string GetProtoName()
        {
            if (protoStr.Length == 0)
                return string.Empty;
            string[] _spl = protoStr.Split(':');
            if(_spl.Length<2)
                return string.Empty;
            return _spl[0];
        }

        public override string GetProtoConent()
        {
            return protoStr;
        }
    }

}
