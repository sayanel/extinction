"""
@author: Alexis Oblet
"""


from PyQt4 import QtCore, QtGui
import json
import datetime
import collections
import os.path as op
import os


class LanguageTreeMVC(QtGui.QTreeWidget):
    """
    TreeWidget listing all the words from a given JsonPath.
    """
    KEY_LANGUAGE                = "language"
    KEY_CODE                    = "code"
    KEY_ELEMENTS                = "elements"
    KEY_AUTOGENERATED_MESSAGE   = "auto-generated"
    KEY_TARGETS                 = "languageTargets"
    KEY_COMMENTS                = "comments"

    DIRNAME_BACKUP              = "ELTK-languageBackup"

    def __init__(self, parent=None):
        QtGui.QTreeWidget.__init__(self, parent)
        self.jsModel = {}
        self.setupUI()

    def load(self, jsonPath):
        # Save backup with date ;)
        self.loadJson(jsonPath)
        self.loadTree()
        self.saveJsonBackup(jsonPath)

    def saveJsonBackup(self, jsonPath):
        jsonPath = str(jsonPath)
        dirBackUp = op.join(op.dirname(jsonPath), LanguageTreeMVC.DIRNAME_BACKUP, str(datetime.date.today()))
        jsFilename, jsExt = op.splitext(op.basename(jsonPath))

        if not op.exists(dirBackUp):
            os.makedirs(dirBackUp)

        # fileBackup: file.bak-YYYY-mm-dd_hh-mm-ss.json
        backupTag = "-bak." + str(datetime.date.today()) + "_" + datetime.datetime.now().strftime("%H-%M-%S")
        fileBackup = op.join(dirBackUp, jsFilename + backupTag + jsExt)

        jsBackup = self.jsModel.copy()
        jsBackup[LanguageTreeMVC.KEY_AUTOGENERATED_MESSAGE] = LanguageTreeMVC.getAutoGeneratedMessage() + ". Backup file."

        try:
            with open(fileBackup, 'w') as out:
                json.dump(jsBackup, out, indent=4)
        except Exception as e:
            print "Error while saving backup " + str(e)

    def saveJson(self, jsonPath):
        # Add some meta data into KEY_AUTOGENERATED_MESSAGE like time saving
        if not self.jsModel:
            QtGui.QMessageBox.information(self, "Json Data Empty", "No json data loaded. Process not done")
            return

        self.jsModel[LanguageTreeMVC.KEY_AUTOGENERATED_MESSAGE] = LanguageTreeMVC.getAutoGeneratedMessage()

        try:
            with open(jsonPath, 'w') as out:
                json.dump(self.jsModel, out, indent=4)
            QtGui.QMessageBox.information(self, "Json saved", "Json data was saved into " + jsonPath)
        except Exception as e:
            QtGui.QMessageBox.critical(self, "Json saving fail", "Error while saving json: " + str(e))

    @staticmethod
    def getAutoGeneratedMessage():
        return "This file was generated by ELTK (Extinction Language Tool Kit) " \
               "at " + str(datetime.datetime.now())

    def loadJson(self, jsonPath):
        jsonPath = str(jsonPath)  # avoid QString problems
        try:
            with open(jsonPath) as jsFile:
                self.jsModel = json.load(jsFile, object_pairs_hook=collections.OrderedDict)
        except Exception as e:
            QtGui.QMessageBox.critical(self, "Wrong Json file structure", "The json file " + jsonPath + " is not correct. \nError: " + str(e))
            raise

    @QtCore.pyqtSlot(str)
    def loadTree(self, _filter=''):
        # clear tree
        self.clear()

        # fill tree
        topLevelItem = LanguageTreeMVCItem(self, self.jsModel[LanguageTreeMVC.KEY_LANGUAGE])
        self.addTopLevelItem(topLevelItem)
        self.fillTreeChildren(topLevelItem, self.jsModel[LanguageTreeMVC.KEY_ELEMENTS], _filter)
        self.expandAll()
        self.resizeColumnToContents(0)

    def fillTreeChildren(self, parentItem, component, _filter=''):
        """
        Method used to construct the tree.
        The filter process is done here.
        Constructs the tree as a real tree
        Sample: Game.Menu.Options =>
        Game
           Menu
               Options
        """

        _filter = str(_filter)


        for key, val in sorted(component.iteritems()):
            if _filter:
                # Sample: _fitler == Game.Menu.Options
                if not str(key).startswith(_filter):
                    continue

            # Constructs the tree as real tree with subkey:
            # Sample: Game.Menu.Options =>
            # Game -> Game.Menu -> Game.Menu.Options
            splitKey = key.split('.')
            finalKey = ""

            parentItemTmp = parentItem
            for idx, subkey in enumerate(splitKey):
                finalKey += subkey
                isRealFinalKey = (idx == len(splitKey) - 1)
                item = LanguageTreeMVCItem(None, finalKey, val if isRealFinalKey else '')

                # If last element: add it to parentItemTmp and break
                if isRealFinalKey:
                    parentItemTmp.addChild(item)
                    break

                # Find potential parent subkey already added
                subkeyAdded = self.findItemFromKey(parentItemTmp, finalKey)

                # If not subkey added: add to parentItemTmp and set item as parentItemTmp
                if not subkeyAdded:
                    parentItemTmp.addChild(item)
                    parentItemTmp = item
                else:
                    parentItemTmp = subkeyAdded

                finalKey += '.'

    def findItemFromKey(self, item, key, col=0):
        for i in range(0, item.childCount()):
            itemAdded = item.child(i)
            if str(itemAdded.text(col)).strip() == key.strip():
                return itemAdded
        return None



    def setupUI(self):
        self.headerItem().setText(0, "Keys")
        self.headerItem().setText(1, "Values")
        self.setColumnCount(2)

    def addToModel(self, key, val):
        """
        Add key and val into jsData.
        Dig into the jsData tree
        Key based onto: A.B.C.D = val
        """
        key = str(key).strip()
        val = str(val).strip()

        if not key or not val:
            QtGui.QMessageBox.warning(self, "Add to Model failed", "Add to model fail: Key or value Empty")
            return

        jsElements = self.jsModel[LanguageTreeMVC.KEY_ELEMENTS]
        if jsElements.get(key):
            confirmErase =  QtGui.QMessageBox.warning(self, "Warning erase value", "The Key: " + key + " will be erased", QtGui.QMessageBox.Ok, QtGui.QMessageBox.Cancel)
            if confirmErase != QtGui.QMessageBox.Ok:
                return

        jsElements[key] = val

        # Finally load tree
        self.loadTree()

    def removeFromModel(self, key):
        key = str(key).strip()

        if not key:
            QtGui.QMessageBox.warning(self, "Remove from Model failed", "Remove from model failed: Key empty")
            return

        confirmRemove = QtGui.QMessageBox.warning(self, "Remove ?", "Are you sure to remove " + key + " ?", QtGui.QMessageBox.Ok, QtGui.QMessageBox.Cancel)
        if confirmRemove != QtGui.QMessageBox.Ok:
            return

        try:
            self.jsModel[LanguageTreeMVC.KEY_ELEMENTS].pop(key)
        except:
            QtGui.QMessageBox.information(self, "Wrong Key", "The key " + key + " does not exist. Process canceled")
            return

        self.loadTree()


class LanguageTreeMVCItem(QtGui.QTreeWidgetItem):
    def __init__(self, parent=None, key="", value=""):
        QtGui.QTreeWidgetItem.__init__(self, parent)
        self.key = ""
        self.value = ""
        self.set(key, value)

    def set(self, key="", val=""):
        self.key = key
        self.value = val
        self.setText(0, key)
        self.setText(1, val)
