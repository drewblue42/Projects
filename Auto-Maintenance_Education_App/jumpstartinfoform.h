/// Assignment: A9
/// Team Members:
/// Rianna McIntyre, Andrew Winward, Olivia Matvejeva,
/// Trichia Crouch, Ben Pond, and Nick Jarvis.
/// Team Name: Qt Pie Programmers.
/// Github Names:
/// rianlmci, drewblue42, omatvejeva,
/// T-crouch, BPond314, jarvisnc
/// Repo Link: github.com/University-of-Utah-CS3505/a9-edu-app-f24-rianlmci
/// @brief Qt Designer Form header for the Jump Start information.

#ifndef JUMPSTARTINFOFORM_H
#define JUMPSTARTINFOFORM_H

#include <QWidget>
#include "PageEnum.h"

namespace Ui
{
    class jumpStartInfoForm;
}

class jumpStartInfoForm : public QWidget
{
    Q_OBJECT

public:
    explicit jumpStartInfoForm(QWidget *parent = nullptr);
    ~jumpStartInfoForm();

signals:
    ///@brief signal that lets the program know to change the displayed screen
    void requestSetPage(Page);

public slots:
    ///@brief slot that is activated when the table of contents button is clicked - emits a requestSetPage
    void on_tableOfContentsButton_clicked();

    ///@brief slot that is activated when the quiz button is clicked - emits a requestSetPage
    void on_jumpStartQuizButton_clicked();

private:
    Ui::jumpStartInfoForm *ui;
};

#endif // JUMPSTARTINFOFORM_H
