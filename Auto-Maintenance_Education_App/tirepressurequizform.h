/// Assignment: A9 "Auto ABC's"
/// Team Members:
/// Rianna McIntyre, Andrew Winward, Olivia Matvejeva,
/// Trichia Crouch, Ben Pond, and Nick Jarvis.
/// Team Name: Qt Pie Programmers.
/// Github Names:
/// rianlmci, drewblue42, omatvejeva,
/// T-crouch, BPond314, jarvisnc
/// Repo Link: github.com/University-of-Utah-CS3505/a9-edu-app-f24-rianlmci
/// @brief Qt Designer Form header for the Tire Pressure quiz.

#ifndef TIREPRESSUREQUIZFORM_H
#define TIREPRESSUREQUIZFORM_H

#include <QWidget>
#include "PageEnum.h"

namespace Ui
{
    class TirePressureQuizForm;
}

class TirePressureQuizForm : public QWidget
{
    Q_OBJECT

public:
    explicit TirePressureQuizForm(QWidget *parent = nullptr);
    ~TirePressureQuizForm();

signals:
    ///@brief signal that lets the program know to change the displayed screen
    void requestSetPage(Page);

    ///@brief signal to have the tire pressure button on the table of contents turn green
    void requestSetTirePressureButtonGreen();

public slots:
    ///@brief slot that is activated when the table of contents button is clicked - emits a requestSetPage
    void on_tableOfContentsButton_clicked();

    ///@brief slot that is activated when the info button is clicked - emits a requestSetPage
    void on_tirePressureInfoButton_clicked();

    ///@brief slot that is activated when the submit button is clicked - submits the quiz for autograding
    void on_submitButton_clicked();

    ///@brief slot that is activated when the reset button is clicked - resets the quiz so that it can be taken again
    void on_resetButton_clicked();

private:
    Ui::TirePressureQuizForm *ui;
};

#endif // TIREPRESSUREQUIZFORM_H
