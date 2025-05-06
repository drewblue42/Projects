/* Code style reviewed by Nick Jarvis.
 *
 */

#include "colorsmodel.h"
#include "toolbarview.h"
#include "ui_toolbarview.h"
#include <QQueue>
#include <QStack>
#include <QToolButton>

ToolBarView::ToolBarView(QWidget *parent)
    : QWidget(parent)
    , ui(new Ui::ToolBarView)
{
    ui->setupUi(this);
    updateColorHistoryView(ColorsModel::getDefaultColors());
    setUpConnections();
}

ToolBarView::~ToolBarView()
{
    delete ui;
}

void ToolBarView::onSelectColorClicked()
{
    QColor selectedColor = QColorDialog::getColor(Qt::white, this, "choose color");
    if(selectedColor.isValid())
    {
        emit requestUpdateColorHistory(selectedColor);
    }
    onPencilButtonClicked();
}

void ToolBarView::updateColorHistoryView(QQueue<QColor> colorHistory)
{
    QWidget* allColorWidgets [] =
        {
            ui->color6,
            ui->color5,
            ui->color4,
            ui->color3,
            ui->color2,
            ui->color1
        };

    for(QWidget* color : allColorWidgets)
    {
        QString style = QString("background-color: %1").arg(colorHistory.dequeue().name());
        color->setStyleSheet(style);
    }
}

void ToolBarView::setUpConnections()
{
    connect
        (
            ui->selectColor,                        //emitter
            &QPushButton::clicked,                  //signal
            this,                                   //reciever
            &ToolBarView::onSelectColorClicked      //slot
        );

    connect
        (
            ui->color2,                             //emitter
            &QToolButton::clicked,                  //signal
            this,                                   //reciever
            &ToolBarView::onColor2Clicked           //slot
        );

    connect
        (   ui->color3,                             //emitter
            &QToolButton::clicked,                  //signal
            this,                                   //reciever
            &ToolBarView::onColor3Clicked           //slot
        );

    connect
        (
            ui->color4,                             //emitter
            &QToolButton::clicked,                  //signal
            this,                                   //reciever
            &ToolBarView::onColor4Clicked           //slot
        );

    connect
        (
            ui->color5,                             //emitter
            &QToolButton::clicked,                  //signal
            this,                                   //reciever
            &ToolBarView::onColor5Clicked           //slot
        );

    connect
        (   ui->color6,                             //emitter
            &QToolButton::clicked,                  //signal
            this,                                   //reciever
            &ToolBarView::onColor6Clicked           //slot
        );

    connect
        (
            ui->pencilButton,
            &QPushButton::clicked,
            this,
            &ToolBarView::onPencilButtonClicked
        );

    connect
        (
            ui->eraserButton,
            &QPushButton::clicked,
            this,
            &ToolBarView::onEraserButtonClicked
        );
}

void ToolBarView::borderPencilTool()
{
    QPalette palette = ui->color1->palette();
    QColor backgroundColor = palette.color(QPalette::Button);

    QString colorHex = backgroundColor.name();
    ui->pencilButton->setStyleSheet("border: 1px solid " + colorHex + ";");
}

void ToolBarView::borderEraserTool()
{
    ui->eraserButton->setStyleSheet("border: 1px solid pink;");
}

void ToolBarView::hideBorderPencilTool()
{
    ui->pencilButton->setStyleSheet("border: none;");
}

void ToolBarView::hideBorderEraserTool()
{
    ui->eraserButton->setStyleSheet("border: none;");
}

void ToolBarView::onPencilButtonClicked()
{
    hideBorderEraserTool();
    borderPencilTool();

    QPalette palette = ui->color1->palette();
    QColor currentColor = palette.color(QPalette::Window);
    emit requestColorUpdate(currentColor);
}

void ToolBarView::onEraserButtonClicked()
{
    hideBorderPencilTool();
    borderEraserTool();
    emit requestColorUpdate(Qt::transparent);
}

void ToolBarView::onColor2Clicked()
{
    ToolBarView::swapButtonColors(ui->color1, ui->color2);
}

void ToolBarView::onColor3Clicked()
{
     ToolBarView::swapButtonColors(ui->color1, ui->color3);
}

void ToolBarView::onColor4Clicked()
{
     ToolBarView::swapButtonColors(ui->color1, ui->color4);
}

void ToolBarView::onColor5Clicked()
{
     ToolBarView::swapButtonColors(ui->color1, ui->color5);
}

void ToolBarView::onColor6Clicked()
{
     ToolBarView::swapButtonColors(ui->color1, ui->color6);
}

void ToolBarView::swapButtonColors(QToolButton *mainButton, QToolButton *otherButton)
{
    QString mainColor = mainButton->styleSheet();
    QString otherColor = otherButton->styleSheet();

    otherButton->setStyleSheet(mainColor);
    mainButton->setStyleSheet(otherColor);

    onPencilButtonClicked();
}

void ToolBarView::borderMainColor()
{
    QString mainColor = ui->color1->styleSheet();
    ui->color1->setStyleSheet("border: 1px solid white; background-color: " + mainColor + ";");
}

void ToolBarView::hideBorderMainColor()
{
    QPalette palette = ui->color1->palette();
    QColor backgroundColor = palette.color(QPalette::Button);
    ui->color1->setStyleSheet("background-color: " + backgroundColor.name() + ";");
}
