from pathlib import Path
from warnings import simplefilter

import matplotlib.pyplot as plt
import numpy as np
import pandas as pd
from statsmodels.tsa.deterministic import DeterministicProcess
import BaseFunctions as BF


def main_trend_development(csv_file_name: str):
    simplefilter("ignore")  # ignore warnings to clean up output cells

    # Set Matplotlib defaults
    plt.style.use("seaborn-whitegrid")
    plt.rc("figure", autolayout=True, figsize=(11, 5))
    plt.rc(
        "axes",
        labelweight="bold",
        labelsize="large",
        titleweight="bold",
        titlesize=14,
        titlepad=10,
    )
    plot_params = dict(
        color="0.75",
        style=".-",
        markeredgecolor="0.25",
        markerfacecolor="0.25",
        legend=False,
    )

    covid_data_set = BF.read_csv(csv_file_name)

    covid_data = covid_data_set.set_index("date").to_period()

    moving_average = covid_data.rolling(
        window=625,       # 365-day window
        center=True,      # puts the average at the center of the window
        min_periods=312,  # choose about half the window size
    ).median()              # compute the mean (could also do median, std, min, max, ...)

    ax = covid_data.plot(style=".", color="0.5")
    moving_average.plot(
        ax=ax, linewidth=3, title="Tunnel Traffic - 365-Day Moving Average", legend=False,
    )
    plt.show()


    dp = DeterministicProcess(
        index=covid_data.index,  # dates from the training data
        constant=True,       # dummy feature for the bias (y_intercept)
        order=1,             # the time dummy (trend)
        drop=True,           # drop terms if necessary to avoid collinearity
    )
    # `in_sample` creates features for the dates given in the `index` argument
    X = dp.in_sample()

    X.head()

