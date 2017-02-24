# Miracle.Settings

Load your application settings into strong typed objects. 

The advantages are many:
* No need to write code that load each appSetting.
* No "magic strings" and no defaults scattered around in your source files.
* Your application fails immediately upon bad configuration.
* Detailed error messages describing exactly which setting is invalid. 
* You can use DI to inject strong typed settings into your application.
* Your settings are always valid, so no need to check each setting manually.
* Supports nested objects, arrays, lists and dictionaries.
* It's super simple.

Wait you might say... You can du that with ConfigurationSections! Well you can but there are many drawbacks, and you have to write a LOT of code.

## Table of content
* [Install](#Install)
* [Usage](#Usage)
* [Using default value](#using-default-value)
* [Nested object](#Nested-object)
* [Arrays, Lists & Dictionaries](#arrays-lists--dictionaries)
* [Type support](#Type-support)
* [Rules](#Rules)

Advanced topics
* [Controlling deserialization with annotations](Annotatons.md)
* [Type converters](TypeConverters.md)
* [Value providers](ValueProviders.md)
* [Load property from multiple AppSetting values](Reference.md)
* [Validating settings](Validation.md)

## Install
Available as a NuGet package: [Miracle.Settings](https://www.nuget.org/packages/Miracle.Settings/)

To install Miracle.Settings, run the following command in the Package Manager Console
```Powershell
PM> Install-Package Miracle.Settings
```
## Usage
A basic example on how to load a POCO object with settings.
```XML
<configuration>
  <appSettings>
    <add key="Foo" value="Foo string" />
    <add key="Bar" value="42" />
  </appSettings>
</configuration>
```
The setting that you wish to load from app.config or web.config.
```CSharp
public class FooBar
{
    public string Foo { get; }
    public int Bar { get; }
}
```
The POCO (Plain Old CLR Object) that settings are serialized/loaded into.
```CSharp
ISettingsLoader settingsLoader = new SettingsLoader();
// Get settings at "root" level (without a prefix) 
var settings = settingsLoader.Create<FooBar>();
```
This code loads settings of type of type FooBar into settings variable. Put this code somewhere in your application startup code.
Note! Load settings ONCE, and expose the initialized setting object to the rest of your code.

## Using default value
If no value is provided for a property, then value is taken from DefaultValueAttribute.

```XML
<configuration>
  <appSettings>
    <add key="Foo" value="Foo string" />
  </appSettings>
</configuration>
```
```CSharp
public class FooBar
{
    public string Foo { get; }
    [DefaultValue(42)]
    public int Bar { get; }
}
```
```CSharp
ISettingsLoader settingsLoader = new SettingsLoader();
// Foo is loaded from setting. 
// No value is provided in settings for Bar, but as it has a DefaultValueAttribute it gets a value of 42.
var settings = settingsLoader.Create<FooBar>();
```

## Nested object
Nested objects are supported using "dot" notation.

```XML
<configuration>
  <appSettings>
    <add key="MyPrefix.Foo" value="Foo string" />
    <add key="MyPrefix.Nested.Foo" value="Foo" />
    <add key="MyPrefix.Nested.Bar" value="42" />
  </appSettings>
</configuration>
```

```CSharp
public class FooBar
{
    public string Foo { get; }
    public Nested Nested { get; }
}

public class Nested
{
    public string Foo { get; }
    public int Bar { get; }
}
```

```CSharp
ISettingsLoader settingsLoader = new SettingsLoader();
// Get settings prefixed by "MyPrefix"
var settings = settingsLoader.Create<FooBar>("MyPrefix");
```

## Arrays, Lists & Dictionaries
Collections (Arrays, Lists & Dictionaries) can be loaded directly or as nested properties. 
Keys must be unique, so collection keys must be suffixed by something to make them unique.

```XML
<configuration>
  <appSettings>
    <add key="MyPrefix.1" value="Foo string" />
    <add key="MyPrefix.2" value="Foo" />
    <add key="MyPrefix.x" value="42" />
  </appSettings>
</configuration>
```

```CSharp
ISettingsLoader settingsLoader = new SettingsLoader();
// Get the same settings as array, list & dictionary.
// With array and list, the "key" path is lost. 
string[] settings1 = settingsLoader.CreateArray<string>("MyPrefix");
List<string> settings2 = settingsLoader.CreateList<string>("MyPrefix");
// With dictionary, the part of the key after prefix is used as dictionary key. 
// In this case this would produce keys: "1","2","x"
Dictionary<string,string> settings3 = settingsLoader.CreateDictionary<string>("MyPrefix");
```
## Type support
Settings can be loaded into all simple value types that implements IConvertible interface.
Support for additional types can be added by providing a type converter for the specific type.

Miracle.Settings has built in support for these additional types:
Type|Comment
----|-------
Enum|incl. flags enum
Guid|Any format that Guid.Parse supports.
DateTime|ISO8601 converted to local date/time
TimeSpan|Any format that TimeSpan.Parse supports.
Type|checks that type exist
Uri|check that url is valid
DirectoryInfo|check that directory exist
FileInfo|check if file exist

## Rules
When loading a strong typed object, the following rules apply:

1. A value MUST be provided for each public property with a setter, or an exception is thrown.
2. Values are attempted loaded from value providers in the order they are specified. DefaultValueAttribute is always considered last.
3. Values are converted to target type using custom "TypeConverters" with fallback to Convert.ChangeType. Add custom type converter using: AddTypeConverter.
4. The key of the setting being loaded is calcualted as: (Prefix + PropertySeparator) + Name. No PropertySeparator is applied if Prefix is null (root). Name is the name of the property, but this can be overwritten by applying a SettingAttribute with "Name" property.
5. Elements in Arrays and Lists are returned in the same order as they are returned by the value provider. The AppSettings value proveider returns values in the same order as they are specified. 


