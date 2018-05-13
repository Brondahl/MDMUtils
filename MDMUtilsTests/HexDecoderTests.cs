using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using MDMUtils;
using NUnit.Framework;

namespace MDMUtilsTests
{
    [TestFixture]
    public class HexDecoderTests
    {
        [Test,
         TestCase("0x00", new byte[] { 0 }),
         TestCase("0x00010203", new byte[] { 0, 1, 2, 3 }),
         TestCase("0x0A", new byte[] { 10 }),
         TestCase("0x3E", new byte[] { 62 })]
        public void ParsesValidStringsCorrectly(string hexString, byte[] bytes)
        {
            byte[] parsedOutput = HexDecoder.ParseHexString(hexString);
            parsedOutput.Should().Equal(bytes);
        }

        [Test]
        public void InvalidStringsGiveAppropriateErrors_NoStarterCharacter()
        {
            try
            {
                HexDecoder.ParseHexString("00");
                Assert.Fail("Did not throw an exception, but should have done so.");
            }
            catch (FormatException e)
            {
                e.Message.Should().Contain(@"Must start with ""0x""");
            }
        }

        [Test]
        public void InvalidStringsGiveAppropriateErrors_NonHexCharacters()
        {
            try
            {
                HexDecoder.ParseHexString("0x0s");
                Assert.Fail("Did not throw an exception, but should have done so.");
            }
            catch (FormatException e)
            {
                e.Message.Should().Contain(@"is not a valid Hex character");
            }
        }

        [Test,
         TestCase("0x1"),
         TestCase("0x01104"),]
        public void InvalidStringsGiveAppropriateErrors_OddNumberOfCharacters(string hexString)
        {
            try
            {
                HexDecoder.ParseHexString(hexString);
                Assert.Fail("Did not throw an exception, but should have done so.");
            }
            catch (ArgumentException e)
            {
                e.Message.Should().Contain(@"Must contain an even number of characters.");
            }
        }

    }
}
