import random
import math
import bisect
from dataclasses import dataclass
from typing import TypeVar, Generic, Sequence, Sized, Callable, overload

INF_TIME = float('inf')
TIME_EPS = 1e-6

T = TypeVar('T')
V = TypeVar('V')


def erlang(lambd: float, k: int) -> float:
    product = 1.0
    for _ in range(k):
        product *= random.random()
    return -1 / lambd * math.log(product)


class _KeyWrapper(Generic[T, V], Sequence[V], Sized):
    """
    This is a helper class that wraps a sequence and provides a way to access the sequence elements using a key function.
    This class is used to facilitate binary search operations on sequences based on a key function.
    """

    def __init__(self, sequence: Sequence[T], key: Callable[[T], V]) -> None:
        self.key = key
        self.sequence = sequence

    def __len__(self) -> int:
        return len(self.sequence)

    @overload
    def __getitem__(self, idx: int) -> V:
        ...

    @overload
    def __getitem__(self, idx: slice) -> Sequence[V]:
        ...

    def __getitem__(self, idx):
        if isinstance(idx, slice):
            return map(self.key, self.sequence[idx])
        return self.key(self.sequence[idx])


@dataclass(eq=False)
class EmpiricalPoint:
    value: float
    cum_proba: float


def empirical(points: list[EmpiricalPoint]) -> float:
    """
    generates a random value based on an empirical distribution defined by a list of EmpiricalPoint instances. This is useful for sampling from a distribution that is defined by observed data rather than a theoretical model.
    """
    num_points = len(points)
    assert num_points >= 2 and points[0].cum_proba == 0 and points[-1].cum_proba == 1, points
    proba = random.uniform(0, 1)
    start_idx = bisect.bisect_right(_KeyWrapper(points, key=lambda point: point.cum_proba), proba) - 1
    end_idx = min(start_idx + 1, num_points - 1)
    start, end = points[start_idx], points[end_idx]
    #linear interpolation between the two points for which we know the cummulative probability to get the actual value which we want to return. 
    return start.value + (end.value - start.value) / (end.cum_proba - start.cum_proba) * (proba - start.cum_proba)
