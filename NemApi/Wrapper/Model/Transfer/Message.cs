using System;
using System.Text;

// ReSharper disable once CheckNamespace

namespace NemApi
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

        internal Message(Connection con, PrivateKey senderKey, UnverifiableAccount recipient, string message, bool encrypted)

        {
            Encrypted = encrypted;
            MessageString = message;

            if (MessageString != null)
            {
                if (Encrypted)
                {
                    if (recipient.PublicKey == null)
                        throw new ArgumentNullException("could not find recipient public key");

                    Sender = new VerifiableAccount(con, senderKey);

                    Recipient = recipient;

                    MessageBytes = new Ed25519BlockCipher(Sender, Recipient).Encrypt(Encoding.UTF8.GetBytes(MessageString));
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
        private VerifiableAccount Sender { get; }
        private UnverifiableAccount Recipient { get; }

        private void CalculateMessageFee()
        {
            Fee += Encrypted
                ? 6000000 + MessageString.Length / 32 + 1000000
                : MessageString.Length / 32 + 1000000;
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