
/// Assignment: A9
/// Team Members:
/// Rianna McIntyre, Andrew Winward, Olivia Matvejeva,
/// Trichia Crouch, Ben Pond, and Nick Jarvis.
/// Team Name: Qt Pie Programmers.
/// Github Names:
/// rianlmci, drewblue42, omatvejeva,
/// T-crouch, BPond314, jarvisnc
/// Repo Link: github.com/University-of-Utah-CS3505/a9-edu-app-f24-rianlmci
/// @brief Qt Designer Form method and slot implementations for the Jump Start information.

#include "jumpstartinfoform.h"
#include "PageEnum.h"
#include "ui_jumpstartinfoform.h"

jumpStartInfoForm::jumpStartInfoForm(QWidget *parent)
    : QWidget(parent)
    , ui(new Ui::jumpStartInfoForm)
{
    ui->setupUi(this);
}

jumpStartInfoForm::~jumpStartInfoForm()
{
    delete ui;
}

void jumpStartInfoForm::on_tableOfContentsButton_clicked()
{
    emit requestSetPage(Page::TABLE_OF_CONTENTS);
}

void jumpStartInfoForm::on_jumpStartQuizButton_clicked()
{
    emit requestSetPage(Page::JUMP_START_QUIZ);
}
