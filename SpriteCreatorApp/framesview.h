#ifndef FRAMESVIEW_H
#define FRAMESVIEW_H

#include <QWidget>
#include <vector>
#include <QPushButton>
#include <QTimer>

using std::vector;


/// Assignment: A8
/// Team Members:
/// Rianna McIntyre, Andrew Winward, Olivia Matvejeva,
/// Trichia Crouch, Ben Pond, and Nick Jarvis.
/// Team Name: Qt Pie Programmers.
/// Github Names:
/// rianlmci, drewblue42, omatvejeva,
/// T-crouch, BPond314, jarvisnc
/// Repo Link: https://github.com/University-of-Utah-CS3505/a8-sprite-editor-f24-jarvisnc
/// @brief this sets up the frames at the bottom of the window, it links the UI and the fames model files.

///Code reviewer: Olivia
namespace Ui {
class FramesView;
}

class FramesView : public QWidget
{
    Q_OBJECT

public:
    explicit FramesView(QWidget *parent = nullptr);
    ~FramesView();

signals:
    void requestSetActiveFrame(size_t);
    void requestGetActiveFrame();
    void requestAddFrame();
    void requestDeleteActiveFrame();
    void requestCopyActiveFrame();
    void requestGetFrame(size_t);


public slots:
    void onAddFrameButtonClicked();

    void onDeleteFrameButtonClicked();

    void onCopyFrameButtonClicked();

    void activeFrameButtonUpdate(QImage);

    void animationUpdate(QImage);

    void resetFramesDisplay();

    void framesUpdate(const std::vector<QImage> &frames, size_t incomingActiveFrameNumber);



private slots:
    void onAnimationCheckBoxCheckStateChanged(const Qt::CheckState &arg1);

    void onFPSSliderValueChanged(int value);

private:
    Ui::FramesView *ui;
    vector<QPushButton*> displayedFrames;
    size_t activeFrameNumber;
    size_t currentPreviewFrame;
    QTimer timer;
    void makeFrameDisplayButton(QPushButton* button);
    void highlightActiveFrame();
    void advancePreviewFrame();
    void framesViewConnections();


};

#endif // FRAMESVIEW_H
