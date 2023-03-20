import configparser
import os
import sys

class EnvManager:
    @staticmethod
    def get_api_key( path_to_api_keys:str, key_owner:str, key_name:str):
        try:
            config = configparser.ConfigParser()
            config.read(path_to_api_keys) 
            return config[key_owner][key_name]
        except (configparser.Error, KeyError) as e:
            print(f"Error while getting API key: {e}")
            sys.exit(1)

    @staticmethod
    def set_up(path):
        if not os.path.exists(path):
            os.makedirs(path)

