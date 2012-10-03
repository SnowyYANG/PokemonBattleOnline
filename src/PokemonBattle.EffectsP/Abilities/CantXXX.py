class ClearBody(AbilityE):
    def Lv7DChanging(self, pm, by, stat, change, showFail):
        if change > 0 or pm.Pokemon.TeamId == by.Pokemon.TeamId:
            return change
        if showFail:
            self.Raise(pm)
            pm.AddReportPm('7DLockAll', None, None)
        return 0
A(ClearBody(13))
A(ClearBody(162)) #white smoke

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
A(CantLvDown(53, StatType.Atk)) #hyper cutter
A(CantLvDown(63, StatType.Accuracy)) #keen eye
A(CantLvDown(93, StatType.Def)) #big pecks

class Contrary(AbilityE):
    def Lv7DChanging(self, pm, by, stat, change, showFail):
        return 0 - change
A(Contrary(91))

class Simple(AbilityE):
    def Lv7DChanging(self, pm, by, stat, change, showFail):
        return change << 1
A(Simple(120))

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
A(CantAddState(60, PokemonState.SLP, AttachedState.SLP)) #insomnia
A(CantAddState(158, PokemonState.SLP, AttachedState.SLP)) #vital spirit
A(CantAddState(69, PokemonState.PAR, AttachedState.PAR)) #limber
A(CantAddState(73, PokemonState.FRZ, AttachedState.FRZ)) #magma armour
A(CantAddState(161, PokemonState.BRN, AttachedState.BRN)) #water veil

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
A(Immunity(57))

class LeafGuard(AbilityE):
    def CanAddState(self, pm, by, state, showFail):
        if pm.Controller.Weather == Weather.IntenseSunlight and (state == AttachedState.PAR or state == AttachedState.SLP or state == AttachedState.FRZ or state == AttachedState.BRN or state == AttachedState.PSN):
            if showFail:
                self.Raise(pm)
                pm.AddReportPm('Cant' + state.ToString(), None, None)
            return False
        return True
A(LeafGuard(65))

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
A(OwnTempo(90))

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
                pm.AddReportPm('CantAttract', None, None)
            return False
        return True
A(Oblivious(87))