# XamlPreprocessor

## What?

A command line utility + MSBuild Task to enable conditional compilation like directives in XAML files.
Compatible Visual Studio 2010+ and Xamarin Studio.

## Why?

* To mutualise more code between Windows Phone, Windows 8 and 10 apps
* To make different versions of an application with the ease of conditional compilation directives
* Incidentally any other light XML transformations

### Illustration

The emulator (left) features two removed lines in the first section, and color attributes manipulations (injection, 
replacement and conditional injection) based on build symbols.
![alt text](https://netspring.files.wordpress.com/2013/05/img_prepro.png "Preprocessor usage illustration")

## How?

### English

Using a special syntax for comment you can conditionally remove node, add or remove attribute, or remove directives.
Boolean expression let you specify complex condition.

[Detailed syntax explanation]().

### French

À lire absolument, car cela fait office de documentation :
* [Préprocesseur Xaml, partie 1 : la syntaxe](https://netspring.wordpress.com/2013/05/15/preprocesseur-xaml-partie-1-la-syntaxe/)
* [Préprocesseur Xaml, partie 2 : utilisation](https://netspring.wordpress.com/2013/05/19/preprocesseur-xaml-partie-2-utilisation/)

## IRL

Used since May 2013 to build multiple versions of my Windows Phone [Japanese-French Dictionary](http://windowsphone.com/s?appId=d9951d66-368e-414c-89db-f76db5697f7b).

## Usage

The project build an executable which can be used either:
* in command line
* as a MSBuild Task

```
The command line syntax is:
XamlPreprocessor.exe <SYMBOL_LIST> <input_file.xml> <output_file.xml>
SYMBOL_LIST is a list of comma separated symbols (eg. DEBUG;WINDOWS_PHONE)
```

For the MSBuild integration, see the sub projects *IntegrationDemoApp*.
