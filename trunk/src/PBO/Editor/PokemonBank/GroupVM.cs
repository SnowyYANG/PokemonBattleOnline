using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Windows;
using Microsoft.Win32;
using PokemonBattleOnline.Game;
using PokemonBattleOnline.PBO.Elements;

namespace PokemonBattleOnline.PBO.Editor
{
    //internal class GroupVM
    //{
    //    private bool team;

    //    public GroupVM(CollectionGroup model, bool isTeamGroup)
    //    {
    //        this.Model = model;
    //        team = isTeamGroup;
    //        InitializeCommands();
    //    }

    //    public CollectionGroup Model
    //    { get; private set; }
    //    public ObservableCollection<MenuCommand> Commands
    //    { get; private set; }
    //    public MenuCommand NewCollectionCommand
    //    { get; private set; }
    //    public MenuCommand ImportCommand
    //    { get; private set; }
    //    public MenuCommand ImportFromClipboardCommand
    //    { get; private set; }

    //    private void InitializeCommands()
    //    {
    //        if (team) NewCollectionCommand = new MenuCommand("New Team", NewCollection) { Icon = Helper.GetImage(@"Balls/Normal.png") };
    //        else NewCollectionCommand = new MenuCommand("New Box", NewCollection) { Icon = BoxVM.ICON };
    //        ImportCommand = new MenuCommand("Import", Import);
    //        ImportFromClipboardCommand = new MenuCommand("从剪贴板导入", ImportFromClipboard);

    //        Commands = new ObservableCollection<MenuCommand>();
    //        Commands.Add(NewCollectionCommand);
    //        Commands.Add(ImportCommand);

    //        Commands.Add(new MenuCommand("Import0791", () => Import(Team0791.GetInstance(), "Text File(*.ptd)|*.ptd")));
    //        Commands.Add(new MenuCommand("ImportPO", () => Import(TeamPO.GetInstance(), "PO File(*.tp)|*.tp")));

    //        Commands.Add(ImportFromClipboardCommand);
    //    }

    //    public void NewCollection()
    //    {
    //        if (team) Model.AddCollection("新队伍");
    //        else Model.AddCollection("新箱子");
    //    }

    //    public void Import()
    //    {
    //        bool succeed = false;
    //        var dialog = new OpenFileDialog();
    //        dialog.Filter = "文本文件(*.txt)|*.txt";
    //        dialog.Multiselect = true;
    //        if (dialog.ShowDialog() == true)
    //            foreach (var f in dialog.FileNames)
    //                try
    //                {
    //                    using (var sr = new System.IO.StreamReader(f, Encoding.Default, true))
    //                        succeed = Model.Import(System.IO.Path.GetFileNameWithoutExtension(f), sr.ReadToEnd());
    //                    if (!succeed) ShowMessageBox.FolderImportFail(string.Format("文件{0}中没有包含可识别的精灵信息。", f));
    //                }
    //                catch (Exception)
    //                {
    //                    ShowMessageBox.FolderImportFail(string.Format("文件{0}读取失败。", f));
    //                }
    //    }

    //    public void ImportFromClipboard()
    //    {
    //        var str = Clipboard.GetText();
    //        if (string.IsNullOrWhiteSpace(str)) ShowMessageBox.FolderImportFail("剪贴板中没有文本。");
    //        if (str.Length > 10000) ShowMessageBox.FolderImportFail("剪贴板中的文本过长。");
    //        else if (!Model.Import("剪贴板", str)) ShowMessageBox.FolderImportFail("剪贴板文本中没有包含可识别的精灵信息。");
    //    }

    //    public void Import(ITeamIO tm, string filter)
    //    {
    //        var dialog = new OpenFileDialog();
    //        dialog.Filter = filter;
    //        dialog.Multiselect = true;
    //        if (dialog.ShowDialog() == true)
    //        {
    //            int count = Model.Count;
    //            foreach (var path in dialog.FileNames)
    //            {
    //                try
    //                {
    //                    var bt = (PokemonBT)tm.Read(path);
    //                    if (string.IsNullOrWhiteSpace(bt.Name))
    //                    {
    //                        bt.Name = Path.GetFileNameWithoutExtension(path);
    //                    }
    //                    if (bt != null && bt.Count > 0) Model.Add(bt);
    //                }
    //                catch { }
    //            }
    //            MessageBox.Show(string.Format("共成功导入 {0} 个队伍!", Model.Count - count));
    //        }
    //    }
    //}
}
