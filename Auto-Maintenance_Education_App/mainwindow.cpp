/// Assignment: A9
/// Team Members:
/// Rianna McIntyre, Andrew Winward, Olivia Matvejeva,
/// Trichia Crouch, Ben Pond, and Nick Jarvis.
/// Team Name: Qt Pie Programmers.
/// Github Names:
/// rianlmci, drewblue42, omatvejeva,
/// T-crouch, BPond314, jarvisnc
/// Repo Link: github.com/University-of-Utah-CS3505/a9-edu-app-f24-rianlmci
/// @brief Method and slot implementations for the MainWindow.

#include "mainwindow.h"
#include "PageEnum.h"
#include "ui_mainwindow.h"

MainWindow::MainWindow(QWidget *parent)
    : QMainWindow(parent)
    , ui(new Ui::MainWindow)
{
    ui->setupUi(this);

    setupConnections();

    // build mapping from page-index to its widget
    pages[Page::WELCOME] = ui->welcomePage;
    pages[Page::TABLE_OF_CONTENTS] = ui->tableOfContentsPage;
    pages[Page::JUMP_START_INFO] = ui->jumpStartInfoPage;
    pages[Page::JUMP_START_QUIZ] = ui->jumpStartQuizPage;
    pages[Page::SPARE_TIRE_INFO] = ui->spareTireInfoPage;
    pages[Page::SPARE_TIRE_QUIZ] = ui->spareTireQuizPage;
    pages[Page::TIRE_PRESSURE_INFO] = ui->tirePressureInfoPage;
    pages[Page::TIRE_PRESSURE_QUIZ] = ui->tirePressureQuizPage;
    pages[Page::WARNING_LIGHTS_INFO] = ui->warningLightsInfoPage;
    pages[Page::WARNING_LIGHTS_QUIZ] = ui->warningLightsQuizPage;
    pages[Page::WIPER_BLADE_INFO] = ui->wiperBladeInfoPage;
    pages[Page::WIPER_BLADE_QUIZ] = ui->wiperBladeQuizPage;
    pages[Page::VICTORY] = ui->victoryPage;
    pages[Page::BASIC_FLUID_INFO] = ui->basicFluidInfoPage;
    pages[Page::BASIC_FLUID_QUIZ] = ui->basicFluidQuizPage;

    // start on the "Welcome" page when the app opens
    setPage(Page::WELCOME);
}

MainWindow::~MainWindow()
{
    delete ui;
}

void MainWindow::setPage(Page pageNum)
{
    QWidget* widget = pages[pageNum];
    ui->stackedWidget->setCurrentWidget(widget);
}

void MainWindow::setupConnections()
{
    connect
        (
            ui->welcomePage,
            &WelcomeForm::requestSetPage,
            this,
            &MainWindow::setPage
        );

    connect
        (
            ui->tableOfContentsPage,
            &TableOfContentsForm::requestSetPage,
            this,
            &MainWindow::setPage
        );

    connect
        (
            ui->tirePressureInfoPage,
            &TirePressureInfoForm::requestSetPage,
            this,
            &MainWindow::setPage
        );

    connect
        (
            ui->tirePressureQuizPage,
            &TirePressureQuizForm::requestSetPage,
            this,
            &MainWindow::setPage
        );

    connect
        (
            ui->jumpStartInfoPage,
            &jumpStartInfoForm::requestSetPage,
            this,
            &MainWindow::setPage
        );

    connect
        (
            ui->jumpStartQuizPage,
            &jumpStartQuizForm::requestSetPage,
            this,
            &MainWindow::setPage
        );

    connect
        (
            ui->spareTireInfoPage,
            &spareTireInfoForm::requestSetPage,
            this,
            &MainWindow::setPage
        );

    connect
        (
            ui->spareTireQuizPage,
            &spareTireQuizForm::requestSetPage,
            this,
            &MainWindow::setPage
        );

    connect
        (
            ui->warningLightsInfoPage,
            &warningLightsInfoForm::requestSetPage,
            this,
            &MainWindow::setPage
        );

    connect
        (
            ui->warningLightsQuizPage,
            &warningLightsQuizForm::requestSetPage,
            this,
            &MainWindow::setPage
        );

    connect
        (
            ui->wiperBladeInfoPage,
            &wiperBladeInfoForm::requestSetPage,
            this,
            &MainWindow::setPage
        );

    connect
        (
            ui->wiperBladeQuizPage,
            &wiperBladeQuizForm::requestSetPage,
            this,
            &MainWindow::setPage
        );

    connect
        (
            ui->basicFluidInfoPage,
            &FluidReplacementInfoForm::requestSetPage,
            this,
            &MainWindow::setPage
        );

    connect
        (
            ui->basicFluidQuizPage,
            &FluidReplacementQuizForm::requestSetPage,
            this,
            &MainWindow::setPage
        );

    connect
        (
            ui->basicFluidQuizPage,
            &FluidReplacementQuizForm::requestSetFluidButtonGreen,
            ui->tableOfContentsPage,
            &TableOfContentsForm::setFluidButtonGreen
        );

    connect
        (
            ui->jumpStartQuizPage,
            &jumpStartQuizForm::requestSetJumpStartButtonGreen,
            ui->tableOfContentsPage,
            &TableOfContentsForm::setJumpStartButtonGreen
        );

    connect
        (
            ui->spareTireQuizPage,
            &spareTireQuizForm::requestSetSpareTireButtonGreen,
            ui->tableOfContentsPage,
            &TableOfContentsForm::setSpareTireButtonGreen
        );

    connect
        (
            ui->tirePressureQuizPage,
            &TirePressureQuizForm::requestSetTirePressureButtonGreen,
            ui->tableOfContentsPage,
            &TableOfContentsForm::setTirePressureButtonGreen
        );

    connect
        (
            ui->warningLightsQuizPage,
            &warningLightsQuizForm::requestSetWarningLightsButtonGreen,
            ui->tableOfContentsPage,
            &TableOfContentsForm::setWarningLightsButtonGreen
        );

    connect
        (
            ui->wiperBladeQuizPage,
            &wiperBladeQuizForm::requestSetWiperBladeButtonGreen,
            ui->tableOfContentsPage,
            &TableOfContentsForm::setWiperBladeButtonGreen
        );

    connect
        (
            ui->victoryPage,
            &VictoryForm::requestSetPage,
            this,
            &MainWindow::setPage
        );

    connect(
        ui->tableOfContentsPage,
        &TableOfContentsForm::requestStartBox2DSimulation,
        ui->victoryPage,
        &VictoryForm::startBox2DSimulation
        );
}
