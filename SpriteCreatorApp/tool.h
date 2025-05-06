#ifndef TOOL_H
#define TOOL_H

#include <QObject>


/// Assignment: A8
/// Team Members:
/// Rianna McIntyre, Andrew Winward, Olivia Matvejeva,
/// Trichia Crouch, Ben Pond, and Nick Jarvis.
/// Team Name: Qt Pie Programmers.
/// Github Names:
/// rianlmci, drewblue42, omatvejeva,
/// T-crouch, BPond314, jarvisnc
/// Repo Link: https://github.com/University-of-Utah-CS3505/a8-sprite-editor-f24-jarvisnc
/// @brief This file sets up the tools that are being used by the tool bar view

class Tool
{
public:

    /// @brief Tool is an parent class and should not be constructed directly.
    /// It is the parent class of the artist's tools for the sprite editor.
    /// An example of a tool would be a Pencil, Eraser, or Eyedropper.
    Tool();

    /// @brief Applies the effect of this tool to the active sprite.
    /// This is the parent version of the tool and should not be
    /// called directly!
    ///virtual void apply();
};

#endif // TOOL_H
