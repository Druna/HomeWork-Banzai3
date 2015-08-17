using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Windows.Forms;
using Banzai3.Properties;

namespace Banzai3
{
    public partial class MainWindow : Form
    {
        private bool editorMode = false;
        private Cross cross;
        private readonly Stack<Cross> editorHistoryUndo = new Stack<Cross>();
        private int scaleSize = ScaleStd;

        public MainWindow()
        {
            InitializeComponent();
            cross = new Cross(1, 1);
        }

        #region load/save

        private void Banzai3_Load(object sender, EventArgs e)
        {
            Localization.SetLocalName(this);
            foreach (var btn in toolStrip.Controls.OfType<ToolStripButton>())
            {
                Localization.SetLocalName(btn);
            }
            var dir = Settings.Default.LastDir;
            var file = Settings.Default.LastFile;
            if (!string.IsNullOrWhiteSpace(dir) && !string.IsNullOrWhiteSpace(file))
            {
                try
                {
                    cross = CrossIO.Import(dir, file);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading last save{Environment.NewLine}{ex}");
                }
            }
            UpdateSize();
            UpdateBtnState();
            cross.CheckLines();
            panelScroll.MouseWheel += PanelCross_MouseWheel;
        }

        private void Banzai3_FormClosed(object sender, FormClosedEventArgs e)
        {
            if(!editorMode)
                SaveCurrent();
        }

        #endregion load/save

        #region paint

        private const int ScaleMax = 50;
        private const int ScaleMin = 15;
        private const int ScaleLow = 20;
        private const int ScaleStd = 30;

        private const int LineWidth = 1;
        private const int LineBoldWidth = 3;
        private const float LineMediumWidth = (LineWidth+LineBoldWidth)/2f;
        private const int LineDelta = (LineBoldWidth - LineWidth)/2;
        private const int MapDelta = (LineBoldWidth + 1)/2;
        private const float FontScale = 0.50f;
        private const int PeriodOfBold = 5;
        private const float DotFillSize = 0.25f;

        private void UpdateSize()
        {
            //height 
            int widthAll = GetLeftWidthPx() + GetTopWidthPx() + LineBoldWidth + LineDelta;
            int heightAll = GetTopHeightPx() + GetLeftHeightPx() + LineBoldWidth + LineDelta;
            font?.Dispose();
            font = new Font("Arial", scaleSize*FontScale);
            int x = (panelScroll.Width - widthAll)/2;
            if (editorMode)
                x -= GetLeftWidthPx()/2 - scaleSize*2;
            x = x > 0 ? x : 0;
            int y = (panelScroll.Height - heightAll)/2;
            if (editorMode)
                y -= GetTopHeightPx()/2 - scaleSize*2;
            y = y > 0 ? y : 0;
            panelCross.SetBounds(x, y, widthAll, heightAll);
        }

        private readonly StringFormat textFormat = new StringFormat(StringFormatFlags.NoClip | StringFormatFlags.NoWrap)
        {
            Alignment = StringAlignment.Center,
            LineAlignment = StringAlignment.Far,
            Trimming = StringTrimming.None,
        };

        private static readonly Color LineColor = Color.FromArgb(36, 36, 36);
        private readonly Pen penBold = new Pen(LineColor, LineBoldWidth);
        private readonly Pen penMedium = new Pen(LineColor, LineMediumWidth);
        private readonly Pen pen = new Pen(LineColor, LineWidth);
        private Font font;

        private void panelCross_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            var graphicsState = g.Save();

            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            // position's calc
            var zoneTop = new Rectangle(GetLeftWidthPx(), LineDelta, GetTopWidthPx(), GetTopHeightPx() - LineDelta);
            var zoneLeft = new Rectangle(LineDelta, GetTopHeightPx(), GetLeftWidthPx() - LineDelta, GetLeftHeightPx());
            //var zoneMain = new Rectangle(zoneLeft.X, zoneTop.Y, zoneLeft.Width + zoneTop.Width, zoneLeft.Height + zoneTop.Height);
            var zoneMain = new Rectangle(zoneLeft.Right, zoneTop.Bottom, zoneTop.Width, zoneLeft.Height);

            // draw backround
            g.FillRectangle(Brushes.White, zoneTop);
            g.FillRectangle(Brushes.White, zoneLeft);
            g.FillRectangle(Brushes.White, zoneMain);

            // draw grid
            for (int i = 1; i < cross.TopSize; i++)
            {
                if (i%5 != 0)
                    g.DrawLine(pen,
                        zoneTop.X + SizePx(i) + LineDelta, zoneTop.Y,
                        zoneTop.X + SizePx(i) + LineDelta, zoneLeft.Bottom);
                else
                    g.DrawLine(penBold,
                        zoneTop.X + SizePx(i), zoneTop.Y,
                        zoneTop.X + SizePx(i), zoneLeft.Bottom);
            }
            for (int i = 1; i < cross.MaxCountTop; i++)
            {
                g.DrawLine(pen,
                    zoneTop.X, zoneTop.Y + i*scaleSize + LineDelta,
                    zoneTop.Right, zoneTop.Y + i*scaleSize + LineDelta);
            }

            for (int i = 1; i < cross.LeftSize; i++)
            {
                if (i%5 != 0)
                    g.DrawLine(pen,
                        zoneLeft.X, zoneLeft.Y + SizePx(i) + LineDelta,
                        zoneTop.Right, zoneLeft.Y + SizePx(i) + LineDelta);
                else
                    g.DrawLine(penBold,
                        zoneLeft.X, zoneLeft.Y + SizePx(i),
                        zoneTop.Right, zoneLeft.Y + SizePx(i));
            }
            for (int i = 1; i < cross.MaxCountLeft; i++)
            {
                g.DrawLine(pen,
                    zoneLeft.X + i*scaleSize + LineDelta, zoneLeft.Y,
                    zoneLeft.X + i*scaleSize + LineDelta, zoneLeft.Bottom);
            }

            // draw text
            var maxCountTop = cross.MaxCountTop;
            for (int i = 0; i < cross.TopSize; i++)
            {
                var values = cross.Top[i].Sections;
                var delta = maxCountTop - values.Length;
                for (int j = 0; j < values.Length; j++)
                {
                    var pos = new Rectangle(
                        zoneTop.X + SizePx(i) + LineWidth,
                        zoneTop.Y + (delta + j)*scaleSize + LineDelta,
                        scaleSize, scaleSize);
                    PaintSectionLen(g, pos, values[j]);
                }
            }

            var maxCountLeft = cross.MaxCountLeft;
            for (int i = 0; i < cross.LeftSize; i++)
            {
                var values = cross.Left[i].Sections;
                var delta = maxCountLeft - values.Length;
                for (int j = 0; j < values.Length; j++)
                {
                    var pos = new Rectangle(
                        zoneLeft.X + (delta + j)*scaleSize + LineDelta,
                        zoneLeft.Y + SizePx(i) + LineWidth,
                        scaleSize, scaleSize);
                    PaintSectionLen(g, pos, values[j]);
                }
            }

            //draw map
            for (int i = 0; i < cross.TopSize; i++)
                for (int j = 0; j < cross.LeftSize; j++)
                {
                    var rect = new Rectangle(zoneMain.X + SizePx(i) + MapDelta, zoneMain.Y + SizePx(j) + MapDelta,
                        scaleSize - LineWidth, scaleSize - LineWidth);
                    if (mouseSelection == null || !mouseSelection.IsSected(i, j))
                    {
                        switch (cross.GetCell(i, j))
                        {
                            case Cross.CellState.Fill:
                            {
                                g.FillRectangle(Brushes.Black, rect);
                                break;
                            }
                            case Cross.CellState.Dot:
                            {
                                g.FillRectangle(Brushes.White, rect);
                                if (!editorMode)
                                {
                                    var rect2 =
                                        new RectangleF(
                                            zoneMain.X + SizePx(i) + MapDelta +
                                            (scaleSize - LineWidth)*(1f - DotFillSize)*0.5f,
                                            zoneMain.Y + SizePx(j) + MapDelta +
                                            (scaleSize - LineWidth)*(1f - DotFillSize)*0.5f,
                                            (scaleSize - LineWidth)*DotFillSize, (scaleSize - LineWidth)*DotFillSize);
                                    g.FillRectangle(Brushes.Black, rect2);
                                }
                                break;
                            }
                            case Cross.CellState.Unknown:
                            {
                                g.FillRectangle(Brushes.WhiteSmoke, rect);
                                break;
                            }
                        }
                    }
                    else
                    {
                        switch (mouseSelection.NewCellState)
                        {
                            case Cross.CellState.Fill:
                            {
                                g.FillRectangle(Brushes.DimGray, rect);
                                break;
                            }
                            case Cross.CellState.Dot:
                            {
                                g.FillRectangle(Brushes.White, rect);
                                var rect2 =
                                    new RectangleF(
                                        zoneMain.X + SizePx(i) + MapDelta +
                                        (scaleSize - LineWidth)*(1f - DotFillSize)*0.5f,
                                        zoneMain.Y + SizePx(j) + MapDelta +
                                        (scaleSize - LineWidth)*(1f - DotFillSize)*0.5f,
                                        (scaleSize - LineWidth)*DotFillSize, (scaleSize - LineWidth)*DotFillSize);
                                g.FillRectangle(Brushes.Gray, rect2);
                                break;
                            }
                            case Cross.CellState.Unknown:
                            {
                                g.FillRectangle(Brushes.Azure, rect);
                                break;
                            }
                        }
                    }
                }

            // bold borders
            g.DrawRectangle(penBold, zoneTop);
            g.DrawRectangle(penBold, zoneLeft);
            g.DrawRectangle(penBold, zoneMain);

            g.Restore(graphicsState);
        }

        private void PaintSectionLen(Graphics g, Rectangle pos, Cross.Section section)
        {
            if (section.State == Cross.SectionState.InProgress || editorMode)
            {
                g.DrawString(section.Len.ToString(), font, Brushes.Black, pos, textFormat);
            }
            else if (section.State == Cross.SectionState.Wrong)
            {
                g.DrawString(section.Len.ToString(), font, Brushes.Red, pos, textFormat);
            }
            else if (section.State == Cross.SectionState.Right)
            {
                g.DrawString(section.Len.ToString(), font, Brushes.Black, pos, textFormat);
                var curPen = //penMedium;
                    (scaleSize > ScaleStd)
                        ? penBold
                        : (scaleSize > ScaleLow)
                            ? penMedium
                            : pen;
                g.DrawLine(curPen, pos.X, pos.Bottom, pos.Right, pos.Y);
                //g.DrawString(section.Len.ToString(), font, Brushes.DarkCyan, pos, textFormat);
            }
            else
            {
                throw new ArgumentOutOfRangeException();
            }
        }

        private int SizePx(int top)
        {
            return top*scaleSize + top/PeriodOfBold*(LineBoldWidth - LineWidth);
        }

        private int GetTopWidthPx()
        {
            return SizePx(cross.TopSize) + ((cross.TopSize%PeriodOfBold) == 0 ? LineWidth : LineBoldWidth) - LineWidth;
        }

        private int GetTopHeightPx()
        {
            return cross.MaxCountTop*scaleSize + LineBoldWidth + LineDelta - LineWidth;
        }

        private int GetLeftWidthPx()
        {
            return cross.MaxCountLeft*scaleSize + LineBoldWidth + LineDelta - LineWidth;
        }

        private int GetLeftHeightPx()
        {
            return SizePx(cross.LeftSize) + ((cross.LeftSize%PeriodOfBold) == 0 ? LineWidth : LineBoldWidth) - LineWidth;
        }

        #endregion paint

        #region mouse selection

        private class MouseGroupSelection
        {
            public MouseButtons Button;
            public Cross.CellState NewCellState;
            public Point Begin;
            public Point End;

            public bool IsSected(int x, int y)
            {
                return IsBetweenMirror(x, Begin.X, End.X) && IsBetweenMirror(y, Begin.Y, End.Y);
            }

            private static bool IsBetweenMirror(int value, int begin, int end)
            {
                return IsBetween(value, begin, end) || IsBetween(value, end, begin);
            }

            private static bool IsBetween(int value, int begin, int end)
            {
                return begin <= end && value >= begin && value <= end;
            }
        }

        // mouse values, if mouse [null] selection inactive
        private MouseGroupSelection mouseSelection;

        private void panelCross_MouseDown(object sender, MouseEventArgs e)
        {
            // 1. pressed other button
            // 2. pressed not left or not right button or pressed more than one button
            // then cancel selection
            if ((mouseSelection != null && mouseSelection.Button != e.Button) ||
                (e.Button != MouseButtons.Left && e.Button != MouseButtons.Right))
            {
                mouseSelection = null;
                //TODO partial invalidate need
                panelCross.Invalidate();
                return;
            }

            // mouse position outgoing of the map, ignore
            var pos = GetMousePosition(e.Location);
            if (pos == null)
                return;

            var newCellState = (e.Button == MouseButtons.Left)
                ? Cross.CellState.Fill
                : (e.Button == MouseButtons.Right)
                    ? Cross.CellState.Dot
                    : Cross.CellState.Unknown;

            if (!editorMode)
            {
                // if oldCellState == newCellState then cells must be cleaned
                newCellState = (cross.GetCell(pos.Value.X, pos.Value.Y) == newCellState)
                    ? Cross.CellState.Unknown
                    : newCellState;
            }

            mouseSelection = new MouseGroupSelection
            {
                Button = e.Button,
                NewCellState = newCellState,
                Begin = pos.Value,
                End = pos.Value
            };

            //TODO more partial invalidation need
            panelCross.Invalidate(new Rectangle(e.X - scaleSize, e.Y - scaleSize, scaleSize*2, scaleSize*2));
        }

        private void panelCross_MouseUp(object sender, MouseEventArgs e)
        {
            if (mouseSelection == null)
                return;
            if (mouseSelection.Button != e.Button)
            {
                mouseSelection = null;
                //TODO partial invalidate need
                panelCross.Invalidate();
                return;
            }

            int x1 = Math.Min(mouseSelection.Begin.X, mouseSelection.End.X),
                x2 = Math.Max(mouseSelection.Begin.X, mouseSelection.End.X),
                y1 = Math.Min(mouseSelection.Begin.Y, mouseSelection.End.Y),
                y2 = Math.Max(mouseSelection.Begin.Y, mouseSelection.End.Y);

            for (int i = x1; i <= x2; i++)
                for (int j = y1; j <= y2; j++)
                    cross.SetCell(i, j, mouseSelection.NewCellState);

            cross.HistoryNextStep();
            mouseSelection = null;

            if (!editorMode)
            {
                //TODO need async update
                cross.CheckLines();
            }
            else
            {
                var oldSize = new Size(cross.MaxCountTop, cross.MaxCountLeft);
                cross.CalcLines();
                var newSize = new Size(cross.MaxCountTop, cross.MaxCountLeft);
                if (oldSize != newSize)
                    UpdateSize();
            }

            UpdateBtnState();
            //TODO partial invalidate need
            panelCross.Invalidate();
        }

        private void panelCross_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseSelection == null)
                return;

            if (mouseSelection.Button != e.Button)
            {
                mouseSelection = null;
                //TODO partial invalidate need
                panelCross.Invalidate();
                return;
            }

            var pos = GetMousePosition(e.Location);
            if (pos == null)
                return;
            if (mouseSelection.End == pos.Value)
                return;
            mouseSelection.End = pos.Value;

            //TODO partial invalidate need
            panelCross.Invalidate();
        }

        private Point? GetMousePosition(Point mouse)
        {
            var zoneMain = new Point(GetLeftWidthPx(), GetTopHeightPx());
            int xPixel = mouse.X - zoneMain.X;
            //int x = -1;
            //for (int i = -1; i < cross.TopSize; i++)
            //{
            //    if (xx > SizePx(i) && xx <= SizePx(i + 1))
            //    {
            //        x = i;
            //        break;
            //    }
            //}
            // This code of finding x equivalent to next LINQ variant, i am thinking non-LINQ is bit faster
            int x =
                Enumerable.Range(0, cross.TopSize)
                    .Where(i => xPixel > SizePx(i) && xPixel < SizePx(i + 1))
                    .DefaultIfEmpty(-1)
                    .First();
            if (x < 0)
                return null;

            int yPixel = mouse.Y - zoneMain.Y;
            int y = -1;
            for (int i = -1; i < cross.LeftSize; i++)
            {
                if (yPixel > SizePx(i) && yPixel <= SizePx(i + 1))
                {
                    y = i;
                    break;
                }
            }
            if (y < 0)
                return null;

            return new Point(x, y);
        }

        #endregion mouse selection

        #region buttons

        private void UpdateBtnState()
        {
            btnUndo.Enabled = cross.IsHistoryUndo || (editorMode && editorHistoryUndo.Any());
            btnRedo.Enabled = cross.IsHistoryRedo;
            btnCrop.Visible = editorMode;
            btnAddLeft.Visible = editorMode;
            btnAddRight.Visible = editorMode;
            btnAddTop.Visible = editorMode;
            btnAddDown.Visible = editorMode;
            btnSave.Visible = editorMode;
            btnZoomIn.Enabled = (scaleSize < ScaleMax);
            btnZoomOut.Enabled = (scaleSize > ScaleMin);
            if (editorMode)
            {
                btnCrop.Enabled =
                    cross.LeftSize > 1 && cross.TopSize > 1 &&
                    (cross.Left.First().Empty ||
                     cross.Left.Last().Empty ||
                     cross.Top.First().Empty ||
                     cross.Top.Last().Empty);
                btnAddLeft.Enabled = true;
                btnAddRight.Enabled = true;
                btnAddTop.Enabled = true;
                btnAddDown.Enabled = true;
                btnSave.Enabled = true;
            }
        }

        private void btnUndo_Click(object sender, EventArgs e)
        {
            if (!editorMode || cross.IsHistoryUndo)
            {
                cross.HistoryUndo();
                if (!editorMode)
                {
                    cross.CheckLines();
                }
                else
                {
                    cross.CalcLines();
                    UpdateSize();
                }
                UpdateBtnState();
                panelCross.Invalidate();
            }
            else if (editorMode && editorHistoryUndo.Any())
            {
                cross = editorHistoryUndo.Pop();
                cross.CalcLines();
                UpdateSize();
                UpdateBtnState();
                panelCross.Invalidate();
            }
        }

        private void btnRedo_Click(object sender, EventArgs e)
        {
            cross.HistoryRedo();
            if (!editorMode)
            {
                cross.CheckLines();
            }
            else
            {
                cross.CalcLines();
                UpdateSize();
            }
            UpdateBtnState();
            panelCross.Invalidate();
        }

        #endregion buttons

        private void crossScroll_Resize(object sender, EventArgs e)
        {
            UpdateSize();
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            if(!editorMode)
                SaveCurrent();
            //TODO need cortage
            var newCross = CrossChoiceForm.SelectCross(Settings.Default.LastDir);
            if (newCross == null)
                return;

            if (newCross.Length == 1)
            {
                var size = SelectSizeForm.GetSize(null);
                if (size == null)
                    return;
                var newDir = newCross[0];
                cross = new Cross(size.Value.Width, size.Value.Height);
                Settings.Default.LastDir = newDir;
                Settings.Default.LastFile = "";
                Settings.Default.Save();
                editorMode = true;
            }
            else
            {
                var newDir = newCross[0];
                var newFile = newCross[1];
                try
                {
                    cross = CrossIO.Import(newDir, newFile);
                }
                catch (Exception)
                {
                    MessageBox.Show(Localization.GetLocalName("ERROR_IO"));
                }
                Settings.Default.LastDir = newDir;
                Settings.Default.LastFile = newFile;
                Settings.Default.Save();
                editorMode = false;
            }
            UpdateSize();
            UpdateBtnState();
            cross.CheckLines();
            panelCross.Invalidate();
        }

        #region crop and extend

        private void btnCrop_Click(object sender, EventArgs e)
        {
            if (!editorMode)
                return;
            var newCross = cross.CloneCrop();
            if (cross.LeftSize == newCross.LeftSize && cross.TopSize == newCross.TopSize)
                return;
            editorHistoryUndo.Push(cross);
            cross = newCross;
            cross.CalcLines();
            UpdateSize();
            UpdateBtnState();
            panelCross.Invalidate();
        }

        private void AddLines(int addLeft, int addRight, int addTop, int addDown)
        {
            editorHistoryUndo.Push(cross);
            cross = cross.CloneAdd(addLeft, addRight, addTop, addDown);
            cross.CalcLines();
            UpdateSize();
            UpdateBtnState();
            panelCross.Invalidate();
        }

        private void btnAddLeft_Click(object sender, EventArgs e)
        {
            AddLines(1, 0, 0, 0);
        }

        private void btnAddRight_Click(object sender, EventArgs e)
        {
            AddLines(0, 1, 0, 0);
        }

        private void btnAddTop_Click(object sender, EventArgs e)
        {
            AddLines(0, 0, 1, 0);
        }

        private void btnAddDown_Click(object sender, EventArgs e)
        {
            AddLines(0, 0, 0, 1);
        }

        #endregion crop and extend

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveEditor();
        }

        private DialogResult SaveEditor()
        {
            var dir = Settings.Default.LastDir;
            var oldFile = Settings.Default.LastFile;
            var newFile = SelectNameForm.SelectName(dir, oldFile);
            if (newFile == null)
                return DialogResult.Cancel;
            CrossIO.ExportEditor(newFile, cross);
            Settings.Default.LastFile = newFile;
            Settings.Default.Save();
            return DialogResult.OK;
        }

        private void SaveCurrent()
        {
            try
            {
                var oldDir = Settings.Default.LastDir;
                var oldFile = Settings.Default.LastFile;
                if (!string.IsNullOrEmpty(oldDir) && !string.IsNullOrEmpty(oldFile))
                    CrossIO.Export(oldDir, oldFile, cross);
            }
            catch (Exception)
            {
                MessageBox.Show(Localization.GetLocalName("ERROR_IO"));
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            var check = MessageBox.Show(
                Localization.GetLocalName("Are you sure?"),
                Localization.GetLocalName("Clear"),
                MessageBoxButtons.OKCancel);
            if (check != DialogResult.OK)
                return;
            for (int i = 0; i < cross.TopSize; i++)
                for (int j = 0; j < cross.LeftSize; j++)
                    cross.SetCell(i, j, Cross.CellState.Unknown);
            cross.CheckLines();
            UpdateBtnState();
            panelCross.Invalidate();
        }

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!editorMode)
                return;
            var check = MessageBox.Show(
                Localization.GetLocalName("Save it before exiting?"),
                Localization.GetLocalName("Clear"),
                MessageBoxButtons.YesNoCancel);
            switch (check)
            {

                case DialogResult.Yes:
                    if (SaveEditor() != DialogResult.OK)
                        e.Cancel = true;
                    break;
                case DialogResult.Cancel:
                    e.Cancel = true;
                    break;
            }
        }

        private void PanelCross_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0)
                btnZoomIn_Click(sender, e);
            if (e.Delta < 0)
                btnZoomOut_Click(sender, e);
        }

        private void btnZoomIn_Click(object sender, EventArgs e)
        {
            scaleSize += scaleSize/10;
            if (scaleSize > ScaleMax)
                scaleSize = ScaleMax;
            UpdateSize();
            UpdateBtnState();
            panelCross.Invalidate();
        }

        private void btnZoomOut_Click(object sender, EventArgs e)
        {
            scaleSize -= scaleSize/10;
            if (scaleSize < ScaleMin)
                scaleSize = ScaleMin;
            UpdateSize();
            UpdateBtnState();
            panelCross.Invalidate();
        }
    }
}
