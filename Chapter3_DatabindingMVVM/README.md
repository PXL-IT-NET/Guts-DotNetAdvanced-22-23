# Exercises - Chapter 4 - WPF Databinding

## Exercise 1
In this exercise you will create a window that shows a list of movies and also a form to add a new movie to the list. 

![Main Window](Images/Exercise1_MainWindow.png)

![Main Window - invalid input](Images/Exercise1_ErrorMessage.png)

Het overzicht van movies moet getoond worden met een *ListView* met 3 kolommen: 
* Een kolom met als hoofding **Title**. De breedte van deze kolom past zich aan de inhoud aan.
* Een kolom met als hoofding **Director**. De breedte van deze kolom past zich aan de inhoud aan.
* Een kolom met als hoofding **Release year**. De breedte van deze kolom past zich aan de inhoud aan.

Leest eerst iets meer over de *GridView* in een *ListView*. Bijvoorbeeld op https://learn.microsoft.com/en-us/dotnet/desktop/wpf/controls/gridview-overview .

De rijen in de *ListView* worden gekoppeld aan een collectie van *Movie* objecten. 
De initiële movies worden in de codebehind geladen. Maak hierbij gebruik van de *GetDummyMovies* methode. 
Zorg ervoor met behulp van databinding dat de juiste waarden in de rijen getoon worden. 

Om een nieuwe movie aan te maken, maak je ook gebruik van databinding. Hierbij koppel je de invulvelden aan een achterliggend *Movie* object. 
Na het klikken op de knop wordt de ingevulde movie toegevoegd aan de *ListView*. Dit moet automatisch gebeuren als je de collectie van movies die aan de *ListView* gebonden is, wijzigt. 
Na het toevoegen van een movie moet het ook mogelijk zijn om een volgende movie toe te voegen.

Als er geen title, director of release year > 0 is ingevuld, dan wordt een foutboodschap weergegeven in de daarvoor voorziene *TextBlock*. 
Na het succesvol toevoegen van een movie, wordt een eventuele (vorige) foutboodschap terug gewist.
