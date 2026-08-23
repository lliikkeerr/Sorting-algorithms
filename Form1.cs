using System;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

namespace Sorting_algorithms;

public partial class Form1 : Form
{
    private List<AlgorithmInfo> algorithms = new List<AlgorithmInfo>
    {
        new AlgorithmInfo { Name = "Bogo Sort", SortMethod = SortingFunctions.BogoSort, Note = "" },
        new AlgorithmInfo {Name = "Bubble Sort", SortMethod = SortingFunctions.BubbleSort, Note = ""},
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

        var ButtonsToRemove = this.Controls.OfType<Button>().ToList();
        foreach (var button in ButtonsToRemove)
        {
            this.Controls.Remove(button);
            button.Dispose();
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

    private IEnumerator<SortStep> enumerator;
    private VisualizationPanel Panel;
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

        for (int i = 1; i < size + 1; i++)
        {
            ToSort[i - 1] = i;
        }

        var rng = new Random();

        for (int i = 0; i < size; i++)
        {
            int j = rng.Next(i + 1);
            int temp = ToSort[i];
            ToSort[i] = ToSort[j];
            ToSort[j] = temp;
        }

        Panel = new VisualizationPanel();

        Panel.CurrentStep = new SortStep { Array = ToSort, SortType = SortType.Begin };

        Panel.Size = new Size(this.ClientSize.Width, this.ClientSize.Height);
        this.Controls.Add(Panel);
        Panel.Invalidate();

        enumerator = sortMethod(ToSort).GetEnumerator();
        System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();

        timer.Tick += Timer_Tick;
        timer.Interval = 1;
        timer.Start();
    }

    private void Timer_Tick(object? sender, EventArgs e)
    {
        var timer = sender as System.Windows.Forms.Timer;
        if (enumerator.MoveNext())
        {
            Panel.CurrentStep = enumerator.Current;
            Panel.Invalidate();
        }
        else
        {
            timer.Stop();
        }
    }
}
public class VisualizationPanel : Panel
{
    public SortStep CurrentStep { get; set; }
    public VisualizationPanel()
    {
        this.DoubleBuffered = true;
        this.Location = new Point(0, 0);
        this.BackColor = Color.White;
    }
    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);

        int Height = this.ClientSize.Height;
        int Width = this.ClientSize.Width;

        int current = Width / CurrentStep.Array.Length;
        int last = 0;

        using Brush brush = new SolidBrush(Color.Black);

        for (int i = 0; i < CurrentStep.Array.Length; i++)
        {
            var rect = new Rectangle();
            rect.Size = new Size(current - last, Height * CurrentStep.Array[i] / CurrentStep.Array.Length);
            rect.Location = new Point(last, Height - Height * CurrentStep.Array[i] / CurrentStep.Array.Length);

            e.Graphics.FillRectangle(brush, rect);

            last = current;
            current = (i + 2) * Width / CurrentStep.Array.Length;
        }

        switch (CurrentStep.SortType)
        {
            case SortType.Swap:
                {
                    int i = (int)CurrentStep.IndexA;
                    int j = (int)CurrentStep.IndexB;
                    var rect = new Rectangle();
                    rect.Size = new Size(
                        (i + 2) * Width / CurrentStep.Array.Length - (i + 1) * Width / CurrentStep.Array.Length,
                        Height * CurrentStep.Array[i] / CurrentStep.Array.Length);

                    rect.Location = new Point(
                        (i + 1) * Width / CurrentStep.Array.Length,
                        Height - Height * CurrentStep.Array[i] / CurrentStep.Array.Length);

                    var rect2 = new Rectangle();
                    rect2.Size = new Size(
                        (j + 2) * Width / CurrentStep.Array.Length - (j + 1) * Width / CurrentStep.Array.Length,
                        Height * CurrentStep.Array[j] / CurrentStep.Array.Length);

                    rect2.Location = new Point(
                        (j + 1) * Width / CurrentStep.Array.Length,
                        Height - Height * CurrentStep.Array[j] / CurrentStep.Array.Length);

                    {
                        using Brush RedBrush = new SolidBrush(Color.Red);
                        e.Graphics.FillRectangle(RedBrush, rect);
                        e.Graphics.FillRectangle(RedBrush, rect2);
                    }
                }
                break;
            case SortType.Compare:
                {
                    int i = (int)CurrentStep.IndexA;
                    int j = (int)CurrentStep.IndexB;
                    var rect = new Rectangle();
                    rect.Size = new Size(
                        (i + 2) * Width / CurrentStep.Array.Length - (i + 1) * Width / CurrentStep.Array.Length,
                        Height * CurrentStep.Array[i] / CurrentStep.Array.Length);

                    rect.Location = new Point(
                        (i + 1) * Width / CurrentStep.Array.Length,
                        Height - Height * CurrentStep.Array[i] / CurrentStep.Array.Length);

                    var rect2 = new Rectangle();
                    rect2.Size = new Size(
                        (j + 2) * Width / CurrentStep.Array.Length - (j + 1) * Width / CurrentStep.Array.Length,
                        Height * CurrentStep.Array[j] / CurrentStep.Array.Length);

                    rect2.Location = new Point(
                        (j + 1) * Width / CurrentStep.Array.Length,
                        Height - Height * CurrentStep.Array[j] / CurrentStep.Array.Length);

                    {
                        using Brush GreenBrush = new SolidBrush(Color.Green);
                        e.Graphics.FillRectangle(GreenBrush, rect);
                        e.Graphics.FillRectangle(GreenBrush, rect2);
                    }
                }
                break;
            case SortType.Done:
                {
                    using Brush GreenBrush = new SolidBrush(Color.Green);
                    current = Width / CurrentStep.Array.Length;
                    last = 0;
                    for (int i = 0; i < CurrentStep.Array.Length; i++)
                    {
                        var rect = new Rectangle();
                        rect.Size = new Size(current - last, Height * CurrentStep.Array[i] / CurrentStep.Array.Length);
                        rect.Location = new Point(last, Height - Height * CurrentStep.Array[i] / CurrentStep.Array.Length);

                        e.Graphics.FillRectangle(GreenBrush, rect);

                        last = current;
                        current = (i + 2) * Width / CurrentStep.Array.Length;
                    }
                    break;
                }
        }
    }
}
public class AlgorithmInfo
{
    public string Name { get; set; }
    public Func<int[], IEnumerable<SortStep>> SortMethod { get; set; }
    public string Note { get; set; } = "";
}
public enum SortType { Begin, Compare, Swap, Done }
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
            for (int i = 0; i < array.Length; i++)
            {
                int j = random.Next(0, array.Length);
                int temp = array[i];
                array[i] = array[j];
                array[j] = temp;

                yield return new SortStep { Array = array, SortType = SortType.Swap, IndexA = i, IndexB = j };
            }
            IsSorted = true;
            for (int i = 0; i < array.Length - 1; i++)
            {
                yield return new SortStep { Array = array, SortType = SortType.Compare, IndexA = i, IndexB = i + 1 };
                if (array[i] > array[i + 1])
                {
                    IsSorted = false;
                    break;
                }
            }
        }
        yield return new SortStep { Array = array, SortType = SortType.Done };
    }

    public static IEnumerable<SortStep> BubbleSort(int[] array)
    {
        bool IsSorted = false;
        int i = 1;
        while (!IsSorted)
        {
            IsSorted = true;
            for (int j = 0; j < array.Length - i; j++)
            {
                yield return new SortStep { Array = array, SortType = SortType.Compare, IndexA = j, IndexB = j + 1 };
                if (array[j] > array[j + 1])
                {
                    IsSorted = false;
                    int temp = array[j];
                    array[j] = array[j + 1];
                    array[j + 1] = temp;

                    yield return new SortStep { Array = array, SortType = SortType.Swap, IndexA = j, IndexB = j + 1 };
                }
            }
            i++;
        }
        yield return new SortStep { Array = array, SortType = SortType.Done };
    }
}
