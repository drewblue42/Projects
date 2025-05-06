/// Assignment: A9
/// Team Members:
/// Rianna McIntyre, Andrew Winward, Olivia Matvejeva,
/// Trichia Crouch, Ben Pond, and Nick Jarvis.
/// Team Name: Qt Pie Programmers.
/// Github Names:
/// rianlmci, drewblue42, omatvejeva,
/// T-crouch, BPond314, jarvisnc
/// Repo Link: github.com/University-of-Utah-CS3505/a9-edu-app-f24-rianlmci
/// @brief Qt Designer Form method and slot implementations for the Tire Pressure quiz.

#include "PageEnum.h"
#include "QuizUtilities.h"
#include "tirepressurequizform.h"
#include "ui_tirepressurequizform.h"

TirePressureQuizForm::TirePressureQuizForm(QWidget *parent)
    : QWidget(parent)
    , ui(new Ui::TirePressureQuizForm)
{
    ui->setupUi(this);
}

TirePressureQuizForm::~TirePressureQuizForm()
{
    delete ui;
}

void TirePressureQuizForm::on_submitButton_clicked()
{
    ui->submitButton->setEnabled(false);
    ui->resetButton->setEnabled(true);

    bool q1 = QuizUtilities::gradeMultipleChoice(ui->q1ButtonGroup, ui->q1AnswerBButton, ui->q1ResultLabel);
    bool q2 = QuizUtilities::gradeMultipleChoice(ui->q2ButtonGroup, ui->q2AnswerBButton, ui->q2ResultLabel);
    bool q3 = QuizUtilities::gradeMultipleChoice(ui->q3ButtonGroup, ui->q3AnswerCButton, ui->q3ResultLabel);
    bool q4 = QuizUtilities::gradeMultipleChoice(ui->q4ButtonGroup, ui->q4AnswerCButton, ui->q4ResultLabel);
    bool q5 = QuizUtilities::gradeMultipleChoice(ui->q5ButtonGroup, ui->q5AnswerTrueButton, ui->q5ResultLabel);
    bool q6 = QuizUtilities::gradeMultipleChoice(ui->q6ButtonGroup, ui->q6AnswerFalseButton, ui->q6ResultLabel);
    bool q7 = QuizUtilities::gradeMultipleChoice(ui->q7ButtonGroup, ui->q7AnswerFalseButton, ui->q7ResultLabel);
    bool q8 = QuizUtilities::gradeMultipleChoice(ui->q8ButtonGroup, ui->q8AnswerTrueButton, ui->q8ResultLabel);


    bool q9 = QuizUtilities::gradeFillInTheBlank(ui->q9AnswerLine, {
                                                                    "tpms",
                                                                    "t.p.m.s.",
                                                                    "tire pressure monitoring system"
                                                                   },
                                                 ui->q9ResultLabel);

    // handle question #10 uniquely, because it has two input fields
    ui->q10AnswerLine1->setEnabled(false);
    ui->q10AnswerLine2->setEnabled(false);
    QString userAnswer1 = ui->q10AnswerLine1->text().toLower().simplified();
    QString userAnswer2 = ui->q10AnswerLine2->text().toLower().simplified();
    QList acceptableAnswers1{"75", "seventy-five", "seventy five"};
    QList acceptableAnswers2{"100", "one-hundred", "one hundred"};
    bool q10 = acceptableAnswers1.contains(userAnswer1) && acceptableAnswers2.contains(userAnswer2);
    QuizUtilities::markResultLabel(ui->q10ResultLabel, q10);

    bool q11 = QuizUtilities::gradeFillInTheBlank(ui->q11AnswerLine, {"60", "sixty"}, ui->q11ResultLabel);
    bool q12 = QuizUtilities::gradeFillInTheBlank(ui->q12AnswerLine, {"cold", "cool"}, ui->q12ResultLabel);

    if (q1 && q2 && q3 && q4 && q5 && q6 && q7 && q8 && q9 && q10 && q11 & q12)
    {
        emit requestSetTirePressureButtonGreen();
    }
}

void TirePressureQuizForm::on_resetButton_clicked()
{
    ui->submitButton->setEnabled(true);
    ui->resetButton->setEnabled(false);

    QuizUtilities::resetMultipleChoice(ui->q1ButtonGroup, ui->q1ResultLabel);
    QuizUtilities::resetMultipleChoice(ui->q2ButtonGroup, ui->q2ResultLabel);
    QuizUtilities::resetMultipleChoice(ui->q3ButtonGroup, ui->q3ResultLabel);
    QuizUtilities::resetMultipleChoice(ui->q4ButtonGroup, ui->q4ResultLabel);
    QuizUtilities::resetMultipleChoice(ui->q5ButtonGroup, ui->q5ResultLabel);
    QuizUtilities::resetMultipleChoice(ui->q6ButtonGroup, ui->q6ResultLabel);
    QuizUtilities::resetMultipleChoice(ui->q7ButtonGroup, ui->q7ResultLabel);
    QuizUtilities::resetMultipleChoice(ui->q8ButtonGroup, ui->q8ResultLabel);

    QuizUtilities::resetFillInTheBlank(ui->q9AnswerLine, ui->q9ResultLabel);

    // handle question #10 uniquely
    ui->q10AnswerLine1->setText("");
    ui->q10AnswerLine2->setText("");
    ui->q10AnswerLine1->setEnabled(true);
    ui->q10AnswerLine2->setEnabled(true);
    ui->q10ResultLabel->setText("");

    QuizUtilities::resetFillInTheBlank(ui->q11AnswerLine, ui->q11ResultLabel);
    QuizUtilities::resetFillInTheBlank(ui->q12AnswerLine, ui->q12ResultLabel);
}

void TirePressureQuizForm::on_tableOfContentsButton_clicked()
{
    emit requestSetPage(Page::TABLE_OF_CONTENTS);
}

void TirePressureQuizForm::on_tirePressureInfoButton_clicked()
{
    emit requestSetPage(Page::TIRE_PRESSURE_INFO);
}
