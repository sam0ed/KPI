
def print_json_attr_names(data):
    json_attribute_names:list[str]=get_json_attr_names(data)
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

# def get_json_attr_names(data, prefix='', visited=None):
#     if visited is None:
#         visited = set()
#     json_attribute_names = []
#     if isinstance(data, dict):
#         for key, value in data.items():
#             if key not in visited:
#                 visited.add(key)
#                 json_attribute_names.extend(get_json_attr_names(value, prefix + key + '.', visited))
#     elif isinstance(data, list):
#         if len(data) > 0:
#             item = data[0]
#             json_attribute_names.extend(get_json_attr_names(item, prefix, visited))
#     else:
#         json_attribute_names.append(prefix[:-1])
#     return json_attribute_names