import random
from dataclasses import dataclass, field
from typing import Callable, Optional

from common import TIME_FORMATTER, format_params, instance_counter

GetDelayFn = Callable[[], float]


# This class is only responsible for counting the amount of events that pass through an element
# More sophisticated stats could be found in dedicated classes
@dataclass(eq=False)
class Stats:
    element: 'Element'

    num_events: int = field(init=False, default=0)

    def __repr__(self) -> str:
        return f'Num events: {self.num_events}'

# does branching while children do sceduling
@instance_counter
class Element:

    def __init__(self, get_delay: GetDelayFn, name: Optional[str] = None) -> None:
        self.name = self._get_name() if name is None else name
        self.get_delay = get_delay
        self.current_time = 0
        self.next_time = 0
        self.next_elements: list[Element] = []
        self.next_probas: list[float] = []
        self.stats = Stats(self)

    # self._next_id() is a class method that increments the count of the class,
    # it runs on initialization of the new element(being called from __init__ -> _get_name -> _next_id)
    def _get_name(self) -> str:
        return f'{self.__class__.__name__}{self._next_id()}'  # pylint: disable=no-member

    def _get_str_state(self) -> str:
        return format_params(self, ['stats.num_events', ('next_time', TIME_FORMATTER)])

    def __repr__(self) -> str:
        return f'{self.name} ({self._get_str_state()})'

    def start_action(self) -> None:
        pass

    # end action implements the call of the next element in the chain
    def end_action(self) -> None:
        self.stats.num_events += 1
        next_element = self._get_next_element()
        if next_element is not None:
            next_element.start_action()

    def set_current_time(self, next_time: float) -> None:
        self.current_time = next_time

    # BM 6: probabilistic branching of the elements
    def add_next_element(self, element: 'Element', proba: float = 1.0) -> None:
        self.next_probas.append(proba)
        self.next_elements.append(element)

    # We can not switch the code below to the commented code because if we do -- we will never leave the loop:
    # the else part of the code below is responsible for setting the probability of the None element to 1
    # if there are no next elements registered. This is necessary to break the loop.
    '''
    def _get_next_element(self) -> Optional['Element']:
        proba_sum = sum(self.next_probas)
        probas = [p / proba_sum for p in self.next_probas]  # Normalize probabilities
        elements = self.next_elements
        # The k parameter specifies the number of elements to choose from the elements sequence
        return random.choices(elements, probas, k=1)[0]

    '''

    def _get_next_element(self) -> Optional['Element']:
        proba_sum = sum(self.next_probas)
        if proba_sum > 1:
            raise RuntimeError(
                f'Next elements\' probas must sum to 1 instead of {proba_sum}')
        if proba_sum == 1:
            probas, elements = self.next_probas, self.next_elements
        else:
            probas = self.next_probas + [1 - proba_sum]
            elements = self.next_elements + [None]
        return random.choices(elements, probas, k=1)[0]

    def _predict_next_time(self) -> float:
        return self.current_time + self.get_delay()
