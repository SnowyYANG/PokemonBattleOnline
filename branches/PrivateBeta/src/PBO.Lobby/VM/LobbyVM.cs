using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Data;
using LightStudio.Tactic.Messaging;
using LightStudio.PokemonBattle.Messaging;

namespace LightStudio.PokemonBattle.PBO.Lobby
{
  class LobbyVM
  {
    Dictionary<int, UserVM> usersDictionary;
    ObservableCollection<UserVM> users;

    public LobbyVM()
    {
      PBOClient.Client.Disconnected += (sender, e) =>
        {
          System.Windows.MessageBox.Show("连接与服务器中断");
        };
      PBOClient.Client.UserChanged += model_UserChanged;
      //if it's possible to relogin, better Disconnected+=()=>Model.Dispose();

      var _us = PBOClient.Client.Users;
      usersDictionary = new Dictionary<int, UserVM>(_us.Count());
      users = new ObservableCollection<UserVM>();
      foreach (User u in _us) AddUser(u);
      User = new UserVM(PBOClient.Client.User);
      usersDictionary.Add(User.Id, User); users.Add(User);
      Users = new ReadOnlyObservableCollection<UserVM>(users);

      UsersView = CollectionViewSource.GetDefaultView(Users);
      UsersView.SortDescriptions.Add(new SortDescription("State", ListSortDirection.Descending));
    }

    public UserVM User { get; private set; }
    public ICollectionView UsersView { get; private set; }
    public ReadOnlyObservableCollection<UserVM> Users { get; private set; }

    void AddUser(User user)
    {
      UserVM u = new UserVM(user);
      usersDictionary.Add(u.Id, u);
      users.Add(u);
    }
    void model_UserChanged(int userId)
    {
      //thread safe?
      UIDispatcher.Invoke(delegate
        {
          var uinfo = PBOClient.Client.GetUser(userId);
          if (uinfo != null)
          {
            if (usersDictionary.ContainsKey(userId)) usersDictionary[userId].RefreshProperties();
            else AddUser(uinfo);
          }
          else
          { //Remove user
            UserVM u;
            usersDictionary.TryGetValue(userId, out u);
            users.Remove(u);
            usersDictionary.Remove(userId);
          }
        });
    }
    public void Exit()
    {
      PBOClient.Client.Logout();
    }

    public void Dispose()
    {
      PBOClient.Client.Dispose(); //thread safe?
    }
  }
}
