import shutil
import pandas as pd


class InputSource:
    __slots__=('source_path','source', 'dest_path', 'dest', 'functions_for_transform')
    def __init__(self, source_path:str, source:str, dest_path:str ,dest:str ):
        self.source_path=source_path
        self.source=source
        self.dest_path=dest_path
        self.dest=dest
        self.functions_for_transform=set()

    def create_destination(self):
        shutil.copy(self.source_path+'\\'+self.source, self.dest_path+'\\'+self.dest )

    def transform_destination(self):
        dest_path=self.get_full_dest_path()
        df = pd.read_csv(dest_path)
        for func in self.functions_for_transform:
            res=func(df)
            df = res if res is not None else df
        df.to_csv(dest_path, index=False)

    def add_transforming_function(self, func:set):
        self.functions_for_transform.update(func)

    def get_full_dest_path(self):
        return self.dest_path+'\\'+self.dest
    
    def get_full_source_path(self):
        return self.source_path+'\\'+self.source

        