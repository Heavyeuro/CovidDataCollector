import numpy as np
import pandas as pd
from pandas import DataFrame
from sklearn.impute import SimpleImputer

import BaseFunctions as BF


# Main pipeline for dataset interpolation
def main_handle_dataset(csv_file_name: str):
    covid_data = BF.read_csv(csv_file_name)
    replace_val_in_col_with_consecutive_numbers(covid_data, ['date'])
    imputed = simple_imputing_data(covid_data, covid_data)[0]
    handled_df = null_to_nan(imputed, [])
    BF.write_csv(handled_df, 'handled.csv')


#   replace_val_in_col_with_consecutive_numbers(data_csv, ['date'])
def replace_val_in_col_with_consecutive_numbers(data_csv: [DataFrame, DataFrame], list_column):
    for col in list_column:
        for i in list(range(0, data_csv[col].size)):
            data_csv[col].iat[i] = i+1
    return data_csv


# Replacing missing values (imputing) according to certain strategy
def simple_imputing_data(x_train: DataFrame, x_valid: DataFrame):
    simple_impute = SimpleImputer(strategy='most_frequent')
    imputed_x_train = pd.DataFrame(simple_impute.fit_transform(x_train))
    imputed_x_valid = pd.DataFrame(simple_impute.transform(x_valid))
    # Imputation removed column names; put them back
    imputed_x_train.columns = x_train.columns
    imputed_x_valid.columns = x_valid.columns

    return imputed_x_train, imputed_x_valid


# replacing NaN with 0 in dataset
def null_to_nan(x, except_list):
    cols_with_missing_val = detect_null_val(x, except_list).index[0]
    x = replace_val_in_cols(x, [cols_with_missing_val], 0, np.nan)
    return x


# Counting an amount of null values in dataset by column
def detect_null_val(obj_to_describe: DataFrame, exclude_col=None):
    if exclude_col is None:
        exclude_col = []
    file_name = "NullValues.csv"
    obj_to_describe = obj_to_describe.drop(exclude_col, axis=1)
    missing_val_count_by_column = (obj_to_describe.isin([0]).sum())
    BF.write_csv(missing_val_count_by_column, file_name)
    return missing_val_count_by_column


# Replace each val in appropriate cols. Usage
#       replaceValInCols(data_csv, ['Diabetes'], 'Healthy', True)
#       replaceValInCols(data_csv, ['Diabetes'], 'Sick', False)
def replace_val_in_cols(data_csv: [DataFrame, DataFrame], list_column, old_value: str, new_value: str):
    for col in list_column:
        data_csv[col] = data_csv[col].replace(old_value, new_value)
    return data_csv
