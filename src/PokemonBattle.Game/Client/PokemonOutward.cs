using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;
using LightStudio.PokemonBattle.Data;

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
    void ImageIdChanged(); //幻影 变身
    void Withdrawn();
  }

  [KnownType(typeof(PairValue))]
  [DataContract(Namespace = Namespaces.DEFAULT)]
  public class PokemonOutward : INotifyPropertyChanged
  {
    private static readonly PropertyChangedEventArgs NAME = new PropertyChangedEventArgs("Name");
    private static readonly PropertyChangedEventArgs STATE = new PropertyChangedEventArgs("State");
    
    [DataMember]
    internal readonly int Id;
    [DataMember]
    private readonly Position _position;

    [DataMember]
    public int OwnerId { get; private set; }
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
    [DataMember]
    public int ImageId { get; private set; }
    [DataMember(EmitDefaultValue = false)]
    public PokemonGender Gender { get; internal set; }
    [DataMember(EmitDefaultValue = false)]
    private PokemonState _state;
    public PokemonState State
    { 
      get { return _state; }
      internal set
      {
        if (_state != value)
        {
          _state = value;
          OnPropertyChanged(STATE);
        }
      }
    }
    [DataMember(EmitDefaultValue = false)]
    public bool IsSubstitute { get; internal set; }
    [DataMember]
    public PairValue Hp { get; private set; }
    [DataMember]
    public int Lv { get; private set; }
    public IPosition Position
    { get { return _position; } }

    #region Host
    internal PokemonOutward(PokemonProxy pm)
    {
      Pokemon o = pm.OnboardPokemon.GetCondition<Pokemon>("Illusion") ?? pm.Pokemon;
      _listeners = new List<IPokemonOutwardEvents>();
      OwnerId = pm.Pokemon.Owner.Id;
      Id = pm.Id;
      _position = new Position(pm.Pokemon.TeamId, pm.OnboardPokemon.X, pm.OnboardPokemon.CoordY);
      State = pm.State;
      IsSubstitute = pm.OnboardPokemon.HasCondition("Substitute");
      Hp = new PairValue(pm.Pokemon.Hp.Origin, pm.Pokemon.Hp.Value, 48);
      Lv = pm.Pokemon.Lv;

      Name = o.Name;
      ImageId = o.PokemonType.Id;
      Gender = o.Gender;
    }
    #endregion

    #region Client
    public event PropertyChangedEventHandler PropertyChanged;
    private List<IPokemonOutwardEvents> _listeners;
    private List<IPokemonOutwardEvents> listeners
    { 
      get
      {
        if (_listeners == null) _listeners = new List<IPokemonOutwardEvents>();
        return _listeners;
      }
    }

    #region Events
    /// <summary>
    /// PokemonOutward是可以序列化的，主机端不要调用这些方法
    /// </summary>
    public void Faint()
    {
      State = PokemonState.Faint;
      var listeners = this.listeners.ToArray();
      foreach (IPokemonOutwardEvents l in listeners)
        l.Faint();
    }
    /// <summary>
    /// PokemonOutward是可以序列化的，主机端不要调用这些方法
    /// </summary>
    public void Hurt(int damage)
    {
      Hp.Value -= damage;
      foreach (IPokemonOutwardEvents l in listeners)
        l.Hurt();
    }
    /// <summary>
    /// PokemonOutward是可以序列化的，主机端不要调用这些方法
    /// </summary>
    public void ChangePosition(int x, CoordY y)
    {
      if (Position.X == x && Position.Y == y) return;
      _position.X = x;
      _position.Y = y;
      foreach (IPokemonOutwardEvents l in listeners)
        l.PositionChanged();
    }
    /// <summary>
    /// PokemonOutward是可以序列化的，主机端不要调用这些方法
    /// </summary>
    public void UseItem()
    {
      foreach (IPokemonOutwardEvents l in listeners)
        l.UseItem();
    }
    /// <summary>
    /// PokemonOutward是可以序列化的，主机端不要调用这些方法
    /// </summary>
    public void UseMove(int moveType)
    {
      foreach (IPokemonOutwardEvents l in listeners)
        l.UseMove(moveType);
    }
    /// <summary>
    /// PokemonOutward是可以序列化的，主机端不要调用这些方法
    /// </summary>
    public void RecoverHp(int currentHp)
    {
      Hp.Value = currentHp;
      foreach (IPokemonOutwardEvents l in listeners)
        l.HpRecovered();
    }
    /// <summary>
    /// PokemonOutward是可以序列化的，主机端不要调用这些方法
    /// </summary>
    public void IncreaseLv5D()
    {
      foreach (IPokemonOutwardEvents l in listeners)
        l.Lv5DUp();
    }
    /// <summary>
    /// PokemonOutward是可以序列化的，主机端不要调用这些方法
    /// </summary>
    public void DecreaseLv5D()
    {
      foreach (IPokemonOutwardEvents l in listeners)
        l.Lv5DDown();
    }
    /// <summary>
    /// PokemonOutward是可以序列化的，主机端不要调用这些方法
    /// </summary>
    public void ShowSubstitute()
    {
      foreach (IPokemonOutwardEvents l in listeners)
        l.SubstituteAppear();
    }
    /// <summary>
    /// PokemonOutward是可以序列化的，主机端不要调用这些方法
    /// </summary>
    public void HideSubstitute()
    {
      foreach (IPokemonOutwardEvents l in listeners)
        l.SubstituteDisappear();
    }
    /// <summary>
    /// PokemonOutward是可以序列化的，主机端不要调用这些方法
    /// </summary>
    public void ChangeImageId(int newImageId)
    {
      ImageId = newImageId;
      foreach (IPokemonOutwardEvents l in listeners)
        l.ImageIdChanged();
    }
    /// <summary>
    /// PokemonOutward是可以序列化的，主机端不要调用这些方法
    /// </summary>
    public void Withdraw()
    {
      var listeners = this.listeners.ToArray();
      foreach (IPokemonOutwardEvents l in listeners)
        l.Withdrawn();
    }
    #endregion

    private void OnPropertyChanged(PropertyChangedEventArgs e)
    {
      if (PropertyChanged != null)
        PropertyChanged(this, e);//据说性别虽然改变但不会显示出来
    }
    public void AddListener(IPokemonOutwardEvents listener)
    {
      listeners.Add(listener);
    }
    public void RemoveListener(IPokemonOutwardEvents listener)
    {
      listeners.Remove(listener);
    }
    #endregion
  }
}
