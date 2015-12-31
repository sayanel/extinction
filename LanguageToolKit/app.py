"""
@author: Alexis Oblet
"""

import argparse
import sys
from PyQt4 import QtGui
from src.ui.languageToolKit import LanguageToolKit
import qdarkstyle

parser = argparse.ArgumentParser(description='ELTK - Extinction Language Tool Kit')
parser.add_argument('--dark', help='Set the app with DarkTheme', action='store_true')
args = parser.parse_args()


if __name__ == '__main__':
    app = QtGui.QApplication(sys.argv)
    if args.dark:
        app.setStyleSheet(qdarkstyle.load_stylesheet(pyside=False))
    widget = LanguageToolKit()
    widget.show()
    app.exec_()
