/// Assignment: A9
/// Team Members:
/// Rianna McIntyre, Andrew Winward, Olivia Matvejeva,
/// Trichia Crouch, Ben Pond, and Nick Jarvis.
/// Team Name: Qt Pie Programmers.
/// Github Names:
/// rianlmci, drewblue42, omatvejeva,
/// T-crouch, BPond314, jarvisnc
/// Repo Link: github.com/University-of-Utah-CS3505/a9-edu-app-f24-rianlmci
/// @brief Box2D SceneWidget.

#ifndef SCENEWIDGET_H
#define SCENEWIDGET_H

#include <Box2D/Box2D.h>
#include <QTimer>
#include <QWidget>

///Author: QTPiProgrammers
///Assignment: A9
///This is the Widget that contains the visualization of the box2D Simulation
class SceneWidget : public QWidget
{
    Q_OBJECT

public:
    //QT explict constructor for the SceneWidget
    explicit SceneWidget(QWidget *parent = nullptr);

    //Draws the visualization of the box2D world onto the QImage.
    void paintEvent(QPaintEvent *);

    //Starts the timer that controls the simulation of the box2D world
    void startTimer();

public slots:
    /// @brief updateWorld
    /// updates the world where the box2D simulation lives.
    void updateWorld();

private:
    //The simulated box2D world where the simulated physics objects live.
    b2World world;

    //The collection of physical objects, or bodies, that live in the simulated box2D world
    std::vector<b2Body*> bodies;

    //The timer that controls the start of the box2D simulation
    QTimer timer;

    //The visual element that displays the visual interpritation of what's happening in the box2D world.
    QImage image;
};

#endif // SCENEWIDGET_H
