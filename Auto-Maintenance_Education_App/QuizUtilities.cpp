/// Assignment: A9
/// Team Members:
/// Rianna McIntyre, Andrew Winward, Olivia Matvejeva,
/// Trichia Crouch, Ben Pond, and Nick Jarvis.
/// Team Name: Qt Pie Programmers.
/// Github Names:
/// rianlmci, drewblue42, omatvejeva,
/// T-crouch, BPond314, jarvisnc
/// Repo Link: github.com/University-of-Utah-CS3505/a9-edu-app-f24-rianlmci
/// @brief Function implementations for the quiz utilities; these are helper-functions
/// which evaluate a user's performance on a quiz question.

#include <QButtonGroup>
#include <QComboBox>
#include <QLabel>
#include <QLineEdit>
#include <QRadioButton>
#include "QuizUtilities.h"

void QuizUtilities::markResultLabel(QLabel *resultLabel, bool correct)
{
    if (correct)
    {
        resultLabel->setText("🗹");
        resultLabel->setStyleSheet("color: green");
    }
    else
    {
        resultLabel->setText("🗷");
        resultLabel->setStyleSheet("color: red");
    }
}

bool QuizUtilities::gradeMultipleChoice(QButtonGroup *buttonGroup,
                                        QRadioButton *answerButton,
                                        QLabel *resultLabel)
{
    // Disable question
    QList<QAbstractButton *> buttons = buttonGroup->buttons();
    for (QAbstractButton *button : buttons)
    {
        button->setEnabled(false);
    }

    // Check answer and mark result
    QuizUtilities::markResultLabel(resultLabel, answerButton->isChecked());
    return answerButton->isChecked();
}

bool QuizUtilities::gradeFillInTheBlank(QLineEdit *inputLine,
                                        const QList<QString> &acceptableAnswers,
                                        QLabel *resultLabel)
{
    // Disable question
    inputLine->setEnabled(false);

    // Sanitize user's answer
    QString userAnswer = inputLine->text().toLower().simplified();

    // Check answer and mark result
    QuizUtilities::markResultLabel(resultLabel, acceptableAnswers.contains(userAnswer));
    return acceptableAnswers.contains(userAnswer);
}

bool QuizUtilities::gradeDropDown(QComboBox *inputBox, int correctAnswerIndex, QLabel *resultLabel)
{
    // Disable question
    inputBox->setEnabled(false);

    // Check answer and mark result
    QuizUtilities::markResultLabel(resultLabel, inputBox->currentIndex() == correctAnswerIndex);
    return inputBox->currentIndex() == correctAnswerIndex;
}

void QuizUtilities::resetMultipleChoice(QButtonGroup *buttonGroup, QLabel *resultLabel)
{
    buttonGroup->setExclusive(false);
    QList<QAbstractButton *> buttons = buttonGroup->buttons();
    for (QAbstractButton *button : buttons)
    {
        button->setEnabled(true);
        button->setChecked(false);
    }
    buttonGroup->setExclusive(true);
    resultLabel->setText("");
}

void QuizUtilities::resetFillInTheBlank(QLineEdit *inputLine, QLabel *resultLabel)
{
    inputLine->setText("");
    inputLine->setEnabled(true);
    resultLabel->setText("");
}

void QuizUtilities::resetDropDown(QComboBox *inputBox, QLabel *resultLabel)
{
    inputBox->setCurrentIndex(-1);
    inputBox->setEnabled(true);
    resultLabel->setText("");
}
