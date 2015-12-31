# ELTK - Extinction Language Tool Kit - Dev Helper

## Why?

This helper toolkit aims to easily handle multi language inside Extinction Game for developers.

This is the bridge between developers and language data stored as json structure.


## What?

It contains:

- UI Interface (app.py): Extinction Language Tool Kit (ELTK):
  - Edit words/texts according unique Key
  - Add
  - Replace
  - Remove
  - Save to json
  - Generate backups files into ELTK-languageBackup
  
- Translation Script (translate.py): Extinction Language Tool Kit Scripting (ELTKS)
  - Translate and generate json files according given targets provided inside en.json:
  - Request Microsoft Translator API


## How?
### ELTK

You can either use it by python command line or .exe.

#### Command line
You need to install qdarkstyle with pip

```bash
cmd> python pip install qdarkstyle 
```

With dark theme:

```bash
extinction_dev/LanguageToolKit> python app.py --dark 
```

Classic theme:

```bash
extinction_dev/LanguageToolKit> python app.py 
```

#### Windows exe
To generate .exe file you need to install py2exe and run:

```bash
extinction_dev/LanguageToolKit> python exe.py py2exe
```

Dark Theme: Generate a shortcut and add at the end of target field `--dark`.


### ELTKS
Must be used very few : Limited at 2 000 000 characters per month

```bash
extinction_dev/LanguageToolKit> python translate.py -h
```

```bash
extinction_dev/LanguageToolKit> python translate.py pathToJsonFile clientID clientSecret
```