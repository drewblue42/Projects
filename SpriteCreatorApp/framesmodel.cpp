/* Reviewed by:
 *
 */

#include "framesmodel.h"
#include <QImage>

FramesModel::FramesModel(QObject *parent, SpriteSize size)
    : QObject{parent}
    , frames{}
    , activeFrameNumber{0}
{
    switch(size)
    {
        case SpriteSize::SMALL: frameSize =  QSize(16, 16); break;
        case SpriteSize::MEDIUM: frameSize = QSize(32, 32); break;
        case SpriteSize::LARGE: frameSize =  QSize(64, 64);
    }

    frames.emplace_back(frameSize, QImage::Format_ARGB32);
    activeColor = Qt::black;
    frames[0].fill(Qt::transparent);
}

// Slots:

void FramesModel::setActiveFrame(size_t frameNumber)
{
    activeFrameNumber = frameNumber;
    emit requestCanvasUpdate(frames[activeFrameNumber]);
}

void FramesModel::pixelUpdate(int x, int y)
{
    frames[activeFrameNumber].setPixelColor(x, y, activeColor);
    emit requestCanvasUpdate(frames[activeFrameNumber]);

    if (activeFrameNumber == 0)
    {
        emit requestAnimationUpdate(frames[activeFrameNumber]);
    }
}

void FramesModel::colorUpdate(QColor newColor)
{
    activeColor = newColor;
}

void FramesModel::copyActiveFrame()
{
    activeFrameNumber++;
    frames.insert(frames.begin() + activeFrameNumber, frames[activeFrameNumber - 1].copy());
    emit requestCanvasUpdate(frames[activeFrameNumber]);
}

void FramesModel::deleteActiveFrame()
{
    frames.erase(frames.begin() + activeFrameNumber);

    if (activeFrameNumber > 0)
    {
        activeFrameNumber--;
    }
    if (frames.empty())
    {
        frames.emplace_back(frameSize, QImage::Format_ARGB32);
        frames[0].fill(Qt::transparent);
        emit requestAnimationUpdate(frames[0]);
    }
    emit requestCanvasUpdate(frames[activeFrameNumber]);
}

void FramesModel::addFrame()
{
    activeFrameNumber++;
    QImage newImage(frameSize, QImage::Format_ARGB32);
    newImage.fill(Qt::transparent);
    frames.insert(frames.begin() + activeFrameNumber, newImage);
    emit requestCanvasUpdate(newImage);
}

void FramesModel::getFrame(size_t frameIndex)
{
    emit requestAnimationUpdate(frames[frameIndex % frames.size()]);
}

void FramesModel::clear(SpriteSize size)
{
    activeFrameNumber = 0;
    frames.clear();

    switch(size)
    {
    case SpriteSize::SMALL: frameSize =  QSize(16, 16); break;
    case SpriteSize::MEDIUM: frameSize = QSize(32, 32); break;
    case SpriteSize::LARGE: frameSize =  QSize(64, 64);
    }
}

void FramesModel::addEmptyFrame()
{
    frames.emplace_back(frameSize, QImage::Format_ARGB32);
    frames[0].fill(Qt::transparent);
}
