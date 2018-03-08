using System;
using System.Collections.Generic;
using CSharp2nem.Constants;
using CSharp2nem.Model.AccountSetup;
using CSharp2nem.RequestClients;
using CSharp2nem.Serialize;
using CSharp2nem.Utils;

namespace CSharp2nem.Model.Transfer.Mosaics
{
    /*
    * A list of mosaics which can be serilized.
    */

    internal class MosaicList
    {
        internal MosaicList(List<Mosaic> mosaicList, Connectivity.Connection connection, PublicKey sender)
        {
            Sender = sender;
            Connection = connection;
            Serializer = new Serializer();
            ListOfMosaics = mosaicList;

            Serialize();

            Bytes = ByteUtils.TruncateByteArray(Serializer.GetBytes(), Length);
            CalculateMosaicTransferFee();               
        }

        private PublicKey Sender { get; }
        private Connectivity.Connection Connection { get; }
        private List<Mosaic> ListOfMosaics { get; }
        private Serializer Serializer { get; }
        private byte[] Bytes { get; }
        internal int Length { get; set; }
        private long TotalFee { get; set; }

        internal byte[] GetMosaicListBytes()
        {
            return Bytes;
        }

        private void CalculateMosaicTransferFee()
        {
            // loop through mosaics to be sent
            foreach (var mosaicToBeSent in ListOfMosaics)
            {

                // get all mosaics under the same namespace as the mosaic to be sent 
                new NamespaceMosaicClient(Connection).BeginGetMosaicsByNameSpace(body =>
                {
                    // TODO: fix fee for when xem isnt included in the transfer 
                    if (body?.Content?.Data == null)
                    {
                        return;
                    }

                    // loop through mosaics found under namespace
                    foreach (var mosaicDefinition in body.Content.Data)
                    {
                        int fractionOfWhole = 20;
                        // skip if mosaic to send doesnt match mosaic found
                        if (mosaicDefinition.Mosaic.Id.Name != mosaicToBeSent.MosaicName) continue;

                        // get mosaic properties
                        var q = mosaicToBeSent.Quantity;
                        var d = Convert.ToInt32(mosaicDefinition.Mosaic.Properties[0].Value);
                        var s = Convert.ToInt64(mosaicDefinition.Mosaic.Properties[1].Value);

                        // check for business mosaic
                        if (s <= 10000 && d == 0)
                        {
                            TotalFee += 1000000;
                        }
                        // compute regular mosaic fee
                        else
                        {
                            // get xem equivilent
                            var xemEquivalent = 8999999999 * (q / Math.Pow(10, d)) / (s * 10 ^ d) * 1000000;

                            // apply xem transfer fee formula 
                            var xemFee = Math.Max(1, Math.Min((long)Math.Ceiling((decimal)xemEquivalent / 1000000000), 25)) * 1000000;

                            // Adjust fee based on supply
                            const long maxMosaicQuantity = 9000000000000000;

                            // get total mosaic quantity
                            var totalMosaicQuantity = s * Math.Pow(10, d);

                            // get supply related adjustment
                            var supplyRelatedAdjustment = Math.Floor(0.8 * Math.Log(maxMosaicQuantity / totalMosaicQuantity)) * 1000000;

                            // get final individual mosaic fee
                            var individualMosaicfee = (long)Math.Max(1000000, xemFee - supplyRelatedAdjustment);

                            // add individual fee to total fee for all mosaics to be sent 
                            TotalFee += individualMosaicfee / 20;  
                        }
                        break;
                    }
                }, mosaicToBeSent.NameSpaceId).AsyncWaitHandle.WaitOne();
            }
        
        }

        internal long GetFee()
        {
            return TotalFee;
        }

        /*
        * Serializes the list of mosaics one by one.
        *
        * If there are no mosaics, an empty 4 bytes are serialized in place of the mosaics.
        *
        */
        private void Serialize()
        {
            Serializer.WriteInt(ListOfMosaics.Count);
            Length += 4;

            foreach (var mosaic in ListOfMosaics)
            {
                Length += mosaic.LengthOfMosaicStructure + ByteLength.FourBytes;

                Serializer.WriteInt(mosaic.LengthOfMosaicStructure);

                Serializer.WriteInt(mosaic.LengthOfMosaicIdStructure);

                Serializer.WriteInt(mosaic.LengthOfNameSpaceId);

                Serializer.WriteString(mosaic.NameSpaceId);

                Serializer.WriteInt(mosaic.LengthOfMosaicName);

                Serializer.WriteString(mosaic.MosaicName);

                Serializer.WriteLong(mosaic.Quantity);
              
            }
        }
    }
}