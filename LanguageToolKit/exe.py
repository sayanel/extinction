from distutils.core import setup
import py2exe

# Python to exe application building script

setup(
    version = "0.5.0",
    description = "ELTK - Extinction Language Tool Kit",
    name = "ELTK",
    data_files = [
            ('imageformats', [
              r'C:\Python27\Lib\site-packages\PyQt4\plugins\imageformats\qico4.dll'
              ])],
    # targets to build
    windows = [{"script": "app.py", "icon_resources": [(1, "img/app-icon.ico")]}],
    options = {
            "py2exe":{
                "packages" : ["gzip"],
                "includes" : ["sip"],
                "dll_excludes": ["MSVCP90.dll", "HID.DLL", "w9xpopen.exe"],
        }}
    )