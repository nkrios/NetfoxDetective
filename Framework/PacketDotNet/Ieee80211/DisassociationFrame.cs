﻿using System;
using System.Net.NetworkInformation;
using PacketDotNet.MiscUtil.Conversion;
using PacketDotNet.Utils;

namespace PacketDotNet.Ieee80211
{
    namespace Ieee80211
    {
        /// <summary>
        ///     Disassociation frame.
        /// </summary>
        public class DisassociationFrame : ManagementFrame
        {
            /// <summary>
            ///     Constructor
            /// </summary>
            /// <param name="bas">
            ///     A <see cref="ByteArraySegment" />
            /// </param>
            public DisassociationFrame(ByteArraySegment bas)
            {
                this.header = new ByteArraySegment(bas);

                this.FrameControl = new FrameControlField(this.FrameControlBytes);
                this.Duration = new DurationField(this.DurationBytes);
                this.DestinationAddress = this.GetAddress(0);
                this.SourceAddress = this.GetAddress(1);
                this.BssId = this.GetAddress(2);
                this.SequenceControl = new SequenceControlField(this.SequenceControlBytes);
                this.Reason = this.ReasonBytes;

                this.header.Length = this.FrameSize;
            }

            /// <summary>
            ///     Initializes a new instance of the <see cref="DisassociationFrame" /> class.
            /// </summary>
            /// <param name='SourceAddress'>
            ///     Source address.
            /// </param>
            /// <param name='DestinationAddress'>
            ///     Destination address.
            /// </param>
            /// <param name='BssId'>
            ///     Bss identifier (MAC Address of the Access Point).
            /// </param>
            public DisassociationFrame(PhysicalAddress SourceAddress, PhysicalAddress DestinationAddress, PhysicalAddress BssId)
            {
                this.FrameControl = new FrameControlField();
                this.Duration = new DurationField();
                this.DestinationAddress = DestinationAddress;
                this.SourceAddress = SourceAddress;
                this.BssId = BssId;
                this.SequenceControl = new SequenceControlField();

                this.FrameControl.SubType = FrameControlField.FrameSubTypes.ManagementDisassociation;
            }

            /// <summary>
            ///     Gets the size of the frame.
            /// </summary>
            /// <value>
            ///     The size of the frame.
            /// </value>
            public override int FrameSize
            {
                get
                {
                    return (MacFields.FrameControlLength + MacFields.DurationIDLength + (MacFields.AddressLength*3) + MacFields.SequenceControlLength
                            + DisassociationFields.ReasonCodeLength);
                }
            }

            /// <summary>
            ///     Gets or sets the reason for disassociation.
            /// </summary>
            /// <value>
            ///     The reason.
            /// </value>
            public ReasonCode Reason { get; set; }

            private ReasonCode ReasonBytes
            {
                get
                {
                    if(this.header.Length >= (DisassociationFields.ReasonCodePosition + DisassociationFields.ReasonCodeLength)) {
                        return (ReasonCode) EndianBitConverter.Little.ToUInt16(this.header.Bytes, this.header.Offset + DisassociationFields.ReasonCodePosition);
                    }
                    return ReasonCode.Unspecified;
                }

                set { EndianBitConverter.Little.CopyBytes((UInt16) value, this.header.Bytes, this.header.Offset + DisassociationFields.ReasonCodePosition); }
            }

            /// <summary>
            ///     Writes the current packet properties to the backing ByteArraySegment.
            /// </summary>
            public override void UpdateCalculatedValues()
            {
                if((this.header == null) || (this.header.Length > (this.header.BytesLength - this.header.Offset)) || (this.header.Length < this.FrameSize)) {
                    this.header = new ByteArraySegment(new Byte[this.FrameSize]);
                }

                this.FrameControlBytes = this.FrameControl.Field;
                this.DurationBytes = this.Duration.Field;
                this.SetAddress(0, this.DestinationAddress);
                this.SetAddress(1, this.SourceAddress);
                this.SetAddress(2, this.BssId);
                this.SequenceControlBytes = this.SequenceControl.Field;
                this.ReasonBytes = this.Reason;

                this.header.Length = this.FrameSize;
            }

            private class DisassociationFields
            {
                public static readonly int ReasonCodeLength = 2;
                public static readonly int ReasonCodePosition;
                static DisassociationFields() { ReasonCodePosition = MacFields.SequenceControlPosition + MacFields.SequenceControlLength; }
            }
        }
    }
}