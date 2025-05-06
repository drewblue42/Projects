/// Assignment: A9
/// Team Members:
/// Rianna McIntyre, Andrew Winward, Olivia Matvejeva,
/// Trichia Crouch, Ben Pond, and Nick Jarvis.
/// Team Name: Qt Pie Programmers.
/// Github Names:
/// rianlmci, drewblue42, omatvejeva,
/// T-crouch, BPond314, jarvisnc
/// Repo Link: github.com/University-of-Utah-CS3505/a9-edu-app-f24-rianlmci
/// @brief Qt Designer Form method and slot implementations for the Jump Start quiz.

#include "jumpstartquizform.h"
#include "PageEnum.h"
#include "QuizUtilities.h"
#include "ui_jumpstartquizform.h"

jumpStartQuizForm::jumpStartQuizForm(QWidget *parent)
    : QWidget(parent)
    , ui(new Ui::jumpStartQuizForm)
{
    ui->setupUi(this);
}

jumpStartQuizForm::~jumpStartQuizForm()
{
    delete ui;
}

void jumpStartQuizForm::on_tableOfContentsButton_clicked()
{
    emit requestSetPage(Page::TABLE_OF_CONTENTS);
}

void jumpStartQuizForm::on_jumpStartInfoButton_clicked()
{
    emit requestSetPage(Page::JUMP_START_INFO);
}

void jumpStartQuizForm::on_submitButton_clicked()
{
    ui->submitButton->setEnabled(false);
    ui->resetButton->setEnabled(true);

    bool q1 = QuizUtilities::gradeMultipleChoice(ui->q1ButtonGroup, ui->q1AnswerDButton, ui->q1ResultLabel);
    bool q2 = QuizUtilities::gradeMultipleChoice(ui->q2ButtonGroup, ui->q2AnswerAButton, ui->q2ResultLabel);
    bool q3 = QuizUtilities::gradeMultipleChoice(ui->q3ButtonGroup, ui->q3AnswerBButton, ui->q3ResultLabel);
    bool q4 = QuizUtilities::gradeMultipleChoice(ui->q4ButtonGroup, ui->q4AnswerFalseButton, ui->q4ResultLabel);
    bool q5 = QuizUtilities::gradeMultipleChoice(ui->q5ButtonGroup, ui->q5AnswerTrueButton, ui->q5ResultLabel);
    bool q6 = QuizUtilities::gradeMultipleChoice(ui->q6ButtonGroup, ui->q6AnswerTrueButton, ui->q6ResultLabel);

    bool q7 = QuizUtilities::gradeFillInTheBlank(ui->q7AnswerLine, {"red"}, ui->q7ResultLabel);
    bool q8 = QuizUtilities::gradeFillInTheBlank(ui->q8AnswerLine, {"metal"}, ui->q8ResultLabel);
    bool q9 = QuizUtilities::gradeFillInTheBlank(ui->q9AnswerLine,
                                       {"20-30",
                                        "20 to 30",
                                        "20 - 30",
                                        "twenty to thirty",
                                        "20",
                                        "21",
                                        "22",
                                        "23",
                                        "24",
                                        "25",
                                        "26",
                                        "27",
                                        "28",
                                        "29",
                                        "30"},
                                       ui->q9ResultLabel);

    bool q10 = QuizUtilities::gradeMultipleChoice(ui->q10ButtonGroup, ui->q10AnswerCButton, ui->q10ResultLabel);
    bool q11 = QuizUtilities::gradeMultipleChoice(ui->q11ButtonGroup, ui->q11AnswerCButton, ui->q11ResultLabel);

    if (q1 && q2 && q3 && q4 && q5 && q6 && q7 && q8 && q9 && q10 && q11)
    {
        emit requestSetJumpStartButtonGreen();
    }
}

void jumpStartQuizForm::on_resetButton_clicked()
{
    ui->submitButton->setEnabled(true);
    ui->resetButton->setEnabled(false);

    QuizUtilities::resetMultipleChoice(ui->q1ButtonGroup, ui->q1ResultLabel);
    QuizUtilities::resetMultipleChoice(ui->q2ButtonGroup, ui->q2ResultLabel);
    QuizUtilities::resetMultipleChoice(ui->q3ButtonGroup, ui->q3ResultLabel);
    QuizUtilities::resetMultipleChoice(ui->q4ButtonGroup, ui->q4ResultLabel);
    QuizUtilities::resetMultipleChoice(ui->q5ButtonGroup, ui->q5ResultLabel);
    QuizUtilities::resetMultipleChoice(ui->q6ButtonGroup, ui->q6ResultLabel);

    QuizUtilities::resetFillInTheBlank(ui->q7AnswerLine, ui->q7ResultLabel);
    QuizUtilities::resetFillInTheBlank(ui->q8AnswerLine, ui->q8ResultLabel);
    QuizUtilities::resetFillInTheBlank(ui->q9AnswerLine, ui->q9ResultLabel);

    QuizUtilities::resetMultipleChoice(ui->q10ButtonGroup, ui->q10ResultLabel);
    QuizUtilities::resetMultipleChoice(ui->q11ButtonGroup, ui->q11ResultLabel);
}
