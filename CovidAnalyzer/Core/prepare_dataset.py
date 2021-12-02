from matplotlib import pyplot as plt
from pandas import DataFrame
from sklearn.impute import SimpleImputer
import core_action as ca
import pandas as pd
from sklearn.preprocessing import LabelEncoder


# Can be replaced in future by Label Encoding for categorical variable
def prepare_dataset_core(name_csv: str):
    df = ca.read_csv('../DataSource/' + name_csv)

    # ca.replace_val_in_cols_except(df, 'diagnostic_superclass', "['NORM']", False)
    # ca.replace_val_in_cols(df, ['diagnostic_superclass'], "['NORM']", True)

    sid = simple_imputing_data(df, df)

    df = apply_encoder(sid[0])

    return df


# Replacing missing values (imputing) according to certain strategy
def simple_imputing_data(X_train, X_valid):
    simple_imputer = SimpleImputer(strategy='most_frequent')
    imputed_X_train = pd.DataFrame(simple_imputer.fit_transform(X_train))
    imputed_X_valid = pd.DataFrame(simple_imputer.transform(X_valid))
    # Imputation removed column names; put them back
    imputed_X_train.columns = X_train.columns
    imputed_X_valid.columns = X_valid.columns

    return imputed_X_train, imputed_X_valid


# Encoding categorical variables
def detect_vals(obj_to_describe, val, exclude_col=[]):
    # missing values by columns
    obj_to_describe = obj_to_describe.drop(exclude_col, axis=1)
    missing_val_count_by_column = (obj_to_describe.isin([val]).sum())
    return (missing_val_count_by_column)


def apply_encoder(X: DataFrame):
    s = (X.dtypes == 'object')
    object_cols = list(s[s].index)

    for col in object_cols:
        X[col] = LabelEncoder().fit_transform(X[col])

    return X
