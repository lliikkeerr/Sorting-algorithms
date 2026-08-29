namespace Sorting_algorithms;

public partial class MainForm : Form
{
    private List<AlgorithmInfo> algorithms = new List<AlgorithmInfo>
    {
        new AlgorithmInfo { Name = "Bogo Sort", SortMethod = SortingFunctions.BogoSort, Note = "" },
        new AlgorithmInfo {Name = "Bubble Sort", SortMethod = SortingFunctions.BubbleSort, Note = ""},
        new AlgorithmInfo {Name = "Shaker Sort", SortMethod = SortingFunctions.ShakerSort, Note = ""},
        new AlgorithmInfo {Name = "Selection Sort", SortMethod = SortingFunctions.SelectSort, Note = ""},
        new AlgorithmInfo {Name = "Double Selection Sort", SortMethod = SortingFunctions.MinMaxSelect, Note = ""},
        new AlgorithmInfo {Name = "Heap Sort", SortMethod = SortingFunctions.HeapSort, Note = ""},
        new AlgorithmInfo {Name = "Merge Sort", SortMethod = SortingFunctions.MergeSort, Note = ""},
        new AlgorithmInfo {Name = "Quick Sort", SortMethod = SortingFunctions.QuickSort, Note = ""},
        new AlgorithmInfo {Name = "Insertion Sort", SortMethod = SortingFunctions.InsertSort, Note = ""},
        // Add more algorithms here
    };


    public MainForm()
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

            int X = 40;
            int Y = 40 + algorithms.IndexOf(algorithm) * 40;

            int NormalizeWith = (this.ClientSize.Height - 30) - (this.ClientSize.Height - 30) % 40;

            if (NormalizeWith == 0)
            {
                NormalizeWith = 40;
            }

            while (Y > this.ClientSize.Height - 30)
            {
                X += 160;
                Y -= NormalizeWith;
            }

            DynamicButton.Location = new Point(X, Y);
            DynamicButton.Size = new Size(150, 30);

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
            Func<int[], IEnumerable<SortStep>> sortMethod = clickedButton.Tag as Func<int[], IEnumerable<SortStep>>;

            TextBox textBox = this.Controls["txtInput"] as TextBox;

            

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
            timer.Disposed += Timer_Disposed;
            timer.Interval = 1;
            timer.Start();

            this.Controls.Remove(textBox);
            textBox.Dispose();

            this.Controls.Remove(clickedButton);
            clickedButton.Dispose();
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

        
    }

    private void Timer_Disposed(object? sender, EventArgs e)
    {
        Button End = new Button();
        End.Size = new Size(120, 30);
        End.Text = "Back to the start";
        End.Name = "btnEnd";
        End.Location = new Point(Width / 2 - 60, Height / 2 - 15);

        End.Click += End_Click; ;

        this.Controls.Add(End);
        End.BringToFront();
    }

    private void End_Click(object? sender, EventArgs e)
    {
        this.Controls.Clear();

        LoadInitializeButton();
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
            timer.Dispose();
        }
    }
}


