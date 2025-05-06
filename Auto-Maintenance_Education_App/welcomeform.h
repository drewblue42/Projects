/// Assignment: A9
/// Team Members:
/// Rianna McIntyre, Andrew Winward, Olivia Matvejeva,
/// Trichia Crouch, Ben Pond, and Nick Jarvis.
/// Team Name: Qt Pie Programmers.
/// Github Names:
/// rianlmci, drewblue42, omatvejeva,
/// T-crouch, BPond314, jarvisnc
/// Repo Link: github.com/University-of-Utah-CS3505/a9-edu-app-f24-rianlmci
/// @brief Qt Designer Form header for the Welcome page.

#ifndef WELCOMEFORM_H
#define WELCOMEFORM_H

#include <QSoundEffect>
#include <QWidget>
#include "PageEnum.h"

namespace Ui
{
    class WelcomeForm;
}

class WelcomeForm : public QWidget
{
    Q_OBJECT

public:
    explicit WelcomeForm(QWidget *parent = nullptr);
    ~WelcomeForm();

signals:
    ///@brief signal that lets the program know to change the displayed screen
    void requestSetPage(Page);

public slots:
    ///@brief slot that is activated when the start button is clicked - takes the user to the main content page
    void on_engineStartButton_clicked();

private:
    Ui::WelcomeForm *ui;

    ///@brief sets up the sound that is played on startup
    QSoundEffect *introSound;

    ///@brief sets up the sound that is played on the engine button being clicked
    QSoundEffect *engineStartSound;

    ///@brief resize event for the image to resize
    void resizeEvent(QResizeEvent *event);
};

#endif // WELCOMEFORM_H
