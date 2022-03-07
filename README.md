# SpreadCommander

**Spread Commander** is an application for office-style data analysis that uses *PowerShell* script for data manipulation.

**SpreadCommander** contains advanced *PowerShell console* with output into *Book* (rich text editor), *Spreadsheet*, powerfull *DataGrid* and ability to generate *Charts* and *Pivot tables*. Combination with standalone *Book*, *Spreadsheet*, *SQL*, *Chart*, *Pivot*, *Dashboard* controls makes **SpreadCommander** a powerfull *data analysis* application for work with various kind of data.

Combination of *PowerShell* script and advanced *data visualization* controls makes work with data very comfortable.

Building solutions requires Ultimate license of [DevExpress](https://www.devexpress.com) controls.

Program site - <https://www.spreadcommander.net>, <https://www.spreadcommander.com>.

## PRIVACY STATEMENT

SpreadCommander does not collect private data. However MicrosoftTM does collect telemetry from executing PowerShell scripts.

Map cmdlets use Bing and OpenStreetMap and send requests to corresponding web services.

## UPDATES

### Version 2.2
- Update F# engine and built-in components.
- Fix some GDI leaks in syntax editor.
- Third-party components are updated.

### Version 2.1.1
- Fix error when disposing F# engine.
- Third-party components are updated.

### Version 2.1
- F# is enhanced.
- Third-party components updated.
- Minor updates and fixes.

### Version 2.0
- F# is added as scripting language.
- '\~\\' as project root is changed to '\~#\\'.
- Support for PowerShell is enhanced - modules in folder 'Modules' are referenced automatically. PowerShell is updated to version 7.2.
- Third-party components are updated.

### Version 1.3
- Third-party components updated.
- Enhanced and fixed import/export of text files and output to databases.
- Minor updates and fixes.

### Version 1.2
- Third-party components updated.
- Minor updates and fixes.

### Version 1.1
- First release of **SpreadCommander**.
- *Sankey diagram* is added, 2 new cmdlets - Write-SankeyDiagram and Save-SankeyDiagram.
- New Charts in *SpreadSheet*.
- New function NewID() to generate GUID in *Spreadsheet* and *SQLite*.
- Fixed export to database from *Spreadsheet*.
- Third-party components updated.
- Minor updates and fixes.

### Version 0.9.5-beta
- SpreadCommander's spreadsheet functions are updated to return empty string instead of empty value (later one is displayed as zero).
- Third-party components updated.
- Minor updates and fixes.

### Version 0.9.4-beta
- Third-party components updated.

### Version 0.9.3-beta
- Hash functions added to *Spreadsheet*, *SQLite* and *Grid*.
- Drag/drop from *Spreadsheet* to other applications is added, with Shift+Ctrl+Alt+<drag range>,
and drag/drop from other applications to *Spreadsheet*.
- Cmdlet Invoke-GenericMethod.
- Third-party components updated.
- Minor updates and fixes.

### Version 0.9.2-beta
- Program and setup are now signed.
- Third-party components updated.

### Version 0.9.1-beta
- Minor updates and fixes.
- Third-party components updated.

### Version 0.9-beta
- **SpreadCommander** now uses standalone .Net framework instead of self-contained .Net framework.
- Third-party components (*.Net*, *PowerShell*, *DevExpress* etc) are updated to new versions. Many of them are updates to new major versions.
- In projects folder "bin" is configured for probing referenced *.Net* (.Net Standard, .Net Core, .Net 5) assemblies and unmanaged dlls.
- New cmdlets *Invoke-AsyncCommands*, *New-SCRunspace*, *New-SCRunspacePool* for asynchronous tasks. Unlike other PowerShell cmdlets 
these cmdlets initialize Runspace and RunspacePool to be able to use other **SpreadCommander** cmdlets. See new example *Async* for details.
- *Book* can now load PDF files; in this case text is extracted from PDF *without* formatting.
- Cmdlets to import/export text and csv files are enhanced.
- Multiple minor fixes and enhancements.

### Version 0.8.5-beta
- Updated components to convert data, now it is possible to specify which columns to use.
- Updated third-party components.
- Minor fixes and enhancements.

Next version will be based on .Net 5 and PowerShell 7.1.

### Version 0.8-beta
- Third-party components (*DevExpress*, *SQLite* etc) are updated to new versions.
- Added import-export to-from text files, both delimited and fixed-length, and DBF files. New cmdlets are *Import-DelimitedText*, *Export-DelimitedText*, *Import-FixedLengthText*, *Export-FixedLengthText*, *Import-DBF*, *Export-DBF*. Look sample *SQL* for examples.
- Added *foonotes* and *endnotes* to output into **Book**.
- Minor fixes and enhancements.

### Version 0.7.1-beta
- SQL document: processing tag #connection is fixed;
- Third-party components (*PowerShell*, *DevExpress*) are updated to new versions.