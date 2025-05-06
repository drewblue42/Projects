/// Assignment: A9
/// Team Members:
/// Rianna McIntyre, Andrew Winward, Olivia Matvejeva,
/// Trichia Crouch, Ben Pond, and Nick Jarvis.
/// Team Name: Qt Pie Programmers.
/// Github Names:
/// rianlmci, drewblue42, omatvejeva,
/// T-crouch, BPond314, jarvisnc
/// Repo Link: github.com/University-of-Utah-CS3505/a9-edu-app-f24-rianlmci
/// @brief Qt Designer Form method and slot implementations for the Table of Contents.

#include <QIcon>
#include <QPixmap>
#include <QSvgRenderer>
#include <QtSvg>
#include "PageEnum.h"
#include "tableofcontentsform.h"
#include "ui_tableofcontentsform.h"

TableOfContentsForm::TableOfContentsForm(QWidget *parent)
    : QWidget(parent)
    , ui(new Ui::TableOfContentsForm)
{
    ui->setupUi(this);

    ui->quizButton->setDisabled(true);
    //Background image setup
    QPixmap pixmap(":/images/car.png");
    int width = ui->homeScreenImage->width();
    int height = ui->homeScreenImage->height();
    ui->homeScreenImage->setPixmap(pixmap.scaled(width,height,Qt::KeepAspectRatio));
    ui->homeScreenImage->setScaledContents(true);

    //Mapping for button to svg
    QMap<QCommandLinkButton *, QSvgRenderer *> buttonIconMap;
    QSvgRenderer *oilSvg = new QSvgRenderer(QStringLiteral(":/images/oil-can-svgrepo-com.svg"));
    buttonIconMap[ui->basicFluidReplacementButton] = oilSvg;
    QSvgRenderer *tirePressureSvg = new QSvgRenderer(
        QStringLiteral(":/images/tire_pressure_icon.svg"));
    buttonIconMap[ui->tirePressureInfoButton] = tirePressureSvg;
    QSvgRenderer *batterySvg = new QSvgRenderer(QStringLiteral(":/images/car_battery_icon.svg"));
    buttonIconMap[ui->jumpStartingButton] = batterySvg;
    QSvgRenderer *tireSvg = new QSvgRenderer(QStringLiteral(":/images/tire_icon.svg"));
    buttonIconMap[ui->spareTireButton] = tireSvg;
    QSvgRenderer *engineSvg = new QSvgRenderer(QStringLiteral(":/images/engine_warning_icon.svg"));
    buttonIconMap[ui->warningLightsButton] = engineSvg;
    QSvgRenderer *wiperSvg = new QSvgRenderer(QStringLiteral(":/images/windshield_wiper_icon.svg"));
    buttonIconMap[ui->wiperBladeReplacement] = wiperSvg;

    for (QMap<QCommandLinkButton *, QSvgRenderer *>::iterator it = buttonIconMap.begin();
         it != buttonIconMap.end();
         ++it)
    {
        QIcon icon = svgToIcon(it.value());
        it.key()->setIcon(icon);
    }
}

TableOfContentsForm::~TableOfContentsForm()
{
    delete ui;
}

void TableOfContentsForm::on_tirePressureInfoButton_clicked()
{
    emit requestSetPage(Page::TIRE_PRESSURE_INFO);
}

void TableOfContentsForm::on_warningLightsButton_clicked()
{
    emit requestSetPage(Page::WARNING_LIGHTS_INFO);
}

void TableOfContentsForm::on_spareTireButton_clicked()
{
    emit requestSetPage(Page::SPARE_TIRE_INFO);
}

void TableOfContentsForm::on_jumpStartingButton_clicked()
{
    emit requestSetPage(Page::JUMP_START_INFO);
}

void TableOfContentsForm::on_wiperBladeReplacement_clicked()
{
    emit requestSetPage(Page::WIPER_BLADE_INFO);
}

void TableOfContentsForm::on_basicFluidReplacementButton_clicked()
{
    emit requestSetPage(Page::BASIC_FLUID_INFO);
}

QIcon TableOfContentsForm::svgToIcon(QSvgRenderer *svg)
{
    QPixmap pixmap(40, 40);
    pixmap.fill(Qt::transparent);
    QPainter painter(&pixmap);
    svg->render(&painter);
    QIcon icon(pixmap);
    return icon;
}

void TableOfContentsForm::on_quizButton_clicked()
{
    emit requestSetPage(Page::VICTORY);

    //signal the start of the physics simulation for victory
    emit requestStartBox2DSimulation();

}

void TableOfContentsForm::setFluidButtonGreen()
{
    ui->basicFluidReplacementButton->setStyleSheet("background-color: green; border-radius: 25%; text-align: center;");
    checkButtonStyles();
}

void TableOfContentsForm::setJumpStartButtonGreen()
{
    ui->jumpStartingButton->setStyleSheet("background-color: green; border-radius: 25%; text-align: center;");
    checkButtonStyles();
}

void TableOfContentsForm::setSpareTireButtonGreen()
{
    ui->spareTireButton->setStyleSheet("background-color: green; border-radius: 25%; text-align: center;");
    checkButtonStyles();
}

void TableOfContentsForm::setTirePressureButtonGreen()
{
    ui->tirePressureInfoButton->setStyleSheet("background-color: green; border-radius: 25%; text-align: center;");
    checkButtonStyles();
}

void TableOfContentsForm::setWarningLightsButtonGreen()
{
    ui->warningLightsButton->setStyleSheet("background-color: green; border-radius: 25%; text-align: center;");
    checkButtonStyles();
}

void TableOfContentsForm::setWiperBladeButtonGreen()
{
    ui->wiperBladeReplacement->setStyleSheet("background-color: green; border-radius: 25%; text-align: center;");
    checkButtonStyles();
}

void TableOfContentsForm::checkButtonStyles()
{
    QPushButton* buttons[] = {
        ui->basicFluidReplacementButton,
        ui->jumpStartingButton,
        ui->spareTireButton,
        ui->tirePressureInfoButton,
        ui->warningLightsButton,
        ui->wiperBladeReplacement
    };

    bool enableVictory = true;

    for (const auto &button : buttons)
    {
        QString styleSheet = button->styleSheet();
        if (!styleSheet.contains("background-color: green;", Qt::CaseInsensitive))
        {
            enableVictory = false;
        }
    }

    if (enableVictory)
    {
        // enable victory button
        ui->quizButton->setEnabled(true);
        qDebug() << "Victory!";
    }
}

void TableOfContentsForm::on_pushButton_clicked()
{
    ui->quizButton->setEnabled(true);
    setFluidButtonGreen();
    setJumpStartButtonGreen();
    setSpareTireButtonGreen();
    setTirePressureButtonGreen();
    setFluidButtonGreen();
    setWarningLightsButtonGreen();
    setWiperBladeButtonGreen();
}

