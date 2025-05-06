#ifndef MAINWINDOW_H
#define MAINWINDOW_H

#include <QMainWindow>
#include <QQueue>
#include <QFileDialog>
#include "toolbarview.h"
#include "tool.h"
#include "colorsmodel.h"
#include "canvas.h"
#include "framesmodel.h"

/// Assignment: A8
/// Team Members:
/// Rianna McIntyre, Andrew Winward, Olivia Matvejeva,
/// Trichia Crouch, Ben Pond, and Nick Jarvis.
/// Team Name: Qt Pie Programmers.
/// Github Names:
/// rianlmci, drewblue42, omatvejeva,
/// T-crouch, BPond314, jarvisnc
/// Repo Link: https://github.com/University-of-Utah-CS3505/a8-sprite-editor-f24-jarvisnc
/// @brief this file sets up and holds the main functions of the project.


QT_BEGIN_NAMESPACE
namespace Ui {
    class MainWindow;
}
QT_END_NAMESPACE

class MainWindow : public QMainWindow
{
    Q_OBJECT

public:
    ///@brief constucts an instance of the main window .
    MainWindow(QWidget *parent = nullptr);
    ///@brief destructs the instance of the main window
    ~MainWindow();

signals:
    void requestResetFramesDisplay();
    void requestAllFrames();

private slots:

    ///@brief creates a new canvas with a size of 16px by 16px
    void onAction16x16Triggered();

    ///@brief creates a new canvas with a size of 32px by 32px
    void onAction32x32Triggered();

    ///@brief creates a new canvas with a size of 64px by 64px
    void onAction64x64Triggered();

private:
    ///@brief connects the UI to the main window
    Ui::MainWindow *ui;

    ///@brief connects the tool bar view to the main window
    ToolBarView toolbarView;

    ///@brief sets up the tool box model to the main window
    QMap<std::string, Tool> toolboxModel;

    ///@brief connects the colors model to the main window
    ColorsModel colorsModel;

    ///@brief sets up the connections in the main window
    /// @param instance of the colors model
    void setUpConnections(ColorsModel &colorsModel);

    ///@brief a pointer to the model object
    FramesModel* model;

    ///@brief this sets up the connections for the frame view
    void framesViewConnect();

    ///@brief this creates an instance of the canvas in the main window
    Canvas canvas;

    ///@brief this sets up the connections used by the main window
    void setUpConnections();

    /// @brief Causes the state of the model to be saved to a .json file on disk.
    void save();

    /// @brief Causes the model to be reconfigured according to data from a .json file on disk.
    void open();
};
#endif // MAINWINDOW_H
