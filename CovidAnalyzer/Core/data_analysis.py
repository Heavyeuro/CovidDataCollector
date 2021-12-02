from sklearn.linear_model import LinearRegression

import numpy as np
import pandas as pd
from pandas import DataFrame
import core_action as ca


# main pipeline for plots and data discovering
from statsmodels.tsa.deterministic import CalendarFourier, DeterministicProcess


def describe_dataframe_core(df: DataFrame):
    # describe_dataset(df)
    seasoning_plots(df)


def describe_dataset(df: DataFrame):
    pd.set_option('display.max_columns', 30)
    print(df.describe())


def seasoning_plots(data_frame: DataFrame):
    cols_to_disp = ['new_cases', 'new_deaths_smoothed']
    for col in cols_to_disp:
        y = data_frame[[col, 'date']]
        ca.replace_val_in_cols(y, [col], np.nan, 0)
        y['date'] = y.date.dt.to_period('D')
        y.replace(np.nan, 0)
        y = y.set_index(['date'])

        fourier = CalendarFourier(freq='M', order=4)
        dp = DeterministicProcess(
                constant=True,
                index=y.index,
                order=1,
                seasonal=True,
                drop=True,
                additional_terms=[fourier],
            )

        X_time = dp.in_sample()
        X_time['NewYearsDay'] = (X_time.index.dayofyear == 1)

        model = LinearRegression(fit_intercept=False)
        model.fit(X_time, y)
        y_deseason = y - model.predict(X_time)
        y_deseason.name = 'sales_deseasoned'

        ax = y_deseason.plot()
