/// Assignment: A9
/// Team Members:
/// Rianna McIntyre, Andrew Winward, Olivia Matvejeva,
/// Trichia Crouch, Ben Pond, and Nick Jarvis.
/// Team Name: Qt Pie Programmers.
/// Github Names:
/// rianlmci, drewblue42, omatvejeva,
/// T-crouch, BPond314, jarvisnc
/// Repo Link: github.com/University-of-Utah-CS3505/a9-edu-app-f24-rianlmci
/// @brief Qt Designer Form method and slot implementations for the Warning Lights information.

#include "PageEnum.h"
#include "ui_warninglightsinfoform.h"
#include "warninglightsinfoform.h"

warningLightsInfoForm::warningLightsInfoForm(QWidget *parent)
    : QWidget(parent)
    , ui(new Ui::warningLightsInfoForm)
{
    ui->setupUi(this);
}

warningLightsInfoForm::~warningLightsInfoForm()
{
    delete ui;
}

void warningLightsInfoForm::on_tableOfContentsButton_clicked()
{
    emit requestSetPage(Page::TABLE_OF_CONTENTS);
}

void warningLightsInfoForm::on_warningLightsQuizButton_clicked()
{
    emit requestSetPage(Page::WARNING_LIGHTS_QUIZ);
}
