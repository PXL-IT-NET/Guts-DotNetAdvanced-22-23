# Exercises - Chapter 1 - WPF XAML

## Exercise 1 - Code breaker
Create a WPF application that shows 5 input controls (code keys) and a button.

![alt text][img_exercise1_start]
 
The goal of the app is that the users fill in the 5 keys of a code.
To break the code the user has to fill in the following values:
* 'PXL' in the top left textbox
* 'ForLife' in the top middle passwordbox
* Check the checkbox in the top right
* Select the 3th item in the bottom left dropdown
* Select the 2nd radio button in the bottom middle

When the user clicks the button in the bottom right, a feedback message is displayed at the bottom:

![alt text][img_exercise1_success]

When one of the code keys is wrong, a failure feedback message is displayed. It shows how many attempts are left (you get 5 attempts to break the code).

![alt text][img_exercise1_failure]

When the code is not broken after 5 attempts, a game over message is displayed and the button is disabled.

![alt text][img_exercise1_gameover]

Use the automated test to guide you to the correct solution.
Do as much as possible **in XAML**. Only the code validation logic should be in MainWindow.xaml.cs.

## Exercise 2 - Course info
Create a WPF application that shows some information about the .NET Advanced course.
There are 2 tabs. The first tab shows the chapters of the course:

![alt text][img_exercise2_chapters]
 
The second tab shows some extra info about the course (from the [study guide](https://ibamaflexweb.pxl.be/BMFUIDetailxOLOD.aspx?b=1&c=1&a=79913)):

![alt text][img_exercise2_about_large]

Notice how the background color starts with a color in the top-left corner, gradually changes into another color in the middle and then changes back to the first color in the bottom-right corner.

The text is shown in one TextBlock, even though it contains multiple lines of text.
The automated tests will give you tips on how to achieve this.

When the window gets smaller, a scrollbar is visible to be able to view all the text:

![alt text][img_exercise2_about_small]

Use the automated test to guide you to the correct solution.
Everything should be done **in XAML**.

PS: You can freely chose the colors, paddings and margins you use.

[img_exercise1_start]:images/exercise1_mainwindow_start.png "MainWindow at start"
[img_exercise1_success]:images/exercise1_mainwindow_success.png "MainWindow when code is broken"
[img_exercise1_failure]:images/exercise1_mainwindow_failure.png "MainWindow after invalid attempt"
[img_exercise1_gameover]:images/exercise1_mainwindow_gamover.png "MainWindow game over"

[img_exercise2_chapters]:images/exercise2_mainwindow_chapters.png "MainWindow - Chapters"
[img_exercise2_about_large]:images/exercise2_mainwindow_about_large.png "MainWindow - About (large)"
[img_exercise2_about_small]:images/exercise2_mainwindow_about_small.png "MainWindow - About (small)"