"""
@author: Alexis Oblet
"""


from PyQt4 import QtCore, QtGui
from src.ui.languageTreeMVC import LanguageTreeMVC
import os.path as op


class LanguageToolKit(QtGui.QWidget):
    """
    Util widget for Extinction developers.
    It makes the UI words insertion easier into the game.
    This is the bridge between developer and word insertion into Json file.
    Creates backups into LanguageTreeMVC.DIRNAME_BACKUP
    """

    KEY_PREFS_JSON_PATH = "jsonPath"

    def __init__(self):
        QtGui.QWidget.__init__(self)
        # self.showMaximized()
        self.setWindowIcon(QtGui.QIcon("img/app-icon.ico"))
        self.setWindowTitle("ELTK - Extinction Language Tool Kit")
        self.treeComponent = LanguageTreeMVC(self)

        # UI
        self.setMinimumWidth(650)

        self.jsonChooser = QtGui.QPushButton("...")
        self.jsonPath = QtGui.QLineEdit()
        self.keyInput = QtGui.QLineEdit()
        self.valueInput = QtGui.QLineEdit()
        self.updateJson = QtGui.QPushButton("Update")
        self.removeJson = QtGui.QPushButton("Remove")
        self.saveButton = QtGui.QPushButton("Save to JSON")
        self.clearBackup = QtGui.QPushButton("Clear backup")
        self.autoGeneratedMessage = QtGui.QLineEdit(self)

        self.updateJson.setDefault(True)
        self.autoGeneratedMessage.setReadOnly(True)
        self.jsonPath.setEnabled(False)

        self.jsonChooser.setToolTip("Select Json File")
        self.updateJson.setToolTip("Update to Model")
        self.removeJson.setToolTip("Remove from Model")

        # Preferences
        self.prefs = QtCore.QSettings("ExtinctionTeam", "LanguageToolKit")

        self.setupUI()
        self.setupConnections()
        self.loadFromCache()

    def loadMeta(self):
        if LanguageTreeMVC.KEY_AUTOGENERATED_MESSAGE not in self.treeComponent.jsModel:
            return

        self.autoGeneratedMessage.setText(self.treeComponent.jsModel[LanguageTreeMVC.KEY_AUTOGENERATED_MESSAGE])

    def onClickedJSON(self):
        xmlFile = QtGui.QFileDialog.getOpenFileName(self, "Select Json file", "", "*.json")
        if not xmlFile:
            return

        self.prefs.setValue(LanguageToolKit.KEY_PREFS_JSON_PATH, xmlFile)
        self.jsonPath.setText(xmlFile)
        self.treeComponent.load(xmlFile)

        self.loadMeta()

    def onItemClicked(self, item):
        # on item clicked: fill input fields
        self.keyInput.setText(item.key)
        self.valueInput.setText(item.value)

    def setupConnections(self):
        self.jsonChooser.clicked.connect(self.onClickedJSON)
        self.treeComponent.itemClicked.connect(self.onItemClicked)
        self.updateJson.clicked.connect(lambda: self.treeComponent.addToModel(self.keyInput.text(), self.valueInput.text()))
        self.saveButton.clicked.connect(lambda: self.treeComponent.saveJson(self.jsonPath.text()))
        self.removeJson.clicked.connect(lambda: self.treeComponent.removeFromModel(self.keyInput.text()))
        self.keyInput.textChanged.connect(self.treeComponent.loadTree)

    def loadFromCache(self):
        # Cache json path
        cacheJsonPath = self.prefs.value(LanguageToolKit.KEY_PREFS_JSON_PATH)
        cacheJsonPath = cacheJsonPath.toString() if cacheJsonPath else None

        if not cacheJsonPath or not op.exists(cacheJsonPath):
            return

        self.jsonPath.setText(cacheJsonPath)
        self.treeComponent.load(cacheJsonPath)
        self.loadMeta()

    def setupUI(self):
        buttonsStyle = "QPushButton:checked{ border:none }"
        self.updateJson.setStyleSheet(buttonsStyle)
        self.removeJson.setStyleSheet(buttonsStyle)
        self.saveButton.setStyleSheet(buttonsStyle)

        layoutJson = QtGui.QHBoxLayout()
        layoutJson.addWidget(self.jsonPath)
        layoutJson.addWidget(self.jsonChooser)

        form = QtGui.QFormLayout()
        form.addRow("Json Path ", layoutJson)
        form.addRow("Meta ", self.autoGeneratedMessage)
        form.addRow("Key UI ", self.keyInput)
        form.addRow("Value UI ", self.valueInput)

        paddingButton = "padding : 10px;"

        self.updateJson.setStyleSheet(paddingButton)
        self.removeJson.setStyleSheet(paddingButton)
        self.saveButton.setStyleSheet(paddingButton)

        layoutJsonButtons = QtGui.QHBoxLayout()
        layoutJsonButtons.addWidget(self.updateJson)
        layoutJsonButtons.addWidget(self.removeJson)
        # layoutJsonButtons.setAlignment(QtCore.Qt.AlignCenter)

        mainLayout = QtGui.QVBoxLayout()
        mainLayout.addLayout(form)
        mainLayout.addLayout(layoutJsonButtons)
        mainLayout.addWidget(self.saveButton)
        mainLayout.addSpacing(20)
        mainLayout.addWidget(self.treeComponent)
        mainLayout.setMargin(20)
        self.setLayout(mainLayout)
