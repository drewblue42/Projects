/* Code style reviewed by Andrew Winward.
 *
 */
#ifndef COLORSMODEL_H
#define COLORSMODEL_H
#include <QQueue>
#include <QColor>
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
/// @brief A model representing the colors being used to paint with in the sprite editor.
class ColorsModel : public QObject
{
    Q_OBJECT
    public:
        /// @brief A model reresenting the history (maximum of six) of the colors selected by the user.
        explicit ColorsModel(QObject *parent = nullptr);

        /// @brief gets the default colors for the sprite editor
        /// @return  the colors as a queue.
        static QQueue<QColor>getDefaultColors();

    private:
        ///@brief a queue of the color history
        QQueue<QColor> colorHistory;

    public slots:
        /// @brief updates the color history
        void updateColorHistory(QColor newColor);

    signals:
        /// @brief a signal to request the visual component of the color history to update itself
        /// @param colorHistory the model representation of the history
        void requestUpdateColorHistoryView(QQueue<QColor> colorHistory);
};

#endif // COLORSMODEL_H
