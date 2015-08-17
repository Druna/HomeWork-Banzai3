using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Banzai3
{
    public class Cross
    {
        // Used 'Top' and 'Left', because function named 'GetTopWidth' and 'GextLeftHeight', better than 'GetWidthWidth' and 'GetHeightHeight'

        public enum CellState : byte
        {
            Unknown = 0,
            Dot = 1,
            Fill = 2
        };

        public struct Section
        {
            public readonly int Len;
            public SectionState State;

            public Section(int len)
            {
                Len = len;
                State = SectionState.InProgress;
            }
        }

        public class Line
        {
            public bool IsSolveChecked;
            public readonly Section[] Sections;

            public Line(IEnumerable<int> sections)
            {
                Sections = sections.Select(l => new Section(l)).ToArray();
                IsSolveChecked = false;
            }

            public Line()
            {
                Sections = new[] {new Section(0)};
                IsSolveChecked = false;
            }

            public bool Empty => (Sections.Length == 0) || (Sections.Length == 1 && Sections[0].Len == 0);
        }

        private readonly string name;

        public readonly Line[] Top;
        public readonly Line[] Left;

        private readonly CellState[,] map;

        public int TopSize => Top.Length;
        public int LeftSize => Left.Length;

        public int MaxCountTop => GetMaxCount(Top);
        public int MaxCountLeft => GetMaxCount(Left);

        private static int GetMaxCount(Line[] values)
        {
            return values
                .Select(w => w.Sections.Length)
                .Concat(new[] {1})
                .Max();
        }

        public Cross(int width, int height)
        {
            name = $@"{width} x {height}";
            Top = new Line[width];
            Left = new Line[height];
            for (int i = 0; i < width; i++)
                Top[i] = new Line();
            for (int i = 0; i < height; i++)
                Left[i] = new Line();
            map = new CellState[width, height];
            for (int i = 0; i < width; i++)
                for (int j = 0; j < height; j++)
                    map[i, j] = CellState.Dot;
        }

        /// <summary>
        /// it should be wrapped in try/catch
        /// </summary>
        public Cross(TextReader t)
        {
            // header
            name = t.ReadLine()?.Trim();
            if (string.IsNullOrWhiteSpace(name))
                throw new FormatException("First line have to contains name");
            var s = t.ReadLine();
            if (string.IsNullOrWhiteSpace(s))
                throw new FormatException("Second line must contain 2 spase-separated value: width and height");
            var ss = s.Split((char[]) null, StringSplitOptions.RemoveEmptyEntries);
            if (ss.Length != 2)
                throw new FormatException("Second line must contain 2 spase-separated value: width and height");
            int width = int.Parse(ss[0]);
            int height = int.Parse(ss[1]);
            Top = new Line[width];
            Left = new Line[height];
            map = new CellState[width, height];

            // top block
            if (!string.IsNullOrWhiteSpace(t.ReadLine()))
                throw new FormatException("Line 3 is not empty");
            for (int i = 0; i < width; i++)
            {
                Top[i] = ImportLine(t);
            }

            // left block
            if (!string.IsNullOrWhiteSpace(t.ReadLine()))
                throw new FormatException($"Line {width + 4} is not empty");
            for (int i = 0; i < height; i++)
            {
                Left[i] = ImportLine(t);
            }

            // map (if exists)
            var mapFlag = t.ReadLine();
            if (mapFlag == null)
                return;
            if (!string.IsNullOrWhiteSpace(mapFlag))
                throw new FormatException($"Line {width + height + 5} is not empty");
            for (int i = 0; i < height; i++)
            {
                var mapLine = t.ReadLine();
                if (string.IsNullOrWhiteSpace(mapLine) || mapLine.Length != width)
                    throw new FormatException($"Line {width + height + 6 + i} must contains map line");
                for (int j = 0; j < width; j++)
                {
                    if (!"012".Contains(mapLine[j]))
                        throw new FormatException($"Line {width + height + 6 + i} map containing wrong symbol");
                    map[j, i] = (CellState) (byte) (mapLine[j] - '0');
                }
            }
        }

        private static Line ImportLine(TextReader t)
        {
            var s = t.ReadLine();

            if (s == null)
                return new Line();

            var ss = s.Split((char[]) null, StringSplitOptions.RemoveEmptyEntries);

            if (ss.Length == 0)
                return new Line();

            var lines =
                ss
                    .Select(int.Parse);

            return new Line(lines);
        }

        /// <summary>
        /// it should be wrapped in try/catch
        /// </summary>
        public void Export(StreamWriter stream, bool exportMap)
        {
            stream.WriteLine(name);
            stream.WriteLine("{0} {1}", TopSize, LeftSize);
            stream.WriteLine();
            foreach (var row in Top)
            {
                ExportLine(stream, row);
            }
            stream.WriteLine();
            foreach (var row in Left)
            {
                ExportLine(stream, row);
            }

            if (!exportMap)
                return;

            stream.WriteLine();
            for (int i = 0; i < LeftSize; i++)
            {
                for (int j = 0; j < TopSize; j++)
                {
                    stream.Write((char) ('0' + (byte) map[j, i]));
                }
                stream.WriteLine();
            }
        }

        private static void ExportLine(TextWriter t, Line row)
        {
            if (row != null && row.Sections.Length > 0)
                t.WriteLine(string.Join(" ", row.Sections.Select(l => l.Len)));
            else
                t.WriteLine();
        }

        #region History

        private struct HistoryEvent
        {
            public readonly bool HeadStep;
            public readonly CellState NewValue, OldValue;
            public readonly int X, Y;

            public HistoryEvent(bool headStep, int x, int y, CellState newValue, CellState oldValue)
            {
                X = x;
                Y = y;
                NewValue = newValue;
                OldValue = oldValue;
                HeadStep = headStep;
            }
        }

        private readonly List<HistoryEvent> history = new List<HistoryEvent>(1000);
        private int historyPointer;
        private bool historyIsStepDone = true;

        public void SetCell(int x, int y, CellState value)
        {
            if (map[x, y] == value)
                return;
            if (history.Count > historyPointer)
            {
                history.RemoveRange(historyPointer, history.Count - historyPointer);
            }
            //TODO autoclean history need
            history.Add(new HistoryEvent(historyIsStepDone, x, y, value, map[x, y]));
            historyPointer++;
            map[x, y] = value;
            Top[x].IsSolveChecked = false;
            Left[y].IsSolveChecked = false;
            historyIsStepDone = false;
        }

        public CellState GetCell(int x, int y)
        {
            return map[x, y];
        }

        public void HistoryNextStep()
        {
            historyIsStepDone = true;
        }

        public void HistoryUndo()
        {
            while (historyPointer > 0)
            {
                historyPointer--;
                var pos = history[historyPointer];
                map[pos.X, pos.Y] = pos.OldValue;
                Top[pos.X].IsSolveChecked = false;
                Left[pos.Y].IsSolveChecked = false;
                if (pos.HeadStep)
                    break;
            }
            historyIsStepDone = true;
        }

        public void HistoryRedo()
        {
            while (historyPointer < history.Count)
            {
                var pos = history[historyPointer];
                map[pos.X, pos.Y] = pos.NewValue;
                Top[pos.X].IsSolveChecked = false;
                Left[pos.Y].IsSolveChecked = false;
                historyPointer++;
                if ((historyPointer >= history.Count) ||
                    history[historyPointer].HeadStep)
                    break;
            }
            historyIsStepDone = true;
        }

        public bool IsHistoryUndo => (historyPointer > 0);

        public bool IsHistoryRedo => (historyPointer < history.Count);

        #endregion History

        #region current state

        public enum SectionState : byte
        {
            Wrong = 0,
            Right = 1,
            InProgress = 2
        }

        /// <summary>
        /// There is not <i>'ugly class'</i>
        /// It's the class contains very slow function
        /// So, need three stages to develop the class
        /// 1. Create algorytm, test and fix all bugs
        /// <b>2. It's reafactoring and preparing to re-develop to native C/C++ code</b>
        /// 3. Converting and refactoring to native C/C++
        /// Now, the class is in the <b>second stage</b>
        /// TODO need converting to native C/C++
        /// </summary>
        private class CheckLineClass
        {

            public CheckLineClass(CellState[] cells, int[] sections)
            {
                this.cells = cells;
                this.sections = sections;
                var count = sections.Length;
                positions = new int[count];
                minTailLenght = new int[count];
                for (int i = 0; i < count; i++)
                {
                    minTailLenght[i] = this.sections.Skip(i).Sum() + count - i - 1;
                }
            }

            // source data for recursion
            private readonly CellState[] cells;
            private readonly int[] sections;
            private readonly int[] minTailLenght;

            // results of recursion, begin position of each section
            private readonly int[] positions;

            private delegate void PassResultDelegate();

            private PassResultDelegate passResult;

            // result of recursion, mode 'Check'
            private bool[,] flags;

            // no LINQ - there need maximize speed, then there used only arrays, not used 'yyeld return' and etc
            private void CheckLineRecursion(int section, int pos)
            {
                int len = sections[section];
                //TODO add comments, many comments to this function
                int posLast = cells.Length - minTailLenght[section];
                for (int i = pos; i <= posLast; i++)
                {
                    if (i - 1 >= 0 && cells[i - 1] == CellState.Fill)
                        break;
                    if (i + len < cells.Length && cells[i + len] == CellState.Fill)
                        continue;
                    bool placed = true;
                    for (int j = 0; j < len; j++)
                    {
                        if (cells[i + j] != CellState.Dot)
                            continue;
                        placed = false;
                        break;
                    }
                    if (!placed)
                        continue;
                    // we have found begin position for this section number
                    positions[section] = i;
                    if (section + 1 < sections.Length)
                    {
                        //recursion call
                        CheckLineRecursion(section + 1, i + len + 1);
                    }
                    else
                    {
                        for (int j = i + len; j < cells.Length; j++)
                        {
                            if (cells[j] != CellState.Fill)
                                continue;
                            placed = false;
                            break;
                        }
                        if (placed)
                            // delegate check
                            passResult();
                    }
                }
            }

            private void PassResultCheck()
            {
                //recursion finish point, we found variant, fix it as accepted
                for (int j = 0; j < positions.Length; j++)
                {
                    flags[j, positions[j]] = true;
                }
            }

            public SectionState[] DoCheckLine()
            {
                var count = sections.Length;
                var result = new SectionState[count];

                if (count == 0 || (count == 1 && sections[0] == 0))
                {
                    var state =
                        cells.Any(c => c == CellState.Fill)
                            ? SectionState.Wrong
                            : cells.All(c => c == CellState.Dot)
                                ? SectionState.Right
                                : SectionState.InProgress;
                    result.Fill(state);
                    return result;
                }

                if (cells.All(c => c == CellState.Unknown))
                {
                    result.Fill(SectionState.InProgress);
                    return result;
                }

                flags = new bool[count, cells.Length];
                passResult = PassResultCheck;

                CheckLineRecursion(0, 0);

                for (int i = 0; i < count; i++)
                {
                    int trueCount =
                        flags
                            .AsEnumerableDimension1(i)
                            .Count(f => f);
                    switch (trueCount)
                    {
                        //not founded any position, answer is wrong
                        case 0:
                            result.Fill(SectionState.Wrong);
                            return result;
                        //only one position, is good
                        case 1:
                            //check position of True value
                            int pos =
                                flags
                                    .AsEnumerableDimension1(i)
                                    .TakeWhile(f => !f)
                                    .Count();
                            //check all Fill from position
                            bool allIsFill =
                                cells
                                    .Skip(pos)
                                    .Take(sections[i])
                                    .All(cs => cs == CellState.Fill);

                            result[i] = allIsFill ? SectionState.Right : SectionState.InProgress;
                            break;

                        default:
                            result[i] = SectionState.InProgress;
                            break;
                    }
                }
                return result;
            }
        }

        private static void CheckLine(Line line, CellState[] cells)
        {
            var lines = line.Sections.Select(l => l.Len).ToArray();
            var checkLine = new CheckLineClass(cells, lines);
            var result = checkLine.DoCheckLine();
            for (int i = 0; i < result.Length; i++)
            {
                line.Sections[i].State = result[i];
            }
            line.IsSolveChecked = true;
        }

        public void CheckLines()
        {
            CheckLines(Top, false);
            CheckLines(Left, true);
        }

        private void CheckLines(Line[] side, bool isDimension0)
        {
            for (int i = 0; i < side.Length; i++)
            {
                if (side[i].IsSolveChecked)
                    continue;
                var cells =
                    isDimension0
                        ? map.AsEnumerableDimension0(i).ToArray()
                        : map.AsEnumerableDimension1(i).ToArray();
                CheckLine(side[i], cells);
            }
        }

        #endregion current state

        public void CalcLines()
        {
            for (int i = 0; i < Top.Length; i++)
            {
                Top[i] = new Line(CalcLine(map.AsEnumerableDimension1(i)));
            }
            for (int i = 0; i < Left.Length; i++)
            {
                Left[i] = new Line(CalcLine(map.AsEnumerableDimension0(i)));
            }
        }

        private static IEnumerable<int> CalcLine(IEnumerable<CellState> cells)
        {
            int curLen = 0;
            bool empty = true;
            foreach (var cell in cells)
            {
                if (cell == CellState.Fill)
                {
                    curLen++;
                }
                else if (curLen != 0)
                {
                    yield return curLen;
                    curLen = 0;
                    empty = false;
                }
            }
            if (curLen != 0 || empty)
                yield return curLen;
        }

        public Cross CloneCrop()
        {
            if (Top.All(l => l.Empty))
                return this;
            var newTop = CropGetNewSize(Top);
            var newLeft = CropGetNewSize(Left);

            var newCross = new Cross(newTop.Len, newLeft.Len);

            CropCloneSide(Top, newTop, newCross.Top);
            CropCloneSide(Left, newLeft, newCross.Left);

            for (int i = 0; i < newTop.Len; i++)
                for (int j = 0; j < newLeft.Len; j++)
                    newCross.map[i, j] = map[newTop.Min + i, newLeft.Min + j];
            return newCross;
        }

        private struct CropNewSize
        {
            public readonly int Min;
            public readonly int Len;

            public CropNewSize(int min, int len)
            {
                Min = min;
                Len = len;
            }
        }

        private static CropNewSize CropGetNewSize(Line[] side)
        {
            int min = side.TakeWhile(l => l.Empty).Count();
            int max = side.Reverse().TakeWhile(l => l.Empty).Count();
            int len = side.Length - min - max;
            return new CropNewSize(min, len);
        }

        private static void CropCloneSide(Line[] side, CropNewSize size, Line[] newSide)
        {
            for (int i = 0; i < size.Len; i++)
            {
                newSide[i] = new Line(side[size.Min + i].Sections.Select(s => s.Len));
            }
        }

        private static void CropCloneSide(Line[] side, int pos, Line[] newSide)
        {
            for (int i = 0; i < side.Length; i++)
            {
                newSide[pos + i] = new Line(side[i].Sections.Select(s => s.Len));
            }
        }

        public Cross CloneAdd(int addLeft, int addRight, int addTop, int addDown)
        {
            var newCross = new Cross(TopSize + addLeft + addRight, LeftSize + addTop + addDown);
            CropCloneSide(Top, addLeft, newCross.Top);
            CropCloneSide(Left, addTop, newCross.Left);

            for (int i = 0; i < Top.Length; i++)
                for (int j = 0; j < Left.Length; j++)
                    newCross.map[i + addLeft, j + addTop] = map[i, j];
            return newCross;
        }
    }
}
