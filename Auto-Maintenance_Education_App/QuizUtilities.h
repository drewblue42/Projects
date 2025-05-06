/// Assignment: A9
/// Team Members:
/// Rianna McIntyre, Andrew Winward, Olivia Matvejeva,
/// Trichia Crouch, Ben Pond, and Nick Jarvis.
/// Team Name: Qt Pie Programmers.
/// Github Names:
/// rianlmci, drewblue42, omatvejeva,
/// T-crouch, BPond314, jarvisnc
/// Repo Link: github.com/University-of-Utah-CS3505/a9-edu-app-f24-rianlmci
/// @brief Helper functions which are used when grading quiz questions.

#ifndef QUIZUTILITIES_H
#define QUIZUTILITIES_H

#include <QButtonGroup>
#include <QComboBox>
#include <QLabel>
#include <QLineEdit>
#include <QRadioButton>

namespace QuizUtilities
{
    ///@brief sets the result label text to show whether the answer was correct or not
    void markResultLabel(QLabel *resultLabel, bool correct);

    ///@brief grades the multiple choice questions
    bool gradeMultipleChoice(QButtonGroup *buttonGroup, QRadioButton *answerButton, QLabel *resultLabel);

    ///@brief grades the fill in the blank questions
    bool gradeFillInTheBlank(QLineEdit *inputLine,
                         const QList<QString> &acceptableAnswers,
                         QLabel *resultLabel);

    ///@brief grades the drop down questions
    bool gradeDropDown(QComboBox *inputBox, int correctAnswerIndex, QLabel *resultLabel);

    ///@brief resets the multiple choice questions
    void resetMultipleChoice(QButtonGroup *buttonGroup, QLabel *resultLabel);

    ///@brief resets the fill in the blank qestions
    void resetFillInTheBlank(QLineEdit *inputLine, QLabel *resultLabel);

    ///@brief resets the drop down questions
    void resetDropDown(QComboBox *inputBox, QLabel *resultLabel);
}

#endif // QUIZUTILITIES_H
