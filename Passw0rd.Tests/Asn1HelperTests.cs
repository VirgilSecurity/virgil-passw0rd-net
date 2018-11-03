﻿namespace Passw0rd.Tests
{
    using System.Linq;
    using Passw0rd.Utils;
    using Xunit;

    public class Asn1HelperTests
    {
        [Fact]
        public void Should_EncodeToAsn1_When_TwoByteArraysArePassed()
        {
            var resultBase64 = "MEQEIF9FaIoBlwvyV1HuIYw1cEL0GF6TyjJqYpO/b/uzsg88BCB0Cx2dnG8QKFyHr/nTOjQr7qeWgrM7T9CAg0D8p+EvVQ==";

            var arr1 = Bytes.FromString("5F45688A01970BF25751EE218C357042F4185E93CA326A6293BF6FFBB3B20F3C", StringEncoding.HEX);
            var arr2 = Bytes.FromString("740B1D9D9C6F10285C87AFF9D33A342BEEA79682B33B4FD0808340FCA7E12F55", StringEncoding.HEX);

            var encodedBytes = Asn1Helper.Encode(arr1, arr2);
            var encodedBase64 = Bytes.ToString(encodedBytes, StringEncoding.BASE64);

            Assert.Equal(resultBase64, encodedBase64);
        }

        [Fact]
        public void Should_DecodeFromAsn1_When_ByteArrayIsPassed()
        {
            var asn1Bytes = Bytes.FromString("MEQEIF9FaIoBlwvyV1HuIYw1cEL0GF6TyjJqYpO/b/uzsg88BCB0Cx2dnG8QKFyHr/nTOjQr7qeWgrM7T9CAg0D8p+EvVQ==", StringEncoding.BASE64);

            var decodedByteArrays = Asn1Helper.Decode(asn1Bytes);

            Assert.Equal(2, decodedByteArrays.Count());

            var arr1Hex = "5F45688A01970BF25751EE218C357042F4185E93CA326A6293BF6FFBB3B20F3C";
            var arr2Hex = "740B1D9D9C6F10285C87AFF9D33A342BEEA79682B33B4FD0808340FCA7E12F55";

            Assert.Equal(Bytes.ToString(decodedByteArrays.ElementAt(0), StringEncoding.HEX).ToUpper(), arr1Hex);
            Assert.Equal(Bytes.ToString(decodedByteArrays.ElementAt(1), StringEncoding.HEX).ToUpper(), arr2Hex);
        }
    }
}