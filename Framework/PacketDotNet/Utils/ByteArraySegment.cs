using System;
using System.Linq;
using System.Reflection;
using log4net;

namespace PacketDotNet.Utils
{
    /// <summary>
    ///     Container class that refers to a segment of bytes in a byte[]
    ///     Used to ensure high performance by allowing memory copies to
    ///     be avoided
    /// </summary>
    public class ByteArraySegment
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

        private int length;

        /// <value>
        ///     The byte[] array
        /// </value>
        public byte[] Bytes { get; }

        /// <value>
        ///     The maximum number of bytes we should treat Bytes as having, allows
        ///     for controling the number of bytes produced by EncapsulatedBytes()
        /// </value>
        public int BytesLength { get; }

        /// <value>
        ///     Number of bytes beyond the offset into Bytes
        ///     Take care when setting this parameter as many things are based on
        ///     the value of this property being correct
        /// </value>
        public int Length
        {
            get { return this.length; }
            set
            {
                // check for invalid values
                if(value < 0) { throw new InvalidOperationException("attempting to set a negative length of " + value); }

                this.length = value;
                log.DebugFormat("Length: {0}", value);
            }
        }

        /// <value>
        ///     Offset into Bytes
        /// </value>
        public int Offset { get; }

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="Bytes">
        ///     A <see cref="System.Byte[]" />
        /// </param>
        public ByteArraySegment(byte[] Bytes) : this(Bytes, 0, Bytes.Length) {}

        /// <summary>
        ///     Constructor from a byte array, offset into the byte array and
        ///     a length beyond that offset of the bytes this class is referencing
        /// </summary>
        /// <param name="Bytes">
        ///     A <see cref="System.Byte" />
        /// </param>
        /// <param name="Offset">
        ///     A <see cref="System.Int32" />
        /// </param>
        /// <param name="Length">
        ///     A <see cref="System.Int32" />
        /// </param>
        public ByteArraySegment(byte[] Bytes, int Offset, int Length) : this(Bytes, Offset, Length, Bytes.Length) {}

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="Bytes">
        ///     A <see cref="System.Byte[]" />
        /// </param>
        /// <param name="Offset">
        ///     A <see cref="System.Int32" />
        /// </param>
        /// <param name="Length">
        ///     A <see cref="System.Int32" />
        /// </param>
        /// <param name="BytesLength">
        ///     A <see cref="System.Int32" />
        /// </param>
        public ByteArraySegment(byte[] Bytes, int Offset, int Length, int BytesLength)
        {
            log.DebugFormat("Bytes.Length {0}, Offset {1}, Length {2}, BytesLength {3}", Bytes.Length, Offset, Length, BytesLength);

            this.Bytes = Bytes;
            this.Offset = Offset;
            this.Length = Length;
            this.BytesLength = Math.Min(BytesLength, Bytes.Length);
        }

        /// <summary>
        ///     Copy constructor
        /// </summary>
        /// <param name="original">
        ///     A <see cref="ByteArraySegment" />
        /// </param>
        public ByteArraySegment(ByteArraySegment original)
        {
            this.Bytes = original.Bytes;
            this.Offset = original.Offset;
            this.Length = original.Length;
            this.BytesLength = original.BytesLength;
        }

        /// <summary>
        ///     Returns a contiguous byte[] from this container, if necessary, by copying
        ///     the bytes from the current offset into a newly allocated byte[].
        ///     NeedsCopyForActualBytes can be used to determine if the copy is necessary
        /// </summary>
        /// <returns>
        ///     A <see cref="System.Byte" />
        /// </returns>
        public byte[] ActualBytes()
        {
            log.DebugFormat("{0}", this);

            if(this.NeedsCopyForActualBytes)
            {
                log.Debug("needs copy");
                var newBytes = new byte[this.Length];
                try {
                    Array.Copy(this.Bytes, this.Offset, newBytes, 0, this.Length);
                }
                catch(Exception)
                {
                    var len = this.Bytes.Count() - this.Offset;
                    newBytes = new byte[len];
                    Array.Copy(this.Bytes, this.Offset, newBytes, 0, len);
                }
                return newBytes;
            }
            log.Debug("does not need copy");
            return this.Bytes;
        }

        /// <summary>
        ///     Return true if we need to perform a copy to get
        ///     the bytes represented by this class
        /// </summary>
        /// <returns>
        ///     A <see cref="System.Boolean" />
        /// </returns>
        public bool NeedsCopyForActualBytes
        {
            get
            {
                // we need a copy unless we are at the start of the byte[]
                // and the length is the total byte[] length
                var okWithoutCopy = ((this.Offset == 0) && (this.Length == this.Bytes.Length));
                var retval = !okWithoutCopy;

                log.DebugFormat("retval {0}", retval);

                return retval;
            }
        }

        /// <summary>
        ///     Helper method that returns the segment immediately following
        ///     this instance, useful for processing where the parent
        ///     wants to pass the next segment to a sub class for processing
        /// </summary>
        /// <returns>
        ///     A <see cref="ByteArraySegment" />
        /// </returns>
        public ByteArraySegment EncapsulatedBytes()
        {
            var numberOfBytesAfterThisSegment = this.BytesLength - (this.Offset + this.Length);
            return this.EncapsulatedBytes(numberOfBytesAfterThisSegment);
        }

        /// <summary>
        ///     Create the segment after the current one
        /// </summary>
        /// <param name="NewSegmentLength">
        ///     A <see cref="System.Int32" /> that can be used to limit the segment length
        ///     of the ByteArraySegment that is to be returned. Often used to exclude trailing bytes.
        /// </param>
        /// <returns>
        ///     A <see cref="ByteArraySegment" />
        /// </returns>
        public ByteArraySegment EncapsulatedBytes(int NewSegmentLength)
        {
            log.DebugFormat("NewSegmentLength {0}", NewSegmentLength);

            var startingOffset = this.Offset + this.Length; // start at the end of the current segment
            log.DebugFormat("startingOffset({0}) = Offset({1}) + Length({2})", startingOffset, this.Offset, this.Length);

            // ensure that the new segment length isn't longer than the number of bytes
            // available after the current segment
            NewSegmentLength = Math.Min(NewSegmentLength, this.BytesLength - startingOffset);

            // calculate the ByteLength property of the new ByteArraySegment
            var NewByteLength = startingOffset + NewSegmentLength;

            log.DebugFormat("NewSegmentLength {0}, NewByteLength {1}, BytesLength {2}", NewSegmentLength, NewByteLength, this.BytesLength);

            return new ByteArraySegment(this.Bytes, startingOffset, NewSegmentLength, NewByteLength);
        }

        /// <summary>
        ///     Format the class information as a string
        /// </summary>
        /// <returns>
        ///     A <see cref="System.String" />
        /// </returns>
        public override string ToString()
        {
            return string.Format("[ByteArraySegment: Length={0}, Bytes.Length={1}, BytesLength={2}, Offset={3}, NeedsCopyForActualBytes={4}]", this.Length, this.Bytes.Length,
                this.BytesLength, this.Offset, this.NeedsCopyForActualBytes);
        }
    }
}