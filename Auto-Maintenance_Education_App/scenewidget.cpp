/// Assignment: A9
/// Team Members:
/// Rianna McIntyre, Andrew Winward, Olivia Matvejeva,
/// Trichia Crouch, Ben Pond, and Nick Jarvis.
/// Team Name: Qt Pie Programmers.
/// Github Names:
/// rianlmci, drewblue42, omatvejeva,
/// T-crouch, BPond314, jarvisnc
/// Repo Link: github.com/University-of-Utah-CS3505/a9-edu-app-f24-rianlmci
/// @brief Function implementations for Box2D SceneWidget class.

#include <QPainter>
#include "scenewidget.h"

SceneWidget::SceneWidget(QWidget *parent)
    : QWidget(parent)
    , world(b2Vec2(0.0f, 10.0f))
    , timer(this)
    , image(":/images/tire-svg.svg")
{
    image = image.scaled(QSize(20, 20), Qt::KeepAspectRatio, Qt::SmoothTransformation);
    // Define the ground body.
    b2BodyDef groundBodyDef;
    groundBodyDef.position.Set(0.0f, 20.0f);

    // Call the body factory which allocates memory for the ground body
    // from a pool and creates the ground box shape (also from a pool).
    // The body is also added to the world.
    b2Body *groundBody = world.CreateBody(&groundBodyDef);

    // Define the ground box shape.
    b2PolygonShape groundBox;

    // The extents are the half-widths of the box.
    groundBox.SetAsBox(200.0f, 10.0f);

    // Add the ground fixture to the ground body.
    groundBody->CreateFixture(&groundBox, 0.0f);

    int numBodies = 6;

    std::vector<b2BodyDef> bodyDefs;

    for (int i = 0; i < numBodies; i++)
    {
        b2BodyDef bodyDef;
        bodyDef.type = b2_dynamicBody;
        float scale = 20.0f;
        float lastPos = (float) (scale * (i - 1));
        bodyDef.position.Set(lastPos + (float) (scale * i), 4.0f);
        b2Body *body = world.CreateBody(&bodyDef);
        b2PolygonShape dynamicBox;
        dynamicBox.SetAsBox(1.0f, 1.0f);

        // Define the dynamic body fixture.
        b2FixtureDef fixtureDef;
        fixtureDef.shape = &dynamicBox;

        // Set the box density to be non-zero, so it will be dynamic.
        fixtureDef.density = 1.0f;

        // Override the default friction.
        fixtureDef.friction = 0.3f;
        fixtureDef.restitution = 0.9 - (0.01 * i);
        // Add the shape to the body.
        body->CreateFixture(&fixtureDef);
        bodies.push_back(body);
    }

    connect(&timer, &QTimer::timeout, this, &SceneWidget::updateWorld);
}

void SceneWidget::paintEvent(QPaintEvent *)
{
    // Create a painter
    QPainter painter(this);
    for (b2Body *body : bodies)
    {
        b2Vec2 position = body->GetPosition();

        int centerX = (this->size().width()) / 20 - image.width() / 20;
        int centerY = (this->size().height()) / 20 - image.height() / 20;
        //painter.drawImage((int)(position.x*20), (int)(position.y*20), image);
        painter.drawImage((int)(position.x + 2.75*(centerX + image.size().width())) + this->width()/6, (int)(position.y*15 + (centerY + image.size().height())) + this->height()/6,image);
    }

    painter.end();
}

void SceneWidget::updateWorld()
{
    // It is generally best to keep the time step and iterations fixed.
    world.Step(1.0 / 60.0, 6, 2);
    update();
}

void SceneWidget::startTimer(){
    timer.start(10);
}
