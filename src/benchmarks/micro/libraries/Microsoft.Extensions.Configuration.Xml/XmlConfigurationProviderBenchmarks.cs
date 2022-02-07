﻿using BenchmarkDotNet.Attributes;
using MicroBenchmarks;
using System.IO;

namespace Microsoft.Extensions.Configuration.Xml
{
    [BenchmarkCategory(Categories.Libraries)]
    public class XmlConfigurationProviderBenchmarks
    {
        private MemoryStream _memoryStream;
        private XmlConfigurationProvider _provider;

        [Params("simple.xml", "deep.xml", "names.xml")]
        public string FileName { get; set; }

        [GlobalSetup]
        public void GlobalSetup()
        {
            _provider = new XmlConfigurationProvider(new XmlConfigurationSource());

            using (FileStream fileStream = File.OpenRead(Path.Combine("./libraries/Microsoft.Extensions.Configuration.Xml/TestFiles", FileName)))
            {
                _memoryStream = new MemoryStream();

                fileStream.CopyTo(_memoryStream);
            }
        }

        [GlobalCleanup]
        public void Cleanup() => _memoryStream.Dispose();

        [Benchmark]
        public void Load()
        {
            _memoryStream.Position = 0;

            _provider.Load(_memoryStream);
        }
    }
}