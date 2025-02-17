from collections import deque
import heapq
import itertools
import inspect
from enum import Enum
from abc import ABC, abstractmethod
from dataclasses import dataclass, field, fields, _MISSING_TYPE
from typing import (TypeVar, Generic, Optional, Iterable, Callable, SupportsFloat, Protocol, Union, Any, cast,
                    runtime_checkable)

INF_TIME = float('inf')
TIME_EPS = 1e-6

I = TypeVar('I', bound='Item')
M = TypeVar('M', bound='Metrics')
T = TypeVar('T')


@runtime_checkable
class SupportsDict(Protocol):

    def to_dict(self) -> dict[str, Any]:
        ...


class ActionType(str, Enum):
    IN = 'in'
    OUT = 'out'


@dataclass(eq=False)
class ActionRecord(SupportsDict, Generic[T]):
    """
    The ActionRecord class is used to keep track of actions performed by nodes on items in the simulation.
    Each action is recorded in Item history with following fields:
    - node involved,
    - the type of action,
    - and the time it occurred.
    """
    node: T
    action_type: ActionType
    time: float

    def to_dict(self) -> dict[str, Any]:
        return {'node': self.node, 'action_type': self.action_type, 'time': self.time}


@dataclass(eq=False)
class Item(SupportsDict):
    """
    Item represents an item that moves through the system, records actions performed on it, and tracks its time in the system.
    The Item class focuses on representing the item and tracking its history.
    """
    id: str
    created_time: float = field(repr=False)
    current_time: float = field(init=False, repr=False)
    processed: bool = field(init=False, repr=False, default=False)
    history: list[ActionRecord] = field(init=False, repr=False, default_factory=list)

    def __post_init__(self) -> None:
        self.current_time = self.created_time

    @property
    def released_time(self) -> Optional[float]:
        return self.current_time if self.processed else None

    @property
    def time_in_system(self) -> float:
        return self.current_time - self.created_time

    def to_dict(self) -> dict[str, Any]:
        return {'id': self.id}


@dataclass(eq=False)
class Metrics(Protocol):
    passed_time: float = field(init=False, default=0)

    def to_dict(self) -> dict[str, Any]:
        """will include all properties of the class that implements the Metrics protocol"""
        metrics_dict = {
            name: getattr(self, name)
            for name, _ in inspect.getmembers(type(self),
                                              lambda value: isinstance(value, property) and value.fget is not None)
        }
        return metrics_dict

    def reset(self) -> None:
        for param in fields(self):
            if not isinstance(param.default, _MISSING_TYPE):
                default = param.default
            elif not isinstance(param.default_factory, _MISSING_TYPE):
                default = param.default_factory()
            else:
                # If neither a default value nor a default factory is found, skip setting the attribute by using continue
                continue
            setattr(self, param.name, default)


class BoundedCollection(ABC, SupportsDict, Generic[T]):
    """The BoundedCollection class is an abstract base class (ABC) that defines a collection with optional size constraints."""

    @abstractmethod
    def __len__(self) -> int:
        raise NotImplementedError

    @property
    @abstractmethod
    def bounded(self) -> bool:
        raise NotImplementedError

    @property
    @abstractmethod
    def maxlen(self) -> Optional[int]:
        raise NotImplementedError

    @property
    @abstractmethod
    def data(self) -> Iterable[T]:
        raise NotImplementedError

    @property
    def is_empty(self) -> bool:
        return len(self) == 0

    @property
    def is_full(self) -> bool:
        return self.bounded and len(self) == self.maxlen

    @abstractmethod
    def clear(self) -> None:
        raise NotImplementedError

    @abstractmethod
    def push(self, item: T) -> Optional[T]:
        raise NotImplementedError

    @abstractmethod
    def pop(self) -> T:
        raise NotImplementedError

    def to_dict(self) -> dict[str, Any]:
        return {'items': list(self.data), 'max_size': self.maxlen}


class Queue(BoundedCollection[T]):
    """
    We only need this custom abstraction on top of deque(FIFO - First In, First Out) to ensure that it adheres to a specific interface(BoundedCollection)
    and can be used interchangeably with other collections that implement the same interface.
    """

    def __init__(self, maxlen: Optional[int] = None) -> None:
        self.queue: deque[T] = deque(maxlen=maxlen)

    def __len__(self) -> int:
        return len(self.queue)

    @property
    def bounded(self) -> bool:
        return self.maxlen is not None

    @property
    def maxlen(self) -> Optional[int]:
        return self.queue.maxlen

    @property
    def data(self) -> Iterable[T]:
        return self.queue

    def clear(self) -> None:
        self.queue.clear()

    def push(self, item: T) -> Optional[T]:  # pylint: disable=useless-return
        self.queue.append(item)
        return None

    def pop(self) -> T:
        return self.queue.popleft()


class LIFOQueue(Queue[T]):

    def pop(self) -> T:
        return self.queue.pop()


class MinHeap(BoundedCollection[T]):
    """
    In the context of a simulation, the MinHeap class is used to manage and prioritize events or tasks based on their scheduled times or priorities. 
    By maintaining a heap structure, the MinHeap ensures that the next event to be processed is always the one with the earliest scheduled time or highest priority.
    """

    def __init__(self, maxlen: Optional[int] = None) -> None:
        self._maxlen = maxlen
        self.heap: list[T] = []
        heapq.heapify(self.heap)

    def __len__(self) -> int:
        return len(self.heap)

    @property
    def bounded(self) -> bool:
        return self.maxlen is not None

    @property
    def maxlen(self) -> Optional[int]:
        return self._maxlen

    @property
    def data(self) -> Iterable[T]:
        return self.heap

    @property
    def min(self) -> Optional[T]:
        return None if self.is_empty else self.heap[0]

    def clear(self) -> None:
        self.heap.clear()

    def push(self, item: T) -> Optional[T]:
        if self.is_full:
            return heapq.heapreplace(self.heap, item)
        heapq.heappush(self.heap, item)
        return None

    def pop(self) -> T:
        return heapq.heappop(self.heap)


"""
type used in the priority queeue:
A tuple with a priority value and an item: (priority, item)
A tuple with a priority value, a counter (for FIFO or LIFO behavior), and an item: (priority, counter, item)
"""
PriorityTuple = Union[tuple[SupportsFloat, T], tuple[SupportsFloat, int, T]]


class PriorityQueue(MinHeap[T]):
    """
    The heap orders elements based on the first element of the tuple (the priority). If the priorities are equal, the second element (the counter) is used to maintain insertion order.

    Without FIFO:
    - The heap orders elements based on the priority.
    - If two items have the same priority, their order in the heap is undefined.
    
    With FIFO:
    - The heap orders elements based on the priority.
    - If two items have the same priority, the counter ensures that the item added first is processed first (FIFO).
    """

    def __init__(self, priority_fn: Callable[[T], SupportsFloat], fifo: Optional[bool] = None, **kwargs: Any) -> None:
        super().__init__(**kwargs)
        self.fifo = fifo
        self.priority_fn = priority_fn

        if self.fifo is not None:
            # every time we push to the heap, we will increment the counter
            self.counter = itertools.count()

    @property
    def data(self) -> Iterable[T]:
        """
        This property returns an iterable of the original items in the priority queue, ignoring the priority and counter values.
        The [-1] index accesses the last element of the tuple, which is the original item.
        """
        return (cast(PriorityTuple[T], item)[-1] for item in self.heap)

    def push(self, item: T) -> Optional[T]:
        priority = self.priority_fn(item)
        if self.fifo is None:
            element: PriorityTuple[T] = (priority, item)
        else:
            count = next(self.counter)
            element = (priority, count if self.fifo else -count, item)
        # The cast function is used to signal the type checker that element is of type T
        return super().push(cast(T, element))

    def pop(self) -> T:
        return cast(PriorityTuple[T], super().pop())[-1]
