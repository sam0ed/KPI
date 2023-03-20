import pandas as pd

def remove_abbreviation(string):
    if 'K' in string or 'k' in string:
        return int(float(string[:-1]) * 1000)
    elif 'M' in string or 'm' in string:
        return int(float(string[:-1]) * 1000000)
    elif 'B' in string or 'b' in string:
        return int(float(string[:-1]) * 1000000000)
    else:
        return int(string)
def remove_commas(val):
    if isinstance(val, str):
        cleaned= val.replace(',', '')
        if cleaned.isnumeric():
            return cleaned
        else:
            return val
    else:
        return val



# def remove_commas_from_numeric_csv(file_path: str) -> None:
#     # Read the CSV file into a DataFrame
#     df = pd.read_csv(file_path, delimiter=',', quotechar='"')
#
#     # Define a function to remove commas from numeric fields
#
#     # Apply the function to each cell in the DataFrame
#     df = df.applymap(remove_commas)
#
#     # Write the modified DataFrame back to the same file
#     df.to_csv(file_path, index=False, encoding='utf-8')
#
# def remove_empty_rows_csv(file_path: str) -> None:
#     # Read the CSV file into a DataFrame
#     df = pd.read_csv(file_path)
#
#     # Remove rows with empty fields
#     df = df.dropna()
#
#     # Write the modified DataFrame back to the same CSV file
#     df.to_csv(file_path, index=False)
#
# def remove_column_csv(file_path:str, column_to_drop:str)->None:
#     # Read the CSV file into a DataFrame
#     df = pd.read_csv(file_path)
#
#     # Drop the specified column from the DataFrame
#     df.drop(columns=[column_to_drop], inplace=True)
#
#     # Rewrite the modified DataFrame to the original CSV file
#     df.to_csv(file_path, index=False)
#
# def remove_abbreviation_num_csv(file_path:str, columnns:list[str]):
#
#     apply_to={col: remove_abbreviation for col in columnns }
#     df = pd.read_csv(file_path, converters=apply_to)
#     df.to_csv(file_path, index=False)


   