﻿namespace Virgil.PureKit.Tests
{
    using Org.BouncyCastle.Asn1.Nist;
    using Org.BouncyCastle.Crypto.Digests;
    using Org.BouncyCastle.Math.EC;
    using Org.BouncyCastle.Security;

    using Virgil.PureKit.Phe;

    using Xunit;

    public class SwuTests
    {
        [Fact]
        public void HashToPoint_Should_GeneratePointOnCurve_When_RandomHashesArePassed()
        {
            var phe = new PheCrypto();
            var swu = new Swu(phe.Curve.Q, phe.Curve.B.ToBigInteger());
            var rng = new SecureRandom();
            var sha512 = new SHA512Helper();

            var random = new byte[32];

            for (int i = 0; i <= 15000; i++)
            {
                rng.NextBytes(random);
                var hash = sha512.ComputeHash(null, random);
                var (x, y) = swu.DataToPoint(hash);
                Assert.True(phe.Curve.CreatePoint(x, y).IsValid());
            }
        }

        [Fact]
        public void HashToPoint_Should_GeneratePointOnCurve_When_RandomHashesArePassed2()
        {
            var phe = new PheCrypto();
            var swu = new Swu(phe.Curve.Q, phe.Curve.B.ToBigInteger());
            var sha512 = new SHA512Helper();

            var data = new byte[]
            {
                0x80, 0x39, 0x05, 0x35, 0x49, 0x44, 0x70, 0xbe,
                0x0b, 0x29, 0x65, 0x01, 0x58, 0x6b, 0xfc, 0xd9,
                0xe1, 0x31, 0xc3, 0x9e, 0x2d, 0xec, 0xc7, 0x53,
                0xd4, 0xf2, 0x5f, 0xdd, 0xd2, 0x28, 0x1e, 0xe3,
            };

            var hash = sha512.ComputeHash(null, data);

            for (int i = 0; i <= 15000; i++)
            {
                var (x, y) = swu.DataToPoint(hash);
                Assert.True(phe.Curve.CreatePoint(x, y).IsValid());
                hash = sha512.ComputeHash(null, hash);
            }
        }

        [Fact]
        public void SwuHashToPoint_Should_ReturnExpectedPoint()
        {
            var data = new byte[]
            {
                0x02, 0x6c, 0x68, 0xba, 0x79, 0x9b, 0x95, 0x8d,
                0xa1, 0xdd, 0xec, 0x47, 0xcf, 0x77, 0xb6, 0x1a,
                0x68, 0xe3, 0x27, 0xbb, 0x16, 0xdd, 0x04, 0x6f,
                0x90, 0xfe, 0x2d, 0x7e, 0x46, 0xc7, 0x86, 0x1b,
                0xf9, 0x7a, 0xdb, 0xda, 0x15, 0xef, 0x5c, 0x13,
                0x63, 0xe7, 0x0d, 0x7c, 0xfa, 0x78, 0x24, 0xca,
                0xb9, 0x29, 0x74, 0x96, 0x09, 0x47, 0x15, 0x4d,
                0x34, 0xc4, 0x38, 0xe3, 0xeb, 0xcf, 0xfc, 0xbc,
            };
            var phe = new PheCrypto();
            var a = phe.Curve;
            var expectedX = "41644486759784367771047752285976210905566569374059610763941558650382638987514";
            var expectedY = "47123545766650584118634862924645280635136629360149764686957339607865971771956";

            // var curveParams = NistNamedCurves.GetByName("P-256");
            var swu = new Swu(phe.Curve.Q, phe.Curve.B.ToBigInteger());
            var (x, y) = swu.DataToPoint(data);
            Assert.Equal(expectedX, x.ToString());
            Assert.Equal(expectedY, y.ToString());
        }
    }
}
