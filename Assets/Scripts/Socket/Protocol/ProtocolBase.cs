
namespace SocketClient
{
    /// <summary>
    /// Э�����
    ///  Э���ʽ ����Э����:Э�鳤��:Э�����ݡ�;
    /// </summary>
    public class ProtocolBase
    {
        /// <summary>
        /// ���ݱ���
        /// </summary>
        /// <returns></returns>
        public virtual byte[] EnCode()
        {
            return null;
        }


        /// <summary>
        /// ���ݽ���
        /// </summary>
        /// <returns></returns>
        public  virtual ProtocolBase DeCode(byte[] pBuffer,int pStart,int pLen)
        {
            return null;
        }

        /// <summary>
        /// ��ȡЭ����
        /// </summary>
        /// <returns></returns>
        public virtual string GetProtoName()
        {
            return null;
        }

        /// <summary>
        /// ��ȡЭ������
        /// </summary>
        /// <returns></returns>
        public virtual string GetProtoConent()
        {
            return null;
        }

    }

 


}

