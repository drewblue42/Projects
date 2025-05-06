/// Assignment: A9
/// Team Members:
/// Rianna McIntyre, Andrew Winward, Olivia Matvejeva,
/// Trichia Crouch, Ben Pond, and Nick Jarvis.
/// Team Name: Qt Pie Programmers.
/// Github Names:
/// rianlmci, drewblue42, omatvejeva,
/// T-crouch, BPond314, jarvisnc
/// Repo Link: github.com/University-of-Utah-CS3505/a9-edu-app-f24-rianlmci
/// @brief Qt Designer Form header for the Table of Contents page.

#ifndef TABLEOFCONTENTSFORM_H
#define TABLEOFCONTENTSFORM_H

#include <QIcon>
#include <QSvgRenderer>
#include <QWidget>
#include "PageEnum.h"

namespace Ui
{
    class TableOfContentsForm;
}

class TableOfContentsForm : public QWidget
{
    Q_OBJECT

public:
    explicit TableOfContentsForm(QWidget *parent = nullptr);
    ~TableOfContentsForm();

signals:
    ///@brief signal that lets the program know to change the displayed screen
    void requestSetPage(Page);
    void requestStartBox2DSimulation();

public slots:
    ///@brief called when all the questions in the Fluid Replacement quiz are correct,
    void setFluidButtonGreen();

    ///@brief called when all the questions in the Jump Start quiz are correct
    void setJumpStartButtonGreen();

    ///@brief called when all the questions in the Tire Replacement/Spare Tire quiz are correct
    void setSpareTireButtonGreen();

    ///@brief called when all the questions in the Tire Pressure quiz are correct
    void setTirePressureButtonGreen();

    ///@brief called when all the questions in the Warning Lights quiz are correct
    void setWarningLightsButtonGreen();

    ///@brief called when all the questions in the Wiper Blade quiz are correct
    void setWiperBladeButtonGreen();

    ///@brief called when the Tire Pressure info button is clicked; emits a signal
    /// to set the active page/form to the Tire Pressure info page
    void on_tirePressureInfoButton_clicked();

    ///@brief called when the Tire Pressure info button is clicked; emits a signal
    /// to set the active page/form to the Warning Lights info page
    void on_warningLightsButton_clicked();

    ///@brief called when the Tire Pressure info button is clicked; emits a signal
    /// to set the active page/form to the Tire Replacement/Spare Tire info page
    void on_spareTireButton_clicked();

    ///@brief called when the Tire Pressure info button is clicked; emits a signal
    /// to set the active page/form to the Jump Start info page
    void on_jumpStartingButton_clicked();

    ///@brief called when the Tire Pressure info button is clicked; emits a signal
    /// to set the active page/form to the Wiper Blade info page
    void on_wiperBladeReplacement_clicked();

    ///@brief called when the Tire Pressure info button is clicked; emits a signal
    /// to set the active page/form to the Fluid Replacement info page
    void on_basicFluidReplacementButton_clicked();

    ///@brief called when the Tire Pressure info button is clicked; emits a signal
    /// to set the active page/form to the Tire Pressure info page
    void on_quizButton_clicked();

private slots:
    void on_pushButton_clicked();

private:
    Ui::TableOfContentsForm *ui;
    QIcon svgToIcon(QSvgRenderer *svg);

    ///@brief examines each of the table-of-contents' navigation buttons and checks
    /// whether they are all green (indicating all of the quizzes have been passed);
    /// if so, allow access to the "victory" page
    void checkButtonStyles();
};

#endif // TABLEOFCONTENTSFORM_H
