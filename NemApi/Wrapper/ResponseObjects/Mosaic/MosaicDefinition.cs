using System.Collections.Generic;

namespace CSharp2nem.ResponseObjects.Mosaic
{
    /// <summary>
    /// Contains a number of classes that contain mosaic properties
    /// </summary>
    public class MosaicDefinition
    {
        /// <summary>
        /// Mosaic meta data
        /// </summary>
        public class Meta
        {
            /// <summary>
            /// Mosaic ID. contains namespaceId and mosaic name
            /// </summary>
            public int Id { get; set; }
        }

        /// <summary>
        /// The mosaic ID
        /// </summary>
        public class Id
        {
            /// <summary>
            /// The name space underwhich the mosaic was created
            /// </summary>
            public string NamespaceId { get; set; }

            /// <summary>
            /// The name of the mosaic
            /// </summary>
            public string Name { get; set; }
        }

        /// <summary>
        /// The mosaic property
        /// </summary>
        /// <remarks>
        /// The properties include the following and are returned in this order:
        ///
        /// - divisibility
        /// - initialSupply
        /// - supplyMutable
        /// - Transaferable
        /// </remarks>
        public class Property
        {
            /// <summary>
            /// The property name
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// the value of the property.
            /// </summary>
            public string Value { get; set; }
        }

        /// <summary>
        /// The optional levy fee that is enforced when transfering a mosaic
        /// </summary>
        public class Levy
        {
            /// <summary>
            /// The type of levy fee.
            /// </summary>
            /// <remarks>
            /// A type of 1 denotes an absolute fee.
            /// A type of 2 denotes a percentile fee.
            /// </remarks>
            public int Type { get; set; }

            /// <summary>
            /// The beneficiary of the fee.
            /// </summary>
            public string Recipient { get; set; }

            /// <summary>
            /// The mosaic ID that should be included as a levy fee. See <see cref="Id"/>
            /// </summary>
            public Id MosaicId { get; set; }

            /// <summary>
            /// The Fee that must be paid
            /// </summary>
            /// <remarks>
            /// When the levy fee type is absolute, this property denotes the absolute  fee to be paid in the smallest units of the mosaic.
            /// When the levy fee type is percentile, this property denotes the percent fee of the transfer amount that should be paid to the beneficiary.
            /// </remarks>
            public int Fee { get; set; }
        }

        /// <summary>
        /// The mosaic definition
        /// </summary>
        public class Mosaic
        {
            /// <summary>
            /// The public key of the creator of the mosaic
            /// </summary>
            public string Creator { get; set; }
            /// <summary>
            /// The description of the mosaic
            /// </summary>
            public string Description { get; set; }

            /// <summary>
            /// The <see cref="Id"/> of the mosaic
            /// </summary>
            public Id Id { get; set; }

            /// <summary>
            /// The list of properties of the mosaic. See <see cref="Property"/>
            /// </summary>
            public List<Property> Properties { get; set; }

            /// <summary>
            /// The levy for the mosaic. See <see cref="Levy"/>
            /// </summary>
            public Levy Levy { get; set; }
        }

        /// <summary>
        /// the mosaic data
        /// </summary>
        public class Datum
        {
            /// <summary>
            /// The mosaic meta data. See <see cref="Meta"/>
            /// </summary>
            public Meta Meta { get; set; }

            /// <summary>
            /// The mosaic definition. See <see cref="Mosaic"/>
            /// </summary>
            public Mosaic Mosaic { get; set; }
        }

        /// <summary>
        /// The list of mosaic definitions.
        /// </summary>
        public class List
        {
            /// <summary>
            /// The list of mosaic definitions. See <see cref="Datum"/>
            /// </summary>
            public List<Datum> Data { get; set; }
        }

        public class Definition
        {
            /// <summary>
            /// The mosaic creator public key
            /// </summary>
            public string Creator { get; set; }
            /// <summary>
            /// The mosaic ID. See <see cref="Id"/>
            /// </summary>
            public Id Id { get; set; }

            /// <summary>
            /// The mosaic description
            /// </summary>
            public string Description { get; set; }
            /// <summary>
            /// The mosaic properties. See <see cref="Property"/>
            /// </summary>
            public List<Property> Properties { get; set; }
        }

        /// <summary>
        /// The list of mosaic definitions
        /// </summary>
        public class DefinitionList
        {
            /// <summary>
            /// The definitions. See <see cref="Definition"/>
            /// </summary>
            public List<Definition> Data { get; set; }
        }

        /// <summary>
        /// The mosaics owned
        /// </summary>
        public class MosaicOwned
        {
            /// <summary>
            /// The mosaic ID. See <see cref="Id"/>
            /// </summary>
            public Id MosaicId { get; set; }
            /// <summary>
            /// The quantity owned.
            /// </summary>
            public long Quantity { get; set; }
        }

        /// <summary>
        /// The list of owned mosaics.
        /// </summary>
        public class OwnedList
        {
            /// <summary>
            /// The mosaics. See <see cref="MosaicOwned"/>
            /// </summary>
            public List<MosaicOwned> Data { get; set; }
        }
    }
}