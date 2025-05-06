#include "canvas.h"
#include "mainwindow.h"
#include "ui_mainwindow.h"
#include "tool.h"
#include "toolbarview.h"
#include "colorsmodel.h"
#include <QBuffer>
#include <QByteArray>
#include <QDebug>
#include <QFile>
#include <QFileDialog>
#include <QImage>
#include <QJsonArray>
#include <QJsonDocument>
#include <QJsonObject>
#include <QRandomGenerator>
#include <QVBoxLayout>


MainWindow::MainWindow(QWidget *parent)
    : QMainWindow(parent)
    , ui(new Ui::MainWindow)
    , model{nullptr}
{
    ui->setupUi(this);

    FramesModel* newModel = new FramesModel(nullptr, SpriteSize::LARGE);
    model = newModel;

    // Canvas *canvas = new Canvas(this);
    // ui->gridLayout->addWidget(canvas, 0, 0);
    // ui->gridLayout->addWidget();

    framesViewConnect();
    setUpConnections(colorsModel);
    setUpConnections();

    connect(ui->toolbarView,
            &ToolBarView::requestColorUpdate,
            model,
            &FramesModel::colorUpdate);
}

MainWindow::~MainWindow()
{
    delete ui;
}

void MainWindow::framesViewConnect()
{
    connect(
        ui->framesView,
        &FramesView::requestSetActiveFrame,
        model,
        &FramesModel::setActiveFrame
    );

    connect(
        ui->framesView,
        &FramesView::requestAddFrame,
        model,
        &FramesModel::addFrame
    );

    connect(
        ui->framesView,
        &FramesView::requestCopyActiveFrame,
        model,
        &FramesModel::copyActiveFrame
        );

    connect(
        ui->framesView,
        &FramesView::requestDeleteActiveFrame,
        model,
        &FramesModel::deleteActiveFrame
    );

    connect(
        model,
        &FramesModel::requestCanvasUpdate,
        ui->framesView,
        &FramesView::activeFrameButtonUpdate
    );

    connect(
        ui->framesView,
        &FramesView::requestGetFrame,
        model,
        &FramesModel::getFrame
    );

    connect(
        model,
        &FramesModel::requestAnimationUpdate,
        ui->framesView,
        &FramesView::animationUpdate
    );

    connect(
        this,
        &MainWindow::requestResetFramesDisplay,
        ui->framesView,
        &FramesView::resetFramesDisplay
        );
}

void MainWindow::setUpConnections(ColorsModel &colorsModel)
{
    connect(ui->toolbarView,                            //emitter
            &ToolBarView::requestUpdateColorHistory,    //signal
            &colorsModel,                               //reciever
            &ColorsModel::updateColorHistory            //slot
            );

    connect(&colorsModel,                               //emitter
            &ColorsModel::requestUpdateColorHistoryView,//signal
            ui->toolbarView,                            //reciever
            &ToolBarView::updateColorHistoryView        //slot
            );
}

void MainWindow::setUpConnections(){
    connect(model,
            &FramesModel::requestCanvasUpdate,
            ui->canvas,
            &Canvas::canvasUpdate);

    connect(ui->canvas,
            &Canvas::requestPixelUpdate,
            model,
            &FramesModel::pixelUpdate);

    connect(ui->action16x16,
            &QAction::triggered,
            this,
            &MainWindow::onAction16x16Triggered);

    connect(ui->action32x32,
            &QAction::triggered,
            this,
            &MainWindow::onAction32x32Triggered);

    connect(ui->action64x64,
            &QAction::triggered,
            this,
            &MainWindow::onAction64x64Triggered);

    connect(ui->actionSave_Project,
            &QAction::triggered,
            this,
            &MainWindow::save);

    connect(ui->actionOpen_Project,
            &QAction::triggered,
            this,
            &MainWindow::open);

    connect(model,
            &FramesModel::requestFramesUpdate,
            ui->framesView,
            &FramesView::framesUpdate);
}

void MainWindow::onAction16x16Triggered()
{
    model->clear(SpriteSize::SMALL);
    model->addEmptyFrame();
    emit requestResetFramesDisplay();
}

void MainWindow::onAction32x32Triggered()
{
    model->clear(SpriteSize::MEDIUM);
    model->addEmptyFrame();
    emit requestResetFramesDisplay();
}

void MainWindow::onAction64x64Triggered()
{
    model->clear(SpriteSize::LARGE);
    model->addEmptyFrame();
    emit requestResetFramesDisplay();
}

void MainWindow::save()
{
    QFileDialog dialog = QFileDialog(this);
    dialog.setDefaultSuffix(".json");
    dialog.setAcceptMode(QFileDialog::AcceptSave);
    dialog.setDirectory(".");
    dialog.setNameFilter("All files (*)");

    QString fileName = "not_set";

    if (dialog.exec() == QFileDialog::Accepted)
    {
        QStringList selectedFiles = dialog.selectedFiles();
        fileName = selectedFiles.front();
    }

    //qDebug() << fileName;

    QJsonArray framesArray;
    for (const auto &image : model->frames) {
        QByteArray byteArray;
        QBuffer buffer(&byteArray);
        buffer.open(QIODevice::WriteOnly);
        image.save(&buffer, "PNG");
        framesArray.append(QString::fromLatin1(byteArray.toBase64()));
    }

    QJsonObject jsonObject;
    jsonObject["frames"] = framesArray;
    jsonObject["activeFrameNumber"] = static_cast<int>(model->activeFrameNumber); // Cast because no constructor for size_t
    jsonObject["frameSize"] = model->frameSize.width();

    QJsonDocument jsonDoc(jsonObject);
    QFile file(fileName);

    if (!file.open(QIODevice::WriteOnly)) {
        qDebug() << "Error opening file: " << fileName;
        return;
    }

    QTextStream out(&file);
    out << jsonDoc.toJson(QJsonDocument::Indented);
    file.close();
}

void MainWindow::open()
{
    QString fileName = QFileDialog::getOpenFileName(this, "Open File", "", "JSON Files (*.json)");
    if (!fileName.isEmpty())
    {
        qDebug() << fileName;
    }

    QFile file(fileName);
    if (!file.open(QIODevice::ReadOnly)) {
        qDebug() << "Failed to open file: " << fileName;
        return;
    }

    QByteArray jsonData = file.readAll();
    file.close();

    QJsonDocument jsonDoc = QJsonDocument::fromJson(jsonData);
    QJsonObject jsonObj = jsonDoc.object();

    size_t sizePx = static_cast<size_t>(jsonObj["frameSize"].toInt());

    SpriteSize spriteSize;
    switch (sizePx)
    {
    case 16: spriteSize = SpriteSize::SMALL; break;
    case 32: spriteSize = SpriteSize::MEDIUM; break;
    default: spriteSize = SpriteSize::LARGE;
    }

    model->clear(spriteSize);
    model->activeFrameNumber = static_cast<size_t>(jsonObj["activeFrameNumber"].toInt());

    qDebug() << "activeFrameNumber = " << model->activeFrameNumber;

    QJsonArray imagesArray = jsonObj["frames"].toArray();
    for (const auto &savedImage : imagesArray)
    {
        QString imageString = savedImage.toString();
        QByteArray byteArray = QByteArray::fromBase64(imageString.toLatin1());
        QImage image;
        image.loadFromData(byteArray);
        model->frames.push_back(image);
    }

    QImage activeFrame = model->frames[model->activeFrameNumber];
    emit model->requestCanvasUpdate(activeFrame);
    emit model->requestFramesUpdate(model->frames, model->activeFrameNumber);
}
