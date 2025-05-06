/// Assignment: A9
/// Team Members:
/// Rianna McIntyre, Andrew Winward, Olivia Matvejeva,
/// Trichia Crouch, Ben Pond, and Nick Jarvis.
/// Team Name: Qt Pie Programmers.
/// Github Names:
/// rianlmci, drewblue42, omatvejeva,
/// T-crouch, BPond314, jarvisnc
/// Repo Link: github.com/University-of-Utah-CS3505/a9-edu-app-f24-rianlmci
/// @brief Qt Designer Form method and slot implementations for the Tire Replacement/Spare Tire quiz.

#include "PageEnum.h"
#include "sparetirequizform.h"
#include "QuizUtilities.h"
#include "ui_sparetirequizform.h"

spareTireQuizForm::spareTireQuizForm(QWidget *parent)
    : QWidget(parent)
    , ui(new Ui::spareTireQuizForm)
{
    ui->setupUi(this);

    ui->q3LineSelection->addItems({"Fully tighten the lug nuts",
                                   "Loosen the lug nuts slightly",
                                   "Remove the jack from under the car",
                                   "Check the air pressure in the spare tire"});

    ui->q5LineSelection->addItems({"Flat surface", "Sloped driveway", "Gravel road", "Sandy area"});

    ui->q7LineSelection->addItems({"Hand-tighten in a clockwise direction",
                                   "Hand-tighten in a criss-cross pattern",
                                   "Fully tighten one lug nut before moving to the next",
                                   "Use the wrench to tighten all at once"});

    ui->q9LineSelection->addItems({"Leave it on the side of the road",
                                   "Place it back in the vehicle's storage space",
                                   "Use it as a backup spare",
                                   "Discard it immediately"});
}

spareTireQuizForm::~spareTireQuizForm()
{
    delete ui;
}

void spareTireQuizForm::on_tableOfContentsButton_clicked()
{
    emit requestSetPage(Page::TABLE_OF_CONTENTS);
}

void spareTireQuizForm::on_spareTireInfoButton_clicked()
{
    emit requestSetPage(Page::SPARE_TIRE_INFO);
}

void spareTireQuizForm::on_submitButton_clicked()
{
    ui->submitButton->setEnabled(false);
    ui->resetButton->setEnabled(true);

    bool q1 = QuizUtilities::gradeMultipleChoice(ui->q1ButtonGroup, ui->q1AnswerBButton, ui->q1ResultLabel);
    bool q2 = QuizUtilities::gradeMultipleChoice(ui->q2ButtonGroup, ui->q2AnswerBButton, ui->q2ResultLabel);
    bool q3 = QuizUtilities::gradeDropDown(ui->q3LineSelection, 1, ui->q3ResultLabel);
    bool q4 = QuizUtilities::gradeMultipleChoice(ui->q4ButtonGroup, ui->q4AnswerCButton, ui->q4ResultLabel);
    bool q5 = QuizUtilities::gradeDropDown(ui->q5LineSelection, 0, ui->q5ResultLabel);
    bool q6 = QuizUtilities::gradeMultipleChoice(ui->q6ButtonGroup, ui->q6AnswerBButton, ui->q6ResultLabel);
    bool q7 = QuizUtilities::gradeDropDown(ui->q7LineSelection, 1, ui->q7ResultLabel);
    bool q8 = QuizUtilities::gradeMultipleChoice(ui->q8ButtonGroup, ui->q8AnswerAButton, ui->q8ResultLabel);
    bool q9 = QuizUtilities::gradeDropDown(ui->q9LineSelection, 1, ui->q9ResultLabel);
    bool q10 = QuizUtilities::gradeMultipleChoice(ui->q10ButtonGroup, ui->q10AnswerBButton, ui->q10ResultLabel);

    if (q1 && q2 && q3 && q4 && q5 && q6 && q7 && q8 && q9 && q10)
    {
        emit requestSetSpareTireButtonGreen();
    }
}

void spareTireQuizForm::on_resetButton_clicked()
{
    ui->submitButton->setEnabled(true);
    ui->resetButton->setEnabled(false);

    QuizUtilities::resetMultipleChoice(ui->q1ButtonGroup, ui->q1ResultLabel);
    QuizUtilities::resetMultipleChoice(ui->q2ButtonGroup, ui->q2ResultLabel);
    QuizUtilities::resetDropDown(ui->q3LineSelection, ui->q3ResultLabel);
    QuizUtilities::resetMultipleChoice(ui->q4ButtonGroup, ui->q4ResultLabel);
    QuizUtilities::resetDropDown(ui->q5LineSelection, ui->q5ResultLabel);
    QuizUtilities::resetMultipleChoice(ui->q6ButtonGroup, ui->q6ResultLabel);
    QuizUtilities::resetDropDown(ui->q7LineSelection, ui->q7ResultLabel);
    QuizUtilities::resetMultipleChoice(ui->q8ButtonGroup, ui->q8ResultLabel);
    QuizUtilities::resetDropDown(ui->q9LineSelection, ui->q9ResultLabel);
    QuizUtilities::resetMultipleChoice(ui->q10ButtonGroup, ui->q10ResultLabel);
}
