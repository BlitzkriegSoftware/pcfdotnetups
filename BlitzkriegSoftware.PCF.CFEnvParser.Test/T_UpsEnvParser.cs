using System;
using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace BlitzkriegSoftware.PCF.CFEnvParser.Test
{
    public class T_UpsEnvParser
    {
        #region "Boiler Plate"

        private readonly ITestOutputHelper output;

        public T_UpsEnvParser(ITestOutputHelper output)
        {
            this.output = output;
        }

        #endregion

        [Fact]
        [Trait("Category", "Unit")]
        [Trait("Module", "UpsEnvParser")]
        public void UpsEnvParser_1()
        {
            var upsFolder = Helpers.TestHelper.UPS_Folder();

            var upsParser = new UpsEnvParser(upsFolder);

            Assert.Equal(2, upsParser.UpsCount);

            Assert.True(upsParser.IsLocal);

            var names = upsParser.UpsNames;
            foreach (var k in names) this.output.WriteLine("{0}", k);

            var upsName = names[0];

            Assert.True(upsParser.UpsExists(upsName));

            var key = "name";
            var valueExpected = "Service01";
            Assert.True(upsParser.UpsKeyExists(upsName, key));

            var value = upsParser.UpsKeyGetValue(upsName, key);

            Assert.Equal(valueExpected, value);

            var fakeKey = "FakeKey";
            value = upsParser.UpsKeyGetValue(upsName, fakeKey);

            Assert.True(string.IsNullOrWhiteSpace(value));
        }

        [Fact]
        [Trait("Category", "Unit")]
        [Trait("Module", "UpsEnvParser")]
        public void UpsEnvParser_2()
        {
            var upsFolder = Helpers.TestHelper.UPS_Folder();

            var vcap = File.ReadAllText("vcap.json");
            Environment.SetEnvironmentVariable(UpsEnvParser.ENV_VAR, vcap);

            var upsParser = new UpsEnvParser(upsFolder);

            Assert.Equal(2, upsParser.UpsCount);

            Assert.False(upsParser.IsLocal);

            var names = upsParser.UpsNames;
            foreach (var k in names) this.output.WriteLine("{0}", k);

            var upsName = names[0];

            Assert.True(upsParser.UpsExists(upsName));

            var key = "name";
            var valueExpected = "Service01";
            Assert.True(upsParser.UpsKeyExists(upsName, key));

            var value = upsParser.UpsKeyGetValue(upsName, key);

            Assert.Equal(valueExpected, value);

            var keys = upsParser.UpsKeys(upsName);
            foreach (var k in keys) this.output.WriteLine("{0}", k);

            var d = upsParser.UpsKeyValues(upsName);
            foreach(var item in d)
            {
                this.output.WriteLine("{0}: {1}", item.Key, item.Value);
            }

            var fakeKey = "FakeKey";
            value = upsParser.UpsKeyGetValue(upsName, fakeKey);

            Assert.True(string.IsNullOrWhiteSpace(value));
        }


    }
}
