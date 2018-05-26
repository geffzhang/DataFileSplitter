# DataFileSplitter
.NET Core console app to randomly split a datafile into train/test/dev files.
## Options
```
  -f, --file              Required. Input file to be processed.

  --verbose               (Default: false) Prints all messages to standard output.

  --hasheader             (Default: true) [true] if input file has header, otherwise [false]

  --includeheader         (Default: true) [true] if output file should include header, otherwise [false]

  --numericalfilenames    (Default: false) [true] if output filenames should be suffixed with the corresponding fraction, [false] if they should be suffixed with -train/-test/-dev

  --help                  Display this help screen.

  --version               Display version information.

  test (pos. 0)           (Default: 20) Fraction (percentage) of rows to allocate for TEST dataset.

  dev (pos. 0)            (Default: 0) Fraction (percentage) of rows to allocate for DEV dataset.
```
## Syntax
Running the console app with the parameters below will result in a 3-file split (train, test & dev).
```
dotnet DataFileSplitter.dll --file "c:\dev\ml\automobile price data (raw).csv" --numericalfilenames 25 5
```
## Output
```
---- DataFileSplitterSinceYoureLazy --
initializing..

---- file details --------------------
filepath: c:\dev\ml\automobile price data (raw).csv
file type: .csv
has header: True

---- fractions -----------------------
train: 70 %
test: 25 %
dev: 5 %

---- progress ------------------------
extracted header
rows: 205
randomizing rows
splitting into multiple tables

---- writing output files ------------
files will include header row
train file: 140 rows written to c:\dev\ml\automobile price data (raw)-70.csv
test file: 50 rows written to c:\dev\ml\automobile price data (raw)-25.csv
dev file: 15 rows written to c:\dev\ml\automobile price data (raw)-5.csv
```
