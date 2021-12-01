import pandas as pd
from pandas import DataFrame
from sklearn.feature_selection import mutual_info_regression
import matplotlib.pyplot as plt
import numpy as nmp
import seaborn as sns

import BaseFunctions as BF


# Main pipeline for INTERPOLATED dataset analysis without any changes
def main_handle_dataset(csv_file_name: str):
    covid_data = BF.read_csv(csv_file_name)
    describe_data(csv_file_name)


def build_heatmap(df: DataFrame):
    plt.figure(figsize=(14, 7))

    # Add title
    plt.title("Average Arrival Delay for Each Airline, by Month")

    # Heatmap showing average arrival delay for each airline by month
    sns.heatmap(data=df, annot=True)

    # Add label for horizontal axis
    plt.xlabel("Airline")


# print a summary of the data
def describe_data(csv_file_name: str):
    file_name = "Description.csv"
    pd.set_option('display.max_columns', 25)
    obj_to_describe = BF.read_csv(csv_file_name)
    BF.write_csv(obj_to_describe.describe(), file_name)


# print a summary of the data
def describe_data(csv_file_name: str):
    file_name = "Description.csv"
    pd.set_option('display.max_columns', 25)
    obj_to_describe = BF.read_csv(csv_file_name)
    BF.write_csv(obj_to_describe.describe(), file_name)


def make_mi_scores(X, y, discrete_features):
    mi_scores = mutual_info_regression(X, y, discrete_features=discrete_features)
    mi_scores = pd.Series(mi_scores, name="MI Scores", index=X.columns)
    mi_scores = mi_scores.sort_values(ascending=False)
    return mi_scores


def plot_mi_scores(scores):
    scores = scores.sort_values(ascending=True)
    width = nmp.arange(len(scores))
    ticks = list(scores.index)
    plt.barh(width, scores)
    plt.yticks(width, ticks)
    plt.title("Mutual Information Scores")


def describe_data(csv_file_name: str):
    covid_data = BF.read_csv(csv_file_name)
    plt.style.use("seaborn-whitegrid")
    covid_data.head()

    X = covid_data.copy()
    y = X.pop("new_cases")

    # Label encoding for categorical
    for colname in X.select_dtypes("object"):
        X[colname], _ = X[colname].factorize()

    # All discrete features should now have integer dtypes (double-check this before using MI!)
    discrete_features = X.dtypes == int

    mi_scores = make_mi_scores(X, y, discrete_features)
    mi_scores[::3]  # show a few features with their MI scores

    plt.figure(dpi=100, figsize=(8, 5))
    plot_mi_scores(mi_scores)

    sns.relplot(x="date", y="new_cases", data=covid_data)
    plt.show()
