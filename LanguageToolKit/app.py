"""
@author: Alexis Oblet
"""

import sys
from PyQt4 import QtGui
from src.ui.languageToolKit import LanguageToolKit

if __name__ == '__main__':
    app = QtGui.QApplication(sys.argv)
    widget = LanguageToolKit()
    widget.show()
    app.exec_()
