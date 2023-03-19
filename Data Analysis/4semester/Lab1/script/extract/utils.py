from typing import List


def print_json_attr_names(data):
    json_attribute_names:List[str]=get_json_attr_names(data)
    for i, name in enumerate(json_attribute_names):
        print(f'{i} '+name)

def get_json_attr_names(data, prefix=''):
    json_attribute_names=[]
    if isinstance(data, dict):
        for key, value in data.items():
            json_attribute_names.extend(get_json_attr_names(value, prefix + key + '.'))
    elif isinstance(data, list):
        for item in data:
            json_attribute_names.extend(get_json_attr_names(item, prefix))
    else:
        json_attribute_names.append( prefix[:-1])
    return json_attribute_names