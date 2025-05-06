/// Assignment: A9
/// Team Members:
/// Rianna McIntyre, Andrew Winward, Olivia Matvejeva,
/// Trichia Crouch, Ben Pond, and Nick Jarvis.
/// Team Name: Qt Pie Programmers.
/// Github Names:
/// rianlmci, drewblue42, omatvejeva,
/// T-crouch, BPond314, jarvisnc
/// Repo Link: github.com/University-of-Utah-CS3505/a9-edu-app-f24-rianlmci
/// @brief Qt Designer Form method and slot implementations for the Fluid Replacement information.

#include "fluidreplacementinfoform.h"
#include "PageEnum.h"
#include "ui_fluidreplacementinfoform.h"

FluidReplacementInfoForm::FluidReplacementInfoForm(QWidget *parent)
    : QWidget(parent)
    , ui(new Ui::FluidReplacementInfoForm)
{
    ui->setupUi(this);
}

FluidReplacementInfoForm::~FluidReplacementInfoForm()
{
    delete ui;
}

void FluidReplacementInfoForm::on_tableOfContentsButton_clicked()
{
    emit requestSetPage(Page::TABLE_OF_CONTENTS);
}

void FluidReplacementInfoForm::on_fluidReplacementQuizButton_clicked()
{
    emit requestSetPage(Page::BASIC_FLUID_QUIZ);
}
