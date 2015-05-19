# XamlPreprocessor

À lire absolument, car cela fait office de documentation :
* [Préprocesseur Xaml, partie 1 : la syntaxe](https://netspring.wordpress.com/2013/05/15/preprocesseur-xaml-partie-1-la-syntaxe/)
* [Préprocesseur Xaml, partie 2 : utilisation](https://netspring.wordpress.com/2013/05/19/preprocesseur-xaml-partie-2-utilisation/)

## Usage

The project build an executable which can be used either:
* in command line
* as a MSBuild Task

The command line syntax is:
XamlPreprocessor.exe <SYMBOL_LIST> <input_file.xml> <output_file.xml>
SYMBOL_LIST is a list of comma separated symbols (eg. DEBUG;WINDOWS_PHONE)