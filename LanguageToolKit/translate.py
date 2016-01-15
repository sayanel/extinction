"""
@author: Alexis Oblet

Dev usage: LanguageToolKit$ python translate.py pathToJson/en.json clientID clientSecret
"""

import argparse

parser = argparse.ArgumentParser(description='ELTKS - Extinction Language Tool Kit Scripting. Translate English to other languages ')

parser.add_argument('json_path',    help='Json path pointing to the reference language file', type=str)
parser.add_argument('client_id',    help='Cliend id used for Microsoft Translator API', type=str)
parser.add_argument('client_secret',help='Client secret used for Microsoft Translator API', type=str)
args = parser.parse_args()

# Scripting process
import src.translator.api as translatorAPI

token = translatorAPI.getAccessToken(args.client_id, args.client_secret)
translatorAPI.translate(args.json_path, token)
