using System;
using System.Text;
using Chaos.NaCl;
using CSharp2nem.Constants;
using CSharp2nem.CryptographicFunctions;
using CSharp2nem.Model.AccountSetup;
using CSharp2nem.RequestClients;
using CSharp2nem.Serialize;
using CSharp2nem.Utils;

namespace CSharp2nem.Model.Transfer
{
    /*
    * Creates/prepares a message object to be serialized
    *
    */

    internal class Message
    {
        internal Serializer Serializer = new Serializer();


        /*
        * Constructs the message object and initiates its serialization
        *
        * @Param: Serializer, The serializer to use during serialization
        * @Param: Message, The message string. Note: if null, a zero value byte[4] is serialized instead
        * @Param: Encrypted, Whether the message should be encrypted or not
        */

        internal Message(Connectivity.Connection con, PrivateKey senderKey, string address, string message, bool encrypted)

        {
            Encrypted = encrypted;
            
            MessageString = message;

            if (MessageString != null)
            {
                if (Encrypted)
                {
                    var a = new AccountClient(con).BeginGetAccountInfoFromAddress(body =>
                    {
                        PublicKey = body.Content.Account.PublicKey ?? throw new Exception("recipient public key cannot be null for encryption");
                              
                    }, address);

                    a.AsyncWaitHandle.WaitOne();

                    if (PublicKey == null)
                        throw new ArgumentNullException("could not find recipient public key");

                    Sender = new PrivateKeyAccountClient(con, senderKey);

                    MessageBytes = CryptoBytes.FromHexString(Encryption.Encode(MessageString, senderKey, PublicKey));
                }
                else
                {
                    MessageBytes = Encoding.UTF8.GetBytes(MessageString);   
                }

                PayloadLengthInBytes = MessageBytes.Length;
            }

            Serialize();

            CalculateMessageFee();
        }

        internal int Length { get; set; }
        private string MessageString { get; }
        private byte[] MessageBytes { get; set; }
        private bool Encrypted { get; }
        private int PayloadLengthInBytes { get; }
        private long Fee { get; set; }
        private PrivateKeyAccountClient Sender { get; } 
        private string PublicKey { get; set; }

        private void CalculateMessageFee()
        {          
            Fee += ((long)(50000 * (Math.Floor(((double)PayloadLengthInBytes / 32)) + 1))  );
        }

        internal long GetFee()
        {   
            return Fee;
        }

        internal byte[] GetMessageBytes()
        {
           
            return MessageBytes;
        }

        private void Serialize()
        {
            if (MessageBytes != null)
            {
                Serializer.WriteInt(MessageBytes.Length + ByteLength.EightBytes);
                Serializer.WriteInt(Encrypted ? 2 : 1);
                Serializer.WriteInt(MessageBytes.Length);
                Serializer.WriteBytes(MessageBytes);
                MessageBytes = ByteUtils.TruncateByteArray(Serializer.GetBytes(), PayloadLengthInBytes + 12);
                Length = StructureLength.MessageStructure + PayloadLengthInBytes;
            }
            else
            {
                MessageBytes = DefaultBytes.ZeroByteValue;
                Length = ByteLength.Zero;
            }
        }
    }
}