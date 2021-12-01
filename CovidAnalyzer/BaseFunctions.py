import pandas as pd
from pandas import DataFrame


def read_csv(file_name: str):
    file_path = '../CsvStorage/'
    return pd.read_csv(file_path + file_name, parse_dates=["date"], encoding='utf-8')


def write_csv(pd_set: DataFrame, filename: str):
    file_path = '../CsvStorage/'
    return pd_set.to_csv("{0}{1}".format(file_path, filename))


def drop_col_df(pd_set: DataFrame, col_name: str):
    return pd_set.drop([col_name],  axis='columns')
