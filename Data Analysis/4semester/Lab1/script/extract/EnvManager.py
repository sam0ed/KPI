import configparser
import os
import sys
from ConfigProgram import ConfigProgram

class EnvManager:
    path_to_api_keys: str
    key_owner: str
    key_name: str
    path_to_extracted:str

    def __init__(self, 
                 path_to_api_keys: str,
                 key_owner: str,
                 key_name: str,
                 path_to_extracted:str):
        self.path_to_api_keys=path_to_api_keys
        self.key_owner=key_owner
        self.key_name=key_name
        self.path_to_extracted=path_to_extracted
    
    def get_api_key(self):
        try:
            config = configparser.ConfigParser()
            config.read(self.path_to_api_keys) 
            return config[self.key_owner][self.key_name]
        except (configparser.Error, KeyError) as e:
            print(f"Error while getting API key: {e}")
            sys.exit(1)

    
    def set_up_os(self):
        if not os.path.exists(self.path_to_extracted):
            os.makedirs(self.path_to_extracted)

    # def save_dataframe(self,df):
    #     file_name = None
    #     for name, value in locals().items():
    #         if id(value) == id(df):
    #             file_name = name
    #             break
    #     df.to_csv(f'{self.path_to_extracted}/{file_name}.csv', index=False)
