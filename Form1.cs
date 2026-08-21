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

        inputTextBox.Location = new Point(40, 40);
        inputTextBox.Size = new Size(200, 30);

        inputTextBox.Name = "txtInput";
        inputTextBox.Text = "";

        this.Controls.Add(inputTextBox);

        Button submitButton = new Button();
        submitButton.Location = new Point(40, 80);
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
        
        try
        {
            int size = int.Parse(this.Controls["txtInput"].Text);
            object sortMethod = clickedButton.Tag;
        }
        catch (Exception ex)
        {
            TextBox TB = this.Controls["txtInput"] as TextBox;
            
            this.Controls.Remove(TB);
            TB.Dispose();

            this.Controls.Remove(clickedButton);
            clickedButton.Dispose();
            
            LoadInitializeButton();
            return;
        }
        
        TextBox textBox = this.Controls["txtInput"] as TextBox;
        this.Controls.Remove(textBox);
        textBox.Dispose();

        this.Controls.Remove(clickedButton);
        clickedButton.Dispose();
    }
}
public class AlgorithmInfo
{
    public string Name { get; set; }
    public Action<int[]> SortMethod { get; set; }
    public string Note { get; set; } = "";
}
class SortingFunctions
{

    public static void BogoSort(int[] array)
    {
        Random random = new Random();
        while (!IsSorted(array))
        {
            Shuffle(array, random);
        }
    }
    private static bool IsSorted(int[] array)
    {
        for (int i = 0; i < array.Length - 1; i++)
        {
            //TODO
            //here i will colour the two elements being compared
            if (array[i] > array[i + 1])
            {
                return false;
            }
        }
        return true;
    }
    private static void Shuffle(int[] array, Random random)
    {
        for (int i = array.Length - 1; i > 0; i--)
        {
            int j = random.Next(i + 1);
            int temp = array[i];
            array[i] = array[j];
            array[j] = temp;
            //TODO
            //here i will flip the two elements
        }
    }
}
