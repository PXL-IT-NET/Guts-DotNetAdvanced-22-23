# Exercises - Chapter 2 - Language Features

## Exercise 1 - Search Compositions
Create a Console application that searches for music compositions in a list using a keyword.
You can do a Quick Search: this will filter compositions by searching the keyword only in the Title of the composition.
You can also do a Detailed Search: this wil filter the compositions by searching the keyword in the Title and the Description of the composition.
The last option is to filter the compositions by searching a ReleaseYear.

When you start the program, you have to enter a keyword. First the program will do a QuickSearch, then a DetailedSearch en at last a RelaseYearSearch

Create a CompositionSearcher class who contains a SearchMusic method. Pass a method reference (delegate) to this method, so you dan pass each Filter method to this SearchMucic method.
The delegate you have to create for this returns a list of compositions (search result) and takes 2 arguments: an argument of type Composition (music where you will look for the searchstring) and an argument of type string (searchstring)

Create and implement the 3 Search methods in the CompositionFilters class

A Composition has a Title, a Description, a Composer and a ReleaseDate property.

Complete the GetAllCompositions method in the CompositionSearcher by creating a list of Compositions.

![alt text][img_exercise1_output]
 

[img_exercise1_output]:images/exercise1_Program_output.png "Ouptut Program"
