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
#include "sparetireinfoform.h"
#include "ui_sparetireinfoform.h"

spareTireInfoForm::spareTireInfoForm(QWidget *parent)
    : QWidget(parent)
    , ui(new Ui::spareTireInfoForm)
{
    ui->setupUi(this);
}

spareTireInfoForm::~spareTireInfoForm()
{
    delete ui;
}

void spareTireInfoForm::on_tableOfContentsButton_clicked()
{
    emit requestSetPage(Page::TABLE_OF_CONTENTS);
}

void spareTireInfoForm::on_spareTireQuizButton_clicked()
{
    emit requestSetPage(Page::SPARE_TIRE_QUIZ);
}
