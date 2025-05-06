/// Assignment: A9
/// Team Members:
/// Rianna McIntyre, Andrew Winward, Olivia Matvejeva,
/// Trichia Crouch, Ben Pond, and Nick Jarvis.
/// Team Name: Qt Pie Programmers.
/// Github Names:
/// rianlmci, drewblue42, omatvejeva,
/// T-crouch, BPond314, jarvisnc
/// Repo Link: github.com/University-of-Utah-CS3505/a9-edu-app-f24-rianlmci
/// @brief Qt Designer Form method and slot implementations for the Wiper Blade information.

#include "PageEnum.h"
#include "ui_wiperbladeinfoform.h"
#include "wiperbladeinfoform.h"

wiperBladeInfoForm::wiperBladeInfoForm(QWidget *parent)
    : QWidget(parent)
    , ui(new Ui::wiperBladeInfoForm)
{
    ui->setupUi(this);
}

wiperBladeInfoForm::~wiperBladeInfoForm()
{
    delete ui;
}

void wiperBladeInfoForm::on_tableOfContentsButton_clicked()
{
    emit requestSetPage(Page::TABLE_OF_CONTENTS);
}

void wiperBladeInfoForm::on_wiperBladeQuizButton_clicked()
{
    emit requestSetPage(Page::WIPER_BLADE_QUIZ);
}
