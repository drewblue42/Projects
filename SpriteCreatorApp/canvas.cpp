/*  Author:         Trichia Crouch and Andrew Winward
    Git:            https://github.com/University-of-Utah-CS3505/a8-sprite-editor-f24-jarvisnc.git
    Course:         CS 3505
    Assingment:     A8 Sprite Editor
    Date:           Nov 12, 2024
    Reviewed By:    Benjamin Pond
*/

#include "canvas.h"
#include "ui_canvas.h"
#include <QImage>
#include <QMouseEvent>
#include <QColor>
#include <QVBoxLayout>

Canvas::Canvas(QWidget *parent)
    : QWidget(parent)
    , ui(new Ui::Canvas)
    , defaultImageSize(16)
    , imageHeight(defaultImageSize)
    , imageWidth(defaultImageSize)
{
    ui->setupUi(this);
    ui->imageLabel->setVisible(true);
    ui->imageLabel->setScaledContents(false);
    ui->imageLabel->setStyleSheet("background-color:white");
}

Canvas::~Canvas()
{
    delete ui;
}

void Canvas::canvasUpdate(const QImage &currentSprite)
{
    imageHeight = currentSprite.height();
    imageWidth = currentSprite.width();
    QSize labelSize = ui->imageLabel->size();
    QPixmap pixImage = QPixmap::fromImage(currentSprite);
    ui->imageLabel->setPixmap(pixImage.scaled(labelSize, Qt::IgnoreAspectRatio, Qt::FastTransformation));

}

QPoint Canvas::convertToPixelCoordinates(QPoint widgetCoordinates)
{
    QPoint point(widgetCoordinates.x()*imageWidth/width(),
                 widgetCoordinates.y()*imageHeight/height());
    return point;
}

bool Canvas::isWithinRange(QPoint imageCoordinates)
{
    if(imageCoordinates.x() < 0 || imageCoordinates.x() > imageWidth)
    {
        return false;
    }else if(imageCoordinates.y() < 0 || imageCoordinates.y() > imageHeight)
    {
        return false;
    }
    return true;
}

void Canvas::mousePressEvent(QMouseEvent *event)
{
    QPoint point = event->pos();
    QPoint labelPoint = ui->imageLabel->mapFromParent(point);
    QPoint imageCor = convertToPixelCoordinates(labelPoint);
    if(isWithinRange(imageCor))
    {
        emit requestPixelUpdate(imageCor.x(),imageCor.y());
    }
}

void Canvas::mouseMoveEvent(QMouseEvent *event)
{
    QPoint point = event->pos();
    QPoint labelPoint = ui->imageLabel->mapFromParent(point);
    QPoint imageCor = convertToPixelCoordinates(labelPoint);
    if(isWithinRange(imageCor))
    {
        emit requestPixelUpdate(imageCor.x(),imageCor.y());
    }
}

