using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Banzai3;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Banzai3Test
{
    [TestClass]
    public class TestCross
    {
        [TestMethod]
        public void TestHistory()
        {
            var cross = new Cross(1, 2);
            cross.SetCell(0, 0, Cross.CellState.Fill);
            cross.SetCell(0, 1, Cross.CellState.Fill);
            cross.HistoryNextStep();
            cross.SetCell(0, 0, Cross.CellState.Dot);
            cross.SetCell(0, 1, Cross.CellState.Dot);
            cross.HistoryNextStep();
            cross.SetCell(0, 0, Cross.CellState.Unknown);
            cross.SetCell(0, 1, Cross.CellState.Unknown);
            cross.HistoryNextStep();

            if (cross.GetCell(0, 0) != Cross.CellState.Unknown || cross.GetCell(0, 1) != Cross.CellState.Unknown)
                throw new Exception();

            cross.HistoryUndo();
            if (cross.GetCell(0, 0) != Cross.CellState.Dot || cross.GetCell(0, 1) != Cross.CellState.Dot)
                throw new Exception();

            cross.HistoryUndo();
            if (cross.GetCell(0, 0) != Cross.CellState.Fill || cross.GetCell(0, 1) != Cross.CellState.Fill)
                throw new Exception();

            cross.HistoryUndo();
            if (cross.GetCell(0, 0) != Cross.CellState.Dot || cross.GetCell(0, 1) != Cross.CellState.Dot)
                throw new Exception();

            cross.HistoryUndo();
            if (cross.GetCell(0, 0) != Cross.CellState.Dot || cross.GetCell(0, 1) != Cross.CellState.Dot)
                throw new Exception();

            cross.HistoryRedo();
            if (cross.GetCell(0, 0) != Cross.CellState.Fill || cross.GetCell(0, 1) != Cross.CellState.Fill)
                throw new Exception();

            cross.HistoryRedo();
            if (cross.GetCell(0, 0) != Cross.CellState.Dot || cross.GetCell(0, 1) != Cross.CellState.Dot)
                throw new Exception();

            cross.HistoryRedo();
            if (cross.GetCell(0, 0) != Cross.CellState.Unknown || cross.GetCell(0, 1) != Cross.CellState.Unknown)
                throw new Exception();
        }

        private void TestCheckLineVal(string sample, string len, string state)
        {
            var results = new Dictionary<Cross.SectionState, char>
            {
                {Cross.SectionState.Right, '+'},
                {Cross.SectionState.Wrong, '-'},
                {Cross.SectionState.InProgress, '?'}
            };
            var source = new Dictionary<char, Cross.CellState>
            {
                {'#', Cross.CellState.Fill},
                {'.', Cross.CellState.Dot},
                {' ', Cross.CellState.Unknown}
            };
            var cross = new Cross(sample.Length, 1);
            for (int i = 0; i < sample.Length; i++)
            {
                cross.SetCell(i, 0, source[sample[i]]);
            }
            cross.Left[0] = new Cross.Line(len.Split().Select(int.Parse));
            cross.CheckLines();

            var res =
                new string(
                    cross.Left[0]
                        .Sections
                        .Select(s => results[s.State])
                        .ToArray());

            if (res != state)
                throw new Exception($"[{sample}] [{len}] [{state}]{Environment.NewLine}==> Wrong [{res}]");
        }

        [TestMethod]
        public void TestCheckLine1()
        {
            TestCheckLineVal("      ", "2 2", "??");
        }

        [TestMethod]
        public void TestCheckLine2()
        {
            TestCheckLineVal("#     ", "2 2", "??");
            TestCheckLineVal(" #    ", "2 2", "??");
            TestCheckLineVal("  #   ", "2 2", "??");
            TestCheckLineVal("   #  ", "2 2", "??");
            TestCheckLineVal("    # ", "2 2", "??");
            TestCheckLineVal("     #", "2 2", "??");
        }

        [TestMethod]
        public void TestCheckLine3()
        {
            TestCheckLineVal("##    ", "2 2", "+?");
            TestCheckLineVal(" ##   ", "2 2", "+?");
            TestCheckLineVal("  ##  ", "2 2", "--");
            TestCheckLineVal("   ## ", "2 2", "?+");
            TestCheckLineVal("    ##", "2 2", "?+");
        }

        [TestMethod]
        public void TestCheckLine4()
        {
            TestCheckLineVal("  ##  ", "2 2", "--");
            TestCheckLineVal("###   ", "2 2", "--");
            TestCheckLineVal(" ###  ", "2 2", "--");
            TestCheckLineVal("  ### ", "2 2", "--");
            TestCheckLineVal("   ###", "2 2", "--");
            TestCheckLineVal("## # #", "2 2", "--");
            TestCheckLineVal("# # ##", "2 2", "--");
            TestCheckLineVal(" # # #", "2 2", "--");
            TestCheckLineVal("# # # ", "2 2", "--");
            TestCheckLineVal("## ###", "2 2", "--");
            TestCheckLineVal("### ##", "2 2", "--");
            TestCheckLineVal("######", "2 2", "--");
        }

        [TestMethod]
        public void TestCheckLine5()
        {
            TestCheckLineVal("## ##", "2 2", "++");
            TestCheckLineVal("## ## ", "2 2", "++");
            TestCheckLineVal("##  ##", "2 2", "++");
            TestCheckLineVal(" ## ##", "2 2", "++");
            TestCheckLineVal(" ## ## ", "2 2", "++");
        }

        [TestMethod]
        public void TestCheckLine6()
        {
            TestCheckLineVal(".     ", "2 2", "??");
            TestCheckLineVal(" .    ", "2 2", "--");
            TestCheckLineVal("  .   ", "2 2", "??");
            TestCheckLineVal("   .  ", "2 2", "??");
            TestCheckLineVal("    . ", "2 2", "--");
            TestCheckLineVal("     .", "2 2", "??");
        }

        [TestMethod]
        public void TestCheckLine7()
        {
            TestCheckLineVal("..    ", "2 2", "--");
            TestCheckLineVal(" ..   ", "2 2", "--");
            TestCheckLineVal("  ..  ", "2 2", "??");
            TestCheckLineVal("   .. ", "2 2", "--");
            TestCheckLineVal("    ..", "2 2", "--");
        }

        [TestMethod]
        public void TestCheckLine8()
        {
            for (int i = 0; i < 5; i++)
                for (int j = 1; j < 5; j++)
                    for (int k = 1; k < 5; k++)
                        for (int l = 0; l < 5; l++)
                        {
                            var space1 = new string('.', i);
                            var space2 = new string('.', j);
                            var space3 = new string('.', k);
                            var space4 = new string('.', l);
                            TestCheckLineVal($"{space1}#{space2}#{space3}#{space4}", "1 1 1", "+++");
                        }
            for (int i = 0; i < 5; i++)
                for (int j = 1; j < 5; j++)
                    for (int k = 1; k < 5; k++)
                        for (int l = 0; l < 5; l++)
                        {
                            var space1 = new string('.', i);
                            var space2 = new string('.', j);
                            var space3 = new string('.', k);
                            var space4 = new string('.', l);
                            TestCheckLineVal($"{space1}#{space2}##{space3}###{space4}", "1 2 3", "+++");
                        }
            for (int i = 0; i < 5; i++)
                for (int j = 1; j < 5; j++)
                    for (int k = 1; k < 5; k++)
                        for (int l = 0; l < 5; l++)
                        {
                            var space1 = new string('.', i);
                            var space2 = new string('.', j);
                            var space3 = new string('.', k);
                            var space4 = new string('.', l);
                            TestCheckLineVal($"{space1}###{space2}##{space3}#{space4}", "3 2 1", "+++");
                        }
            for (int i = 0; i < 5; i++)
                for (int j = 1; j < 5; j++)
                    for (int k = 0; k < 5; k++)
                        for (int l = 1; l < 5; l++)
                            for (int m = 0; m < 5; m++)
                            {
                                var space1 = new string('.', i);
                                var space2 = new string('#', j);
                                var space3 = new string('.', k);
                                var space4 = new string('#', l);
                                var space5 = new string('.', l);
                                TestCheckLineVal($"{space1}{space2}{space3}{space4}{space5}", "1", "-");
                            }
        }

        [TestMethod]
        public void TestCheckLine9()
        {
            for (int i = 0; i < 10; i++)
                for (int j = 1; j < 10; j++)
                {
                    var space1 = new string('.', i);
                    var space2 = new string('.', j);
                    TestCheckLineVal($"{space1}#{space2}", "1", "+");
                }
            for (int i = 0; i < 10; i++)
                for (int j = 1; j < 10; j++)
                {
                    var space1 = new string('.', i);
                    var space2 = new string('.', j);
                    TestCheckLineVal($"{space1}##{space2}", "2", "+");
                }
            for (int i = 0; i < 10; i++)
                for (int j = 1; j < 10; j++)
                {
                    var space1 = new string(' ', i);
                    var space2 = new string('.', j);
                    TestCheckLineVal($"{space1}##{space2}", "2", "+");
                }
            for (int i = 0; i < 10; i++)
                for (int j = 1; j < 10; j++)
                {
                    var space1 = new string('.', i);
                    var space2 = new string(' ', j);
                    TestCheckLineVal($"{space1}##{space2}", "2", "+");
                }
            for (int i = 0; i < 10; i++)
                for (int j = 1; j < 10; j++)
                {
                    var space1 = new string(' ', i);
                    var space2 = new string(' ', j);
                    TestCheckLineVal($"{space1}##{space2}", "2", "+");
                }
            for (int i = 0; i < 10; i++)
                for (int j = 1; j < 10; j++)
                {
                    var space1 = new string('.', i);
                    var space2 = new string('.', j);
                    TestCheckLineVal($"{space1}#{space2}", "2", "-");
                }
        }

        [TestMethod]
        public void TestCheckLine10()
        {
            const int max = 6;
            const int addLen = 40;
            for (int i = 1; i < max; i++)
            {
                TestCheckLineVal(
                    "." + new string(' ', max*2 - 1 + addLen) + ".",
                    string.Join(" ", Enumerable.Repeat("1", i)),
                    string.Join("", Enumerable.Repeat("?", i)));
            }
        }

        [TestMethod]
        public void TestLoadSave()
        {
            var tmpFolder = "Users.0000";
            var tmpName = "Test1";
            var exts = new[] {"banzai", "editor", "solve"};
            var cross = new Cross(10, 10);
            cross.SetCell(0, 0, Cross.CellState.Fill);

            CrossIO.ExportEditor(tmpName, cross);
            var cross0 = CrossIO.Import(tmpFolder, tmpName);
            if (cross0.GetCell(0, 0) != Cross.CellState.Unknown)
                throw new Exception();
            CrossIO.Export(tmpFolder, tmpName, cross);
            var cross1 = CrossIO.Import(tmpFolder, tmpName);
            if (cross1.GetCell(0, 0) != Cross.CellState.Fill)
                throw new Exception();

            var dir =
                $@"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\{Application.CompanyName}\{
                    Application.ProductName}\{tmpFolder}\";
            Array.ForEach(exts, e => File.Delete(dir + tmpName + '.' + e));
            if (!Directory.EnumerateFiles(dir).Any())
                Directory.Delete(dir);
        }
    }
}
