/* Reviewed by:
 *
 */

#ifndef FRAMESMODEL_H
#define FRAMESMODEL_H

#include <QColor>
#include <QImage>
#include <QObject>
#include <QSize>
#include <vector>

enum class SpriteSize
{
    SMALL, MEDIUM, LARGE
};

/// Assignment: A8
/// Team Members:
/// Rianna McIntyre, Andrew Winward, Olivia Matvejeva,
/// Trichia Crouch, Ben Pond, and Nick Jarvis.
/// Team Name: Qt Pie Programmers.
/// Github Names:
/// rianlmci, drewblue42, omatvejeva,
/// T-crouch, BPond314, jarvisnc
/// Repo Link: https://github.com/University-of-Utah-CS3505/a8-sprite-editor-f24-jarvisnc
/// @brief A model representing the framess being used to represent a sprite in the editor.
/// Style review by: Rianna McIntyre
class FramesModel : public QObject
{
    Q_OBJECT
public:
    /// @brief Constructor
    /// @param parent Pointer to QObject which is the parent of this QObject
    /// @param size Represents which of the three sprite sizes is being created.
    explicit FramesModel(QObject *parent = nullptr, SpriteSize size = SpriteSize::SMALL);

    void clear(SpriteSize size);

    friend class MainWindow;

signals:

    /// @brief Emitted any time the FramesModel needs to report
    /// that the currently-active image has changed, and provides it.
    /// @param QImage representing the up-to-date state of the current image.
    void requestCanvasUpdate(QImage);

    /// @brief Emitted any time the FramesModel needs to report that
    /// a different frame has been chosen from the animation controls
    /// @param QImage representing the newly-selected image.
    void requestAnimationUpdate(QImage);

    void requestFramesUpdate(std::vector<QImage> const &frames, size_t activeFrameNumber);

public slots:

    /// @brief Update the color of the given pixel of the active frame with
    /// the currently-active color.
    /// @param x, y Coordinates of pixel to be updated.
    void pixelUpdate(int, int);

    /// @brief Update the currently-active color.
    /// @param New current color.
    void colorUpdate(QColor);

    /// @brief Update the currently-active frame number/index.
    /// @param New current frame index.
    void setActiveFrame(size_t);

    /// @brief After the current frame, create a new frame
    /// which is a copy of the current frame.
    void copyActiveFrame();

    /// @brief Remove the current frame and set the active
    /// frame to the previous.
    void deleteActiveFrame();

    /// @brief After the current frame, create a new blank frame.
    void addFrame();

    /// @brief Triggers a signal to provide the QImage of the current frame.
    /// @param Index of the frame being requested.
    void getFrame(size_t);

private:
    std::vector<QImage> frames;
    size_t activeFrameNumber;

    /// @brief Represents the width/height size of the sprites, and controls
    /// the size of any new QImages that are put in the model.
    QSize frameSize;

    /// @brief Stores the currently-active color for the purpose of
    /// setting a pixel, when the pixelUpdate slot is triggered.
    QColor activeColor;

    /// @brief
    void addEmptyFrame();
};

#endif // FRAMESMODEL_H
