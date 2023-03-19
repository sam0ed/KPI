import shutil


class InputSource:
    __slots__=('source_path','source', 'dest_path', 'dest', 'functions_for_clearing')
    def __init__(self, source_path:str, source:str, dest_path:str ,dest:str ):
        self.source_path=source_path
        self.source=source
        self.dest_path=dest_path
        self.dest=dest
        self.functions_for_clearing=set()

    def create_destination(self):
        shutil.copy(self.source_path+'\\'+self.source, self.dest_path+'\\'+self.dest )

    def clear_destination(self):
        for func in self.functions_for_clearing:
            func()

    def add_clearing_function(self, func:set):
        self.functions_for_clearing.update(func)

    def get_full_dest_path(self):
        return self.dest_path+'\\'+self.dest
    
    def get_full_source_path(self):
        return self.source_path+'\\'+self.source

        