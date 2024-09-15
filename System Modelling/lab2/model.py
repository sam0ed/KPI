from element import Element, Stats
from common import INF_TIME
from process import TIME_EPS


class Model:

    def __init__(self, parent: Element) -> None:
        self._extract_elements(parent)

    # TODO: add return statement to move self.elements = list(elements) into init
    def _extract_elements(self, parent: Element) -> list[Element]:
        # set used to store all the elements that are part of the model
        elements: set[Element] = set()

        # Recursive function to extract all elements that are part of the model
        def process_element(parent: Element) -> None:
            elements.add(parent)
            for element in parent.next_elements:
                if element not in elements:
                    process_element(element)

        process_element(parent)
        self.elements = list(elements)

    ''' Each element in the model keeps track only of the events which pertain to it. For example the Process element only 
    keeps track of the events like incoming requests, and requests being completed. 
    It doesn't keep track of the events like generation of requests, or requests being sent to the next element. 
    
    The Model class is responsible for coordinating the simulation of all the elements in the model.
    It finds the next event that will occur in the model and updates the time of all the elements in the model to that time.
    
    For the next closest event we run the out method of the event. We only run in method of the event inside other event's out method.'''
    def simulate(self, end_time: float, verbose: bool = False) -> dict[str, Stats]:
        current_time = 0

        while current_time < end_time:
            # find the closest next event among elements of the model
            next_time = INF_TIME
            for element in self.elements:
                if element.next_time < next_time:
                    next_time = element.next_time
            current_time = next_time

            for element in self.elements:
                element.set_current_time(current_time)

            ''' Invoking the end_action of all the processes which have next_action at this moment of model timeline. 
            next_time is the current time at which the model is at.
            element.next_time is the time at which the next event of the element will occur.'''
            updated_names: list[str] = []
            for element in self.elements:
                if abs(next_time - element.next_time) < TIME_EPS:
                    element.end_action()
                    updated_names.append(element.name)

            if verbose:
                self._print_states(current_time, updated_names)

        return {element.name: element.stats for element in self.elements}

    def _print_states(self, current_time: float, updated_names: list[str]) -> None:
        states_repr = ' | '.join([str(element) for element in self.elements])
        print(f'{current_time:.5f}: [Updated: {updated_names}]. States: {states_repr}\n')
