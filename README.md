# PCF .NET User Provided Services #

Using UPS in .NET (core).

## Overview ## 

There is lots of good library code for dealing with Cloud-Foundry (CF) environment variables, but not all of them work as expected and many do not provide support for UPS values while debugging in Visual Studio e.g. locally.

This is intended to be a simple, and easy to use library that is fairly easy to use.

## Code Description ##

The project has one core library that does the work:

* `BlitzkriegSoftware.PCF.CFEnvParser` which has a key class of `UpsEnvParser` that does the parsing. 

The project has some ways you can explore how it works:
* An XUnit test project that has 100% coverage
* A console app
* A WebAPI app (navigate to `/swagger` to play with the API)

> Hint: Swashbuckle is a great library for documenting WebAPI, see: <a href="https://github.com/domaindrivendev/Swashbuckle.AspNetCore" target="_blank">https://github.com/domaindrivendev/Swashbuckle.AspNetCore</a>

## API Documentation ##

The project also has a nice demo of the Sandcastle help file builder, which is a great way to generate documentation for your APIs.

> See: <a href="https://github.com/EWSoftware/SHFB" target="_blank">https://github.com/EWSoftware/SHFB</a>

A sample CHM file is included. *(You may have to right-mouse, properties, and UNBLOCK to be able to open the file).*

API Documentation: <a href="file:Sandcastle\Help\BlitzkriegSoftware.PCF.CFEnvParser.chm" target="_blank">chm</a>

## Example ##

In the console app, there is a sample of how to new up the parser. Not that *new* does not cause the UPS to be parsed, but accessing any of the methods does.

```csharp
var localPath = @".\UPS\";
var upsParser = new UpsEnvParser(localPath);

// UPS information loaded here to satisfy the request
var valueIWant = upsParser.UpsKeyGetValue(upsName, key);
```

This being the case, you may want to consider passing an instance to code that needs it rather than new it up.

### Where did my UPS data come from? ###

To see if the values came from JSON files (local) or the CF UPS you can:

```csharp
// True means that it read the files, not the UPS!
// If you app is running in CF this is BAD.
// Consider using .cfignore to avoid pushing UPS files to CF
var isFromJsonFiles = upsParser.IsLocal;
```

## About me ##

**Stuart Williams**

* Cloud/DevOps Practice Lead and National Markets Consultant
* <a href="http://magenic.com" target="_blank">Magenic Technologies</a>
* <a href="mailto:stuartw@magenic.com" target="_blank">stuartw@magenic.com</a> (e-mail work)
* <a href="mailto:spookdejur@hotmail.com" target="_blank">spookdejur@hotmail.com</a> (e-mail personal)
* Blog: <a href="http://blitzkriegsoftware.net/Blog" target="_blank">http://blitzkriegsoftware.net/Blog</a>
* LinkedIn: <a href="http://lnkd.in/P35kVT" target="_blank">http://lnkd.in/P35kVT</a>
* YouTube: <a href="https://www.youtube.com/channel/UCO88zFRJMTrAZZbYzhvAlMg" target="_blank">https://www.youtube.com/channel/UCO88zFRJMTrAZZbYzhvAlMg</a> 
* Github: <a href="https://github.com/BlitzkriegSoftware/" target="_blank">https://github.com/BlitzkriegSoftware/</a>
