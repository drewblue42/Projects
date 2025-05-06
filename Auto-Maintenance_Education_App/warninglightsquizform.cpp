/// Assignment: A9
/// Team Members:
/// Rianna McIntyre, Andrew Winward, Olivia Matvejeva,
/// Trichia Crouch, Ben Pond, and Nick Jarvis.
/// Team Name: Qt Pie Programmers.
/// Github Names:
/// rianlmci, drewblue42, omatvejeva,
/// T-crouch, BPond314, jarvisnc
/// Repo Link: github.com/University-of-Utah-CS3505/a9-edu-app-f24-rianlmci
/// @brief Qt Designer Form method and slot implementations for the Warning Lights quiz.

#include "PageEnum.h"
#include "QuizUtilities.h"
#include "ui_warninglightsquizform.h"
#include "warninglightsquizform.h"

warningLightsQuizForm::warningLightsQuizForm(QWidget *parent)
    : QWidget(parent)
    , ui(new Ui::warningLightsQuizForm)
{
    ui->setupUi(this);
}

warningLightsQuizForm::~warningLightsQuizForm()
{
    delete ui;
}

void warningLightsQuizForm::on_tableOfContentsButton_clicked()
{
    emit requestSetPage(Page::TABLE_OF_CONTENTS);
}

void warningLightsQuizForm::on_warningLightsInfoButton_clicked()
{
    emit requestSetPage(Page::WARNING_LIGHTS_INFO);
}

void warningLightsQuizForm::on_submitButton_clicked()
{
    ui->submitButton->setEnabled(false);
    ui->resetButton->setEnabled(true);

    bool q1 = QuizUtilities::gradeMultipleChoice(ui->q1ButtonGroup, ui->q1AnswerBButton, ui->q1ResultLabel);
    bool q2 = QuizUtilities::gradeMultipleChoice(ui->q2ButtonGroup, ui->q2AnswerAButton, ui->q2ResultLabel);
    bool q3 = QuizUtilities::gradeMultipleChoice(ui->q3ButtonGroup, ui->q3AnswerBButton, ui->q3ResultLabel);
    bool q4 = QuizUtilities::gradeMultipleChoice(ui->q4ButtonGroup, ui->q4AnswerDButton, ui->q4ResultLabel);
    bool q5 = QuizUtilities::gradeMultipleChoice(ui->q5ButtonGroup, ui->q5AnswerTrueButton, ui->q5ResultLabel);
    bool q6 = QuizUtilities::gradeMultipleChoice(ui->q6ButtonGroup, ui->q6AnswerFalseButton, ui->q6ResultLabel);
    bool q7 = QuizUtilities::gradeMultipleChoice(ui->q7ButtonGroup, ui->q7AnswerTrueButton, ui->q7ResultLabel);

    bool q8 = QuizUtilities::gradeFillInTheBlank(ui->q8AnswerLine,
                                       {"coolant", "coolant temperature", "water temperature"},
                                       ui->q8ResultLabel);
    bool q9 = QuizUtilities::gradeFillInTheBlank(ui->q9AnswerLine,
                                       {"battery", "charging system"},
                                       ui->q9ResultLabel);
    bool q10 = QuizUtilities::gradeFillInTheBlank(ui->q10AnswerLine,
                                       {"seat belt",
                                        "seatbelt",
                                        "seat-belt",
                                        "seat belts",
                                        "seatbelts",
                                        "seat-belts",
                                        "fasten seat belts",
                                        "fasten seatbelts",
                                        "fasten seat-belts"},
                                       ui->q10ResultLabel);

    bool q11 = QuizUtilities::gradeMultipleChoice(ui->q11ButtonGroup, ui->q11AnswerBButton, ui->q11ResultLabel);
    bool q12 = QuizUtilities::gradeMultipleChoice(ui->q12ButtonGroup, ui->q12AnswerCButton, ui->q12ResultLabel);

    if (q1 && q2 && q3 && q4 && q5 && q6 && q7 && q8 && q9 && q10 && q11 && q12)
    {
        emit requestSetWarningLightsButtonGreen();
    }
}

void warningLightsQuizForm::on_resetButton_clicked()
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

    QuizUtilities::resetFillInTheBlank(ui->q8AnswerLine, ui->q8ResultLabel);
    QuizUtilities::resetFillInTheBlank(ui->q9AnswerLine, ui->q9ResultLabel);
    QuizUtilities::resetFillInTheBlank(ui->q10AnswerLine, ui->q10ResultLabel);

    QuizUtilities::resetMultipleChoice(ui->q11ButtonGroup, ui->q11ResultLabel);
    QuizUtilities::resetMultipleChoice(ui->q12ButtonGroup, ui->q12ResultLabel);
}
