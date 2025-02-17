from typing import Iterable, Optional, TypeVar

T = TypeVar('T')


def filter_none(values: Iterable[Optional[T]]) -> Iterable[T]:
    """This function filters out None values from an iterable"""
    return (value for value in values if value is not None)
