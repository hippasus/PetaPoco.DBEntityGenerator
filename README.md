# PetaPoco.DBEntityGenerator

[PetaPoco](https://github.com/CollaboratingPlatypus/PetaPoco) has stopped T4 template support since Version 6.
But there are still lots of people loves generating DB entity classes from database.
This is a tool that can help. It is ported from the PetaPoco T4 template.

## How to use

It can be installed via nuget by installing the package [PetaPoco.DBEntityGenerator](https://www.nuget.org/packages/PetaPoco.DBEntityGenerator/).

It supports both net45 and netcoreapp2.2.

By using it on netcore, you can run `dotnet ~/.nuget/packages/petapoco.dbentitygenerator/0.1.0/tools/netcoreapp2.2/PetaPoco.DBEntityGenerator.dll`.

By using it on net45, you can run `packages/petapoco.dbentitygenerator/0.1.0/tools/net45/PetaPoco.DBEntityGenerator.exe`.

Here are the parameters it supports:

```
--config                  Config file path

-p, --providerName        Database provider name, supported arguments are Npgsql, SqlServer, MySql and Oracle

-c, --connectionString    Connection string to the database

--namespace               (Default: Entities) Namespace of generated classes

--explicitColumns         (Default: true) Explicit columns

--trackModifiedColumns    (Default: true) Track modified columns

-o, --output              (Default: console) Output, valid options are console, file

--outputFile              (Default: Database.cs) Output file name

--help                    Display this help screen.

--version                 Display version information.
```

You can either provide a config file or just leave it empty and set all other parameters, but with table customizations feature absent.

You can do table customization(e.g. table ignore, entity name change, columns customization) with a config file. Sample config file can be found in source code [sample_config.json](https://github.com/hippasus/PetaPoco.DBEntityGenerator/blob/master/PetaPoco.DBEntityGenerator/sample_config.json).

