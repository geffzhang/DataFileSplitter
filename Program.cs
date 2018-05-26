using CommandLine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DataFileSplitter
{
    class Program
    {

        static async Task Main(string[] args)
        {
            CommandLine.Parser.Default.ParseArguments<Options>(args)
                .WithParsed<Options>(opts => RunOptionsAndReturnExitCode(opts))
                .WithNotParsed<Options>((errs) => HandleParseError(errs));
        }

        private static void HandleParseError(IEnumerable<Error> errs)
        {
            foreach (var err in errs)
            {
                Console.WriteLine(err.ToString());
            }
        }

        private static void RunOptionsAndReturnExitCode(Options opts)
        {
            Console.WriteLine("---- DataFileSplitterSinceYoureLazy --");
            Console.WriteLine("initializing..");
            Console.WriteLine("");
            float trainFraction = 100 - (opts.testFraction + opts.devFraction);

            if ((trainFraction + opts.testFraction + opts.devFraction) != 100)
            {
                throw new ArgumentOutOfRangeException("all fractions (together with training fraction) must end up totaling 100");
            }

            if (!File.Exists(opts.InputFile))
            {
                throw new FileNotFoundException("the specified file wasn't found.", opts.InputFile);
            }

            var fi = new FileInfo(opts.InputFile);
            var filetype = fi.Extension;
        
            var lines = new ArrayList(File.ReadAllLines(opts.InputFile));

            Console.WriteLine("---- file details --------------------");
            Console.WriteLine($"filepath: {opts.InputFile}");
            Console.WriteLine($"file type: {filetype}");
            Console.WriteLine($"has header: {opts.hasHeader}");
            Console.WriteLine("");
            Console.WriteLine("---- fractions -----------------------");
            Console.WriteLine($"train: {(trainFraction / 100):P0}");
            Console.WriteLine($"test: {(opts.testFraction / 100):P0}");
            Console.WriteLine($"dev: {(opts.devFraction / 100):P0}");
            Console.WriteLine("");
            Console.WriteLine("---- progress ------------------------");
            var header = string.Empty;
            if (opts.hasHeader)
            {
                Console.WriteLine("extracted header");
                header = lines[0] as string;
                lines.RemoveAt(0);

            }

            var dt = new DataTable();
            dt.Columns.Add("content");
            dt.Columns.Add("rnd");
            foreach (var line in lines)
            {
                dt.Rows.Add(line, new Random().Next());
            }

            Console.WriteLine($"rows: {dt.Rows.Count}");

            Console.WriteLine("randomizing rows");
            var dv = dt.DefaultView;
            dv.Sort = "rnd desc";
            var sorted = dv.ToTable();

            Console.WriteLine("splitting into multiple tables");
            var tables = sorted.Select().Split((int)opts.testFraction, (int)opts.devFraction).ToList();

            var trainFilePath = string.Empty;
            var testFilePath = string.Empty;
            var devFilePath = string.Empty;
            if (opts.numericalFilenames)
            {
                trainFilePath = opts.InputFile.Replace(filetype, $"-{100-(opts.testFraction+opts.devFraction)}{filetype}");
                testFilePath = opts.InputFile.Replace(filetype, $"-{opts.testFraction}{filetype}");
                devFilePath = opts.InputFile.Replace(filetype, $"-{opts.devFraction}{filetype}");
            }
            else
            {
                trainFilePath = opts.InputFile.Replace(filetype, $"-train{filetype}");
                testFilePath = opts.InputFile.Replace(filetype, $"-test{filetype}");
                devFilePath = opts.InputFile.Replace(filetype, $"-dev{filetype}");
            }

            ArrayList outputLines = new ArrayList();
            Console.WriteLine("");
            Console.WriteLine("---- writing output files ------------");
            if (opts.includeHeader)
            {
                Console.WriteLine("files will include header row");
                outputLines.Add(header);
            }
            Console.WriteLine($"train file: {tables[0].Count()} rows written to {trainFilePath}");
            outputLines.AddRange(tables[0].Select(row => row["content"] as string).ToList());
            string[] writeLines = (string[])outputLines.ToArray(typeof(string));
            File.WriteAllLines(trainFilePath, writeLines);

            if (opts.testFraction > 0)
            {
                Console.WriteLine($"test file: {tables[1].Count()} rows written to {testFilePath}");
                outputLines = new ArrayList();
                if (opts.includeHeader)
                {
                    outputLines.Add(header);
                }
                outputLines.AddRange(tables[1].Select(row => row["content"] as string).ToList());
                writeLines = (string[])outputLines.ToArray(typeof(string));
                File.WriteAllLines(testFilePath, writeLines);
            }

            if (opts.devFraction > 0)
            {
                Console.WriteLine($"dev file: {tables[2].Count()} rows written to {devFilePath}");
                outputLines = new ArrayList();
                if (opts.includeHeader)
                {
                    outputLines.Add(header);
                }
                outputLines.AddRange(tables[2].Select(row => row["content"] as string).ToList());
                writeLines = (string[])outputLines.ToArray(typeof(string));
                File.WriteAllLines(devFilePath, writeLines);
            }
        }
    }
}
