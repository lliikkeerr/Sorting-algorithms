namespace Sorting_algorithms
{
    internal class SortingFunctions
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

                    yield return new SortStep { Array = array, SortType = SortType.Swap, IndexA = i, IndexB = j };

                    int temp = array[i];
                    array[i] = array[j];
                    array[j] = temp;
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
                        yield return new SortStep { Array = array, SortType = SortType.Swap, IndexA = j, IndexB = j + 1 };

                        IsSorted = false;
                        int temp = array[j];
                        array[j] = array[j + 1];
                        array[j + 1] = temp;
                    }
                }
                i++;
            }
            yield return new SortStep { Array = array, SortType = SortType.Done };
        }
        public static IEnumerable<SortStep> HeapSort(int[] array)
        {
            //sestaveni haldy

            for (int z = 0; z < array.Length; z++)
            {
                int temp = array[z];
                int i = z;

                while (i > 0 && temp > array[(i - 1) / 2])
                {
                    yield return new SortStep { Array = array, SortType = SortType.Swap, IndexA = i, IndexB = (i - 1) / 2 };
                    array[i] = array[(i - 1) / 2];
                    i = (i - 1) / 2;
                }
                array[i] = temp;

            }

            //trideni

            for (int z = array.Length - 1; z > 0; z--)
            {
                int temp = array[z];
                yield return new SortStep { Array = array, SortType = SortType.Swap, IndexA = z, IndexB = 0 };
                array[z] = array[0];
                int i = 0;

                while ((2 * i + 1 < z && array[2 * i + 1] > temp) ||
                        (2 * i + 2 < z && array[2 * i + 2] > temp))
                {
                    if (2 * i + 2 < z && array[2 * i + 1] < array[2 * i + 2])
                    {
                        yield return new SortStep { Array = array, SortType = SortType.Swap, IndexA = i, IndexB = 2 * i + 2 };
                        array[i] = array[2 * i + 2];
                        i = 2 * i + 2;
                    }
                    else
                    {
                        yield return new SortStep { Array = array, SortType = SortType.Swap, IndexA = i, IndexB = 2 * i + 1 };
                        array[i] = array[2 * i + 1];
                        i = 2 * i + 1;
                    }
                }
                array[i] = temp;

                yield return new SortStep { Array = array, SortType = SortType.Swap, IndexA = i, IndexB = z };
            }
            yield return new SortStep { Array = array, SortType = SortType.Done };
        }
        public static IEnumerable<SortStep> InsertSort(int[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                int j = i;
                while (j > 0 && array[j] < array[j - 1])
                {
                    yield return new SortStep { Array = array, SortType = SortType.Swap, IndexA = j, IndexB = j - 1 };
                    int temp = array[j];
                    array[j] = array[j - 1];
                    array[j - 1] = temp;
                    j--;
                }
            }
            yield return new SortStep { Array = array, SortType = SortType.Done };
        }
        public static IEnumerable<SortStep> MergeSort(int[] array)
        {
            yield return new SortStep { Array = array, SortType = SortType.Begin };
            int[] auxiliary = new int[array.Length];
            int run = 1;
            while (run < array.Length)
            {
                int first = 0;
                int second = run;

                while (first < array.Length)
                {
                    int IndexFirst = first;
                    int IndexSecond = second;
                    int GlobalIndex = first;
                    while (IndexFirst < first + run && (IndexSecond < second + run && IndexSecond < array.Length))
                    {
                        yield return new SortStep { Array = array, SortType = SortType.Compare, IndexA = IndexFirst, IndexB = IndexSecond };
                        if (array[IndexFirst] > array[IndexSecond])
                        {
                            auxiliary[GlobalIndex] = array[IndexSecond];
                            IndexSecond++;
                            GlobalIndex++;
                        }
                        else
                        {
                            auxiliary[GlobalIndex] = array[IndexFirst];
                            IndexFirst++;
                            GlobalIndex++;
                        }
                    }
                    while (IndexFirst < first + run && IndexFirst < array.Length)
                    {
                        auxiliary[GlobalIndex] = array[IndexFirst];
                        GlobalIndex++;
                        IndexFirst++;
                    }
                    while (IndexSecond < second + run && IndexSecond < array.Length)
                    {
                        auxiliary[GlobalIndex] = array[IndexSecond];
                        GlobalIndex++;
                        IndexSecond++;
                    }
                    first += 2 * run;
                    second += 2 * run;
                }
                for (int i = 0; i < array.Length; i++)
                {
                    array[i] = auxiliary[i];
                    yield return new SortStep { Array = array, SortType = SortType.Swap, IndexA = i, IndexB = i };
                }
                run *= 2;
            }
            yield return new SortStep { Array = array, SortType = SortType.Done };
        }
        public static IEnumerable<SortStep> MinMaxSelect(int[] array)
        {
            for (int i = 0; i < array.Length / 2; i++)
            {
                int min = array[i];
                int minIndex = i;
                int max = array[i];
                int maxIndex = i;
                for (int j = i; j < array.Length - i; j++)
                {
                    yield return new SortStep { Array = array, SortType = SortType.Compare, IndexA = j, IndexB = minIndex };
                    if (array[j] < min)
                    {
                        min = array[j];
                        minIndex = j;
                    }

                    yield return new SortStep { Array = array, SortType = SortType.Compare, IndexA = j, IndexB = maxIndex };
                    if (array[j] > max)
                    {
                        max = array[j];
                        maxIndex = j;
                    }
                }
                yield return new SortStep { Array = array, SortType = SortType.Swap, IndexA = array.Length - 1 - i, IndexB = maxIndex };

                int temp = array[array.Length - 1 - i];
                array[array.Length - 1 - i] = array[maxIndex];
                array[maxIndex] = temp;

                if (minIndex == array.Length - 1 - i)
                {
                    minIndex = maxIndex;
                }

                yield return new SortStep { Array = array, SortType = SortType.Swap, IndexA = minIndex, IndexB = i };

                temp = array[i];
                array[i] = array[minIndex];
                array[minIndex] = temp;
            }
            yield return new SortStep { Array = array, SortType = SortType.Done };
        }
        public static IEnumerable<SortStep> QuickSort(int[] array)
        {
            yield return new SortStep { Array = array, SortType = SortType.Begin };

            var S = new Stack<(int low, int high)>();

            S.Push((0, array.Length - 1));

            while (S.Count > 0)
            {
                int start;
                int end;
                (start, end) = S.Pop();

                if (end - start == 1)
                {
                    yield return new SortStep { Array = array, SortType = SortType.Compare, IndexA = start, IndexB = end };
                    if (array[start] > array[end])
                    {
                        yield return new SortStep { Array = array, SortType = SortType.Swap, IndexA = start, IndexB = end };
                        int temp = array[start];
                        array[start] = array[end];
                        array[end] = temp;
                    }
                    continue;
                }
                else if (end <= start)
                {
                    continue;
                }
                int pivot = array[end];

                int i = start;
                int j = end - 1;

                while (i < j)
                {
                    while (array[i] < pivot && i < end)
                    {
                        yield return new SortStep { Array = array, SortType = SortType.Compare, IndexA = i, IndexB = end };
                        i++;
                    }
                    while (array[j] > pivot && j > start)
                    {
                        yield return new SortStep { Array = array, SortType = SortType.Compare, IndexA = j, IndexB = end };
                        j--;
                    }
                    if (i < j)
                    {
                        yield return new SortStep { Array = array, SortType = SortType.Swap, IndexA = i, IndexB = j };
                        int temp = array[i];
                        array[i] = array[j];
                        array[j] = temp;
                    }
                }

                yield return new SortStep { Array = array, SortType = SortType.Swap, IndexA = i, IndexB = end };
                int temp2 = array[i];
                array[i] = array[end];
                array[end] = temp2;

                S.Push((start, i - 1));
                S.Push((i + 1, end));
            }
            yield return new SortStep { Array = array, SortType = SortType.Done };
        }
        public static IEnumerable<SortStep> SelectSort(int[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                int max = array[0];
                int maxIndex = 0;
                for (int j = 0; j < array.Length - i; j++)
                {
                    yield return new SortStep { Array = array, SortType = SortType.Compare, IndexA = j, IndexB = maxIndex };
                    if (array[j] > max)
                    {
                        max = array[j];
                        maxIndex = j;
                    }
                }
                yield return new SortStep { Array = array, SortType = SortType.Swap, IndexA = array.Length - 1 - i, IndexB = maxIndex };

                int temp = array[array.Length - 1 - i];
                array[array.Length - 1 - i] = max;
                array[maxIndex] = temp;
            }
            yield return new SortStep { Array = array, SortType = SortType.Done };
        }
        public static IEnumerable<SortStep> ShakerSort(int[] array)
        {
            bool IsSorted = false;
            int i = 1;
            while (!IsSorted)
            {
                IsSorted = true;
                for (int j = i - 1; j < array.Length - i; j++)
                {
                    yield return new SortStep { Array = array, SortType = SortType.Compare, IndexA = j, IndexB = j + 1 };
                    if (array[j] > array[j + 1])
                    {
                        yield return new SortStep { Array = array, SortType = SortType.Swap, IndexA = j, IndexB = j + 1 };

                        IsSorted = false;
                        int temp = array[j];
                        array[j] = array[j + 1];
                        array[j + 1] = temp;
                    }
                    yield return new SortStep { Array = array, SortType = SortType.Compare, IndexA = array.Length - 1 - j, IndexB = array.Length - j - 2 };
                    if (array[array.Length - j - 1] < array[array.Length - j - 2])
                    {
                        yield return new SortStep { Array = array, IndexA = array.Length - j - 1, IndexB = array.Length - j - 2 };

                        IsSorted = false;
                        int temp = array[array.Length - j - 1];
                        array[array.Length - j - 1] = array[array.Length - j - 2];
                        array[array.Length - j - 2] = temp;
                    }
                }
                i++;
            }
            yield return new SortStep { Array = array, SortType = SortType.Done };
        }
    }
}