using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Diagnostics.Contracts;

namespace PokemonBattleOnline.Game
{
  [DataContract(Name = "pd", Namespace = PBOMarks.PBO)]
  public class PokemonData : ObservableObject, ICloneable, IPokemonData
  {
    #region const
    private const int WORMADAM = 413;
    private const int ROTOM = 479;
    private const int GIRATINA = 487;
    private const int ARCEUS = 493;
    private const int DEERLING = 585;
    private const int SAWSBUCK = 586;
    private const int GENESECT = 649;
    private const int KYUREM = 646;
    private const int KELDEO = 647;
    private const int SECRET_SWORD = 548;
    private const int GRISEOUS_ORB = 1;
    private const int PLATE_MINID = 75;
    private const int PLATE_MAXID = 90;
    private const int DRIVE_MINID = 98;
    private const int DRIVE_MAXID = 101;
    #endregion

    private static bool CanChangeIv(I6D sender, int oldValue, int newValue)
    {
      return 0 <= newValue && newValue < 32;
    }

    private static bool CanChangeEv(I6D sender, int oldValue, int newValue)
    {
      return 0 <= newValue && newValue <= 255 && sender.Sum() + newValue - oldValue <= 510;
    }

    [DataMember(Name = "n")]
    private short number;
    [DataMember(Name = "f", EmitDefaultValue = false)]
    private byte form;

    public PokemonData(int number, int form)
    {
      _moves = new ObservableCollection<LearnedMove>();
      _ev = new Observable6D();
      Form = RomData.GetPokemon(number, form);
    }

    #region properties
    [DataMember(Name = "nc", EmitDefaultValue = false)]
    private string _name;
    public string Name
    {
      get { return _name; }
      set
      {
        if (string.IsNullOrWhiteSpace(value)) value = null;
        else value = value.Trim();
        if (_name != value && PokemonValidator.ValidateName(value))
        {
          _name = value;
          OnPropertyChanged("Name");
        }
      }
    }

    private static readonly int[] CAN_CHOOSE_FORM = { 201, 386, 412, 413, 422, 423, 479, 492, 641, 642, 645, 646 };
    public bool CanChooseForm
    { get { return CAN_CHOOSE_FORM.Contains(number) || number == KELDEO && HasMove(SECRET_SWORD); } }

    private PokemonForm _form;
    public PokemonForm Form
    {
      get
      {
        if (_form == null)
        {
          CheckSpForm();
          _form = RomData.GetPokemon(number, form);
        }
        return _form;
      }
      set
      {
        if (!(number == value.Species.Number && form == value.Index))
        {
          _form = null;
          var oldNumber = number;
          var oldForm = form;

          number = value.Species.Number;
          form = (byte)value.Index;
          CheckSpForm();

          if (oldNumber != number)
          {
            _moves.Clear();
            _gender = value.Species.Genders.First();
            _ev.SetStat(StatType.All, 0);
            _lv = 0;
            _nature = default(PokemonNature);
            Iv.SetStat(StatType.All, 31);
            _happiness = 0;
            _itemId = 0;
          }
          else if (number == 413 || number == 479 || number == 646) _moves.Clear();
          _abilityIndex = 0;

          OnPropertyChanged();
        }
      }
    }

    [DataMember(Name = "l", EmitDefaultValue = false)]
    private byte _lv;
    public int Lv
    {
      get { return 100 - _lv; }
      set
      {
        if (_lv != value && PokemonValidator.ValidateLv(value))
        {
          _lv = (byte)(100 - value);
          OnPropertyChanged("Lv");
        }
      }
    }

    [DataMember(Name = "g", EmitDefaultValue = false)]
    private PokemonGender _gender;
    public PokemonGender Gender
    {
      get { return _gender; }
      set
      {
        if (_gender != value && Form.Species.Genders.Contains(value))
        {
          _gender = value;
          OnPropertyChanged("Gender");
        }
      }
    }

    [DataMember(Name = "t", EmitDefaultValue = false)]
    private PokemonNature _nature;
    public PokemonNature Nature
    {
      get { return _nature; }
      set
      {
        if (_nature != value)
        {
          _nature = value;
          OnPropertyChanged("Nature");
        }
      }
    }

    [DataMember(Name = "a", EmitDefaultValue = false)]
    private byte _abilityIndex;
    int IPokemonData.AbilityIndex
    { get { return _abilityIndex; } }
    public int Ability
    {
      get { return Form.Data.GetAbility(_abilityIndex); }
      set
      {
        if (Ability != value)
        {
          _abilityIndex = 0;
          if (Form.Data.Abilities[1] == value) _abilityIndex = 1;
          else if (Form.Data.Abilities[2] == value) _abilityIndex = 2;
          else return;
          OnPropertyChanged("Ability");
        }
      }
    }

    private Observable6D _iv;
    public Observable6D Iv
    {
      get
      {
        if (_iv == null)
        {
          _iv = new Observable6D(31, 31, 31, 31, 31, 31);
          _iv.CanChange6D += CanChangeIv;
        }
        return _iv;
      }
    }
    I6D IPokemonData.Iv
    { get { return Iv; } }
    [DataMember(Name = "iv", EmitDefaultValue = false)]
    private ReadOnly6D _Iv
    {
      get { return new ReadOnly6D(31 - Iv.Hp, 31 - Iv.Atk, 31 - Iv.Def, 31 - Iv.SpAtk, 31 - Iv.SpDef, 31 - Iv.Speed); }
      set
      {
        _iv = new Observable6D(31 - value.Hp, 31 - value.Atk, 31 - value.Def, 31 - value.SpAtk, 31 - value.SpDef, 31 - value.Speed);
        _iv.CanChange6D += CanChangeIv;
      }
    }

    [DataMember(Name = "i", EmitDefaultValue = false)]
    private short _itemId;
    int IPokemonData.ItemId
    { get { return _itemId; } }
    public Item Item
    {
      get { return RomData.GetItem(_itemId); }
      set
      {
        if (Item != value)
        {
          _itemId = (short)(value == null ? 0 : value.Id);
          if (CheckSpForm()) OnPropertyChanged();
          else OnPropertyChanged("Item");
        }
      }
    }

    [DataMember(Name = "h", EmitDefaultValue = false)]
    private byte _happiness;
    public int Happiness
    {
      get { return 255 - _happiness; }
      set
      {
        if (Happiness != value && 0 <= value && value <= 255)
        {
          _happiness = (byte)(255 - value);
          OnPropertyChanged("Happiness");
        }
      }
    }

    [DataMember(Name = "e")]
    private Observable6D _ev;
    I6D IPokemonData.Ev
    { get { return _ev; } }
    bool got;
    public Observable6D Ev
    {
      get
      {
        if (!got)
        {
          got = true;
          _ev.CanChange6D += CanChangeEv;
        }
        return _ev;
      }
    }

    private string _chatter;
    [DataMember(Name = "chat", EmitDefaultValue = false, Order = 0)]
    public string Chatter
    {
      get { return number != 441 || _chatter == null || _chatter.Length > 15 ? null : _chatter; }
      set
      {
        if (number != 441 || string.IsNullOrWhiteSpace(value)) _chatter = null;
        else
        {
          var ca = value.ToCharArray(0, value.Length < 15 ? value.Length : 15);
          for (int i = 0; i < ca.Length; ++i)
            if (Char.IsWhiteSpace(ca[i])) ca[i] = ' ';
          _chatter = new String(ca);
        }
        OnPropertyChanged("Chatter");
      }
    }

    private byte GetMoveIds_PPx(int x)
    {
      byte r = 0;
      byte i = 1;
      foreach (var m in _moves)
      {
        if (m.PPUp == x) r |= i;
        i <<= 1;
      }
      return r;
    }
    private void SetMoveIds_PPx(int x, byte value)
    {
      if ((value & 1) != 0) _moves[0].PPUp = 2;
      if ((value & 2) != 0) _moves[1].PPUp = 2;
      if ((value & 4) != 0) _moves[2].PPUp = 2;
      if ((value & 8) != 0) _moves[3].PPUp = 2;
    }
    [DataMember]
    private List<int> moveIds
    {
      get { return Moves.Select((m) => m.Move.Id).ToList(); }
      set { _moves = new ObservableCollection<LearnedMove>(value.Select((m) => new LearnedMove(RomData.GetMove(m)))); }
    }
    [DataMember(EmitDefaultValue = false)]
    private byte moveIds_PP2
    {
      get { return GetMoveIds_PPx(2); }
      set { SetMoveIds_PPx(2, value); }
    }
    [DataMember(EmitDefaultValue = false)]
    private byte moveIds_PP1
    {
      get { return GetMoveIds_PPx(1); }
      set { SetMoveIds_PPx(1, value); }
    }
    [DataMember(EmitDefaultValue = false)]
    private byte moveIds_PP0
    {
      get { return GetMoveIds_PPx(0); }
      set { SetMoveIds_PPx(0, value); }
    }

    private ObservableCollection<LearnedMove> _moves;
    public IEnumerable<LearnedMove> Moves
    { get { return _moves; } }
    #endregion

    private PokemonCollection _container;
    public PokemonCollection Container
    {
      get { return _container; }
      internal set
      {
        if (_container != value)
        {
          _container = value;
          OnPropertyChanged("Container");
        }
      }
    }

    private bool CheckSpForm()
    {
      switch (number)
      {
        case DEERLING:
        case SAWSBUCK:
          form = (byte)((DateTime.Now.Month - 1) & 3);
          break;
        case ARCEUS:
          form = (byte)(PLATE_MINID <= _itemId && _itemId <= PLATE_MAXID ? _itemId - PLATE_MINID + 1 : 0);
          break;
        case GIRATINA:
          form = (byte)(_itemId == GRISEOUS_ORB ? 1 : 0);
          break;
        case GENESECT:
          form = (byte)(DRIVE_MINID <= _itemId && _itemId <= DRIVE_MAXID ? _itemId - DRIVE_MINID + 1 : 0);
          break;
        case KELDEO:
          if (!HasMove(SECRET_SWORD)) form = 0;
          break;
      }
      if (_form != null && _form.Index != form) _form = null;
      return _form == null;
    }

    public bool HasMove(int moveId)
    {
      return _moves.Any((m) => m.Move.Id == moveId);
    }

    public bool AddMove(MoveType move)
    {
      if (_moves.Count < 4 && !HasMove(move.Id))
      {
        _moves.Add(new LearnedMove(move));
        if (number == KELDEO && move.Id == SECRET_SWORD) OnPropertyChanged("CanChooseForm");
        return true;
      }
      return false;
    }

    public void RemoveMove(MoveType move)
    {
      foreach (var m in _moves)
        if (m.Move == move)
        {
          _moves.Remove(m);
          if (CheckSpForm()) OnPropertyChanged();
          if (number == KELDEO && move.Id == SECRET_SWORD) OnPropertyChanged("CanChooseForm");
          break;
        }
    }

    #region ICloneable
    public PokemonData Clone()
    {
      var clone = MemberwiseClone() as PokemonData;
      clone._propertyChanged = null;
      clone._iv = new Observable6D(Iv);
      clone._ev = new Observable6D(Ev);
      clone.got = false;
      clone._moves = new ObservableCollection<LearnedMove>();
      foreach (var m in Moves) clone._moves.Add(new LearnedMove(m.Move, m.PPUp));
      return clone;
    }
    object ICloneable.Clone()
    {
      return Clone();
    }
    #endregion
  }
}
