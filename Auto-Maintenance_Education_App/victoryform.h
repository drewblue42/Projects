/// Assignment: A9
/// Team Members:
/// Rianna McIntyre, Andrew Winward, Olivia Matvejeva,
/// Trichia Crouch, Ben Pond, and Nick Jarvis.
/// Team Name: Qt Pie Programmers.
/// Github Names:
/// rianlmci, drewblue42, omatvejeva,
/// T-crouch, BPond314, jarvisnc
/// Repo Link: github.com/University-of-Utah-CS3505/a9-edu-app-f24-rianlmci
/// @brief Qt Designer Form header for the Victory page.

#ifndef VICTORYFORM_H
#define VICTORYFORM_H

#include <Box2D/Box2D.h>
#include <QGraphicsScene>
#include <QTimer>
#include "PageEnum.h"
#include "qwidget.h"



namespace Ui
{
    class VictoryForm;
}

///Author: QTPiProgrammers
///Assignment: A9
///This is the QWidget Form that contains the victory animation for successful quiz completion.
class VictoryForm : public QWidget
{
    Q_OBJECT

public:
    //QT explict constructor for the VictoryForm
    explicit VictoryForm(QWidget *parent = nullptr);

    //Destructor for the VictoryForm
    ~VictoryForm();

signals:
    void requestSetPage(Page);

public slots:
    //Starts the box2D simulation
    void startBox2DSimulation();

private slots:
    //Actions performed when the user clicks the table of contents button (takes them to the table)
    void on_tableOfContentsButton_clicked();

private:
    //The user interface belonging to this form.
    Ui::VictoryForm *ui;
};

#endif // VICTORYFORM_H
