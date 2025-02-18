using System;
using System.Reflection;
using log4net;
using PacketDotNet.Utils;

namespace PacketDotNet.LLDP
{
    /// <summary>
    ///     An Organization Specific TLV
    ///     [TLV Type Length : 2][Organizationally Unique Identifier OUI : 3]
    ///     [Organizationally Defined Subtype : 1][Organizationally Defined Information String : 0 - 507]
    /// </summary>
    public class OrganizationSpecific : TLV
    {
#if DEBUG
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
#else
    // NOTE: No need to warn about lack of use, the compiler won't
    //       put any calls to 'log' here but we need 'log' to exist to compile
#pragma warning disable 0169, 0649
        private static readonly ILogInactive log;
#pragma warning restore 0169, 0649
#endif

        private const int OUILength = 3;
        private const int OUISubTypeLength = 1;

        #region Constructors
        /// <summary>
        ///     Creates an Organization Specific TLV
        /// </summary>
        /// <param name="bytes">
        ///     The LLDP Data unit being modified
        /// </param>
        /// <param name="offset">
        ///     The Organization Specific TLV's offset from the
        ///     origin of the LLDP
        /// </param>
        public OrganizationSpecific(byte[] bytes, int offset) : base(bytes, offset)
        {
            log.Debug("");
        }

        /// <summary>
        ///     Creates an Organization Specific TLV and sets it value
        /// </summary>
        /// <param name="oui">
        ///     An Organizationally Unique Identifier
        /// </param>
        /// <param name="subType">
        ///     An Organizationally Defined SubType
        /// </param>
        /// <param name="infoString">
        ///     An Organizationally Defined Information String
        /// </param>
        public OrganizationSpecific(byte[] oui, int subType, byte[] infoString)
        {
            log.Debug("");

            var length = TLVTypeLength.TypeLengthLength + OUILength + OUISubTypeLength;
            var bytes = new byte[length];
            var offset = 0;
            this.tlvData = new ByteArraySegment(bytes, offset, length);

            this.Type = TLVTypes.OrganizationSpecific;

            this.OrganizationUniqueID = oui;
            this.OrganizationDefinedSubType = subType;
            this.OrganizationDefinedInfoString = infoString;
        }
        #endregion

        #region Properties
        /// <summary>
        ///     An Organizationally Unique Identifier
        /// </summary>
        public byte[] OrganizationUniqueID
        {
            get
            {
                var oui = new byte[OUILength];
                Array.Copy(this.tlvData.Bytes, this.ValueOffset, oui, 0, OUILength);
                return oui;
            }

            set { Array.Copy(value, 0, this.tlvData.Bytes, this.ValueOffset, OUILength); }
        }

        /// <summary>
        ///     An Organizationally Defined SubType
        /// </summary>
        public int OrganizationDefinedSubType
        {
            get { return this.tlvData.Bytes[this.ValueOffset + OUILength]; }
            set { this.tlvData.Bytes[this.ValueOffset + OUILength] = (byte) value; }
        }

        /// <summary>
        ///     An Organizationally Defined Information String
        /// </summary>
        public byte[] OrganizationDefinedInfoString
        {
            get
            {
                var length = this.Length - (OUILength + OUISubTypeLength);

                var bytes = new byte[length];
                Array.Copy(this.tlvData.Bytes, this.ValueOffset + OUILength + OUISubTypeLength, bytes, 0, length);

                return bytes;
            }

            set
            {
                var length = this.Length - (OUILength + OUISubTypeLength);

                // do we have the right sized tlv?
                if(value.Length != length)
                {
                    var headerLength = TLVTypeLength.TypeLengthLength + OUILength + OUISubTypeLength;

                    // resize the tlv
                    var newLength = headerLength + value.Length;
                    var bytes = new byte[newLength];

                    // copy the header bytes over
                    Array.Copy(this.tlvData.Bytes, this.tlvData.Offset, bytes, 0, headerLength);

                    // assign a new ByteArrayAndOffset to tlvData
                    var offset = 0;
                    this.tlvData = new ByteArraySegment(bytes, offset, newLength);
                }

                // copy the byte array in
                Array.Copy(value, 0, this.tlvData.Bytes, this.ValueOffset + OUILength + OUISubTypeLength, value.Length);
            }
        }

        /// <summary>
        ///     Convert this Organization Specific TLV to a string.
        /// </summary>
        /// <returns>
        ///     A human readable string
        /// </returns>
        public override string ToString()
        {
            return string.Format("[OrganizationSpecific: OrganizationUniqueID={0}, OrganizationDefinedSubType={1}, OrganizationDefinedInfoString={2}]", this.OrganizationUniqueID,
                this.OrganizationDefinedSubType, this.OrganizationDefinedInfoString);
        }
        #endregion
    }
}