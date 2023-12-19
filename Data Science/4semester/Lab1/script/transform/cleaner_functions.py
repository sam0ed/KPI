from datetime import datetime


def remove_abbreviation(string):
    if 'K' in string or 'k' in string:
        return int(float(string[:-1]) * 1000)
    elif 'M' in string or 'm' in string:
        return int(float(string[:-1]) * 1000000)
    elif 'B' in string or 'b' in string:
        return int(float(string[:-1]) * 1000000000)
    else:
        try:
            return int(string)
        except:
            return None
def remove_commas(val):
    if isinstance(val, str):
        cleaned= val.replace(',', '')
        if cleaned.isnumeric():
            return cleaned
        else:
            return val
    else:
        return val

def transform_date_to_iso(val):
    return datetime.strptime(val, '%b %d, %Y').strftime('%Y-%m-%d')



   