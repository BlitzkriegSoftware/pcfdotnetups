using System;

namespace BlitzkriegSoftware.PCF.CFEnvParser.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var localPath = @".\UPS\";
            var upsParser = new UpsEnvParser(localPath);

            Assert.Equal(2, upsParser.UpsCount);

            Assert.True("IsLocal", upsParser.IsLocal);

            var names = upsParser.UpsNames;
            foreach (var k in names) System.Console.WriteLine("{0}", k);

            var upsName = names[0];

            Assert.True("UpsExists: " + upsName, upsParser.UpsExists(upsName));

            var key = "name";
            var valueExpected = "Service01";
            Assert.True(string.Format("Exists: {0}->{1}", upsName, key), upsParser.UpsKeyExists(upsName, key));

            var value = upsParser.UpsKeyGetValue(upsName, key);

            Assert.Equal(valueExpected, value);

            var fakeKey = "FakeKey";
            value = upsParser.UpsKeyGetValue(upsName, fakeKey);

            Assert.True("Empty", string.IsNullOrWhiteSpace(value));

            Environment.ExitCode = 0;

            System.Console.WriteLine("Please `cf stop PcfDotNetCoreConsole`!");

            while(true)
            {
                System.Threading.Thread.Sleep(1000);
            }
        }
    }
}
