/*  Author:         Trichia Crouch and Andrew Winward
    Git:            https://github.com/University-of-Utah-CS3505/a8-sprite-editor-f24-jarvisnc.git
    Course:         CS 3505
    Assingment:     A8 Sprite Editor
    Date:           Nov 12, 2024
    Reviewed By:    Benjamin Pond
*/

#ifndef CANVAS_H
#define CANVAS_H

#include <QWidget>
#include <QImage>
#include <QMouseEvent>

namespace Ui {
class Canvas;
}

/// Assignment: A8
/// Team Members:
/// Rianna McIntyre, Andrew Winward, Olivia Matvejeva,
/// Trichia Crouch, Ben Pond, and Nick Jarvis.
/// Team Name: Qt Pie Programmers.
/// Github Names:
/// rianlmci, drewblue42, omatvejeva,
/// T-crouch, BPond314, jarvisnc
/// Repo Link: https://github.com/University-of-Utah-CS3505/a8-sprite-editor-f24-jarvisnc
/// @brief The Canvas class is a custom widget that is used to display
/// and modify a sprite's pixels. It handles the display of the current
/// sprite and allows the user to interact with it using mouse events.
class Canvas : public QWidget
{
    Q_OBJECT

public slots:
    /// @brief CanvasUpdate - Called anytime the image to be displayed,
    /// current sprite, is updated.
    /// @param currentSprite - Image to be displayed.
    void canvasUpdate(const QImage &currentSprite);

signals:
    /// @brief requestSpriteUpdate - Signal to update pixels of QImage at location.
    /// To be emitted anytime mouse events on widget occur.
    /// @param xPixel - X corrdinate of pixel to be updated.
    /// @param yPixel - Y corrdinate of pixel to be updated.
    void requestPixelUpdate(int xPixel, int yPixel);

public:
    /// @brief Canvas - Constructs a new Canvas object.
    /// @param parent - The parent widget of the Canvas, defaults to nullptr.
    explicit Canvas(QWidget *parent = nullptr);

    ~Canvas();

    /// @brief ui - Canvas user interface
    Ui::Canvas *ui;


private:
    /// @brief defaultImageSize
    const int defaultImageSize;

    /// @brief imageHeight - Height of current image.
    int imageHeight;

    /// @brief imageWidth - Width of current image.
    int imageWidth;

    /// @brief convertToPixelCoordinates - Converts widget coordinates to QImage pixel
    /// coordinates.
    /// @param widgetCoordinates - x and y.
    /// @return Image coordinates - x and y.
    QPoint convertToPixelCoordinates(QPoint widgetCoordinates);

    /// @brief isWithinRange - Check is image coordinates are valid.
    /// @param imageCoordinates - x and y.
    /// @return true if coordinates are on image plane.
    bool isWithinRange(QPoint imageCoordinates);

protected:
    /// @brief mousePressEvent - Emits requestPixelUpdate when mouse is clicked.
    /// @param event - contains the widget coordinates.
    void mousePressEvent(QMouseEvent *event) override;

    /// @brief mouseMoveEvent - Emits requestPixelUpdate when mouse is clicked and drag
    /// on the widget.
    /// @param event - contains the widget coordinates.
    void mouseMoveEvent(QMouseEvent *event) override;
};

#endif // CANVAS_H
