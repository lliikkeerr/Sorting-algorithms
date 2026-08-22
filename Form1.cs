using System;
using System.Security.Cryptography.X509Certificates;

namespace Sorting_algorithms;

public partial class Form1 : Form
{
    private List<AlgorithmInfo> algorithms = new List<AlgorithmInfo>
    {
        new AlgorithmInfo { Name = "Bogo Sort", SortMethod = SortingFunctions.BogoSort, Note = "" },
        // Add more algorithms here
    };


    public Form1()
    {
        InitializeComponent();
    }
    private void LoadInitializeButton()
    {
        Button Initialize = new Button();

        Initialize.Text = "Initialize";
        Initialize.Name = "btnInitialize";

        Initialize.Location = new Point(40, 40);
        Initialize.Size = new Size(90, 30);

        Initialize.Click += Initialize_Click;

        this.Controls.Add(Initialize);
    }
    private void Form1_Load(object sender, EventArgs e)
    {
        LoadInitializeButton();
    }

    private void Initialize_Click(object? sender, EventArgs e)
    {
        foreach (var algorithm in algorithms)
        {
            Button DynamicButton = new Button();

            DynamicButton.Text = algorithm.Name;
            DynamicButton.Name = "btn" + algorithm.Name.Replace(" ", "");

            DynamicButton.Location = new Point(40, 40 + algorithms.IndexOf(algorithm) * 40);
            DynamicButton.Size = new Size(90, 30);

            DynamicButton.Click += new EventHandler(SortButton_Click);
            DynamicButton.Tag = algorithm.SortMethod;
            this.Controls.Add(DynamicButton);
        }
        Button clickedButton = sender as Button;
        this.Controls.Remove(clickedButton);
        clickedButton.Dispose();
    }
    private void SortButton_Click(object? sender, EventArgs e)
    {
        Button clickedButton = sender as Button;

        foreach (var item in this.Controls)
        {
            if (item is Button button)
            {
                this.Controls.Remove(button);
                button.Dispose();
            }
        }

        //creating textbox and submit button. Submit button has saved the sorting methon in the tag.

        TextBox inputTextBox = new TextBox();

        Label PleadsForNumber = new Label();

        PleadsForNumber.Name = "PleaseTXT";
        PleadsForNumber.Text = "Please enter number of elements in array:";
        PleadsForNumber.Location = new Point(40, 40);
        PleadsForNumber.AutoSize = true;

        this.Controls.Add(PleadsForNumber);

        inputTextBox.Location = new Point(40, 80);
        inputTextBox.Size = new Size(200, 30);

        inputTextBox.Name = "txtInput";
        inputTextBox.Text = "";

        this.Controls.Add(inputTextBox);

        Button submitButton = new Button();
        submitButton.Location = new Point(40, 120);
        submitButton.Size = new Size(90, 30);
        submitButton.Text = "Submit";
        submitButton.Tag = clickedButton.Tag;
        submitButton.Name = "btnSubmit";
        submitButton.Click += SubmitButton_Click; ;
        this.Controls.Add(submitButton);
    }

    private void SubmitButton_Click(object sender, EventArgs e)
    {
        Button clickedButton = sender as Button;
        int size = 1;
        Label ToDelete = this.Controls["PleaseTXT"] as Label;
        this.Controls.Remove(ToDelete);
        ToDelete.Dispose();

        try
        {
            size = int.Parse(this.Controls["txtInput"].Text);
        }
        catch (Exception ex)
        {
            TextBox TB = this.Controls["txtInput"] as TextBox;

            this.Controls.Remove(TB);
            TB.Dispose();

            this.Controls.Remove(clickedButton);
            clickedButton.Dispose();

            MessageBox.Show("You did not enter the number in the correct format");

            LoadInitializeButton();
            return;
        }

        Func<int[], IEnumerable<SortStep>> sortMethod = clickedButton.Tag as Func<int[], IEnumerable<SortStep>>;

        TextBox textBox = this.Controls["txtInput"] as TextBox;

        this.Controls.Remove(textBox);
        textBox.Dispose();

        this.Controls.Remove(clickedButton);
        clickedButton.Dispose();

        //change of plans, the sortin function will return ienumerable steps, that i will read here and display

        //shuffle and display the array

        int[] ToSort = new int[size];

        var rng = new Random();

        for (int i = 0; i < size; i++)
        {
            int j = rng.Next(i + 1);
            int temp = ToSort[i];
            ToSort[i] = ToSort[j];
            ToSort[j] = temp;
        }

        foreach (var step in sortMethod(ToSort))
        {
            
        }
    }
}
public class AlgorithmInfo
{
    public string Name { get; set; }
    public Func<int[], IEnumerable<SortStep>> SortMethod { get; set; }
    public string Note { get; set; } = "";
}
public enum SortType { Compare, Swap, Done}
public class SortStep
{
    public int[] Array { get; set; }
    public SortType SortType { get; set; }
    public int? IndexA { get; set; }
    public int? IndexB { get; set; }
}

class SortingFunctions
{

    public static IEnumerable<SortStep> BogoSort(int[] array)
    {
        Random random = new Random();
        bool IsSorted = false;
        while (!IsSorted)
        {
            for (int i = array.Length - 1; i > 0; i--)
            {
                int j = random.Next(i + 1);
                int temp = array[i];
                array[i] = array[j];
                array[j] = temp;

                yield return new SortStep { Array = array, SortType = SortType.Compare, IndexA = i, IndexB = j };
            }
            IsSorted = true;
            for (int i = 0; i < array.Length - 1; i++)
            {
                yield return new SortStep { Array = array, SortType = SortType.Swap, IndexA = i, IndexB = i + 1 };
                if (array[i] > array[i + 1])
                {
                    IsSorted = false;
                    break;
                }
            }
        }
        yield return new SortStep { Array = array, SortType = SortType.Done };
    }


}
