/// Assignment: A9
/// Team Members:
/// Rianna McIntyre, Andrew Winward, Olivia Matvejeva,
/// Trichia Crouch, Ben Pond, and Nick Jarvis.
/// Team Name: Qt Pie Programmers.
/// Github Names:
/// rianlmci, drewblue42, omatvejeva,
/// T-crouch, BPond314, jarvisnc
/// Repo Link: github.com/University-of-Utah-CS3505/a9-edu-app-f24-rianlmci
/// @brief Qt Designer Form header for the Jump Start quiz.

#ifndef JUMPSTARTQUIZFORM_H
#define JUMPSTARTQUIZFORM_H

#include <QWidget>
#include "PageEnum.h"

namespace Ui
{
    class jumpStartQuizForm;
}

class jumpStartQuizForm : public QWidget
{
    Q_OBJECT

public:
    explicit jumpStartQuizForm(QWidget *parent = nullptr);
    ~jumpStartQuizForm();

signals:
    ///@brief signal that lets the program know to change the displayed screen
    void requestSetPage(Page);

    ///@brief signal to have the jump start button on the table of contents turn green
    void requestSetJumpStartButtonGreen();

public slots:
    ///@brief slot that is activated when the table of contents button is clicked - emits a requestSetPage
    void on_tableOfContentsButton_clicked();

    ///@brief slot that is activated when the info button is clicked - emits a requestSetPage
    void on_jumpStartInfoButton_clicked();

    ///@brief slot that is activated when the submit button is clicked - submits the quiz for autograding
    void on_submitButton_clicked();

    ///@brief slot that is activated when the reset button is clicked - resets the quiz so that it can be taken again
    void on_resetButton_clicked();

private:
    Ui::jumpStartQuizForm *ui;
};

#endif // JUMPSTARTQUIZFORM_H
