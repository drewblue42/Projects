/* Code style reviewed by Andrew Winward.
 *
 */
#include "colorsmodel.h"
#include <QQueue>
#include <QColor>
#include <QObject>

ColorsModel::ColorsModel(QObject *parent) : QObject(parent)
{
    colorHistory = getDefaultColors();
}
void ColorsModel::updateColorHistory(QColor newColor)
{
    colorHistory.dequeue();
    colorHistory.enqueue(newColor);
    emit requestUpdateColorHistoryView(colorHistory);
}

QQueue<QColor> ColorsModel::getDefaultColors()
{
    QQueue<QColor> defaultColors;
    defaultColors.enqueue("green");
    defaultColors.enqueue("blue");
    defaultColors.enqueue("yellow");
    defaultColors.enqueue("red");
    defaultColors.enqueue("white");
    defaultColors.enqueue("black");
    return defaultColors;
}

