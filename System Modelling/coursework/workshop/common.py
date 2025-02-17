from dataclasses import dataclass, field

from qnet.common import Item


@dataclass(eq=False)
class CarUnit(Item):
    repair_time: float = field(init=False, repr=False, default=0)  # last, used to prioritize machines in the repair queue, as seen in the _first_repair_priority_fn
    num_repairs: int = field(init=False, repr=False, default=0) # used to calculate proba of cycle
    repair_wait_time: float = field(init=False, repr=False, default=0)  # total wait time in repair quee, used for metrics such as repair_wait_time_mean and repair_wait_time_std

    def __lt__(self, other: 'CarUnit') -> bool:
        if self.num_repairs >= 1 and other.num_repairs >= 1: # if machine was cycled
            return self.time_in_system > other.time_in_system   # use time spent in system for comparison, the oldest machine would have priority in quee
        return self.repair_time < other.repair_time # use repair time for comparison
