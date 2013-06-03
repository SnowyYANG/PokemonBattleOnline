using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using PokemonBattle.PokemonData;
using System.Diagnostics;
namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var types = new List<PokemonType>();
            var moves = new List<MoveData>();
            var pokemons = new List<PokemonData>();
            using (var stream = new FileStream("data.pgd", FileMode.Open))
            {
                using (var reader = new BinaryReader(stream))
                {
                    int count = reader.ReadInt32();
                    for (int i = 0; i < count; i++)
                    {
                        types.Add(PokemonType.FromStream(stream));
                    }
                    count = reader.ReadInt32();
                    for (int i = 0; i < count; i++)
                    {
                        moves.Add(MoveData.FromStream(stream));
                    }
                    count = reader.ReadInt32();
                    for (int i = 0; i < count; i++)
                    {
                        pokemons.Add(PokemonData.FromStream(stream));
                    }
                }
            }
            using (var writer = new StreamWriter("m.txt"))
            {
                moves.ForEach(m => writer.WriteLine("{0} {1} {2}", m.Identity, m.Name, m.Info));
            }
            /*
            using (var stream = new FileStream("data.pgd", FileMode.Create))
            {
                using (var writer = new BinaryWriter(stream))
                {
                    writer.Write(types.Count);
                    types.ForEach(t => t.Save(stream));

                    writer.Write(moves.Count);
                    moves.ForEach(m => m.Save(stream));

                    writer.Write(pokemons.Count);
                    pokemons.ForEach(p => p.Save(stream));
                }
            }
             */
            Console.Read();
        }

        /*
        static void AddMove()
        {
            moves.ForEach(m => { if (m.Accuracy > 1)m.Accuracy = MoveData.MaxAccuracy; });
            using (var stream = new FileStream("move.txt", FileMode.Open))
            {
                using (var reader = new StreamReader(stream))
                {
                    while (!reader.EndOfStream)
                    {
                        string[] data = reader.ReadLine().Split(new char[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);
                        string name = data[0];
                        string type = data[1];
                        string movetype = data[2];
                        string power = data[3];
                        string acc = data[4];
                        string pp = data[5];
                        string info = data[6];
                        string target = data[7];
                        string priority = "0";
                        if (data.Length == 9)
                            priority = data[8];
                        var move = moves.Find(m => m.Name == name);
                        if (move == null)
                            Debugger.Break();

                        var t = types.Find(typ => typ.Name.Contains(type));
                        if (t == null)
                            Debugger.Break();
                        move.Type = t.Identity;

                        if (movetype == "物")
                            move.MoveType = MoveType.物理;
                        else if (movetype == "特")
                            move.MoveType = MoveType.特殊;
                        else if (movetype == "变")
                            move.MoveType = MoveType.其他;
                        else
                            Debugger.Break();

                        move.Power = int.Parse(power);
                        if (acc == "--")
                            move.Accuracy = MoveData.MaxAccuracy;
                        else
                            move.Accuracy = int.Parse(acc) / 100.0;
                        move.PP = int.Parse(pp);
                        move.Info = info;
                        move.Priority = int.Parse(priority);

                        switch (target)
                        {
                            case "单体":
                                move.Target = MoveTarget.选一;
                                break;
                            case "自身":
                                move.Target = MoveTarget.自身;
                                break;
                            case "对方二体":
                                move.Target = MoveTarget.二体;
                                break;
                            case "己方场地":
                                move.Target = MoveTarget.己场;
                                break;
                            case "全场五体":
                                move.Target = MoveTarget.全体;
                                break;
                            case "全场场地":
                                move.Target = MoveTarget.全场;
                                break;
                        }
                    }
                }
            }
        }
        */

        /*
        static void CreateTypeId()
        {
             
            for (int i = 1; i <= types.Count; i++)
            {
                var type = types[i - 1];
                type.Identity = i;
                Console.WriteLine("Set Type{0} Id:{1}", type.Name, i);
                moves.FindAll(m => m.Type == type.Name).ForEach(m => m.Type = i);
                pokemons.ForEach(p =>
                    {
                        if (p.Type1 == type.Name) p.Type1 = i;
                        else if (p.Type2 == type.Name) p.Type2 = i;
                    });
            }
        }
        */

        /*
        static void CreateMoveId()
        {


            using (var stream = new FileStream("moveId.txt", FileMode.Open))
            {
                using (var reader = new StreamReader(stream))
                {
                    while (!reader.EndOfStream)
                    {
                        string[] data = reader.ReadLine().Split(new char[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);
                        int id = int.Parse(data[0]);
                        string name = data[1];
                        string oldName = null;
                        if (data.Length == 3) oldName = data[2];

                        if (!string.IsNullOrEmpty(oldName))
                        {
                            var move = moves.Find(m => m.Name == oldName);
                            if (move == null)
                            {
                                System.Diagnostics.Debugger.Break();
                            }
                            else
                            {
                                move.Identity = id;
                                move.Name = name;
                                if (move.Accuracy == 99.99)
                                {
                                    move.Accuracy = 100;
                                }
                                if (move.Power == 9999)
                                {
                                    move.Power = 0;
                                }
                                Console.WriteLine("Set Move({0}) to {1}:{2}", oldName, id, name);
                            }
                            pokemons.ForEach(p =>
                                p.Moves.ForEach(m =>
                                {
                                    if (m.MoveName == oldName)
                                    {
                                        m.MoveId = id;
                                        Console.WriteLine("Set Move({0}) to {1}", oldName, id);
                                    }
                                }
                                ));
                        }
                        else
                        {
                            //System.Diagnostics.Debugger.Break();
                            MoveData move = new MoveData() { Identity = id, Name = name };
                            moves.Add(move);
                            Console.WriteLine("Add Move :{0}", name);
                        }
                    }
                }
            }
        }
        */

        /*
         * 
         * 
            var dict = new Dictionary<string, int>();
            using (var reader = new StreamReader("dic.txt"))
            {
                while (!reader.EndOfStream)
                {
                    var str = reader.ReadLine().Split('\t');
                    dict.Add(str[2], int.Parse(str[0]));
                }
            }

            using (var reader = new BinaryReader(new FileStream("move.dat", FileMode.Open)))
            {
                foreach (var pm in pokemons)
                {
                    int id = reader.ReadInt32();
                    if (id != pm.Identity)
                        Debugger.Break();

                    var list = new List<string>(reader.ReadInt32());
                    if (list.Capacity != pm.Moves.Count)
                        Debugger.Break();
                    for (int i = 0; i < list.Capacity; i++)
                        list.Add(reader.ReadString());

                    var invalidMoves = pm.Moves.FindAll(m => m.MoveId == 0);
                    foreach (var move in invalidMoves)
                    {
                        int index = pm.Moves.IndexOf(move);
                        move.MoveId = dict[list[index]];
                    }

                }
            }
         */
    }
}
