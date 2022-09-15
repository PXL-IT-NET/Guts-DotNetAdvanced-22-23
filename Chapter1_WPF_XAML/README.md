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

[img_exercise1_start]:images/exercise1_mainwindow_start.png "MainWindow at start"
[img_exercise1_success]:images/exercise1_mainwindow_success.png "MainWindow when code is broken"
[img_exercise1_failure]:images/exercise1_mainwindow_failure.png "MainWindow after invalid attempt"
[img_exercise1_gameover]:images/exercise1_mainwindow_gamover.png "MainWindow game over"