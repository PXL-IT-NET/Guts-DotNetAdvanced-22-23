using System.Windows.Controls;

namespace Exercise2;

public interface IBlackBoard
{
    void SubscribeToStudentAdministrationEvents(IStudentAdministration administration, TextBlock outputTextBlock);
}