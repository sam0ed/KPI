from dataclasses import dataclass
import os


@dataclass
class TransformConfig:
    path_to_source:str
    path_to_dest:str
    dest_prefix:str='transformed_'

    def set_up(self, path:str):
        if not os.path.exists(path):
            os.makedirs(path)
