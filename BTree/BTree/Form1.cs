using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace BTree
{
    public partial class Form1 : Form
    {
        public string workingPath = "";
        public BTree tree;
        public int columnsSize = 0;

        public Form1()
        {
            InitializeComponent();
            openFileDialog1.Filter = "DataBase files (*.db)|*.db|All files (*.*)|*.*";
            saveFileDialog1.Filter = "DataBase files (*.db)|*.db|All files (*.*)|*.*";
        }

        private void CreateNewDB_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                workingPath = saveFileDialog1.FileName;
            }

            tree = new BTree(10);
            TreeViewConverter.ConvertToTreeView(tree, treeView1);
            FileName.Text = "File Selected: " + Path.GetFileName(workingPath);
            columnsSize = File.ReadAllLines(workingPath)[0]
                .Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                .Length;
        }

        private void OpenDB_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK) workingPath = openFileDialog1.FileName;

            tree = new BTree(10);
            using (var sr = new StreamReader(workingPath))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    var columns = line.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(x => int.Parse(x))
                        .ToArray();
                    tree.Insert(columns[0]);
                }
            }

            TreeViewConverter.ConvertToTreeView(tree, treeView1);
            FileName.Text = "File Selected: " + Path.GetFileName(workingPath);
            columnsSize = File.ReadAllLines(workingPath)[0]
                .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Length;
        }

        private void PasrseCommands()
        {
            if (workingPath == "")
            {
                MessageBox.Show("Please, open or create a DataBase");
                Logs.Text += "Please, open or create a DataBase" + Environment.NewLine;
                return;
            }

            var commands = textBox1.Text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (commands.Length == 0)
            {
                MessageBox.Show("Please, enter a command");
                Logs.Text += "Please, enter a command" + Environment.NewLine;
                return;
            }

            switch (commands[0].ToLower())
            {
                case "add":
                {
                    if (!commands[1].ToLower().Equals("values"))
                    {
                        MessageBox.Show("Incorrect syntax");
                        Logs.Text += "Incorrect syntax" + Environment.NewLine;
                        return;
                    }

                    if (commands[2][0] != '(' || commands[2][commands[2].Length - 1] != ')')
                    {
                        MessageBox.Show("Incorrect syntax");
                        Logs.Text += "Incorrect syntax" + Environment.NewLine;
                        return;
                    }

                    commands[2] = commands[2].Trim(new char[] { '(', ')' });
                    var columns = commands[2]
                        .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(x => int.Parse(x))
                        .ToArray();
                    if (columns.Length != columnsSize)
                    {
                        MessageBox.Show("Incorrect number of columns");
                        Logs.Text += "Incorrect number of columns" + Environment.NewLine;
                        return;
                    }

                    tree.Insert(columns[0]);

                    using (var sr = new StreamReader(workingPath))
                    {
                        while (sr.ReadLine() is { } line)
                        {
                            var values = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                            if (values[0]
                                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                    .Select(x => int.Parse(x))
                                    .ToArray()[0] == columns[0])
                            {
                                MessageBox.Show("This key already exists");
                                Logs.Text += "This key already exists" + Environment.NewLine;
                                return;
                            }
                        }
                    }

                    using (var sw = File.AppendText(workingPath))
                    {
                        sw.WriteLine(commands[2]);
                    }

                    Logs.Text += "Added " + commands[2] + Environment.NewLine;
                }
                    break;
                case "delete":
                {
                    var key = -1;
                    if (!int.TryParse(commands[1], out key))
                    {
                        MessageBox.Show("Incorrect syntax");
                        Logs.Text += "Incorrect syntax" + Environment.NewLine;
                        return;
                    }

                    tree.Remove(key);
                    var lines = File.ReadAllLines(workingPath);
                    using (var sw = new StreamWriter(workingPath))
                    {
                        foreach (var line in lines)
                            if (!line.Contains(key.ToString()))
                                sw.WriteLine(line);
                    }

                    Logs.Text += "Deleted " + commands[1] + Environment.NewLine;
                }
                    break;
                case "search":
                {
                    var key = -1;
                    if (!int.TryParse(commands[1], out key) && commands[1].ToLower() != "test")
                    {
                        MessageBox.Show("Incorrect syntax");
                        Logs.Text += "Incorrect syntax" + Environment.NewLine;
                        return;
                    }

                    if (commands[1].ToLower() == "test")
                    {
                        var count = -1;
                        if (!int.TryParse(commands[2], out count))
                        {
                            MessageBox.Show("Incorrect syntax");
                            Logs.Text += "Incorrect syntax" + Environment.NewLine;
                            return;
                        }

                        var time = new TimeSpan();
                        for (var i = 0; i < count; i++)
                        {
                            var rnd = new Random();
                            var start = DateTime.Now;
                            tree.Search(rnd.Next(0, 10000));
                            var end = DateTime.Now;
                            time += end - start;
                        }

                        Logs.Text +=
                            $"Average time for {count} searches: {time.TotalMilliseconds / count} ms{Environment.NewLine}";
                        return;
                    }

                    if (tree.Search(key) != null)
                    {
                        var sr = new StreamReader(workingPath);
                        while (sr.ReadLine() is { } line)
                        {
                            if (line.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                    .Select(int.Parse)
                                    .ToArray()[0] == key)
                            {
                                Logs.Text += "Found key " + key + "; Values " + line + Environment.NewLine;
                                break;
                            }
                        }
                        sr.Close();
                    }
                    else
                    {
                        Logs.Text += "Not found " + commands[1] + Environment.NewLine;
                    }
                }
                    break;
                case "generate":
                {
                    var count = -1;
                    if (!int.TryParse(commands[1], out count))
                    {
                        MessageBox.Show("Incorrect syntax");
                        Logs.Text += "Incorrect syntax" + Environment.NewLine;
                        return;
                    }

                    var rnd = new Random();

                    for (var i = 0; i < count; i++)
                    {
                        var key = rnd.Next(0, 10000);
                        if (tree.root.numberOfKeys == 0)
                        {
                            tree.root = new Node(2);
                            tree.root.keys[0] = key;
                            tree.root.numberOfKeys++;
                            using var sw = File.AppendText(workingPath);
                            sw.WriteLine(key);
                        }

                        else
                        {
                            var foundSame = false;
                            using (var sr = new StreamReader(workingPath))
                            {
                                while (sr.ReadLine() is { } line)
                                {
                                    var values = line.Split(new char[] { ' ' },
                                        StringSplitOptions.RemoveEmptyEntries);
                                    if (values[0]
                                            .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                            .Select(x => int.Parse(x))
                                            .ToArray()[0] == key)
                                    {
                                        Logs.Text += "Key " + key + " already exists" + Environment.NewLine;
                                        i--;
                                        foundSame = true;
                                        break;
                                    }
                                }
                            }

                            if (!foundSame)
                            {
                                using var sw = File.AppendText(workingPath);
                                var line = key.ToString();
                                for (var j = 1; j < columnsSize; j++) line += " " + rnd.Next(0, 10000);

                                sw.WriteLine(line);
                                tree.Insert(key);
                            }
                        }
                    }

                    Logs.Text += "Generated " + count + " lines" + Environment.NewLine;
                    TreeViewConverter.ConvertToTreeView(tree, treeView1);
                }
                    break;
                case "edit":
                {
                    var key = -1;
                    if (!int.TryParse(commands[1], out key))
                    {
                        MessageBox.Show("Incorrect syntax");
                        Logs.Text += "Incorrect syntax" + Environment.NewLine;
                        return;
                    }

                    if (tree.Search(key) == null)
                    {
                        MessageBox.Show("Key not found");
                        Logs.Text += "Key not found" + Environment.NewLine;
                        return;
                    }

                    var lines = File.ReadAllLines(workingPath);
                    using var sw = new StreamWriter(workingPath);
                    foreach (var line in lines)
                        if (line.Contains(key.ToString()))
                            sw.WriteLine(commands[2]);
                        else
                            sw.WriteLine(line);
                }
                    break;
                default:
                    MessageBox.Show("Incorrect syntax");
                    Logs.Text += "Incorrect syntax" + Environment.NewLine;
                    break;
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                PasrseCommands();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PasrseCommands();
        }

        private void Logs_TextChanged(object sender, EventArgs e)
        {
            string[] lines = Logs.Text.Split("\n");
            if (lines.Length > 11)
            {
                string[] newLines = new string[11];
                for (int i = 0; i < 11; i++)
                {
                    newLines[i] = lines[i + 1];
                }

                Logs.Text = string.Join("\n", newLines);
            }
        }
    }

    public class TreeViewConverter
    {
        public static void ConvertToTreeView(BTree tree, TreeView treeView)
        {
            treeView.Nodes.Clear();
            if (tree.root != null)
            {
                TreeNode rootNode = new TreeNode(tree.root.KeysToString());
                treeView.Nodes.Add(rootNode);
                ConvertToTreeView(tree.root, rootNode);
            }
        }

        private static void ConvertToTreeView(Node node, TreeNode treeNode)
        {
            for (int i = 0; i < node.children.Length; i++)
            {
                if (node.children[i] != null)
                {
                    TreeNode childNode = new TreeNode(node.children[i].KeysToString());
                    treeNode.Nodes.Add(childNode);
                    ConvertToTreeView(node.children[i], childNode);
                }
            }
        }
    }
}