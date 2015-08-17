using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Banzai3.Properties;

namespace Banzai3
{
    public partial class CrossChoiceForm : Form
    {
        private CrossChoiceForm()
        {
            InitializeComponent();
            Name = Localization.GetLocalName(nameof(CrossChoiceForm));
        }

        private string currentDir;
        private string currentFile;

        private static Image _folder;
        private static Image _new;
        private static readonly Font FontButtons = new Font("Arial", 24);

        private void CrossChoiceForm_Load(object sender, EventArgs e)
        {
            if (_folder == null)
                _folder = Resources.icoFolder;
            if (_new == null)
                _new = Resources.icoNew;
            RefreshList(currentDir, false);
        }

        private void RefreshList(string directory, bool isEditorMode)
        {
            //clear and dispose old items
            var disposeItems = panelLibrary.Controls.OfType<Control>().ToList();
            panelLibrary.Controls.Clear();
            disposeItems.ForEach(i => i.Dispose());

            currentDir = directory;
            //add new items
            if (currentDir == null)
            {
                LoadDirectories();
            }
            else
            {
                if (!isEditorMode)
                    LoadFiles();
                else
                    LoadEditor();
            }
        }

        private void LoadFiles()
        {
            var list = CrossIO.GetListFiles(currentDir);
            AddItemToPanel(_folder, "...", null, OnClickBack);
            if (CrossIO.IsUserDir(currentDir))
            {
                AddItemToPanel(_new, Localization.GetLocalName("Create New"), null, OnClickNew);
                AddItemToPanel(_new, Localization.GetLocalName("Edit"), currentDir, OnClickEditor);
            }
            foreach (var fileName in list)
            {
                Cross cross;
                try
                {
                    cross = CrossIO.Import(currentDir, fileName);
                }
                catch (Exception)
                {
                    //TODO log here
                    continue;
                }
                string label = $"{cross.TopSize} x {cross.LeftSize}";
                AddItemToPanel(CrossMakeIcon(cross), label, fileName, OnClickFile);
            }
        }

        private void LoadEditor()
        {
            var list = CrossIO.GetListEditors();
            AddItemToPanel(_folder, "...", null, OnClickBack);
            foreach (var fileName in list)
            {
                Cross cross;
                try
                {
                    cross = CrossIO.ImportEditor(fileName);
                }
                catch (Exception)
                {
                    //TODO log here
                    continue;
                }
                string label = $"{fileName}";
                AddItemToPanel(CrossMakeIcon(cross), label, fileName, OnClickEdit);
            }
        }

        private void LoadDirectories()
        {
            var list = CrossIO.GetListDirs()
                .Select(
                    dir => new
                    {
                        Name = dir,
                        Label = Localization.GetLocalName(Path.GetFileNameWithoutExtension(dir))
                    });

            foreach (var dir in list)
            {
                AddItemToPanel(_folder, dir.Label, dir.Name, OnClickDirectory);
            }
        }

        private static Image CrossMakeIcon(Cross cross)
        {
            var topSize = cross.TopSize;
            var leftSize = cross.LeftSize;

            //TODO remove magic values : 179, 135
            const int minScale = 2;
            const int maxScale = 8;
            var scale = Math.Min(179/topSize, 135/leftSize);
            scale = Math.Min(maxScale, Math.Max(minScale, scale));

            var bmp = new Bitmap(topSize*scale + 1, leftSize*scale + 1);
            using (var g = Graphics.FromImage(bmp))
            {
                for (int i = 0; i < topSize; i++)
                    for (int j = 0; j < leftSize; j++)
                    {
                        Brush brush;
                        switch (cross.GetCell(i, j))
                        {
                            case Cross.CellState.Fill:
                                brush = Brushes.Black;
                                break;
                            case Cross.CellState.Unknown:
                                brush = Brushes.LightGray;
                                break;
                            case Cross.CellState.Dot:
                                brush = Brushes.White;
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                        g.FillRectangle(brush, i*scale + 1, j*scale + 1, scale - 1, scale - 1);
                    }
            }
            return bmp;
        }

        private void AddItemToPanel(Image image, string label, string dir, Action<string> onClick)
        {
            //TODO remove magic values : 180
            var button = new Button
            {
                Image = image, Text = label, Size = new Size(180, 180), TextAlign = ContentAlignment.BottomCenter, TextImageRelation = TextImageRelation.ImageAboveText, Font = FontButtons
            };
            button.Click += (sender, args) => onClick(dir);
            panelLibrary.Controls.Add(button);
        }

        private void OnClickDirectory(string directory)
        {
            RefreshList(directory, false);
        }

        private void OnClickBack(string empty)
        {
            RefreshList(null, false);
        }

        private void OnClickFile(string file)
        {
            currentFile = file;
            DialogResult = DialogResult.OK;
        }

        private void OnClickNew(string empty)
        {
            currentFile = null;
            DialogResult = DialogResult.Yes;
        }

        private void OnClickEditor(string userDir)
        {
            RefreshList(userDir, true);
        }

        private void OnClickEdit(string file)
        {
            currentFile = file;
            DialogResult = DialogResult.No;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns> TODO need refactoring
        /// null - cancel,
        /// couple values - load dir\file,
        /// single value - open editor with new empty user cross in dir
        /// triple values - edit exists user cross
        ///</returns>
        public static string[] SelectCross(string path)
        {
            var form = new CrossChoiceForm
            {
                currentDir = path?.Split(new[] {'\\', '/'}, StringSplitOptions.RemoveEmptyEntries)[0]
            };
            var res = form.ShowDialog();

            //TODO need refactoring
            switch (res)
            {
                case DialogResult.OK:
                    return new[] {form.currentDir, form.currentFile};
                case DialogResult.Yes:
                    return new[] {form.currentDir};
                case DialogResult.No:
                    return new[] { form.currentDir, form.currentFile, "EdiorMode" };
                default:
                    return null;
            }
        }
    }
}
