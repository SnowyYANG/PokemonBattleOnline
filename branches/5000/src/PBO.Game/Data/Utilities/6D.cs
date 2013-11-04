﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace PokemonBattleOnline.Game
{
    public interface I6D : IEnumerable<int>, IEquatable<I6D>
    {
        int Hp { get; }
        int Atk { get; }
        int Def { get; }
        int SpAtk { get; }
        int SpDef { get; }
        int Speed { get; }
        int GetStat(StatType type);
    }

    [DataContract(Namespace = PBOMarks.PBO)]
    public struct ReadOnly6D : I6D
    {
        public ReadOnly6D(int h, int a, int d, int sa, int sd, int s)
        {
            _hp = h;
            _atk = a;
            _def = d;
            _spAtk = sa;
            _spDef = sd;
            _speed = s;
        }

        public ReadOnly6D(I6D values)
        {
            _hp = values.Hp;
            _atk = values.Atk;
            _def = values.Def;
            _spAtk = values.SpAtk;
            _spDef = values.SpDef;
            _speed = values.Speed;
        }

        #region props

        [DataMember(EmitDefaultValue = false)]
        private readonly int _hp;
        public int Hp
        { get { return _hp; } }
        [DataMember(EmitDefaultValue = false)]
        private readonly int _atk;
        public int Atk
        { get { return _atk; } }
        [DataMember(EmitDefaultValue = false)]
        private readonly int _def;
        public int Def
        { get { return _def; } }
        [DataMember(EmitDefaultValue = false)]
        private readonly int _spAtk;
        public int SpAtk
        { get { return _spAtk; } }
        [DataMember(EmitDefaultValue = false)]
        private readonly int _spDef;
        public int SpDef
        { get { return _spDef; } }
        [DataMember(EmitDefaultValue = false)]
        private readonly int _speed;
        public int Speed
        { get { return _speed; } }

        #endregion

        public int GetStat(StatType type)
        {
            int value;
            switch (type)
            {
                case StatType.Hp:
                    value = Hp;
                    break;
                case StatType.Atk:
                    value = Atk;
                    break;
                case StatType.Def:
                    value = Def;
                    break;
                case StatType.SpAtk:
                    value = SpAtk;
                    break;
                case StatType.SpDef:
                    value = SpDef;
                    break;
                case StatType.Speed:
                    value = Speed;
                    break;
                default:
                    value = 0;
                    break;
            }
            return value;
        }

        #region IEnumerator

        public IEnumerator<int> GetEnumerator()
        {
            yield return this.Hp;
            yield return this.Atk;
            yield return this.Def;
            yield return this.SpAtk;
            yield return this.SpDef;
            yield return this.Speed;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            yield return this.Hp;
            yield return this.Atk;
            yield return this.Def;
            yield return this.SpAtk;
            yield return this.SpDef;
            yield return this.Speed;
        }

        #endregion

        #region IEquatable

        public bool Equals(I6D other)
        {
            return
              this.Hp == other.Hp &&
              this.Atk == other.Atk &&
              this.Def == other.Def &&
              this.SpAtk == other.SpAtk &&
              this.SpDef == other.SpDef &&
              this.Speed == other.Speed;
        }

        #endregion
    }

    [DataContract(Namespace = PBOMarks.PBO)]
    public class Observable6D : ObservableObject, I6D
    {
        private static readonly PropertyChangedEventArgs HP = new PropertyChangedEventArgs("Hp");
        private static readonly PropertyChangedEventArgs ATK = new PropertyChangedEventArgs("Atk");
        private static readonly PropertyChangedEventArgs DEF = new PropertyChangedEventArgs("Def");
        private static readonly PropertyChangedEventArgs SPATK = new PropertyChangedEventArgs("SpAtk");
        private static readonly PropertyChangedEventArgs SPDEF = new PropertyChangedEventArgs("SpDef");
        private static readonly PropertyChangedEventArgs SPEED = new PropertyChangedEventArgs("Speed");

        public event Func<I6D, int, int, bool> CanChange6D;

        public Observable6D(I6D obj)
            : this(obj.Hp, obj.Atk, obj.Def, obj.SpAtk, obj.SpDef, obj.Speed)
        {
        }

        public Observable6D(int h = 0, int a = 0, int d = 0, int sa = 0, int sd = 0, int s = 0)
        {
            _hp = h;
            _atk = a;
            _def = d;
            _spAtk = sa;
            _spDef = sd;
            _speed = s;
        }

        #region props

        [DataMember(EmitDefaultValue = false)]
        private int _hp;
        public int Hp
        {
            get { return _hp; }
            set
            {
                if (_hp != value && OnPropertyChanging(_hp, value))
                {
                    _hp = value;
                    OnPropertyChanged(HP);
                }
            }
        }

        [DataMember(EmitDefaultValue = false)]
        private int _atk;
        public int Atk
        {
            get { return _atk; }
            set
            {
                if (_atk != value && OnPropertyChanging(_atk, value))
                {
                    _atk = value;
                    OnPropertyChanged(ATK);
                }
            }
        }

        [DataMember(EmitDefaultValue = false)]
        private int _def;
        public int Def
        {
            get { return _def; }
            set
            {
                if (_def != value && OnPropertyChanging(_def, value))
                {
                    _def = value;
                    OnPropertyChanged(DEF);
                }
            }
        }

        [DataMember(EmitDefaultValue = false)]
        private int _spAtk;
        public int SpAtk
        {
            get { return _spAtk; }
            set
            {
                if (_spAtk != value && OnPropertyChanging(_spAtk, value))
                {
                    _spAtk = value;
                    OnPropertyChanged(SPATK);
                }
            }
        }

        [DataMember(EmitDefaultValue = false)]
        private int _spDef;
        public int SpDef
        {
            get { return _spDef; }
            set
            {
                if (_spDef != value && OnPropertyChanging(_spDef, value))
                {
                    _spDef = value;
                    OnPropertyChanged(SPDEF);
                }
            }
        }

        [DataMember(EmitDefaultValue = false)]
        private int _speed;
        public int Speed
        {
            get { return _speed; }
            set
            {
                if (_speed != value && OnPropertyChanging(_speed, value))
                {
                    _speed = value;
                    OnPropertyChanged(SPEED);
                }
            }
        }

        #endregion

        private bool OnPropertyChanging(int oldValue, int newValue)
        {
            return CanChange6D == null ? true : CanChange6D(this, oldValue, newValue);
        }

        public int GetStat(StatType type)
        {
            switch (type)
            {
                case StatType.Hp:
                    return Hp;
                case StatType.Atk:
                    return Atk;
                case StatType.Def:
                    return Def;
                case StatType.SpAtk:
                    return SpAtk;
                case StatType.SpDef:
                    return SpDef;
                case StatType.Speed:
                    return Speed;
                default:
                    return 0;
            }
        }

        public void SetStat(StatType type, int value)
        {
            switch (type)
            {
                case StatType.Hp:
                    Hp = value;
                    break;
                case StatType.Atk:
                    Atk = value;
                    break;
                case StatType.Def:
                    Def = value;
                    break;
                case StatType.SpAtk:
                    SpAtk = value;
                    break;
                case StatType.SpDef:
                    SpDef = value;
                    break;
                case StatType.Speed:
                    Speed = value;
                    break;
                case StatType.All:
                    Hp = Atk = Def = SpAtk = SpDef = Speed = value;
                    break;
            }
        }

        public void SetByIndex(int index, int value)
        {
            switch (index)
            {
                case 0:
                    Hp = value;
                    break;
                case 1:
                    Atk = value;
                    break;
                case 2:
                    Def = value;
                    break;
                case 3:
                    SpAtk = value;
                    break;
                case 4:
                    SpDef = value;
                    break;
                case 5:
                    Speed = value;
                    break;
            }
        }

        #region IEnumerator

        public IEnumerator<int> GetEnumerator()
        {
            yield return this.Hp;
            yield return this.Atk;
            yield return this.Def;
            yield return this.SpAtk;
            yield return this.SpDef;
            yield return this.Speed;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            yield return this.Hp;
            yield return this.Atk;
            yield return this.Def;
            yield return this.SpAtk;
            yield return this.SpDef;
            yield return this.Speed;
        }

        #endregion

        #region IEquatable

        public bool Equals(I6D other)
        {
            return
              this.Hp == other.Hp &&
              this.Atk == other.Atk &&
              this.Def == other.Def &&
              this.SpAtk == other.SpAtk &&
              this.SpDef == other.SpDef &&
              this.Speed == other.Speed;
        }

        #endregion

    }
}