class CantAddState(AbilityE):
    def __new__(cls, id, pmstate, atstate):
        return AbilityE.__new__(cls, id)
    def __init__(self, id, pmstate, atstate):
        AbilityE.__init__(self, id)
        self.PmState = pmstate
        self.AtState = atstate
    def Attach(self, pm):
        if pm.State == self.PmState:
            self.Raise(pm)
            pm.DeAbnormalState(False)
    def CanAddState(self, pm, by, state, showFail):
        if state == self.AtState:
            if showFail:
                self.Raise(pm)
                pm.AddReportPm('Cant' + self.AtState.ToString())
            return False
        return True
A(CantAddState(7, PokemonState.PAR, AttachedState.PAR)) #limber
A(CantAddState(15, PokemonState.SLP, AttachedState.SLP)) #insomnia
A(CantAddState(40, PokemonState.FRZ, AttachedState.FRZ)) #magma armour
A(CantAddState(41, PokemonState.BRN, AttachedState.BRN)) #water veil
A(CantAddState(72, PokemonState.SLP, AttachedState.SLP)) #vital spirit

class Oblivious(AbilityE):
    def Attach(self, pm):
        if pm.OnboardPokemon.HasCondition('Attract'):
            self.Raise(pm)
            pm.OnboardPokemon.RemoveCondition('Attract')
            pm.AddReportPm('DeAttract', None, None)
    def CanAddState(self, pm, by, state, showFail):
        if state == AttachedState.Attract:
            if showFail:
                self.Raise(pm)
                pm.AddReportPm('NoEffect', None, None)
            return False
        return True
    def CanImplement(self, d):
        if d.AtkContext.Move.Id == 445: #captivate
            self.Raise(pm)
            pm.AddReportPm('NoEffect', None, None)
            return False
        return True
A(Oblivious(12))

class Immunity(AbilityE):
    def Attach(self, pm):
        if pm.State == PokemonState.PSN or pm.State == PokemonState.BadlyPSN:
            self.Raise(pm)
            pm.DeAbnormalState(False)
    def CanAddState(self, pm, by, state, showFail):
        if state == AttachedState.PSN:
            if showFail:
                self.Raise(pm)
                pm.AddReportPm('CantPSN', None, None)
            return False
        return True
A(Immunity(17))

class OwnTempo(AbilityE):
    def Attach(self, pm):
        if pm.OnboardPokemon.HasCondition('Confuse'):
            self.Raise(pm)
            pm.OnboardPokemon.RemoveCondition('Confuse')
            pm.AddReportPm('DeConfuse')
    def CanAddState(self, pm, by, state, showFail):
        if state == AttachedState.Confusion:
            if showFail:
                self.Raise(pm)
                pm.AddReportPm('CantConfuse', None, None)
            return False
        return True
A(OwnTempo(20))

class ClearBody(AbilityE):
    def Lv7DChanging(self, pm, by, stat, change, showFail):
        if change > 0 or pm.Pokemon.TeamId == by.Pokemon.TeamId:
            return change
        if showFail:
            self.Raise(pm)
            pm.AddReportPm('7DLockAll', None, None)
        return 0
A(ClearBody(29))
A(ClearBody(73)) #white smoke

class CantLvDown(AbilityE):
    def __new__(cls, id, stat):
        return AbilityE.__new__(cls, id)
    def __init__(self, id, stat):
        AbilityE.__init__(self, id)
        self.Stat = stat
    def Lv7DChanging(self, pm, by, stat, change, showFail):
        if change < 0 and stat == self.Stat and pm.Pokemon.TeamId != by.Pokemon.TeamId:
            if showFail:
                self.Raise(pm)
                pm.AddReportPm('7DLock', stat)
            return 0
        return change
A(CantLvDown(51, StatType.Accuracy)) #keen eye
A(CantLvDown(52, StatType.Atk)) #hyper cutter
A(CantLvDown(145, StatType.Def)) #big pecks

class Simple(AbilityE):
    def Lv7DChanging(self, pm, by, stat, change, showFail):
        return change << 1
A(Simple(86))

class LeafGuard(AbilityE):
    def CanAddState(self, pm, by, state, showFail):
        if pm.Controller.Weather == Weather.IntenseSunlight and (state == AttachedState.PAR or state == AttachedState.SLP or state == AttachedState.FRZ or state == AttachedState.BRN or state == AttachedState.PSN):
            if showFail:
                self.Raise(pm)
                pm.AddReportPm('Cant' + state.ToString(), None, None)
            return False
        return True
A(LeafGuard(102))

class Contrary(AbilityE):
    def Lv7DChanging(self, pm, by, stat, change, showFail):
        return 0 - change
A(Contrary(126))

