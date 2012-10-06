using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;
using LightStudio.PokemonBattle.Data;
using LightStudio.PokemonBattle.Game.Host;

namespace LightStudio.PokemonBattle.Game
{
  public interface IPokemonOutwardEvents
  {
    void Faint();
    void Hurt();
    void PositionChanged();
    void UseItem(); //褐色光圈
    void UseMove(int moveType);
    void HpRecovered(); //绿色光上升
    void Lv5DUp(); //绿色上升
    void Lv5DDown(); //下降
    void SubstituteAppear();
    void SubstituteDisappear();
    void FormChanged(); //幻影 变身
    void Withdrawn();
  }

  [KnownType(typeof(PairValue))]
  [DataContract(Namespace = Namespaces.PBO)]
  public class PokemonOutward : INotifyPropertyChanged
  {
    private static readonly PropertyChangedEventArgs NAME = new PropertyChangedEventArgs("Name");
    private static readonly PropertyChangedEventArgs GENDER = new PropertyChangedEventArgs("Gender");
    private static readonly PropertyChangedEventArgs STATE = new PropertyChangedEventArgs("State");
    
    [DataMember]
    internal readonly int Id;
    [DataMember]
    private int number;
    [DataMember(EmitDefaultValue = false)]
    private int form;

    #region Host
    internal PokemonOutward(PokemonProxy pm)
    {
      OwnerId = pm.Pokemon.Owner.Id;
      Id = pm.Id;
      _position = new Position(pm.Pokemon.TeamId, pm.OnboardPokemon.X, pm.OnboardPokemon.CoordY);
      State = pm.State;
      IsSubstitute = pm.OnboardPokemon.HasCondition("Substitute");
      Hp = new PairValue(pm.Pokemon.Hp.Origin, pm.Pokemon.Hp.Value, 48);
      Lv = pm.Pokemon.Lv;

      Pokemon o = pm.OnboardPokemon.GetCondition<Pokemon>("Illusion");
      if (o == null)
      {
        Name = pm.Pokemon.Name;
        Form = pm.OnboardPokemon.Form;
        Gender = pm.OnboardPokemon.Gender;
      }
      else
      {
        Name = o.Name;
        Form = o.Form;
        Gender = o.Gender;
      }
    }
    #endregion

    [DataMember]
    public int OwnerId
    { get; private set; }

    [DataMember]
    private string _name;
    public string Name
    {
      get { return _name; }
      internal set
      {
        if (_name != value)
        {
          _name = value;
          OnPropertyChanged(NAME);
        }
      }
    }

    public PokemonForm Form
    {
      get { return DataService.GetPokemon(number, form); }
      private set
      {
        number = value.Type.Number;
        form = value.Index;
      }
    }

    [DataMember(EmitDefaultValue = false)]
    private PokemonGender _gender;
    public PokemonGender Gender
    {
      get { return _gender; }
      internal set
      {
        if (_gender != value)
        {
          _gender = value;
          OnPropertyChanged(GENDER);
        }
      }
    }
    
    [DataMember(EmitDefaultValue = false)]
    private PokemonState _state;
    public PokemonState State
    { 
      get { return _state; }
      set
      {
        if (_state != value)
        {
          var former = _state;
          _state = value;
          if (team != null) team.StateChanged(this, former);
          OnPropertyChanged(STATE);
        }
      }
    }
    
    [DataMember(EmitDefaultValue = false)]
    public bool IsSubstitute
    { get; internal set; }
    
    [DataMember]
    public PairValue Hp
    { get; private set; }
    
    [DataMember]
    public int Lv
    { get; private set; }
    
    [DataMember]
    private readonly Position _position;
    public IPosition Position
    { get { return _position; } }

    #region Client
    public event PropertyChangedEventHandler PropertyChanged;
    private IPokemonOutwardEvents listener;
    private TeamOutward team;

    #region Events
    /// <summary>
    /// PokemonOutward是可以序列化的，主机端不要调用这些方法
    /// </summary>
    public void Faint()
    {
      State = PokemonState.Faint;
      listener.Faint();
    }
    /// <summary>
    /// PokemonOutward是可以序列化的，主机端不要调用这些方法
    /// </summary>
    public void Hurt(int damage)
    {
      Hp.Value -= damage;
      listener.Hurt();
    }
    /// <summary>
    /// PokemonOutward是可以序列化的，主机端不要调用这些方法
    /// </summary>
    public void ChangePosition(int x, CoordY y)
    {
      if (Position.X == x && Position.Y == y) return;
      _position.X = x;
      _position.Y = y;
      listener.PositionChanged();
    }
    /// <summary>
    /// PokemonOutward是可以序列化的，主机端不要调用这些方法
    /// </summary>
    public void UseItem()
    {
      listener.UseItem();
    }
    /// <summary>
    /// PokemonOutward是可以序列化的，主机端不要调用这些方法
    /// </summary>
    public void UseMove(int moveType)
    {
      listener.UseMove(moveType);
    }
    /// <summary>
    /// PokemonOutward是可以序列化的，主机端不要调用这些方法
    /// </summary>
    public void RecoverHp(int currentHp)
    {
      Hp.Value = currentHp;
      listener.HpRecovered();
    }
    /// <summary>
    /// PokemonOutward是可以序列化的，主机端不要调用这些方法
    /// </summary>
    public void IncreaseLv5D()
    {
      listener.Lv5DUp();
    }
    /// <summary>
    /// PokemonOutward是可以序列化的，主机端不要调用这些方法
    /// </summary>
    public void DecreaseLv5D()
    {
      listener.Lv5DDown();
    }
    /// <summary>
    /// PokemonOutward是可以序列化的，主机端不要调用这些方法
    /// </summary>
    public void ShowSubstitute()
    {
      listener.SubstituteAppear();
    }
    /// <summary>
    /// PokemonOutward是可以序列化的，主机端不要调用这些方法
    /// </summary>
    public void HideSubstitute()
    {
      listener.SubstituteDisappear();
    }
    /// <summary>
    /// PokemonOutward是可以序列化的，主机端不要调用这些方法
    /// </summary>
    public void ChangeForm(int number, int form)
    {
      this.number = number;
      this.form = form;
      listener.FormChanged();
    }
    /// <summary>
    /// PokemonOutward是可以序列化的，主机端不要调用这些方法
    /// </summary>
    public void Withdraw()
    {
      listener.Withdrawn();
    }
    #endregion

    private void OnPropertyChanged(PropertyChangedEventArgs e)
    {
      if (PropertyChanged != null)
        PropertyChanged(this, e);//据说性别虽然改变但不会显示出来
    }
    public string GetProperty(string propertyName)
    {
      string r = null;
      switch (propertyName)
      {
        case "Name":
          r = Name;
          break;
        case "Lv":
          r = Lv.ToString();
          break;
        case "Type":
          r = Form.Type.GetLocalizedName();
          break;
        case "State":
          r = State.GetLocalizedName();
          break;
      }
      return r;
    }
    public void Init(GameOutward game)
    {
      team = game.Teams[Position.Team];
    }
    public void AddListener(IPokemonOutwardEvents listener)
    {
#if DEBUG
      if (this.listener != null) System.Diagnostics.Debugger.Break();
#endif
      this.listener = listener;
    }
    public void RemoveListener(IPokemonOutwardEvents listener)
    {
#if DEBUG
      if (this.listener != listener) System.Diagnostics.Debugger.Break();
#endif
      this.listener = null;
    }
    #endregion

    public override string ToString()
    {
      return string.Format("{0}(Lv.{1} {2})", Name, Lv, Form.Type.GetLocalizedName());
    }
  }
}
