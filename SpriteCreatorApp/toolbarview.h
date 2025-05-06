/* Reviewed by Nick Jarvis u0454648
 */

#ifndef TOOLBARVIEW_H
#define TOOLBARVIEW_H

#include <QColor>
#include <QColorDialog>
#include <QPalette>
#include <QQueue>
#include <QToolButton>
#include <QWidget>

///A8  Sprite Editor
///Names: Rianna McIntyre, Andrew Winward, Olivia Matvejeva, Trichia Crouch, Ben Pond, and Nick Jarvis.
///Team Name: Qt Pie Programmers
///Github names: rianlmci, drewblue42, omatvejeva, T-crouch, BPond314, jarvisnc
///Git repo link: https://github.com/University-of-Utah-CS3505/a8-sprite-editor-f24-jarvisnc
/// @brief This header file sets up the tool bar view, it holds the ui and the connections from
/// the colors model and handles display.

namespace Ui
{
    class ToolBarView;
}

class ToolBarView : public QWidget
{
    Q_OBJECT

public:

    ///@brief sets up the tool bar view
    explicit ToolBarView(QWidget *parent = nullptr);

    ///@brief the destructor for the tool bar view
    ~ToolBarView();

signals:

    /// @brief requests the ColorsModel to update its history with a new color
    /// @param newColor
    void requestUpdateColorHistory(QColor newColor);

    /// @brief requests the ColorsModel to update its active color
    /// @param newColor
    void requestColorUpdate(QColor newColor);

public slots:
    /// @brief updates the visualization of the color history in this widget.
    /// @param the data representation of the colors.
    void updateColorHistoryView(QQueue<QColor> colorHistory);

    /// @brief When the button for 'color' is clicked,
    /// a color dialogue box is opened and the color model
    /// and display are updated.
    void onSelectColorClicked();

private slots:

    ///@brief is activated on the pencil button being clicked. This will emit the current color the the model to change the canvas
    void onPencilButtonClicked();

    ///@brief is activated on the eraser button being clicked. This will emit a transparent color to the model to change the canvas
    void onEraserButtonClicked();

    ///@brief is activated on a color button being clicked, it will swap the main color with the button clicked
    void onColor2Clicked();

    ///@brief is activated on a color button being clicked, it will swap the main color with the button clicked
    void onColor3Clicked();

    ///@brief is activated on a color button being clicked, it will swap the main color with the button clicked
    void onColor4Clicked();

    ///@brief is activated on a color button being clicked, it will swap the main color with the button clicked
    void onColor5Clicked();

    ///@brief is activated on a color button being clicked, it will swap the main color with the button clicked
    void onColor6Clicked();

private:

    ///@brief links the UI for the tool bar view
    Ui::ToolBarView *ui;

    ///@brief sets the color buttons to the default colors
    void setColorDisplayToDefault();

    ///@brief sets up the connections to the signal and slots
    void setUpConnections();

    ///@brief swaps the color of the button clicked with the main button
    /// @param main button is the first color
    /// @param the other button which was clicked
    void swapButtonColors(QToolButton* mainButton, QToolButton* otherButton);

    ///@brief this method adds a border with the color of the current color
    /// being used around the pencil button
    void borderPencilTool();

    ///@brief adds a white border around the eraser tool when it is being used
    void borderEraserTool();

    ///@brief hides the border around the pencil tool when it isn't being used
    void hideBorderPencilTool();

    ///@brief hides the border around the eraser tool when it isn't being used
    void hideBorderEraserTool();

    ///@brief adds a broder around the main color
    void borderMainColor();

    ///@brief removes the border around the main color
    void hideBorderMainColor();
};

#endif // TOOLBARVIEW_H
