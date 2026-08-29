namespace Sorting_algorithms;

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
                        (i + 1) * Width / CurrentStep.Array.Length - i * Width / CurrentStep.Array.Length,
                        Height * CurrentStep.Array[i] / CurrentStep.Array.Length);

                    rect.Location = new Point(
                        i * Width / CurrentStep.Array.Length,
                        Height - Height * CurrentStep.Array[i] / CurrentStep.Array.Length);

                    var rect2 = new Rectangle();
                    rect2.Size = new Size(
                        (j + 1) * Width / CurrentStep.Array.Length - j * Width / CurrentStep.Array.Length,
                        Height * CurrentStep.Array[j] / CurrentStep.Array.Length);

                    rect2.Location = new Point(
                        j * Width / CurrentStep.Array.Length,
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
                        (i + 1) * Width / CurrentStep.Array.Length - i * Width / CurrentStep.Array.Length,
                        Height * CurrentStep.Array[i] / CurrentStep.Array.Length);

                    rect.Location = new Point(
                        i * Width / CurrentStep.Array.Length,
                        Height - Height * CurrentStep.Array[i] / CurrentStep.Array.Length);

                    var rect2 = new Rectangle();
                    rect2.Size = new Size(
                        (j + 1) * Width / CurrentStep.Array.Length - j * Width / CurrentStep.Array.Length,
                        Height * CurrentStep.Array[j] / CurrentStep.Array.Length);

                    rect2.Location = new Point(
                        j * Width / CurrentStep.Array.Length,
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
                    using Brush GreenBrush = new SolidBrush(Color.DeepPink);
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

