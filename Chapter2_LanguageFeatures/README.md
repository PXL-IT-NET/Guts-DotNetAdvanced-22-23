# Exercises - Chapter 2 - Language Features

## Exercise 1 - Search Compositions
Create a Console application that searches for music compositions in a list using a keyword.
You can do a Quick Search: this will filter compositions by searching the keyword only in the Title of the composition.
You can also do a Detailed Search: this wil filter the compositions by searching the keyword in the Title and the Description of the composition.
The last option is to filter the compositions by searching a ReleaseYear.

When you start the program, you have to enter a keyword. First the program will do a QuickSearch, then a DetailedSearch en at last a RelaseYearSearch

The CompositionFilterDelegate is already defined in the CompositionFilterDelegate.cs file.

Complete teh Composition class.

Implement the 3 Search methods in the CompositionFilters class

In the CompositionSearcher class you have to implement the SearchMusic method. Pass a method reference (delegate) to this method, so you can pass each Filter method to this SearchMucic method.
The delegate returns a list of compositions (search result) and takes 2 arguments: an argument of type Composition (music where you will be looking for the searchstring) and an argument of type string (searchstring)

The method GetAllCompositions is given in the CompositionSearcher class and creates and returns a list of Compositions.

![alt text][img_exercise1_output]
 

[img_exercise1_output]:images/exercise1_output.png "Ouptut Program"



## Exercise 2 - Student Administration
Create a WPF application to register new Students. 
![alt text][student_registration]
After filling in the firstname, lastname of the student and the department for which he will be registred, 
the RegisterStudent method adds the student to a list and fires the NewStudentRegistred event. 
The BlackBoard class listens to this event and will add the Student to Blackboard. This will be shown in de textbox.

![alt text][outputAfterRegistration]

[student_registration]:images/Exercise2_MainWindow.png "Student Registration"
[outputAfterRegistration]:images/Exercise2_StudentRegistration.png "Added To Blackboard"
