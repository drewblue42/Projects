/// Assignment: A9
/// Team Members:
/// Rianna McIntyre, Andrew Winward, Olivia Matvejeva,
/// Trichia Crouch, Ben Pond, and Nick Jarvis.
/// Team Name: Qt Pie Programmers.
/// Github Names:
/// rianlmci, drewblue42, omatvejeva,
/// T-crouch, BPond314, jarvisnc
/// Repo Link: github.com/University-of-Utah-CS3505/a9-edu-app-f24-rianlmci
/// @brief Qt Designer Form method and slot implementations for the Welcome screen.

#include <QLabel>
#include <QPixmap>
#include <QSoundEffect>
#include <QTimer>
#include "PageEnum.h"
#include "ui_welcomeform.h"
#include "welcomeform.h"

WelcomeForm::WelcomeForm(QWidget *parent)
    : QWidget(parent)
    , ui(new Ui::WelcomeForm)
    , introSound(new QSoundEffect(this))
    , engineStartSound(new QSoundEffect(this))
{
    ui->setupUi(this);

    QPixmap pixmap(":/images/WelcomeScreen2.0.png");
    ui->welcomeImage->setPixmap(pixmap.scaled(ui->welcomeImage->size(), Qt::KeepAspectRatio, Qt::SmoothTransformation));
    ui->welcomeImage->setScaledContents(true);

    introSound->setSource(QUrl::fromLocalFile(
        ":/sound/Kevin James Daytona 2007.-[AudioTrimmer.com]-[AudioTrimmer.com].wav"));
    introSound->setVolume(0.2);

    introSound->play();

    engineStartSound->setSource(
        QUrl::fromLocalFile(":/sound/479351__grasopt__car-engine-starting.wav"));
    engineStartSound->setVolume(0.5);
}

WelcomeForm::~WelcomeForm()
{
    delete ui;
}

void WelcomeForm::on_engineStartButton_clicked()
{
    engineStartSound->play();
    QTimer::singleShot(3500, this, [this]() { emit requestSetPage(Page::TABLE_OF_CONTENTS); });
}

void WelcomeForm::resizeEvent(QResizeEvent *event) {
    QWidget::resizeEvent(event);
    QPixmap pixmap(":/images/WelcomeScreen2.0.png");
    ui->welcomeImage->setPixmap(pixmap.scaled(ui->welcomeImage->size(), Qt::KeepAspectRatio, Qt::SmoothTransformation));
}
