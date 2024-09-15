import random
from functools import partial
import pandas as pd
import re
import os

from create import CreateElement
from process import ProcessElement
from model import Model

# BM 1: basic simulation
# BM 3: multiple elements in the model


def run_simulation() -> None:
    ''' partial(random.expovariate, lambd=1.0 / 0.2) creates a new function 
    where random.expovariate is called with lambd fixed to some value

    Lower lambda (small λ) means that events happen less frequently, so the delays between events are longer.
    Higher lambda (large λ) means that events happen more frequently, so the delays between events are shorter.
    '''
    create1 = CreateElement(get_delay=partial(
        random.expovariate, lambd=1.0 / 0.2))
    process1 = ProcessElement(max_queue_size=10, num_handlers=5, get_delay=partial(
        random.expovariate, lambd=1.0 / 1.2))
    process2 = ProcessElement(max_queue_size=8, num_handlers=7, get_delay=partial(
        random.expovariate, lambd=1.0 / 2.0))
    process3 = ProcessElement(max_queue_size=1, num_handlers=2, get_delay=partial(
        random.expovariate, lambd=1.0 / 1.0))

    create1.add_next_element(process1)
    process1.add_next_element(process2)
    process2.add_next_element(process3)
    ''' This line creates a loop where 10% of the time, after an event is processed by process3, it goes back to process1,
    thus implementing the branching requirement from the lab task'''
    process3.add_next_element(process1, proba=0.1)

    model = Model(parent=create1)
    stats = model.simulate(end_time=1000, verbose=False)

    print('Final statistics:')
    for name, element_stats in stats.items():
        print(f'{name}:\n{element_stats}\n')


def verify_model(num_runs=10):
    lambda_values = [0.5, 1.0, 1.5, 2.0]  
    num_handlers_list = [1, 2, 3]
    queue_sizes = [3, 5, 7, 10]

    for _ in range(num_runs):
        create1 = CreateElement(get_delay=partial(
            random.expovariate, random.choice(lambda_values)))
        process1 = ProcessElement(max_queue_size=random.choice(queue_sizes), num_handlers=random.choice(num_handlers_list), get_delay=partial(
            random.expovariate, random.choice(lambda_values)))
        process2 = ProcessElement(max_queue_size=random.choice(queue_sizes), num_handlers=random.choice(num_handlers_list), get_delay=partial(
            random.expovariate, random.choice(lambda_values)))
        process3 = ProcessElement(max_queue_size=random.choice(queue_sizes), num_handlers=random.choice(num_handlers_list), get_delay=partial(
            random.expovariate, random.choice(lambda_values)))

        create1.add_next_element(process1)
        process1.add_next_element(process2)
        process2.add_next_element(process3)
        ''' This line creates a loop where 10% of the time, after an event is processed by process3, it goes back to process1,
        thus implementing the branching requirement from the lab task'''
        process3.add_next_element(process1, proba=0.1)

        model = Model(parent=create1)
        model.simulate(end_time=1000, verbose=False)


        # saving stats for late analysis
        element_types = (re.sub(r'\d+', '', element.name) for element in model.elements)
        element_data = {element_type: [] for element_type in element_types}
        for element in model.elements:
            element_data[re.sub(r'\d+', '', element.name)].append({key.strip(): float(value.strip()) for pair in element.stats.__repr__().split('. ') if ': ' in pair for key, value in [pair.split(': ')]})
        
        for element_name, stats_list in element_data.items():
            df = pd.DataFrame(stats_list)
            
            if not os.path.exists('data'):
                os.makedirs('data')

            filepath = os.path.join('data', f"{element_name}.csv")
            
            df.to_csv(filepath, mode='a', header=not pd.io.common.file_exists(filepath), index=False)
            
            print(f"Data for {element_name} appended to {filepath}")


if __name__ == '__main__':
    # verify_model()
    run_simulation()
