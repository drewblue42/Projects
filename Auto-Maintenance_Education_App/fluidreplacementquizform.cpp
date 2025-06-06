/// Assignment: A9
/// Team Members:
/// Rianna McIntyre, Andrew Winward, Olivia Matvejeva,
/// Trichia Crouch, Ben Pond, and Nick Jarvis.
/// Team Name: Qt Pie Programmers.
/// Github Names:
/// rianlmci, drewblue42, omatvejeva,
/// T-crouch, BPond314, jarvisnc
/// Repo Link: github.com/University-of-Utah-CS3505/a9-edu-app-f24-rianlmci
/// @brief Qt Designer Form method and slot implementations for the Fluid Replacement quiz.

#include "fluidreplacementquizform.h"
#include "PageEnum.h"
#include "QuizUtilities.h"
#include "ui_fluidreplacementquizform.h"

FluidReplacementQuizForm::FluidReplacementQuizForm(QWidget *parent)
    : QWidget(parent)
    , ui(new Ui::FluidReplacementQuizForm)
{
    ui->setupUi(this);
}

FluidReplacementQuizForm::~FluidReplacementQuizForm()
{
    delete ui;
}

void FluidReplacementQuizForm::on_tableOfContentsButton_clicked()
{
    emit requestSetPage(Page::TABLE_OF_CONTENTS);
}

void FluidReplacementQuizForm::on_fluidReplacementInfoButton_clicked()
{
    emit requestSetPage(Page::BASIC_FLUID_INFO);
}

void FluidReplacementQuizForm::on_submitButton_clicked()
{
    ui->submitButton->setEnabled(false);
    ui->resetButton->setEnabled(true);

    bool q1 = QuizUtilities::gradeMultipleChoice(ui->q1ButtonGroup, ui->q1AnswerBButton, ui->q1ResultLabel);
    bool q2 = QuizUtilities::gradeMultipleChoice(ui->q2ButtonGroup, ui->q2AnswerDButton, ui->q2ResultLabel);
    bool q3 = QuizUtilities::gradeMultipleChoice(ui->q3ButtonGroup, ui->q3AnswerAButton, ui->q3ResultLabel);
    bool q4 = QuizUtilities::gradeMultipleChoice(ui->q4ButtonGroup, ui->q4AnswerCButton, ui->q4ResultLabel);
    bool q5 = QuizUtilities::gradeMultipleChoice(ui->q5ButtonGroup, ui->q5AnswerTrueButton, ui->q5ResultLabel);
    bool q6 = QuizUtilities::gradeMultipleChoice(ui->q6ButtonGroup, ui->q6AnswerFalseButton, ui->q6ResultLabel);

    bool q7 = QuizUtilities::gradeFillInTheBlank(ui->q7AnswerLine,
                                       {"power steering", "steering"},
                                       ui->q7ResultLabel);
    bool q8 = QuizUtilities::gradeFillInTheBlank(ui->q8AnswerLine,
                                       {"oil", "engine oil", "motor oil", "everything"},
                                       ui->q8ResultLabel);

    bool q9 = QuizUtilities::gradeMultipleChoice(ui->q9ButtonGroup, ui->q9AnswerBButton, ui->q9ResultLabel);
    bool q10 = QuizUtilities::gradeMultipleChoice(ui->q10ButtonGroup, ui->q10AnswerAButton, ui->q10ResultLabel);

    if (q1 && q2 && q3 && q4 && q5 && q6 && q7 && q8 && q9 && q10)
    {
        emit requestSetFluidButtonGreen();
    }
}

void FluidReplacementQuizForm::on_resetButton_clicked()
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

    QuizUtilities::resetMultipleChoice(ui->q9ButtonGroup, ui->q9ResultLabel);
    QuizUtilities::resetMultipleChoice(ui->q10ButtonGroup, ui->q10ResultLabel);
}
