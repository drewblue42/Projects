/// Assignment: A9
/// Team Members:
/// Rianna McIntyre, Andrew Winward, Olivia Matvejeva,
/// Trichia Crouch, Ben Pond, and Nick Jarvis.
/// Team Name: Qt Pie Programmers.
/// Github Names:
/// rianlmci, drewblue42, omatvejeva,
/// T-crouch, BPond314, jarvisnc
/// Repo Link: github.com/University-of-Utah-CS3505/a9-edu-app-f24-rianlmci
/// @brief Qt Designer Form header for the Tire Replacement/Spare Tire quiz.

#ifndef SPARETIREQUIZFORM_H
#define SPARETIREQUIZFORM_H

#include <QWidget>
#include "PageEnum.h"

namespace Ui
{
    class spareTireQuizForm;
}

class spareTireQuizForm : public QWidget
{
    Q_OBJECT

public:
    explicit spareTireQuizForm(QWidget *parent = nullptr);
    ~spareTireQuizForm();

signals:

    ///@brief signal that lets the program know to change the displayed screen
    void requestSetPage(Page);

    ///@brief signal to have the spare tire button on the table of contents turn green
    void requestSetSpareTireButtonGreen();

public slots:

    ///@brief slot that is activated when the table of contents button is clicked - emits a requestSetPage
    void on_tableOfContentsButton_clicked();

    ///@brief slot that is activated when the info button is clicked - emits a requestSetPage
    void on_spareTireInfoButton_clicked();

private slots:

    ///@brief slot that is activated when the submit button is clicked - submits the quiz for autograding
    void on_submitButton_clicked();

    ///@brief slot that is activated when the reset button is clicked - resets the quiz so that it can be taken again
    void on_resetButton_clicked();

private:
    Ui::spareTireQuizForm *ui;
};

#endif // SPARETIREQUIZFORM_H
