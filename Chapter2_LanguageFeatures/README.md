# Exercises - Chapter 2 - Language Features

## Exercise 1 - Search Compositions
Create a Console application that searches for music compositions in a list using a keyword.
You can do a Quick Search: this will filter compositions by searching the keyword only in the Title of the composition.
You can also do a Detailed Search: this wil filter the compositions by searching the keyword in the Title and the Description of the composition.
The last option is to filter the compositions by searching a ReleaseYear.

When you start the program, you have to enter a keyword. First the program will do a QuickSearch, then a DetailedSearch en at last a RelaseYearSearch

The CompositionFilterDelegate is already defined in the CompositionFilterDelegate.cs file.

Complete the Composition class. By running the tests and turning the tests into red, you can discover what code you have to add.

Implement the 3 Search methods in the CompositionFilters class. Again, use the tests to help with the implementation.

In the CompositionSearcher class you have to implement the SearchMusic method. Pass a method reference (delegate) to this method, so you can pass each Filter method to this SearchMucic method.
The delegate returns a list of compositions (search result) and takes 2 arguments: an argument of type Composition (music where you will be looking for the searchstring) and an argument of type string (searchstring)

The method GetAllCompositions is already written in the CompositionSearcher class and creates and returns a list of Compositions.

![alt text][img_exercise1_output]
 

[img_exercise1_output]:images/exercise1_output.png "Ouptut Program"



## Exercise 2 - Student Administration
Create a WPF application to register new Students. 
![alt text][empty_mainwindow]
After filling in the firstname, lastname of the student and the departmentname, clicking the Add student button will register the student. ![alt_text][exercise_mainwindow_withdata] 


<<<<<<< HEAD
[empty_mainwindow]:images/exercise2_mainwindow_empty.png "Student Registration"
=======
[empty_mainwindow]:images/exercise2_mainWindow_empty.png "Student Registration"
>>>>>>> 89fabfa625a9423c867b955285f56a35112104aa
[mainwindow_withdata]:images/exercise2_mainwindow_withdata.png "Register student with data"
