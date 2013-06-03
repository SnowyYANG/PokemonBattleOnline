using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using NetworkLib.Utilities;
using System.Text.RegularExpressions;
using System.CodeDom.Compiler;
using System.IO;
namespace PokemonBattle.RoomServer
{
    public partial class RoomCoderForm : Form
    {
        public RoomCoderForm()
        {
            InitializeComponent();
        }

        private void RoomCoder_Load(object sender, EventArgs e)
        {
            Icon = Properties.Resources.PokemonBall;
            if (CodeSetting == null || CodeSetting.Files == null)
            {
                CodeSetting = new RoomCodeSetting();
                CodeSetting.Files = new List<string>();
                LanguageCombo.SelectedIndex = 0;
                SourceCombo.SelectedIndex = 0;
            }
            else
            {
                UseCheck.Checked = CodeSetting.UseRoomCoder;
                LanguageCombo.Text = CodeSetting.Language;
                ClassText.Text = CodeSetting.EntranceClass;
                ReferenceText.Text = CodeSetting.References;
                SourceCombo.SelectedIndex = CodeSetting.Source;
                foreach (string file in CodeSetting.Files)
                {
                    FileList.Items.Add(file);
                }
            }
            string codePath = Path.Combine(Application.StartupPath, "Code\\code.txt");
            if (File.Exists(codePath))
            {
                CodeText.Text = File.ReadAllText(codePath);
            }
            else
            {
                string dirPath = Path.Combine(Application.StartupPath, "Code");
                if (!Directory.Exists(dirPath)) Directory.CreateDirectory(dirPath);
                FileStream codeFile = File.Create(codePath);
                codeFile.Close();
            }
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            try
            {
                Save();
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message, "保存文件失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            DialogResult = DialogResult.OK;
            Close();
        }
        private void ComplieButton_Click(object sender, EventArgs e)
        {
            if (UseCheck.Checked && !IsEmptyString(ClassText.Text) && LanguageCombo.SelectedIndex != -1)
            {
                try
                {
                    Compile();
                }
                catch (Exception error)
                {
                    MessageBox.Show(error.Message, "编译代码错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else
            {
                if (!UseCheck.Checked) RoomCodeHelper.Unload();
            }
        }

        private void Save()
        {
            CodeSetting.UseRoomCoder = UseCheck.Checked;
            CodeSetting.EntranceClass = ClassText.Text;
            CodeSetting.Language = LanguageCombo.Text;
            CodeSetting.Source = SourceCombo.SelectedIndex;
            CodeSetting.References = ReferenceText.Text;
            CodeSetting.Files = GetFileList();
            File.WriteAllText(Path.Combine(Application.StartupPath, "Code\\code.txt"), CodeText.Text);
        }

        private void Compile()
        {
            string lang = CodeSetting.Language;
            string[] references = CodeSetting.References.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            string output = GetOutputPath();
            CompilerParameters parameters = ReflectionHelper.GetCompilerParameters(references, output, false);
            CompilerResults result;
            if (SourceCombo.SelectedIndex == 0)
            {
                if (IsEmptyString(CodeText.Text)) return;
                string source = CodeText.Text;
                result = ReflectionHelper.CompileAssemblyFromSource(lang, parameters, source);
            }
            else
            {
                string[] files = GetFileList().ToArray();
                if (files.Length == 0) return;
                result = ReflectionHelper.CompileAssemblyFromFile(lang, parameters, files);
            }
            if (!result.Errors.HasErrors)
            {
                RoomCodeHelper.LoadCode(output);
            }
            else
            {
                MessageBox.Show(string.Format("有{0}个编译错误", result.Errors.Count), "提示",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private string GetOutputPath()
        {
            int i = 0;
            string path = Path.Combine(Application.StartupPath, "Code\\code.dll");
            string dir = Path.Combine(Application.StartupPath, "Code");
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            else
            {
                while (File.Exists(path))
                {
                    i++;
                    path = Path.Combine(Application.StartupPath, string.Format("Code\\code{0}.dll", i));
                };
            }
            return path;
        }

        public static void RemoveAssemblies()
        {
            DirectoryInfo directory = new DirectoryInfo(Path.Combine(Application.StartupPath, "Code"));
            if (directory.Exists)
            {
                foreach (FileInfo file in directory.GetFiles("*.dll"))
                {
                    if (file.FullName != CodeSetting.RoomCodePath)
                    {
                        file.Delete();
                    }
                }
            }
        }

        private static bool IsEmptyString(string input)
        {
            return Regex.IsMatch(input, "^\\s*$");
        }

        private void Help_Button_Click(object sender, EventArgs e)
        {
            string message = "为房间提供初级的可编程功能,支持VisualBasic和C#两种语言.\r\n"
                + "入口类必须为有零个参数的构造函数的可实例化类.\r\n\r\n" 
                + "该类实现签名为 public bool VerifyUser(string, string, ref string) 的方法以验证用户,\r\n第一参数为用户名,第二参数为用户地址,"
                + "第三参数为返回的验证失败信息,验证成功方法返回true,反则返回false;\r\n\r\n"
                + "实现签名为 public void SetCallback(Action<string>, Action<string[]>) 的方法以保存向用户发送消息的回调,\r\n"
                + "第一参数为广播消息回调,该回调以发送的消息为参数,第二参数为向特定用户发送消息的回调,该回调以用户标识和发送的消息为参数\r\n\r\n"
                + "实现签名为 public bool ParseMessage(int, string) 的方法以解析用户消息,\r\n第一参数为用户标识, 第二参数为用户消息,"
                + "第三参数为返回的信息,该信息将广播给所有用户,解析成功方法返回true,反则返回false.\r\n\r\n点击编译按钮后各种改动才会生效";
                
            MessageBox.Show(message, "帮助信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void AddFileButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = true;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                foreach (string file in dialog.FileNames)
                {
                    FileList.Items.Add(file);
                }
            }
        }

        private void RemoveFileButton_Click(object sender, EventArgs e)
        {
            if (FileList.SelectedIndex != -1)
            {
                FileList.Items.RemoveAt(FileList.SelectedIndex);
            }
        }

        private List<string> GetFileList()
        {
            List<string> files = new List<string>();
            foreach (object item in FileList.Items)
            {
                files.Add((string)item);
            }
            return files;
        }

        private static RoomCodeSetting CodeSetting
        {
            get
            {
                return Properties.Settings.Default.CodeSetting;
            }
            set
            {
                Properties.Settings.Default.CodeSetting = value;
            }
        }

    }
}
