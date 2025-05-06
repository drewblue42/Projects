#include <QHBoxLayout>
#include <QTimer>
#include "framesview.h"
#include "ui_framesview.h"

FramesView::FramesView(QWidget *parent)
    : QWidget(parent)
    , ui(new Ui::FramesView)
    , activeFrameNumber{0}
    , currentPreviewFrame{0}
{
    ui->setupUi(this);
    ui->framesScrollArea->setHorizontalScrollBarPolicy(Qt::ScrollBarAlwaysOn);
    ui->framesScrollArea->setVerticalScrollBarPolicy(Qt::ScrollBarAlwaysOff);
    ui->framesScrollArea->setWidgetResizable(true);

    QWidget *frameWidget = new QWidget();
    frameWidget->setLayout(ui->framesDisplayArea);
    ui->framesScrollArea->setWidget(frameWidget);

    QPushButton *button = new QPushButton();
    makeFrameDisplayButton(button);

    QImage newImage(QSize(64, 64), QImage::Format_ARGB32);
    newImage.fill(Qt::transparent);
    QPixmap pixmap = QPixmap::fromImage(newImage);
    ui->animationPreview->setPixmap(pixmap);

    framesViewConnections();

    QTimer::singleShot(100, [this]{emit requestSetActiveFrame(0);});
}

FramesView::~FramesView()
{
    delete ui;
}

void FramesView::onAddFrameButtonClicked()
{
    activeFrameNumber++;
    QPushButton *button = new QPushButton();
    makeFrameDisplayButton(button);
    emit requestAddFrame();
}

void FramesView::onDeleteFrameButtonClicked()
{

    QPushButton *target = displayedFrames[activeFrameNumber];
    ui->framesDisplayArea->removeWidget(target);
    displayedFrames.erase(displayedFrames.begin() + activeFrameNumber);
    delete target;
    if (activeFrameNumber > 0)
    {
        activeFrameNumber--;
    }
    if (displayedFrames.empty())
    {

        QPushButton *button = new QPushButton();
        makeFrameDisplayButton(button);
    }
    highlightActiveFrame();
    emit requestDeleteActiveFrame();

}

void FramesView::onCopyFrameButtonClicked()
{
    activeFrameNumber++;
    QPushButton *button = new QPushButton();
    makeFrameDisplayButton(button);
    emit requestCopyActiveFrame();
}

void FramesView::activeFrameButtonUpdate(QImage image)
{
    QPixmap pixmap = QPixmap::fromImage(image);
    QIcon icon(pixmap.scaled(displayedFrames[activeFrameNumber]->size(), Qt::IgnoreAspectRatio, Qt::FastTransformation));

    displayedFrames[activeFrameNumber]->setIcon(icon);
    displayedFrames[activeFrameNumber]->setIconSize(QSize(displayedFrames[activeFrameNumber]->size()));
}

// Helper methods

void FramesView::makeFrameDisplayButton(QPushButton* button)
{
    size_t frameNumber = activeFrameNumber;
    button->setFixedSize(70, 70);
    ui->framesDisplayArea->insertWidget(frameNumber, button);
    displayedFrames.insert(displayedFrames.begin() + frameNumber, button);


    connect
    (
        button,
        &QPushButton::clicked,
        this,
        [this, frameNumber, button]()
        {
            auto it = std::find(displayedFrames.begin(), displayedFrames.end(), button);
            activeFrameNumber = it - displayedFrames.begin();
            emit requestSetActiveFrame(activeFrameNumber);

            highlightActiveFrame();
        }
    );
    highlightActiveFrame();
}

void FramesView::highlightActiveFrame()
{
    for(auto frame : displayedFrames)
    {
        frame->setStyleSheet("border: 1px solid black; padding: 5px;");
    }
    displayedFrames[activeFrameNumber]->setStyleSheet("border: 4px solid blue; padding: 5px;");
}

void FramesView::onAnimationCheckBoxCheckStateChanged(const Qt::CheckState &state)
{
    currentPreviewFrame = 0;
    if (state == Qt::Checked)
    {
        timer.start(1000 / (ui->fPSSlider->value()));
    }
    else
    {
        timer.stop();
        emit requestGetFrame(0);
    }
}

void FramesView::animationUpdate(QImage image)
{
    QPixmap pixmap = QPixmap::fromImage(image);
    QSize labelSize = ui->animationPreview->size();
    ui->animationPreview->setPixmap(pixmap.scaled(labelSize, Qt::IgnoreAspectRatio, Qt::FastTransformation));
}

void FramesView::advancePreviewFrame()
{
    emit requestGetFrame(currentPreviewFrame);
    currentPreviewFrame++;
}

void FramesView::onFPSSliderValueChanged(int value)
{
    if (ui->animationCheckBox->isChecked())
    {
        timer.stop();
        timer.start(1000 / (value));
    }
}

void FramesView::resetFramesDisplay()
{
    for (auto button : displayedFrames)
    {
        ui->framesDisplayArea->removeWidget(button);
        delete button;
    }

    displayedFrames.clear();
    activeFrameNumber = 0;
    QPushButton *button = new QPushButton();
    makeFrameDisplayButton(button);
    emit requestSetActiveFrame(0);
    emit requestGetFrame(0);
}

void FramesView::framesViewConnections()
{
    connect
        (
            ui->addFrameButton,
            &QPushButton::clicked,
            this,
            &FramesView::onAddFrameButtonClicked
            );

    connect
        (
            ui->copyFrameButton,
            &QPushButton::clicked,
            this,
            &FramesView::onCopyFrameButtonClicked
            );

    connect
        (
            ui->deleteFrameButton,
            &QPushButton::clicked,
            this,
            &FramesView::onDeleteFrameButtonClicked
            );

    connect
        (
            ui->animationCheckBox,
            &QCheckBox::checkStateChanged,
            this,
            &FramesView::onAnimationCheckBoxCheckStateChanged
            );

    connect
        (
            ui->fPSSlider,
            &QSlider::valueChanged,
            this,
            &FramesView::onFPSSliderValueChanged
            );

    connect
        (
            &timer,
            &QTimer::timeout,
            this,
            &FramesView::advancePreviewFrame
            );
}

void FramesView::framesUpdate(const std::vector<QImage> &frames, size_t incomingActiveFrameNumber)
{
    for (auto button : displayedFrames)
    {
        ui->framesDisplayArea->removeWidget(button);
        delete button;
    }

    displayedFrames.clear();
    activeFrameNumber = 0;

    for (auto const &image : frames)
    {
        QPushButton *button = new QPushButton();
        makeFrameDisplayButton(button);
        QPixmap pixmap = QPixmap::fromImage(image);
        QIcon icon(pixmap.scaled(button->size(), Qt::IgnoreAspectRatio, Qt::FastTransformation));

        button->setIcon(icon);
        button->setIconSize(button->size());
        activeFrameNumber++;
    }

    activeFrameNumber = incomingActiveFrameNumber;
    highlightActiveFrame();
    emit requestSetActiveFrame(activeFrameNumber);
    emit requestGetFrame(0);
}
