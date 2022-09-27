using System.Windows.Controls;

namespace Exercise2
{
    public class BlackBoard
    {
        private readonly TextBlock _outputTextBlock;

        public BlackBoard(TextBlock outputTextBlock)
        {
            _outputTextBlock = outputTextBlock;
            StudentAdministration.Instance.NewStudentRegistered += OnNewStudentRegistered;
        }

        private void OnNewStudentRegistered(object sender, StudentEventArgs e)
        {
            _outputTextBlock.Text += $"Student added by {sender}\n";
            _outputTextBlock.Text += $"{e.Student}\n";
        }
    }
}
