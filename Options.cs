using CommandLine;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataFileSplitter
{
    class Options
    {
        [Option('f', "file", Required = true, HelpText = "Input file to be processed.")]
        public string InputFile { get; set; }

        // Omitting long name, defaults to name of property, ie "--verbose"
        [Option(Default = false, HelpText = "Prints all messages to standard output.")]
        public bool Verbose { get; set; }

        [Option("hasheader", Default = true, HelpText = "[true] if input file has header, otherwise [false]")]
        public bool hasHeader { get; set; }

        [Option("includeheader", Default = true, HelpText = "[true] if output file should include header, otherwise [false]")]
        public bool includeHeader { get; set; }

        [Option("numericalfilenames", Default = false, HelpText = "[true] if output filenames should be suffixed with the corresponding fraction, [false] if they should be suffixed with -train/-test/-dev")]
        public bool numericalFilenames { get; set; }

        [Value(0, MetaName = "test", Default = 20, HelpText = "Fraction (percentage) of rows to allocate for TEST dataset.")]
        public float testFraction { get; set; }

        [Value(0, MetaName = "dev", Default = 0, HelpText = "Fraction (percentage) of rows to allocate for DEV dataset.")]
        public float devFraction { get; set; }
    }
}
