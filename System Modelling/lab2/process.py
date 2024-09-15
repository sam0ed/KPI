import heapq
from collections import deque
from dataclasses import dataclass, field
from typing import Any

from element import Element, Stats
from common import INF_TIME, TIME_EPS, TIME_PR, TIME_FORMATTER, format_params

# BM 2: look through the ProcessElement to see where the stats are modified for the process element
@dataclass(eq=False)
class ProcessStats(Stats):
    element: 'ProcessElement'

    num_in_events: int = field(init=False, default=1)
    wait_time: int = field(init=False, default=0)
    busy_time: int = field(init=False, default=0)
    num_failures: int = field(init=False, default=0)

    def __repr__(self) -> str:
        return f'Num processed: {self.num_events}. Mean queue size: {self.mean_queue_size:.{TIME_PR}f}. Mean busy handlers: {self.mean_busy_handlers:.{TIME_PR}f}. Mean wait time: {self.mean_wait_time:.{TIME_PR}f}. Failure probability: {self.failure_proba:.{TIME_PR}f}'  # pylint: disable=line-too-long

    @property
    def mean_queue_size(self) -> float:
        return self.wait_time / max(self.element.current_time, TIME_EPS)

    @property
    def mean_busy_handlers(self) -> float:
        return self.busy_time / max(self.element.current_time, TIME_EPS)

    @property
    def failure_proba(self) -> float:
        return self.num_failures / max(self.num_in_events, 1)

    @property
    def mean_wait_time(self) -> float:
        return self.wait_time / max(self.num_events, 1)

# dataclass implements comparison operator by default
# in_event is excluded from being used for comparison, the order of fields is set as true(in decorator)
# that's why we don't need to implement __lt__ for comparison to be made by the value of next_time
@dataclass(order=True)
class Handler:
    in_event: int = field(compare=False)
    next_time: int

    def __repr__(self) -> str:
        return f'{self.__class__.__name__}({format_params(self, ["in_event", ("next_time", TIME_FORMATTER)])})'


class ProcessElement(Element):

    def __init__(self, max_queue_size: int, num_handlers: int = 1, **kwargs: Any) -> None:
        super().__init__(**kwargs)

        self.max_queue_size = max_queue_size
        # uses double ended queue to store incoming events
        self.queue: deque[int] = deque([])
        self.next_time = INF_TIME
        # stats class inside each process element to keep track of the statistics of the process element
        self.stats = ProcessStats(self)

        # BM 5: multiple handlers inside one process element
        #uses heap to store handlers
        self.num_handlers = num_handlers
        self.handlers: list[Handler] = []
        heapq.heapify(self.handlers)

    def _get_str_state(self) -> str:
        return format_params(self, [
            'stats.num_events', ('next_time', TIME_FORMATTER), 'num_handlers', 'handlers', 'queue', 'stats.num_failures'
        ])

    def start_action(self) -> None:
        if len(self.handlers) == self.num_handlers:
            if len(self.queue) < self.max_queue_size:
                self.queue.append(self.stats.num_in_events)
            else:
                self.stats.num_failures += 1
        else:
            handler = Handler(in_event=self.stats.num_in_events, next_time=self._predict_next_time())
            heapq.heappush(self.handlers, handler)
            self.next_time = self.handlers[0].next_time
        self.stats.num_in_events += 1

    def end_action(self) -> None:
        handler = heapq.heappop(self.handlers)

        if len(self.queue) > 0:
            # replacing the event handler is working on 
            # in order for the heap to work properly after update we need to pop the element(see beginning) and push it back
            handler.in_event = self.queue.popleft()
            handler.next_time = self._predict_next_time()
            heapq.heappush(self.handlers, handler)

        # if there are no events in the queue, the process is free
        self.next_time = self.handlers[0].next_time if self.handlers else INF_TIME
        super().end_action()

    def set_current_time(self, next_time: float) -> None:
        dtime = next_time - self.current_time
        self.stats.wait_time += len(self.queue) * dtime
        self.stats.busy_time += len(self.handlers) * dtime
        super().set_current_time(next_time)
