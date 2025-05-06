/// Assignment: A9
/// Team Members:
/// Rianna McIntyre, Andrew Winward, Olivia Matvejeva,
/// Trichia Crouch, Ben Pond, and Nick Jarvis.
/// Team Name: Qt Pie Programmers.
/// Github Names:
/// rianlmci, drewblue42, omatvejeva,
/// T-crouch, BPond314, jarvisnc
/// Repo Link: github.com/University-of-Utah-CS3505/a9-edu-app-f24-rianlmci
/// @brief Qt Designer Form header for the Tire Pressure information.

#ifndef TIREPRESSUREINFOFORM_H
#define TIREPRESSUREINFOFORM_H

#include <QWidget>
#include "PageEnum.h"

namespace Ui
{
    class TirePressureInfoForm;
}

class TirePressureInfoForm : public QWidget
{
    Q_OBJECT

public:
    explicit TirePressureInfoForm(QWidget *parent = nullptr);
    ~TirePressureInfoForm();

signals:
    ///@brief signal that lets the program know to change the displayed screen
    void requestSetPage(Page);

public slots:
    ///@brief slot that is activated when the table of contents button is clicked - emits a requestSetPage
    void on_tableOfContentsButton_clicked();

    ///@brief slot that is activated when the quiz button is clicked - emits a requestSetPage
    void on_tirePressureQuizButton_clicked();

private:
    Ui::TirePressureInfoForm *ui;
};

#endif // TIREPRESSUREINFOFORM_H
