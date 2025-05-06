#include "victoryform.h"
#include "ui_victoryform.h"

VictoryForm::VictoryForm(QWidget *parent)
    : QWidget(parent)
    , ui(new Ui::VictoryForm)
{
    ui->setupUi(this);
}

VictoryForm::~VictoryForm()
{
    delete ui;
}

void VictoryForm::on_tableOfContentsButton_clicked()
{
    emit requestSetPage(Page::TABLE_OF_CONTENTS);
}

void VictoryForm::startBox2DSimulation(){
    ui->sceneWidget->startTimer();
}

