/// Assignment: A9
/// Team Members:
/// Rianna McIntyre, Andrew Winward, Olivia Matvejeva,
/// Trichia Crouch, Ben Pond, and Nick Jarvis.
/// Team Name: Qt Pie Programmers.
/// Github Names:
/// rianlmci, drewblue42, omatvejeva,
/// T-crouch, BPond314, jarvisnc
/// Repo Link: github.com/University-of-Utah-CS3505/a9-edu-app-f24-rianlmci
/// @brief Qt Designer Form method and slot implementations for the Spare Tire information.

#include "PageEnum.h"
#include "tirepressureinfoform.h"
#include "ui_tirepressureinfoform.h"

TirePressureInfoForm::TirePressureInfoForm(QWidget *parent)
    : QWidget(parent)
    , ui(new Ui::TirePressureInfoForm)
{
    ui->setupUi(this);
}

TirePressureInfoForm::~TirePressureInfoForm()
{
    delete ui;
}

void TirePressureInfoForm::on_tableOfContentsButton_clicked()
{
    emit requestSetPage(Page::TABLE_OF_CONTENTS);
}

void TirePressureInfoForm::on_tirePressureQuizButton_clicked()
{
    emit requestSetPage(Page::TIRE_PRESSURE_QUIZ);
}
