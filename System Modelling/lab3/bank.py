import random
from functools import partial

from src.bank import BankQueueingNode, BankQueueingMetrics, BankTransitionNode

from qnet.common import Item, Queue
from qnet.node import NodeMetrics
from qnet.queueing import Task, ChannelPool
from qnet.factory import FactoryNode
from qnet.logger import CLILogger
from qnet.model import Model, ModelMetrics, Nodes, Evaluation, Verbosity


def simulate() -> None:
    car_spawner = FactoryNode[NodeMetrics](name='1_car_spawner',
                                             metrics=NodeMetrics(),
                                             delay_fn=partial(random.expovariate, lambd=1.0 / 0.5))
    switch = BankTransitionNode[Item, NodeMetrics](name='2_switch', metrics=NodeMetrics())
    cashier1 = BankQueueingNode[Item](name='3_cashier1',
                                       min_queuelen_diff=2,
                                       queue=Queue(maxlen=3),
                                       metrics=BankQueueingMetrics(),
                                       channel_pool=ChannelPool[Item](max_channels=1),
                                       delay_fn=partial(random.expovariate, lambd=1.0 / 0.3))
    cashier2 = BankQueueingNode[Item](name='4_cashier2',
                                       min_queuelen_diff=2,
                                       queue=Queue(maxlen=3),
                                       metrics=BankQueueingMetrics(),
                                       channel_pool=ChannelPool[Item](max_channels=1),
                                       delay_fn=partial(random.expovariate, lambd=1.0 / 0.3))

    car_spawner.set_next_node(switch)
    switch.set_next_nodes(first=cashier1, second=cashier2)
    cashier1.set_neighbor(cashier2)

    # Initial conditions
    # Adding a car to each cashier
    cashier1.add_task(Task[Item](item=Item(id=car_spawner.next_id, created_time=0.0),
                                  next_time=random.normalvariate(mu=1.0, sigma=0.3)))
    cashier2.add_task(Task[Item](item=Item(id=car_spawner.next_id, created_time=0.0),
                                  next_time=random.normalvariate(mu=1.0, sigma=0.3)))
    
    #adding 2 cars to each queue
    for _ in range(2):
        cashier1.queue.push(Item(id=car_spawner.next_id, created_time=0.0))
        cashier2.queue.push(Item(id=car_spawner.next_id, created_time=0.0))
    car_spawner.next_time = 0.1

    def failure_prob(_: Model[Item, ModelMetrics]) -> float:
        metrics1 = cashier1.metrics
        metrics2 = cashier2.metrics
        return (metrics1.num_failures + metrics2.num_failures) / max(metrics1.num_in + metrics2.num_in, 1)

    def count_switch_cashier(_: Model[Item, ModelMetrics]) -> int:
        return cashier1.metrics.num_from_neighbor + cashier2.metrics.num_from_neighbor

    def mean_cars_in_bank(_: Model[Item, ModelMetrics]) -> float:
        # mean_channels_load is the sum of average number of cars in the channels of the cashier
        # mean_queuelen is the sum of average number of cars in the queue of the cashier
        metrics1 = cashier1.metrics
        metrics2 = cashier2.metrics
        return metrics1.mean_channels_load + metrics1.mean_queuelen + metrics2.mean_channels_load + metrics2.mean_queuelen

    model = Model(nodes=Nodes[Item].from_node_tree_root(car_spawner),
                  evaluations=[
                      Evaluation[float](name='failure_prob', evaluate=failure_prob),
                      Evaluation[float](name='mean_cars_in_bank', evaluate=mean_cars_in_bank),
                      Evaluation[int](name='count_switch_cashier', evaluate=count_switch_cashier),
                  ],
                  metrics=ModelMetrics[Item](),
                  logger=CLILogger[Item]())
    model.simulate(end_time=10000, verbosity=Verbosity.METRICS)


if __name__ == '__main__':
    simulate()
