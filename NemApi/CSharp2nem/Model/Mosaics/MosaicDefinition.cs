using System.Collections.Generic;
using System.Text;
using Chaos.NaCl;
using CSharp2nem.Constants;
using CSharp2nem.Model.AccountSetup;
using CSharp2nem.Model.DataModels;
using CSharp2nem.Serialize;
using CSharp2nem.Utils;

namespace CSharp2nem.Model.Mosaics
{
    /// <summary>
    /// 
    /// </summary>
    internal class MosaicDefinition
    {
        internal readonly Serializer Serializer = new Serializer();

        internal MosaicDefinition(MosaicCreationData data, PublicKey creator)
        {
            Creator = creator;
            Model = data;
            NamespaceId = Encoding.Default.GetBytes(data.NameSpaceId);
            MosaicName = Encoding.Default.GetBytes(data.MosaicName);
            Description = Encoding.Default.GetBytes(data.Description);

            SetProperties(data.Divisibility, data.InitialSupply, data.SupplyMutable, data.Transferable);
            Serialize();

            DefinitionBytes = ByteUtils.TruncateByteArray(Serializer.GetBytes(), Length);
        }

        internal byte[] NamespaceId { get; set; }
        internal byte[] MosaicName { get; set; }
        internal byte[] Description { get; set; }
        internal List<MosaicProperty> MosaicProperties { get; set; }
        internal MosaicCreationData Model { get; set; }
        internal PublicKey Creator { get; set; }
        internal int Length { get; set; }
        internal byte[] DefinitionBytes { get; set; }

        private void SetProperties(int divisibility, long initialSupply, bool supplyMutable, bool transferable)
        {
            MosaicProperties = new List<MosaicProperty>
            {
                new MosaicProperty("divisibility", divisibility.ToString()),
                new MosaicProperty("initialSupply", initialSupply.ToString()),
                new MosaicProperty("supplyMutable", supplyMutable ? "true" : "false"),
                new MosaicProperty("transferable", transferable ? "true" : "false")
            };
        }

        private void Serialize()
        {
            var sumLen = 0;

            foreach (var p in MosaicProperties)
            {
                sumLen += p.PropertyLength + 4;
            }

            Length = 60 + NamespaceId.Length
                     + MosaicName.Length
                     + Description.Length
                     + sumLen;

            // mosaic definition length
            if (Model.MosaicLevy != null)
            {
                Serializer.WriteInt(Length + (Model.MosaicLevy.Length - 4));
            }
            else
            {
                Serializer.WriteInt(Length);
            }

            // length if creator public key
            Serializer.WriteInt(ByteLength.PublicKeyLength);

            // creator public key
            Serializer.WriteBytes(CryptoBytes.FromHexString(Creator.Raw));

            // mosaic id structure length
            Serializer.WriteInt(ByteLength.EightBytes + NamespaceId.Length + MosaicName.Length);

            // namespace id length
            Serializer.WriteInt(NamespaceId.Length);

            // namespace
            Serializer.WriteBytes(NamespaceId);

            // mosaic name length
            Serializer.WriteInt(MosaicName.Length);

            // mosaic name  
            Serializer.WriteBytes(MosaicName);

            // description length
            Serializer.WriteInt(Description.Length);

            // description
            Serializer.WriteBytes(Description);

            // properties count
            Serializer.WriteInt(MosaicProperties.Count);

            foreach (var mp in MosaicProperties)
                Serializer.WriteBytes(mp.GetBytes());

        }

        internal byte[] GetBytes()
        {
            return DefinitionBytes;
        }
    }
}